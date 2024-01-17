using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	public TextAsset[] languages;

	private Dictionary<string, string> mDictionary = new Dictionary<string, string>();

	private static Localization mInstance;

	private string mLanguage;

	public string startingLanguage = "English";

	public string currentLanguage
	{
		get
		{
			return this.mLanguage;
		}
		set
		{
			if (!(this.mLanguage != value))
			{
				return;
			}
			this.startingLanguage = value;
			if (!string.IsNullOrEmpty(value))
			{
				if (this.languages != null)
				{
					int i = 0;
					for (int num = this.languages.Length; i < num; i++)
					{
						TextAsset textAsset = this.languages[i];
						if (textAsset != null && textAsset.name == value)
						{
							this.Load(textAsset);
							return;
						}
					}
				}
				TextAsset textAsset2 = Resources.Load(value, typeof(TextAsset)) as TextAsset;
				if (textAsset2 != null)
				{
					this.Load(textAsset2);
					return;
				}
			}
			this.mDictionary.Clear();
			PlayerPrefs.DeleteKey("Language");
		}
	}

	public static Localization instance
	{
		get
		{
			if (Localization.mInstance == null)
			{
				Localization.mInstance = Object.FindObjectOfType(typeof(Localization)) as Localization;
				if (Localization.mInstance == null)
				{
					GameObject obj = new GameObject("_Localization");
					Object.DontDestroyOnLoad(obj);
					Localization.mInstance = obj.AddComponent<Localization>();
				}
			}
			return Localization.mInstance;
		}
	}

	public static bool isActive => Localization.mInstance != null;

	private void Awake()
	{
		if (Localization.mInstance == null)
		{
			Localization.mInstance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.currentLanguage = PlayerPrefs.GetString("Language", this.startingLanguage);
			if (string.IsNullOrEmpty(this.mLanguage) && this.languages != null && this.languages.Length != 0)
			{
				this.currentLanguage = this.languages[0].name;
			}
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public string Get(string key)
	{
		if (this.mDictionary.TryGetValue(key, out var value))
		{
			return value;
		}
		return key;
	}

	private void Load(TextAsset asset)
	{
		this.mLanguage = asset.name;
		PlayerPrefs.SetString("Language", this.mLanguage);
		this.mDictionary = new ByteReader(asset).ReadDictionary();
		UIRoot.Broadcast("OnLocalize", this);
	}

	public static string Localize(string key)
	{
		if (!(Localization.instance == null))
		{
			return Localization.instance.Get(key);
		}
		return key;
	}

	private void OnDestroy()
	{
		if (Localization.mInstance == this)
		{
			Localization.mInstance = null;
		}
	}

	private void OnEnable()
	{
		if (Localization.mInstance == null)
		{
			Localization.mInstance = this;
		}
	}
}
