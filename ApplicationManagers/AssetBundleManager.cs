using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ApplicationManagers;

public class AssetBundleManager : MonoBehaviour
{
	public static AssetBundle MainAssetBundle;

	public static AssetBundleStatus Status = AssetBundleStatus.Loading;

	public static bool CloseFailureBox = false;

	private static AssetBundleManager _instance;

	private static Dictionary<string, Object> _cache = new Dictionary<string, Object>();

	private static readonly string RootDataPath = Application.dataPath;

	private static readonly string LocalAssetBundlePath = "file:///" + AssetBundleManager.RootDataPath + "/RCAssets.unity3d";

	private static readonly string BackupAssetBundleURL = AutoUpdateManager.PlatformUpdateURL + "/RCAssets.unity3d";

	public static void Init()
	{
		AssetBundleManager._instance = SingletonFactory.CreateSingleton(AssetBundleManager._instance);
		AssetBundleManager.LoadAssetBundle();
	}

	public static void LoadAssetBundle()
	{
		AssetBundleManager._instance.StartCoroutine(AssetBundleManager._instance.LoadAssetBundleCoroutine());
	}

	public static Object LoadAsset(string name, bool cached = false)
	{
		if (cached)
		{
			if (!AssetBundleManager._cache.ContainsKey(name))
			{
				AssetBundleManager._cache.Add(name, AssetBundleManager.MainAssetBundle.Load(name));
			}
			return AssetBundleManager._cache[name];
		}
		return AssetBundleManager.MainAssetBundle.Load(name);
	}

	public static T InstantiateAsset<T>(string name) where T : Object
	{
		return (T)Object.Instantiate(AssetBundleManager.MainAssetBundle.Load(name));
	}

	public static T InstantiateAsset<T>(string name, Vector3 position, Quaternion rotation) where T : Object
	{
		return (T)Object.Instantiate(AssetBundleManager.MainAssetBundle.Load(name), position, rotation);
	}

	private IEnumerator LoadAssetBundleCoroutine()
	{
		AssetBundleManager.Status = AssetBundleStatus.Loading;
		while (AutoUpdateManager.Status == AutoUpdateStatus.Updating || !Caching.ready)
		{
			yield return null;
		}
		using WWW wwwLocal = new WWW(AssetBundleManager.LocalAssetBundlePath);
		yield return wwwLocal;
		if (wwwLocal.error != null)
		{
			Debug.Log("Failed to load local asset bundle, trying backup URL at " + AssetBundleManager.BackupAssetBundleURL + ": " + wwwLocal.error);
			using WWW wwwBackup = WWW.LoadFromCacheOrDownload(AssetBundleManager.BackupAssetBundleURL, 20211122);
			yield return wwwBackup;
			if (wwwBackup.error != null)
			{
				Debug.Log("The backup asset bundle failed too: " + wwwBackup.error);
				AssetBundleManager.Status = AssetBundleStatus.Failed;
				yield break;
			}
			this.OnAssetBundleLoaded(wwwBackup);
		}
		else
		{
			this.OnAssetBundleLoaded(wwwLocal);
		}
	}

	private void OnAssetBundleLoaded(WWW www)
	{
		FengGameManagerMKII.RCassets = www.assetBundle;
		FengGameManagerMKII.isAssetLoaded = true;
		AssetBundleManager.MainAssetBundle = FengGameManagerMKII.RCassets;
		MainApplicationManager.FinishLoadAssets();
		AssetBundleManager.Status = AssetBundleStatus.Ready;
	}
}
