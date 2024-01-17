using System.Collections.Generic;
using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class KeybindSettingElement : BaseSettingElement
{
	private List<Text> _buttonLabels = new List<Text>();

	private KeybindPopup _keybindPopup;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType> { SettingType.Keybind };

	public void Setup(BaseSetting setting, ElementStyle style, string title, KeybindPopup keybindPopup, string tooltip, float elementWidth, float elementHeight, int bindCount)
	{
		this._keybindPopup = keybindPopup;
		for (int i = 0; i < bindCount; i++)
		{
			this.CreateKeybindButton(i, style, elementWidth, elementHeight);
		}
		base.Setup(setting, style, title, tooltip);
	}

	private void CreateKeybindButton(int index, ElementStyle style, float width, float height)
	{
		GameObject obj = AssetBundleManager.InstantiateAsset<GameObject>("KeybindButton");
		Text component = obj.transform.Find("Text").GetComponent<Text>();
		component.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "KeybindTextColor");
		component.fontSize = style.FontSize;
		obj.GetComponent<LayoutElement>().preferredWidth = width;
		obj.GetComponent<LayoutElement>().preferredHeight = height;
		obj.transform.SetParent(base.transform, worldPositionStays: false);
		obj.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnButtonClicked(index);
		});
		obj.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Keybind");
		this._buttonLabels.Add(component);
	}

	protected void OnButtonClicked(int index)
	{
		this._keybindPopup.Show(((KeybindSetting)base._setting).InputKeys[index], this._buttonLabels[index]);
	}

	public override void SyncElement()
	{
		for (int i = 0; i < this._buttonLabels.Count; i++)
		{
			this._buttonLabels[i].text = ((KeybindSetting)base._setting).InputKeys[i].ToString();
		}
	}
}
