using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	public string keyName;

	private UICheckbox mCheck;

	private UIPopupList mList;

	private string key
	{
		get
		{
			if (string.IsNullOrEmpty(this.keyName))
			{
				return "NGUI State: " + base.name;
			}
			return this.keyName;
		}
	}

	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.mCheck = base.GetComponent<UICheckbox>();
		if (this.mList != null)
		{
			this.mList.onSelectionChange = (UIPopupList.OnSelectionChange)Delegate.Combine(this.mList.onSelectionChange, new UIPopupList.OnSelectionChange(SaveSelection));
		}
		if (this.mCheck != null)
		{
			this.mCheck.onStateChange = (UICheckbox.OnStateChange)Delegate.Combine(this.mCheck.onStateChange, new UICheckbox.OnStateChange(SaveState));
		}
	}

	private void OnDestroy()
	{
		if (this.mCheck != null)
		{
			this.mCheck.onStateChange = (UICheckbox.OnStateChange)Delegate.Remove(this.mCheck.onStateChange, new UICheckbox.OnStateChange(SaveState));
		}
		if (this.mList != null)
		{
			this.mList.onSelectionChange = (UIPopupList.OnSelectionChange)Delegate.Remove(this.mList.onSelectionChange, new UIPopupList.OnSelectionChange(SaveSelection));
		}
	}

	private void OnDisable()
	{
		if (!(this.mCheck == null) || !(this.mList == null))
		{
			return;
		}
		UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			UICheckbox uICheckbox = componentsInChildren[i];
			if (uICheckbox.isChecked)
			{
				this.SaveSelection(uICheckbox.name);
				break;
			}
		}
	}

	private void OnEnable()
	{
		if (this.mList != null)
		{
			string @string = PlayerPrefs.GetString(this.key);
			if (!string.IsNullOrEmpty(@string))
			{
				this.mList.selection = @string;
			}
			return;
		}
		if (this.mCheck != null)
		{
			this.mCheck.isChecked = PlayerPrefs.GetInt(this.key, 1) != 0;
			return;
		}
		string string2 = PlayerPrefs.GetString(this.key);
		UICheckbox[] componentsInChildren = base.GetComponentsInChildren<UICheckbox>(includeInactive: true);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			UICheckbox obj = componentsInChildren[i];
			obj.isChecked = obj.name == string2;
		}
	}

	private void SaveSelection(string selection)
	{
		PlayerPrefs.SetString(this.key, selection);
	}

	private void SaveState(bool state)
	{
		PlayerPrefs.SetInt(this.key, state ? 1 : 0);
	}
}
