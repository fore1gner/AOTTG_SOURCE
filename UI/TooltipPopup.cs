using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class TooltipPopup : BasePopup
{
	private Text _label;

	private RectTransform _panel;

	public TooltipButton Caller;

	protected override float AnimationTime => 0.15f;

	protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;

	public override void Setup(BasePanel parent = null)
	{
		this._label = base.transform.Find("Panel/Label").GetComponent<Text>();
		this._label.text = string.Empty;
		this._panel = base.transform.Find("Panel").GetComponent<RectTransform>();
		this._label.color = UIManager.GetThemeColor(this.ThemePanel, "DefaultSetting", "TooltipTextColor");
		this._panel.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "DefaultSetting", "TooltipBackgroundColor");
	}

	public void Show(string message, TooltipButton caller)
	{
		if (base.gameObject.activeSelf)
		{
			base.StopAllCoroutines();
			base.SetTransformAlpha(this.MaxFadeAlpha);
		}
		this._label.text = message;
		this.Caller = caller;
		Canvas.ForceUpdateCanvases();
		this.SetTooltipPosition();
		base.Show();
	}

	private void SetTooltipPosition()
	{
		float num = (base.GetComponent<RectTransform>().sizeDelta.x * 0.5f + 40f) * UIManager.CurrentCanvasScale;
		Vector3 position = this.Caller.transform.position;
		if (position.x + num > (float)Screen.width)
		{
			position.x -= num;
		}
		else
		{
			position.x += num;
		}
		base.transform.position = position;
	}

	private void Update()
	{
		if (this.Caller != null)
		{
			this.SetTooltipPosition();
		}
	}
}
