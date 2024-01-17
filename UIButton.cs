using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	public Color disabledColor = Color.grey;

	public bool isEnabled
	{
		get
		{
			Collider collider = base.collider;
			if (collider != null)
			{
				return collider.enabled;
			}
			return false;
		}
		set
		{
			Collider collider = base.collider;
			if (collider != null && collider.enabled != value)
			{
				collider.enabled = value;
				this.UpdateColor(value, immediate: false);
			}
		}
	}

	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			base.OnEnable();
		}
		else
		{
			this.UpdateColor(shouldBeEnabled: false, immediate: true);
		}
	}

	public override void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			base.OnHover(isOver);
		}
	}

	public override void OnPress(bool isPressed)
	{
		if (this.isEnabled)
		{
			base.OnPress(isPressed);
		}
	}

	public void UpdateColor(bool shouldBeEnabled, bool immediate)
	{
		if (base.tweenTarget != null)
		{
			if (!base.mStarted)
			{
				base.mStarted = true;
				base.Init();
			}
			Color color = ((!shouldBeEnabled) ? this.disabledColor : base.defaultColor);
			TweenColor tweenColor = TweenColor.Begin(base.tweenTarget, 0.15f, color);
			if (immediate)
			{
				tweenColor.color = color;
				tweenColor.enabled = false;
			}
		}
	}
}
