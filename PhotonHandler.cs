using System;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

internal class PhotonHandler : Photon.MonoBehaviour, IPhotonPeerListener
{
	public static bool AppQuits;

	internal static CloudRegionCode BestRegionCodeCurrently = CloudRegionCode.none;

	private int nextSendTickCount;

	private int nextSendTickCountOnSerialize;

	public static Type PingImplementation;

	private const string PlayerPrefsKey = "PUNCloudBestRegion";

	private static bool sendThreadShouldRun;

	public static PhotonHandler SP;

	public int updateInterval;

	public int updateIntervalOnSerialize;

	internal static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			string @string = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				return Region.Parse(@string);
			}
			return CloudRegionCode.none;
		}
		set
		{
			if (value == CloudRegionCode.none)
			{
				PlayerPrefs.DeleteKey("PUNCloudBestRegion");
			}
			else
			{
				PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
			}
		}
	}

	protected void Awake()
	{
		if (PhotonHandler.SP != null && PhotonHandler.SP != this && PhotonHandler.SP.gameObject != null)
		{
			UnityEngine.Object.DestroyImmediate(PhotonHandler.SP.gameObject);
		}
		PhotonHandler.SP = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.updateInterval = 1000 / PhotonNetwork.sendRate;
		this.updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
		PhotonHandler.StartFallbackSendAckThread();
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		switch (level)
		{
		case DebugLevel.ERROR:
			Debug.LogError(message);
			return;
		case DebugLevel.WARNING:
			Debug.LogWarning(message);
			return;
		case DebugLevel.INFO:
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(message);
				return;
			}
			break;
		}
		if (level == DebugLevel.ALL && PhotonNetwork.logLevel == PhotonLogLevel.Full)
		{
			Debug.Log(message);
		}
	}

	public static bool FallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun && PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.SendAcksOnly();
		}
		return PhotonHandler.sendThreadShouldRun;
	}

	protected void OnApplicationQuit()
	{
		PhotonHandler.AppQuits = true;
		PhotonHandler.StopFallbackSendAckThread();
		PhotonNetwork.Disconnect();
	}

	protected void OnCreatedRoom()
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
	}

	public void OnEvent(EventData photonEvent)
	{
	}

	protected void OnJoinedRoom()
	{
		PhotonNetwork.networkingPeer.LoadLevelIfSynced();
	}

	protected void OnLevelWasLoaded(int level)
	{
		PhotonNetwork.networkingPeer.NewSceneLoaded();
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(Application.loadedLevelName);
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
	}

	public static void StartFallbackSendAckThread()
	{
		if (!PhotonHandler.sendThreadShouldRun)
		{
			PhotonHandler.sendThreadShouldRun = true;
			SupportClass.CallInBackground(FallbackSendAckThread);
		}
	}

	public static void StopFallbackSendAckThread()
	{
		PhotonHandler.sendThreadShouldRun = false;
	}

	protected void Update()
	{
		if (PhotonNetwork.networkingPeer == null)
		{
			Debug.LogError("NetworkPeer broke!");
		}
		else
		{
			if (PhotonNetwork.connectionStateDetailed == PeerStates.PeerCreated || PhotonNetwork.connectionStateDetailed == PeerStates.Disconnected || PhotonNetwork.offlineMode || !PhotonNetwork.isMessageQueueRunning)
			{
				return;
			}
			bool flag = true;
			while (PhotonNetwork.isMessageQueueRunning && flag)
			{
				flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
			}
			int num = (int)(Time.realtimeSinceStartup * 1000f);
			if (PhotonNetwork.isMessageQueueRunning && num > this.nextSendTickCountOnSerialize)
			{
				PhotonNetwork.networkingPeer.RunViewUpdate();
				this.nextSendTickCountOnSerialize = num + this.updateIntervalOnSerialize;
				this.nextSendTickCount = 0;
			}
			num = (int)(Time.realtimeSinceStartup * 1000f);
			if (num > this.nextSendTickCount)
			{
				bool flag2 = true;
				while (PhotonNetwork.isMessageQueueRunning && flag2)
				{
					flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
				}
				this.nextSendTickCount = num + this.updateInterval;
			}
		}
	}
}
