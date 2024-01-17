using Settings;
using UnityEngine.Events;

namespace UI;

internal class SetNamePopup : PromptPopup
{
	private UnityAction _onSave;

	private InputSettingElement _element;

	public StringSetting NameSetting = new StringSetting(string.Empty);

	private string _initialValue;

	protected override string Title => string.Empty;

	protected override float Width => 320f;

	protected override float Height => 240f;

	protected override int VerticalPadding => 40;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 100f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Save"), 0f, 0f, delegate
		{
			this.OnButtonClick("Save");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Cancel"), 0f, 0f, delegate
		{
			this.OnButtonClick("Cancel");
		});
		this._element = ElementFactory.CreateInputSetting(base.SinglePanel, style, this.NameSetting, UIManager.GetLocaleCommon("SetName"), "", 120f).GetComponent<InputSettingElement>();
	}

	public void Show(string initialValue, UnityAction onSave, string title)
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			this._initialValue = initialValue;
			this._onSave = onSave;
			base.SetTitle(title);
			this.NameSetting.Value = initialValue;
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
			if (this.NameSetting.Value == string.Empty)
			{
				this.NameSetting.Value = this._initialValue;
			}
			this._onSave();
			this.Hide();
		}
	}
}
