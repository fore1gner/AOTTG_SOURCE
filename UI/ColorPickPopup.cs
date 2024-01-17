using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class ColorPickPopup : PromptPopup
{
	private float PreviewWidth = 90f;

	private float PreviewHeight = 40f;

	private Image _image;

	private ColorSetting _setting;

	private Image _preview;

	private FloatSetting _red = new FloatSetting(0f, 0f, 1f);

	private FloatSetting _green = new FloatSetting(0f, 0f, 1f);

	private FloatSetting _blue = new FloatSetting(0f, 0f, 1f);

	private FloatSetting _alpha = new FloatSetting(0f, 0f, 1f);

	private List<GameObject> _sliders = new List<GameObject>();

	protected override string Title => UIManager.GetLocale("SettingsPopup", "ColorPickPopup", "Title");

	protected override float Width => 450f;

	protected override float Height => 450f;

	protected override float VerticalSpacing => 20f;

	protected override TextAnchor PanelAlignment => TextAnchor.UpperCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Save"), 0f, 0f, delegate
		{
			this.OnButtonClick("Save");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Cancel"), 0f, 0f, delegate
		{
			this.OnButtonClick("Cancel");
		});
		GameObject gameObject = ElementFactory.InstantiateAndBind(base.SinglePanel, "ColorPreview");
		gameObject.GetComponent<LayoutElement>().preferredWidth = this.PreviewWidth;
		gameObject.GetComponent<LayoutElement>().preferredHeight = this.PreviewHeight;
		this._preview = gameObject.transform.Find("Image").GetComponent<Image>();
	}

	private void Update()
	{
		if (this._preview != null)
		{
			this._preview.color = this.GetColorFromSliders();
		}
	}

	public void Show(ColorSetting setting, Image image)
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			this._setting = setting;
			this._image = image;
			this._red.Value = setting.Value.r;
			this._green.Value = setting.Value.g;
			this._blue.Value = setting.Value.b;
			this._alpha.MinValue = setting.MinAlpha;
			this._alpha.Value = setting.Value.a;
			this._preview.color = this.GetColorFromSliders();
			this.CreateSliders();
		}
	}

	private void CreateSliders()
	{
		foreach (GameObject slider in this._sliders)
		{
			Object.Destroy(slider);
		}
		ElementStyle style = new ElementStyle(24, 85f, this.ThemePanel);
		this._sliders.Add(ElementFactory.CreateSliderInputSetting(base.SinglePanel, style, this._red, "Red", "", 150f, 16f, 70f, 40f, 3));
		this._sliders.Add(ElementFactory.CreateSliderInputSetting(base.SinglePanel, style, this._green, "Green", "", 150f, 16f, 70f, 40f, 3));
		this._sliders.Add(ElementFactory.CreateSliderInputSetting(base.SinglePanel, style, this._blue, "Blue", "", 150f, 16f, 70f, 40f, 3));
		this._sliders.Add(ElementFactory.CreateSliderInputSetting(base.SinglePanel, style, this._alpha, "Alpha", "", 150f, 16f, 70f, 40f, 3));
	}

	private void OnButtonClick(string name)
	{
		if (name == "Cancel")
		{
			this.Hide();
		}
		else if (name == "Save")
		{
			this._setting.Value = this.GetColorFromSliders();
			this._image.color = this._setting.Value;
			this.Hide();
		}
	}

	private Color GetColorFromSliders()
	{
		return new Color(this._red.Value, this._green.Value, this._blue.Value, Mathf.Clamp(this._alpha.Value, this._setting.MinAlpha, 1f));
	}
}
