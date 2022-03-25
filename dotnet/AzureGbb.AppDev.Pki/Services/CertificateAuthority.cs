namespace AzureGBB.AppDev.Pki.Services;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using AzureGBB.AppDev.Pki.Models.RSA;

public abstract class CertificateAuthority
{
	protected RSA? _caPrivateKey;
	protected  abstract void CreateRootCaInKeyVault();
	protected abstract X509Certificate2 GetRootCertificate();
	protected X509SignatureGenerator RSASignatureGenerator(){
		return X509SignatureGenerator.CreateForRSA(_caPrivateKey, RSASignaturePadding.Pkcs1);
	}
	public abstract X509Certificate2 IssueLeafCertificate(string subjectName, RSAPublicKeyParameters publicKey);

	protected static Byte[] SimpleSerialNumberGenerator()
	{
		long today = DateTime.UtcNow.ToBinary();

		Byte[] nowBytes = BitConverter.GetBytes(today);

		return nowBytes;
	}
}
