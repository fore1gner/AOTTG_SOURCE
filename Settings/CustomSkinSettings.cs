using System.IO;
using UnityEngine;

namespace Settings;

internal class CustomSkinSettings : SaveableSettingsContainer
{
	public HumanCustomSkinSettings Human = new HumanCustomSkinSettings();

	public BaseCustomSkinSettings<TitanCustomSkinSet> Titan = new BaseCustomSkinSettings<TitanCustomSkinSet>();

	public BaseCustomSkinSettings<ShifterCustomSkinSet> Shifter = new BaseCustomSkinSettings<ShifterCustomSkinSet>();

	public BaseCustomSkinSettings<SkyboxCustomSkinSet> Skybox = new BaseCustomSkinSettings<SkyboxCustomSkinSet>();

	public BaseCustomSkinSettings<ForestCustomSkinSet> Forest = new BaseCustomSkinSettings<ForestCustomSkinSet>();

	public BaseCustomSkinSettings<CityCustomSkinSet> City = new BaseCustomSkinSettings<CityCustomSkinSet>();

	public BaseCustomSkinSettings<CustomLevelCustomSkinSet> CustomLevel = new BaseCustomSkinSettings<CustomLevelCustomSkinSet>();

	protected override string FileName => "CustomSkins.json";

	public override void Load()
	{
		string filePath = this.GetFilePath();
		if (File.Exists(filePath))
		{
			string text = File.ReadAllText(filePath);
			if (this.Encrypted)
			{
				text = new SimpleAES().Decrypt(text);
			}
			this.DeserializeFromJsonString(text);
			ICustomSkinSettings[] array = new ICustomSkinSettings[7] { this.Human, this.Titan, this.Shifter, this.Skybox, this.Forest, this.City, this.CustomLevel };
			foreach (ICustomSkinSettings customSkinSettings in array)
			{
				if (customSkinSettings.GetSkinSets().GetItems().Count > 0)
				{
					customSkinSettings.GetSets().Clear();
					foreach (BaseSetSetting ıtem in customSkinSettings.GetSkinSets().GetItems())
					{
						customSkinSettings.GetSets().AddItem(ıtem);
					}
				}
				customSkinSettings.GetSkinSets().Clear();
			}
			return;
		}
		try
		{
			this.LoadLegacy();
		}
		catch
		{
			Debug.Log("Exception occurred while loading legacy settings.");
		}
	}
}
