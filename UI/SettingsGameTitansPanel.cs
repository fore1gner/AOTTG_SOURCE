using Settings;

namespace UI;

internal class SettingsGameTitansPanel : SettingsCategoryPanel
{
	protected override bool ScrollBar => true;

	protected override float VerticalSpacing => 20f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsGamePanel obj = (SettingsGamePanel)parent;
		_ = (SettingsPopup)obj.Parent;
		obj.CreateGategoryDropdown(base.DoublePanelLeft);
		float elementWidth = 120f;
		ElementStyle style = new ElementStyle(24, 240f, this.ThemePanel);
		LegacyGameSettings legacyGameSettingsUI = SettingsManager.LegacyGameSettingsUI;
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanNumberEnabled, "Custom titan number");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanNumber, "Titan amount", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSpawnEnabled, "Custom titan spawns", "Spawn rates must add up to 100.");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSpawnNormal, "Normal", "", elementWidth);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSpawnAberrant, "Aberrant", "", elementWidth);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSpawnJumper, "Jumper", "", elementWidth);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSpawnCrawler, "Crawler", "", elementWidth);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSpawnPunk, "Punk", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSizeEnabled, "Custom titan sizes");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSizeMin, "Minimum size", "", elementWidth);
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanSizeMax, "Maximum size", "", elementWidth);
		ElementFactory.CreateToggleGroupSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanHealthMode, "Titan health", new string[3] { "Off", "Fixed", "Scaled" });
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanHealthMin, "Minimum health", "", elementWidth);
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanHealthMax, "Maximum health", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanArmorEnabled, "Titan armor");
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanArmor, "Armor amount", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanExplodeEnabled, "Titan explode");
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanExplodeRadius, "Explode radius", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.RockThrowEnabled, "Punk rock throwing");
	}
}
