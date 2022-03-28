namespace AzureGBB.AppDev.Pki.Services;
using System.Security.Cryptography.X509Certificates;
using AzureGBB.AppDev.Pki.Models.RSA;

public abstract class CertificateAuthority
{
	protected  abstract void CreateRootCaInKeyVault();
	protected abstract X509Certificate2 GetRootCertificate();
	protected abstract X509SignatureGenerator RSASignatureGenerator();
	public abstract String IssueLeafCertificate(string subjectName, RSAPublicKeyParameters publicKey);

	protected static Byte[] DefaultSerialNumberGenerator()
	{
		long today = DateTime.UtcNow.ToBinary();

		Byte[] nowBytes = BitConverter.GetBytes(today);

		return nowBytes;
	}
}
