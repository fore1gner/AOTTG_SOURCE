using UnityEngine;

[AddComponentMenu("NGUI/Tween/Position")]
public class TweenPosition : UITweener
{
	public Vector3 from;

	private Transform mTrans;

	public Vector3 to;

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	public Vector3 position
	{
		get
		{
			return this.cachedTransform.localPosition;
		}
		set
		{
			this.cachedTransform.localPosition = value;
		}
	}

	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = tweenPosition.position;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, isFinished: true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.cachedTransform.localPosition = this.from * (1f - factor) + this.to * factor;
	}
}
