// using System.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace AzureGBB.AppDev.Pki.Models.Certificates;

public partial class Attributes
{
	/// <summary>
	/// Initializes a new instance of the Attributes class.
	/// </summary>
	public Attributes()
	{
		CustomInit();
	}

	/// <summary>
	/// Initializes a new instance of the Attributes class.
	/// </summary>
	/// <param name="enabled">Determines whether the object is
	/// enabled.</param>
	/// <param name="notBefore">Not before date in UTC.</param>
	/// <param name="expires">Expiry date in UTC.</param>
	/// <param name="created">Creation time in UTC.</param>
	/// <param name="updated">Last updated time in UTC.</param>
	public Attributes(
		bool? enabled = default(bool?),
		System.DateTime? notBefore = default(System.DateTime?),
		System.DateTime? expires = default(System.DateTime?),
		System.DateTime? created = default(System.DateTime?),
		System.DateTime? updated = default(System.DateTime?)
	)
	{
		Enabled = enabled;
		NotBefore = notBefore;
		Expires = expires;
		Created = created;
		Updated = updated;
		CustomInit();
	}

	/// <summary>
	/// An initialization method that performs custom operations like setting defaults
	/// </summary>
	partial void CustomInit();

	/// <summary>
	/// Gets or sets determines whether the object is enabled.
	/// </summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; set; }

	/// <summary>
	/// Gets or sets not before date in UTC.
	/// </summary>
	// [JsonConverter(typeof(UnixTimeJsonConverter))]
	[JsonPropertyName("nbf")]
	public System.DateTime? NotBefore { get; set; }

	/// <summary>
	/// Gets or sets expiry date in UTC.
	/// </summary>
	// [JsonConverter(typeof(UnixTimeJsonConverter))]
	[JsonPropertyName("exp")]
	public System.DateTime? Expires { get; set; }

	/// <summary>
	/// Gets creation time in UTC.
	/// </summary>
	// [JsonConverter(typeof(UnixTimeJsonConverter))]
	[JsonPropertyName("created")]
	public System.DateTime? Created { get; private set; }

	/// <summary>
	/// Gets last updated time in UTC.
	/// </summary>
	// [JsonConverter(typeof(UnixTimeJsonConverter))]
	[JsonPropertyName("updated")]
	public System.DateTime? Updated { get; private set; }

}