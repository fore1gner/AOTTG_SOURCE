using System;
using System.Collections.Generic;
using System.IO;
using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI;

internal class UIManager : MonoBehaviour
{
	private static readonly string LanguageFolderPath = Application.dataPath + "/Resources/Languages";

	private static readonly string UIThemeFolderPath = Application.dataPath + "/Resources/UIThemes";

	private static Dictionary<string, JSONObject> _languages = new Dictionary<string, JSONObject>();

	private static Dictionary<string, JSONObject> _uiThemes = new Dictionary<string, JSONObject>();

	private static Dictionary<Type, string> _lastCategories = new Dictionary<Type, string>();

	private static string _currentUITheme;

	private static UIManager _instance;

	public static BaseMenu CurrentMenu;

	public static float CurrentCanvasScale = 1f;

	public static void Init()
	{
		UIManager._instance = SingletonFactory.CreateSingleton(UIManager._instance);
		UIManager.LoadLanguages();
		UIManager.LoadUIThemes();
	}

	public static void FinishLoadAssets()
	{
		UIManager.LoadEmojis();
		UIManager.SetMenu(MenuType.Main);
	}

	public static void SetLastCategory(Type t, string category)
	{
		if (UIManager._lastCategories.ContainsKey(t))
		{
			UIManager._lastCategories[t] = category;
		}
		else
		{
			UIManager._lastCategories.Add(t, category);
		}
	}

	public static string GetLastcategory(Type t)
	{
		if (UIManager._lastCategories.ContainsKey(t))
		{
			return UIManager._lastCategories[t];
		}
		return string.Empty;
	}

	private static void LoadEmojis()
	{
		foreach (string availableEmoji in GameMenu.AvailableEmojis)
		{
			GameMenu.EmojiTextures.Add(availableEmoji, AssetBundleManager.MainAssetBundle.Load("Emoji" + availableEmoji) as Texture2D);
		}
	}

	public static void SetMenu(MenuType menu)
	{
		UIManager._currentUITheme = SettingsManager.UISettings.UITheme.Value;
		if (UIManager.CurrentMenu != null)
		{
			UnityEngine.Object.Destroy(UIManager.CurrentMenu);
		}
		switch (menu)
		{
		case MenuType.Main:
			UIManager._lastCategories.Clear();
			UIManager.CurrentMenu = ElementFactory.CreateDefaultMenu<MainMenu>().GetComponent<BaseMenu>();
			break;
		case MenuType.Game:
			UIManager.CurrentMenu = ElementFactory.CreateDefaultMenu<GameMenu>().GetComponent<GameMenu>();
			break;
		}
	}

	public static string GetLocale(string category, string subCategory, string item = "", string forcedLanguage = "", string defaultValue = "")
	{
		JSONObject jSONObject = null;
		string text = ((forcedLanguage != string.Empty) ? forcedLanguage : SettingsManager.GeneralSettings.Language.Value);
		if (UIManager._languages.ContainsKey(text))
		{
			jSONObject = UIManager._languages[text];
		}
		string text2 = subCategory;
		if (item != string.Empty)
		{
			text2 = text2 + "." + item;
		}
		if (jSONObject == null || jSONObject[category] == null || jSONObject[category][text2] == null)
		{
			if (text == "English")
			{
				if (defaultValue != string.Empty)
				{
					return defaultValue;
				}
				return $"{text2} locale error.";
			}
			return UIManager.GetLocale(category, subCategory, item, "English");
		}
		return jSONObject[category][text2].Value;
	}

	public static string[] GetLocaleArray(string category, string subCategory, string item = "", string forcedLanguage = "")
	{
		JSONObject jSONObject = null;
		string text = ((forcedLanguage != string.Empty) ? forcedLanguage : SettingsManager.GeneralSettings.Language.Value);
		if (UIManager._languages.ContainsKey(text))
		{
			jSONObject = UIManager._languages[text];
		}
		string text2 = subCategory;
		if (item != string.Empty)
		{
			text2 = text2 + "." + item;
		}
		if (jSONObject == null || jSONObject[category] == null || jSONObject[category][text2] == null)
		{
			if (text == "English")
			{
				return new string[1] { $"{text2} locale error." };
			}
			return UIManager.GetLocaleArray(category, subCategory, item, "English");
		}
		List<string> list = new List<string>();
		JSONNode.Enumerator enumerator = ((JSONArray)jSONObject[category][text2]).GetEnumerator();
		while (enumerator.MoveNext())
		{
			JSONString jSONString = (JSONString)(JSONNode)enumerator.Current;
			list.Add(jSONString.Value);
		}
		return list.ToArray();
	}

	public static string GetLocaleCommon(string item)
	{
		return UIManager.GetLocale("Common", item);
	}

	public static string[] GetLocaleCommonArray(string item)
	{
		return UIManager.GetLocaleArray("Common", item);
	}

