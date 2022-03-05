// Partially Replicating Microsoft.Azure.KeyVault.Models
using Newtonsoft.Json;

namespace AzureGBB.AppDev.Pki.Models;
public partial class X509CertificateProperties
{
	public X509CertificateProperties(
		string? subject = default(string), 
		IList<string>? ekus = default(IList<string>), 
		SubjectAlternativeNames? subjectAlternativeNames = default(SubjectAlternativeNames), 
		IList<string>? keyUsage = default(IList<string>), 
		int? validityInMonths = default(int?),
		BasicConstraintsExtension? basicConstraints = null
	)
	{
		Subject = subject;
		Ekus = ekus;
		SubjectAlternativeNames = subjectAlternativeNames;
		KeyUsage = keyUsage;
		ValidityInMonths = validityInMonths;
		BasicConstraints = basicConstraints;
	}

	[JsonProperty(PropertyName = "subject")]
	public string? Subject { get; set; }

	[JsonProperty(PropertyName = "ekus")]
	public IList<string>? Ekus { get; set; }

	[JsonProperty(PropertyName = "sans")]
	public SubjectAlternativeNames? SubjectAlternativeNames { get; set; }

	[JsonProperty(PropertyName = "key_usage")]
	public IList<string>? KeyUsage { get; set; }

	[JsonProperty(PropertyName = "validity_months")]
	public int? ValidityInMonths { get; set; }

	[JsonProperty("basic_constraints")]
	public BasicConstraintsExtension? BasicConstraints { get; set; }
}
