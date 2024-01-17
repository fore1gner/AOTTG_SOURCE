using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class ColorSettingElement : BaseSettingElement
{
	private Image _image;

	private ColorPickPopup _colorPickPopup;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType> { SettingType.Color };

	public void Setup(BaseSetting setting, ElementStyle style, string title, ColorPickPopup colorPickPopup, string tooltip, float elementWidth, float elementHeight)
	{
		this._colorPickPopup = colorPickPopup;
		GameObject gameObject = base.transform.Find("ColorButton").gameObject;
		gameObject.GetComponent<LayoutElement>().preferredWidth = elementWidth;
		gameObject.GetComponent<LayoutElement>().preferredHeight = elementHeight;
		gameObject.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnButtonClicked();
		});
		gameObject.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Icon");
		this._image = gameObject.transform.Find("Border/Image").GetComponent<Image>();
		base.Setup(setting, style, title, tooltip);
	}

	protected void OnButtonClicked()
	{
		this._colorPickPopup.Show((ColorSetting)base._setting, this._image);
	}

	public override void SyncElement()
	{
		this._image.color = ((ColorSetting)base._setting).Value;
	}
}
