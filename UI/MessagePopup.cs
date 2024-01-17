using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MessagePopup : PromptPopup
{
	protected float LabelHeight = 60f;

	private Text _label;

	protected override string Title => string.Empty;

	protected override float Width => 300f;

	protected override float Height => 240f;

	protected override int VerticalPadding => 30;

	protected override int HorizontalPadding => 30;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle style = new ElementStyle(24, 120f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style2, UIManager.GetLocaleCommon("Okay"), 0f, 0f, delegate
		{
			this.OnButtonClick("Okay");
		});
		this._label = ElementFactory.CreateDefaultLabel(base.SinglePanel, style, string.Empty).GetComponent<Text>();
		this._label.GetComponent<LayoutElement>().preferredHeight = this.LabelHeight;
		this._label.GetComponent<LayoutElement>().preferredWidth = this.Width - (float)(this.HorizontalPadding * 2);
	}

	public void Show(string message)
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			this._label.text = message;
		}
	}

	private void OnButtonClick(string name)
	{
		this.Hide();
	}
}
