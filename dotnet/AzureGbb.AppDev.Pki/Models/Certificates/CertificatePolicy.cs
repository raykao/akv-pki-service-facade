using System.Text.Json.Serialization;

namespace AzureGBB.AppDev.Pki.Models.Certificates;

public partial class CertificatePolicy
{
    /// <summary>
    /// Initializes a new instance of the CertificatePolicy class.
    /// </summary>
    public CertificatePolicy()
    {
        CustomInit();
    }

    /// <summary>
    /// Initializes a new instance of the CertificatePolicy class.
    /// </summary>
    /// <param name="id">The certificate id.</param>
    /// <param name="keyProperties">Properties of the key backing a
    /// certificate.</param>
    /// <param name="secretProperties">Properties of the secret backing a
    /// certificate.</param>
    /// <param name="x509CertificateProperties">Properties of the X509
    /// component of a certificate.</param>
    /// <param name="lifetimeActions">Actions that will be performed by Key
    /// Vault over the lifetime of a certificate.</param>
    /// <param name="issuerParameters">Parameters for the issuer of the
    /// X509 component of a certificate.</param>
    /// <param name="attributes">The certificate attributes.</param>
    public CertificatePolicy(
        string? id = default(string),
        KeyProperties? keyProperties = default(KeyProperties),
        SecretProperties? secretProperties = default(SecretProperties),
        X509CertificateProperties? x509CertificateProperties = default(X509CertificateProperties),
        IList<LifetimeAction>? lifetimeActions = default(IList<LifetimeAction>),
        IssuerParameters? issuerParameters = default(IssuerParameters), 
        CertificateAttributes? attributes = default(CertificateAttributes))
    {
        Id = id;
        KeyProperties = keyProperties;
        SecretProperties = secretProperties;
        X509CertificateProperties = x509CertificateProperties;
        LifetimeActions = lifetimeActions;
        IssuerParameters = issuerParameters;
        Attributes = attributes;
        CustomInit();
    }

    /// <summary>
    /// An initialization method that performs custom operations like setting defaults
    /// </summary>
    partial void CustomInit();

    /// <summary>
    /// Gets the certificate id.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; private set; }

    /// <summary>
    /// Gets or sets properties of the key backing a certificate.
    /// </summary>
    [JsonPropertyName("key_props")]
    public KeyProperties? KeyProperties { get; set; }

    /// <summary>
    /// Gets or sets properties of the secret backing a certificate.
    /// </summary>
    [JsonPropertyName("secret_props")]
    public SecretProperties? SecretProperties { get; set; }

    /// <summary>
    /// Gets or sets properties of the X509 component of a certificate.
    /// </summary>
    [JsonPropertyName("x509_props")]
    public X509CertificateProperties? X509CertificateProperties { get; set; }

    /// <summary>
    /// Gets or sets actions that will be performed by Key Vault over the
    /// lifetime of a certificate.
    /// </summary>
    [JsonPropertyName("lifetime_actions")]
    public IList<LifetimeAction>? LifetimeActions { get; set; }

    /// <summary>
    /// Gets or sets parameters for the issuer of the X509 component of a
    /// certificate.
    /// </summary>
    [JsonPropertyName("issuer")]
    public IssuerParameters? IssuerParameters { get; set; }

    /// <summary>
    /// Gets or sets the certificate attributes.
    /// </summary>
    [JsonPropertyName("attributes")]
    public CertificateAttributes? Attributes { get; set; }
}