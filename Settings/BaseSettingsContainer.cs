using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using SimpleJSONFixed;

namespace Settings;

internal abstract class BaseSettingsContainer : BaseSetting
{
	public OrderedDictionary Settings = new OrderedDictionary();

	public BaseSettingsContainer()
	{
		this.Setup();
	}

	protected virtual void Setup()
	{
		this.RegisterSettings();
		this.Apply();
	}

	protected void RegisterSettings()
	{
		foreach (FieldInfo item in from field in base.GetType().GetFields()
			where field.FieldType.IsSubclassOf(typeof(BaseSetting))
			select field)
		{
			this.Settings.Add(item.Name, (BaseSetting)item.GetValue(this));
		}
	}

	public override void SetDefault()
	{
		foreach (BaseSetting value in this.Settings.Values)
		{
			value.SetDefault();
		}
	}

	public virtual void Apply()
	{
	}

	public override JSONNode SerializeToJsonObject()
	{
		JSONObject jSONObject = new JSONObject();
		foreach (string key in this.Settings.Keys)
		{
			jSONObject.Add(key, ((BaseSetting)this.Settings[key]).SerializeToJsonObject());
		}
		return jSONObject;
	}

	public override void DeserializeFromJsonObject(JSONNode json)
	{
		JSONObject jSONObject = (JSONObject)json;
		foreach (string key in this.Settings.Keys)
		{
			if (jSONObject[key] != null)
			{
				((BaseSetting)this.Settings[key]).DeserializeFromJsonObject(jSONObject[key]);
			}
		}
		if (!this.Validate())
		{
			this.SetDefault();
		}
	}

	protected virtual bool Validate()
	{
		return true;
	}
}
