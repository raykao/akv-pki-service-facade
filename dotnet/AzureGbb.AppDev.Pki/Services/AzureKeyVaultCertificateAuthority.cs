using Newtonsoft.Json;

using Microsoft.Extensions.Logging;

using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Identity;
using Azure.Core;

namespace AzureGBB.AppDev.Pki.Services;
using AzureGBB.AppDev.Pki.Models;
using System.Text.Json;
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
	public readonly Byte[] rootCertificate;

	
	public AzureKeyVaultCertificateAuthority(ILogger<AzureKeyVaultCertificateAuthority> logger, string vaultName, string keyName, string fqdn)
	{
		this._logger = logger;
		this.vaultName = vaultName;
		this._keyName = keyName;
		this._fqdn = fqdn;

		this._vaultUri = "https://" + this.vaultName + ".vault.azure.net/";
		
		this._keyClient = new KeyClient(
			vaultUri: new Uri(this._vaultUri), 
			credential: this._credential
		);

		this._certificateClient = new CertificateClient(
			vaultUri: new Uri(this._vaultUri), 
			credential: this._credential
		);

		try {
			this._logger.LogInformation("Attempting to get Key: " + keyName);
			this._keyMetadata = this._keyClient.GetKey(this._keyName);
		}
		catch(Azure.RequestFailedException error)
		{
			this._logger.LogError("Key Not Found.  Attempting to Generate A Key.");
			this._logger.LogInformation(error.ToString());
			
			GenerateRootCa();
			this._keyMetadata = this._keyClient.GetKey(this._keyName);
		}

		this.rootCertificate = GetRootCertificate();
		
		this._cryptographyClient = new CryptographyClient(
			keyId: this._keyMetadata.Id, 
			credential: this._credential
		);
		
	}

	protected override void GenerateRootCa()
	{
		this._logger.LogInformation("Synchronously Generating RootCA.");
		CreateRootCaInKeyVault();
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
		AccessToken token =  this._credential.GetToken(
			new Azure.Core.TokenRequestContext(
				new[] { "https://vault.azure.net/.default" },
				null
			)
		);

		String requestUri = this._vaultUri + "/certificates/" + this._keyName + "/create?api-version=7.0";
		String bearerToken = "Bearer " + token.Token;

		CertificatePolicy certificatePolicy = new CertificatePolicy(
			keyProperties: new KeyProperties(),
			x509CertificateProperties: new X509CertificateProperties("CN=" + this._fqdn),
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

		this._logger.LogDebug("content: " + JsonConvert.SerializeObject(policy));

		HttpResponseMessage response = this.httpClient.Send(request);

		StreamReader reader = new StreamReader(response.Content.ReadAsStream());
            
    this._logger.LogDebug(reader.ReadToEnd());
	}

	protected override Byte[] GetRootCertificate()
	{
		return this._certificateClient.GetCertificate(this._keyName).Value.Cer;
	}

	public override Byte[]? SignContentWithRootCa(Byte[] content)
	{
		return this._cryptographyClient.SignData("RS256", content).Signature;
	}
}