using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Update Manager")]
[ExecuteInEditMode]
public class UpdateManager : MonoBehaviour
{
	public class DestroyEntry
	{
		public Object obj;

		public float time;
	}

	public delegate void OnUpdate(float delta);

	public class UpdateEntry
	{
		public OnUpdate func;

		public int index;

		public bool isMonoBehaviour;

		public MonoBehaviour mb;
	}

	private BetterList<DestroyEntry> mDest = new BetterList<DestroyEntry>();

	private static UpdateManager mInst;

	private List<UpdateEntry> mOnCoro = new List<UpdateEntry>();

	private List<UpdateEntry> mOnLate = new List<UpdateEntry>();

	private List<UpdateEntry> mOnUpdate = new List<UpdateEntry>();

	private float mTime;

	private void Add(MonoBehaviour mb, int updateOrder, OnUpdate func, List<UpdateEntry> list)
	{
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			if (list[i].func == func)
			{
				return;
			}
		}
		UpdateEntry item = new UpdateEntry
		{
			index = updateOrder,
			func = func,
			mb = mb,
			isMonoBehaviour = (mb != null)
		};
		list.Add(item);
		if (updateOrder != 0)
		{
			list.Sort(Compare);
		}
	}

	public static void AddCoroutine(MonoBehaviour mb, int updateOrder, OnUpdate func)
	{
		UpdateManager.CreateInstance();
		UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnCoro);
	}

	public static void AddDestroy(Object obj, float delay)
	{
		if (!(obj != null))
		{
			return;
		}
		if (Application.isPlaying)
		{
			if (delay > 0f)
			{
				UpdateManager.CreateInstance();
				DestroyEntry item = new DestroyEntry
				{
					obj = obj,
					time = Time.realtimeSinceStartup + delay
				};
				UpdateManager.mInst.mDest.Add(item);
			}
			else
			{
				Object.Destroy(obj);
			}
		}
		else
		{
			Object.DestroyImmediate(obj);
		}
	}

	public static void AddLateUpdate(MonoBehaviour mb, int updateOrder, OnUpdate func)
	{
		UpdateManager.CreateInstance();
		UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnLate);
	}

	public static void AddUpdate(MonoBehaviour mb, int updateOrder, OnUpdate func)
	{
		UpdateManager.CreateInstance();
		UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnUpdate);
	}

	private static int Compare(UpdateEntry a, UpdateEntry b)
	{
		if (a.index < b.index)
		{
			return 1;
		}
		if (a.index > b.index)
		{
			return -1;
		}
		return 0;
	}

	private IEnumerator CoroutineFunction()
	{
		while (Application.isPlaying && this.CoroutineUpdate())
		{
			yield return null;
		}
	}

	private bool CoroutineUpdate()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = realtimeSinceStartup - this.mTime;
		if (num >= 0.001f)
		{
			this.mTime = realtimeSinceStartup;
			this.UpdateList(this.mOnCoro, num);
			bool isPlaying = Application.isPlaying;
			int num2 = this.mDest.size;
			while (num2 > 0)
			{
				DestroyEntry destroyEntry = this.mDest.buffer[--num2];
				if (!isPlaying || destroyEntry.time < this.mTime)
				{
					if (destroyEntry.obj != null)
					{
						NGUITools.Destroy(destroyEntry.obj);
						destroyEntry.obj = null;
					}
					this.mDest.RemoveAt(num2);
				}
			}
			if (this.mOnUpdate.Count == 0 && this.mOnLate.Count == 0 && this.mOnCoro.Count == 0 && this.mDest.size == 0)
			{
				NGUITools.Destroy(base.gameObject);
				return false;
			}
		}
		return true;
	}

	private static void CreateInstance()
	{
		if (UpdateManager.mInst == null)
		{
			UpdateManager.mInst = Object.FindObjectOfType(typeof(UpdateManager)) as UpdateManager;
			if (UpdateManager.mInst == null && Application.isPlaying)
			{
				GameObject obj = new GameObject("_UpdateManager");
				Object.DontDestroyOnLoad(obj);
				UpdateManager.mInst = obj.AddComponent<UpdateManager>();
			}
		}
	}

	private void LateUpdate()
	{
		this.UpdateList(this.mOnLate, Time.deltaTime);
		if (!Application.isPlaying)
		{
			this.CoroutineUpdate();
		}
	}

	private void OnApplicationQuit()
	{
		Object.DestroyImmediate(base.gameObject);
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			this.mTime = Time.realtimeSinceStartup;
			base.StartCoroutine(this.CoroutineFunction());
		}
	}

	private void Update()
	{
		if (UpdateManager.mInst != this)
		{
			NGUITools.Destroy(base.gameObject);
		}
		else
		{
			this.UpdateList(this.mOnUpdate, Time.deltaTime);
		}
	}

	private void UpdateList(List<UpdateEntry> list, float delta)
	{
		int num = list.Count;
		while (num > 0)
		{
			UpdateEntry updateEntry = list[--num];
			if (updateEntry.isMonoBehaviour)
			{
				if (updateEntry.mb == null)
				{
					list.RemoveAt(num);
					continue;
				}
				if (!updateEntry.mb.enabled || !NGUITools.GetActive(updateEntry.mb.gameObject))
				{
					continue;
				}
			}
			updateEntry.func(delta);
		}
	}
}
