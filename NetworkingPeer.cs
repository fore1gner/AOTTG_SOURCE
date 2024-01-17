using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using Settings;
using UnityEngine;

internal class NetworkingPeer : LoadbalancingPeer, IPhotonPeerListener
{
	private HashSet<int> allowedReceivingGroups;

	private HashSet<int> blockSendingGroups;

	protected internal short currentLevelPrefix;

	protected internal const string CurrentSceneProperty = "curScn";

	private bool didAuthenticate;

	private IPhotonPeerListener externalListener;

	private string[] friendListRequested;

	private int friendListTimestamp;

	public bool hasSwitchedMC;

	public bool insideLobby;

	public Dictionary<int, GameObject> instantiatedObjects;

	private bool isFetchingFriends;

	public bool IsInitialConnect;

	protected internal bool loadingLevelAndPausedNetwork;

	public Dictionary<int, PhotonPlayer> mActors;

	protected internal string mAppId;

	protected internal string mAppVersion;

	public Dictionary<string, RoomInfo> mGameList;

	public RoomInfo[] mGameListCopy;

	private JoinType mLastJoinType;

	public PhotonPlayer mMasterClient;

	private Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache;

	public PhotonPlayer[] mOtherPlayerListCopy;

	public PhotonPlayer[] mPlayerListCopy;

	private bool mPlayernameHasToBeUpdated;

	public string NameServerAddress;

	protected internal Dictionary<int, PhotonView> photonViewList;

	private string playername;

	public static Dictionary<string, GameObject> PrefabCache;

	private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort;

	public bool requestSecurity;

	private readonly Dictionary<string, int> rpcShortcuts;

	private Dictionary<int, object[]> tempInstantiationData;

	public static bool UsePrefabCache;

	public List<Region> AvailableRegions { get; protected internal set; }

	public CloudRegionCode CloudRegion { get; protected internal set; }

	public AuthenticationValues CustomAuthenticationValues { get; set; }

	protected internal int FriendsListAge
	{
		get
		{
			if (this.isFetchingFriends || this.friendListTimestamp == 0)
			{
				return 0;
			}
			return Environment.TickCount - this.friendListTimestamp;
		}
	}

	public bool IsAuthorizeSecretAvailable => false;

	public bool IsUsingNameServer { get; protected internal set; }

	public TypedLobby lobby { get; set; }

	protected internal string mAppVersionPun => string.Format("{0}_{1}", this.mAppVersion, "1.28");

	public string MasterServerAddress { get; protected internal set; }

	public Room mCurrentGame
	{
		get
		{
			if (this.mRoomToGetInto != null && this.mRoomToGetInto.isLocalClientInside)
			{
				return this.mRoomToGetInto;
			}
			return null;
		}
	}

	public int mGameCount { get; internal set; }

	public string mGameserver { get; internal set; }

	public PhotonPlayer mLocalActor { get; internal set; }

	public int mPlayersInRoomsCount { get; internal set; }

	public int mPlayersOnMasterCount { get; internal set; }

	public int mQueuePosition { get; internal set; }

	internal RoomOptions mRoomOptionsForCreate { get; set; }

	internal TypedLobby mRoomToEnterLobby { get; set; }

	internal Room mRoomToGetInto { get; set; }

	public string PlayerName
	{
		get
		{
			return this.playername;
		}
		set
		{
			if (!string.IsNullOrEmpty(value) && !value.Equals(this.playername))
			{
				if (this.mLocalActor != null)
				{
					this.mLocalActor.name = value;
				}
				this.playername = value;
				if (this.mCurrentGame != null)
				{
					this.SendPlayerName();
				}
			}
		}
	}

	protected internal ServerConnection server { get; private set; }

	public PeerStates State { get; internal set; }

	static NetworkingPeer()
	{
		NetworkingPeer.PrefabCache = new Dictionary<string, GameObject>();
		NetworkingPeer.UsePrefabCache = true;
		NetworkingPeer.ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>
		{
			{
				ConnectionProtocol.Udp,
				5055
			},
			{
				ConnectionProtocol.Tcp,
				4530
			}
		};
	}

	public NetworkingPeer(IPhotonPeerListener listener, string playername, ConnectionProtocol connectionProtocol)
		: base(listener, connectionProtocol)
	{
		this.playername = string.Empty;
		this.mActors = new Dictionary<int, PhotonPlayer>();
		this.mOtherPlayerListCopy = new PhotonPlayer[0];
		this.mPlayerListCopy = new PhotonPlayer[0];
		this.requestSecurity = true;
		this.monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		this.instantiatedObjects = new Dictionary<int, GameObject>();
		this.allowedReceivingGroups = new HashSet<int>();
		this.blockSendingGroups = new HashSet<int>();
		this.photonViewList = new Dictionary<int, PhotonView>();
		this.NameServerAddress = "ns.exitgamescloud.com";
		this.tempInstantiationData = new Dictionary<int, object[]>();
		base.Listener = this;
		this.lobby = TypedLobby.Default;
		base.LimitOfUnreliableCommands = 40;
		this.externalListener = listener;
		this.PlayerName = playername;
		this.mLocalActor = new PhotonPlayer(isLocal: true, -1, this.playername);
		this.AddNewPlayer(this.mLocalActor.ID, this.mLocalActor);
		this.rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
		for (int i = 0; i < PhotonNetwork.PhotonServerSettings.RpcList.Count; i++)
		{
			string key = PhotonNetwork.PhotonServerSettings.RpcList[i];
			this.rpcShortcuts[key] = i;
		}
		this.State = PeerStates.PeerCreated;
		base.SerializationProtocolType = SerializationProtocol.GpBinaryV16;
	}

	private void AddNewPlayer(int ID, PhotonPlayer player)
	{
		if (!this.mActors.ContainsKey(ID))
		{
			this.mActors[ID] = player;
			this.RebuildPlayerListCopies();
		}
		else
		{
			Debug.LogError("Adding player twice: " + ID);
		}
	}

	private bool AlmostEquals(object[] lastData, object[] currentContent)
	{
		if (lastData != null || currentContent != null)
		{
			if (lastData == null || currentContent == null || lastData.Length != currentContent.Length)
			{
				return false;
			}
			for (int i = 0; i < currentContent.Length; i++)
			{
				object one = currentContent[i];
				object two = lastData[i];
				if (!this.ObjectIsSameWithInprecision(one, two))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void ChangeLocalID(int newID)
	{
		if (this.mLocalActor == null)
		{
			Debug.LogWarning($"Local actor is null or not in mActors! mLocalActor: {this.mLocalActor} mActors==null: {this.mActors == null} newID: {newID}");
		}
		if (this.mActors.ContainsKey(this.mLocalActor.ID))
		{
			this.mActors.Remove(this.mLocalActor.ID);
		}
		this.mLocalActor.InternalChangeLocalID(newID);
		this.mActors[this.mLocalActor.ID] = this.mLocalActor;
		this.RebuildPlayerListCopies();
	}

	public void checkLAN()
	{
		if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType != MultiplayerServerType.Cloud && this.MasterServerAddress != null && this.MasterServerAddress != string.Empty && this.mGameserver != null && this.mGameserver != string.Empty && this.MasterServerAddress.Contains(":") && this.mGameserver.Contains(":"))
		{
			this.mGameserver = this.MasterServerAddress.Split(':')[0] + ":" + this.mGameserver.Split(':')[1];
		}
	}

	private void CheckMasterClient(int leavingPlayerId)
	{
		bool flag = this.mMasterClient != null && this.mMasterClient.ID == leavingPlayerId;
		bool flag2 = leavingPlayerId > 0;
		if (!(!flag2 || flag))
		{
			return;
		}
		if (this.mActors.Count <= 1)
		{
			this.mMasterClient = this.mLocalActor;
		}
		else
		{
			int num = int.MaxValue;
			foreach (int key in this.mActors.Keys)
			{
				if (key < num && key != leavingPlayerId)
				{
					num = key;
				}
			}
			this.mMasterClient = this.mActors[num];
		}
		if (flag2)
		{
			object[] parameters = new object[1] { this.mMasterClient };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, parameters);
		}
	}

	private bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
	{
		if (methodParameters.Length < callParameterTypes.Length)
		{
			return false;
		}
		for (int i = 0; i < callParameterTypes.Length; i++)
		{
			Type parameterType = methodParameters[i].ParameterType;
			if (callParameterTypes[i] != null && !parameterType.Equals(callParameterTypes[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void CleanRpcBufferIfMine(PhotonView view)
	{
		if (view.ownerId != this.mLocalActor.ID && !this.mLocalActor.isMasterClient)
		{
			Debug.LogError(string.Concat("Cannot remove cached RPCs on a PhotonView thats not ours! ", view.owner, " scene: ", view.isSceneView));
		}
		else
		{
			this.OpCleanRpcBuffer(view);
		}
	}

	public bool Connect(string serverAddress, ServerConnection type)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		if (PhotonNetwork.connectionStateDetailed == PeerStates.Disconnecting)
		{
			Debug.LogError("Connect() failed. Can't connect while disconnecting (still). Current state: " + PhotonNetwork.connectionStateDetailed);
			return false;
		}
		bool flag = base.Connect(serverAddress, string.Empty);
		if (flag)
		{
			switch (type)
			{
			case ServerConnection.MasterServer:
				this.State = PeerStates.ConnectingToMasterserver;
				return flag;
			case ServerConnection.GameServer:
				this.State = PeerStates.ConnectingToGameserver;
				return flag;
			case ServerConnection.NameServer:
				this.State = PeerStates.ConnectingToNameServer;
				return flag;
			}
		}
		return flag;
	}

	public override bool Connect(string serverAddress, string applicationName)
	{
		Debug.LogError("Avoid using this directly. Thanks.");
		return false;
	}

	public bool ConnectToNameServer()
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = CloudRegionCode.none;
		if (this.State != PeerStates.ConnectedToNameServer)
		{
			string text = this.NameServerAddress;
			if (!text.Contains(":"))
			{
				int value = 0;
				NetworkingPeer.ProtocolToNameServerPort.TryGetValue(base.UsedProtocol, out value);
				text = $"{text}:{value}";
				Debug.Log("Server to connect to: " + text + " settings protocol: " + PhotonNetwork.PhotonServerSettings.Protocol);
			}
			if (!base.Connect(text, "ns"))
			{
				return false;
			}
			this.State = PeerStates.ConnectingToNameServer;
		}
		return true;
	}

	public bool ConnectToRegionMaster(CloudRegionCode region)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = region;
		if (this.State == PeerStates.ConnectedToNameServer)
		{
			return this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, region.ToString());
		}
		string text = this.NameServerAddress;
		if (!text.Contains(":"))
		{
			int value = 0;
			NetworkingPeer.ProtocolToNameServerPort.TryGetValue(base.UsedProtocol, out value);
			text = $"{text}:{value}";
		}
		if (!base.Connect(text, "ns"))
		{
			return false;
		}
		this.State = PeerStates.ConnectingToNameServer;
		return true;
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		this.externalListener.DebugReturn(level, message);
	}

	private bool DeltaCompressionRead(PhotonView view, ExitGames.Client.Photon.Hashtable data)
	{
		if (!data.ContainsKey((byte)1))
		{
			if (view.lastOnSerializeDataReceived == null)
			{
				return false;
			}
			if (!(data[(byte)2] is object[] array))
			{
				return false;
			}
			int[] array2 = data[(byte)3] as int[];
			if (array2 == null)
			{
				array2 = new int[0];
			}
			object[] lastOnSerializeDataReceived = view.lastOnSerializeDataReceived;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null && !array2.Contains(i))
				{
					array[i] = lastOnSerializeDataReceived[i];
				}
			}
			data[(byte)1] = array;
		}
		return true;
	}

