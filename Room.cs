using ExitGames.Client.Photon;
using UnityEngine;

public class Room : RoomInfo
{
	public bool autoCleanUp => base.autoCleanUpField;

	public new int maxPlayers
	{
		get
		{
			return base.maxPlayersField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set maxPlayers when not in that room.");
			}
			if (value > 255)
			{
				Debug.LogWarning("Can't set Room.maxPlayers to: " + value + ". Using max value: 255.");
				value = 255;
			}
			if (value != base.maxPlayersField && !PhotonNetwork.offlineMode)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add(byte.MaxValue, (byte)value);
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, broadcast: true, 0);
			}
			base.maxPlayersField = (byte)value;
		}
	}

	public new string name
	{
		get
		{
			return base.nameField;
		}
		internal set
		{
			base.nameField = value;
		}
	}

	public new bool open
	{
		get
		{
			return base.openField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set open when not in that room.");
			}
			if (value != base.openField && !PhotonNetwork.offlineMode)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add((byte)253, value);
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, broadcast: true, 0);
			}
			base.openField = value;
		}
	}

	public new int playerCount
	{
		get
		{
			if (PhotonNetwork.playerList != null)
			{
				return PhotonNetwork.playerList.Length;
			}
			return 0;
		}
	}

	public string[] propertiesListedInLobby { get; private set; }

	public new bool visible
	{
		get
		{
			return base.visibleField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set visible when not in that room.");
			}
			if (value != base.visibleField && !PhotonNetwork.offlineMode)
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add((byte)254, value);
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, broadcast: true, 0);
			}
			base.visibleField = value;
		}
	}

	internal Room(string roomName, RoomOptions options)
		: base(roomName, null)
	{
		if (options == null)
		{
			options = new RoomOptions();
		}
		base.visibleField = options.isVisible;
		base.openField = options.isOpen;
		base.maxPlayersField = (byte)options.maxPlayers;
		base.autoCleanUpField = false;
		base.CacheProperties(options.customRoomProperties);
		this.propertiesListedInLobby = options.customRoomPropertiesForLobby;
	}

	public void SetCustomProperties(Hashtable propertiesToSet)
	{
		if (propertiesToSet != null)
		{
			base.customProperties.MergeStringKeys(propertiesToSet);
			base.customProperties.StripKeysWithNullValues();
			Hashtable gameProperties = propertiesToSet.StripToStringKeys();
			if (!PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfRoom(gameProperties, broadcast: true, 0);
			}
			object[] parameters = new object[1] { propertiesToSet };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, parameters);
		}
	}

	public void SetPropertiesListedInLobby(string[] propsListedInLobby)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[(byte)250] = propsListedInLobby;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, broadcast: false, 0);
		this.propertiesListedInLobby = propsListedInLobby;
	}

	public override string ToString()
	{
		object[] args = new object[5]
		{
			base.nameField,
			(!base.visibleField) ? "hidden" : "visible",
			(!base.openField) ? "closed" : "open",
			base.maxPlayersField,
			this.playerCount
		};
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", args);
	}

	public new string ToStringFull()
	{
		object[] args = new object[6]
		{
			base.nameField,
			(!base.visibleField) ? "hidden" : "visible",
			(!base.openField) ? "closed" : "open",
			base.maxPlayersField,
			this.playerCount,
			base.customProperties.ToStringFull()
		};
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", args);
	}
}
