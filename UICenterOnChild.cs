using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center On Child")]
public class UICenterOnChild : MonoBehaviour
{
	private GameObject mCenteredObject;

	private UIDraggablePanel mDrag;

	public SpringPanel.OnFinished onFinished;

	public float springStrength = 8f;

	public GameObject centeredObject => this.mCenteredObject;

	private void OnDragFinished()
	{
		if (base.enabled)
		{
			this.Recenter();
		}
	}

	private void OnEnable()
	{
		this.Recenter();
	}

	public void Recenter()
	{
		if (this.mDrag == null)
		{
			this.mDrag = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
			if (this.mDrag == null)
			{
				Debug.LogWarning(string.Concat(base.GetType(), " requires ", typeof(UIDraggablePanel), " on a parent object in order to work"), this);
				base.enabled = false;
				return;
			}
			this.mDrag.onDragFinished = OnDragFinished;
			if (this.mDrag.horizontalScrollBar != null)
			{
				this.mDrag.horizontalScrollBar.onDragFinished = OnDragFinished;
			}
			if (this.mDrag.verticalScrollBar != null)
			{
				this.mDrag.verticalScrollBar.onDragFinished = OnDragFinished;
			}
		}
		if (!(this.mDrag.panel != null))
		{
			return;
		}
		Vector4 clipRange = this.mDrag.panel.clipRange;
		Transform cachedTransform = this.mDrag.panel.cachedTransform;
		Vector3 localPosition = cachedTransform.localPosition;
		localPosition.x += clipRange.x;
		localPosition.y += clipRange.y;
		localPosition = cachedTransform.parent.TransformPoint(localPosition);
		Vector3 vector = localPosition - this.mDrag.currentMomentum * (this.mDrag.momentumAmount * 0.1f);
		this.mDrag.currentMomentum = Vector3.zero;
		float num = float.MaxValue;
		Transform transform = null;
		Transform transform2 = base.transform;
		int i = 0;
		for (int childCount = transform2.childCount; i < childCount; i++)
		{
			Transform child = transform2.GetChild(i);
			float num2 = Vector3.SqrMagnitude(child.position - vector);
			if (num2 < num)
			{
				num = num2;
				transform = child;
			}
		}
		if (transform != null)
		{
			this.mCenteredObject = transform.gameObject;
			Vector3 vector2 = cachedTransform.InverseTransformPoint(transform.position);
			Vector3 vector3 = cachedTransform.InverseTransformPoint(localPosition);
			Vector3 vector4 = vector2 - vector3;
			if (this.mDrag.scale.x == 0f)
			{
				vector4.x = 0f;
			}
			if (this.mDrag.scale.y == 0f)
			{
				vector4.y = 0f;
			}
			if (this.mDrag.scale.z == 0f)
			{
				vector4.z = 0f;
			}
			SpringPanel.Begin(this.mDrag.gameObject, cachedTransform.localPosition - vector4, this.springStrength).onFinished = this.onFinished;
		}
		else
		{
			this.mCenteredObject = null;
		}
	}
}
