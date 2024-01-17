using Settings;

namespace UI;

internal class SettingsGameMiscPanel : SettingsCategoryPanel
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
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanPerWavesEnabled, "Custom titans/wave");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanPerWaves, "Titan amount", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanMaxWavesEnabled, "Custom max waves");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.TitanMaxWaves, "Wave amount", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.EndlessRespawnEnabled, "Endless respawn");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.EndlessRespawnTime, "Respawn time", "", elementWidth);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.PunksEveryFive, "Punks every 5 waves");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.GlobalMinimapDisable, "Global minimap disable");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.PreserveKDR, "Preserve KDR", "Preserve player stats when they leave and rejoin the room.");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.RacingEndless, "Endless racing", "Racing round continues even if someone finishes.");
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, legacyGameSettingsUI.RacingStartTime, "Racing start time", "", elementWidth);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.KickShifters, "Kick shifters");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.AllowHorses, "Allow horses");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.GlobalHideNames, "Global hide names");
		ElementFactory.CreateInputSetting(base.DoublePanelRight, new ElementStyle(24, 160f, this.ThemePanel), legacyGameSettingsUI.Motd, "MOTD", "", 200f);
	}
}
