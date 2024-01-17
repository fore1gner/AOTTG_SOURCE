using System.Collections.Generic;
using System.Globalization;
using Settings;
using UnityEngine.UI;

namespace UI;

internal class SliderSettingElement : BaseSettingElement
{
	protected Slider _slider;

	protected Text _valueLabel;

	protected NumberFormatInfo _formatInfo;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>
	{
		SettingType.Float,
		SettingType.Int
	};

	public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, float elementWidth, float elementHeight, int decimalPlaces)
	{
		this._formatInfo = new NumberFormatInfo();
		this._formatInfo.NumberDecimalDigits = decimalPlaces;
		this._slider = base.transform.Find("Slider").GetComponent<Slider>();
		this._valueLabel = base.transform.Find("Value").GetComponent<Text>();
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
		this._slider.GetComponent<LayoutElement>().preferredWidth = elementWidth;
		this._slider.GetComponent<LayoutElement>().preferredHeight = elementHeight;
		this._slider.onValueChanged.AddListener(delegate(float value)
		{
			this.OnValueChanged(value);
		});
		this._valueLabel.fontSize = style.FontSize;
		this._slider.transform.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderBackgroundColor");
		this._slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderFillColor");
		this._slider.transform.Find("Handle Slide Area/Handle").GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "SliderHandleColor");
		this._valueLabel.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
		base.Setup(setting, style, title, tooltip);
	}

	protected void OnValueChanged(float value)
	{
		if (base._settingType == SettingType.Float)
		{
			((FloatSetting)base._setting).Value = value;
		}
		else if (base._settingType == SettingType.Int)
		{
			((IntSetting)base._setting).Value = (int)value;
		}
		this.UpdateValueLabel();
	}

	protected void UpdateValueLabel()
	{
		if (base._settingType == SettingType.Float)
		{
			this._valueLabel.text = string.Format(this._formatInfo, "{0:N}", this._slider.value);
		}
		else if (base._settingType == SettingType.Int)
		{
			this._valueLabel.text = ((int)this._slider.value).ToString();
		}
	}

	public override void SyncElement()
	{
		if (base._settingType == SettingType.Float)
		{
			this._slider.value = ((FloatSetting)base._setting).Value;
		}
		else if (base._settingType == SettingType.Int)
		{
			this._slider.value = ((IntSetting)base._setting).Value;
		}
		this.UpdateValueLabel();
	}
}
