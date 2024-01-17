using UnityEngine.UI;

namespace UI;

internal class IntroButton : Button
{
	private float _fadeTime = 0.1f;

	private Image _hoverImage;

	protected override void Awake()
	{
		this._hoverImage = base.transform.Find("HoverImage").GetComponent<Image>();
		this._hoverImage.canvasRenderer.SetAlpha(0f);
		base.transition = Transition.ColorTint;
		base.targetGraphic = base.transform.Find("Content/Label").GetComponent<Graphic>();
		if (base.gameObject.name.StartsWith("Settings") || base.gameObject.name.StartsWith("Quit") || base.gameObject.name.StartsWith("Profile"))
		{
			base.targetGraphic.GetComponent<Text>().text = UIManager.GetLocaleCommon(base.gameObject.name.Replace("Button", string.Empty));
		}
		else
		{
			base.targetGraphic.GetComponent<Text>().text = UIManager.GetLocale("MainMenu", "Intro", base.gameObject.name);
		}
		ColorBlock colorBlock = default(ColorBlock);
		ColorBlock themeColorBlock = UIManager.GetThemeColorBlock("MainMenu", "IntroButton", "");
		colorBlock.normalColor = themeColorBlock.normalColor;
		colorBlock.highlightedColor = themeColorBlock.highlightedColor;
		colorBlock.pressedColor = themeColorBlock.pressedColor;
		colorBlock.colorMultiplier = 1f;
		colorBlock.fadeDuration = this._fadeTime;
		base.colors = colorBlock;
		base.navigation = new Navigation
		{
			mode = Navigation.Mode.None
		};
	}

	protected override void DoStateTransition(SelectionState state, bool instant)
	{
		base.DoStateTransition(state, instant);
		Image component = base.transform.Find("Content/Icon").GetComponent<Image>();
		switch (state)
		{
		case SelectionState.Highlighted:
		case SelectionState.Pressed:
			this._hoverImage.CrossFadeAlpha(1f, this._fadeTime, ignoreTimeScale: true);
			switch (state)
			{
			case SelectionState.Pressed:
				component.CrossFadeColor(UIManager.GetThemeColor("MainMenu", "IntroButton", "PressedColor"), this._fadeTime, ignoreTimeScale: true, useAlpha: true);
				break;
			case SelectionState.Highlighted:
				component.CrossFadeColor(UIManager.GetThemeColor("MainMenu", "IntroButton", "HighlightedColor"), this._fadeTime, ignoreTimeScale: true, useAlpha: true);
				break;
			}
			break;
		case SelectionState.Normal:
			this._hoverImage.CrossFadeAlpha(0f, this._fadeTime, ignoreTimeScale: true);
			component.CrossFadeColor(UIManager.GetThemeColor("MainMenu", "IntroButton", "NormalColor"), this._fadeTime, ignoreTimeScale: true, useAlpha: true);
			break;
		}
	}
}
