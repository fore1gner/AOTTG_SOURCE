using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI;

internal class BasePopup : HeadedPanel
{
	protected float _currentAnimationValue;

	protected HashSet<Transform> _staticTransforms = new HashSet<Transform>();

	protected virtual float MinTweenScale => 0.3f;

	protected virtual float MaxTweenScale => 1f;

	protected virtual float MinFadeAlpha => 0f;

	protected virtual float MaxFadeAlpha => 1f;

	protected virtual float AnimationTime => 0.1f;

	protected virtual PopupAnimation PopupAnimationType => PopupAnimation.Tween;

	public override void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			base.transform.SetAsLastSibling();
			base.StopAllCoroutines();
			if (this.PopupAnimationType == PopupAnimation.Tween)
			{
				base.StartCoroutine(this.TweenIn());
			}
			else if (this.PopupAnimationType == PopupAnimation.Fade)
			{
				base.StartCoroutine(this.FadeIn());
			}
		}
	}

	public override void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			this.HideAllPopups();
			base.StopAllCoroutines();
			if (this.PopupAnimationType == PopupAnimation.Tween)
			{
				base.StartCoroutine(this.TweenOut());
			}
			else if (this.PopupAnimationType == PopupAnimation.Fade)
			{
				base.StartCoroutine(this.FadeOut());
			}
			else if (this.PopupAnimationType == PopupAnimation.None)
			{
				this.FinishHide();
			}
		}
	}

	protected virtual void FinishHide()
	{
		base.gameObject.SetActive(value: false);
	}

	protected IEnumerator TweenIn()
	{
		this._currentAnimationValue = this.MinTweenScale;
		while (this._currentAnimationValue < this.MaxTweenScale)
		{
			this.SetTransformScale(this._currentAnimationValue);
			this._currentAnimationValue += this.GetAnimmationSpeed(this.MinTweenScale, this.MaxTweenScale) * Time.unscaledDeltaTime;
			yield return null;
		}
		this.SetTransformScale(this.MaxTweenScale);
	}

	protected IEnumerator TweenOut()
	{
		this._currentAnimationValue = this.MaxTweenScale;
		while (this._currentAnimationValue > this.MinTweenScale)
		{
			this.SetTransformScale(this._currentAnimationValue);
			this._currentAnimationValue -= this.GetAnimmationSpeed(this.MinTweenScale, this.MaxTweenScale) * Time.unscaledDeltaTime;
			yield return null;
		}
		this.SetTransformScale(this.MinTweenScale);
		this.FinishHide();
	}

	protected IEnumerator FadeIn()
	{
		this._currentAnimationValue = this.MinFadeAlpha;
		while (this._currentAnimationValue < this.MaxFadeAlpha)
		{
			this.SetTransformAlpha(this._currentAnimationValue);
			this._currentAnimationValue += this.GetAnimmationSpeed(this.MinFadeAlpha, this.MaxFadeAlpha) * Time.unscaledDeltaTime;
			yield return null;
		}
		this.SetTransformAlpha(this.MaxFadeAlpha);
	}

	protected IEnumerator FadeOut()
	{
		this._currentAnimationValue = this.MaxFadeAlpha;
		while (this._currentAnimationValue > this.MinFadeAlpha)
		{
			this.SetTransformAlpha(this._currentAnimationValue);
			this._currentAnimationValue -= this.GetAnimmationSpeed(this.MinFadeAlpha, this.MaxFadeAlpha) * Time.unscaledDeltaTime;
			yield return null;
		}
		this.SetTransformAlpha(this.MinFadeAlpha);
		this.FinishHide();
	}

	protected void SetTransformScale(float scale)
	{
		base.transform.localScale = this.GetVectorFromScale(scale);
		foreach (Transform staticTransform in this._staticTransforms)
		{
			float num = 1f;
			IgnoreScaler component = staticTransform.GetComponent<IgnoreScaler>();
			if (component != null)
			{
				num = component.Scale;
			}
			staticTransform.localScale = this.GetVectorFromScale(num / Mathf.Max(scale, 0.1f));
		}
	}

	protected void SetTransformAlpha(float alpha)
	{
		base.transform.GetComponent<CanvasGroup>().alpha = alpha;
	}

	private Vector3 GetVectorFromScale(float scale)
	{
		return new Vector3(scale, scale, scale);
	}

	private float GetAnimmationSpeed(float min, float max)
	{
		return (max - min) / this.AnimationTime;
	}
}
