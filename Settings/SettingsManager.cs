namespace Settings;

internal class SettingsManager
{
	public static MultiplayerSettings MultiplayerSettings;

	public static ProfileSettings ProfileSettings;

	public static SingleplayerGameSettings SingleplayerGameSettings;

	public static MultiplayerGameSettings MultiplayerGameSettings;

	public static CustomSkinSettings CustomSkinSettings;

	public static GraphicsSettings GraphicsSettings;

	public static GeneralSettings GeneralSettings;

	public static UISettings UISettings;

	public static AbilitySettings AbilitySettings;

	public static InputSettings InputSettings;

	public static LegacyGameSettings LegacyGameSettings;

	public static LegacyGameSettings LegacyGameSettingsUI;

	public static LegacyGeneralSettings LegacyGeneralSettings;

	public static WeatherSettings WeatherSettings;

	public static void Init()
	{
		SettingsManager.MultiplayerSettings = new MultiplayerSettings();
		SettingsManager.ProfileSettings = new ProfileSettings();
		SettingsManager.SingleplayerGameSettings = new SingleplayerGameSettings();
		SettingsManager.MultiplayerGameSettings = new MultiplayerGameSettings();
		SettingsManager.CustomSkinSettings = new CustomSkinSettings();
		SettingsManager.GraphicsSettings = new GraphicsSettings();
		SettingsManager.GeneralSettings = new GeneralSettings();
		SettingsManager.UISettings = new UISettings();
		SettingsManager.AbilitySettings = new AbilitySettings();
		SettingsManager.InputSettings = new InputSettings();
		SettingsManager.LegacyGameSettings = new LegacyGameSettings();
		SettingsManager.LegacyGameSettingsUI = new LegacyGameSettings();
		SettingsManager.LegacyGeneralSettings = new LegacyGeneralSettings();
		SettingsManager.WeatherSettings = new WeatherSettings();
	}
}
