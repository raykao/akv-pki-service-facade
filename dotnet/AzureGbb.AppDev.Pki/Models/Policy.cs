using Newtonsoft.Json;

namespace AzureGBB.AppDev.Pki.Models;

public partial class Policy
{
    /// <summary>
    /// Initializes a new instance of the Policy class.
    /// </summary>
    public Policy()
    {
        CustomInit();
    }

    /// <summary>
    /// Initializes a new instance of the Policy class.
    /// </summary>
    /// <param name="policy">The certificate policy.</param>
    public Policy(
        CertificatePolicy? certificatePolicy = default(CertificatePolicy)
		)
    {
        CertificatePolicy = certificatePolicy;
    }

    /// <summary>
    /// An initialization method that performs custom operations like setting defaults
    /// </summary>
    partial void CustomInit();

    /// <summary>
    /// Gets the certificate id.
    /// </summary>
    [JsonProperty(PropertyName = "policy")]
    public CertificatePolicy? CertificatePolicy { get; private set; }
}