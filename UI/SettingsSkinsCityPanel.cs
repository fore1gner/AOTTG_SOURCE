using Settings;

namespace UI;

internal class SettingsSkinsCityPanel : SettingsCategoryPanel
{
	protected override float VerticalSpacing => 20f;

	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsSkinsPanel obj = (SettingsSkinsPanel)parent;
		SettingsPopup obj2 = (SettingsPopup)obj.Parent;
		CityCustomSkinSet cityCustomSkinSet = (CityCustomSkinSet)SettingsManager.CustomSkinSettings.City.GetSelectedSet();
		string localeCategory = obj2.LocaleCategory;
		string subCategory = "Skins.City";
		ElementStyle style = new ElementStyle(24, 140f, this.ThemePanel);
		obj.CreateCommonSettings(base.DoublePanelLeft, base.DoublePanelRight);
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, cityCustomSkinSet.Ground, UIManager.GetLocale(localeCategory, "Skins.Common", "Ground"), "", 260f);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, cityCustomSkinSet.Wall, UIManager.GetLocale(localeCategory, subCategory, "Wall"), "", 260f);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, cityCustomSkinSet.Gate, UIManager.GetLocale(localeCategory, subCategory, "Gate"), "", 260f);
		obj.CreateSkinListStringSettings(cityCustomSkinSet.Houses, base.DoublePanelRight, UIManager.GetLocale(localeCategory, subCategory, "Houses"));
	}
}
