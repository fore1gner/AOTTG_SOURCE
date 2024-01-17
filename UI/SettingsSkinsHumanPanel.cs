using Settings;

namespace UI;

internal class SettingsSkinsHumanPanel : SettingsCategoryPanel
{
	protected override float VerticalSpacing => 20f;

	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsSkinsPanel obj = (SettingsSkinsPanel)parent;
		SettingsPopup settingsPopup = (SettingsPopup)obj.Parent;
		HumanCustomSkinSettings human = SettingsManager.CustomSkinSettings.Human;
		obj.CreateCommonSettings(base.DoublePanelLeft, base.DoublePanelRight);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, new ElementStyle(24, 200f, this.ThemePanel), human.GasEnabled, UIManager.GetLocale(settingsPopup.LocaleCategory, "Skins.Human", "GasEnabled"));
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, new ElementStyle(24, 200f, this.ThemePanel), human.HookEnabled, UIManager.GetLocale(settingsPopup.LocaleCategory, "Skins.Human", "HookEnabled"));
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		obj.CreateSkinStringSettings(base.DoublePanelLeft, base.DoublePanelRight, 200f, 200f, 9);
	}
}
