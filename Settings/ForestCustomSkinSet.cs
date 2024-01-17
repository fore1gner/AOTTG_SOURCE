namespace Settings;

internal class ForestCustomSkinSet : BaseSetSetting
{
	public BoolSetting RandomizedPairs = new BoolSetting(defaultValue: false);

	public ListSetting<StringSetting> TreeTrunks = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 8);

	public ListSetting<StringSetting> TreeLeafs = new ListSetting<StringSetting>(new StringSetting(string.Empty, 200), 8);

	public StringSetting Ground = new StringSetting(string.Empty, 200);

	protected override bool Validate()
	{
		if (this.TreeTrunks.Value.Count == 8)
		{
			return this.TreeLeafs.Value.Count == 8;
		}
		return false;
	}
}
