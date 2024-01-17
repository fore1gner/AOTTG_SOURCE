using System.IO;
using UnityEngine;

namespace Settings;

internal abstract class SaveableSettingsContainer : BaseSettingsContainer
{
	protected virtual string FolderPath => Application.dataPath + "/UserData/Settings";

	protected abstract string FileName { get; }

	protected virtual bool Encrypted => false;

	protected override void Setup()
	{
		base.RegisterSettings();
		this.Load();
		this.Apply();
	}

	public virtual void Save()
	{
		Directory.CreateDirectory(this.FolderPath);
		string text = this.SerializeToJsonString();
		if (this.Encrypted)
		{
			text = new SimpleAES().Encrypt(text);
		}
		File.WriteAllText(this.GetFilePath(), text);
	}

	public virtual void Load()
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

	protected virtual void LoadLegacy()
	{
	}

	protected virtual string GetFilePath()
	{
		return this.FolderPath + "/" + this.FileName;
	}
}
