using Settings;

namespace UI;

internal class SettingsSkinsForestPanel : SettingsCategoryPanel
{
	protected override float VerticalSpacing => 20f;

	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsSkinsPanel obj = (SettingsSkinsPanel)parent;
		SettingsPopup obj2 = (SettingsPopup)obj.Parent;
		ForestCustomSkinSet forestCustomSkinSet = (ForestCustomSkinSet)SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
		string localeCategory = obj2.LocaleCategory;
		string subCategory = "Skins.Forest";
		obj.CreateCommonSettings(base.DoublePanelLeft, base.DoublePanelRight);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, new ElementStyle(24, 200f, this.ThemePanel), forestCustomSkinSet.RandomizedPairs, UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairs"), UIManager.GetLocale(localeCategory, "Skins.Common", "RandomizedPairsTooltip"));
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateInputSetting(base.DoublePanelRight, new ElementStyle(24, 140f, this.ThemePanel), forestCustomSkinSet.Ground, UIManager.GetLocale(localeCategory, "Skins.Common", "Ground"), "", 260f);
		obj.CreateSkinListStringSettings(forestCustomSkinSet.TreeTrunks, base.DoublePanelLeft, UIManager.GetLocale(localeCategory, subCategory, "TreeTrunks"));
		obj.CreateSkinListStringSettings(forestCustomSkinSet.TreeLeafs, base.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "TreeLeafs"));
	}
}
