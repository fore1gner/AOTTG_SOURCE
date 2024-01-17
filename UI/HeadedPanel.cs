using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class HeadedPanel : BasePanel
{
	protected Transform BottomBar;

	protected Transform TopBar;

	protected Dictionary<string, Button> _topButtons = new Dictionary<string, Button>();

	protected virtual string Title => "Default";

	protected virtual float TopBarHeight => 65f;

	protected virtual float BottomBarHeight => 65f;

	protected override float BorderVerticalPadding => 5f;

	protected override float BorderHorizontalPadding => 5f;

	protected override int VerticalPadding => 25;

	protected override int HorizontalPadding => 35;

	protected virtual int TitleFontSize => 30;

	protected virtual int ButtonFontSize => 28;

	protected virtual bool CategoryButtons => false;

	public override void Setup(BasePanel parent = null)
	{
		this.TopBar = base.transform.Find("Background/TopBar");
		this.BottomBar = base.transform.Find("Background/BottomBar");
		Transform obj = base.transform.Find("Background/TopBarLine");
		Transform transform = base.transform.Find("Background/BottomBarLine");
		this.TopBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.TopBarHeight);
		this.BottomBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.BottomBarHeight);
		obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f - this.TopBarHeight);
		transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, this.BottomBarHeight);
		if (this.TopBar.Find("Label") != null)
		{
			if (this.CategoryButtons)
			{
				this.TopBar.Find("Label").gameObject.SetActive(value: false);
			}
			else
			{
				this.TopBar.Find("Label").GetComponent<Text>().fontSize = this.TitleFontSize;
				this.TopBar.Find("Label").GetComponent<Text>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "TitleColor");
				this.SetTitle(this.Title);
			}
		}
		this.TopBar.GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "TopBarColor");
		this.BottomBar.GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BottomBarColor");
		obj.GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BorderColor");
		transform.GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BorderColor");
		base.transform.Find("Border").GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BorderColor");
		base.transform.Find("Background").GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BackgroundColor");
		base.Setup(parent);
		if (this.CategoryButtons)
		{
			this.SetupTopButtons();
			this.SetTopButton(base._currentCategoryPanelName.Value);
		}
	}

	public override void SetCategoryPanel(string name)
	{
		base.SetCategoryPanel(name);
		this.SetTopButton(name);
	}

	protected virtual void SetTopButton(string name)
	{
		if (this._topButtons.Count <= 0)
		{
			return;
		}
		foreach (Button value in this._topButtons.Values)
		{
			value.interactable = true;
		}
		this._topButtons[name].interactable = false;
	}

	protected void SetTitle(string title)
	{
		this.TopBar.Find("Label").GetComponent<Text>().text = title;
	}

	protected virtual void SetupTopButtons()
	{
		Canvas.ForceUpdateCanvases();
		float num = 0f;
		foreach (Button value in this._topButtons.Values)
		{
			num += value.GetComponent<RectTransform>().rect.width;
		}
		this.TopBar.GetComponent<HorizontalLayoutGroup>().spacing = (this.Width - num) / (float)(this._topButtons.Count + 1);
	}

	protected override float GetPanelHeight()
	{
		float y = this.TopBar.GetComponent<RectTransform>().sizeDelta.y;
		float y2 = this.BottomBar.GetComponent<RectTransform>().sizeDelta.y;
		return this.Height - y - y2 - this.BorderVerticalPadding * 2f;
	}
}
