namespace AzureGBB.AppDev.Pki.Services;

public abstract class CertificateAuthority
{
	protected abstract String? GetRootCertificate();
	protected abstract void GenerateRootCa();
	protected  abstract void CreateRootCaInKeyVault();
	// public abstract Byte[]? SignContentWithRootCa(Byte[] content);
}
