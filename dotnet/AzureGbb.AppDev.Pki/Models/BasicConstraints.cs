using Newtonsoft.Json;

namespace AzureGBB.AppDev.Pki.Models;
public class BasicConstraintsExtension
{
	public BasicConstraintsExtension(bool isCa, int pathLenConstraint)
	{
		IsCA = isCa;
		PathLenConstraint = pathLenConstraint;
	}

	// ReSharper disable once InconsistentNaming
	[JsonProperty("ca")] public bool IsCA { get; set; }
	[JsonProperty("path_len_constraint")] public int PathLenConstraint { get; set; }
}