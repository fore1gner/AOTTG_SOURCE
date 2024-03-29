using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

[Serializable]
public class ServerSettings : ScriptableObject
{
	public enum HostingOption
	{
		NotSet,
		PhotonCloud,
		SelfHosted,
		OfflineMode,
		BestRegion
	}

	public string AppID = string.Empty;

	[HideInInspector]
	public bool DisableAutoOpenWizard;

	public HostingOption HostType;

	public bool PingCloudServersOnAwake;

	public CloudRegionCode PreferredRegion;

	public ConnectionProtocol Protocol;

	public List<string> RpcList = new List<string>();

	public string ServerAddress = string.Empty;

	public int ServerPort = 5055;

	public override string ToString()
	{
		return string.Concat("ServerSettings: ", this.HostType, " ", this.ServerAddress);
	}

	public void UseCloud(string cloudAppid)
	{
		this.HostType = HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
	}

	public void UseCloud(string cloudAppid, CloudRegionCode code)
	{
		this.HostType = HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
		this.PreferredRegion = code;
	}

	public void UseCloudBestResion(string cloudAppid)
	{
		this.HostType = HostingOption.BestRegion;
		this.AppID = cloudAppid;
	}

	public void UseMyServer(string serverAddress, int serverPort, string application)
	{
		this.HostType = HostingOption.SelfHosted;
		this.AppID = ((application == null) ? "master" : application);
		this.ServerAddress = serverAddress;
		this.ServerPort = serverPort;
	}
}
