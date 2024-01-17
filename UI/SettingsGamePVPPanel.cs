using Settings;

namespace UI;

internal class SettingsGamePVPPanel : SettingsCategoryPanel
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
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.PointModeEnabled, "Point mode", "End game after player or team reaches certain number of points.");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.PointModeAmount, "Point amount", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.BombModeEnabled, "Bomb mode");
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.BombModeCeiling, "Bomb ceiling");
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.BombModeInfiniteGas, "Bomb infinite gas");
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, legacyGameSettingsUI.BombModeDisableTitans, "Bomb disable titans");
		ElementFactory.CreateToggleGroupSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TeamMode, "Team mode", new string[4] { "Off", "No sort", "Size lock", "Skill lock" });
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.InfectionModeEnabled, "Infection mode");
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, legacyGameSettingsUI.InfectionModeAmount, "Starting titans", "", elementWidth);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateToggleGroupSetting(base.DoublePanelRight, style, legacyGameSettingsUI.BladePVP, "Blade/AHSS PVP", new string[3] { "Off", "Teams", "FFA" });
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.FriendlyMode, "Friendly mode", "Prevent normal AHSS/Blade PVP.");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.AHSSAirReload, "AHSS air reload");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, legacyGameSettingsUI.CannonsFriendlyFire, "Cannons friendly fire");
	}
}
