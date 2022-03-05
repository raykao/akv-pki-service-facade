using Microsoft.Extensions.Logging;

using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Identity;
using Azure.Core;

namespace AzureGBB.AppDev.Pki.Services;
using AzureGBB.AppDev.Pki.Models;
using System.Text.Json;

public class AzureKeyVaultCertificateAuthority : CertificateAuthority
{
	private readonly string vaultName;
	private readonly string _keyName;
	private readonly String _vaultUri;
	private readonly DefaultAzureCredential _credential = new DefaultAzureCredential();
	private readonly KeyClient _keyClient;
	private readonly CryptographyClient _cryptographyClient;
	private readonly CertificateClient _certificateClient;
	private readonly KeyVaultKey _key;
	public readonly String? rootCertificate;

	private readonly ILogger<AzureKeyVaultCertificateAuthority> _logger;

	public AzureKeyVaultCertificateAuthority(ILogger<AzureKeyVaultCertificateAuthority> logger, string vaultName, string keyName)
	{
		this._logger = logger;
		this.vaultName = vaultName;
		this._keyName = keyName;

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
			this._key = this._keyClient.GetKey(this._keyName);			
		}
		catch(Azure.RequestFailedException error)
		{
			this._logger.LogInformation(error.ToString());
			GenerateRootCa();
			this._key = this._keyClient.GetKey(this._keyName);
		}

		this.rootCertificate = GetRootCertificate();
		
		this._cryptographyClient = new CryptographyClient(
			keyId: this._key.Id, 
			credential: this._credential
		);
	}

	protected override void GenerateRootCa(){
		CreateRootCaInKeyVaultAsync();
	}

	protected override async void CreateRootCaInKeyVaultAsync() {
		// Version 4 of the KeyVault SDK does not allow the use of a full Custom Certificate Policy
		// We will instead create a certificate in KeyVault "manually" via the REST API but using our
		// AuthToken we would normally use for the SDK (Azure Identity)
		//  
		// - Get Auth Token
		// - Create Certificate Policy
		// - Make HTTP call to vault certificate endpoint with policy as payload

		AccessToken token = await this._credential.GetTokenAsync(
			new Azure.Core.TokenRequestContext(
				new[] { "https://vault.azure.net/.default" },
				null
			)
		);

		X509CertificateProperties x509CertificateProperty = new X509CertificateProperties(
			"CN=" + this._keyName,
			keyUsage: new List<string> {"keyCertSign"},
 			ekus: new List<string> {"1.3.6.1.5.5.7.3.2", "1.3.6.1.5.5.7.3.1"},
			basicConstraints: new BasicConstraints(true, 1)
		);

		IssuerParameters issuerParameters = new IssuerParameters("Self");

		CertificatePolicy certificate = new CertificatePolicy(
			keyProperties: new KeyProperties(false, "RSA", 2048, false),
			x509CertificateProperties: x509CertificateProperty,
			issuerParameters: issuerParameters
		);
		
		StringContent content = new StringContent(JsonSerializer.Serialize(certificate));

		HttpClient httpClient = new HttpClient();

		HttpResponseMessage response = await httpClient.PostAsync(
			requestUri: this._vaultUri + "/certificates/" + this._keyName,
			content: content
		);
	}

	protected override String GetRootCertificate() {
		return this._keyClient.GetKey(this._keyName).ToString();
	}

	public override Byte[]? SignContentWithRootCa(Byte[] content){
		return this._cryptographyClient.SignData("RS256", content).Signature;
	}
}