using Settings;

namespace UI;

internal class ExportPopup : PromptPopup
{
	private InputSettingElement _element;

	public StringSetting ExportSetting = new StringSetting(string.Empty);

	protected override string Title => UIManager.GetLocaleCommon("Export");

	protected override float Width => 500f;

	protected override float Height => 600f;

	protected override int VerticalPadding => 20;

	protected override int HorizontalPadding => 20;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Done"), 0f, 0f, delegate
		{
			this.OnButtonClick("Done");
		});
		this._element = ElementFactory.CreateInputSetting(base.SinglePanel, style, this.ExportSetting, string.Empty, "", 460f, 440f, multiLine: true).GetComponent<InputSettingElement>();
	}

	public void Show(string value)
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			this.ExportSetting.Value = value;
			this._element.SyncElement();
		}
	}

	private void OnButtonClick(string name)
	{
		if (name == "Done")
		{
			this.Hide();
		}
	}
}
