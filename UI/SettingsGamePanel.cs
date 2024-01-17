using Settings;
using UnityEngine;

namespace UI;

internal class SettingsGamePanel : SettingsCategoryPanel
{
	protected override bool CategoryPanel => true;

	protected override string DefaultCategoryPanel => "Titans";

	public void CreateGategoryDropdown(Transform panel, bool includeReset = true, float elementWidth = 260f)
	{
		ElementStyle style = new ElementStyle(24, 140f, this.ThemePanel);
		string[] options = new string[4] { "Titans", "PVP", "Misc", "Weather" };
		ElementFactory.CreateDropdownSetting(panel, style, base._currentCategoryPanelName, "Category", options, "", elementWidth, 40f, 300f, null, delegate
		{
			base.RebuildCategoryPanel();
		});
		if (includeReset)
		{
			ElementFactory.CreateDefaultButton(ElementFactory.CreateHorizontalGroup(panel, 0f, TextAnchor.MiddleRight).transform, style, "Reset to default", 0f, 0f, delegate
			{
				this.OnResetButtonClick();
			});
			base.CreateHorizontalDivider(panel);
		}
	}

	protected void OnResetButtonClick()
	{
		SettingsManager.LegacyGameSettingsUI.SetDefault();
		base.RebuildCategoryPanel();
	}

	protected override void RegisterCategoryPanels()
	{
		base._categoryPanelTypes.Add("Titans", typeof(SettingsGameTitansPanel));
		base._categoryPanelTypes.Add("PVP", typeof(SettingsGamePVPPanel));
		base._categoryPanelTypes.Add("Misc", typeof(SettingsGameMiscPanel));
		base._categoryPanelTypes.Add("Weather", typeof(SettingsGameWeatherPanel));
	}
}
