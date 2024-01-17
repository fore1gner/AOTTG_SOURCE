public class TypedLobby
{
	public static readonly TypedLobby Default = new TypedLobby();

	public string Name;

	public LobbyType Type;

	public bool IsDefault
	{
		get
		{
			if (this.Type == LobbyType.Default)
			{
				return string.IsNullOrEmpty(this.Name);
			}
			return false;
		}
	}

	public TypedLobby()
	{
		this.Name = string.Empty;
		this.Type = LobbyType.Default;
	}

	public TypedLobby(string name, LobbyType type)
	{
		this.Name = name;
		this.Type = type;
	}

	public override string ToString()
	{
		return $"Lobby '{this.Name}'[{this.Type}]";
	}
}
