using System.Text.Json.Serialization;

namespace AzureGBB.AppDev.Pki.Models.Certificates;

public partial class CertificateAttributes : Attributes
{
	/// <summary>
	/// Initializes a new instance of the CertificateAttributes class.
	/// </summary>
	public CertificateAttributes()
	{
		CustomInit();
	}

	/// <summary>
	/// Initializes a new instance of the CertificateAttributes class.
	/// </summary>
	/// <param name="enabled">Determines whether the object is
	/// enabled.</param>
	/// <param name="notBefore">Not before date in UTC.</param>
	/// <param name="expires">Expiry date in UTC.</param>
	/// <param name="created">Creation time in UTC.</param>
	/// <param name="updated">Last updated time in UTC.</param>
	/// <param name="recoveryLevel">Reflects the deletion recovery level
	/// currently in effect for certificates in the current vault. If it
	/// contains 'Purgeable', the certificate can be permanently deleted by
	/// a privileged user; otherwise, only the system can purge the
	/// certificate, at the end of the retention interval. Possible values
	/// include: 'Purgeable', 'Recoverable+Purgeable', 'Recoverable',
	/// 'Recoverable+ProtectedSubscription'</param>
	public CertificateAttributes(
		bool? enabled = default(bool?),
		System.DateTime? notBefore = default(System.DateTime?),
		System.DateTime? expires = default(System.DateTime?),
		System.DateTime? created = default(System.DateTime?),
		System.DateTime? updated = default(System.DateTime?),
		string? recoveryLevel = default(string)
	)
	: base(
		enabled,
		notBefore,
		expires,
		created,
		updated
	)
	{
		RecoveryLevel = recoveryLevel;
		CustomInit();
	}

	/// <summary>
	/// An initialization method that performs custom operations like setting defaults
	/// </summary>
	partial void CustomInit();

	/// <summary>
	/// Gets reflects the deletion recovery level currently in effect for
	/// certificates in the current vault. If it contains 'Purgeable', the
	/// certificate can be permanently deleted by a privileged user;
	/// otherwise, only the system can purge the certificate, at the end of
	/// the retention interval. Possible values include: 'Purgeable',
	/// 'Recoverable+Purgeable', 'Recoverable',
	/// 'Recoverable+ProtectedSubscription'
	/// </summary>
	[JsonPropertyName("recoveryLevel")]
	public string? RecoveryLevel { get; private set; }

}