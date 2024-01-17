using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Play Idle Animations")]
public class PlayIdleAnimations : MonoBehaviour
{
	private Animation mAnim;

	private List<AnimationClip> mBreaks = new List<AnimationClip>();

	private AnimationClip mIdle;

	private int mLastIndex;

	private float mNextBreak;

	private void Start()
	{
		this.mAnim = base.GetComponentInChildren<Animation>();
		if (this.mAnim == null)
		{
			Debug.LogWarning(NGUITools.GetHierarchy(base.gameObject) + " has no Animation component");
			Object.Destroy(this);
			return;
		}
		foreach (AnimationState item in this.mAnim)
		{
			if (item.clip.name == "idle")
			{
				item.layer = 0;
				this.mIdle = item.clip;
				this.mAnim.Play(this.mIdle.name);
			}
			else if (item.clip.name.StartsWith("idle"))
			{
				item.layer = 1;
				this.mBreaks.Add(item.clip);
			}
		}
		if (this.mBreaks.Count == 0)
		{
			Object.Destroy(this);
		}
	}

	private void Update()
	{
		if (!(this.mNextBreak < Time.time))
		{
			return;
		}
		if (this.mBreaks.Count == 1)
		{
			AnimationClip animationClip = this.mBreaks[0];
			this.mNextBreak = Time.time + animationClip.length + Random.Range(5f, 15f);
			this.mAnim.CrossFade(animationClip.name);
			return;
		}
		int num = Random.Range(0, this.mBreaks.Count - 1);
		if (this.mLastIndex == num)
		{
			num++;
			if (num >= this.mBreaks.Count)
			{
				num = 0;
			}
		}
		this.mLastIndex = num;
		AnimationClip animationClip2 = this.mBreaks[num];
		this.mNextBreak = Time.time + animationClip2.length + Random.Range(2f, 8f);
		this.mAnim.CrossFade(animationClip2.name);
	}
}
