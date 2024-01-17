using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MainMenu : BaseMenu
{
	public BasePopup _singleplayerPopup;

	public BasePopup _multiplayerMapPopup;

	public BasePopup _settingsPopup;

	public BasePopup _toolsPopup;

	public BasePopup _multiplayerRoomListPopup;

	public BasePopup _editProfilePopup;

	public BasePopup _questsPopup;

	protected Text _multiplayerStatusLabel;

	public override void Setup()
	{
		base.Setup();
		if (!SettingsManager.GraphicsSettings.AnimatedIntro.Value)
		{
			ElementFactory.InstantiateAndBind(base.transform, "MainBackground").AddComponent<IgnoreScaler>();
		}
		this.SetupIntroPanel();
		this.SetupLabels();
	}

	public void ShowMultiplayerRoomListPopup()
	{
		this.HideAllPopups();
		this._multiplayerRoomListPopup.Show();
	}

	public void ShowMultiplayerMapPopup()
	{
		this.HideAllPopups();
		this._multiplayerMapPopup.Show();
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
		this._singleplayerPopup = ElementFactory.CreateHeadedPanel<SingleplayerPopup>(base.transform).GetComponent<BasePopup>();
		this._multiplayerMapPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerMapPopup>(base.transform, "MultiplayerMapPopup").GetComponent<BasePopup>();
		this._editProfilePopup = ElementFactory.CreateHeadedPanel<EditProfilePopup>(base.transform).GetComponent<BasePopup>();
		this._settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(base.transform).GetComponent<BasePopup>();
		this._toolsPopup = ElementFactory.CreateHeadedPanel<ToolsPopup>(base.transform).GetComponent<BasePopup>();
		this._multiplayerRoomListPopup = ElementFactory.InstantiateAndSetupPanel<MultiplayerRoomListPopup>(base.transform, "MultiplayerRoomListPopup").GetComponent<BasePopup>();
		this._questsPopup = ElementFactory.CreateHeadedPanel<QuestPopup>(base.transform).GetComponent<BasePopup>();
		base._popups.Add(this._singleplayerPopup);
		base._popups.Add(this._multiplayerMapPopup);
		base._popups.Add(this._editProfilePopup);
		base._popups.Add(this._settingsPopup);
		base._popups.Add(this._toolsPopup);
		base._popups.Add(this._multiplayerRoomListPopup);
		base._popups.Add(this._questsPopup);
	}

	private void SetupIntroPanel()
	{
		GameObject obj = ElementFactory.InstantiateAndBind(base.transform, "IntroPanel");
		ElementFactory.SetAnchor(obj, TextAnchor.LowerRight, TextAnchor.LowerRight, new Vector2(-10f, 30f));
		foreach (Transform item in obj.transform.Find("Buttons"))
		{
			IntroButton introButton = item.gameObject.AddComponent<IntroButton>();
			introButton.onClick.AddListener(delegate
			{
				this.OnIntroButtonClick(introButton.name);
			});
		}
	}

	private void SetupLabels()
	{
		GameObject obj = ElementFactory.InstantiateAndBind(base.transform, "Aottg2DonateButton");
		ElementFactory.SetAnchor(obj, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-20f, -20f));
		obj.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnIntroButtonClick("Donate");
		});
		this._multiplayerStatusLabel = ElementFactory.CreateDefaultLabel(base.transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
		ElementFactory.SetAnchor(this._multiplayerStatusLabel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
		this._multiplayerStatusLabel.color = Color.white;
		Text component = ElementFactory.CreateDefaultLabel(base.transform, ElementStyle.Default, string.Empty).GetComponent<Text>();
		ElementFactory.SetAnchor(component.gameObject, TextAnchor.LowerCenter, TextAnchor.LowerCenter, new Vector2(0f, 20f));
		component.color = Color.white;
		if (ApplicationConfig.DevelopmentMode)
		{
			component.text = "RC MOD DEVELOPMENT VERSION";
		}
		else
		{
			component.text = "RC Mod Version 11/02/2023.";
		}
	}

	private void Update()
	{
		if (this._multiplayerStatusLabel != null)
		{
			this._multiplayerStatusLabel.text = PhotonNetwork.connectionStateDetailed.ToString();
			if (PhotonNetwork.connected)
			{
				Text multiplayerStatusLabel = this._multiplayerStatusLabel;
				multiplayerStatusLabel.text = multiplayerStatusLabel.text + " ping:" + PhotonNetwork.GetPing();
			}
		}
	}

	private void OnIntroButtonClick(string name)
	{
		this.HideAllPopups();
		switch (name)
		{
		case "SingleplayerButton":
			this._singleplayerPopup.Show();
			break;
		case "MultiplayerButton":
			this._multiplayerMapPopup.Show();
			break;
		case "ProfileButton":
			this._editProfilePopup.Show();
			break;
		case "QuestsButton":
			this._questsPopup.Show();
			break;
		case "SettingsButton":
			this._settingsPopup.Show();
			break;
		case "ToolsButton":
			this._toolsPopup.Show();
			break;
		case "QuitButton":
			Application.Quit();
			break;
		case "Donate":
			Application.OpenURL("https://www.patreon.com/aottg2");
			break;
		}
	}
}
