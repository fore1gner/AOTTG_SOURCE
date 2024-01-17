using System.Collections.Generic;
using UnityEngine;

namespace Settings;

internal class MultiplayerSettings : SaveableSettingsContainer
{
	public static string PublicLobby = "01042015";

	public static string PrivateLobby = "verified343";

	public static string PublicAppId = "5578b046-8264-438c-99c5-fb15c71b6744";

	public IntSetting LobbyMode = new IntSetting(0);

	public IntSetting AppIdMode = new IntSetting(0);

	public StringSetting CustomLobby = new StringSetting(string.Empty);

	public StringSetting CustomAppId = new StringSetting(string.Empty);

	public StringSetting LanIP = new StringSetting(string.Empty);

	public IntSetting LanPort = new IntSetting(5055);

	public StringSetting LanPassword = new StringSetting(string.Empty);

	public MultiplayerServerType CurrentMultiplayerServerType;

	public readonly Dictionary<MultiplayerRegion, string> CloudAddresses = new Dictionary<MultiplayerRegion, string>
	{
		{
			MultiplayerRegion.EU,
			"app-eu.exitgamescloud.com"
		},
		{
			MultiplayerRegion.US,
			"app-us.exitgamescloud.com"
		},
		{
			MultiplayerRegion.SA,
			"app-sa.exitgames.com"
		},
		{
			MultiplayerRegion.ASIA,
			"app-asia.exitgamescloud.com"
		},
		{
			MultiplayerRegion.CN,
			"app-asia.exitgamescloud.com"
		}
	};

	public readonly Dictionary<MultiplayerRegion, string> PublicAddresses = new Dictionary<MultiplayerRegion, string>
	{
		{
			MultiplayerRegion.EU,
			"135.125.239.180"
		},
		{
			MultiplayerRegion.US,
			"142.44.242.29"
		},
		{
			MultiplayerRegion.SA,
			"108.181.69.221"
		},
		{
			MultiplayerRegion.ASIA,
			"51.79.164.137"
		},
		{
			MultiplayerRegion.CN,
			"47.116.117.128"
		}
	};

	public readonly int DefaultPort = 5055;

	public StringSetting Name = new StringSetting("GUEST" + Random.Range(0, 100000), 50);

	public StringSetting Guild = new StringSetting(string.Empty, 50);

	protected override string FileName => "Multiplayer.json";

	public void ConnectServer(MultiplayerRegion region)
	{
		FengGameManagerMKII.JustLeftRoom = false;
		PhotonNetwork.Disconnect();
		if (this.AppIdMode.Value == 0)
		{
			string masterServerAddress = this.PublicAddresses[region];
			this.CurrentMultiplayerServerType = MultiplayerServerType.Public;
			PhotonNetwork.ConnectToMaster(masterServerAddress, this.DefaultPort, string.Empty, this.GetCurrentLobby());
		}
		else
		{
			string masterServerAddress2 = this.CloudAddresses[region];
			this.CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
			PhotonNetwork.ConnectToMaster(masterServerAddress2, this.DefaultPort, this.CustomAppId.Value, this.GetCurrentLobby());
		}
	}

	public string GetCurrentLobby()
	{
		if (this.LobbyMode.Value == 0)
		{
			return MultiplayerSettings.PublicLobby;
		}
		if (this.LobbyMode.Value == 1)
		{
			return MultiplayerSettings.PrivateLobby;
		}
		return this.CustomLobby.Value;
	}

	public void ConnectLAN()
	{
		PhotonNetwork.Disconnect();
		if (PhotonNetwork.ConnectToMaster(this.LanIP.Value, this.LanPort.Value, string.Empty, this.GetCurrentLobby()))
		{
			this.CurrentMultiplayerServerType = MultiplayerServerType.LAN;
			FengGameManagerMKII.PrivateServerAuthPass = this.LanPassword.Value;
		}
	}

	public void ConnectOffline()
	{
		PhotonNetwork.Disconnect();
		PhotonNetwork.offlineMode = true;
		this.CurrentMultiplayerServerType = MultiplayerServerType.Cloud;
		FengGameManagerMKII.instance.OnJoinedLobby();
	}
}
