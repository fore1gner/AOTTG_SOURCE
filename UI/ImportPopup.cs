using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Weather;

namespace UI;

internal class ImportPopup : PromptPopup
{
	private UnityAction _onSave;

	private InputSettingElement _element;

	private Text _text;

	public StringSetting ImportSetting = new StringSetting(string.Empty);

	protected override string Title => UIManager.GetLocaleCommon("Import");

	protected override float Width => 500f;

	protected override float Height => 600f;

	protected override int VerticalPadding => 20;

	protected override int HorizontalPadding => 20;

	protected override float VerticalSpacing => 10f;

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
		this._element = ElementFactory.CreateInputSetting(base.SinglePanel, style, this.ImportSetting, string.Empty, "", 460f, 390f, multiLine: true).GetComponent<InputSettingElement>();
		this._text = ElementFactory.CreateDefaultLabel(base.SinglePanel, style, "").GetComponent<Text>();
		this._text.color = Color.red;
	}

	public void Show(UnityAction onSave)
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			this._onSave = onSave;
			this.ImportSetting.Value = string.Empty;
			this._text.text = string.Empty;
			this._element.SyncElement();
		}
	}

	private void OnButtonClick(string name)
	{
		if (name == "Cancel")
		{
			this.Hide();
		}
		else if (name == "Save")
		{
			string text = new WeatherSchedule().DeserializeFromCSV(this.ImportSetting.Value);
			if (text != string.Empty)
			{
				this._text.text = text;
				return;
			}
			this._onSave();
			this.Hide();
		}
	}
}
