using System;
using SimpleJSONFixed;

namespace Settings;

internal class IntSetting : TypedSetting<int>
{
	public int MinValue = int.MinValue;

	public int MaxValue = int.MaxValue;

	public IntSetting()
		: base(0)
	{
	}

	public IntSetting(int defaultValue, int minValue = int.MinValue, int maxValue = int.MaxValue)
	{
		this.MinValue = minValue;
		this.MaxValue = maxValue;
		base.DefaultValue = this.SanitizeValue(defaultValue);
		this.SetDefault();
	}

	public override void DeserializeFromJsonObject(JSONNode json)
	{
		base.Value = json.AsInt;
	}

	public override JSONNode SerializeToJsonObject()
	{
		return new JSONNumber(base.Value);
	}

	protected override int SanitizeValue(int value)
	{
		value = Math.Min(value, this.MaxValue);
		value = Math.Max(value, this.MinValue);
		return value;
	}
}
