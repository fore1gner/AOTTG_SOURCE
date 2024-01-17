using Settings;
using UnityEngine;

namespace UI;

internal class MultiplayerSettingsPopup : PromptPopup
{
	protected override string Title => UIManager.GetLocale("MainMenu", "MultiplayerSettingsPopup", "Title");

	protected override float Width => 480f;

	protected override float Height => 550f;

	protected override bool DoublePanel => false;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		string category = "MainMenu";
		string subCategory = "MultiplayerSettingsPopup";
		MultiplayerSettings multiplayerSettings = SettingsManager.MultiplayerSettings;
		float elementWidth = 180f;
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(24, 160f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Save"), 0f, 0f, delegate
		{
			this.OnSaveButtonClick();
		});
		ElementFactory.CreateToggleGroupSetting(base.SinglePanel, style2, multiplayerSettings.LobbyMode, UIManager.GetLocale(category, subCategory, "Lobby"), UIManager.GetLocaleArray(category, subCategory, "LobbyOptions"), UIManager.GetLocale(category, subCategory, "LobbyTooltip"));
		ElementFactory.CreateInputSetting(base.SinglePanel, style2, multiplayerSettings.CustomAppId, UIManager.GetLocale(category, subCategory, "LobbyCustom"), "", elementWidth);
		base.CreateHorizontalDivider(base.SinglePanel);
		ElementFactory.CreateToggleGroupSetting(base.SinglePanel, style2, multiplayerSettings.AppIdMode, UIManager.GetLocale(category, subCategory, "AppId"), UIManager.GetLocaleArray(category, subCategory, "AppIdOptions"), UIManager.GetLocale(category, subCategory, "AppIdTooltip"));
		ElementFactory.CreateInputSetting(base.SinglePanel, style2, multiplayerSettings.CustomAppId, UIManager.GetLocale(category, subCategory, "AppIdCustom"), "", elementWidth);
	}

	protected void OnSaveButtonClick()
	{
		SettingsManager.MultiplayerSettings.Save();
		this.Hide();
	}
}
