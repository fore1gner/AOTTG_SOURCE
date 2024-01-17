using SimpleJSONFixed;
using UnityEngine;

namespace Settings;

internal class FloatSetting : TypedSetting<float>
{
	public float MinValue = float.MinValue;

	public float MaxValue = float.MaxValue;

	public FloatSetting()
		: base(0f)
	{
	}

	public FloatSetting(float defaultValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
	{
		this.MinValue = minValue;
		this.MaxValue = maxValue;
		base.DefaultValue = this.SanitizeValue(defaultValue);
		this.SetDefault();
	}

	public override void DeserializeFromJsonObject(JSONNode json)
	{
		base.Value = json.AsFloat;
	}

	public override JSONNode SerializeToJsonObject()
	{
		return new JSONNumber(base.Value);
	}

	protected override float SanitizeValue(float value)
	{
		return Mathf.Clamp(value, this.MinValue, this.MaxValue);
	}
}
