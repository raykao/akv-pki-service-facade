namespace AzureGBB.AppDev.Pki.Services;

public abstract class CertificateAuthority
{
	protected  abstract void CreateRootCaInKeyVault();
	protected abstract Byte[] GetRootCertificate();
	public abstract Byte[]? SignContentWithRootCa(Byte[] content);
}
