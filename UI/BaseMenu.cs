using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal abstract class BaseMenu : MonoBehaviour
{
	protected List<BasePopup> _popups = new List<BasePopup>();

	public TooltipPopup TooltipPopup;

	public MessagePopup MessagePopup;

	public ConfirmPopup ConfirmPopup;

	public virtual void Setup()
	{
		this.SetupPopups();
	}

	public void ApplyScale()
	{
		base.StartCoroutine(this.WaitAndApplyScale());
	}

	protected IEnumerator WaitAndApplyScale()
	{
		float num = 1f / SettingsManager.UISettings.UIMasterScale.Value;
		base.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920f * num, 1080f * num);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		UIManager.CurrentCanvasScale = base.GetComponent<RectTransform>().localScale.x;
		BaseScaler[] componentsInChildren = base.GetComponentsInChildren<BaseScaler>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].ApplyScale();
		}
	}

	protected virtual void SetupPopups()
	{
		this.TooltipPopup = ElementFactory.CreateTooltipPopup<TooltipPopup>(base.transform).GetComponent<TooltipPopup>();
		this.MessagePopup = ElementFactory.CreateHeadedPanel<MessagePopup>(base.transform).GetComponent<MessagePopup>();
		this.ConfirmPopup = ElementFactory.CreateHeadedPanel<ConfirmPopup>(base.transform).GetComponent<ConfirmPopup>();
		this._popups.Add(this.TooltipPopup);
		this._popups.Add(this.MessagePopup);
		this._popups.Add(this.ConfirmPopup);
	}

	protected virtual void HideAllPopups()
	{
		foreach (BasePopup popup in this._popups)
		{
			popup.Hide();
		}
	}
}
