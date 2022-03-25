namespace AzureGBB.AppDev.Pki.Services;
using System.Security.Cryptography.X509Certificates;
using AzureGBB.AppDev.Pki.Models.RSA;

public abstract class CertificateAuthority
{
	protected  abstract void CreateRootCaInKeyVault();
	protected abstract X509Certificate2 GetRootCertificate();
	public abstract X509Certificate2 IssueLeafCertificate(string subjectName, RSAPublicKeyParameters publicKey);
	public abstract X509SignatureGenerator RSASignatureGenerator();
}
