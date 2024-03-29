using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : MonoBehaviour
{
	public enum Direction
	{
		Down,
		Up
	}

	public delegate void OnReposition();

	public int columns;

	public Direction direction;

	public bool hideInactive = true;

	public bool keepWithinPanel;

	private List<Transform> mChildren = new List<Transform>();

	private UIDraggablePanel mDrag;

	private UIPanel mPanel;

	private bool mStarted;

	public OnReposition onReposition;

	public Vector2 padding = Vector2.zero;

	public bool repositionNow;

	public bool sorted;

	public List<Transform> children
	{
		get
		{
			if (this.mChildren.Count == 0)
			{
				Transform transform = base.transform;
				this.mChildren.Clear();
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					if (child != null && child.gameObject != null && (!this.hideInactive || NGUITools.GetActive(child.gameObject)))
					{
						this.mChildren.Add(child);
					}
				}
				if (this.sorted)
				{
					this.mChildren.Sort(SortByName);
				}
			}
			return this.mChildren;
		}
	}

	private void LateUpdate()
	{
		if (this.repositionNow)
		{
			this.repositionNow = false;
			this.Reposition();
		}
	}

	public void Reposition()
	{
		if (this.mStarted)
		{
			Transform target = base.transform;
			this.mChildren.Clear();
			List<Transform> list = this.children;
			if (list.Count > 0)
			{
				this.RepositionVariableSize(list);
			}
			if (this.mDrag != null)
			{
				this.mDrag.UpdateScrollbars(recalculateBounds: true);
				this.mDrag.RestrictWithinBounds(instant: true);
			}
			else if (this.mPanel != null)
			{
				this.mPanel.ConstrainTargetToBounds(target, immediate: true);
			}
			if (this.onReposition != null)
			{
				this.onReposition();
			}
		}
		else
		{
			this.repositionNow = true;
		}
	}

	private void RepositionVariableSize(List<Transform> children)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = ((this.columns <= 0) ? 1 : (children.Count / this.columns + 1));
		int num4 = ((this.columns <= 0) ? children.Count : this.columns);
		Bounds[,] array = new Bounds[num3, num4];
		Bounds[] array2 = new Bounds[num4];
		Bounds[] array3 = new Bounds[num3];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			Transform obj = children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(obj);
			Vector3 localScale = obj.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num6, num5] = bounds;
			array2[num5].Encapsulate(bounds);
			array3[num6].Encapsulate(bounds);
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
			}
		}
		num5 = 0;
		num6 = 0;
		int j = 0;
		for (int count2 = children.Count; j < count2; j++)
		{
			Transform obj2 = children[j];
			Bounds bounds2 = array[num6, num5];
			Bounds bounds3 = array2[num5];
			Bounds bounds4 = array3[num6];
			Vector3 localPosition = obj2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x += bounds2.min.x - bounds3.min.x + this.padding.x;
			if (this.direction == Direction.Down)
			{
				localPosition.y = 0f - num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - this.padding.y;
			}
			else
			{
				localPosition.y = num2 + bounds2.extents.y - bounds2.center.y;
				localPosition.y += (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - this.padding.y;
			}
			num += bounds3.max.x - bounds3.min.x + this.padding.x * 2f;
			obj2.localPosition = localPosition;
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
				num = 0f;
				num2 += bounds4.size.y + this.padding.y * 2f;
			}
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.keepWithinPanel)
		{
			this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			this.mDrag = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
		}
		this.Reposition();
	}
}
