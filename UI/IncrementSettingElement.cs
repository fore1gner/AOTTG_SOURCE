using System.Collections.Generic;
using Settings;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI;

internal class IncrementSettingElement : BaseSettingElement
{
	protected Text _valueLabel;

	protected string[] _options;

	protected UnityAction _onValueChanged;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType> { SettingType.Int };

	public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, float elementWidth, float elementHeight, string[] options, UnityAction onValueChanged)
	{
		this._valueLabel = base.transform.Find("Increment/ValueLabel").GetComponent<Text>();
		this._valueLabel.fontSize = style.FontSize;
		this._options = options;
		this._onValueChanged = onValueChanged;
		Button component = base.transform.Find("Increment/LeftButton").GetComponent<Button>();
		Button component2 = base.transform.Find("Increment/RightButton").GetComponent<Button>();
		LayoutElement component3 = component.GetComponent<LayoutElement>();
		LayoutElement component4 = component2.GetComponent<LayoutElement>();
		component.onClick.AddListener(delegate
		{
			this.OnButtonPressed(increment: false);
		});
		component2.onClick.AddListener(delegate
		{
			this.OnButtonPressed(increment: true);
		});
		float preferredWidth = (component4.preferredWidth = elementWidth);
		component3.preferredWidth = preferredWidth;
		preferredWidth = (component4.preferredHeight = elementHeight);
		component3.preferredHeight = preferredWidth;
		base.Setup(setting, style, title, tooltip);
		component.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
		component2.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
		this._valueLabel.color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "TextColor");
	}

	protected void OnButtonPressed(bool increment)
	{
		if (base._settingType == SettingType.Int)
		{
			if (increment)
			{
				((IntSetting)base._setting).Value++;
			}
			else
			{
				((IntSetting)base._setting).Value--;
			}
		}
		this.UpdateValueLabel();
		if (this._onValueChanged != null)
		{
			this._onValueChanged();
		}
	}

	protected void UpdateValueLabel()
	{
		if (base._settingType == SettingType.Int)
		{
			if (this._options == null)
			{
				this._valueLabel.text = ((IntSetting)base._setting).Value.ToString();
			}
			else
			{
				this._valueLabel.text = this._options[((IntSetting)base._setting).Value];
			}
		}
	}

	public override void SyncElement()
	{
		this.UpdateValueLabel();
	}
}
