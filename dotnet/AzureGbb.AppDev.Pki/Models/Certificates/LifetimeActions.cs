using System.Text.Json.Serialization;


namespace AzureGBB.AppDev.Pki.Models.Certificates;

/// <summary>
/// Action and its trigger that will be performed by Key Vault over the
/// lifetime of a certificate.
/// </summary>
public partial class LifetimeAction
{
	/// <summary>
	/// Initializes a new instance of the LifetimeAction class.
	/// </summary>
	public LifetimeAction()
	{
		CustomInit();
	}

	/// <summary>
	/// Initializes a new instance of the LifetimeAction class.
	/// </summary>
	/// <param name="trigger">The condition that will execute the
	/// action.</param>
	/// <param name="action">The action that will be executed.</param>
	public LifetimeAction(Trigger? trigger = default(Trigger), Action? action = default(Action))
	{
		Trigger = trigger;
		Action = action;
		CustomInit();
	}

	/// <summary>
	/// An initialization method that performs custom operations like setting defaults
	/// </summary>
	partial void CustomInit();

	/// <summary>
	/// Gets or sets the condition that will execute the action.
	/// </summary>
	[JsonPropertyName("trigger")]
	public Trigger? Trigger { get; set; }

	/// <summary>
	/// Gets or sets the action that will be executed.
	/// </summary>
	[JsonPropertyName("action")]
	public Action? Action { get; set; }
}