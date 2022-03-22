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
		BasicConstraints? basicConstraints = null
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
	public IList<string>? Ekus { get; set; } = new List<string> {"1.3.6.1.5.5.7.3.2", "1.3.6.1.5.5.7.3.1"};

	[JsonProperty(PropertyName = "sans")]
	public SubjectAlternativeNames? SubjectAlternativeNames { get; set; }

	[JsonProperty(PropertyName = "key_usage")]
	public IList<string>? KeyUsage { get; set; } = new List<string> {"keyCertSign"};

	[JsonProperty(PropertyName = "validity_months")]
	public int? ValidityInMonths { get; set; }

	[JsonProperty("basic_constraints")]
	public BasicConstraints? BasicConstraints { get; set; } = new BasicConstraints(true, 1);
}
