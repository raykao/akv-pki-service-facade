using Newtonsoft.Json;

using Microsoft.Extensions.Logging;

using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Identity;
using Azure.Core;

namespace AzureGBB.AppDev.Pki.Services;
using AzureGBB.AppDev.Pki.Models;
using System.Text;
using System.Net;

public class AzureKeyVaultCertificateAuthority : CertificateAuthority
{
	private readonly HttpClient httpClient = new HttpClient();
	private readonly DefaultAzureCredential _credential = new DefaultAzureCredential();

	private readonly string vaultName;
	private readonly string _keyName;
	private readonly string _fqdn;
	private readonly String _vaultUri;
	private readonly KeyClient _keyClient;
	private readonly CertificateClient _certificateClient;
	private readonly CryptographyClient _cryptographyClient;
	private readonly KeyVaultKey _keyMetadata;
	private readonly ILogger<AzureKeyVaultCertificateAuthority> _logger;
	public readonly Byte[] RootCertificate;

	
	public AzureKeyVaultCertificateAuthority(ILogger<AzureKeyVaultCertificateAuthority> logger, string azureKeyVaultName, string keyName, string fqdn)
	{
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

		try {
			_logger.LogInformation("Attempting to get Key: " + keyName);
			_keyMetadata = _keyClient.GetKey(_keyName);
		}
		catch(Azure.RequestFailedException error)
		{
			_logger.LogError("Key Not Found.  Attempting to Create Root CA/Key in AKV.");
			_logger.LogInformation(error.ToString());
			
			CreateRootCaInKeyVault();
			_keyMetadata = _keyClient.GetKey(_keyName);
		}

		RootCertificate = GetRootCertificate();
		
		_cryptographyClient = new CryptographyClient(
			keyId: _keyMetadata.Id, 
			credential: _credential
		);
		
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
			)
		);

		String requestUri = _vaultUri + "/certificates/" + _keyName + "/create?api-version=7.0";
		String bearerToken = "Bearer " + token.Token;

		CertificatePolicy certificatePolicy = new CertificatePolicy(
			keyProperties: new KeyProperties(),
			x509CertificateProperties: new X509CertificateProperties("CN=" + _fqdn),
			issuerParameters: new IssuerParameters()
		);

		Policy policy = new Policy(
			certificatePolicy: certificatePolicy
		);

		StringContent content = new StringContent(JsonConvert.SerializeObject(policy), Encoding.UTF8, "application/json");

		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post ,requestUri)
		{
			Content = content,
			Headers = { 
				{ HttpRequestHeader.Authorization.ToString(), bearerToken },
				{ HttpRequestHeader.Accept.ToString(), "application/json" }
			}
		};

		_logger.LogDebug("content: " + JsonConvert.SerializeObject(policy));

		HttpResponseMessage response = httpClient.Send(request);

		StreamReader reader = new StreamReader(response.Content.ReadAsStream());
            
    _logger.LogDebug(reader.ReadToEnd());
	}

	protected override Byte[] GetRootCertificate()
	{
		return _certificateClient.GetCertificate(_keyName).Value.Cer;
	}

	public override Byte[]? SignContentWithRootCa(Byte[] content)
	{
		return _cryptographyClient.SignData("RS256", content).Signature;
	}
}