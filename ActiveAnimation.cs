using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Active Animation")]
[RequireComponent(typeof(Animation))]
public class ActiveAnimation : IgnoreTimeScale
{
	public delegate void OnFinished(ActiveAnimation anim);

	public string callWhenFinished;

	public GameObject eventReceiver;

	private Animation mAnim;

	private Direction mDisableDirection;

	private Direction mLastDirection;

	private bool mNotify;

	public OnFinished onFinished;

	public bool isPlaying
	{
		get
		{
			if (this.mAnim != null)
			{
				foreach (AnimationState item in this.mAnim)
				{
					if (!this.mAnim.IsPlaying(item.name))
					{
						continue;
					}
					if (this.mLastDirection == Direction.Forward)
					{
						if (item.time < item.length)
						{
							return true;
						}
						continue;
					}
					if (this.mLastDirection == Direction.Reverse)
					{
						if (item.time > 0f)
						{
							return true;
						}
						continue;
					}
					return true;
				}
			}
			return false;
		}
	}

	private void Play(string clipName, Direction playDirection)
	{
		if (!(this.mAnim != null))
		{
			return;
		}
		base.enabled = true;
		this.mAnim.enabled = false;
		if (playDirection == Direction.Toggle)
		{
			playDirection = ((this.mLastDirection != Direction.Forward) ? Direction.Forward : Direction.Reverse);
		}
		if (string.IsNullOrEmpty(clipName))
		{
			if (!this.mAnim.isPlaying)
			{
				this.mAnim.Play();
			}
		}
		else if (!this.mAnim.IsPlaying(clipName))
		{
			this.mAnim.Play(clipName);
		}
		foreach (AnimationState item in this.mAnim)
		{
			if (string.IsNullOrEmpty(clipName) || item.name == clipName)
			{
				float num = Mathf.Abs(item.speed);
				item.speed = num * (float)playDirection;
				if (playDirection == Direction.Reverse && item.time == 0f)
				{
					item.time = item.length;
				}
				else if (playDirection == Direction.Forward && item.time == item.length)
				{
					item.time = 0f;
				}
			}
		}
		this.mLastDirection = playDirection;
		this.mNotify = true;
		this.mAnim.Sample();
	}

	public static ActiveAnimation Play(Animation anim, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
	{
		return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
	}

	public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
	{
		if (!NGUITools.GetActive(anim.gameObject))
		{
			if (enableBeforePlay != EnableCondition.EnableThenPlay)
			{
				return null;
			}
			NGUITools.SetActive(anim.gameObject, state: true);
			UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				componentsInChildren[i].Refresh();
			}
		}
		ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
		if (activeAnimation == null)
		{
			activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
		}
		activeAnimation.mAnim = anim;
		activeAnimation.mDisableDirection = (Direction)disableCondition;
		activeAnimation.eventReceiver = null;
		activeAnimation.callWhenFinished = null;
		activeAnimation.onFinished = null;
		activeAnimation.Play(clipName, playDirection);
		return activeAnimation;
	}

	public void Reset()
	{
		if (!(this.mAnim != null))
		{
			return;
		}
		foreach (AnimationState item in this.mAnim)
		{
			if (this.mLastDirection == Direction.Reverse)
			{
				item.time = item.length;
			}
			else if (this.mLastDirection == Direction.Forward)
			{
				item.time = 0f;
			}
		}
	}

	private void Update()
	{
		float num = base.UpdateRealTimeDelta();
		if (num == 0f)
		{
			return;
		}
		if (this.mAnim != null)
		{
			bool flag = false;
			foreach (AnimationState item in this.mAnim)
			{
				if (!this.mAnim.IsPlaying(item.name))
				{
					continue;
				}
				float num2 = item.speed * num;
				item.time += num2;
				if (num2 < 0f)
				{
					if (item.time > 0f)
					{
						flag = true;
					}
					else
					{
						item.time = 0f;
					}
				}
				else if (item.time < item.length)
				{
					flag = true;
				}
				else
				{
					item.time = item.length;
				}
			}
			this.mAnim.Sample();
			if (flag)
			{
				return;
			}
			base.enabled = false;
			if (this.mNotify)
			{
				this.mNotify = false;
				if (this.onFinished != null)
				{
					this.onFinished(this);
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				if (this.mDisableDirection != 0 && this.mLastDirection == this.mDisableDirection)
				{
					NGUITools.SetActive(base.gameObject, state: false);
				}
			}
		}
		else
		{
			base.enabled = false;
		}
	}
}
