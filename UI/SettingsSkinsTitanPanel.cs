using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace UI;

internal class SettingsSkinsTitanPanel : SettingsCategoryPanel
{
	protected override float VerticalSpacing => 20f;

	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsSkinsPanel settingsSkinsPanel = (SettingsSkinsPanel)parent;
		SettingsPopup obj = (SettingsPopup)settingsSkinsPanel.Parent;
		TitanCustomSkinSet titanCustomSkinSet = (TitanCustomSkinSet)SettingsManager.CustomSkinSettings.Titan.GetSelectedSet();
		string localeCategory = obj.LocaleCategory;
		string subCategory = "Skins.Titan";
		settingsSkinsPanel.CreateCommonSettings(base.DoublePanelLeft, base.DoublePanelRight);
		ElementStyle elementStyle = new ElementStyle(24, 200f, this.ThemePanel);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, elementStyle, titanCustomSkinSet.RandomizedPairs, UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairs"), UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairsTooltip"));
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateDefaultLabel(base.DoublePanelLeft, elementStyle, UIManager.GetLocale(localeCategory, subCategory, "Hairs"));
		List<string> list = new List<string> { "Random" };
		for (int i = 0; i < 10; i++)
		{
			list.Add("Hair " + i);
		}
		string[] options = list.ToArray();
		elementStyle.TitleWidth = 0f;
		for (int j = 0; j < titanCustomSkinSet.Hairs.GetCount(); j++)
		{
			GameObject obj2 = ElementFactory.CreateHorizontalGroup(base.DoublePanelLeft, 20f);
			ElementFactory.CreateInputSetting(obj2.transform, elementStyle, titanCustomSkinSet.Hairs.GetItemAt(j), string.Empty, "", 260f);
			ElementFactory.CreateDropdownSetting(obj2.transform, elementStyle, titanCustomSkinSet.HairModels.GetItemAt(j), string.Empty, options);
		}
		settingsSkinsPanel.CreateSkinListStringSettings(titanCustomSkinSet.Bodies, base.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "Bodies"));
		base.CreateHorizontalDivider(base.DoublePanelRight);
		settingsSkinsPanel.CreateSkinListStringSettings(titanCustomSkinSet.Eyes, base.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "Eyes"));
	}
}