	public static string[] GetLanguages()
	{
		List<string> list = new List<string>();
		foreach (string key in UIManager._languages.Keys)
		{
			if (key == "English")
			{
				list.Insert(0, key);
			}
			else
			{
				list.Add(key);
			}
		}
		return list.ToArray();
	}

	private static void LoadLanguages()
	{
		if (!Directory.Exists(UIManager.LanguageFolderPath))
		{
			Directory.CreateDirectory(UIManager.LanguageFolderPath);
			Debug.Log("No language folder found, creating it.");
			return;
		}
		string[] files = Directory.GetFiles(UIManager.LanguageFolderPath, "*.json");
		for (int i = 0; i < files.Length; i++)
		{
			JSONObject jSONObject = (JSONObject)JSON.Parse(File.ReadAllText(files[i]));
			if (!UIManager._languages.ContainsKey(jSONObject["Name"]))
			{
				UIManager._languages.Add(jSONObject["Name"].Value, jSONObject);
			}
		}
		if (!UIManager._languages.ContainsKey(SettingsManager.GeneralSettings.Language.Value))
		{
			SettingsManager.GeneralSettings.Language.Value = "English";
			SettingsManager.GeneralSettings.Save();
		}
	}

	public static Color GetThemeColor(string panel, string category, string item, string fallbackPanel = "DefaultPanel")
	{
		JSONObject jSONObject = null;
		if (UIManager._uiThemes.ContainsKey(UIManager._currentUITheme))
		{
			jSONObject = UIManager._uiThemes[UIManager._currentUITheme];
		}
		if (jSONObject == null || jSONObject[panel] == null || jSONObject[panel][category] == null || jSONObject[panel][category][item] == null)
		{
			if (panel != fallbackPanel)
			{
				return UIManager.GetThemeColor(fallbackPanel, category, item, fallbackPanel);
			}
			Debug.Log($"{panel} {category} {item} theme error.");
			return Color.white;
		}
		try
		{
			List<float> list = new List<float>();
			JSONNode.Enumerator enumerator = ((JSONArray)jSONObject[panel][category][item]).GetEnumerator();
			while (enumerator.MoveNext())
			{
				JSONNumber jSONNumber = (JSONNumber)(JSONNode)enumerator.Current;
				list.Add(float.Parse(jSONNumber.Value) / 255f);
			}
			return new Color(list[0], list[1], list[2], list[3]);
		}
		catch
		{
			Debug.Log($"{panel} {category} {item} theme error.");
			return Color.white;
		}
	}

	public static ColorBlock GetThemeColorBlock(string panel, string category, string item, string fallbackPanel = "DefaultPanel")
	{
		Color themeColor = UIManager.GetThemeColor(panel, category, item + "NormalColor", fallbackPanel);
		Color themeColor2 = UIManager.GetThemeColor(panel, category, item + "HighlightedColor", fallbackPanel);
		Color themeColor3 = UIManager.GetThemeColor(panel, category, item + "PressedColor", fallbackPanel);
		ColorBlock result = default(ColorBlock);
		result.colorMultiplier = 1f;
		result.fadeDuration = 0.1f;
		result.normalColor = themeColor;
		result.highlightedColor = themeColor2;
		result.pressedColor = themeColor3;
		result.disabledColor = themeColor3;
		return result;
	}

	public static string[] GetUIThemes()
	{
		List<string> list = new List<string>();
		bool flag = false;
		bool flag2 = false;
		foreach (string key in UIManager._uiThemes.Keys)
		{
			if (key == "Light")
			{
				flag = true;
			}
			else if (key == "Dark")
			{
				flag2 = true;
			}
			else
			{
				list.Add(key);
			}
		}
		if (flag2)
		{
			list.Insert(0, "Dark");
		}
		if (flag)
		{
			list.Insert(0, "Light");
		}
		return list.ToArray();
	}

	private static void LoadUIThemes()
	{
		if (!Directory.Exists(UIManager.UIThemeFolderPath))
		{
			Directory.CreateDirectory(UIManager.UIThemeFolderPath);
			Debug.Log("No UI theme folder found, creating it.");
			return;
		}
		string[] files = Directory.GetFiles(UIManager.UIThemeFolderPath, "*.json");
		for (int i = 0; i < files.Length; i++)
		{
			JSONObject jSONObject = (JSONObject)JSON.Parse(File.ReadAllText(files[i]));
			if (!UIManager._uiThemes.ContainsKey(jSONObject["Name"]))
			{
				UIManager._uiThemes.Add(jSONObject["Name"].Value, jSONObject);
			}
		}
		if (!UIManager._uiThemes.ContainsKey(SettingsManager.UISettings.UITheme.Value))
		{
			SettingsManager.UISettings.UITheme.Value = "Dark";
			SettingsManager.UISettings.Save();
		}
	}
}
