using SimpleJSONFixed;

namespace Settings;

internal class NameSetting : StringSetting
{
	public int MaxStrippedLength = int.MaxValue;

	public NameSetting()
		: base(string.Empty)
	{
	}

	public NameSetting(string defaultValue, int maxLength = int.MaxValue, int maxStrippedLength = int.MaxValue)
		: base(defaultValue)
	{
		base.MaxLength = maxLength;
		this.MaxStrippedLength = maxStrippedLength;
	}

	public override void DeserializeFromJsonObject(JSONNode json)
	{
		base.Value = json.Value;
	}

	public override JSONNode SerializeToJsonObject()
	{
		return new JSONString(base.Value);
	}

	protected override string SanitizeValue(string value)
	{
		if (value.Length > base.MaxLength)
		{
			return value.Substring(0, base.MaxLength);
		}
		string text = value.StripHex();
		if (text.Length > this.MaxStrippedLength)
		{
			return text.Substring(0, this.MaxStrippedLength);
		}
		return value;
	}
}
