using Settings;
using UnityEngine;

namespace UI;

internal class MultiplayerLanPopup : PromptPopup
{
	protected override string Title => "LAN";

	protected override float Width => 400f;

	protected override float Height => 370f;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		string category = "MainMenu";
		string subCategory = "MultiplayerLanPopup";
		MultiplayerSettings multiplayerSettings = SettingsManager.MultiplayerSettings;
		float elementWidth = 200f;
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(24, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocale(category, subCategory, "Connect"), 0f, 0f, delegate
		{
			this.OnButtonClick("Connect");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			this.OnButtonClick("Back");
		});
		ElementFactory.CreateInputSetting(base.SinglePanel, style2, multiplayerSettings.LanIP, "IP", "", elementWidth);
		ElementFactory.CreateInputSetting(base.SinglePanel, style2, multiplayerSettings.LanPort, "Port", "", elementWidth);
		ElementFactory.CreateInputSetting(base.SinglePanel, style2, multiplayerSettings.LanPassword, "Password (optional)", "", elementWidth);
	}

	protected void OnButtonClick(string name)
	{
		if (name == "Connect")
		{
			SettingsManager.MultiplayerSettings.ConnectLAN();
		}
		else if (name == "Back")
		{
			this.Hide();
		}
	}
}
