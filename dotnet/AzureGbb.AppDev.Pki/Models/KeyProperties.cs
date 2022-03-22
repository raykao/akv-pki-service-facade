using Newtonsoft.Json;

namespace AzureGBB.AppDev.Pki.Models;

public partial class KeyProperties
{
    /// <summary>
    /// Initializes a new instance of the KeyProperties class.
    /// </summary>
    public KeyProperties()
    {
        CustomInit();
    }

    /// <summary>
    /// Initializes a new instance of the KeyProperties class.
    /// </summary>
    /// <param name="exportable">Indicates if the private key can be
    /// exported.</param>
    /// <param name="keyType">The type of key pair to be used for the
    /// certificate. Possible values include: 'EC', 'EC-HSM', 'RSA',
    /// 'RSA-HSM', 'oct'</param>
    /// <param name="keySize">The key size in bits. For example: 2048,
    /// 3072, or 4096 for RSA.</param>
    /// <param name="reuseKey">Indicates if the same key pair will be used
    /// on certificate renewal.</param>
    /// <param name="curve">Elliptic curve name. For valid values, see
    /// Microsoft.Azure.KeyVault.WebKey.JsonWebKeyCurveName. Possible values include: 'P-256', 'P-384',
    /// 'P-521', 'P-256K'</param>
    public KeyProperties(
        string curve,
        string keyType,
        int keySize
    )
    {
        Curve = curve;
        KeyType = keyType;
        KeySize = keySize;
        CustomInit();
    }

    /// <summary>
    /// An initialization method that performs custom operations like setting defaults
    /// </summary>
    partial void CustomInit();

    /// <summary>
    /// Gets or sets indicates if the private key can be exported.
    /// </summary>
    [JsonProperty(PropertyName = "exportable")]
    public bool Exportable { get; } = false;

    /// <summary>
    /// Gets or sets indicates if the same key pair will be used on
    /// certificate renewal.
    /// </summary>
    [JsonProperty(PropertyName = "reuse_key")]
    public bool ReuseKey { get; } = false;

    /// <summary>
    /// Gets or sets the type of key pair to be used for the certificate.
    /// Possible values include: 'EC', 'EC-HSM', 'RSA', 'RSA-HSM', 'oct'
    /// </summary>
    [JsonProperty(PropertyName = "kty")]
    public string KeyType { get; set; } = "RSA";

    /// <summary>
    /// Gets or sets the key size in bits. For example: 2048, 3072, or 4096
    /// for RSA.
    /// </summary>
    [JsonProperty(PropertyName = "key_size")]
    public int KeySize { get; set; } = 2048;

    /// <summary>
    /// Gets or sets elliptic curve name. For valid values, see
    /// Microsoft.Azure.KeyVault.WebKey.JsonWebKeyCurveName. Possible values include: 'P-256', 'P-384',
    /// 'P-521', 'P-256K'
    /// </summary>
    [JsonProperty(PropertyName = "crv")]
    public string? Curve { get; set; }

}