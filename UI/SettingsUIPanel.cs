using Settings;

namespace UI;

internal class SettingsUIPanel : SettingsCategoryPanel
{
	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		string localeCategory = ((SettingsPopup)parent).LocaleCategory;
		string subCategory = "UI";
		_ = SettingsManager.UISettings;
		ElementStyle style = new ElementStyle(24, 200f, this.ThemePanel);
		ElementFactory.CreateDropdownSetting(base.DoublePanelLeft, style, SettingsManager.UISettings.UITheme, UIManager.GetLocale(localeCategory, subCategory, "Theme"), UIManager.GetUIThemes(), UIManager.GetLocaleCommon("RequireRestart"), 160f);
		ElementFactory.CreateSliderSetting(base.DoublePanelLeft, style, SettingsManager.UISettings.UIMasterScale, UIManager.GetLocale(localeCategory, subCategory, "UIScale"), "", 135f);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, SettingsManager.UISettings.GameFeed, UIManager.GetLocale(localeCategory, subCategory, "GameFeed"), UIManager.GetLocale(localeCategory, subCategory, "GameFeedTooltip"));
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, SettingsManager.UISettings.ShowEmotes, UIManager.GetLocale(localeCategory, subCategory, "ShowEmotes"));
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, SettingsManager.UISettings.ShowInterpolation, UIManager.GetLocale(localeCategory, subCategory, "ShowInterpolation"), UIManager.GetLocale(localeCategory, subCategory, "ShowInterpolationTooltip"));
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, SettingsManager.UISettings.HideNames, UIManager.GetLocale(localeCategory, subCategory, "HideNames"));
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, SettingsManager.UISettings.DisableNameColors, UIManager.GetLocale(localeCategory, subCategory, "DisableNameColors"));
		ElementFactory.CreateDropdownSetting(base.DoublePanelRight, style, SettingsManager.UISettings.CrosshairStyle, UIManager.GetLocale(localeCategory, subCategory, "CrosshairStyle"), UIManager.GetLocaleArray(localeCategory, subCategory, "CrosshairStyleOptions"), "", 200f);
		ElementFactory.CreateSliderSetting(base.DoublePanelRight, new ElementStyle(24, 150f, this.ThemePanel), SettingsManager.UISettings.CrosshairScale, UIManager.GetLocale(localeCategory, subCategory, "CrosshairScale"), "", 185f);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, SettingsManager.UISettings.ShowCrosshairDistance, UIManager.GetLocale(localeCategory, subCategory, "ShowCrosshairDistance"));
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, SettingsManager.UISettings.ShowCrosshairArrows, UIManager.GetLocale(localeCategory, subCategory, "ShowCrosshairArrows"));
		ElementFactory.CreateToggleGroupSetting(base.DoublePanelRight, style, SettingsManager.UISettings.Speedometer, UIManager.GetLocale(localeCategory, subCategory, "Speedometer"), UIManager.GetLocaleArray(localeCategory, subCategory, "SpeedometerOptions"));
	}
}
