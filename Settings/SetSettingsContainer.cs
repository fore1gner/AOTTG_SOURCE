using System;
using System.Collections.Generic;

namespace Settings;

internal class SetSettingsContainer<T> : BaseSettingsContainer, ISetSettingsContainer where T : BaseSetSetting, new()
{
	public IntSetting SelectedSetIndex = new IntSetting(0, 0);

	public ListSetting<T> Sets = new ListSetting<T>(new T());

	protected override bool Validate()
	{
		return this.Sets.GetCount() > 0;
	}

	public BaseSetSetting GetSelectedSet()
	{
		int value = this.SelectedSetIndex.Value;
		value = Math.Min(value, this.Sets.GetCount() - 1);
		value = Math.Max(value, 0);
		return (BaseSetSetting)this.Sets.GetItemAt(value);
	}

	public IntSetting GetSelectedSetIndex()
	{
		return this.SelectedSetIndex;
	}

	public void CreateSet(string name)
	{
		T item = new T
		{
			Name = 
			{
				Value = name
			}
		};
		this.Sets.Value.Add(item);
	}

	public void CopySelectedSet(string name)
	{
		T val = new T();
		val.Copy(this.GetSelectedSet());
		val.Name.Value = name;
		val.Preset.Value = false;
		this.Sets.Value.Add(val);
	}

	public bool CanDeleteSelectedSet()
	{
		if (this.Sets.GetCount() > 1)
		{
			return this.CanEditSelectedSet();
		}
		return false;
	}

	public bool CanEditSelectedSet()
	{
		return !this.GetSelectedSet().Preset.Value;
	}

	public void DeleteSelectedSet()
	{
		this.Sets.Value.Remove((T)this.GetSelectedSet());
	}

	public IListSetting GetSets()
	{
		return this.Sets;
	}

	public void SetPresetsFromJsonString(string json)
	{
		SetSettingsContainer<T> setSettingsContainer = new SetSettingsContainer<T>();
		setSettingsContainer.DeserializeFromJsonString(json);
		this.Sets.Value.RemoveAll((T x) => x.Preset.Value);
		for (int i = 0; i < setSettingsContainer.Sets.Value.Count; i++)
		{
			setSettingsContainer.Sets.Value[i].Preset.Value = true;
			this.Sets.Value.Insert(i, setSettingsContainer.Sets.Value[i]);
		}
	}

	public string[] GetSetNames()
	{
		List<string> list = new List<string>();
		foreach (BaseSetSetting ıtem in this.Sets.GetItems())
		{
			list.Add(ıtem.Name.Value);
		}
		return list.ToArray();
	}
}
