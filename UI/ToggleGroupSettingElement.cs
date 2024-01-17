using System;
using System.Collections.Generic;
using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class ToggleGroupSettingElement : BaseSettingElement
{
	protected ToggleGroup _toggleGroup;

	protected GameObject _optionsPanel;

	protected string[] _options;

	protected List<Toggle> _toggles = new List<Toggle>();

	private float _checkMarkSizeMultiplier = 0.67f;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>
	{
		SettingType.String,
		SettingType.Int
	};

	public void Setup(BaseSetting setting, ElementStyle style, string title, string[] options, string tooltip, float elementWidth, float elementHeight)
	{
		if (options.Length == 0)
		{
			throw new ArgumentException("ToggleGroup cannot have 0 options.");
		}
		this._options = options;
		this._optionsPanel = base.transform.Find("Options").gameObject;
		this._toggleGroup = this._optionsPanel.GetComponent<ToggleGroup>();
		for (int i = 0; i < options.Length; i++)
		{
			this._toggles.Add(this.CreateOptionToggle(options[i], i, style, elementWidth, elementHeight));
		}
		base.gameObject.transform.Find("Label").GetComponent<LayoutElement>().preferredHeight = elementHeight;
		base.Setup(setting, style, title, tooltip);
	}

	protected Toggle CreateOptionToggle(string option, int index, ElementStyle style, float width, float height)
	{
		GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("ToggleGroupOption");
		gameObject.transform.SetParent(this._optionsPanel.transform, worldPositionStays: false);
		gameObject.transform.Find("Label").GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
		base.SetupLabel(gameObject.transform.Find("Label").gameObject, option, style.FontSize);
		LayoutElement component = gameObject.transform.Find("Background").GetComponent<LayoutElement>();
		RectTransform component2 = component.transform.Find("Checkmark").GetComponent<RectTransform>();
		component.preferredWidth = width;
		component.preferredHeight = height;
		component2.sizeDelta = new Vector2(width * this._checkMarkSizeMultiplier, height * this._checkMarkSizeMultiplier);
		Toggle component3 = gameObject.GetComponent<Toggle>();
		component3.group = this._toggleGroup;
		component3.isOn = false;
		component3.onValueChanged.AddListener(delegate(bool value)
		{
			this.OnValueChanged(option, index, value);
		});
		component3.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Toggle");
		component2.GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "ToggleFilledColor");
		return component3;
	}

	protected void OnValueChanged(string option, int index, bool value)
	{
		if (value)
		{
			if (base._settingType == SettingType.String)
			{
				((StringSetting)base._setting).Value = option;
			}
			else if (base._settingType == SettingType.Int)
			{
				((IntSetting)base._setting).Value = index;
			}
		}
	}

	public override void SyncElement()
	{
		this._toggleGroup.SetAllTogglesOff();
		if (base._settingType == SettingType.String)
		{
			int index = this.FindOptionIndex(((StringSetting)base._setting).Value);
			this._toggles[index].isOn = true;
		}
		else if (base._settingType == SettingType.Int)
		{
			this._toggles[((IntSetting)base._setting).Value].isOn = true;
		}
	}

	private int FindOptionIndex(string option)
	{
		for (int i = 0; i < this._options.Length; i++)
		{
			if (this._options[i] == option)
			{
				return i;
			}
		}
		throw new ArgumentOutOfRangeException("Option not found");
	}
}
