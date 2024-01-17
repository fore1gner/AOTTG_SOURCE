using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonPlayer
{
	private int actorID;

	public readonly bool isLocal;

	private string nameField;

	public object TagObject;

	public Hashtable allProperties
	{
		get
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Merge(this.customProperties);
			hashtable[byte.MaxValue] = this.name;
			return hashtable;
		}
	}

	public Hashtable customProperties { get; private set; }

	public int ID => this.actorID;

	public bool isMasterClient => PhotonNetwork.networkingPeer.mMasterClient == this;

	public string name
	{
		get
		{
			return this.nameField;
		}
		set
		{
			if (!this.isLocal)
			{
				Debug.LogError("Error: Cannot change the name of a remote player!");
			}
			else
			{
				this.nameField = value;
			}
		}
	}

	public static void CleanProperties()
	{
		if (PhotonNetwork.player != null)
		{
			PhotonNetwork.player.customProperties.Clear();
			PhotonNetwork.player.SetCustomProperties(new Hashtable { 
			{
				PhotonPlayerProperty.name,
				LoginFengKAI.player.name
			} });
		}
	}

	protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
	{
		this.actorID = -1;
		this.nameField = string.Empty;
		this.customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		this.InternalCacheProperties(properties);
	}

	public PhotonPlayer(bool isLocal, int actorID, string name)
	{
		this.actorID = -1;
		this.nameField = string.Empty;
		this.customProperties = new Hashtable();
		this.isLocal = isLocal;
		this.actorID = actorID;
		this.nameField = name;
	}

	public override bool Equals(object p)
	{
		if (p is PhotonPlayer photonPlayer)
		{
			return this.GetHashCode() == photonPlayer.GetHashCode();
		}
		return false;
	}

	public static PhotonPlayer Find(int ID)
	{
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			if (photonPlayer.ID == ID)
			{
				return photonPlayer;
			}
		}
		return null;
	}

	public Hashtable ChangeLocalPlayer(int NewID, string inputname)
	{
		this.actorID = NewID;
		string value = (this.nameField = $"{inputname}_ID:{NewID}");
		Hashtable result = new Hashtable
		{
			{
				PhotonPlayerProperty.name,
				value
			},
			{
				PhotonPlayerProperty.guildName,
				LoginFengKAI.player.guildname
			},
			{
				PhotonPlayerProperty.kills,
				0
			},
			{
				PhotonPlayerProperty.max_dmg,
				0
			},
			{
				PhotonPlayerProperty.total_dmg,
				0
			},
			{
				PhotonPlayerProperty.deaths,
				0
			},
			{
				PhotonPlayerProperty.dead,
				true
			},
			{
				PhotonPlayerProperty.isTitan,
				0
			},
			{
				PhotonPlayerProperty.RCteam,
				0
			},
			{
				PhotonPlayerProperty.currentLevel,
				string.Empty
			}
		};
		PhotonNetwork.AllocateViewID();
		PhotonNetwork.player.SetCustomProperties(result);
		return result;
	}

	public PhotonPlayer Get(int id)
	{
		return PhotonPlayer.Find(id);
	}

	public override int GetHashCode()
	{
		return this.ID;
	}

	public PhotonPlayer GetNext()
	{
		return this.GetNextFor(this.ID);
	}

	public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
	{
		if (currentPlayer == null)
		{
			return null;
		}
		return this.GetNextFor(currentPlayer.ID);
	}

	public PhotonPlayer GetNextFor(int currentPlayerId)
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
		{
			return null;
		}
		Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
		int num = int.MaxValue;
		int num2 = currentPlayerId;
		foreach (int key in mActors.Keys)
		{
			if (key < num2)
			{
				num2 = key;
			}
			else if (key > currentPlayerId && key < num)
			{
				num = key;
			}
		}
		if (num != int.MaxValue)
		{
			return mActors[num];
		}
		return mActors[num2];
	}

	internal void InternalCacheProperties(Hashtable properties)
	{
		if (properties != null && properties.Count != 0 && !this.customProperties.Equals(properties))
		{
			if (properties.ContainsKey(byte.MaxValue))
			{
				this.nameField = (string)properties[byte.MaxValue];
			}
			this.customProperties.MergeStringKeys(properties);
			this.customProperties.StripKeysWithNullValues();
		}
	}

	internal void InternalChangeLocalID(int newID)
	{
		if (!this.isLocal)
		{
			Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
		}
		else
		{
			this.actorID = newID;
		}
	}

	public void SetCustomProperties(Hashtable propertiesToSet)
	{
		if (propertiesToSet != null)
		{
			this.customProperties.MergeStringKeys(propertiesToSet);
			this.customProperties.StripKeysWithNullValues();
			Hashtable actorProperties = propertiesToSet.StripToStringKeys();
			if (this.actorID > 0 && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetCustomPropertiesOfActor(this.actorID, actorProperties, broadcast: true, 0);
			}
			object[] parameters = new object[2] { this, propertiesToSet };
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, parameters);
		}
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.name))
		{
			return string.Format("#{0:00}{1}", this.ID, (!this.isMasterClient) ? string.Empty : "(master)");
		}
		return string.Format("'{0}'{1}", this.name, (!this.isMasterClient) ? string.Empty : "(master)");
	}

	public string ToStringFull()
	{
		return $"#{this.ID:00} '{this.name}' {this.customProperties.ToStringFull()}";
	}
}
