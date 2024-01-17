using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	public enum Scaling
	{
		PixelPerfect,
		FixedSize,
		FixedSizeOnMobiles
	}

	[HideInInspector]
	public bool automatic;

	public int manualHeight = 720;

	public int maximumHeight = 1536;

	public int minimumHeight = 320;

	private static List<UIRoot> mRoots = new List<UIRoot>();

	private Transform mTrans;

	public Scaling scalingStyle = Scaling.FixedSize;

	public int activeHeight
	{
		get
		{
			int num = Mathf.Max(2, Screen.height);
			if (this.scalingStyle == Scaling.FixedSize)
			{
				return this.manualHeight;
			}
			if (num < this.minimumHeight)
			{
				return this.minimumHeight;
			}
			if (num > this.maximumHeight)
			{
				return this.maximumHeight;
			}
			return num;
		}
	}

	public static List<UIRoot> list => UIRoot.mRoots;

	public float pixelSizeAdjustment => this.GetPixelSizeAdjustment(Screen.height);

	private void Awake()
	{
		this.mTrans = base.transform;
		UIRoot.mRoots.Add(this);
		if (this.automatic)
		{
			this.scalingStyle = Scaling.PixelPerfect;
			this.automatic = false;
		}
	}

	public static void Broadcast(string funcName)
	{
		int i = 0;
		for (int count = UIRoot.mRoots.Count; i < count; i++)
		{
			UIRoot uIRoot = UIRoot.mRoots[i];
			if (uIRoot != null)
			{
				uIRoot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
			return;
		}
		int i = 0;
		for (int count = UIRoot.mRoots.Count; i < count; i++)
		{
			UIRoot uIRoot = UIRoot.mRoots[i];
			if (uIRoot != null)
			{
				uIRoot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.scalingStyle == Scaling.FixedSize)
		{
			return (float)this.manualHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(go);
		if (!(uIRoot == null))
		{
			return uIRoot.pixelSizeAdjustment;
		}
		return 1f;
	}

	private void OnDestroy()
	{
		UIRoot.mRoots.Remove(this);
	}

	private void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
	}

	private void Update()
	{
		if (!(this.mTrans != null))
		{
			return;
		}
		float num = this.activeHeight;
		if (num > 0f)
		{
			float num2 = 2f / num;
			Vector3 localScale = this.mTrans.localScale;
			if (Mathf.Abs(localScale.x - num2) > float.Epsilon || Mathf.Abs(localScale.y - num2) > float.Epsilon || Mathf.Abs(localScale.z - num2) > float.Epsilon)
			{
				this.mTrans.localScale = new Vector3(num2, num2, num2);
			}
		}
	}
}