	private bool DeltaCompressionWrite(PhotonView view, ExitGames.Client.Photon.Hashtable data)
	{
		if (view.lastOnSerializeDataSent != null)
		{
			object[] lastOnSerializeDataSent = view.lastOnSerializeDataSent;
			if (!(data[(byte)1] is object[] array))
			{
				return false;
			}
			if (lastOnSerializeDataSent.Length != array.Length)
			{
				return true;
			}
			object[] array2 = new object[array.Length];
			int num = 0;
			List<int> list = new List<int>();
			for (int i = 0; i < array2.Length; i++)
			{
				object obj = array[i];
				object two = lastOnSerializeDataSent[i];
				if (this.ObjectIsSameWithInprecision(obj, two))
				{
					num++;
					continue;
				}
				array2[i] = array[i];
				if (obj == null)
				{
					list.Add(i);
				}
			}
			if (num > 0)
			{
				data.Remove((byte)1);
				if (num == array.Length)
				{
					return false;
				}
				data[(byte)2] = array2;
				if (list.Count > 0)
				{
					data[(byte)3] = list.ToArray();
				}
			}
		}
		return true;
	}

	public void DestroyAll(bool localOnly)
	{
		if (!localOnly)
		{
			this.OpRemoveCompleteCache();
			this.SendDestroyOfAll();
		}
		this.LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
	}

	public void DestroyPlayerObjects(int playerId, bool localOnly)
	{
		if (playerId <= 0)
		{
			Debug.LogError("Failed to Destroy objects of playerId: " + playerId);
			return;
		}
		if (!localOnly)
		{
			this.OpRemoveFromServerInstantiationsOfPlayer(playerId);
			this.OpCleanRpcBuffer(playerId);
			this.SendDestroyOfPlayer(playerId);
		}
		Queue<GameObject> queue = new Queue<GameObject>();
		int num = playerId * PhotonNetwork.MAX_VIEW_IDS;
		int num2 = num + PhotonNetwork.MAX_VIEW_IDS;
		foreach (KeyValuePair<int, GameObject> instantiatedObject in this.instantiatedObjects)
		{
			if (instantiatedObject.Key > num && instantiatedObject.Key < num2)
			{
				queue.Enqueue(instantiatedObject.Value);
			}
		}
		foreach (GameObject item in queue)
		{
			this.RemoveInstantiatedGO(item, localOnly: true);
		}
	}

	public override void Disconnect()
	{
		if (base.PeerState == PeerStateValue.Disconnected)
		{
			if (!PhotonHandler.AppQuits)
			{
				Debug.LogWarning($"Can't execute Disconnect() while not connected. Nothing changed. State: {this.State}");
			}
		}
		else
		{
			this.State = PeerStates.Disconnecting;
			base.Disconnect();
		}
	}

	private void DisconnectToReconnect()
	{
		switch (this.server)
		{
		case ServerConnection.MasterServer:
			this.State = PeerStates.DisconnectingFromMasterserver;
			base.Disconnect();
			break;
		case ServerConnection.GameServer:
			this.State = PeerStates.DisconnectingFromGameserver;
			base.Disconnect();
			break;
		case ServerConnection.NameServer:
			this.State = PeerStates.DisconnectingFromNameServer;
			base.Disconnect();
			break;
		}
	}

	private void DisconnectToReconnect2()
	{
		this.checkLAN();
		switch (this.server)
		{
		case ServerConnection.MasterServer:
			this.State = FengGameManagerMKII.returnPeerState(2);
			base.Disconnect();
			break;
		case ServerConnection.GameServer:
			this.State = FengGameManagerMKII.returnPeerState(3);
			base.Disconnect();
			break;
		case ServerConnection.NameServer:
			this.State = FengGameManagerMKII.returnPeerState(4);
			base.Disconnect();
			break;
		}
	}

