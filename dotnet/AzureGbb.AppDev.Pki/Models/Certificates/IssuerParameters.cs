using System.Text.Json.Serialization;

namespace AzureGBB.AppDev.Pki.Models.Certificates;
public partial class IssuerParameters
{
	/// <summary>
	/// Initializes a new instance of the IssuerParameters class.
	/// </summary>
	public IssuerParameters()
	{
		CustomInit();
	}

	/// <summary>
	/// Initializes a new instance of the IssuerParameters class.
	/// </summary>
	/// <param name="name">Name of the referenced issuer object or reserved
	/// names; for example, 'Self' or 'Unknown'.</param>
	/// <param name="certificateType">Type of certificate to be requested
	/// from the issuer provider.</param>
	/// <param name="certificateTransparency">Indicates if the certificates
	/// generated under this policy should be published to certificate
	/// transparency logs.</param>
	public IssuerParameters(
		bool certificateTransparency,
		string certificateType,
		string name
	)
	{
		CertificateType = certificateType;
		CertificateTransparency = certificateTransparency;
		Name = name;
		CustomInit();
	}

	/// <summary>
	/// An initialization method that performs custom operations like setting defaults
	/// </summary>
	partial void CustomInit();

	/// <summary>
	/// Gets or sets name of the referenced issuer object or reserved
	/// names; for example, 'Self' or 'Unknown'.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; } = "Self";

	/// <summary>
	/// Gets or sets type of certificate to be requested from the issuer
	/// provider.
	/// </summary>
	[JsonPropertyName("cty")]
	public string? CertificateType { get; set; }

	/// <summary>
	/// Gets or sets indicates if the certificates generated under this
	/// policy should be published to certificate transparency logs.
	/// </summary>
	[JsonPropertyName("cert_transparency")]
	public bool CertificateTransparency { get; set; } = false;

}