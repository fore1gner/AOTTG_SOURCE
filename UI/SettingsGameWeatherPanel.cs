using Settings;
using UnityEngine;
using Weather;

namespace UI;

internal class SettingsGameWeatherPanel : SettingsCategoryPanel
{
	protected override bool ScrollBar => true;

	protected override float VerticalSpacing => 20f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsGamePanel obj = (SettingsGamePanel)parent;
		SettingsPopup settingsPopup = (SettingsPopup)obj.Parent;
		obj.CreateGategoryDropdown(base.DoublePanelLeft, includeReset: false, 205f);
		ElementStyle style = new ElementStyle(24, 180f, this.ThemePanel);
		WeatherSettings weatherSettings = SettingsManager.WeatherSettings;
		ElementFactory.CreateDropdownSetting(base.DoublePanelLeft, new ElementStyle(24, 140f, this.ThemePanel), weatherSettings.WeatherSets.GetSelectedSetIndex(), "Weather set", weatherSettings.WeatherSets.GetSetNames(), "* = preset and cannot be modified or deleted. Create a new set to save custom settings.", 205f, 40f, 300f, null, delegate
		{
			base.Parent.RebuildCategoryPanel();
		});
		GameObject gameObject = ElementFactory.CreateHorizontalGroup(base.DoublePanelLeft, 10f);
		string[] array = new string[4] { "Create", "Delete", "Rename", "Copy" };
		foreach (string button2 in array)
		{
			ElementFactory.CreateDefaultButton(gameObject.transform, style, UIManager.GetLocaleCommon(button2), 0f, 0f, delegate
			{
				this.OnWeatherPanelButtonClick(button2);
			});
		}
		WeatherSet weatherSet = (WeatherSet)weatherSettings.WeatherSets.GetSelectedSet();
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		new ElementStyle(24, 150f, this.ThemePanel);
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, weatherSet.UseSchedule, "Use schedule", "Follow a programmed weather schedule.");
		ElementFactory.CreateToggleSetting(base.DoublePanelLeft, style, weatherSet.ScheduleLoop, "Loop schedule");
		gameObject = ElementFactory.CreateHorizontalGroup(base.DoublePanelLeft, 10f);
		array = new string[2] { "Import", "Export" };
		foreach (string button in array)
		{
			ElementFactory.CreateDefaultButton(gameObject.transform, style, UIManager.GetLocaleCommon(button), 0f, 0f, delegate
			{
				this.OnWeatherPanelButtonClick(button);
			});
		}
		ElementFactory.CreateDropdownSetting(base.DoublePanelRight, style, weatherSet.Skybox, "Skybox", RCextensions.EnumToStringArray<WeatherSkybox>());
		ElementFactory.CreateColorSetting(base.DoublePanelRight, style, weatherSet.SkyboxColor, "Skybox color", settingsPopup.ColorPickPopup);
		ElementFactory.CreateColorSetting(base.DoublePanelRight, style, weatherSet.Daylight, "Daylight", settingsPopup.ColorPickPopup);
		ElementFactory.CreateColorSetting(base.DoublePanelRight, style, weatherSet.AmbientLight, "Ambient light", settingsPopup.ColorPickPopup);
		ElementFactory.CreateColorSetting(base.DoublePanelRight, style, weatherSet.Flashlight, "Flashlight", settingsPopup.ColorPickPopup);
		ElementFactory.CreateColorSetting(base.DoublePanelRight, style, weatherSet.FogColor, "Fog color", settingsPopup.ColorPickPopup);
		ElementFactory.CreateSliderSetting(base.DoublePanelRight, style, weatherSet.FogDensity, "Fog density");
		ElementFactory.CreateSliderSetting(base.DoublePanelRight, style, weatherSet.Rain, "Rain");
		ElementFactory.CreateSliderSetting(base.DoublePanelRight, style, weatherSet.Thunder, "Thunder");
		ElementFactory.CreateSliderSetting(base.DoublePanelRight, style, weatherSet.Snow, "Snow");
		ElementFactory.CreateSliderSetting(base.DoublePanelRight, style, weatherSet.Wind, "Wind");
	}

	private void OnWeatherPanelButtonClick(string name)
	{
		SettingsPopup settingsPopup = (SettingsPopup)base.Parent.Parent;
		SetNamePopup setNamePopup = settingsPopup.SetNamePopup;
		WeatherSettings weatherSettings = SettingsManager.WeatherSettings;
		switch (name)
		{
		case "Edit schedule":
			break;
		case "Create":
			setNamePopup.Show("New set", delegate
			{
				this.OnWeatherSetOperationFinish(name);
			}, UIManager.GetLocaleCommon("Create"));
			break;
		case "Delete":
			if (weatherSettings.WeatherSets.CanDeleteSelectedSet())
			{
				UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), delegate
				{
					this.OnWeatherSetOperationFinish(name);
				}, UIManager.GetLocaleCommon("Delete"));
			}
			break;
		case "Rename":
			if (weatherSettings.WeatherSets.CanEditSelectedSet())
			{
				string value = weatherSettings.WeatherSets.GetSelectedSet().Name.Value;
				setNamePopup.Show(value, delegate
				{
					this.OnWeatherSetOperationFinish(name);
				}, UIManager.GetLocaleCommon("Rename"));
			}
			break;
		case "Copy":
			setNamePopup.Show("New set", delegate
			{
				this.OnWeatherSetOperationFinish(name);
			}, UIManager.GetLocaleCommon("Copy"));
			break;
		case "Import":
			settingsPopup.ImportPopup.Show(delegate
			{
				this.OnWeatherSetOperationFinish(name);
			});
			break;
		case "Export":
			settingsPopup.ExportPopup.Show(((WeatherSet)weatherSettings.WeatherSets.GetSelectedSet()).Schedule.Value);
			break;
		}
	}

	private void OnWeatherSetOperationFinish(string name)
	{
		SettingsPopup settingsPopup = (SettingsPopup)base.Parent.Parent;
		SetNamePopup setNamePopup = settingsPopup.SetNamePopup;
		SetSettingsContainer<WeatherSet> weatherSets = SettingsManager.WeatherSettings.WeatherSets;
		switch (name)
		{
		case "Create":
			weatherSets.CreateSet(setNamePopup.NameSetting.Value);
			weatherSets.GetSelectedSetIndex().Value = weatherSets.GetSets().GetCount() - 1;
			break;
		case "Delete":
			weatherSets.DeleteSelectedSet();
			weatherSets.GetSelectedSetIndex().Value = 0;
			break;
		case "Rename":
			weatherSets.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
			break;
		case "Copy":
			weatherSets.CopySelectedSet(setNamePopup.NameSetting.Value);
			weatherSets.GetSelectedSetIndex().Value = weatherSets.GetSets().GetCount() - 1;
			break;
		case "Import":
		{
			ImportPopup ımportPopup = settingsPopup.ImportPopup;
			((WeatherSet)weatherSets.GetSelectedSet()).Schedule.Value = ımportPopup.ImportSetting.Value;
			break;
		}
		}
		base.Parent.RebuildCategoryPanel();
	}
}
