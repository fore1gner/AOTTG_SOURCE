using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Utility;

namespace ApplicationManagers;

public class AutoUpdateManager : MonoBehaviour
{
	public static AutoUpdateStatus Status = AutoUpdateStatus.Updating;

	public static bool CloseFailureBox = false;

	private static AutoUpdateManager _instance;

	private static readonly string RootDataPath = Application.dataPath;

	private static readonly string Platform = File.ReadAllText(AutoUpdateManager.RootDataPath + "/PlatformInfo");

	private static readonly string RootUpdateURL = "http://aottgrc.com/Patch";

	private static readonly string LauncherVersionURL = AutoUpdateManager.RootUpdateURL + "/LauncherVersion.txt";

	public static readonly string PlatformUpdateURL = AutoUpdateManager.RootUpdateURL + "/" + AutoUpdateManager.Platform;

	private static readonly string ChecksumURL = AutoUpdateManager.PlatformUpdateURL + "/Checksum.txt";

	public static void Init()
	{
		AutoUpdateManager._instance = SingletonFactory.CreateSingleton(AutoUpdateManager._instance);
		AutoUpdateManager.StartUpdate();
	}

	public static void StartUpdate()
	{
		if (ApplicationConfig.DevelopmentMode)
		{
			AutoUpdateManager.Status = AutoUpdateStatus.Updated;
		}
		else
		{
			AutoUpdateManager._instance.StartCoroutine(AutoUpdateManager._instance.StartUpdateCoroutine());
		}
	}

	private IEnumerator StartUpdateCoroutine()
	{
		AutoUpdateManager.Status = AutoUpdateStatus.Updating;
		bool downloadedFile = false;
		if (Application.platform == RuntimePlatform.OSXPlayer && !AutoUpdateManager.RootDataPath.Contains("Applications"))
		{
			AutoUpdateManager.Status = AutoUpdateStatus.MacTranslocated;
			yield break;
		}
		using (WWW wWW = new WWW(AutoUpdateManager.LauncherVersionURL))
		{
			yield return wWW;
			if (wWW.error != null)
			{
				this.OnUpdateFail("Error fetching launcher version", wWW.error);
				yield break;
			}
			if (!float.TryParse(wWW.text, out var _))
			{
				this.OnUpdateFail("Received an invalid launcher version", wWW.text);
				yield break;
			}
			if (wWW.text != "1.0")
			{
				this.OnLauncherOutdated();
				yield break;
			}
		}
		List<string> list;
		using (WWW wWW = new WWW(AutoUpdateManager.ChecksumURL))
		{
			yield return wWW;
			if (wWW.error != null)
			{
				this.OnUpdateFail("Error fetching checksum", wWW.error);
				yield break;
			}
			list = wWW.text.Split('\n').ToList();
		}
		foreach (string item in list)
		{
			string[] array = item.Split(':');
			string fileName = array[0].Trim();
			string text = array[1].Trim();
			string filePath = AutoUpdateManager.RootDataPath + "/" + fileName;
			string text2;
			if (File.Exists(filePath))
			{
				try
				{
					text2 = this.GenerateMD5(filePath);
				}
				catch (Exception ex)
				{
					this.OnUpdateFail("Error generating checksum for " + fileName, ex.Message);
					yield break;
				}
			}
			else
			{
				text2 = string.Empty;
			}
			if (!(text2 != text))
			{
				continue;
			}
			Debug.Log("File diff found, downloading " + fileName);
			downloadedFile = true;
			using WWW wWW = new WWW(AutoUpdateManager.PlatformUpdateURL + "/" + fileName);
			yield return wWW;
			if (wWW.error != null)
			{
				this.OnUpdateFail("Error fetching file " + fileName, wWW.error);
				yield break;
			}
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(filePath));
				File.WriteAllBytes(filePath, wWW.bytes);
			}
			catch (Exception ex2)
			{
				this.OnUpdateFail("Error writing file " + fileName, ex2.Message);
				yield break;
			}
		}
		if (downloadedFile)
		{
			AutoUpdateManager.Status = AutoUpdateStatus.NeedRestart;
		}
		else
		{
			AutoUpdateManager.Status = AutoUpdateStatus.Updated;
		}
	}

	private void OnUpdateFail(string message, string error)
	{
		Debug.Log(message + ": " + error);
		AutoUpdateManager.Status = AutoUpdateStatus.FailedUpdate;
	}

	private void OnLauncherOutdated()
	{
		AutoUpdateManager.Status = AutoUpdateStatus.LauncherOutdated;
	}

	private string GenerateMD5(string filePath)
	{
		byte[] buffer = File.ReadAllBytes(filePath);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = MD5.Create().ComputeHash(buffer);
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
