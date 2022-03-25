namespace AzureGBB.AppDev.Pki.Models.RSA;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public class RSAPublicKey
{
	public RSA key { get; }

	public const string SubjectIdExtensionOid = "2.5.29.14";
	public const string AuthorityIdExtensionOid = "2.5.29.35";
	public const string ClientAuthEkuOid = "1.3.6.1.5.5.7.3.2";
	public const string ServerAuthEkuOid = "1.3.6.1.5.5.7.3.1";

	public RSAPublicKey(RSAPublicKeyParameters publicKeyParameters){
		RSAParameters parameters = new RSAParameters { Modulus = publicKeyParameters.Modulus, Exponent = publicKeyParameters.Exponent };
		key = RSA.Create(parameters);
	}

	// Reconstruct a public key using a previous Key's Modulus and Exponent properties (these are the important parts of a Public Key)
	public CertificateRequest CreateCertificateSigningRequest(string subjectName, X509Extension authorityKeyIdentifier)
	{
		X500DistinguishedName x509SubjectName= new X500DistinguishedName("CN=" + subjectName);
		
		CertificateRequest csr = new CertificateRequest(x509SubjectName, key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

		csr.CertificateExtensions.Add(
			new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true)
		);

		csr.CertificateExtensions.Add(
			new X509BasicConstraintsExtension(false, true, 0, true)
		);

		csr.CertificateExtensions.Add(
			new X509EnhancedKeyUsageExtension(
				new OidCollection {
					new Oid(ClientAuthEkuOid), 
					new Oid(ServerAuthEkuOid)
				},
				false
			)
		);

		csr.CertificateExtensions.Add(
			new X509SubjectKeyIdentifierExtension(csr.PublicKey, false)
		);

		csr.CertificateExtensions.Add(
			BuildAuthorityKeyIdentifierExtension(authorityKeyIdentifier)
		);

		return csr;
	}

	private static X509Extension BuildAuthorityKeyIdentifierExtension(X509Extension authorityKeyIdentifierExtension)
	{
		var authoritySubjectKey = authorityKeyIdentifierExtension.RawData;
		var segment = new Span<byte>(authoritySubjectKey, 2, authoritySubjectKey.Length - 2);
		var authorityKeyIdentifier = new byte[segment.Length + 4];
		// these bytes define the "KeyID" part of the AuthorityKeyIdentifier
		authorityKeyIdentifier[0] = 0x30;
		authorityKeyIdentifier[1] = 0x16;
		authorityKeyIdentifier[2] = 0x80;
		authorityKeyIdentifier[3] = 0x14;
		segment.CopyTo(new Span<byte>(authorityKeyIdentifier, 4, authorityKeyIdentifier.Length - 4));

		return new X509Extension(AuthorityIdExtensionOid, authorityKeyIdentifier, false);
	}
}