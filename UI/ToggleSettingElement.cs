using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class ToggleSettingElement : BaseSettingElement
{
	protected Toggle _toggle;

	private float _checkMarkSizeMultiplier = 0.66f;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType> { SettingType.Bool };

	public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, float elementWidth, float elementHeight)
	{
		this._toggle = base.transform.Find("Toggle").GetComponent<Toggle>();
		this._toggle.onValueChanged.AddListener(delegate(bool value)
		{
			this.OnValueChanged(value);
		});
		LayoutElement component = this._toggle.transform.Find("Background").GetComponent<LayoutElement>();
		RectTransform component2 = component.transform.Find("Checkmark").GetComponent<RectTransform>();
		component.preferredHeight = elementHeight;
		component.preferredWidth = elementWidth;
		component2.sizeDelta = new Vector2(elementWidth * this._checkMarkSizeMultiplier, elementHeight * this._checkMarkSizeMultiplier);
		base.Setup(setting, style, title, tooltip);
		component2.GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "ToggleFilledColor");
		this._toggle.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Toggle");
	}

	protected void OnValueChanged(bool value)
	{
		((BoolSetting)base._setting).Value = value;
	}

	public override void SyncElement()
	{
		this._toggle.isOn = ((BoolSetting)base._setting).Value;
	}
}
