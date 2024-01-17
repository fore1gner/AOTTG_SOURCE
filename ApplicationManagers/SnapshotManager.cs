using System;
using System.IO;
using UnityEngine;
using Utility;

namespace ApplicationManagers;

internal class SnapshotManager : MonoBehaviour
{
	private static SnapshotManager _instance;

	public static readonly string SnapshotPath = Application.dataPath + "/UserData/Snapshots";

	private static readonly string SnapshotTempPath = Application.dataPath + "/UserData/Snapshots/Temp";

	private static readonly string SnapshotFilePrefix = "Snapshot";

	private static readonly int MaxSnapshots = 500;

	private static int _currentSnapshotSaveId = 0;

	private static int _maxSnapshotSaveId = 0;

	private static int[] _damages = new int[SnapshotManager.MaxSnapshots];

	public static void Init()
	{
		SnapshotManager._instance = SingletonFactory.CreateSingleton(SnapshotManager._instance);
		SnapshotManager.ClearTemp();
	}

	private void OnApplicationQuit()
	{
		SnapshotManager.ClearTemp();
	}

	private static void ClearTemp()
	{
		if (Directory.Exists(SnapshotManager.SnapshotTempPath))
		{
			try
			{
				Directory.Delete(SnapshotManager.SnapshotTempPath, recursive: true);
			}
			catch (Exception ex)
			{
				Debug.Log($"Error deleting snapshot temp folder: {ex.Message}");
			}
		}
	}

	private static string GetFileName(int snapshotId)
	{
		return SnapshotManager.SnapshotFilePrefix + snapshotId;
	}

	public static void AddSnapshot(Texture2D texture, int damage)
	{
		try
		{
			if (!Directory.Exists(SnapshotManager.SnapshotTempPath))
			{
				Directory.CreateDirectory(SnapshotManager.SnapshotTempPath);
			}
			File.WriteAllBytes(SnapshotManager.SnapshotTempPath + "/" + SnapshotManager.GetFileName(SnapshotManager._currentSnapshotSaveId), SnapshotManager.SerializeSnapshot(texture));
			SnapshotManager._damages[SnapshotManager._currentSnapshotSaveId] = damage;
			SnapshotManager._currentSnapshotSaveId++;
			SnapshotManager._maxSnapshotSaveId++;
			SnapshotManager._maxSnapshotSaveId = Math.Min(SnapshotManager._maxSnapshotSaveId, SnapshotManager.MaxSnapshots);
			if (SnapshotManager._currentSnapshotSaveId >= SnapshotManager.MaxSnapshots)
			{
				SnapshotManager._currentSnapshotSaveId = 0;
			}
		}
		catch (Exception ex)
		{
			Debug.Log($"Exception while adding snapshot: {ex.Message}");
		}
	}

	private static byte[] SerializeSnapshot(Texture2D texture)
	{
		Color32[] pixels = texture.GetPixels32();
		byte[] array = new byte[pixels.Length * 3 + 8];
		int num = 0;
		byte[] bytes = BitConverter.GetBytes(texture.width);
		foreach (byte b in bytes)
		{
			array[num] = b;
			num++;
		}
		bytes = BitConverter.GetBytes(texture.height);
		foreach (byte b2 in bytes)
		{
			array[num] = b2;
			num++;
		}
		Color32[] array2 = pixels;
		for (int i = 0; i < array2.Length; i++)
		{
			Color32 color = array2[i];
			array[num] = color.r;
			array[num + 1] = color.g;
			array[num + 2] = color.b;
			num += 3;
		}
		return array;
	}

	private static Texture2D DeserializeSnapshot(byte[] bytes)
	{
		int width = BitConverter.ToInt32(bytes, 0);
		int height = BitConverter.ToInt32(bytes, 4);
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, mipmap: false);
		int num = 8;
		Color32[] array = new Color32[(bytes.Length - 8) / 3];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Color32(bytes[num], bytes[num + 1], bytes[num + 2], byte.MaxValue);
			num += 3;
		}
		texture2D.SetPixels32(array);
		texture2D.Apply();
		return texture2D;
	}

	public static void SaveSnapshotFinish(Texture2D texture, string fileName)
	{
		if (!Directory.Exists(SnapshotManager.SnapshotPath))
		{
			Directory.CreateDirectory(SnapshotManager.SnapshotPath);
		}
		File.WriteAllBytes(SnapshotManager.SnapshotPath + "/" + fileName, texture.EncodeToPNG());
	}

	public static int GetDamage(int index)
	{
		if (index >= SnapshotManager._maxSnapshotSaveId)
		{
			return 0;
		}
		return SnapshotManager._damages[index];
	}

	public static Texture2D GetSnapshot(int index)
	{
		if (index >= SnapshotManager._maxSnapshotSaveId)
		{
			return null;
		}
		string path = SnapshotManager.SnapshotTempPath + "/" + SnapshotManager.GetFileName(index);
		if (File.Exists(path))
		{
			Texture2D result = SnapshotManager.DeserializeSnapshot(File.ReadAllBytes(path));
			FengGameManagerMKII.instance.unloadAssets();
			return result;
		}
		return null;
	}

	public static int GetLength()
	{
		return SnapshotManager._maxSnapshotSaveId;
	}
}
