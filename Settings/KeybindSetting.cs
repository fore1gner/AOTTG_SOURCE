using System.Collections.Generic;
using SimpleJSONFixed;
using UnityEngine;

namespace Settings;

internal class KeybindSetting : BaseSetting
{
	public List<InputKey> InputKeys = new List<InputKey>();

	protected string[] _defaultKeyStrings;

	public KeybindSetting(string[] defaultKeyStrings)
	{
		this._defaultKeyStrings = defaultKeyStrings;
		this.SetDefault();
	}

	public override void SetDefault()
	{
		this.LoadFromStringArray(this._defaultKeyStrings);
	}

	protected void LoadFromStringArray(string[] keyStrings)
	{
		this.InputKeys.Clear();
		for (int i = 0; i < keyStrings.Length; i++)
		{
			InputKey item = new InputKey(keyStrings[i]);
			this.InputKeys.Add(item);
		}
	}

	public override string ToString()
	{
		List<string> list = new List<string>();
		foreach (InputKey ınputKey in this.InputKeys)
		{
			if (!ınputKey.IsNone())
			{
				list.Add(ınputKey.ToString());
			}
		}
		if (list.Count == 0)
		{
			return "None";
		}
		return string.Join(" / ", list.ToArray());
	}

	public bool Contains(InputKey key)
	{
		foreach (InputKey ınputKey in this.InputKeys)
		{
			if (ınputKey.Equals(key))
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(KeyCode key)
	{
		foreach (InputKey ınputKey in this.InputKeys)
		{
			if (ınputKey.MatchesKeyCode(key))
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKeyDown()
	{
		foreach (InputKey ınputKey in this.InputKeys)
		{
			if (ınputKey.GetKeyDown())
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKey()
	{
		foreach (InputKey ınputKey in this.InputKeys)
		{
			if (ınputKey.GetKey())
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKeyUp()
	{
		foreach (InputKey ınputKey in this.InputKeys)
		{
			if (ınputKey.GetKeyUp())
			{
				return true;
			}
		}
		return false;
	}

	public override JSONNode SerializeToJsonObject()
	{
		JSONArray jSONArray = new JSONArray();
		foreach (InputKey ınputKey in this.InputKeys)
		{
			jSONArray.Add(new JSONString(ınputKey.ToString()));
		}
		return jSONArray;
	}

	public override void DeserializeFromJsonObject(JSONNode json)
	{
		List<string> list = new List<string>();
		JSONNode.Enumerator enumerator = json.AsArray.GetEnumerator();
		while (enumerator.MoveNext())
		{
			JSONString jSONString = (JSONString)(JSONNode)enumerator.Current;
			list.Add(jSONString.Value);
		}
		this.LoadFromStringArray(list.ToArray());
	}
}
