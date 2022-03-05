using Newtonsoft.Json;

namespace AzureGBB.AppDev.Pki.Models;
public partial class Trigger
{
	/// <summary>
	/// Initializes a new instance of the Trigger class.
	/// </summary>
	public Trigger()
	{
		CustomInit();
	}

	/// <summary>
	/// Initializes a new instance of the Trigger class.
	/// </summary>
	/// <param name="lifetimePercentage">Percentage of lifetime at which to
	/// trigger. Value should be between 1 and 99.</param>
	/// <param name="daysBeforeExpiry">Days before expiry to attempt
	/// renewal. Value should be between 1 and validity_in_months
	/// multiplied by 27. If validity_in_months is 36, then value should be
	/// between 1 and 972 (36 * 27).</param>
	public Trigger(int? lifetimePercentage = default(int?), int? daysBeforeExpiry = default(int?))
	{
		LifetimePercentage = lifetimePercentage;
		DaysBeforeExpiry = daysBeforeExpiry;
		CustomInit();
	}

	/// <summary>
	/// An initialization method that performs custom operations like setting defaults
	/// </summary>
	partial void CustomInit();

	/// <summary>
	/// Gets or sets percentage of lifetime at which to trigger. Value
	/// should be between 1 and 99.
	/// </summary>
	[JsonProperty(PropertyName = "lifetime_percentage")]
	public int? LifetimePercentage { get; set; }

	/// <summary>
	/// Gets or sets days before expiry to attempt renewal. Value should be
	/// between 1 and validity_in_months multiplied by 27. If
	/// validity_in_months is 36, then value should be between 1 and 972
	/// (36 * 27).
	/// </summary>
	[JsonProperty(PropertyName = "days_before_expiry")]
	public int? DaysBeforeExpiry { get; set; }
}