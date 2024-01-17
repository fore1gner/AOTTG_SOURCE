using System;
using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI;

internal class ElementFactory
{
	public static ElementStyle CurrentElementStyle = new ElementStyle();

	public static GameObject CreateDefaultMenu<T>() where T : BaseMenu
	{
		GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("DefaultMenu");
		gameObject.transform.position = Vector3.zero;
		T val = gameObject.AddComponent<T>();
		val.Setup();
		val.ApplyScale();
		return gameObject;
	}

	public static GameObject CreateDefaultPanel<T>(Transform parent, bool enabled = false) where T : BasePanel
	{
		return ElementFactory.InstantiateAndSetupPanel<T>(parent, "DefaultPanel", enabled);
	}

	public static GameObject CreateDefaultPanel(Transform parent, Type t, bool enabled = false)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "DefaultPanel");
		((BasePanel)gameObject.AddComponent(t)).Setup(parent.GetComponent<BasePanel>());
		gameObject.SetActive(enabled);
		return gameObject;
	}

	public static GameObject CreateHeadedPanel<T>(Transform parent, bool enabled = false) where T : HeadedPanel
	{
		return ElementFactory.InstantiateAndSetupPanel<T>(parent, "HeadedPanel", enabled);
	}

	public static GameObject CreateTooltipPopup<T>(Transform parent, bool enabled = false) where T : TooltipPopup
	{
		return ElementFactory.InstantiateAndSetupPanel<T>(parent, "TooltipPopup", enabled);
	}

	public static GameObject CreateDefaultButton(Transform parent, ElementStyle style, string title, float elementWidth = 0f, float elementHeight = 0f, UnityAction onClick = null)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "DefaultButton");
		Text component = gameObject.transform.Find("Text").GetComponent<Text>();
		component.text = title;
		component.fontSize = style.FontSize;
		LayoutElement component2 = gameObject.GetComponent<LayoutElement>();
		if (elementWidth > 0f)
		{
			component2.preferredWidth = elementWidth;
		}
		if (elementHeight > 0f)
		{
			component2.preferredHeight = elementHeight;
		}
		if (onClick != null)
		{
			gameObject.GetComponent<Button>().onClick.AddListener(onClick);
		}
		gameObject.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
		component.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
		return gameObject;
	}

	public static GameObject CreateCategoryButton(Transform parent, ElementStyle style, string title, UnityAction onClick = null)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "CategoryButton");
		Text component = gameObject.GetComponent<Text>();
		component.text = title;
		component.fontSize = style.FontSize;
		if (onClick != null)
		{
			gameObject.GetComponent<Button>().onClick.AddListener(onClick);
		}
		gameObject.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "CategoryButton", "");
		return gameObject;
	}

	public static GameObject CreateDropdownSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string[] options, string tooltip = "", float elementWidth = 140f, float elementHeight = 40f, float maxScrollHeight = 300f, float? optionsWidth = null, UnityAction onDropdownOptionSelect = null)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "DropdownSetting");
		DropdownSettingElement dropdownSettingElement = gameObject.AddComponent<DropdownSettingElement>();
		if (!optionsWidth.HasValue)
		{
			optionsWidth = elementWidth;
		}
		dropdownSettingElement.Setup(setting, style, title, options, tooltip, elementWidth, elementHeight, optionsWidth.Value, maxScrollHeight, onDropdownOptionSelect);
		return gameObject;
	}

	public static GameObject CreateIncrementSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "", float elementWidth = 33f, float elementHeight = 30f, string[] options = null, UnityAction onValueChanged = null)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "IncrementSetting");
		gameObject.AddComponent<IncrementSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight, options, onValueChanged);
		return gameObject;
	}

	public static GameObject CreateToggleSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "", float elementWidth = 30f, float elementHeight = 30f)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "ToggleSetting");
		gameObject.AddComponent<ToggleSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight);
		return gameObject;
	}

	public static GameObject CreateToggleGroupSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string[] options, string tooltip = "", float elementWidth = 30f, float elementHeight = 30f)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "ToggleGroupSetting");
		gameObject.AddComponent<ToggleGroupSettingElement>().Setup(setting, style, title, options, tooltip, elementWidth, elementHeight);
		return gameObject;
	}

	public static GameObject CreateSliderSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "", float elementWidth = 150f, float elementHeight = 16f, int decimalPlaces = 2)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "SliderSetting");
		gameObject.AddComponent<SliderSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight, decimalPlaces);
		return gameObject;
	}

	public static GameObject CreateInputSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "", float elementWidth = 140f, float elementHeight = 40f, bool multiLine = false, UnityAction onValueChanged = null, UnityAction onEndEdit = null)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "InputSetting");
		gameObject.AddComponent<InputSettingElement>().Setup(setting, style, title, tooltip, elementWidth, elementHeight, multiLine, onValueChanged, onEndEdit);
		return gameObject;
	}

	public static GameObject CreateSliderInputSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, string tooltip = "", float sliderWidth = 150f, float sliderHeight = 16f, float inputWidth = 70f, float inputHeight = 40f, int decimalPlaces = 2)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "SliderInputSetting");
		gameObject.AddComponent<SliderInputSettingElement>().Setup(setting, style, title, tooltip, sliderWidth, sliderHeight, inputWidth, inputHeight, decimalPlaces);
		return gameObject;
	}

	public static GameObject CreateDefaultLabel(Transform parent, ElementStyle style, string title, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleCenter)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "DefaultLabel");
		Text component = gameObject.GetComponent<Text>();
		component.fontSize = style.FontSize;
		component.text = title;
		component.fontStyle = fontStyle;
		component.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultLabel", "TextColor");
		component.alignment = alignment;
		if (parent.GetComponent<VerticalLayoutGroup>() != null)
		{
			component.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			return gameObject;
		}
		component.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		return gameObject;
	}

	public static GameObject CreateKeybindSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, KeybindPopup keybindPopup, string tooltip = "", float elementWidth = 120f, float elementHeight = 35f, int bindCount = 2)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "KeybindSetting");
		gameObject.AddComponent<KeybindSettingElement>().Setup(setting, style, title, keybindPopup, tooltip, elementWidth, elementHeight, bindCount);
		return gameObject;
	}

	public static GameObject CreateColorSetting(Transform parent, ElementStyle style, BaseSetting setting, string title, ColorPickPopup colorPickPopup, string tooltip = "", float elementWidth = 90f, float elementHeight = 30f)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "ColorSetting");
		gameObject.AddComponent<ColorSettingElement>().Setup(setting, style, title, colorPickPopup, tooltip, elementWidth, elementHeight);
		return gameObject;
	}

	public static GameObject CreateHorizontalLine(Transform parent, ElementStyle style, float width, float height = 1f)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "HorizontalLine");
		gameObject.transform.Find("LineImage").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
		gameObject.transform.Find("LineImage").gameObject.AddComponent<HorizontalLineScaler>();
		gameObject.transform.Find("LineImage").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "MainBody", "HorizontalLineColor");
		return gameObject;
	}

	public static GameObject CreateHorizontalGroup(Transform parent, float spacing, TextAnchor alignment = TextAnchor.UpperLeft)
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, "HorizontalGroup");
		gameObject.GetComponent<HorizontalLayoutGroup>().spacing = spacing;
		gameObject.GetComponent<HorizontalLayoutGroup>().childAlignment = alignment;
		return gameObject;
	}

	public static GameObject InstantiateAndSetupPanel<T>(Transform parent, string asset, bool enabled = false) where T : BasePanel
	{
		GameObject gameObject = ElementFactory.InstantiateAndBind(parent, asset);
		gameObject.AddComponent<T>().Setup(parent.GetComponent<BasePanel>());
		gameObject.SetActive(enabled);
		return gameObject;
	}

	public static GameObject InstantiateAndBind(Transform parent, string asset)
	{
		GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>(asset);
		gameObject.transform.SetParent(parent, worldPositionStays: false);
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject;
	}

	public static void SetAnchor(GameObject obj, TextAnchor anchor, TextAnchor pivot, Vector2 offset)
	{
		RectTransform component = obj.GetComponent<RectTransform>();
		Vector2 anchorMin = (component.anchorMax = ElementFactory.GetAnchorVector(anchor));
		component.anchorMin = anchorMin;
		component.pivot = ElementFactory.GetAnchorVector(pivot);
		component.anchoredPosition = offset;
	}

	public static Vector2 GetAnchorVector(TextAnchor anchor)
	{
		Vector2 result = new Vector2(0f, 0f);
		switch (anchor)
		{
		case TextAnchor.UpperLeft:
			result = new Vector2(0f, 1f);
			break;
		case TextAnchor.MiddleLeft:
			result = new Vector2(0f, 0.5f);
			break;
		case TextAnchor.LowerLeft:
			result = new Vector2(0f, 0f);
			break;
		case TextAnchor.UpperCenter:
			result = new Vector2(0.5f, 1f);
			break;
		case TextAnchor.MiddleCenter:
			result = new Vector2(0.5f, 0.5f);
			break;
		case TextAnchor.LowerCenter:
			result = new Vector2(0.5f, 0f);
			break;
		case TextAnchor.UpperRight:
			result = new Vector2(1f, 1f);
			break;
		case TextAnchor.MiddleRight:
			result = new Vector2(1f, 0.5f);
			break;
		case TextAnchor.LowerRight:
			result = new Vector2(1f, 0f);
			break;
		}
		return result;
	}
}
