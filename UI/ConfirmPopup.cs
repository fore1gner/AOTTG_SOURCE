using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI;

internal class ConfirmPopup : PromptPopup
{
	protected float LabelHeight = 60f;

	private Text _label;

	private UnityAction _onConfirm;

	protected override string Title => UIManager.GetLocaleCommon("Confirm");

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
		ElementFactory.CreateDefaultButton(base.BottomBar, style2, UIManager.GetLocaleCommon("Confirm"), 0f, 0f, delegate
		{
			this.OnButtonClick("Confirm");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, style2, UIManager.GetLocaleCommon("Cancel"), 0f, 0f, delegate
		{
			this.OnButtonClick("Cancel");
		});
		this._label = ElementFactory.CreateDefaultLabel(base.SinglePanel, style, string.Empty).GetComponent<Text>();
		this._label.GetComponent<LayoutElement>().preferredWidth = this.Width - (float)(this.HorizontalPadding * 2);
		this._label.GetComponent<LayoutElement>().preferredHeight = this.LabelHeight;
	}

	public void Show(string message, UnityAction onConfirm, string title = null)
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			this._label.text = message;
			this._onConfirm = onConfirm;
			if (title != null)
			{
				base.SetTitle(title);
			}
			else
			{
				base.SetTitle(this.Title);
			}
		}
	}

	private void OnButtonClick(string name)
	{
		if (name == "Confirm")
		{
			this._onConfirm();
		}
		this.Hide();
	}
}
