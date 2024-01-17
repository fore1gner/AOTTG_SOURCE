using Settings;
using UnityEngine;

namespace UI;

internal class SettingsCustomMapPanel : SettingsCategoryPanel
{
	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		_ = ((SettingsPopup)parent).LocaleCategory;
		LegacyGameSettings legacyGameSettingsUI = SettingsManager.LegacyGameSettingsUI;
		ElementStyle style = new ElementStyle(24, 200f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(24, 120f, this.ThemePanel);
		ElementStyle style3 = new ElementStyle(28, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultLabel(base.DoublePanelLeft, style, "Map script");
		ElementFactory.CreateInputSetting(base.DoublePanelLeft, style2, legacyGameSettingsUI.LevelScript, string.Empty, "", 420f, 300f, multiLine: true);
		ElementFactory.CreateDefaultButton(ElementFactory.CreateHorizontalGroup(base.DoublePanelLeft, 0f, TextAnchor.UpperCenter).transform, style3, "Clear", 0f, 0f, delegate
		{
			this.OnCustomMapButtonClick("ClearMap");
		});
		string[] options = new string[5] { "Survive", "Waves", "PVP", "Racing", "Custom" };
		ElementFactory.CreateDropdownSetting(base.DoublePanelRight, style, legacyGameSettingsUI.GameType, "Game mode", options);
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style, legacyGameSettingsUI.TitanSpawnCap, "Titan cap");
		base.CreateHorizontalDivider(base.DoublePanelRight);
		ElementFactory.CreateDefaultLabel(base.DoublePanelRight, style, "Logic script");
		ElementFactory.CreateInputSetting(base.DoublePanelRight, style2, legacyGameSettingsUI.LogicScript, string.Empty, "", 420f, 300f, multiLine: true);
		ElementFactory.CreateDefaultButton(ElementFactory.CreateHorizontalGroup(base.DoublePanelRight, 0f, TextAnchor.UpperCenter).transform, style3, "Clear", 0f, 0f, delegate
		{
			this.OnCustomMapButtonClick("ClearLogic");
		});
	}

	private void OnCustomMapButtonClick(string name)
	{
		if (name == "ClearMap")
		{
			SettingsManager.LegacyGameSettingsUI.LevelScript.Value = string.Empty;
		}
		else if (name == "ClearLogic")
		{
			SettingsManager.LegacyGameSettingsUI.LogicScript.Value = string.Empty;
		}
		base.Parent.RebuildCategoryPanel();
	}
}
