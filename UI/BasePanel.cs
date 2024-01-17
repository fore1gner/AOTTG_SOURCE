using System;
using System.Collections;
using System.Collections.Generic;
using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class BasePanel : MonoBehaviour
{
	protected Transform SinglePanel;

	protected Transform DoublePanelLeft;

	protected Transform DoublePanelRight;

	protected List<BasePopup> _popups = new List<BasePopup>();

	protected GameObject _currentCategoryPanel;

	protected StringSetting _currentCategoryPanelName = new StringSetting(string.Empty);

	protected Dictionary<string, Type> _categoryPanelTypes = new Dictionary<string, Type>();

	public BasePanel Parent;

	protected virtual string ThemePanel => "DefaultPanel";

	protected virtual float Width => 800f;

	protected virtual float Height => 600f;

	protected virtual float BorderVerticalPadding => 0f;

	protected virtual float BorderHorizontalPadding => 0f;

	protected virtual int VerticalPadding => 30;

	protected virtual int HorizontalPadding => 40;

	protected virtual float VerticalSpacing => 30f;

	protected virtual TextAnchor PanelAlignment => TextAnchor.UpperLeft;

	protected virtual bool DoublePanel => false;

	protected virtual bool DoublePanelDivider => true;

	protected virtual bool ScrollBar => false;

	protected virtual bool CategoryPanel => false;

	protected virtual bool UseLastCategory => true;

	protected virtual bool HasPremadeContent => false;

	protected virtual string DefaultCategoryPanel => string.Empty;

	protected void OnEnable()
	{
		if (base.transform.Find("Border") != null)
		{
			base.transform.Find("Border").GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
		if (this._currentCategoryPanel != null)
		{
			this._currentCategoryPanel.SetActive(value: true);
		}
	}

	public virtual void Setup(BasePanel parent = null)
	{
		this.Parent = parent;
		base.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.Width, this.Height);
		if (!this.CategoryPanel && !this.HasPremadeContent)
		{
			if (this.DoublePanel)
			{
				GameObject doublePanel = this.CreateDoublePanel(this.ScrollBar, this.DoublePanelDivider);
				this.DoublePanelLeft = this.GetDoublePanelLeftTransform(doublePanel);
				this.DoublePanelRight = this.GetDoublePanelRightTransform(doublePanel);
			}
			else
			{
				this.SinglePanel = this.GetSinglePanelTransform(this.CreateSinglePanel(this.ScrollBar));
			}
		}
		else if (this.HasPremadeContent)
		{
			this.SetupPremadePanel();
		}
		this.SetupPopups();
		if (this.CategoryPanel)
		{
			this.RegisterCategoryPanels();
			string lastcategory = UIManager.GetLastcategory(base.GetType());
			if (this.UseLastCategory && lastcategory != string.Empty)
			{
				this.SetCategoryPanel(lastcategory);
			}
			else
			{
				this.SetCategoryPanel(this.DefaultCategoryPanel);
			}
		}
	}

	public virtual void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public virtual void Hide()
	{
		this.HideAllPopups();
		base.gameObject.SetActive(value: false);
	}

	public virtual void SyncSettingElements()
	{
		BaseSettingElement[] componentsInChildren = base.GetComponentsInChildren<BaseSettingElement>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SyncElement();
		}
	}

	protected virtual void SetupPremadePanel()
	{
		if (this.DoublePanel)
		{
			GameObject gameObject = base.transform.Find("DoublePanelContent").gameObject;
			this.DoublePanelLeft = this.GetDoublePanelLeftTransform(gameObject);
			this.DoublePanelRight = this.GetDoublePanelRightTransform(gameObject);
			this.BindPanel(gameObject, this.ScrollBar);
			this.SetPanelPadding(this.GetDoublePanelLeftTransform(gameObject).gameObject);
			this.SetPanelPadding(this.GetDoublePanelRightTransform(gameObject).gameObject);
		}
		else
		{
			GameObject gameObject2 = base.transform.Find("SinglePanelContent").gameObject;
			this.SinglePanel = this.GetSinglePanelTransform(gameObject2);
			this.BindPanel(gameObject2, this.ScrollBar);
			this.SetPanelPadding(this.GetSinglePanelTransform(gameObject2).gameObject);
		}
	}

	protected virtual void SetupPopups()
	{
	}

	protected virtual void HideAllPopups()
	{
		foreach (BasePopup popup in this._popups)
		{
			popup.Hide();
		}
	}

	protected virtual void RegisterCategoryPanels()
	{
	}

	public virtual void SetCategoryPanel(string name)
	{
		this.HideAllPopups();
		if (this._currentCategoryPanel != null)
		{
			UnityEngine.Object.Destroy(this._currentCategoryPanel);
		}
		Type t = this._categoryPanelTypes[name];
		this._currentCategoryPanelName.Value = name;
		this._currentCategoryPanel = ElementFactory.CreateDefaultPanel(base.transform, t, enabled: true);
		this._currentCategoryPanel.SetActive(value: false);
		base.StartCoroutine(this.WaitAndEnableCategoryPanel());
		UIManager.SetLastCategory(base.GetType(), name);
	}

	private IEnumerator WaitAndEnableCategoryPanel()
	{
		yield return new WaitForEndOfFrame();
		this._currentCategoryPanel.SetActive(value: true);
	}

	public string GetCurrentCategoryName()
	{
		return this._currentCategoryPanelName.Value;
	}

	public void RebuildCategoryPanel()
	{
		this.SetCategoryPanel(this._currentCategoryPanelName);
	}

	public void SetCategoryPanel(StringSetting setting)
	{
		this.SetCategoryPanel(setting.Value);
	}

	protected GameObject CreateHorizontalDivider(Transform parent, float height = 1f)
	{
		return ElementFactory.CreateHorizontalLine(width: (!this.DoublePanel) ? (this.GetPanelWidth() - (float)this.HorizontalPadding * 2f) : (this.GetPanelWidth() * 0.5f - (float)this.HorizontalPadding * 2f), parent: parent, style: new ElementStyle(24, 120f, this.ThemePanel), height: height);
	}

	protected Transform GetSinglePanelTransform(GameObject singlePanel)
	{
		return singlePanel.transform.Find("ScrollView/Panel");
	}

	protected Transform GetDoublePanelLeftTransform(GameObject doublePanel)
	{
		return doublePanel.transform.Find("ScrollView/LeftPanel");
	}

	protected Transform GetDoublePanelRightTransform(GameObject doublePanel)
	{
		return doublePanel.transform.Find("ScrollView/RightPanel");
	}

	protected GameObject CreateSinglePanel(bool scrollBar)
	{
		GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("SinglePanelContent");
		this.GetSinglePanelTransform(gameObject).GetComponent<LayoutElement>().preferredWidth = this.GetPanelWidth();
		this.BindPanel(gameObject, scrollBar);
		this.SetPanelPadding(this.GetSinglePanelTransform(gameObject).gameObject);
		return gameObject;
	}

	protected GameObject CreateDoublePanel(bool scrollBar, bool divider)
	{
		GameObject gameObject = AssetBundleManager.InstantiateAsset<GameObject>("DoublePanelContent");
		this.GetDoublePanelLeftTransform(gameObject).GetComponent<LayoutElement>().preferredWidth = this.GetPanelWidth() * 0.5f;
		this.GetDoublePanelRightTransform(gameObject).GetComponent<LayoutElement>().preferredWidth = this.GetPanelWidth() * 0.5f;
		if (divider)
		{
			gameObject.transform.Find("ScrollView/VerticalLine").gameObject.AddComponent<VerticalLineScaler>();
		}
		else
		{
			gameObject.transform.Find("ScrollView/VerticalLine").gameObject.SetActive(value: false);
		}
		this.BindPanel(gameObject, scrollBar);
		this.SetPanelPadding(this.GetDoublePanelLeftTransform(gameObject).gameObject);
		this.SetPanelPadding(this.GetDoublePanelRightTransform(gameObject).gameObject);
		return gameObject;
	}

	protected virtual void BindPanel(GameObject panel, bool scrollBar)
	{
		panel.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
		panel.transform.localPosition = Vector3.zero;
		float panelHeight = this.GetPanelHeight();
		panel.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetPanelWidth(), panelHeight);
		panel.transform.Find("ScrollView").GetComponent<LayoutElement>().minHeight = panelHeight;
		Scrollbar component = panel.transform.Find("Scrollbar").GetComponent<Scrollbar>();
		component.value = 1f;
		if (!scrollBar)
		{
			component.gameObject.SetActive(value: false);
		}
		panel.GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "BackgroundColor");
		component.colors = UIManager.GetThemeColorBlock(this.ThemePanel, "MainBody", "Scrollbar");
		component.GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "MainBody", "ScrollbarBackgroundColor");
	}

	protected void SetPanelPadding(GameObject panel)
	{
		panel.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(this.HorizontalPadding, this.HorizontalPadding, this.VerticalPadding, this.VerticalPadding);
		panel.GetComponent<VerticalLayoutGroup>().spacing = this.VerticalSpacing;
		panel.GetComponent<VerticalLayoutGroup>().childAlignment = this.PanelAlignment;
	}

	protected virtual float GetPanelWidth()
	{
		return this.Width - this.BorderHorizontalPadding * 2f;
	}

	protected virtual float GetPanelHeight()
	{
		return this.Height - this.BorderVerticalPadding * 2f;
	}
}
