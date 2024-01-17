namespace Settings;

internal class TitanCustomSkinSet : BaseSetSetting
{
	public BoolSetting RandomizedPairs = new BoolSetting(defaultValue: false);

	public ListSetting<StringSetting> Hairs = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 5);

	public ListSetting<IntSetting> HairModels = new ListSetting<IntSetting>(new IntSetting(0, 0), 5);

	public ListSetting<StringSetting> Bodies = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 5);

	public ListSetting<StringSetting> Eyes = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 5);

	protected override bool Validate()
	{
		if (this.Hairs.Value.Count == 5 && this.HairModels.Value.Count == 5 && this.Bodies.Value.Count == 5)
		{
			return this.Eyes.Value.Count == 5;
		}
		return false;
	}
}
