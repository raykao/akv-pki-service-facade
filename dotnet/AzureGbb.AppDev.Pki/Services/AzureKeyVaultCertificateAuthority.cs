using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;

namespace AzureGBB.AppDev.Pki.Services;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.Logging;

using Azure.Core;

using System.Text.Json;

using AzureGBB.AppDev.Pki.Models.Certificates;
using AzureGBB.AppDev.Pki.Models.RSA;

public class AzureKeyVaultCertificateAuthority : CertificateAuthority
{
	private readonly HttpClient httpClient = new HttpClient();
	private readonly TokenCredential _credential;
	private readonly string vaultName;
	private readonly string _keyName;
	private readonly string _fqdn;
	private readonly String _vaultUri;
	private readonly KeyClient _keyClient;
	private readonly CertificateClient _certificateClient;
	private readonly KeyVaultKey _keyMetadata;
	private readonly ILogger<AzureKeyVaultCertificateAuthority> _logger;

	private readonly RSA _caPrivateKey;
	public readonly X509Certificate2 RootCertificate;
	
	public AzureKeyVaultCertificateAuthority(TokenCredential azureCredential, ILogger<AzureKeyVaultCertificateAuthority> logger, string azureKeyVaultName, string keyName, string fqdn)
	{
		_credential = azureCredential;
		_logger = logger;
		vaultName = azureKeyVaultName;
		_keyName = keyName;
		_fqdn = fqdn;

		_vaultUri = "https://" + vaultName + ".vault.azure.net/";
		
		_keyClient = new KeyClient(
			vaultUri: new Uri(_vaultUri), 
			credential: _credential
		);

		_certificateClient = new CertificateClient(
			vaultUri: new Uri(_vaultUri), 
			credential: _credential
		);

		_logger.LogInformation("Attempting to get Key: " + keyName);
		try
		{
			_keyMetadata = _keyClient.GetKey(_keyName);
			_logger.LogInformation("Key: '" + keyName + "' Found");
		}
		catch(Azure.RequestFailedException error)
		{
			_logger.LogError("Key Not Found.  Attempting to Generate a Root CA/Key in AKV.");
			_logger.LogInformation(error.ToString());
			
			CreateRootCaInKeyVault();
			_keyMetadata = _keyClient.GetKey(_keyName);
		}

		RootCertificate = GetRootCertificate();
	
		_caPrivateKey = RSAKeyVaultProvider.RSAFactory.Create(_credential, _keyMetadata.Id, RootCertificate);
	}

	// Version 4 of the KeyVault SDK does not allow the use of a full Custom Certificate Policy
	// We will instead create a certificate in KeyVault "manually" via the REST v7 API but using our
	// AuthToken we would normally use for the SDK (Azure Identity)
	// 
	// Potentially we could revert to v3 of the SDK and KeyVaultClient().CreateCertificateAsync() instead
	//  
	// - Get Auth Token
	// - Create Certificate Policy
	// - Make HTTP call to vault certificate endpoint with policy as payload
	protected override void CreateRootCaInKeyVault()
	{
		_logger.LogInformation("Synchronously Generating RootCA.");

		AccessToken token =  _credential.GetToken(
			new Azure.Core.TokenRequestContext(
				new[] { "https://vault.azure.net/.default" },
				null
			),
			new CancellationToken()
		);

		String requestUri = _vaultUri + "/certificates/" + _keyName + "/create?api-version=7.0";
		String bearerToken = "Bearer " + token.Token;

		Policy policy = new Policy(
			certificatePolicy: new CertificatePolicy(
				keyProperties: new KeyProperties(),
				x509CertificateProperties: new X509CertificateProperties("CN=" + _fqdn),
				issuerParameters: new IssuerParameters()
			)
		);

		StringContent content = new StringContent(JsonSerializer.Serialize(policy), Encoding.UTF8, "application/json");

		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post ,requestUri)
		{
			Content = content,
			Headers = { 
				{ HttpRequestHeader.Authorization.ToString(), bearerToken },
				{ HttpRequestHeader.Accept.ToString(), "application/json" }
			}
		};

		HttpResponseMessage response = httpClient.Send(request);

		if(response.IsSuccessStatusCode)
		{
			_logger.LogInformation("Successfully created RootCA Cert and Keys");
		}
		else
		{
			throw new Exception("RootCA Cert and Key could not be created.");
		}

		// StreamReader reader = new StreamReader(response.Content.ReadAsStream());
            
    // _logger.LogDebug(reader.ReadToEnd());
	}

	protected override X509Certificate2 GetRootCertificate()
	{
		Byte[] rootCertificate =_certificateClient.GetCertificate(_keyName).Value.Cer;
		return new X509Certificate2(rootCertificate);
	}

	protected override X509SignatureGenerator RSASignatureGenerator()
	{
		return X509SignatureGenerator.CreateForRSA(_caPrivateKey, RSASignaturePadding.Pkcs1);
	}

	public override String IssueLeafCertificate(string subjectName, RSAPublicKeyParameters publicKeyParams)
	{
		_logger.LogInformation("Creating a new Leaf/Client Certificate.");
		
		_logger.LogInformation("Regenerating Client RSA Public Key.");
		RSAPublicKey publicKey = new RSAPublicKey(publicKeyParams);

		_logger.LogInformation("Creating a Certificate Signing Request.");
		CertificateRequest csr = publicKey.CreateCertificateSigningRequest(subjectName, RootCertificate.Extensions[RSAPublicKey.SubjectIdExtensionOid]);

		_logger.LogInformation("Signing CSR with RootCA Key.");
		// Certificate expires in 30days - should add ability to modify accordingly - but keep it low and use Passive Certificate Revocation (no Signed CRL)
		X509Certificate2 signedCert = csr.Create(RootCertificate.SubjectName, RSASignatureGenerator(), DateTime.Today.AddDays(-1), DateTime.Today.AddDays(30), DefaultSerialNumberGenerator());

		byte[] rawCert = signedCert.RawData;
		char[] certificatePem = PemEncoding.Write("CERTIFICATE", rawCert);

		_logger.LogInformation("Certificate Signing Complete.  Certificate Issued.");
		return new string(certificatePem);
	}
}