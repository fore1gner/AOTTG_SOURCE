using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class SliderInputSettingElement : BaseSettingElement
{
	protected Slider _slider;

	protected InputField _inputField;

	protected int _inputFontSizeOffset = -4;

	protected NumberFormatInfo _formatInfo;

	protected bool _fixedInputField;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>
	{
		SettingType.Float,
		SettingType.Int
	};

	public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, float sliderWidth, float sliderHeight, float inputWidth, float inputHeight, int decimalPlaces)
	{
		this._formatInfo = new NumberFormatInfo();
		this._formatInfo.NumberDecimalDigits = decimalPlaces;
		this._slider = base.transform.Find("Slider").GetComponent<Slider>();
		base._settingType = base.GetSettingType(setting);
		if (base._settingType == SettingType.Int)
		{
			this._slider.wholeNumbers = true;
			this._slider.minValue = ((IntSetting)setting).MinValue;
			this._slider.maxValue = ((IntSetting)setting).MaxValue;
		}
		else if (base._settingType == SettingType.Float)
		{
			this._slider.wholeNumbers = false;
			this._slider.minValue = ((FloatSetting)setting).MinValue;
			this._slider.maxValue = ((FloatSetting)setting).MaxValue;
		}
		this._slider.GetComponent<LayoutElement>().preferredWidth = sliderWidth;
		this._slider.GetComponent<LayoutElement>().preferredHeight = sliderHeight;
		this._slider.onValueChanged.AddListener(delegate(float value)
		{
			this.OnSliderValueChanged(value);
		});
		this._inputField = base.transform.Find("InputField").GetComponent<InputField>();
		this._inputField.transform.Find("Text").GetComponent<Text>().fontSize = style.FontSize + this._inputFontSizeOffset;
		this._inputField.GetComponent<LayoutElement>().preferredWidth = inputWidth;
		this._inputField.GetComponent<LayoutElement>().preferredHeight = inputHeight;
		base._settingType = base.GetSettingType(setting);
		if (base._settingType == SettingType.Float)
		{
			this._inputField.contentType = InputField.ContentType.DecimalNumber;
		}
		else if (base._settingType == SettingType.Int)
		{
			this._inputField.contentType = InputField.ContentType.IntegerNumber;
		}
		this._inputField.onValueChange.AddListener(delegate(string value)
		{
			this.OnInputValueChanged(value);
		});
		this._inputField.onEndEdit.AddListener(delegate(string value)
		{
			this.OnInputFinishEditing(value);
		});
		base.Setup(setting, style, title, tooltip);
		this._slider.transform.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderBackgroundColor");
		this._slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderFillColor");
		this._slider.transform.Find("Handle Slide Area/Handle").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderHandleColor");
		this._inputField.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Input");
		this._inputField.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputTextColor");
		this._inputField.selectionColor = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputSelectionColor");
		base.StartCoroutine(this.WaitAndFixInputField());
	}

	private void OnEnable()
	{
		if (this._inputField != null && !this._fixedInputField)
		{
			this._inputField.gameObject.SetActive(value: false);
			this._inputField.gameObject.SetActive(value: true);
		}
	}

	private IEnumerator WaitAndFixInputField()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		this._inputField.gameObject.SetActive(value: false);
		this._inputField.gameObject.SetActive(value: true);
		this._fixedInputField = true;
	}

	protected void OnSliderValueChanged(float value)
	{
		if (base._settingType == SettingType.Float)
		{
			((FloatSetting)base._setting).Value = value;
		}
		else if (base._settingType == SettingType.Int)
		{
			((IntSetting)base._setting).Value = (int)value;
		}
		this.SyncInput();
	}

	protected void OnInputValueChanged(string value)
	{
		if (value == string.Empty)
		{
			return;
		}
		int result2;
		if (base._settingType == SettingType.Float)
		{
			if (float.TryParse(value, out var result))
			{
				((FloatSetting)base._setting).Value = Mathf.Clamp(result, this._slider.minValue, this._slider.maxValue);
			}
		}
		else if (base._settingType == SettingType.Int && int.TryParse(value, out result2))
		{
			((IntSetting)base._setting).Value = (int)Mathf.Clamp(result2, this._slider.minValue, this._slider.maxValue);
		}
	}

	protected void OnInputFinishEditing(string value)
	{
		this.SyncElement();
	}

	protected void SyncSlider()
	{
		if (base._settingType == SettingType.Float)
		{
			this._slider.value = ((FloatSetting)base._setting).Value;
		}
		else if (base._settingType == SettingType.Int)
		{
			this._slider.value = ((IntSetting)base._setting).Value;
		}
	}

	protected void SyncInput()
	{
		if (base._settingType == SettingType.Float)
		{
			this._inputField.text = string.Format(this._formatInfo, "{0:N}", ((FloatSetting)base._setting).Value);
		}
		else if (base._settingType == SettingType.Int)
		{
			this._inputField.text = ((IntSetting)base._setting).Value.ToString();
		}
	}

	public override void SyncElement()
	{
		this.SyncSlider();
		this.SyncInput();
	}
}
