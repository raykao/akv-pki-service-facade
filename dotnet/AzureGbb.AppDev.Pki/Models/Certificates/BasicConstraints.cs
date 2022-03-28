using Newtonsoft.Json;

namespace AzureGBB.AppDev.Pki.Models.Certificates;
public class BasicConstraints
{
	public BasicConstraints(bool isCa, int pathLenConstraint)
	{
		IsCA = isCa;
		PathLenConstraint = pathLenConstraint;
	}

	// ReSharper disable once InconsistentNaming
	[JsonProperty("ca")] public bool IsCA { get; set; } = true;
	[JsonProperty("path_len_constraint")] public int PathLenConstraint { get; set; } = 1;
}