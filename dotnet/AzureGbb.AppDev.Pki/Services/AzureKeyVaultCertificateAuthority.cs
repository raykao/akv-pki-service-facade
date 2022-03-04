using Microsoft.Extensions.Logging;

using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Identity;

namespace AzureGBB.AppDev.Pki.Services;

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
	private readonly String? _rootCertificate;

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

		this._rootCertificate = GetRootCertificate();
		
		this._cryptographyClient = new CryptographyClient(
			keyId: this._key.Id, 
			credential: this._credential
		);
	}

	override protected void GenerateRootCa(){
		CreateRootCaInKeyVault();
	}

	override protected void CreateRootCaInKeyVault() {
		// Create Certificate Policy
		// Get Auth Token
		// Make HTTP call to vault certificate endpoint with policy as payload
	}

	override protected String GetRootCertificate() {
		return this._keyClient.GetKey(this._keyName).ToString();
	}

	override public Byte[]? SignContentWithRootCa(Byte[] content){
		return this._cryptographyClient.SignData("RS256", content).Signature;
	}
}