using System;
using System.Collections;
using System.Collections.Generic;
using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI;

internal class DropdownSettingElement : BaseSettingElement
{
	protected GameObject _optionsPanel;

	protected GameObject _selectedButton;

	protected GameObject _selectedButtonLabel;

	protected string[] _options;

	protected float _currentScrollValue = 1f;

	protected Scrollbar _scrollBar;

	private Vector3 _optionsOffset;

	private UnityAction _onDropdownOptionSelect;

	private Vector3 _lastKnownPosition = Vector3.zero;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>
	{
		SettingType.String,
		SettingType.Int
	};

	public void Setup(BaseSetting setting, ElementStyle style, string title, string[] options, string tooltip, float elementWidth, float elementHeight, float optionsWidth, float maxScrollHeight, UnityAction onDropdownOptionSelect)
	{
		if (options.Length == 0)
		{
			throw new ArgumentException("Dropdown cannot have 0 options.");
		}
		this._onDropdownOptionSelect = onDropdownOptionSelect;
		this._options = options;
		this._optionsPanel = base.transform.Find("Dropdown/Mask").gameObject;
		this._selectedButton = base.transform.Find("Dropdown/SelectedButton").gameObject;
		this._selectedButtonLabel = this._selectedButton.transform.Find("Label").gameObject;
		base.SetupLabel(this._selectedButtonLabel, options[0], style.FontSize);
		this._selectedButton.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnDropdownSelectedButtonClick();
		});
		this._selectedButton.GetComponent<LayoutElement>().preferredWidth = elementWidth;
		this._selectedButton.GetComponent<LayoutElement>().preferredHeight = elementHeight;
		for (int i = 0; i < options.Length; i++)
		{
			this.CreateOptionButton(options[i], i, optionsWidth, elementHeight, style.FontSize, style.ThemePanel);
		}
		this._selectedButton.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Dropdown");
		this._selectedButtonLabel.GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownTextColor");
		this._selectedButton.transform.Find("Image").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownTextColor");
		this._optionsPanel.transform.Find("Options").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownBorderColor");
		Canvas.ForceUpdateCanvases();
		float num = this._optionsPanel.transform.Find("Options").GetComponent<RectTransform>().sizeDelta.y;
		if (num > maxScrollHeight)
		{
			num = maxScrollHeight;
		}
		else
		{
			this._optionsPanel.transform.Find("Scrollbar").gameObject.SetActive(value: false);
		}
		this._scrollBar = this._optionsPanel.transform.Find("Scrollbar").GetComponent<Scrollbar>();
		this._scrollBar.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "DropdownScrollbar");
		this._scrollBar.GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "DropdownScrollbarBackgroundColor");
		this._optionsPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(optionsWidth, num);
		base.transform.Find("Label").GetComponent<LayoutElement>().preferredHeight = elementHeight;
		float x = (optionsWidth - elementWidth) * 0.5f;
		float y = (0f - (elementHeight + num)) * 0.5f + 2f;
		this._optionsOffset = new Vector3(x, y, 0f);
		this._optionsPanel.transform.SetParent(base.transform.root, worldPositionStays: true);
		this._optionsPanel.SetActive(value: false);
		base.Setup(setting, style, title, tooltip);
	}

	protected void SetOptionsPosition()
	{
		Vector3 position = this._selectedButton.transform.position + this._optionsOffset * UIManager.CurrentCanvasScale;
		this._optionsPanel.transform.GetComponent<RectTransform>().position = position;
	}

	private void OnDisable()
	{
		if (this._optionsPanel != null)
		{
			this._optionsPanel.SetActive(value: false);
		}
	}

	private void OnDestroy()
	{
		if (this._optionsPanel != null)
		{
			UnityEngine.Object.Destroy(this._optionsPanel);
		}
	}

	private void Update()
	{
		if (this._optionsPanel != null && this._optionsPanel.activeSelf && ((Input.GetKeyUp(KeyCode.Mouse0) && EventSystem.current.currentSelectedGameObject != this._scrollBar.gameObject) || base.transform.position != this._lastKnownPosition))
		{
			base.StartCoroutine(this.WaitAndCloseOptions());
		}
	}

	protected void CreateOptionButton(string option, int index, float width, float height, int fontSize, string themePanel)
	{
		GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("DropdownOption");
		gameObject.transform.SetParent(this._optionsPanel.transform.Find("Options"), worldPositionStays: false);
		base.SetupLabel(gameObject.transform.Find("Label").gameObject, option, fontSize);
		gameObject.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnDropdownOptionClick(option, index);
		});
		gameObject.GetComponent<LayoutElement>().preferredWidth = width;
		gameObject.GetComponent<LayoutElement>().preferredHeight = height;
		gameObject.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(themePanel, "DefaultSetting", "Dropdown");
	}

	protected void OnDropdownSelectedButtonClick()
	{
		if (!this._optionsPanel.activeSelf)
		{
			base.StartCoroutine(this.WaitAndEnableOptions());
		}
		else
		{
			this.CloseOptions();
		}
	}

	private IEnumerator WaitAndEnableOptions()
	{
		yield return new WaitForEndOfFrame();
		this.SetOptionsPosition();
		this._optionsPanel.transform.SetAsLastSibling();
		this._lastKnownPosition = base.transform.position;
		this._optionsPanel.SetActive(value: true);
		yield return new WaitForEndOfFrame();
		this._scrollBar.value = this._currentScrollValue;
	}

	private IEnumerator WaitAndCloseOptions()
	{
		yield return new WaitForEndOfFrame();
		this.CloseOptions();
	}

	protected void OnDropdownOptionClick(string option, int index)
	{
		base.SetupLabel(this._selectedButtonLabel, option);
		this.CloseOptions();
		if (base._settingType == SettingType.String)
		{
			((StringSetting)base._setting).Value = option;
		}
		else if (base._settingType == SettingType.Int)
		{
			((IntSetting)base._setting).Value = index;
		}
		this._onDropdownOptionSelect?.Invoke();
	}

	protected void CloseOptions()
	{
		this._currentScrollValue = this._scrollBar.value;
		this._optionsPanel.SetActive(value: false);
	}

	public override void SyncElement()
	{
		if (base._settingType == SettingType.String)
		{
			base.SetupLabel(this._selectedButtonLabel, ((StringSetting)base._setting).Value);
		}
		else if (base._settingType == SettingType.Int)
		{
			IntSetting ıntSetting = (IntSetting)base._setting;
			if (ıntSetting.Value >= this._options.Length)
			{
				ıntSetting.Value = 0;
			}
			base.SetupLabel(this._selectedButtonLabel, this._options[ıntSetting.Value]);
		}
	}
}
