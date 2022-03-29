using System.Text.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AzureGBB.AppDev.Pki.Models.Certificates;

/// <summary>
/// The subject alternate names of a X509 object.
/// </summary>
public partial class SubjectAlternativeNames
{
	/// <summary>
	/// Initializes a new instance of the SubjectAlternativeNames class.
	/// </summary>
	public SubjectAlternativeNames()
	{
		CustomInit();
	}

	/// <summary>
	/// Initializes a new instance of the SubjectAlternativeNames class.
	/// </summary>
	/// <param name="emails">Email addresses.</param>
	/// <param name="dnsNames">Domain names.</param>
	/// <param name="upns">User principal names.</param>
	public SubjectAlternativeNames(
		IList<string>? emails = default(IList<string>), 
		IList<string>? dnsNames = default(IList<string>), 
		IList<string>? upns = default(IList<string>)
	)
	{
		Emails = emails;
		DnsNames = dnsNames;
		Upns = upns;
		CustomInit();
	}

	/// <summary>
	/// An initialization method that performs custom operations like setting defaults
	/// </summary>
	partial void CustomInit();

	/// <summary>
	/// Gets or sets email addresses.
	/// </summary>
	[JsonPropertyName("emails")]
	public IList<string>? Emails { get; set; }

	/// <summary>
	/// Gets or sets domain names.
	/// </summary>
	[JsonPropertyName("dns_names")]
	public IList<string>? DnsNames { get; set; }

	/// <summary>
	/// Gets or sets user principal names.
	/// </summary>
	[JsonPropertyName("upns")]
	public IList<string>? Upns { get; set; }

}