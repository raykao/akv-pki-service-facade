using Newtonsoft.Json;

namespace AzureGBB.AppDev.Pki.Models.Certificates;
public partial class SecretProperties
{
	/// <summary>
	/// Initializes a new instance of the SecretProperties class.
	/// </summary>
	public SecretProperties()
	{
		CustomInit();
	}

	/// <summary>
	/// Initializes a new instance of the SecretProperties class.
	/// </summary>
	/// <param name="contentType">The media type (MIME type).</param>
	public SecretProperties(string? contentType = default(string))
	{
		ContentType = contentType;
		CustomInit();
	}

	/// <summary>
	/// An initialization method that performs custom operations like setting defaults
	/// </summary>
	partial void CustomInit();

	/// <summary>
	/// Gets or sets the media type (MIME type).
	/// </summary>
	[JsonProperty(PropertyName = "contentType")]
	public string? ContentType { get; set; }

}