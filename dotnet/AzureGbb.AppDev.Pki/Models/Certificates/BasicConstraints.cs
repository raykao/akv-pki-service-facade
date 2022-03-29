using System.Text.Json.Serialization;

namespace AzureGBB.AppDev.Pki.Models.Certificates;
public class BasicConstraints
{
	public BasicConstraints(bool isCa, int pathLenConstraint)
	{
		IsCA = isCa;
		PathLenConstraint = pathLenConstraint;
	}

	// ReSharper disable once InconsistentNaming
	[JsonPropertyName("ca")] public bool IsCA { get; set; } = true;
	[JsonPropertyName("path_len_constraint")] public int PathLenConstraint { get; set; } = 1;
}