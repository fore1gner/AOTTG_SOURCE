using System.Collections.Generic;
using ExitGames.Client.Photon;

public class WebRpcResponse
{
	public string DebugMessage { get; private set; }

	public string Name { get; private set; }

	public Dictionary<string, object> Parameters { get; private set; }

	public int ReturnCode { get; private set; }

	public WebRpcResponse(OperationResponse response)
	{
		response.Parameters.TryGetValue(209, out var value);
		this.Name = value as string;
		response.Parameters.TryGetValue(207, out value);
		this.ReturnCode = ((value == null) ? (-1) : ((byte)value));
		response.Parameters.TryGetValue(208, out value);
		this.Parameters = value as Dictionary<string, object>;
		response.Parameters.TryGetValue(206, out value);
		this.DebugMessage = value as string;
	}

	public string ToStringFull()
	{
		object[] args = new object[4]
		{
			this.Name,
			SupportClass.DictionaryToString(this.Parameters),
			this.ReturnCode,
			this.DebugMessage
		};
		return string.Format("{0}={2}: {1} \"{3}\"", args);
	}
}
