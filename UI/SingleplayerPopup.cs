using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace UI;

internal class SingleplayerPopup : BasePopup
{
	private string[] _characterOptions = new string[12]
	{
		"Mikasa", "Levi", "Armin", "Marco", "Jean", "Eren", "Titan_Eren", "Petra", "Sasha", "Set 1",
		"Set 2", "Set 3"
	};

	private string[] _costumeOptions = new string[3] { "Costume 1", "Costume 2", "Costume 3" };

	protected override string Title => UIManager.GetLocale("MainMenu", "SingleplayerPopup", "Title");

	protected override float Width => 800f;

	protected override float Height => 510f;

	protected override bool DoublePanel => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SingleplayerGameSettings singleplayerGameSettings = SettingsManager.SingleplayerGameSettings;
		string category = "MainMenu";
		string subCategory = "SingleplayerPopup";
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(24, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Start"), 0f, 0f, delegate
		{
			this.OnButtonClick("Start");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			this.OnButtonClick("Back");
		});
		ElementFactory.CreateDropdownSetting(base.DoublePanelLeft, style2, singleplayerGameSettings.Map, UIManager.GetLocaleCommon("Map"), this.GetMapOptions(), "", 180f, 40f, 300f, 360f);
		ElementFactory.CreateDropdownSetting(base.DoublePanelLeft, style2, SettingsManager.WeatherSettings.WeatherSets.SelectedSetIndex, UIManager.GetLocale(category, subCategory, "Weather"), SettingsManager.WeatherSettings.WeatherSets.GetSetNames(), "", 180f);
		ElementFactory.CreateToggleGroupSetting(base.DoublePanelLeft, style2, singleplayerGameSettings.Difficulty, UIManager.GetLocale(category, subCategory, "Difficulty"), UIManager.GetLocaleArray(category, subCategory, "DifficultyOptions"));
		ElementFactory.CreateDropdownSetting(base.DoublePanelRight, style2, singleplayerGameSettings.Character, UIManager.GetLocale(category, subCategory, "Character"), this._characterOptions, "", 180f);
		ElementFactory.CreateToggleGroupSetting(base.DoublePanelRight, style2, singleplayerGameSettings.Costume, UIManager.GetLocale(category, subCategory, "Costume"), this._costumeOptions);
		ElementFactory.CreateToggleGroupSetting(base.DoublePanelRight, style2, singleplayerGameSettings.CameraType, UIManager.GetLocale(category, subCategory, "Camera"), RCextensions.EnumToStringArray<CAMERA_TYPE>());
	}

	private string[] GetMapOptions()
	{
		LevelInfo.Init();
		int[] obj = new int[7] { 15, 16, 12, 13, 14, 24, 18 };
		List<string> list = new List<string>();
		int[] array = obj;
		foreach (int num in array)
		{
			list.Add(LevelInfo.levels[num].name);
		}
		return list.ToArray();
	}

	private void OnButtonClick(string name)
	{
		if (name == "Back")
		{
			this.Hide();
		}
		else if (name == "Start")
		{
			this.StartSinglePlayer();
		}
	}

	private void StartSinglePlayer()
	{
		SingleplayerGameSettings singleplayerGameSettings = SettingsManager.SingleplayerGameSettings;
		IN_GAME_MAIN_CAMERA.difficulty = singleplayerGameSettings.Difficulty.Value;
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.SINGLE;
		IN_GAME_MAIN_CAMERA.singleCharacter = singleplayerGameSettings.Character.Value.ToUpper();
		IN_GAME_MAIN_CAMERA.cameraMode = (CAMERA_TYPE)singleplayerGameSettings.CameraType.Value;
		CheckBoxCostume.costumeSet = singleplayerGameSettings.Costume.Value + 1;
		FengGameManagerMKII.level = singleplayerGameSettings.Map.Value;
		Application.LoadLevel(LevelInfo.getInfo(singleplayerGameSettings.Map.Value).mapName);
	}
}
