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
	// private readonly CryptographyClient _cryptographyClient;
	private readonly CertificateClient _certificateClient;
	private readonly KeyVaultKey _key;
	private readonly ILogger<AzureKeyVaultCertificateAuthority> _logger;

	public readonly String? rootCertificate;

	
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
			this._key = this._keyClient.GetKey(this._keyName);			
		}
		catch(Azure.RequestFailedException error)
		{
			this._logger.LogError("Key Not Found.  Attempting to Generate A Key.");
			this._logger.LogInformation(error.ToString());
			
			GenerateRootCa();
			// this._key = this._keyClient.GetKey(this._keyName);
		}

		this.rootCertificate = GetRootCertificate();
		
		// this._cryptographyClient = new CryptographyClient(
		// 	keyId: this._key.Id, 
		// 	credential: this._credential
		// );
		
	}

	protected override void GenerateRootCa(){
		this._logger.LogInformation("Generating RootCA.");
		CreateRootCaInKeyVaultAsync();
	}

	protected override void CreateRootCaInKeyVaultAsync() {
		// Version 4 of the KeyVault SDK does not allow the use of a full Custom Certificate Policy
		// We will instead create a certificate in KeyVault "manually" via the REST API but using our
		// AuthToken we would normally use for the SDK (Azure Identity)
		// 
		// Potentially we could revert to v3 of the SDK and KeyVaultClient().CreateCertificateAsync() instead
		//  
		// - Get Auth Token
		// - Create Certificate Policy
		// - Make HTTP call to vault certificate endpoint with policy as payload

		AccessToken token =  this._credential.GetToken(
			new Azure.Core.TokenRequestContext(
				new[] { "https://vault.azure.net/.default" },
				null
			)
		);

		String requestUri = this._vaultUri + "/certificates/" + this._keyName + "/create?api-version=7.0";

		KeyProperties keyProperties = new KeyProperties(false, "RSA", 2048, false);

		X509CertificateProperties x509CertificateProperty = new X509CertificateProperties(
			"CN=" + this._fqdn,
			keyUsage: new List<string> {"keyCertSign"},
 			ekus: new List<string> {"1.3.6.1.5.5.7.3.2", "1.3.6.1.5.5.7.3.1"},
			basicConstraints: new BasicConstraints(true, 1)
		);

		IssuerParameters issuerParameters = new IssuerParameters("Self");

		CertificatePolicy certificatePolicy = new CertificatePolicy(
			keyProperties: keyProperties,
			x509CertificateProperties: x509CertificateProperty,
			issuerParameters: issuerParameters
		);

		Policy policy = new Policy(
			certificatePolicy: certificatePolicy
		);
		
		StringContent content = new StringContent(JsonConvert.SerializeObject(policy), Encoding.UTF8, "application/json");

		this._logger.LogInformation("policy: " + policy);

		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post ,requestUri)
		{
			Content = content,
			Headers = { 
				{ HttpRequestHeader.Authorization.ToString(), "Bearer " + token.Token },
				{ HttpRequestHeader.Accept.ToString(), "application/json" }
			},
		};

		this._logger.LogInformation("content: " + JsonConvert.SerializeObject(policy));


		HttpResponseMessage response = this.httpClient.Send(request);

		StreamReader reader = new StreamReader(response.Content.ReadAsStream());
            
    this._logger.LogInformation(reader.ReadToEnd());
	}

	protected override String GetRootCertificate() {
		// return this._keyClient.GetKey(this._keyName).ToString();
		return "hello";
	}

	// public override Byte[]? SignContentWithRootCa(Byte[] content){
	// 	return this._cryptographyClient.SignData("RS256", content).Signature;
	// }
}