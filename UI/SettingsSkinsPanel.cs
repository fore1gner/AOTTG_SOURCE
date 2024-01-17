using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace UI;

internal class SettingsSkinsPanel : SettingsCategoryPanel
{
	protected Dictionary<string, ICustomSkinSettings> _settings = new Dictionary<string, ICustomSkinSettings>();

	protected override bool CategoryPanel => true;

	protected override string DefaultCategoryPanel => "Human";

	public void CreateCommonSettings(Transform panelLeft, Transform panelRight)
	{
		ElementStyle style = new ElementStyle(24, 140f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(24, 200f, this.ThemePanel);
		ICustomSkinSettings currentSettings = this.GetCurrentSettings();
		string value = base._currentCategoryPanelName.Value;
		string[] options = new string[7] { "Human", "Titan", "Shifter", "Skybox", "Forest", "City", "Custom level" };
		ElementFactory.CreateDropdownSetting(panelLeft, style, base._currentCategoryPanelName, UIManager.GetLocaleCommon("Category"), options, "", 260f, 40f, 300f, null, delegate
		{
			base.RebuildCategoryPanel();
		});
		string subCategory = "Skins.Common";
		string localeCategory = ((SettingsPopup)base.Parent).LocaleCategory;
		ElementFactory.CreateDropdownSetting(panelLeft, style, currentSettings.GetSelectedSetIndex(), UIManager.GetLocale(localeCategory, subCategory, "Set"), currentSettings.GetSetNames(), "", 260f, 40f, 300f, null, delegate
		{
			base.RebuildCategoryPanel();
		});
		GameObject gameObject = ElementFactory.CreateHorizontalGroup(panelLeft, 10f, TextAnchor.UpperRight);
		string[] array = new string[4] { "Create", "Delete", "Rename", "Copy" };
		foreach (string button in array)
		{
			ElementFactory.CreateDefaultButton(gameObject.transform, style, UIManager.GetLocaleCommon(button), 0f, 0f, delegate
			{
				this.OnSkinsPanelButtonClick(button);
			});
		}
		ElementFactory.CreateToggleSetting(panelRight, style2, currentSettings.GetSkinsEnabled(), value + " " + UIManager.GetLocale(localeCategory, "Skins.Common", "SkinsEnabled"));
		ElementFactory.CreateToggleSetting(panelRight, style2, currentSettings.GetSkinsLocal(), value + " " + UIManager.GetLocale(localeCategory, "Skins.Common", "SkinsLocal"), UIManager.GetLocale(localeCategory, "Skins.Common", "SkinsLocalTooltip"));
	}

	private void OnSkinsPanelButtonClick(string name)
	{
		SetNamePopup setNamePopup = ((SettingsPopup)base.Parent).SetNamePopup;
		ICustomSkinSettings customSkinSettings = this._settings[base._currentCategoryPanelName.Value];
		switch (name)
		{
		case "Create":
			setNamePopup.Show("New set", delegate
			{
				this.OnSkinsSetOperationFinish(name);
			}, UIManager.GetLocaleCommon("Create"));
			break;
		case "Delete":
			if (customSkinSettings.CanDeleteSelectedSet())
			{
				UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), delegate
				{
					this.OnSkinsSetOperationFinish(name);
				}, UIManager.GetLocaleCommon("Delete"));
			}
			break;
		case "Rename":
		{
			string value = customSkinSettings.GetSelectedSet().Name.Value;
			setNamePopup.Show(value, delegate
			{
				this.OnSkinsSetOperationFinish(name);
			}, UIManager.GetLocaleCommon("Rename"));
			break;
		}
		case "Copy":
			setNamePopup.Show("New set", delegate
			{
				this.OnSkinsSetOperationFinish(name);
			}, UIManager.GetLocaleCommon("Copy"));
			break;
		}
	}

	private void OnSkinsSetOperationFinish(string name)
	{
		SetNamePopup setNamePopup = ((SettingsPopup)base.Parent).SetNamePopup;
		ICustomSkinSettings currentSettings = this.GetCurrentSettings();
		switch (name)
		{
		case "Create":
			currentSettings.CreateSet(setNamePopup.NameSetting.Value);
			currentSettings.GetSelectedSetIndex().Value = currentSettings.GetSets().GetCount() - 1;
			break;
		case "Delete":
			currentSettings.DeleteSelectedSet();
			currentSettings.GetSelectedSetIndex().Value = 0;
			break;
		case "Rename":
			currentSettings.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
			break;
		case "Copy":
			currentSettings.CopySelectedSet(setNamePopup.NameSetting.Value);
			currentSettings.GetSelectedSetIndex().Value = currentSettings.GetSets().GetCount() - 1;
			break;
		}
		base.RebuildCategoryPanel();
	}

	public void CreateSkinListStringSettings(ListSetting<StringSetting> list, Transform panel, string title)
	{
		ElementStyle style = new ElementStyle(24, 0f, this.ThemePanel);
		ElementFactory.CreateDefaultLabel(panel, style, title);
		foreach (StringSetting item in list.Value)
		{
			ElementFactory.CreateInputSetting(panel, style, item, string.Empty, "", 420f);
		}
	}

	public void CreateSkinStringSettings(Transform panelLeft, Transform panelRight, float titleWidth = 140f, float elementWidth = 260f, int leftCount = 0)
	{
		ElementStyle style = new ElementStyle(24, titleWidth, this.ThemePanel);
		BaseSetSetting selectedSet = this.GetCurrentSettings().GetSelectedSet();
		string localeCategory = ((SettingsPopup)base.Parent).LocaleCategory;
		string text = "Skins." + base._currentCategoryPanelName.Value;
		int num = 1;
		foreach (DictionaryEntry setting in selectedSet.Settings)
		{
			BaseSetting baseSetting = (BaseSetting)setting.Value;
			string text2 = (string)setting.Key;
			Transform parent = ((num <= leftCount) ? panelLeft : panelRight);
			if ((baseSetting.GetType() == typeof(StringSetting) || baseSetting.GetType() == typeof(FloatSetting)) && text2 != "Name")
			{
				string subCategory = text;
				if (text2 == "Ground")
				{
					subCategory = "Skins.Common";
				}
				ElementFactory.CreateInputSetting(parent, style, baseSetting, UIManager.GetLocale(localeCategory, subCategory, text2), "", elementWidth);
				num++;
			}
		}
	}

	public ICustomSkinSettings GetCurrentSettings()
	{
		return this._settings[base._currentCategoryPanelName.Value];
	}

	protected override void RegisterCategoryPanels()
	{
		base._categoryPanelTypes.Add("Human", typeof(SettingsSkinsHumanPanel));
		base._categoryPanelTypes.Add("Titan", typeof(SettingsSkinsTitanPanel));
		base._categoryPanelTypes.Add("Forest", typeof(SettingsSkinsForestPanel));
		base._categoryPanelTypes.Add("City", typeof(SettingsSkinsCityPanel));
		base._categoryPanelTypes.Add("Shifter", typeof(SettingsSkinsDefaultPanel));
		base._categoryPanelTypes.Add("Skybox", typeof(SettingsSkinsDefaultPanel));
		base._categoryPanelTypes.Add("Custom level", typeof(SettingsSkinsDefaultPanel));
		this._settings.Add("Human", SettingsManager.CustomSkinSettings.Human);
		this._settings.Add("Titan", SettingsManager.CustomSkinSettings.Titan);
		this._settings.Add("Forest", SettingsManager.CustomSkinSettings.Forest);
		this._settings.Add("City", SettingsManager.CustomSkinSettings.City);
		this._settings.Add("Shifter", SettingsManager.CustomSkinSettings.Shifter);
		this._settings.Add("Skybox", SettingsManager.CustomSkinSettings.Skybox);
		this._settings.Add("Custom level", SettingsManager.CustomSkinSettings.CustomLevel);
	}
}
