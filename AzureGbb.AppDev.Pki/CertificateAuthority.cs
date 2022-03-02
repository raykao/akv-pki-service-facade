namespace AzureGbb.AppDev.Pki.Services;

public abstract class CertificateAuthority
{
	public abstract String? GetRootCertificate();
	public abstract void GenerateRootCa();
	public abstract Task<byte[]> CreateRootCaInKeyVaultAsync();
	public abstract Byte[] CreateRootCaInLocal();
	public abstract void UploadRootCaToKeyVault(Byte[] certificate);
	public void CreateRootCaInLocalAndUploadToKeyVault()
	{
		Byte[] certificate = CreateRootCaInLocal();
		UploadRootCaToKeyVault(certificate);
	}
	public abstract Byte[]? SignContentWithRootCa(Byte[] content);
}
