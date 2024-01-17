using Settings;

namespace UI;

internal class SettingsGraphicsPanel : SettingsCategoryPanel
{
	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		string localeCategory = ((SettingsPopup)parent).LocaleCategory;
		string subCategory = "Graphics";
		GraphicsSettings graphicsSettings = SettingsManager.GraphicsSettings;
		ElementStyle style = new ElementStyle(24, 200f, this.ThemePanel);
		ElementFactory.CreateDropdownSetting(base.DoublePanelLeft, style, graphicsSettings.OverallQuality, UIManager.GetLocale(localeCategory, subCategory, "OverallQuality"), UIManager.GetLocaleArray(localeCategory, subCategory, "OverallQualityOptions"), "", 200f);
		ElementFactory.CreateDropdownSetting(base.DoublePanelLeft, style, graphicsSettings.TextureQuality, UIManager.GetLocale(localeCategory, subCategory, "TextureQuality"), UIManager.GetLocaleArray(localeCategory, subCategory, "TextureQualityOptions"), "", 200f);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, graphicsSettings.VSync, UIManager.GetLocale(localeCategory, subCategory, "VSync"));
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, graphicsSettings.FPSCap, UIManager.GetLocale(localeCategory, subCategory, "FPSCap"), "", 100f);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, graphicsSettings.ShowFPS, UIManager.GetLocale(localeCategory, subCategory, "ShowFPS"));
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, graphicsSettings.ExclusiveFullscreen, UIManager.GetLocale(localeCategory, subCategory, "ExclusiveFullscreen"), UIManager.GetLocale(localeCategory, subCategory, "ExclusiveFullscreenTooltip"));
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, graphicsSettings.InterpolationEnabled, UIManager.GetLocale(localeCategory, subCategory, "InterpolationEnabled"), UIManager.GetLocale(localeCategory, subCategory, "InterpolationEnabledTooltip"));
		ElementFactory.CreateDropdownSetting(base.DoublePanelRight, style, graphicsSettings.WeatherEffects, UIManager.GetLocale(localeCategory, subCategory, "WeatherEffects"), UIManager.GetLocaleArray(localeCategory, subCategory, "WeatherEffectsOptions"), "", 200f);
		ElementFactory.CreateDropdownSetting(base.DoublePanelRight, style, graphicsSettings.AntiAliasing, UIManager.GetLocale(localeCategory, subCategory, "AntiAliasing"), UIManager.GetLocaleArray(localeCategory, subCategory, "AntiAliasingOptions"), "", 200f);
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, graphicsSettings.RenderDistance, UIManager.GetLocale(localeCategory, subCategory, "RenderDistance"), UIManager.GetLocale(localeCategory, subCategory, "RenderDistanceTooltip"), 100f);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, graphicsSettings.AnimatedIntro, UIManager.GetLocale(localeCategory, subCategory, "AnimatedIntro"));
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, graphicsSettings.WindEffectEnabled, UIManager.GetLocale(localeCategory, subCategory, "WindEffectEnabled"));
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, graphicsSettings.WeaponTrailEnabled, UIManager.GetLocale(localeCategory, subCategory, "WeaponTrailEnabled"));
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, graphicsSettings.BlurEnabled, UIManager.GetLocale(localeCategory, subCategory, "BlurEnabled"));
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, graphicsSettings.MipmapEnabled, UIManager.GetLocale(localeCategory, subCategory, "MipmapEnabled"), UIManager.GetLocale(localeCategory, subCategory, "MipmapEnabledTooltip"));
	}
}
