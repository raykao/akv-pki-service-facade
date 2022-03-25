namespace AzureGBB.AppDev.Pki.Models.RSA;

public class RSAPublicKeyParameters
{
	public byte[] Exponent { get; }
	public byte[] Modulus { get; }

	public RSAPublicKeyParameters(byte[] exponent, byte[] modulus){
		Exponent = exponent;
		Modulus = modulus;
	}
}