	internal GameObject DoInstantiate(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
	{
		string text = (string)evData[(byte)0];
		int timestamp = (int)evData[(byte)6];
		int num = (int)evData[(byte)7];
		Vector3 position = ((!evData.ContainsKey((byte)1)) ? Vector3.zero : ((Vector3)evData[(byte)1]));
		Quaternion rotation = Quaternion.identity;
		if (evData.ContainsKey((byte)2))
		{
			rotation = (Quaternion)evData[(byte)2];
		}
		int num2 = 0;
		if (evData.ContainsKey((byte)3))
		{
			num2 = (int)evData[(byte)3];
		}
		short prefix = 0;
		if (evData.ContainsKey((byte)8))
		{
			prefix = (short)evData[(byte)8];
		}
		int[] array = ((!evData.ContainsKey((byte)4)) ? new int[1] { num } : ((int[])evData[(byte)4]));
		if (!InstantiateTracker.instance.checkObj(text, photonPlayer, array))
		{
			return null;
		}
		object[] instantiationData = ((!evData.ContainsKey((byte)5)) ? null : ((object[])evData[(byte)5]));
		if (num2 != 0 && !this.allowedReceivingGroups.Contains(num2))
		{
			return null;
		}
		if (resourceGameObject == null)
		{
			if (!NetworkingPeer.UsePrefabCache || !NetworkingPeer.PrefabCache.TryGetValue(text, out resourceGameObject))
			{
				resourceGameObject = (GameObject)Resources.Load(text, typeof(GameObject));
				if (NetworkingPeer.UsePrefabCache)
				{
					NetworkingPeer.PrefabCache.Add(text, resourceGameObject);
				}
			}
			if (resourceGameObject == null)
			{
				Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + text + "]. Please verify you have this gameobject in a Resources folder.");
				return null;
			}
		}
		PhotonView[] photonViewsInChildren = resourceGameObject.GetPhotonViewsInChildren();
		if (photonViewsInChildren.Length != array.Length)
		{
			throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
		}
		for (int i = 0; i < array.Length; i++)
		{
			photonViewsInChildren[i].viewID = array[i];
			photonViewsInChildren[i].prefix = prefix;
			photonViewsInChildren[i].instantiationId = num;
		}
		this.StoreInstantiationData(num, instantiationData);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(resourceGameObject, position, rotation);
		for (int j = 0; j < array.Length; j++)
		{
			photonViewsInChildren[j].viewID = 0;
			photonViewsInChildren[j].prefix = -1;
			photonViewsInChildren[j].prefixBackup = -1;
			photonViewsInChildren[j].instantiationId = -1;
		}
		this.RemoveInstantiationData(num);
		if (this.instantiatedObjects.ContainsKey(num))
		{
			GameObject gameObject2 = this.instantiatedObjects[num];
			string text2 = string.Empty;
			if (gameObject2 != null)
			{
				PhotonView[] photonViewsInChildren2 = gameObject2.GetPhotonViewsInChildren();
				foreach (PhotonView photonView in photonViewsInChildren2)
				{
					if (photonView != null)
					{
						text2 = text2 + photonView.ToString() + ", ";
					}
				}
			}
			object[] args = new object[8]
			{
				gameObject,
				num,
				this.instantiatedObjects.Count,
				gameObject2,
				text2,
				PhotonNetwork.lastUsedViewSubId,
				PhotonNetwork.lastUsedViewSubIdStatic,
				this.photonViewList.Count
			};
			Debug.LogError(string.Format("DoInstantiate re-defines a GameObject. Destroying old entry! New: '{0}' (instantiationID: {1}) Old: {3}. PhotonViews on old: {4}. instantiatedObjects.Count: {2}. PhotonNetwork.lastUsedViewSubId: {5} PhotonNetwork.lastUsedViewSubIdStatic: {6} this.photonViewList.Count {7}.)", args));
			this.RemoveInstantiatedGO(gameObject2, localOnly: true);
		}
		this.instantiatedObjects.Add(num, gameObject);
		gameObject.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), new PhotonMessageInfo(photonPlayer, timestamp, null), SendMessageOptions.DontRequireReceiver);
		return gameObject;
	}

	internal GameObject DoInstantiate2(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
	{
		string text = (string)evData[(byte)0];
		int timestamp = (int)evData[(byte)6];
		int num = (int)evData[(byte)7];
		Vector3 position = ((!evData.ContainsKey((byte)1)) ? Vector3.zero : ((Vector3)evData[(byte)1]));
		Quaternion rotation = Quaternion.identity;
		if (evData.ContainsKey((byte)2))
		{
			rotation = (Quaternion)evData[(byte)2];
		}
		int num2 = 0;
		if (evData.ContainsKey((byte)3))
		{
			num2 = (int)evData[(byte)3];
		}
		short prefix = 0;
		if (evData.ContainsKey((byte)8))
		{
			prefix = (short)evData[(byte)8];
		}
		int[] array = ((!evData.ContainsKey((byte)4)) ? new int[1] { num } : ((int[])evData[(byte)4]));
		if (!InstantiateTracker.instance.checkObj(text, photonPlayer, array))
		{
			return null;
		}
		object[] instantiationData = ((!evData.ContainsKey((byte)5)) ? null : ((object[])evData[(byte)5]));
		if (num2 != 0 && !this.allowedReceivingGroups.Contains(num2))
		{
			return null;
		}
		if (resourceGameObject == null)
		{
			if (!NetworkingPeer.UsePrefabCache || !NetworkingPeer.PrefabCache.TryGetValue(text, out resourceGameObject))
			{
				resourceGameObject = ((!text.StartsWith("RCAsset/")) ? ((GameObject)Resources.Load(text, typeof(GameObject))) : FengGameManagerMKII.InstantiateCustomAsset(text));
				if (NetworkingPeer.UsePrefabCache)
				{
					NetworkingPeer.PrefabCache.Add(text, resourceGameObject);
				}
			}
			if (resourceGameObject == null)
			{
				Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + text + "]. Please verify you have this gameobject in a Resources folder.");
				return null;
			}
		}
		PhotonView[] photonViewsInChildren = resourceGameObject.GetPhotonViewsInChildren();
		if (photonViewsInChildren.Length != array.Length)
		{
			throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
		}
		for (int i = 0; i < array.Length; i++)
		{
			photonViewsInChildren[i].viewID = array[i];
			photonViewsInChildren[i].prefix = prefix;
			photonViewsInChildren[i].instantiationId = num;
		}
		this.StoreInstantiationData(num, instantiationData);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(resourceGameObject, position, rotation);
		for (int j = 0; j < array.Length; j++)
		{
			photonViewsInChildren[j].viewID = 0;
			photonViewsInChildren[j].prefix = -1;
			photonViewsInChildren[j].prefixBackup = -1;
			photonViewsInChildren[j].instantiationId = -1;
		}
		this.RemoveInstantiationData(num);
		if (this.instantiatedObjects.ContainsKey(num))
		{
			GameObject gameObject2 = this.instantiatedObjects[num];
			string text2 = string.Empty;
			if (gameObject2 != null)
			{
				PhotonView[] photonViewsInChildren2 = gameObject2.GetPhotonViewsInChildren();
				foreach (PhotonView photonView in photonViewsInChildren2)
				{
					if (photonView != null)
					{
						text2 = text2 + photonView.ToString() + ", ";
					}
				}
			}
			object[] args = new object[8]
			{
				gameObject,
				num,
				this.instantiatedObjects.Count,
				gameObject2,
				text2,
				PhotonNetwork.lastUsedViewSubId,
				PhotonNetwork.lastUsedViewSubIdStatic,
				this.photonViewList.Count
			};
			Debug.LogError(string.Format("DoInstantiate re-defines a GameObject. Destroying old entry! New: '{0}' (instantiationID: {1}) Old: {3}. PhotonViews on old: {4}. instantiatedObjects.Count: {2}. PhotonNetwork.lastUsedViewSubId: {5} PhotonNetwork.lastUsedViewSubIdStatic: {6} this.photonViewList.Count {7}.)", args));
			this.RemoveInstantiatedGO(gameObject2, localOnly: true);
		}
		this.instantiatedObjects.Add(num, gameObject);
		gameObject.SendMessage(PhotonNetworkingMessage.OnPhotonInstantiate.ToString(), new PhotonMessageInfo(photonPlayer, timestamp, null), SendMessageOptions.DontRequireReceiver);
		return gameObject;
	}

	public void ExecuteRPC(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
	{
		if (rpcData == null || !rpcData.ContainsKey((byte)0))
		{
			Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
			return;
		}
		int num = (int)rpcData[(byte)0];
		int num2 = 0;
		if (rpcData.ContainsKey((byte)1))
		{
			num2 = (short)rpcData[(byte)1];
		}
		string text;
		if (rpcData.ContainsKey((byte)5))
		{
			int num3 = (byte)rpcData[(byte)5];
			if (num3 > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
			{
				Debug.LogError("Could not find RPC with index: " + num3 + ". Going to ignore! Check PhotonServerSettings.RpcList");
				return;
			}
			text = PhotonNetwork.PhotonServerSettings.RpcList[num3];
		}
		else
		{
			text = (string)rpcData[(byte)3];
		}
		object[] array = null;
		if (rpcData.ContainsKey((byte)4))
		{
			array = (object[])rpcData[(byte)4];
		}
		if (array == null)
		{
			array = new object[0];
		}
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			int num4 = num / PhotonNetwork.MAX_VIEW_IDS;
			bool flag = num4 == this.mLocalActor.ID;
			bool flag2 = num4 == sender.ID;
			if (flag)
			{
				Debug.LogWarning("Received RPC \"" + text + "\" for viewID " + num + " but this PhotonView does not exist! View was/is ours." + ((!flag2) ? " Remote called." : " Owner called."));
			}
			else
			{
				Debug.LogError("Received RPC \"" + text + "\" for viewID " + num + " but this PhotonView does not exist! Was remote PV." + ((!flag2) ? " Remote called." : " Owner called."));
			}
			return;
		}
		if (photonView.prefix != num2)
		{
			Debug.LogError("Received RPC \"" + text + "\" on viewID " + num + " with a prefix of " + num2 + ", our prefix is " + photonView.prefix + ". The RPC has been ignored.");
			return;
		}
		if (text == string.Empty)
		{
			Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
			return;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Received RPC: " + text);
		}
		if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
		{
			return;
		}
		Type[] array2 = new Type[0];
		if (array.Length != 0)
		{
			array2 = new Type[array.Length];
			int num5 = 0;
			foreach (object obj in array)
			{
				if (obj == null)
				{
					array2[num5] = null;
				}
				else
				{
					array2[num5] = obj.GetType();
				}
				num5++;
			}
		}
		int num6 = 0;
		int num7 = 0;
		MonoBehaviour[] components = photonView.GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in components)
		{
			if (monoBehaviour == null)
			{
				Debug.LogError("ERROR You have missing MonoBehaviours on your gameobjects!");
				continue;
			}
			Type type = monoBehaviour.GetType();
			List<MethodInfo> list = null;
			if (this.monoRPCMethodsCache.ContainsKey(type))
			{
				list = this.monoRPCMethodsCache[type];
			}
			if (list == null)
			{
				List<MethodInfo> methods = SupportClass.GetMethods(type, typeof(RPC));
				this.monoRPCMethodsCache[type] = methods;
				list = methods;
			}
			if (list == null)
			{
				continue;
			}
			for (int k = 0; k < list.Count; k++)
			{
				MethodInfo methodInfo = list[k];
				if (!(methodInfo.Name == text))
				{
					continue;
				}
				num7++;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length == array2.Length)
				{
					if (this.CheckTypeMatch(parameters, array2))
					{
						num6++;
						object obj2 = methodInfo.Invoke(monoBehaviour, array);
						if (methodInfo.ReturnType == typeof(IEnumerator))
						{
							monoBehaviour.StartCoroutine((IEnumerator)obj2);
						}
					}
				}
				else if (parameters.Length - 1 == array2.Length)
				{
					if (this.CheckTypeMatch(parameters, array2) && parameters[parameters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
					{
						num6++;
						int timestamp = (int)rpcData[(byte)2];
						object[] array3 = new object[array.Length + 1];
						array.CopyTo(array3, 0);
						array3[array3.Length - 1] = new PhotonMessageInfo(sender, timestamp, photonView);
						object obj3 = methodInfo.Invoke(monoBehaviour, array3);
						if (methodInfo.ReturnType == typeof(IEnumerator))
						{
							monoBehaviour.StartCoroutine((IEnumerator)obj3);
						}
					}
				}
				else if (parameters.Length == 1 && parameters[0].ParameterType.IsArray)
				{
					num6++;
					object[] parameters2 = new object[1] { array };
					object obj4 = methodInfo.Invoke(monoBehaviour, parameters2);
					if (methodInfo.ReturnType == typeof(IEnumerator))
					{
						monoBehaviour.StartCoroutine((IEnumerator)obj4);
					}
				}
			}
		}
		if (num6 == 1)
		{
			return;
		}
		string text2 = string.Empty;
		foreach (Type type2 in array2)
		{
			if (text2 != string.Empty)
			{
				text2 += ", ";
			}
			text2 = ((type2 != null) ? (text2 + type2.Name) : (text2 + "null"));
		}
		if (num6 == 0)
		{
			if (num7 == 0)
			{
				Debug.LogError("PhotonView with ID " + num + " has no method \"" + text + "\" marked with the [RPC](C#) or @RPC(JS) property! Args: " + text2);
			}
			else
			{
				Debug.LogError("PhotonView with ID " + num + " has no method \"" + text + "\" that takes " + array2.Length + " argument(s): " + text2);
			}
		}
		else
		{
			Debug.LogError("PhotonView with ID " + num + " has " + num6 + " methods \"" + text + "\" that takes " + array2.Length + " argument(s): " + text2 + ". Should be just one?");
		}
	}

	public object[] FetchInstantiationData(int instantiationId)
	{
		object[] value = null;
		if (instantiationId == 0)
		{
			return null;
		}
		this.tempInstantiationData.TryGetValue(instantiationId, out value);
		return value;
	}

	private void GameEnteredOnGameServer(OperationResponse operationResponse)
	{
		if (operationResponse.ReturnCode == 0)
		{
			this.State = PeerStates.Joined;
			this.mRoomToGetInto.isLocalClientInside = true;
			ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
			ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
			this.ReadoutProperties(gameProperties, pActorProperties, 0);
			int newID = (int)operationResponse[254];
			this.ChangeLocalID(newID);
			this.CheckMasterClient(-1);
			if (this.mPlayernameHasToBeUpdated)
			{
				this.SendPlayerName();
			}
			if (operationResponse.OperationCode == 227)
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			}
			return;
		}
		switch (operationResponse.OperationCode)
		{
		case 225:
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
				if (operationResponse.ReturnCode == 32758)
				{
					Debug.Log("Most likely the game became empty during the switch to GameServer.");
				}
			}
			object[] parameters2 = new object[2] { operationResponse.ReturnCode, operationResponse.DebugMessage };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, parameters2);
			break;
		}
		case 226:
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
				if (operationResponse.ReturnCode == 32758)
				{
					Debug.Log("Most likely the game became empty during the switch to GameServer.");
				}
			}
			object[] parameters3 = new object[2] { operationResponse.ReturnCode, operationResponse.DebugMessage };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, parameters3);
			break;
		}
		case 227:
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("Create failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
			}
			object[] parameters = new object[2] { operationResponse.ReturnCode, operationResponse.DebugMessage };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, parameters);
			break;
		}
		}
		this.DisconnectToReconnect2();
	}

	private ExitGames.Client.Photon.Hashtable GetActorPropertiesForActorNr(ExitGames.Client.Photon.Hashtable actorProperties, int actorNr)
	{
		if (actorProperties.ContainsKey(actorNr))
		{
			return (ExitGames.Client.Photon.Hashtable)actorProperties[actorNr];
		}
		return actorProperties;
	}

	public int GetInstantiatedObjectsId(GameObject go)
	{
		int result = -1;
		if (go == null)
		{
			Debug.LogError("GetInstantiatedObjectsId() for GO == null.");
			return result;
		}
		PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
		if (photonViewsInChildren != null && photonViewsInChildren.Length != 0 && photonViewsInChildren[0] != null)
		{
			return photonViewsInChildren[0].instantiationId;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log("GetInstantiatedObjectsId failed for GO: " + go);
		}
		return result;
	}

	private ExitGames.Client.Photon.Hashtable GetLocalActorProperties()
	{
		if (PhotonNetwork.player != null)
		{
			return PhotonNetwork.player.allProperties;
		}
		return new ExitGames.Client.Photon.Hashtable { [byte.MaxValue] = this.PlayerName };
	}

	protected internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
	{
		mi = null;
		if (monob != null && !string.IsNullOrEmpty(methodType))
		{
			List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
			for (int i = 0; i < methods.Count; i++)
			{
				MethodInfo methodInfo = methods[i];
				if (methodInfo.Name.Equals(methodType))
				{
					mi = methodInfo;
					return true;
				}
			}
		}
		return false;
	}

	public PhotonView GetPhotonView(int viewID)
	{
		PhotonView value = null;
		this.photonViewList.TryGetValue(viewID, out value);
		if (value == null)
		{
			PhotonView[] array = UnityEngine.Object.FindObjectsOfType(typeof(PhotonView)) as PhotonView[];
			foreach (PhotonView photonView in array)
			{
				if (photonView.viewID == viewID)
				{
					if (photonView.didAwake)
					{
						Debug.LogWarning("Had to lookup view that wasn't in dict: " + photonView);
					}
					return photonView;
				}
			}
		}
		return value;
	}

	private PhotonPlayer GetPlayerWithID(int number)
	{
		if (this.mActors != null && this.mActors.ContainsKey(number))
		{
			return this.mActors[number];
		}
		return null;
	}

	public bool GetRegions()
	{
		if (this.server != ServerConnection.NameServer)
		{
			return false;
		}
		bool num = this.OpGetRegions(this.mAppId);
		if (num)
		{
			this.AvailableRegions = null;
		}
		return num;
	}

	private void HandleEventLeave(int actorID)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log("HandleEventLeave for player ID: " + actorID);
		}
		if (actorID < 0 || !this.mActors.ContainsKey(actorID))
		{
			Debug.LogError($"Received event Leave for unknown player ID: {actorID}");
		}
		else if (PhotonNetwork.player.ID != actorID)
		{
			PhotonPlayer playerWithID = this.GetPlayerWithID(actorID);
			if (playerWithID == null)
			{
				Debug.LogError("HandleEventLeave for player ID: " + actorID + " has no PhotonPlayer!");
			}
			this.CheckMasterClient(actorID);
			if (this.mCurrentGame != null)
			{
				this.DestroyPlayerObjects(actorID, localOnly: true);
			}
			this.RemovePlayer(actorID, playerWithID);
			object[] parameters = new object[1] { playerWithID };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, parameters);
		}
	}

	private void LeftLobbyCleanup()
	{
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		if (this.insideLobby)
		{
			this.insideLobby = false;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby);
		}
	}

	private void LeftRoomCleanup()
	{
		bool num = this.mRoomToGetInto != null;
		bool num2 = ((this.mRoomToGetInto == null) ? PhotonNetwork.autoCleanUpPlayerObjects : this.mRoomToGetInto.autoCleanUp);
		this.hasSwitchedMC = false;
		this.mRoomToGetInto = null;
		this.mActors = new Dictionary<int, PhotonPlayer>();
		this.mPlayerListCopy = new PhotonPlayer[0];
		this.mOtherPlayerListCopy = new PhotonPlayer[0];
		this.mMasterClient = null;
		this.allowedReceivingGroups = new HashSet<int>();
		this.blockSendingGroups = new HashSet<int>();
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		this.isFetchingFriends = false;
		this.ChangeLocalID(-1);
		if (num2)
		{
			this.LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
			PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		}
		if (num)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom);
		}
	}

	protected internal void LoadLevelIfSynced()
	{
		if (!PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient || PhotonNetwork.room == null || !PhotonNetwork.room.customProperties.ContainsKey("curScn"))
		{
			return;
		}
		object obj = PhotonNetwork.room.customProperties["curScn"];
		if (obj is int)
		{
			if (Application.loadedLevel != (int)obj)
			{
				PhotonNetwork.LoadLevel((int)obj);
			}
		}
		else if (obj is string && Application.loadedLevelName != (string)obj)
		{
			PhotonNetwork.LoadLevel((string)obj);
		}
	}

	public void LocalCleanPhotonView(PhotonView view)
	{
		view.destroyedByPhotonNetworkOrQuit = true;
		this.photonViewList.Remove(view.viewID);
	}

	protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
	{
		if (this.tempInstantiationData.Count > 0)
		{
			Debug.LogWarning("It seems some instantiation is not completed, as instantiation data is used. You should make sure instantiations are paused when calling this method. Cleaning now, despite this.");
		}
		if (destroyInstantiatedGameObjects)
		{
			foreach (GameObject item in new HashSet<GameObject>(this.instantiatedObjects.Values))
			{
				this.RemoveInstantiatedGO(item, localOnly: true);
			}
		}
		this.tempInstantiationData.Clear();
		this.instantiatedObjects = new Dictionary<int, GameObject>();
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
	}

	public void NewSceneLoaded()
	{
		if (this.loadingLevelAndPausedNetwork)
		{
			this.loadingLevelAndPausedNetwork = false;
			PhotonNetwork.isMessageQueueRunning = true;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, PhotonView> photonView in this.photonViewList)
		{
			if (photonView.Value == null)
			{
				list.Add(photonView.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			int key = list[i];
			this.photonViewList.Remove(key);
		}
		if (list.Count > 0 && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log("New level loaded. Removed " + list.Count + " scene view IDs from last level.");
		}
	}

	private bool ObjectIsSameWithInprecision(object one, object two)
	{
		if (one == null || two == null)
		{
			if (one == null)
			{
				return two == null;
			}
			return false;
		}
		if (one.Equals(two))
		{
			return true;
		}
		if (one is object target)
		{
			Vector3 second = (Vector3)two;
			if (((Vector3)target).AlmostEquals(second, PhotonNetwork.precisionForVectorSynchronization))
			{
				return true;
			}
		}
		else if (one is object target2)
		{
			Vector2 second2 = (Vector2)two;
			if (((Vector2)target2).AlmostEquals(second2, PhotonNetwork.precisionForVectorSynchronization))
			{
				return true;
			}
		}
		else if (one is object target3)
		{
			Quaternion second3 = (Quaternion)two;
			if (((Quaternion)target3).AlmostEquals(second3, PhotonNetwork.precisionForQuaternionSynchronization))
			{
				return true;
			}
		}
		else if (one is float target4)
		{
			float second4 = (float)two;
			if (target4.AlmostEquals(second4, PhotonNetwork.precisionForFloatSynchronization))
			{
				return true;
			}
		}
		return false;
	}

	public void OnEvent(EventData photonEvent)
	{
		int num = -1;
		PhotonPlayer photonPlayer = null;
		if (photonEvent.Parameters.ContainsKey(254))
		{
			num = (int)photonEvent[254];
			if (this.mActors.ContainsKey(num))
			{
				photonPlayer = this.mActors[num];
			}
		}
		else if (photonEvent.Parameters.Count == 0)
		{
			return;
		}
		switch (photonEvent.Code)
		{
		case 200:
			if (photonPlayer == null || !FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				this.ExecuteRPC(photonEvent[245] as ExitGames.Client.Photon.Hashtable, photonPlayer);
				break;
			}
			return;
		case 201:
		case 206:
		{
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			object obj7 = photonEvent[245];
			if (obj7 == null || !(obj7 is ExitGames.Client.Photon.Hashtable))
			{
				break;
			}
			ExitGames.Client.Photon.Hashtable hashtable4 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			if (!(hashtable4[(byte)0] is int))
			{
				return;
			}
			int networkTime = (int)hashtable4[(byte)0];
			short correctPrefix = -1;
			short num5 = 1;
			if (hashtable4.ContainsKey((byte)1))
			{
				if (!(hashtable4[(byte)1] is short))
				{
					return;
				}
				correctPrefix = (short)hashtable4[(byte)1];
				num5 = 2;
			}
			for (short num6 = num5; num6 < hashtable4.Count; num6++)
			{
				this.OnSerializeRead(hashtable4[num6] as ExitGames.Client.Photon.Hashtable, photonPlayer, networkTime, correctPrefix);
			}
			break;
		}
		case 202:
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			if (photonEvent[245] is ExitGames.Client.Photon.Hashtable)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
				if (hashtable2[(byte)0] is string && (string)hashtable2[(byte)0] != null)
				{
					this.DoInstantiate2(hashtable2, photonPlayer, null);
				}
			}
			break;
		case 203:
			if (photonPlayer != null && photonPlayer.isMasterClient && !photonPlayer.isLocal)
			{
				PhotonNetwork.LeaveRoom();
			}
			break;
		case 204:
		{
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				break;
			}
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			if (hashtable[(byte)0] is int)
			{
				int key = (int)hashtable[(byte)0];
				GameObject value = null;
				this.instantiatedObjects.TryGetValue(key, out value);
				if (value != null && photonPlayer != null)
				{
					this.RemoveInstantiatedGO(value, localOnly: true);
				}
			}
			break;
		}
		case 207:
		{
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				break;
			}
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			if (hashtable[(byte)0] is int)
			{
				int num3 = (int)hashtable[(byte)0];
				if (num3 < 0 && photonPlayer != null && (photonPlayer.isMasterClient || photonPlayer.isLocal))
				{
					this.DestroyAll(localOnly: true);
				}
				else if (photonPlayer != null && (photonPlayer.isMasterClient || photonPlayer.isLocal || num3 != PhotonNetwork.player.ID))
				{
					this.DestroyPlayerObjects(num3, localOnly: true);
				}
			}
			break;
		}
		case 208:
		{
			if (!(photonEvent[245] is ExitGames.Client.Photon.Hashtable))
			{
				break;
			}
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			if (!(hashtable[(byte)1] is int))
			{
				break;
			}
			int num7 = (int)hashtable[(byte)1];
			if (photonPlayer != null && photonPlayer.isMasterClient && num7 == photonPlayer.ID)
			{
				return;
			}
			if (photonPlayer != null && !photonPlayer.isMasterClient && !photonPlayer.isLocal)
			{
				if (PhotonNetwork.isMasterClient)
				{
					FengGameManagerMKII.noRestart = true;
					PhotonNetwork.SetMasterClient(PhotonNetwork.player);
					FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "stealing MC.");
				}
				return;
			}
			if (num7 == this.mLocalActor.ID)
			{
				this.SetMasterClient(num7, sync: false);
			}
			else if (photonPlayer == null || photonPlayer.isMasterClient || photonPlayer.isLocal)
			{
				this.SetMasterClient(num7, sync: false);
			}
			break;
		}
		case 226:
			if (photonPlayer == null || !FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				object obj = photonEvent[229];
				object obj2 = photonEvent[227];
				object obj3 = photonEvent[228];
				if (obj is int && obj2 is int && obj3 is int)
				{
					this.mPlayersInRoomsCount = (int)obj;
					this.mPlayersOnMasterCount = (int)obj2;
					this.mGameCount = (int)obj3;
				}
				break;
			}
			return;
		case 228:
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			if (photonEvent.Parameters.ContainsKey(223))
			{
				object obj6 = photonEvent[223];
				if (obj6 is int)
				{
					this.mQueuePosition = (int)obj6;
				}
			}
			if (this.mQueuePosition == 0)
			{
				if (PhotonNetwork.autoJoinLobby)
				{
					this.State = FengGameManagerMKII.returnPeerState(0);
					this.OpJoinLobby(this.lobby);
				}
				else
				{
					this.State = FengGameManagerMKII.returnPeerState(1);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
				}
			}
			break;
		case 229:
		{
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			object obj6 = photonEvent[222];
			if (!(obj6 is ExitGames.Client.Photon.Hashtable))
			{
				break;
			}
			foreach (DictionaryEntry item in (ExitGames.Client.Photon.Hashtable)obj6)
			{
				string text = (string)item.Key;
				RoomInfo roomInfo = new RoomInfo(text, (ExitGames.Client.Photon.Hashtable)item.Value);
				if (roomInfo.removedFromList)
				{
					this.mGameList.Remove(text);
				}
				else
				{
					this.mGameList[text] = roomInfo;
				}
			}
			this.mGameListCopy = new RoomInfo[this.mGameList.Count];
			this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
			break;
		}
		case 230:
		{
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			object obj6 = photonEvent[222];
			if (!(obj6 is ExitGames.Client.Photon.Hashtable))
			{
				break;
			}
			this.mGameList = new Dictionary<string, RoomInfo>();
			foreach (DictionaryEntry item2 in (ExitGames.Client.Photon.Hashtable)obj6)
			{
				string text2 = (string)item2.Key;
				this.mGameList[text2] = new RoomInfo(text2, (ExitGames.Client.Photon.Hashtable)item2.Value);
			}
			this.mGameListCopy = new RoomInfo[this.mGameList.Count];
			this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate);
			break;
		}
		case 253:
		{
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			if (!(photonEvent[253] is int num4))
			{
				break;
			}
			ExitGames.Client.Photon.Hashtable gameProperties = null;
			ExitGames.Client.Photon.Hashtable hashtable3 = null;
			if (num4 != 0)
			{
				object obj5 = photonEvent[251];
				if (obj5 is ExitGames.Client.Photon.Hashtable)
				{
					hashtable3 = (ExitGames.Client.Photon.Hashtable)obj5;
					if (photonPlayer != null)
					{
						hashtable3["sender"] = photonPlayer;
						if (PhotonNetwork.isMasterClient && !photonPlayer.isLocal && num4 == photonPlayer.ID && (hashtable3.ContainsKey("statACL") || hashtable3.ContainsKey("statBLA") || hashtable3.ContainsKey("statGAS") || hashtable3.ContainsKey("statSPD")))
						{
							if (hashtable3.ContainsKey("statACL") && RCextensions.returnIntFromObject(hashtable3["statACL"]) > 150)
							{
								FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "excessive stats.");
								return;
							}
							if (hashtable3.ContainsKey("statBLA") && RCextensions.returnIntFromObject(hashtable3["statBLA"]) > 125)
							{
								FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "excessive stats.");
								return;
							}
							if (hashtable3.ContainsKey("statGAS") && RCextensions.returnIntFromObject(hashtable3["statGAS"]) > 150)
							{
								FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "excessive stats.");
								return;
							}
							if (hashtable3.ContainsKey("statSPD") && RCextensions.returnIntFromObject(hashtable3["statSPD"]) > 140)
							{
								FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "excessive stats.");
								return;
							}
						}
						if (hashtable3.ContainsKey("name"))
						{
							if (num4 != photonPlayer.ID)
							{
								InstantiateTracker.instance.resetPropertyTracking(num4);
							}
							else if (!InstantiateTracker.instance.PropertiesChanged(photonPlayer))
							{
								return;
							}
						}
					}
				}
			}
			else
			{
				object obj5 = photonEvent[251];
				if (obj5 == null || !(obj5 is ExitGames.Client.Photon.Hashtable))
				{
					return;
				}
				gameProperties = (ExitGames.Client.Photon.Hashtable)obj5;
			}
			this.ReadoutProperties(gameProperties, hashtable3, num4);
			break;
		}
		case 254:
			this.HandleEventLeave(num);
			break;
		case byte.MaxValue:
		{
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			object obj4 = photonEvent[249];
			if (obj4 != null && !(obj4 is ExitGames.Client.Photon.Hashtable))
			{
				break;
			}
			ExitGames.Client.Photon.Hashtable properties = (ExitGames.Client.Photon.Hashtable)obj4;
			if (photonPlayer == null)
			{
				bool isLocal = this.mLocalActor.ID == num;
				this.AddNewPlayer(num, new PhotonPlayer(isLocal, num, properties));
				this.ResetPhotonViewsOnSerialize();
			}
			if (num != this.mLocalActor.ID)
			{
				object[] parameters = new object[1] { this.mActors[num] };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, parameters);
				break;
			}
			object obj5 = photonEvent[252];
			if (!(obj5 is int[]))
			{
				break;
			}
			int[] array = (int[])obj5;
			foreach (int num2 in array)
			{
				if (this.mLocalActor.ID != num2 && !this.mActors.ContainsKey(num2))
				{
					this.AddNewPlayer(num2, new PhotonPlayer(isLocal: false, num2, string.Empty));
				}
			}
			if (this.mLastJoinType == JoinType.JoinOrCreateOnDemand && this.mLocalActor.ID == 1)
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom);
			}
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom);
			break;
		}
		default:
			if (photonPlayer != null && FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
			{
				return;
			}
			if (photonEvent.Code < 200 && PhotonNetwork.OnEventCall != null)
			{
				object content = photonEvent[245];
				PhotonNetwork.OnEventCall(photonEvent.Code, content, num);
			}
			break;
		}
		this.externalListener.OnEvent(photonEvent);
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
		if (PhotonNetwork.networkingPeer.State == PeerStates.Disconnecting)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("OperationResponse ignored while disconnecting. Code: " + operationResponse.OperationCode);
			}
			return;
		}
		if (operationResponse.ReturnCode == 0)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(operationResponse.ToString());
			}
		}
		else if (operationResponse.ReturnCode == -3)
		{
			Debug.LogError("Operation " + operationResponse.OperationCode + " could not be executed (yet). Wait for state JoinedLobby or ConnectedToMaster and their callbacks before calling operations. WebRPCs need a server-side configuration. Enum OperationCode helps identify the operation.");
		}
		else if (operationResponse.ReturnCode == 32752)
		{
			Debug.LogError("Operation " + operationResponse.OperationCode + " failed in a server-side plugin. Check the configuration in the Dashboard. Message from server-plugin: " + operationResponse.DebugMessage);
		}
		else if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.LogError("Operation failed: " + operationResponse.ToStringFull() + " Server: " + this.server);
		}
		if (operationResponse.Parameters.ContainsKey(221))
		{
			if (this.CustomAuthenticationValues == null)
			{
				this.CustomAuthenticationValues = new AuthenticationValues();
			}
			this.CustomAuthenticationValues.Secret = operationResponse[221] as string;
		}
		switch (operationResponse.OperationCode)
		{
		case 219:
		{
			object[] parameters6 = new object[1] { operationResponse };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnWebRpcResponse, parameters6);
			break;
		}
		case 220:
		{
			if (operationResponse.ReturnCode != short.MaxValue)
			{
				string[] array3 = operationResponse[210] as string[];
				string[] array4 = operationResponse[230] as string[];
				if (array3 == null || array4 == null || array3.Length != array4.Length)
				{
					Debug.LogError("The region arrays from Name Server are not ok. Must be non-null and same length.");
					break;
				}
				this.AvailableRegions = new List<Region>(array3.Length);
				for (int j = 0; j < array3.Length; j++)
				{
					string text2 = array3[j];
					if (!string.IsNullOrEmpty(text2))
					{
						CloudRegionCode code = Region.Parse(text2.ToLower());
						Region item2 = new Region
						{
							Code = code,
							HostAndPort = array4[j]
						};
						this.AvailableRegions.Add(item2);
					}
				}
				break;
			}
			Debug.LogError($"The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.");
			object[] parameters7 = new object[1] { DisconnectCause.InvalidAuthentication };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters7);
			this.State = PeerStates.Disconnecting;
			this.Disconnect();
			return;
		}
		case 222:
		{
			bool[] array = operationResponse[1] as bool[];
			string[] array2 = operationResponse[2] as string[];
			if (array == null || array2 == null || this.friendListRequested == null || array.Length != this.friendListRequested.Length)
			{
				Debug.LogError("FindFriends failed to apply the result, as a required value wasn't provided or the friend list length differed from result.");
			}
			else
			{
				List<FriendInfo> list = new List<FriendInfo>(this.friendListRequested.Length);
				for (int i = 0; i < this.friendListRequested.Length; i++)
				{
					FriendInfo item = new FriendInfo
					{
						Name = this.friendListRequested[i],
						Room = array2[i],
						IsOnline = array[i]
					};
					list.Insert(i, item);
				}
				PhotonNetwork.Friends = list;
			}
			this.friendListRequested = null;
			this.isFetchingFriends = false;
			this.friendListTimestamp = Environment.TickCount;
			if (this.friendListTimestamp == 0)
			{
				this.friendListTimestamp = 1;
			}
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList);
			break;
		}
		case 225:
			if (operationResponse.ReturnCode == 0)
			{
				string name = (string)operationResponse[byte.MaxValue];
				this.mRoomToGetInto.name = name;
				this.mGameserver = (string)operationResponse[230];
				this.DisconnectToReconnect2();
				break;
			}
			if (operationResponse.ReturnCode != 32760)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogWarning($"JoinRandom failed: {operationResponse.ToStringFull()}.");
				}
			}
			else if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
			{
				Debug.Log("JoinRandom failed: No open game. Calling: OnPhotonRandomJoinFailed() and staying on master server.");
			}
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed);
			break;
		case 226:
			if (this.server == ServerConnection.GameServer)
			{
				this.GameEnteredOnGameServer(operationResponse);
				break;
			}
			if (operationResponse.ReturnCode == 0)
			{
				this.mGameserver = (string)operationResponse[230];
				this.DisconnectToReconnect2();
				break;
			}
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log($"JoinRoom failed (room maybe closed by now). Client stays on masterserver: {operationResponse.ToStringFull()}. State: {this.State}");
			}
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed);
			break;
		case 227:
			if (this.server != ServerConnection.GameServer)
			{
				if (operationResponse.ReturnCode != 0)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogWarning($"CreateRoom failed, client stays on masterserver: {operationResponse.ToStringFull()}.");
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed);
					break;
				}
				string text = (string)operationResponse[byte.MaxValue];
				if (!string.IsNullOrEmpty(text))
				{
					this.mRoomToGetInto.name = text;
				}
				this.mGameserver = (string)operationResponse[230];
				this.DisconnectToReconnect2();
			}
			else
			{
				this.GameEnteredOnGameServer(operationResponse);
			}
			break;
		case 228:
			this.State = PeerStates.Authenticated;
			this.LeftLobbyCleanup();
			break;
		case 229:
			this.State = PeerStates.JoinedLobby;
			this.insideLobby = true;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby);
			break;
		case 230:
			if (operationResponse.ReturnCode == 0)
			{
				if (this.server == ServerConnection.NameServer)
				{
					this.MasterServerAddress = operationResponse[230] as string;
					this.DisconnectToReconnect2();
				}
				else if (this.server == ServerConnection.MasterServer)
				{
					if (PhotonNetwork.autoJoinLobby)
					{
						this.State = PeerStates.Authenticated;
						this.OpJoinLobby(this.lobby);
					}
					else
					{
						this.State = PeerStates.ConnectedToMaster;
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster);
					}
				}
				else if (this.server == ServerConnection.GameServer)
				{
					this.State = PeerStates.Joining;
					if (this.mLastJoinType == JoinType.JoinGame || this.mLastJoinType == JoinType.JoinRandomGame || this.mLastJoinType == JoinType.JoinOrCreateOnDemand)
					{
						this.OpJoinRoom(this.mRoomToGetInto.name, this.mRoomOptionsForCreate, this.mRoomToEnterLobby, this.mLastJoinType == JoinType.JoinOrCreateOnDemand);
					}
					else if (this.mLastJoinType == JoinType.CreateGame)
					{
						this.OpCreateGame(this.mRoomToGetInto.name, this.mRoomOptionsForCreate, this.mRoomToEnterLobby);
					}
				}
				break;
			}
			if (operationResponse.ReturnCode != -2)
			{
				if (operationResponse.ReturnCode == short.MaxValue)
				{
					Debug.LogError($"The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.");
					object[] parameters = new object[1] { DisconnectCause.InvalidAuthentication };
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters);
				}
				else if (operationResponse.ReturnCode == 32755)
				{
					Debug.LogError($"Custom Authentication failed (either due to user-input or configuration or AuthParameter string format). Calling: OnCustomAuthenticationFailed()");
					object[] parameters2 = new object[1] { operationResponse.DebugMessage };
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, parameters2);
				}
				else
				{
					Debug.LogError($"Authentication failed: '{operationResponse.DebugMessage}' Code: {operationResponse.ReturnCode}");
				}
			}
			else
			{
				Debug.LogError(string.Format("If you host Photon yourself, make sure to start the 'Instance LoadBalancing' " + base.ServerAddress));
			}
			this.State = PeerStates.Disconnecting;
			this.Disconnect();
			if (operationResponse.ReturnCode == 32757)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogWarning($"Currently, the limit of users is reached for this title. Try again later. Disconnecting");
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached);
				object[] parameters3 = new object[1] { DisconnectCause.MaxCcuReached };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters3);
			}
			else if (operationResponse.ReturnCode == 32756)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogError($"The used master server address is not available with the subscription currently used. Got to Photon Cloud Dashboard or change URL. Disconnecting.");
				}
				object[] parameters4 = new object[1] { DisconnectCause.InvalidRegion };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters4);
			}
			else if (operationResponse.ReturnCode == 32753)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogError($"The authentication ticket expired. You need to connect (and authenticate) again. Disconnecting.");
				}
				object[] parameters5 = new object[1] { DisconnectCause.AuthenticationTicketExpired };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters5);
			}
			break;
		case 251:
		{
			ExitGames.Client.Photon.Hashtable pActorProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
			ExitGames.Client.Photon.Hashtable gameProperties = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
			this.ReadoutProperties(gameProperties, pActorProperties, 0);
			break;
		}
		case 254:
			this.DisconnectToReconnect2();
			break;
		default:
			Debug.LogWarning($"OperationResponse unhandled: {operationResponse.ToString()}");
			break;
		case 252:
		case 253:
			break;
		}
		this.externalListener.OnOperationResponse(operationResponse);
	}

	private void OnSerializeRead(ExitGames.Client.Photon.Hashtable data, PhotonPlayer sender, int networkTime, short correctPrefix)
	{
		int num = (int)data[(byte)0];
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			Debug.LogWarning("Received OnSerialization for view ID " + num + ". We have no such PhotonView! Ignored this if you're leaving a room. State: " + this.State);
		}
		else if (photonView.prefix > 0 && correctPrefix != photonView.prefix)
		{
			Debug.LogError("Received OnSerialization for view ID " + num + " with prefix " + correctPrefix + ". Our prefix is " + photonView.prefix);
		}
		else
		{
			if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
			{
				return;
			}
			if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
			{
				if (!this.DeltaCompressionRead(photonView, data))
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log("Skipping packet for " + photonView.name + " [" + photonView.viewID + "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game.");
					}
					return;
				}
				photonView.lastOnSerializeDataReceived = data[(byte)1] as object[];
			}
			if (photonView.observed is MonoBehaviour)
			{
				object[] incomingData = data[(byte)1] as object[];
				PhotonStream pStream = new PhotonStream(write: false, incomingData);
				PhotonMessageInfo info = new PhotonMessageInfo(sender, networkTime, photonView);
				photonView.ExecuteOnSerialize(pStream, info);
			}
			else if (photonView.observed is Transform)
			{
				object[] array = data[(byte)1] as object[];
				Transform transform = (Transform)photonView.observed;
				if (array.Length >= 1 && array[0] != null)
				{
					transform.localPosition = (Vector3)array[0];
				}
				if (array.Length >= 2 && array[1] != null)
				{
					transform.localRotation = (Quaternion)array[1];
				}
				if (array.Length >= 3 && array[2] != null)
				{
					transform.localScale = (Vector3)array[2];
				}
			}
			else if (photonView.observed is Rigidbody)
			{
				object[] array2 = data[(byte)1] as object[];
				Rigidbody rigidbody = (Rigidbody)photonView.observed;
				if (array2.Length >= 1 && array2[0] != null)
				{
					rigidbody.velocity = (Vector3)array2[0];
				}
				if (array2.Length >= 2 && array2[1] != null)
				{
					rigidbody.angularVelocity = (Vector3)array2[1];
				}
			}
			else
			{
				Debug.LogError("Type of observed is unknown when receiving.");
			}
		}
	}

	private ExitGames.Client.Photon.Hashtable OnSerializeWrite(PhotonView view)
	{
		List<object> list = new List<object>();
		if (view.observed is MonoBehaviour)
		{
			PhotonStream photonStream = new PhotonStream(write: true, null);
			PhotonMessageInfo info = new PhotonMessageInfo(this.mLocalActor, base.ServerTimeInMilliSeconds, view);
			view.ExecuteOnSerialize(photonStream, info);
			if (photonStream.Count == 0)
			{
				return null;
			}
			list = photonStream.data;
		}
		else if (view.observed is Transform)
		{
			Transform transform = (Transform)view.observed;
			if (view.onSerializeTransformOption == OnSerializeTransform.OnlyPosition || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
			{
				list.Add(transform.localPosition);
			}
			else
			{
				list.Add(null);
			}
			if (view.onSerializeTransformOption == OnSerializeTransform.OnlyRotation || view.onSerializeTransformOption == OnSerializeTransform.PositionAndRotation || view.onSerializeTransformOption == OnSerializeTransform.All)
			{
				list.Add(transform.localRotation);
			}
			else
			{
				list.Add(null);
			}
			if (view.onSerializeTransformOption == OnSerializeTransform.OnlyScale || view.onSerializeTransformOption == OnSerializeTransform.All)
			{
				list.Add(transform.localScale);
			}
		}
		else
		{
			if (!(view.observed is Rigidbody))
			{
				Debug.LogError("Observed type is not serializable: " + view.observed.GetType());
				return null;
			}
			Rigidbody rigidbody = (Rigidbody)view.observed;
			if (view.onSerializeRigidBodyOption != OnSerializeRigidBody.OnlyAngularVelocity)
			{
				list.Add(rigidbody.velocity);
			}
			else
			{
				list.Add(null);
			}
			if (view.onSerializeRigidBodyOption != 0)
			{
				list.Add(rigidbody.angularVelocity);
			}
		}
		object[] array = list.ToArray();
		if (view.synchronization == ViewSynchronization.UnreliableOnChange)
		{
			if (this.AlmostEquals(array, view.lastOnSerializeDataSent))
			{
				if (view.mixedModeIsReliable)
				{
					return null;
				}
				view.mixedModeIsReliable = true;
				view.lastOnSerializeDataSent = array;
			}
			else
			{
				view.mixedModeIsReliable = false;
				view.lastOnSerializeDataSent = array;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		hashtable[(byte)1] = array;
		if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			bool num = this.DeltaCompressionWrite(view, hashtable);
			view.lastOnSerializeDataSent = array;
			if (!num)
			{
				return null;
			}
		}
		return hashtable;
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log($"OnStatusChanged: {statusCode.ToString()}");
		}
		switch (statusCode)
		{
		case StatusCode.SecurityExceptionOnConnect:
		case StatusCode.ExceptionOnConnect:
		{
			this.State = PeerStates.PeerCreated;
			if (this.CustomAuthenticationValues != null)
			{
				this.CustomAuthenticationValues.Secret = null;
			}
			DisconnectCause disconnectCause = (DisconnectCause)statusCode;
			object[] parameters = new object[1] { disconnectCause };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters);
			break;
		}
		case StatusCode.Connect:
			if (this.State == PeerStates.ConnectingToNameServer)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
				{
					Debug.Log("Connected to NameServer.");
				}
				this.server = ServerConnection.NameServer;
				if (this.CustomAuthenticationValues != null)
				{
					this.CustomAuthenticationValues.Secret = null;
				}
			}
			if (this.State == PeerStates.ConnectingToGameserver)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
				{
					Debug.Log("Connected to gameserver.");
				}
				this.server = ServerConnection.GameServer;
				this.State = PeerStates.ConnectedToGameserver;
			}
			if (this.State == PeerStates.ConnectingToMasterserver)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
				{
					Debug.Log("Connected to masterserver.");
				}
				this.server = ServerConnection.MasterServer;
				this.State = PeerStates.ConnectedToMaster;
				if (this.IsInitialConnect)
				{
					this.IsInitialConnect = false;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton);
				}
			}
			base.EstablishEncryption();
			if (this.IsAuthorizeSecretAvailable)
			{
				this.didAuthenticate = this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, this.CloudRegion.ToString());
				if (this.didAuthenticate)
				{
					this.State = PeerStates.Authenticating;
				}
			}
			break;
		case StatusCode.Disconnect:
			this.didAuthenticate = false;
			this.isFetchingFriends = false;
			if (this.server == ServerConnection.GameServer)
			{
				this.LeftRoomCleanup();
			}
			if (this.server == ServerConnection.MasterServer)
			{
				this.LeftLobbyCleanup();
			}
			if (this.State == PeerStates.DisconnectingFromMasterserver)
			{
				if (this.Connect(this.mGameserver, ServerConnection.GameServer))
				{
					this.State = PeerStates.ConnectingToGameserver;
				}
				break;
			}
			if (this.State == PeerStates.DisconnectingFromGameserver || this.State == PeerStates.DisconnectingFromNameServer)
			{
				if (this.Connect(this.MasterServerAddress, ServerConnection.MasterServer))
				{
					this.State = PeerStates.ConnectingToMasterserver;
				}
				break;
			}
			if (this.CustomAuthenticationValues != null)
			{
				this.CustomAuthenticationValues.Secret = null;
			}
			this.State = PeerStates.PeerCreated;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton);
			break;
		case StatusCode.Exception:
			if (!this.IsInitialConnect)
			{
				this.State = PeerStates.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters4 = new object[1] { disconnectCause };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters4);
			}
			else
			{
				Debug.LogError("Exception while connecting to: " + base.ServerAddress + ". Check if the server is available.");
				if (base.ServerAddress == null || base.ServerAddress.StartsWith("127.0.0.1"))
				{
					Debug.LogWarning("The server address is 127.0.0.1 (localhost): Make sure the server is running on this machine. Android and iOS emulators have their own localhost.");
					if (base.ServerAddress == this.mGameserver)
					{
						Debug.LogWarning("This might be a misconfiguration in the game server config. You need to edit it to a (public) address.");
					}
				}
				this.State = PeerStates.PeerCreated;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters5 = new object[1] { disconnectCause };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters5);
			}
			this.Disconnect();
			break;
		case StatusCode.ExceptionOnReceive:
		case StatusCode.TimeoutDisconnect:
		case StatusCode.DisconnectByServerTimeout:
		case StatusCode.DisconnectByServerUserLimit:
		case StatusCode.DisconnectByServerLogic:
			if (!this.IsInitialConnect)
			{
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters2 = new object[1] { disconnectCause };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, parameters2);
			}
			else
			{
				Debug.LogWarning(string.Concat(statusCode, " while connecting to: ", base.ServerAddress, ". Check if the server is available."));
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				object[] parameters3 = new object[1] { disconnectCause };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, parameters3);
			}
			if (this.CustomAuthenticationValues != null)
			{
				this.CustomAuthenticationValues.Secret = null;
			}
			this.Disconnect();
			break;
		case StatusCode.EncryptionEstablished:
			if (this.server == ServerConnection.NameServer)
			{
				this.State = PeerStates.ConnectedToNameServer;
				if (!this.didAuthenticate && this.CloudRegion == CloudRegionCode.none)
				{
					this.OpGetRegions(this.mAppId);
				}
			}
			if (!this.didAuthenticate && (!this.IsUsingNameServer || this.CloudRegion != CloudRegionCode.none))
			{
				this.didAuthenticate = this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, this.CloudRegion.ToString());
				if (this.didAuthenticate)
				{
					this.State = PeerStates.Authenticating;
				}
			}
			break;
		case StatusCode.EncryptionFailedToEstablish:
			Debug.LogError("Encryption wasn't established: " + statusCode.ToString() + ". Going to authenticate anyways.");
			this.OpAuthenticate(this.mAppId, this.mAppVersionPun, this.PlayerName, this.CustomAuthenticationValues, this.CloudRegion.ToString());
			break;
		default:
			Debug.LogError("Received unknown status code: " + statusCode);
			break;
		case StatusCode.SendError:
			break;
		}
		this.externalListener.OnStatusChanged(statusCode);
	}

	public void OpCleanRpcBuffer(PhotonView view)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions);
	}

	public void OpCleanRpcBuffer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNumber };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		this.OpRaiseEvent(200, null, sendReliable: true, raiseEventOptions2);
	}

	public bool OpCreateGame(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		bool flag = this.server == ServerConnection.GameServer;
		if (!flag)
		{
			this.mRoomOptionsForCreate = roomOptions;
			this.mRoomToGetInto = new Room(roomName, roomOptions);
			this.mRoomToEnterLobby = ((!this.insideLobby) ? null : this.lobby);
		}
		this.mLastJoinType = JoinType.CreateGame;
		return base.OpCreateRoom(roomName, roomOptions, this.mRoomToEnterLobby, this.GetLocalActorProperties(), flag);
	}

	public override bool OpFindFriends(string[] friendsToFind)
	{
		if (this.isFetchingFriends)
		{
			return false;
		}
		this.friendListRequested = friendsToFind;
		this.isFetchingFriends = true;
		return base.OpFindFriends(friendsToFind);
	}

	public override bool OpJoinRandomRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, ExitGames.Client.Photon.Hashtable playerProperties, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter)
	{
		this.mRoomToGetInto = new Room(null, null);
		this.mRoomToEnterLobby = null;
		this.mLastJoinType = JoinType.JoinRandomGame;
		return base.OpJoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, playerProperties, matchingType, typedLobby, sqlLobbyFilter);
	}

	public bool OpJoinRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, bool createIfNotExists)
	{
		bool flag = this.server == ServerConnection.GameServer;
		if (!flag)
		{
			this.mRoomOptionsForCreate = roomOptions;
			this.mRoomToGetInto = new Room(roomName, roomOptions);
			this.mRoomToEnterLobby = null;
			if (createIfNotExists)
			{
				this.mRoomToEnterLobby = ((!this.insideLobby) ? null : this.lobby);
			}
		}
		this.mLastJoinType = ((!createIfNotExists) ? JoinType.JoinGame : JoinType.JoinOrCreateOnDemand);
		return base.OpJoinRoom(roomName, roomOptions, this.mRoomToEnterLobby, createIfNotExists, this.GetLocalActorProperties(), flag);
	}

	public virtual bool OpLeave()
	{
		if (this.State != PeerStates.Joined)
		{
			Debug.LogWarning("Not sending leave operation. State is not 'Joined': " + this.State);
			return false;
		}
		return this.OpCustom(254, null, sendReliable: true, 0);
	}

	public override bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		if (PhotonNetwork.offlineMode)
		{
			return false;
		}
		return base.OpRaiseEvent(eventCode, customEventContent, sendReliable, raiseEventOptions);
	}

	public void OpRemoveCompleteCache()
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			Receivers = ReceiverGroup.MasterClient
		};
		this.OpRaiseEvent(0, null, sendReliable: true, raiseEventOptions);
	}

	public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNumber };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		this.OpRaiseEvent(0, null, sendReliable: true, raiseEventOptions2);
	}

	private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNr };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		this.OpRaiseEvent(202, null, sendReliable: true, raiseEventOptions2);
	}

	private void ReadoutProperties(ExitGames.Client.Photon.Hashtable gameProperties, ExitGames.Client.Photon.Hashtable pActorProperties, int targetActorNr)
	{
		if (this.mCurrentGame != null && gameProperties != null)
		{
			this.mCurrentGame.CacheProperties(gameProperties);
			object[] parameters = new object[1] { gameProperties };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, parameters);
			if (PhotonNetwork.automaticallySyncScene)
			{
				this.LoadLevelIfSynced();
			}
		}
		if (pActorProperties == null || pActorProperties.Count <= 0)
		{
			return;
		}
		if (targetActorNr > 0)
		{
			PhotonPlayer playerWithID = this.GetPlayerWithID(targetActorNr);
			if (playerWithID != null)
			{
				ExitGames.Client.Photon.Hashtable actorPropertiesForActorNr = this.GetActorPropertiesForActorNr(pActorProperties, targetActorNr);
				playerWithID.InternalCacheProperties(actorPropertiesForActorNr);
				object[] parameters2 = new object[2] { playerWithID, actorPropertiesForActorNr };
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters2);
			}
			return;
		}
		foreach (object key in pActorProperties.Keys)
		{
			int num = (int)key;
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)pActorProperties[key];
			string name = (string)hashtable[byte.MaxValue];
			PhotonPlayer photonPlayer = this.GetPlayerWithID(num);
			if (photonPlayer == null)
			{
				photonPlayer = new PhotonPlayer(isLocal: false, num, name);
				this.AddNewPlayer(num, photonPlayer);
			}
			photonPlayer.InternalCacheProperties(hashtable);
			object[] parameters3 = new object[2] { photonPlayer, hashtable };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters3);
		}
	}

	private void RebuildPlayerListCopies()
	{
		this.mPlayerListCopy = new PhotonPlayer[this.mActors.Count];
		this.mActors.Values.CopyTo(this.mPlayerListCopy, 0);
		List<PhotonPlayer> list = new List<PhotonPlayer>();
		PhotonPlayer[] array = this.mPlayerListCopy;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (!photonPlayer.isLocal)
			{
				list.Add(photonPlayer);
			}
		}
		this.mOtherPlayerListCopy = list.ToArray();
	}

	public void RegisterPhotonView(PhotonView netView)
	{
		if (!Application.isPlaying)
		{
			this.photonViewList = new Dictionary<int, PhotonView>();
		}
		else
		{
			if (netView.subId == 0)
			{
				return;
			}
			if (this.photonViewList.ContainsKey(netView.viewID))
			{
				if (netView != this.photonViewList[netView.viewID])
				{
					Debug.LogError($"PhotonView ID duplicate found: {netView.viewID}. New: {netView} old: {this.photonViewList[netView.viewID]}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.");
				}
				this.RemoveInstantiatedGO(this.photonViewList[netView.viewID].gameObject, localOnly: true);
			}
			this.photonViewList.Add(netView.viewID, netView);
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
			{
				Debug.Log("Registered PhotonView: " + netView.viewID);
			}
		}
	}

	public void RemoveAllInstantiatedObjects()
	{
		GameObject[] array = new GameObject[this.instantiatedObjects.Count];
		this.instantiatedObjects.Values.CopyTo(array, 0);
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				this.RemoveInstantiatedGO(gameObject, localOnly: false);
			}
		}
		if (this.instantiatedObjects.Count > 0)
		{
			Debug.LogError("RemoveAllInstantiatedObjects() this.instantiatedObjects.Count should be 0 by now.");
		}
		this.instantiatedObjects = new Dictionary<int, GameObject>();
	}

	private void RemoveCacheOfLeftPlayers()
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[244] = (byte)0;
		dictionary[247] = (byte)7;
		this.OpCustom(253, dictionary, sendReliable: true, 0);
	}

	public void RemoveInstantiatedGO(GameObject go, bool localOnly)
	{
		if (go == null)
		{
			Debug.LogError("Failed to 'network-remove' GameObject because it's null.");
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			Debug.LogError("Failed to 'network-remove' GameObject because has no PhotonView components: " + go);
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int ownerActorNr = photonView.OwnerActorNr;
		int instantiationId = photonView.instantiationId;
		if (!localOnly)
		{
			if (!photonView.isMine && (!this.mLocalActor.isMasterClient || this.mActors.ContainsKey(ownerActorNr)))
			{
				Debug.LogError("Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left: " + photonView);
				return;
			}
			if (instantiationId < 1)
			{
				Debug.LogError("Failed to 'network-remove' GameObject because it is missing a valid InstantiationId on view: " + photonView?.ToString() + ". Not Destroying GameObject or PhotonViews!");
				return;
			}
		}
		if (!localOnly)
		{
			this.ServerCleanInstantiateAndDestroy(instantiationId, ownerActorNr);
		}
		this.instantiatedObjects.Remove(instantiationId);
		for (int num = componentsInChildren.Length - 1; num >= 0; num--)
		{
			PhotonView photonView2 = componentsInChildren[num];
			if (photonView2 != null)
			{
				if (photonView2.instantiationId >= 1)
				{
					this.LocalCleanPhotonView(photonView2);
				}
				if (!localOnly)
				{
					this.OpCleanRpcBuffer(photonView2);
				}
			}
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Network destroy Instantiated GO: " + go.name);
		}
		UnityEngine.Object.Destroy(go);
	}

	private void RemoveInstantiationData(int instantiationId)
	{
		this.tempInstantiationData.Remove(instantiationId);
	}

	private void RemovePlayer(int ID, PhotonPlayer player)
	{
		this.mActors.Remove(ID);
		if (!player.isLocal)
		{
			this.RebuildPlayerListCopies();
		}
	}

	public void RemoveRPCsInGroup(int group)
	{
		foreach (KeyValuePair<int, PhotonView> photonView in this.photonViewList)
		{
			PhotonView value = photonView.Value;
			if (value.group == group)
			{
				this.CleanRpcBufferIfMine(value);
			}
		}
	}

	private void ResetPhotonViewsOnSerialize()
	{
		foreach (PhotonView value in this.photonViewList.Values)
		{
			value.lastOnSerializeDataSent = null;
		}
	}

	private static int ReturnLowestPlayerId(PhotonPlayer[] players, int playerIdToIgnore)
	{
		if (players == null || players.Length == 0)
		{
			return -1;
		}
		int num = int.MaxValue;
		foreach (PhotonPlayer photonPlayer in players)
		{
			if (photonPlayer.ID != playerIdToIgnore && photonPlayer.ID < num)
			{
				num = photonPlayer.ID;
			}
		}
		return num;
	}

	internal void RPC(PhotonView view, string methodName, PhotonPlayer player, params object[] parameters)
	{
		if (!this.blockSendingGroups.Contains(view.group))
		{
			if (view.viewID < 1)
			{
				Debug.LogError("Illegal view ID:" + view.viewID + " method: " + methodName + " GO:" + view.gameObject.name);
			}
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
			{
				Debug.Log(string.Concat("Sending RPC \"", methodName, "\" to player[", player, "]"));
			}
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = view.viewID;
			if (view.prefix > 0)
			{
				hashtable[(byte)1] = (short)view.prefix;
			}
			hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
			int value = 0;
			if (this.rpcShortcuts.TryGetValue(methodName, out value))
			{
				hashtable[(byte)5] = (byte)value;
			}
			else
			{
				hashtable[(byte)3] = methodName;
			}
			if (parameters != null && parameters.Length != 0)
			{
				hashtable[(byte)4] = parameters;
			}
			if (this.mLocalActor == player)
			{
				this.ExecuteRPC(hashtable, player);
				return;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.TargetActors = new int[1] { player.ID };
			RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions2);
		}
	}

	internal void RPC(PhotonView view, string methodName, PhotonTargets target, params object[] parameters)
	{
		if (this.blockSendingGroups.Contains(view.group))
		{
			return;
		}
		if (view.viewID < 1)
		{
			Debug.LogError("Illegal view ID:" + view.viewID + " method: " + methodName + " GO:" + view.gameObject.name);
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Sending RPC \"" + methodName + "\" to " + target);
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = view.viewID;
		if (view.prefix > 0)
		{
			hashtable[(byte)1] = (short)view.prefix;
		}
		hashtable[(byte)2] = base.ServerTimeInMilliSeconds;
		int value = 0;
		if (this.rpcShortcuts.TryGetValue(methodName, out value))
		{
			hashtable[(byte)5] = (byte)value;
		}
		else
		{
			hashtable[(byte)3] = methodName;
		}
		if (parameters != null && parameters.Length != 0)
		{
			hashtable[(byte)4] = parameters;
		}
		switch (target)
		{
		case PhotonTargets.All:
		{
			RaiseEventOptions raiseEventOptions5 = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group
			};
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions5);
			this.ExecuteRPC(hashtable, this.mLocalActor);
			break;
		}
		case PhotonTargets.Others:
		{
			RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group
			};
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions4);
			break;
		}
		case PhotonTargets.AllBuffered:
		{
			RaiseEventOptions raiseEventOptions7 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache
			};
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions7);
			this.ExecuteRPC(hashtable, this.mLocalActor);
			break;
		}
		case PhotonTargets.OthersBuffered:
		{
			RaiseEventOptions raiseEventOptions6 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache
			};
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions6);
			break;
		}
		case PhotonTargets.MasterClient:
		{
			if (this.mMasterClient == this.mLocalActor)
			{
				this.ExecuteRPC(hashtable, this.mLocalActor);
				break;
			}
			RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.MasterClient
			};
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions3);
			break;
		}
		case PhotonTargets.AllViaServer:
		{
			RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group,
				Receivers = ReceiverGroup.All
			};
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions2);
			break;
		}
		case PhotonTargets.AllBufferedViaServer:
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				InterestGroup = (byte)view.group,
				Receivers = ReceiverGroup.All,
				CachingOption = EventCaching.AddToRoomCache
			};
			this.OpRaiseEvent(200, hashtable, sendReliable: true, raiseEventOptions);
			break;
		}
		default:
			Debug.LogError("Unsupported target enum: " + target);
			break;
		}
	}

	public void RunViewUpdate()
	{
		if (!PhotonNetwork.connected || PhotonNetwork.offlineMode || this.mActors == null || this.mActors.Count <= 1)
		{
			return;
		}
		Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
		Dictionary<int, ExitGames.Client.Photon.Hashtable> dictionary2 = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();
		foreach (KeyValuePair<int, PhotonView> photonView in this.photonViewList)
		{
			PhotonView value = photonView.Value;
			if (!(value.observed != null) || value.synchronization == ViewSynchronization.Off || (value.ownerId != this.mLocalActor.ID && (!value.isSceneView || this.mMasterClient != this.mLocalActor)) || !value.gameObject.activeInHierarchy || this.blockSendingGroups.Contains(value.group))
			{
				continue;
			}
			ExitGames.Client.Photon.Hashtable hashtable = this.OnSerializeWrite(value);
			if (hashtable == null)
			{
				continue;
			}
			if (value.synchronization == ViewSynchronization.ReliableDeltaCompressed || value.mixedModeIsReliable)
			{
				if (!hashtable.ContainsKey((byte)1) && !hashtable.ContainsKey((byte)2))
				{
					continue;
				}
				if (!dictionary.ContainsKey(value.group))
				{
					dictionary[value.group] = new ExitGames.Client.Photon.Hashtable();
					dictionary[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
					if (this.currentLevelPrefix >= 0)
					{
						dictionary[value.group][(byte)1] = this.currentLevelPrefix;
					}
				}
				ExitGames.Client.Photon.Hashtable hashtable2 = dictionary[value.group];
				hashtable2.Add((short)hashtable2.Count, hashtable);
				continue;
			}
			if (!dictionary2.ContainsKey(value.group))
			{
				dictionary2[value.group] = new ExitGames.Client.Photon.Hashtable();
				dictionary2[value.group][(byte)0] = base.ServerTimeInMilliSeconds;
				if (this.currentLevelPrefix >= 0)
				{
					dictionary2[value.group][(byte)1] = this.currentLevelPrefix;
				}
			}
			ExitGames.Client.Photon.Hashtable hashtable3 = dictionary2[value.group];
			hashtable3.Add((short)hashtable3.Count, hashtable);
		}
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item in dictionary)
		{
			raiseEventOptions.InterestGroup = (byte)item.Key;
			this.OpRaiseEvent(206, item.Value, sendReliable: true, raiseEventOptions);
		}
		foreach (KeyValuePair<int, ExitGames.Client.Photon.Hashtable> item2 in dictionary2)
		{
			raiseEventOptions.InterestGroup = (byte)item2.Key;
			this.OpRaiseEvent(201, item2.Value, sendReliable: false, raiseEventOptions);
		}
	}

	private void SendDestroyOfAll()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = -1;
		this.OpRaiseEvent(207, hashtable, sendReliable: true, null);
	}

	private void SendDestroyOfPlayer(int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = actorNr;
		this.OpRaiseEvent(207, hashtable, sendReliable: true, null);
	}

	internal ExitGames.Client.Photon.Hashtable SendInstantiate(string prefabName, Vector3 position, Quaternion rotation, int group, int[] viewIDs, object[] data, bool isGlobalObject)
	{
		int num = viewIDs[0];
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)0] = prefabName;
		if (position != Vector3.zero)
		{
			hashtable[(byte)1] = position;
		}
		if (rotation != Quaternion.identity)
		{
			hashtable[(byte)2] = rotation;
		}
		if (group != 0)
		{
			hashtable[(byte)3] = group;
		}
		if (viewIDs.Length > 1)
		{
			hashtable[(byte)4] = viewIDs;
		}
		if (data != null)
		{
			hashtable[(byte)5] = data;
		}
		if (this.currentLevelPrefix > 0)
		{
			hashtable[(byte)8] = this.currentLevelPrefix;
		}
		hashtable[(byte)6] = base.ServerTimeInMilliSeconds;
		hashtable[(byte)7] = num;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = ((!isGlobalObject) ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal)
		};
		this.OpRaiseEvent(202, hashtable, sendReliable: true, raiseEventOptions);
		return hashtable;
	}

	public static void SendMonoMessage(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<GameObject> hashSet;
		if (PhotonNetwork.SendMonoMessageTargets != null)
		{
			hashSet = PhotonNetwork.SendMonoMessageTargets;
		}
		else
		{
			hashSet = new HashSet<GameObject>();
			Component[] array = (Component[])UnityEngine.Object.FindObjectsOfType(typeof(MonoBehaviour));
			for (int i = 0; i < array.Length; i++)
			{
				hashSet.Add(array[i].gameObject);
			}
		}
		string methodName = methodString.ToString();
		foreach (GameObject item in hashSet)
		{
			if (parameters != null && parameters.Length == 1)
			{
				item.SendMessage(methodName, parameters[0], SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				item.SendMessage(methodName, parameters, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	private void SendPlayerName()
	{
		if (this.State == PeerStates.Joining)
		{
			this.mPlayernameHasToBeUpdated = true;
		}
		else if (this.mLocalActor != null)
		{
			this.mLocalActor.name = this.PlayerName;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[byte.MaxValue] = this.PlayerName;
			if (this.mLocalActor.ID > 0)
			{
				base.OpSetPropertiesOfActor(this.mLocalActor.ID, hashtable, broadcast: true, 0);
				this.mPlayernameHasToBeUpdated = false;
			}
		}
	}

	private void ServerCleanInstantiateAndDestroy(int instantiateId, int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[(byte)7] = instantiateId;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		raiseEventOptions.TargetActors = new int[1] { actorNr };
		RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
		this.OpRaiseEvent(202, hashtable, sendReliable: true, raiseEventOptions2);
		ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
		hashtable2[(byte)0] = instantiateId;
		this.OpRaiseEvent(204, hashtable2, sendReliable: true, null);
	}

	public void SetApp(string appId, string gameVersion)
	{
		this.mAppId = appId.Trim();
		if (!string.IsNullOrEmpty(gameVersion))
		{
			this.mAppVersion = gameVersion.Trim();
		}
	}

	protected internal void SetLevelInPropsIfSynced(object levelId)
	{
		if (!PhotonNetwork.automaticallySyncScene || !PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
		{
			return;
		}
		if (levelId == null)
		{
			Debug.LogError("Parameter levelId can't be null!");
			return;
		}
		if (PhotonNetwork.room.customProperties.ContainsKey("curScn"))
		{
			object obj = PhotonNetwork.room.customProperties["curScn"];
			if ((obj is int && Application.loadedLevel == (int)obj) || (obj is string && Application.loadedLevelName.Equals((string)obj)))
			{
				return;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (levelId is int)
		{
			hashtable["curScn"] = (int)levelId;
		}
		else if (levelId is string)
		{
			hashtable["curScn"] = (string)levelId;
		}
		else
		{
			Debug.LogError("Parameter levelId must be int or string!");
		}
		PhotonNetwork.room.SetCustomProperties(hashtable);
		this.SendOutgoingCommands();
	}

	public void SetLevelPrefix(short prefix)
	{
		this.currentLevelPrefix = prefix;
	}

	protected internal bool SetMasterClient(int playerId, bool sync)
	{
		if (this.mMasterClient == null || this.mMasterClient.ID == -1 || !this.mActors.ContainsKey(playerId))
		{
			return false;
		}
		if (sync)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add((byte)1, playerId);
			if (!this.OpRaiseEvent(208, hashtable, sendReliable: true, null))
			{
				return false;
			}
		}
		this.hasSwitchedMC = true;
		this.mMasterClient = this.mActors[playerId];
		object[] parameters = new object[1] { this.mMasterClient };
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, parameters);
		return true;
	}

	public void SetReceivingEnabled(int group, bool enabled)
	{
		if (group <= 0)
		{
			Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + group + ". The group number should be at least 1.");
		}
		else if (enabled)
		{
			if (!this.allowedReceivingGroups.Contains(group))
			{
				this.allowedReceivingGroups.Add(group);
				byte[] groupsToAdd = new byte[1] { (byte)group };
				this.OpChangeGroups(null, groupsToAdd);
			}
		}
		else if (this.allowedReceivingGroups.Contains(group))
		{
			this.allowedReceivingGroups.Remove(group);
			byte[] groupsToRemove = new byte[1] { (byte)group };
			this.OpChangeGroups(groupsToRemove, null);
		}
	}

	public void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		List<byte> list = new List<byte>();
		List<byte> list2 = new List<byte>();
		if (enableGroups != null)
		{
			for (int i = 0; i < enableGroups.Length; i++)
			{
				int num = enableGroups[i];
				if (num <= 0)
				{
					Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + num + ". The group number should be at least 1.");
				}
				else if (!this.allowedReceivingGroups.Contains(num))
				{
					this.allowedReceivingGroups.Add(num);
					list.Add((byte)num);
				}
			}
		}
		if (disableGroups != null)
		{
			for (int j = 0; j < disableGroups.Length; j++)
			{
				int num2 = disableGroups[j];
				if (num2 <= 0)
				{
					Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled was called with an illegal group number: " + num2 + ". The group number should be at least 1.");
				}
				else if (list.Contains((byte)num2))
				{
					Debug.LogError("Error: PhotonNetwork.SetReceivingEnabled disableGroups contains a group that is also in the enableGroups: " + num2 + ".");
				}
				else if (this.allowedReceivingGroups.Contains(num2))
				{
					this.allowedReceivingGroups.Remove(num2);
					list2.Add((byte)num2);
				}
			}
		}
		this.OpChangeGroups((list2.Count <= 0) ? null : list2.ToArray(), (list.Count <= 0) ? null : list.ToArray());
	}

	public void SetSendingEnabled(int group, bool enabled)
	{
		if (!enabled)
		{
			this.blockSendingGroups.Add(group);
		}
		else
		{
			this.blockSendingGroups.Remove(group);
		}
	}

	public void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		int[] array;
		if (enableGroups != null)
		{
			array = enableGroups;
			foreach (int item in array)
			{
				if (this.blockSendingGroups.Contains(item))
				{
					this.blockSendingGroups.Remove(item);
				}
			}
		}
		if (disableGroups == null)
		{
			return;
		}
		array = disableGroups;
		foreach (int item2 in array)
		{
			if (!this.blockSendingGroups.Contains(item2))
			{
				this.blockSendingGroups.Add(item2);
			}
		}
	}

	private void StoreInstantiationData(int instantiationId, object[] instantiationData)
	{
		this.tempInstantiationData[instantiationId] = instantiationData;
	}

	public bool WebRpc(string uriPath, object parameters)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary.Add(209, uriPath);
		dictionary.Add(208, parameters);
		return this.OpCustom(219, dictionary, sendReliable: true, 0);
	}
}
