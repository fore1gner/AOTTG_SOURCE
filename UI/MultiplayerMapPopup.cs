using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MultiplayerMapPopup : BasePopup
{
	protected MultiplayerSettingsPopup _multiplayerSettingsPopup;

	protected MultiplayerLanPopup _lanPopup;

	protected override string ThemePanel => "MultiplayerMapPopup";

	protected override int HorizontalPadding => 0;

	protected override int VerticalPadding => 0;

	protected override float VerticalSpacing => 0f;

	protected override string Title => UIManager.GetLocale("MainMenu", "MultiplayerMapPopup", "Title");

	protected override bool HasPremadeContent => true;

	protected override float Width => 900f;

	protected override float Height => 560f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle elementStyle = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		GameObject gameObject = base.SinglePanel.Find("MultiplayerMap").gameObject;
		Button[] componentsInChildren = gameObject.GetComponentsInChildren<Button>();
		foreach (Button button in componentsInChildren)
		{
			button.onClick.AddListener(delegate
			{
				this.OnButtonClick(button.name);
			});
			button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(elementStyle.ThemePanel, "DefaultButton", "");
			button.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "DefaultButton", "TextColor");
		}
		string category = "MainMenu";
		string subCategory = "MultiplayerMapPopup";
		ElementFactory.CreateDefaultButton(base.BottomBar, elementStyle, "China Server", 0f, 0f, delegate
		{
			this.OnButtonClick("ButtonCN");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, elementStyle, "LAN", 0f, 0f, delegate
		{
			this.OnButtonClick("LAN");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, elementStyle, UIManager.GetLocale(category, subCategory, "ButtonOffline"), 0f, 0f, delegate
		{
			this.OnButtonClick("Offline");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, elementStyle, UIManager.GetLocale(category, subCategory, "ButtonServer"), 0f, 0f, delegate
		{
			this.OnButtonClick("Server");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, elementStyle, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			this.OnButtonClick("Back");
		});
		gameObject.GetComponent<Image>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "MainBody", "MapColor");
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
		this._multiplayerSettingsPopup = ElementFactory.CreateHeadedPanel<MultiplayerSettingsPopup>(base.transform).GetComponent<MultiplayerSettingsPopup>();
		this._lanPopup = ElementFactory.CreateHeadedPanel<MultiplayerLanPopup>(base.transform).GetComponent<MultiplayerLanPopup>();
		base._popups.Add(this._multiplayerSettingsPopup);
		base._popups.Add(this._lanPopup);
	}

	private void OnButtonClick(string name)
	{
		this.HideAllPopups();
		MultiplayerSettings multiplayerSettings = SettingsManager.MultiplayerSettings;
		switch (name)
		{
		case "Back":
			this.Hide();
			break;
		case "Server":
			this._multiplayerSettingsPopup.Show();
			break;
		case "Offline":
			multiplayerSettings.ConnectOffline();
			break;
		case "LAN":
			this._lanPopup.Show();
			break;
		case "ButtonUS":
			multiplayerSettings.ConnectServer(MultiplayerRegion.US);
			break;
		case "ButtonSA":
			multiplayerSettings.ConnectServer(MultiplayerRegion.SA);
			break;
		case "ButtonEU":
			multiplayerSettings.ConnectServer(MultiplayerRegion.EU);
			break;
		case "ButtonASIA":
			multiplayerSettings.ConnectServer(MultiplayerRegion.ASIA);
			break;
		case "ButtonCN":
			multiplayerSettings.ConnectServer(MultiplayerRegion.CN);
			break;
		}
	}
}
