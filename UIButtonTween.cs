using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Tween")]
public class UIButtonTween : MonoBehaviour
{
	public string callWhenFinished;

	public DisableCondition disableWhenFinished;

	public GameObject eventReceiver;

	public EnableCondition ifDisabledOnPlay;

	public bool includeChildren;

	private bool mHighlighted;

	private bool mStarted;

	private UITweener[] mTweens;

	public UITweener.OnFinished onFinished;

	public Direction playDirection = Direction.Forward;

	public bool resetOnPlay;

	public Trigger trigger;

	public int tweenGroup;

	public GameObject tweenTarget;

	private void OnActivate(bool isActive)
	{
		if (base.enabled && (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && isActive) || (this.trigger == Trigger.OnActivateFalse && !isActive)))
		{
			this.Play(isActive);
		}
	}

	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(forward: true);
		}
	}

	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(forward: true);
		}
	}

	private void OnEnable()
	{
		if (this.mStarted && this.mHighlighted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver))
			{
				this.Play(isOver);
			}
			this.mHighlighted = isOver;
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
		{
			this.Play(isPressed);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			this.Play(forward: true);
		}
	}

	public void Play(bool forward)
	{
		GameObject gameObject = ((this.tweenTarget != null) ? this.tweenTarget : base.gameObject);
		if (!NGUITools.GetActive(gameObject))
		{
			if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, state: true);
		}
		this.mTweens = ((!this.includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if (this.mTweens.Length == 0)
		{
			if (this.disableWhenFinished != 0)
			{
				NGUITools.SetActive(this.tweenTarget, state: false);
			}
			return;
		}
		bool flag = false;
		if (this.playDirection == Direction.Reverse)
		{
			forward = !forward;
		}
		int i = 0;
		for (int num = this.mTweens.Length; i < num; i++)
		{
			UITweener uITweener = this.mTweens[i];
			if (uITweener.tweenGroup == this.tweenGroup)
			{
				if (!flag && !NGUITools.GetActive(gameObject))
				{
					flag = true;
					NGUITools.SetActive(gameObject, state: true);
				}
				if (this.playDirection == Direction.Toggle)
				{
					uITweener.Toggle();
				}
				else
				{
					uITweener.Play(forward);
				}
				if (this.resetOnPlay)
				{
					uITweener.Reset();
				}
				uITweener.onFinished = this.onFinished;
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					uITweener.eventReceiver = this.eventReceiver;
					uITweener.callWhenFinished = this.callWhenFinished;
				}
			}
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
	}

	private void Update()
	{
		if (this.disableWhenFinished == DisableCondition.DoNotDisable || this.mTweens == null)
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		int i = 0;
		for (int num = this.mTweens.Length; i < num; i++)
		{
			UITweener uITweener = this.mTweens[i];
			if (uITweener.tweenGroup == this.tweenGroup)
			{
				if (uITweener.enabled)
				{
					flag = false;
					break;
				}
				if (uITweener.direction != (Direction)this.disableWhenFinished)
				{
					flag2 = false;
				}
			}
		}
		if (flag)
		{
			if (flag2)
			{
				NGUITools.SetActive(this.tweenTarget, state: false);
			}
			this.mTweens = null;
		}
	}
}
