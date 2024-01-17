using System;

public class Region
{
	public CloudRegionCode Code;

	public string HostAndPort;

	public int Ping;

	public static CloudRegionCode Parse(string codeAsString)
	{
		codeAsString = codeAsString.ToLower();
		CloudRegionCode result = CloudRegionCode.none;
		if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
		{
			result = (CloudRegionCode)(int)Enum.Parse(typeof(CloudRegionCode), codeAsString);
		}
		return result;
	}

	public override string ToString()
	{
		return $"'{this.Code}' \t{this.Ping}ms \t{this.HostAndPort}";
	}
}
