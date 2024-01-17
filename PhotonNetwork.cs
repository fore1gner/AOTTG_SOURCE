using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public static class PhotonNetwork
{
	public delegate void EventCallback(byte eventCode, object content, int senderId);

	private static bool _mAutomaticallySyncScene;

	private static bool autoJoinLobbyField;

	public static bool InstantiateInRoomOnly;

	private static bool isOfflineMode;

	internal static int lastUsedViewSubId;

	internal static int lastUsedViewSubIdStatic;

	public static PhotonLogLevel logLevel;

	private static bool m_autoCleanUpPlayerObjects;

	private static bool m_isMessageQueueRunning;

	internal static List<int> manuallyAllocatedViewIds;

	public static readonly int MAX_VIEW_IDS;

	internal static NetworkingPeer networkingPeer;

	private static Room offlineModeRoom;

	public static EventCallback OnEventCall;

	internal static readonly PhotonHandler photonMono;

	public static ServerSettings PhotonServerSettings;

	public static float precisionForFloatSynchronization;

	public static float precisionForQuaternionSynchronization;

	public static float precisionForVectorSynchronization;

	public static Dictionary<string, GameObject> PrefabCache;

	private static int sendInterval;

	private static int sendIntervalOnSerialize;

	public static HashSet<GameObject> SendMonoMessageTargets;

	public const string serverSettingsAssetFile = "PhotonServerSettings";

	public const string serverSettingsAssetPath = "Assets/Photon Unity Networking/Resources/PhotonServerSettings.asset";

	public static bool UseNameServer;

	public static bool UsePrefabCache;

	public const string versionPUN = "1.28";

	public static AuthenticationValues AuthValues
	{
		get
		{
			if (PhotonNetwork.networkingPeer != null)
			{
				return PhotonNetwork.networkingPeer.CustomAuthenticationValues;
			}
			return null;
		}
		set
		{
			if (PhotonNetwork.networkingPeer != null)
			{
				PhotonNetwork.networkingPeer.CustomAuthenticationValues = value;
			}
		}
	}

	public static bool autoCleanUpPlayerObjects
	{
		get
		{
			return PhotonNetwork.m_autoCleanUpPlayerObjects;
		}
		set
		{
			if (PhotonNetwork.room != null)
			{
				Debug.LogError("Setting autoCleanUpPlayerObjects while in a room is not supported.");
			}
			else
			{
				PhotonNetwork.m_autoCleanUpPlayerObjects = value;
			}
		}
	}

	public static bool autoJoinLobby
	{
		get
		{
			return PhotonNetwork.autoJoinLobbyField;
		}
		set
		{
			PhotonNetwork.autoJoinLobbyField = value;
		}
	}

	public static bool automaticallySyncScene
	{
		get
		{
			return PhotonNetwork._mAutomaticallySyncScene;
		}
		set
		{
			PhotonNetwork._mAutomaticallySyncScene = value;
			if (PhotonNetwork._mAutomaticallySyncScene && PhotonNetwork.room != null)
			{
				PhotonNetwork.networkingPeer.LoadLevelIfSynced();
			}
		}
	}

	public static bool connected
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return true;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return false;
			}
			if (!PhotonNetwork.networkingPeer.IsInitialConnect && PhotonNetwork.networkingPeer.State != PeerStates.PeerCreated && PhotonNetwork.networkingPeer.State != PeerStates.Disconnected && PhotonNetwork.networkingPeer.State != PeerStates.Disconnecting)
			{
				return PhotonNetwork.networkingPeer.State != PeerStates.ConnectingToNameServer;
			}
			return false;
		}
	}

	public static bool connectedAndReady
	{
		get
		{
			if (!PhotonNetwork.connected)
			{
				return false;
			}
			if (!PhotonNetwork.offlineMode)
			{
				switch (PhotonNetwork.connectionStateDetailed)
				{
				case PeerStates.PeerCreated:
				case PeerStates.ConnectingToGameserver:
				case PeerStates.Joining:
				case PeerStates.Leaving:
				case PeerStates.ConnectingToMasterserver:
				case PeerStates.Disconnecting:
				case PeerStates.Disconnected:
				case PeerStates.ConnectingToNameServer:
				case PeerStates.Authenticating:
					return false;
				}
			}
			return true;
		}
	}

	public static bool connecting
	{
		get
		{
			if (PhotonNetwork.networkingPeer.IsInitialConnect)
			{
				return !PhotonNetwork.offlineMode;
			}
			return false;
		}
	}

	public static ConnectionState connectionState
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return ConnectionState.Connected;
			}
			if (PhotonNetwork.networkingPeer != null)
			{
				switch (PhotonNetwork.networkingPeer.PeerState)
				{
				case PeerStateValue.Disconnected:
					return ConnectionState.Disconnected;
				case PeerStateValue.Connecting:
					return ConnectionState.Connecting;
				case PeerStateValue.Connected:
					return ConnectionState.Connected;
				case PeerStateValue.Disconnecting:
					return ConnectionState.Disconnecting;
				case PeerStateValue.InitializingApplication:
					return ConnectionState.InitializingApplication;
				}
			}
			return ConnectionState.Disconnected;
		}
	}

	public static PeerStates connectionStateDetailed
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				if (PhotonNetwork.offlineModeRoom != null)
				{
					return PeerStates.Joined;
				}
				return PeerStates.ConnectedToMaster;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return PeerStates.Disconnected;
			}
			return PhotonNetwork.networkingPeer.State;
		}
	}

	public static int countOfPlayers => PhotonNetwork.networkingPeer.mPlayersInRoomsCount + PhotonNetwork.networkingPeer.mPlayersOnMasterCount;

	public static int countOfPlayersInRooms => PhotonNetwork.networkingPeer.mPlayersInRoomsCount;

	public static int countOfPlayersOnMaster => PhotonNetwork.networkingPeer.mPlayersOnMasterCount;

	public static int countOfRooms => PhotonNetwork.networkingPeer.mGameCount;

	public static bool CrcCheckEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.CrcEnabled;
		}
		set
		{
			if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
			{
				PhotonNetwork.networkingPeer.CrcEnabled = value;
			}
			else
			{
				Debug.Log("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays " + PhotonNetwork.networkingPeer.CrcEnabled);
			}
		}
	}

	public static List<FriendInfo> Friends { get; set; }

	public static int FriendsListAge
	{
		get
		{
			if (PhotonNetwork.networkingPeer != null)
			{
				return PhotonNetwork.networkingPeer.FriendsListAge;
			}
			return 0;
		}
	}

	public static string gameVersion
	{
		get
		{
			return PhotonNetwork.networkingPeer.mAppVersion;
		}
		set
		{
			PhotonNetwork.networkingPeer.mAppVersion = value;
		}
	}

	public static bool inRoom => PhotonNetwork.connectionStateDetailed == PeerStates.Joined;

	public static bool insideLobby => PhotonNetwork.networkingPeer.insideLobby;

	public static bool isMasterClient
	{
		get
		{
			if (!PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.networkingPeer.mMasterClient == PhotonNetwork.networkingPeer.mLocalActor;
			}
			return true;
		}
	}

	public static bool isMessageQueueRunning
	{
		get
		{
			return PhotonNetwork.m_isMessageQueueRunning;
		}
		set
		{
			if (value)
			{
				PhotonHandler.StartFallbackSendAckThread();
			}
			PhotonNetwork.networkingPeer.IsSendingOnlyAcks = !value;
			PhotonNetwork.m_isMessageQueueRunning = value;
		}
	}

	public static bool isNonMasterClientInRoom
	{
		get
		{
			if (!PhotonNetwork.isMasterClient)
			{
				return PhotonNetwork.room != null;
			}
			return false;
		}
	}

	public static TypedLobby lobby
	{
		get
		{
			return PhotonNetwork.networkingPeer.lobby;
		}
		set
		{
			PhotonNetwork.networkingPeer.lobby = value;
		}
	}

	public static PhotonPlayer masterClient
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.mMasterClient;
		}
	}

	[Obsolete("Used for compatibility with Unity networking only.")]
	public static int maxConnections
	{
		get
		{
			if (PhotonNetwork.room == null)
			{
				return 0;
			}
			return PhotonNetwork.room.maxPlayers;
		}
		set
		{
			PhotonNetwork.room.maxPlayers = value;
		}
	}

	public static int MaxResendsBeforeDisconnect
	{
		get
		{
			return PhotonNetwork.networkingPeer.SentCountAllowance;
		}
		set
		{
			if (value < 3)
			{
				value = 3;
			}
			if (value > 10)
			{
				value = 10;
			}
			PhotonNetwork.networkingPeer.SentCountAllowance = value;
		}
	}

	public static bool NetworkStatisticsEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.TrafficStatsEnabled;
		}
		set
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = value;
		}
	}

	public static bool offlineMode
	{
		get
		{
			return PhotonNetwork.isOfflineMode;
		}
		set
		{
			if (value == PhotonNetwork.isOfflineMode)
			{
				return;
			}
			if (value && PhotonNetwork.connected)
			{
				Debug.LogError("Can't start OFFLINE mode while connected!");
				return;
			}
			if (PhotonNetwork.networkingPeer.PeerState != 0)
			{
				PhotonNetwork.networkingPeer.Disconnect();
			}
			PhotonNetwork.isOfflineMode = value;
			if (PhotonNetwork.isOfflineMode)
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
				PhotonNetwork.networkingPeer.ChangeLocalID(1);
				PhotonNetwork.networkingPeer.mMasterClient = PhotonNetwork.player;
			}
			else
			{
				PhotonNetwork.offlineModeRoom = null;
				PhotonNetwork.networkingPeer.ChangeLocalID(-1);
				PhotonNetwork.networkingPeer.mMasterClient = null;
			}
		}
	}

	public static PhotonPlayer[] otherPlayers
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mOtherPlayerListCopy;
		}
	}

	public static int PacketLossByCrcCheck => PhotonNetwork.networkingPeer.PacketLossByCrc;

	public static PhotonPlayer player
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.mLocalActor;
		}
	}

	public static PhotonPlayer[] playerList
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mPlayerListCopy;
		}
	}

	public static string playerName
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayerName;
		}
		set
		{
			PhotonNetwork.networkingPeer.PlayerName = value;
		}
	}

	public static int ResentReliableCommands => PhotonNetwork.networkingPeer.ResentReliableCommands;

	public static Room room
	{
		get
		{
			if (PhotonNetwork.isOfflineMode)
			{
				return PhotonNetwork.offlineModeRoom;
			}
			return PhotonNetwork.networkingPeer.mCurrentGame;
		}
	}

	public static int sendRate
	{
		get
		{
			return 1000 / PhotonNetwork.sendInterval;
		}
		set
		{
			PhotonNetwork.sendInterval = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateInterval = PhotonNetwork.sendInterval;
			}
			if (value < PhotonNetwork.sendRateOnSerialize)
			{
				PhotonNetwork.sendRateOnSerialize = value;
			}
		}
	}

	public static int sendRateOnSerialize
	{
		get
		{
			return 1000 / PhotonNetwork.sendIntervalOnSerialize;
		}
		set
		{
			if (value > PhotonNetwork.sendRate)
			{
				Debug.LogError("Error, can not set the OnSerialize SendRate more often then the overall SendRate");
				value = PhotonNetwork.sendRate;
			}
			PhotonNetwork.sendIntervalOnSerialize = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateIntervalOnSerialize = PhotonNetwork.sendIntervalOnSerialize;
			}
		}
	}

	public static ServerConnection Server => PhotonNetwork.networkingPeer.server;

	public static string ServerAddress
	{
		get
		{
			if (PhotonNetwork.networkingPeer != null)
			{
				return PhotonNetwork.networkingPeer.ServerAddress;
			}
			return "<not connected>";
		}
	}

	public static double time
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return Time.time;
			}
			return (double)PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds / 1000.0;
		}
	}

	public static int unreliableCommandsLimit
	{
		get
		{
			return PhotonNetwork.networkingPeer.LimitOfUnreliableCommands;
		}
		set
		{
			PhotonNetwork.networkingPeer.LimitOfUnreliableCommands = value;
		}
	}

	static PhotonNetwork()
	{
		PhotonNetwork._mAutomaticallySyncScene = false;
		PhotonNetwork.autoJoinLobbyField = true;
		PhotonNetwork.InstantiateInRoomOnly = true;
		PhotonNetwork.isOfflineMode = false;
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
		PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
		PhotonNetwork.m_autoCleanUpPlayerObjects = true;
		PhotonNetwork.m_isMessageQueueRunning = true;
		PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		PhotonNetwork.MAX_VIEW_IDS = 1000;
		PhotonNetwork.offlineModeRoom = null;
		PhotonNetwork.PhotonServerSettings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));
		PhotonNetwork.precisionForFloatSynchronization = 0.01f;
		PhotonNetwork.precisionForQuaternionSynchronization = 1f;
		PhotonNetwork.precisionForVectorSynchronization = 9.9E-05f;
		PhotonNetwork.PrefabCache = new Dictionary<string, GameObject>();
		PhotonNetwork.sendInterval = 50;
		PhotonNetwork.sendIntervalOnSerialize = 100;
		PhotonNetwork.UseNameServer = true;
		PhotonNetwork.UsePrefabCache = true;
		Application.runInBackground = true;
		GameObject gameObject = new GameObject();
		PhotonNetwork.photonMono = gameObject.AddComponent<PhotonHandler>();
		gameObject.name = "PhotonMono";
		gameObject.hideFlags = HideFlags.HideInHierarchy;
		PhotonNetwork.networkingPeer = new NetworkingPeer(PhotonNetwork.photonMono, string.Empty, ConnectionProtocol.Udp);
		CustomTypes.Register();
	}

	private static int[] AllocateSceneViewIDs(int countOfNewViews)
	{
		int[] array = new int[countOfNewViews];
		for (int i = 0; i < countOfNewViews; i++)
		{
			array[i] = PhotonNetwork.AllocateViewID(0);
		}
		return array;
	}

	public static int AllocateViewID()
	{
		int num = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		PhotonNetwork.manuallyAllocatedViewIds.Add(num);
		return num;
	}

	private static int AllocateViewID(int ownerId)
	{
		if (ownerId == 0)
		{
			int num = PhotonNetwork.lastUsedViewSubIdStatic;
			int num2 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
			for (int i = 1; i < PhotonNetwork.MAX_VIEW_IDS; i++)
			{
				num = (num + 1) % PhotonNetwork.MAX_VIEW_IDS;
				if (num != 0)
				{
					int num3 = num + num2;
					if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num3))
					{
						PhotonNetwork.lastUsedViewSubIdStatic = num;
						return num3;
					}
				}
			}
			throw new Exception($"AllocateViewID() failed. Room (user {ownerId}) is out of subIds, as all room viewIDs are used.");
		}
		int num4 = PhotonNetwork.lastUsedViewSubId;
		int num5 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
		for (int j = 1; j < PhotonNetwork.MAX_VIEW_IDS; j++)
		{
			num4 = (num4 + 1) % PhotonNetwork.MAX_VIEW_IDS;
			if (num4 != 0)
			{
				int num6 = num4 + num5;
				if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num6) && !PhotonNetwork.manuallyAllocatedViewIds.Contains(num6))
				{
					PhotonNetwork.lastUsedViewSubId = num4;
					return num6;
				}
			}
		}
		throw new Exception($"AllocateViewID() failed. User {ownerId} is out of subIds, as all viewIDs are used.");
	}

	public static bool CloseConnection(PhotonPlayer kickPlayer)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return false;
		}
		if (!PhotonNetwork.player.isMasterClient)
		{
			Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
			return false;
		}
		if (kickPlayer == null)
		{
			Debug.LogError("CloseConnection: No such player connected!");
			return false;
		}
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		raiseEventOptions.TargetActors = new int[1] { kickPlayer.ID };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		return PhotonNetwork.networkingPeer.OpRaiseEvent(203, null, sendReliable: true, raiseEventOptions2);
	}

	public static bool ConnectToBestCloudServer(string gameVersion)
	{
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		CloudRegionCode bestRegionCodeInPreferences = PhotonHandler.BestRegionCodeInPreferences;
		if (bestRegionCodeInPreferences != CloudRegionCode.none)
		{
			Debug.Log("Best region found in PlayerPrefs. Connecting to: " + bestRegionCodeInPreferences);
			return PhotonNetwork.networkingPeer.ConnectToRegionMaster(bestRegionCodeInPreferences);
		}
		return PhotonNetwork.networkingPeer.ConnectToNameServer();
	}

	public static bool ConnectToMaster(string masterServerAddress, int port, string appID, string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != 0)
		{
			Debug.LogWarning("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			Debug.LogWarning("ConnectToMaster() disabled the offline mode. No longer offline.");
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			Debug.LogWarning("ConnectToMaster() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.SetApp(appID, gameVersion);
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.MasterServerAddress = masterServerAddress + ":" + port;
		return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
	}

	public static bool ConnectUsingSettings(string gameVersion)
	{
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		PhotonNetwork.SwitchToProtocol(PhotonNetwork.PhotonServerSettings.Protocol);
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			PhotonNetwork.offlineMode = true;
			return true;
		}
		if (PhotonNetwork.offlineMode)
		{
			Debug.LogWarning("ConnectUsingSettings() disabled the offline mode. No longer offline.");
		}
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
		{
			PhotonNetwork.networkingPeer.IsUsingNameServer = false;
			PhotonNetwork.networkingPeer.MasterServerAddress = PhotonNetwork.PhotonServerSettings.ServerAddress + ":" + PhotonNetwork.PhotonServerSettings.ServerPort;
			return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
		{
			return PhotonNetwork.ConnectToBestCloudServer(gameVersion);
		}
		return PhotonNetwork.networkingPeer.ConnectToRegionMaster(PhotonNetwork.PhotonServerSettings.PreferredRegion);
	}

	public static bool CreateRoom(string roomName)
	{
		return PhotonNetwork.CreateRoom(roomName, null, null);
	}

	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				Debug.LogError("CreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.offlineModeRoom = new Room(roomName, roomOptions);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (PhotonNetwork.networkingPeer.server == ServerConnection.MasterServer && PhotonNetwork.connectedAndReady)
		{
			return PhotonNetwork.networkingPeer.OpCreateGame(roomName, roomOptions, typedLobby);
		}
		Debug.LogError("CreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
		return false;
	}

	[Obsolete("Use overload with RoomOptions and TypedLobby parameters.")]
	public static bool CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers)
	{
		RoomOptions roomOptions = new RoomOptions
		{
			isVisible = isVisible,
			isOpen = isOpen,
			maxPlayers = maxPlayers
		};
		return PhotonNetwork.CreateRoom(roomName, roomOptions, null);
	}

	[Obsolete("Use overload with RoomOptions and TypedLobby parameters.")]
	public static bool CreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers, Hashtable customRoomProperties, string[] propsToListInLobby)
	{
		RoomOptions roomOptions = new RoomOptions
		{
			isVisible = isVisible,
			isOpen = isOpen,
			maxPlayers = maxPlayers,
			customRoomProperties = customRoomProperties,
			customRoomPropertiesForLobby = propsToListInLobby
		};
		return PhotonNetwork.CreateRoom(roomName, roomOptions, null);
	}

	public static void Destroy(PhotonView targetView)
	{
		if (targetView != null)
		{
			PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetView.gameObject, !PhotonNetwork.inRoom);
		}
		else
		{
			Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
		}
	}

	public static void Destroy(GameObject targetGo)
	{
		PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetGo, !PhotonNetwork.inRoom);
	}

	public static void DestroyAll()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.networkingPeer.DestroyAll(localOnly: false);
		}
		else
		{
			Debug.LogError("Couldn't call DestroyAll() as only the master client is allowed to call this.");
		}
	}

	public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
	{
		if (PhotonNetwork.player == null)
		{
			Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
		}
		PhotonNetwork.DestroyPlayerObjects(targetPlayer.ID);
	}

	public static void DestroyPlayerObjects(int targetPlayerId)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			if (PhotonNetwork.player.isMasterClient || targetPlayerId == PhotonNetwork.player.ID)
			{
				PhotonNetwork.networkingPeer.DestroyPlayerObjects(targetPlayerId, localOnly: false);
			}
			else
			{
				Debug.LogError("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + PhotonNetwork.isMasterClient);
			}
		}
	}

	public static void Disconnect()
	{
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			PhotonNetwork.offlineModeRoom = null;
			PhotonNetwork.networkingPeer.State = PeerStates.Disconnecting;
			PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.Disconnect);
		}
		else if (PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.Disconnect();
		}
	}

	public static void FetchServerTimestamp()
	{
		if (PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.FetchServerTimestamp();
		}
	}

	public static bool FindFriends(string[] friendsToFind)
	{
		if (PhotonNetwork.networkingPeer != null && !PhotonNetwork.isOfflineMode)
		{
			return PhotonNetwork.networkingPeer.OpFindFriends(friendsToFind);
		}
		return false;
	}

	public static int GetPing()
	{
		return PhotonNetwork.networkingPeer.RoundTripTime;
	}

	public static RoomInfo[] GetRoomList()
	{
		if (!PhotonNetwork.offlineMode && PhotonNetwork.networkingPeer != null)
		{
			return PhotonNetwork.networkingPeer.mGameListCopy;
		}
		return new RoomInfo[0];
	}

	[Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
	public static void InitializeSecurity()
	{
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group)
	{
		return PhotonNetwork.Instantiate(prefabName, position, rotation, group, null);
	}

	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!PhotonNetwork.connected || (PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom))
		{
			Debug.LogError("Failed to Instantiate prefab: " + prefabName + ". Client should be in a room. Current connectionStateDetailed: " + PhotonNetwork.connectionStateDetailed);
			return null;
		}
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out var value))
		{
			value = ((!prefabName.StartsWith("RCAsset/")) ? ((GameObject)Resources.Load(prefabName, typeof(GameObject))) : FengGameManagerMKII.InstantiateCustomAsset(prefabName));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, value);
			}
		}
		if (value == null)
		{
			Debug.LogError("Failed to Instantiate prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (value.GetComponent<PhotonView>() == null)
		{
			Debug.LogError("Failed to Instantiate prefab:" + prefabName + ". Prefab must have a PhotonView component.");
			return null;
		}
		int[] array = new int[value.GetPhotonViewsInChildren().Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		}
		Hashtable evData = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, isGlobalObject: false);
		return PhotonNetwork.networkingPeer.DoInstantiate2(evData, PhotonNetwork.networkingPeer.mLocalActor, value);
	}

	public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, int group, object[] data)
	{
		if (!PhotonNetwork.connected || (PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom))
		{
			Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Client should be in a room. Current connectionStateDetailed: " + PhotonNetwork.connectionStateDetailed);
			return null;
		}
		if (!PhotonNetwork.isMasterClient)
		{
			Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Client is not the MasterClient in this room.");
			return null;
		}
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out var value))
		{
			value = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, value);
			}
		}
		if (value == null)
		{
			Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (value.GetComponent<PhotonView>() == null)
		{
			Debug.LogError("Failed to InstantiateSceneObject prefab:" + prefabName + ". Prefab must have a PhotonView component.");
			return null;
		}
		int[] array = PhotonNetwork.AllocateSceneViewIDs(value.GetPhotonViewsInChildren().Length);
		if (array == null)
		{
			Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". No ViewIDs are free to use. Max is: " + PhotonNetwork.MAX_VIEW_IDS);
			return null;
		}
		Hashtable evData = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, isGlobalObject: true);
		return PhotonNetwork.networkingPeer.DoInstantiate2(evData, PhotonNetwork.networkingPeer.mLocalActor, value);
	}

	public static void InternalCleanPhotonMonoFromSceneIfStuck()
	{
		if (!(UnityEngine.Object.FindObjectsOfType(typeof(PhotonHandler)) is PhotonHandler[] array) || array.Length == 0)
		{
			return;
		}
		Debug.Log("Cleaning up hidden PhotonHandler instances in scene. Please save it. This is not an issue.");
		PhotonHandler[] array2 = array;
		foreach (PhotonHandler photonHandler in array2)
		{
			photonHandler.gameObject.hideFlags = HideFlags.None;
			if (photonHandler.gameObject != null && photonHandler.gameObject.name == "PhotonMono")
			{
				UnityEngine.Object.DestroyImmediate(photonHandler.gameObject);
			}
			UnityEngine.Object.DestroyImmediate(photonHandler);
		}
	}

	public static bool JoinLobby()
	{
		return PhotonNetwork.JoinLobby(null);
	}

	public static bool JoinLobby(TypedLobby typedLobby)
	{
		if (!PhotonNetwork.connected || PhotonNetwork.Server != 0)
		{
			return false;
		}
		if (typedLobby == null)
		{
			typedLobby = TypedLobby.Default;
		}
		bool num = PhotonNetwork.networkingPeer.OpJoinLobby(typedLobby);
		if (num)
		{
			PhotonNetwork.networkingPeer.lobby = typedLobby;
		}
		return num;
	}

	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				Debug.LogError("JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.offlineModeRoom = new Room(roomName, roomOptions);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (PhotonNetwork.networkingPeer.server != 0 || !PhotonNetwork.connectedAndReady)
		{
			Debug.LogError("JoinOrCreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			Debug.LogError("JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
			return false;
		}
		return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, roomOptions, typedLobby, createIfNotExists: true);
	}

	public static bool JoinRandomRoom()
	{
		return PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null);
	}

	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
	{
		return PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null);
	}

	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				Debug.LogError("JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.offlineModeRoom = new Room("offline room", null);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (PhotonNetwork.networkingPeer.server != 0 || !PhotonNetwork.connectedAndReady)
		{
			Debug.LogError("JoinRandomRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.MergeStringKeys(expectedCustomRoomProperties);
		if (expectedMaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = expectedMaxPlayers;
		}
		return PhotonNetwork.networkingPeer.OpJoinRandomRoom(hashtable, 0, null, matchingType, typedLobby, sqlLobbyFilter);
	}

	public static bool JoinRoom(string roomName)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				Debug.LogError("JoinRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.offlineModeRoom = new Room(roomName, null);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			return true;
		}
		if (PhotonNetwork.networkingPeer.server != 0 || !PhotonNetwork.connectedAndReady)
		{
			Debug.LogError("JoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			Debug.LogError("JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
			return false;
		}
		return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, null, null, createIfNotExists: false);
	}

	[Obsolete("Use overload with roomOptions and TypedLobby parameter.")]
	public static bool JoinRoom(string roomName, bool createIfNotExists)
	{
		if (PhotonNetwork.connectionStateDetailed == PeerStates.Joining || PhotonNetwork.connectionStateDetailed == PeerStates.Joined || PhotonNetwork.connectionStateDetailed == PeerStates.ConnectedToGameserver)
		{
			Debug.LogError("JoinRoom aborted: You can only join a room while not currently connected/connecting to a room.");
		}
		else if (PhotonNetwork.room != null)
		{
			Debug.LogError("JoinRoom aborted: You are already in a room!");
		}
		else
		{
			if (!(roomName == string.Empty))
			{
				if (PhotonNetwork.offlineMode)
				{
					PhotonNetwork.offlineModeRoom = new Room(roomName, null);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
					return true;
				}
				return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, null, null, createIfNotExists);
			}
			Debug.LogError("JoinRoom aborted: You must specifiy a room name!");
		}
		return false;
	}

	public static bool LeaveLobby()
	{
		if (PhotonNetwork.connected && PhotonNetwork.Server == ServerConnection.MasterServer)
		{
			return PhotonNetwork.networkingPeer.OpLeaveLobby();
		}
		return false;
	}

	public static bool RejoinRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, bool createIfNotExists, Hashtable Hash)
	{
		bool flag = PhotonNetwork.networkingPeer.server == ServerConnection.GameServer;
		if (!flag)
		{
			new Room(roomName, roomOptions);
			PhotonNetwork.networkingPeer.mRoomToEnterLobby = null;
			if (createIfNotExists)
			{
				PhotonNetwork.networkingPeer.mRoomToEnterLobby = ((!PhotonNetwork.networkingPeer.insideLobby) ? null : PhotonNetwork.networkingPeer.lobby);
			}
		}
		return PhotonNetwork.networkingPeer.OpJoinRoom(roomName, roomOptions, PhotonNetwork.networkingPeer.mRoomToEnterLobby, createIfNotExists, Hash, flag);
	}

	public static bool LeaveRoom()
	{
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineModeRoom = null;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
			return true;
		}
		if (PhotonNetwork.room == null)
		{
			Debug.LogWarning("PhotonNetwork.room is null. You don't have to call LeaveRoom() when you're not in one. State: " + PhotonNetwork.connectionStateDetailed);
		}
		return PhotonNetwork.networkingPeer.OpLeave();
	}

	public static void LoadLevel(int levelNumber)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelNumber);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		Application.LoadLevel(levelNumber);
	}

	public static void LoadLevel(string levelName)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelName);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		Application.LoadLevel(levelName);
	}

	public static void NetworkStatisticsReset()
	{
		PhotonNetwork.networkingPeer.TrafficStatsReset();
	}

	public static string NetworkStatisticsToString()
	{
		if (PhotonNetwork.networkingPeer != null && !PhotonNetwork.offlineMode)
		{
			return PhotonNetwork.networkingPeer.VitalStatsToString(all: false);
		}
		return "Offline or in OfflineMode. No VitalStats available.";
	}

	public static void OverrideBestCloudServer(CloudRegionCode region)
	{
		PhotonHandler.BestRegionCodeInPreferences = region;
	}

	public static bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, RaiseEventOptions options)
	{
		if (PhotonNetwork.inRoom && eventCode < byte.MaxValue)
		{
			return PhotonNetwork.networkingPeer.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
		}
		Debug.LogWarning("RaiseEvent() failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
		return false;
	}

	public static void RefreshCloudServerRating()
	{
		throw new NotImplementedException("not available at the moment");
	}

	public static void RemoveRPCs(PhotonPlayer targetPlayer)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			if (!targetPlayer.isLocal && !PhotonNetwork.isMasterClient)
			{
				Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
			}
			else
			{
				PhotonNetwork.networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
			}
		}
	}

	public static void RemoveRPCs(PhotonView targetPhotonView)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			PhotonNetwork.networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
		}
	}

	public static void RemoveRPCsInGroup(int targetGroup)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			PhotonNetwork.networkingPeer.RemoveRPCsInGroup(targetGroup);
		}
	}

	internal static void RPC(PhotonView view, string methodName, PhotonPlayer targetPlayer, params object[] parameters)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			if (PhotonNetwork.room == null)
			{
				Debug.LogWarning("Cannot send RPCs in Lobby, only processed locally");
			}
			else
			{
				if (PhotonNetwork.player == null)
				{
					Debug.LogError("Error; Sending RPC to player null! Aborted \"" + methodName + "\"");
				}
				if (PhotonNetwork.networkingPeer != null)
				{
					PhotonNetwork.networkingPeer.RPC(view, methodName, targetPlayer, parameters);
				}
				else
				{
					Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
				}
			}
		}
		PhotonNetwork.LogRPC(methodName, parameters);
	}

	internal static void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			if (PhotonNetwork.room == null)
			{
				Debug.LogWarning("Cannot send RPCs in Lobby! RPC dropped.");
			}
			else if (PhotonNetwork.networkingPeer != null)
			{
				PhotonNetwork.networkingPeer.RPC(view, methodName, target, parameters);
			}
			else
			{
				Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
			}
		}
		PhotonNetwork.LogRPC(methodName, parameters);
	}

	private static void LogRPC(string methodName, params object[] parameters)
	{
	}

	public static void SendOutgoingCommands()
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			while (PhotonNetwork.networkingPeer.SendOutgoingCommands())
			{
			}
		}
	}

	public static void SetLevelPrefix(short prefix)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			PhotonNetwork.networkingPeer.SetLevelPrefix(prefix);
		}
	}

	public static bool SetMasterClient(PhotonPlayer masterClientPlayer)
	{
		return PhotonNetwork.networkingPeer.SetMasterClient(masterClientPlayer.ID, sync: true);
	}

	public static void SetPlayerCustomProperties(Hashtable customProperties)
	{
		if (customProperties == null)
		{
			customProperties = new Hashtable();
			foreach (object key in PhotonNetwork.player.customProperties.Keys)
			{
				customProperties[(string)key] = null;
			}
		}
		if (PhotonNetwork.room != null && PhotonNetwork.room.isLocalClientInside)
		{
			PhotonNetwork.player.SetCustomProperties(customProperties);
		}
		else
		{
			PhotonNetwork.player.InternalCacheProperties(customProperties);
		}
	}

	public static void SetReceivingEnabled(int group, bool enabled)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			PhotonNetwork.networkingPeer.SetReceivingEnabled(group, enabled);
		}
	}

	public static void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			PhotonNetwork.networkingPeer.SetReceivingEnabled(enableGroups, disableGroups);
		}
	}

	public static void SetSendingEnabled(int group, bool enabled)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			PhotonNetwork.networkingPeer.SetSendingEnabled(group, enabled);
		}
	}

	public static void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (PhotonNetwork.VerifyCanUseNetwork())
		{
			PhotonNetwork.networkingPeer.SetSendingEnabled(enableGroups, disableGroups);
		}
	}

	public static void SwitchToProtocol(ConnectionProtocol cp)
	{
		if (PhotonNetwork.networkingPeer.UsedProtocol != cp)
		{
			try
			{
				PhotonNetwork.networkingPeer.Disconnect();
				PhotonNetwork.networkingPeer.StopThread();
			}
			catch
			{
			}
			PhotonNetwork.networkingPeer = new NetworkingPeer(PhotonNetwork.photonMono, string.Empty, cp);
			Debug.Log("Protocol switched to: " + cp);
		}
	}

	public static void UnAllocateViewID(int viewID)
	{
		PhotonNetwork.manuallyAllocatedViewIds.Remove(viewID);
		if (PhotonNetwork.networkingPeer.photonViewList.ContainsKey(viewID))
		{
			Debug.LogWarning($"Unallocated manually used viewID: {viewID} but found it used still in a PhotonView: {PhotonNetwork.networkingPeer.photonViewList[viewID]}");
		}
	}

	private static bool VerifyCanUseNetwork()
	{
		if (PhotonNetwork.connected)
		{
			return true;
		}
		Debug.LogError("Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
		return false;
	}

	public static bool WebRpc(string name, object parameters)
	{
		return PhotonNetwork.networkingPeer.WebRpc(name, parameters);
	}
}
