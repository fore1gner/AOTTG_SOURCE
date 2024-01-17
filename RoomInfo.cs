using ExitGames.Client.Photon;

public class RoomInfo
{
	protected bool autoCleanUpField = PhotonNetwork.autoCleanUpPlayerObjects;

	private Hashtable customPropertiesField = new Hashtable();

	protected byte maxPlayersField;

	protected string nameField;

	protected bool openField = true;

	protected bool visibleField = true;

	public Hashtable customProperties => this.customPropertiesField;

	public bool isLocalClientInside { get; set; }

	public byte maxPlayers => this.maxPlayersField;

	public string name => this.nameField;

	public bool open => this.openField;

	public int playerCount { get; private set; }

	public bool removedFromList { get; internal set; }

	public bool visible => this.visibleField;

	protected internal RoomInfo(string roomName, Hashtable properties)
	{
		this.CacheProperties(properties);
		this.nameField = roomName;
	}

	protected internal void CacheProperties(Hashtable propertiesToCache)
	{
		if (propertiesToCache == null || propertiesToCache.Count == 0 || this.customPropertiesField.Equals(propertiesToCache))
		{
			return;
		}
		if (propertiesToCache.ContainsKey((byte)251))
		{
			this.removedFromList = (bool)propertiesToCache[(byte)251];
			if (this.removedFromList)
			{
				return;
			}
		}
		if (propertiesToCache.ContainsKey(byte.MaxValue))
		{
			this.maxPlayersField = (byte)propertiesToCache[byte.MaxValue];
		}
		if (propertiesToCache.ContainsKey((byte)253))
		{
			this.openField = (bool)propertiesToCache[(byte)253];
		}
		if (propertiesToCache.ContainsKey((byte)254))
		{
			this.visibleField = (bool)propertiesToCache[(byte)254];
		}
		if (propertiesToCache.ContainsKey((byte)252))
		{
			this.playerCount = (byte)propertiesToCache[(byte)252];
		}
		if (propertiesToCache.ContainsKey((byte)249))
		{
			this.autoCleanUpField = (bool)propertiesToCache[(byte)249];
		}
		this.customPropertiesField.MergeStringKeys(propertiesToCache);
	}

	public override bool Equals(object p)
	{
		if (p is Room room)
		{
			return this.nameField.Equals(room.nameField);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.nameField.GetHashCode();
	}

	public override string ToString()
	{
		object[] args = new object[5]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.playerCount
		};
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", args);
	}

	public string ToStringFull()
	{
		object[] args = new object[6]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.playerCount,
			this.customPropertiesField.ToStringFull()
		};
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", args);
	}
}
