using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class SettingsPopup : BasePopup
{
	public string LocaleCategory = "SettingsPopup";

	public KeybindPopup KeybindPopup;

	public ColorPickPopup ColorPickPopup;

	public SetNamePopup SetNamePopup;

	public ImportPopup ImportPopup;

	public ExportPopup ExportPopup;

	public EditWeatherSchedulePopup EditWeatherSchedulePopup;

	private List<BaseSettingsContainer> _ignoreDefaultButtonSettings = new List<BaseSettingsContainer>();

	private List<SaveableSettingsContainer> _saveableSettings = new List<SaveableSettingsContainer>();

	protected override string Title => string.Empty;

	protected override float Width => 1010f;

	protected override float Height => 630f;

	protected override bool CategoryPanel => true;

	protected override bool CategoryButtons => true;

	protected override string DefaultCategoryPanel => "General";

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		this.SetupBottomButtons();
		this.SetupSettingsList();
	}

	protected override void SetupTopButtons()
	{
		ElementStyle style = new ElementStyle(28, 120f, this.ThemePanel);
		string[] array = new string[8] { "General", "Graphics", "UI", "Keybinds", "Skins", "CustomMap", "Game", "Ability" };
		foreach (string buttonName in array)
		{
			GameObject gameObject = ElementFactory.CreateCategoryButton(base.TopBar, style, UIManager.GetLocale(this.LocaleCategory, "Top", buttonName + "Button"), delegate
			{
				this.SetCategoryPanel(buttonName);
			});
			base._topButtons.Add(buttonName, gameObject.GetComponent<Button>());
		}
		base.SetupTopButtons();
	}

	protected override void RegisterCategoryPanels()
	{
		base._categoryPanelTypes.Add("General", typeof(SettingsGeneralPanel));
		base._categoryPanelTypes.Add("Graphics", typeof(SettingsGraphicsPanel));
		base._categoryPanelTypes.Add("UI", typeof(SettingsUIPanel));
		base._categoryPanelTypes.Add("Keybinds", typeof(SettingsKeybindsPanel));
		base._categoryPanelTypes.Add("Skins", typeof(SettingsSkinsPanel));
		base._categoryPanelTypes.Add("CustomMap", typeof(SettingsCustomMapPanel));
		base._categoryPanelTypes.Add("Game", typeof(SettingsGamePanel));
		base._categoryPanelTypes.Add("Ability", typeof(SettingsAbilityPanel));
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
		this.KeybindPopup = ElementFactory.CreateHeadedPanel<KeybindPopup>(base.transform).GetComponent<KeybindPopup>();
		this.ColorPickPopup = ElementFactory.CreateHeadedPanel<ColorPickPopup>(base.transform).GetComponent<ColorPickPopup>();
		this.SetNamePopup = ElementFactory.CreateHeadedPanel<SetNamePopup>(base.transform).GetComponent<SetNamePopup>();
		this.ImportPopup = ElementFactory.CreateHeadedPanel<ImportPopup>(base.transform).GetComponent<ImportPopup>();
		this.ExportPopup = ElementFactory.CreateHeadedPanel<ExportPopup>(base.transform).GetComponent<ExportPopup>();
		this.EditWeatherSchedulePopup = ElementFactory.CreateHeadedPanel<EditWeatherSchedulePopup>(base.transform).GetComponent<EditWeatherSchedulePopup>();
		base._popups.Add(this.KeybindPopup);
		base._popups.Add(this.ColorPickPopup);
		base._popups.Add(this.SetNamePopup);
		base._popups.Add(this.ImportPopup);
		base._popups.Add(this.ExportPopup);
		base._popups.Add(this.EditWeatherSchedulePopup);
	}

	private void SetupSettingsList()
	{
		this._saveableSettings.Add(SettingsManager.GeneralSettings);
		this._saveableSettings.Add(SettingsManager.GraphicsSettings);
		this._saveableSettings.Add(SettingsManager.UISettings);
		this._saveableSettings.Add(SettingsManager.InputSettings);
		this._saveableSettings.Add(SettingsManager.CustomSkinSettings);
		this._saveableSettings.Add(SettingsManager.AbilitySettings);
		this._saveableSettings.Add(SettingsManager.LegacyGameSettingsUI);
		this._saveableSettings.Add(SettingsManager.WeatherSettings);
		this._ignoreDefaultButtonSettings.Add(SettingsManager.CustomSkinSettings);
		this._ignoreDefaultButtonSettings.Add(SettingsManager.WeatherSettings);
	}

	private void SetupBottomButtons()
	{
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		string[] array = new string[5] { "Default", "Load", "Save", "Continue", "Quit" };
		foreach (string buttonName in array)
		{
			ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon(buttonName), 0f, 0f, delegate
			{
				this.OnBottomBarButtonClick(buttonName);
			});
		}
	}

	private void OnConfirmSetDefault()
	{
		foreach (SaveableSettingsContainer saveableSetting in this._saveableSettings)
		{
			if (!this._ignoreDefaultButtonSettings.Contains(saveableSetting))
			{
				saveableSetting.SetDefault();
				saveableSetting.Save();
			}
		}
		base.RebuildCategoryPanel();
		UIManager.CurrentMenu.MessagePopup.Show("Settings reset to default.");
	}

	private void OnBottomBarButtonClick(string name)
	{
		switch (name)
		{
		case "Save":
			foreach (SaveableSettingsContainer saveableSetting in this._saveableSettings)
			{
				saveableSetting.Save();
			}
			if (Application.loadedLevel == 0)
			{
				this.Hide();
			}
			else
			{
				GameMenu.TogglePause(pause: false);
			}
			break;
		case "Load":
			foreach (SaveableSettingsContainer saveableSetting2 in this._saveableSettings)
			{
				saveableSetting2.Load();
			}
			base.RebuildCategoryPanel();
			UIManager.CurrentMenu.MessagePopup.Show("Settings loaded from file.");
			break;
		case "Continue":
			if (Application.loadedLevel == 0)
			{
				this.Hide();
			}
			else
			{
				GameMenu.TogglePause(pause: false);
			}
			break;
		case "Default":
			UIManager.CurrentMenu.ConfirmPopup.Show("Are you sure you want to reset to default?", delegate
			{
				this.OnConfirmSetDefault();
			}, "Reset default");
			break;
		case "Quit":
			foreach (SaveableSettingsContainer saveableSetting3 in this._saveableSettings)
			{
				saveableSetting3.Load();
			}
			if (Application.loadedLevel == 0)
			{
				Application.Quit();
				break;
			}
			GameMenu.TogglePause(pause: false);
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
			FengGameManagerMKII.instance.gameStart = false;
			FengGameManagerMKII.instance.DestroyAllExistingCloths();
			Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
			break;
		}
	}

	public override void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			foreach (SaveableSettingsContainer saveableSetting in this._saveableSettings)
			{
				saveableSetting.Apply();
			}
		}
		base.Hide();
	}
}
