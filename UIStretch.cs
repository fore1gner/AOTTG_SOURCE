using UnityEngine;

[AddComponentMenu("NGUI/UI/Stretch")]
[ExecuteInEditMode]
public class UIStretch : MonoBehaviour
{
	public enum Style
	{
		None,
		Horizontal,
		Vertical,
		Both,
		BasedOnHeight,
		FillKeepingRatio,
		FitInternalKeepingRatio
	}

	public Vector2 initialSize = Vector2.one;

	private Animation mAnim;

	private Rect mRect;

	private UIRoot mRoot;

	private Transform mTrans;

	public UIPanel panelContainer;

	public Vector2 relativeSize = Vector2.one;

	public Style style;

	public Camera uiCamera;

	public UIWidget widgetContainer;

	private void Awake()
	{
		this.mAnim = base.animation;
		this.mRect = default(Rect);
		this.mTrans = base.transform;
	}

	private void Start()
	{
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
	}

	private void Update()
	{
		if ((!(this.mAnim == null) && this.mAnim.isPlaying) || this.style == Style.None)
		{
			return;
		}
		float num = 1f;
		if (this.panelContainer != null)
		{
			if (this.panelContainer.clipping == UIDrawCall.Clipping.None)
			{
				this.mRect.xMin = (float)(-Screen.width) * 0.5f;
				this.mRect.yMin = (float)(-Screen.height) * 0.5f;
				this.mRect.xMax = 0f - this.mRect.xMin;
				this.mRect.yMax = 0f - this.mRect.yMin;
			}
			else
			{
				Vector4 clipRange = this.panelContainer.clipRange;
				this.mRect.x = clipRange.x - clipRange.z * 0.5f;
				this.mRect.y = clipRange.y - clipRange.w * 0.5f;
				this.mRect.width = clipRange.z;
				this.mRect.height = clipRange.w;
			}
		}
		else if (this.widgetContainer != null)
		{
			Transform cachedTransform = this.widgetContainer.cachedTransform;
			Vector3 localScale = cachedTransform.localScale;
			Vector3 localPosition = cachedTransform.localPosition;
			Vector3 vector = this.widgetContainer.relativeSize;
			Vector3 vector2 = this.widgetContainer.pivotOffset;
			vector2.y -= 1f;
			vector2.x *= this.widgetContainer.relativeSize.x * localScale.x;
			vector2.y *= this.widgetContainer.relativeSize.y * localScale.y;
			this.mRect.x = localPosition.x + vector2.x;
			this.mRect.y = localPosition.y + vector2.y;
			this.mRect.width = vector.x * localScale.x;
			this.mRect.height = vector.y * localScale.y;
		}
		else
		{
			if (!(this.uiCamera != null))
			{
				return;
			}
			this.mRect = this.uiCamera.pixelRect;
			if (this.mRoot != null)
			{
				num = this.mRoot.pixelSizeAdjustment;
			}
		}
		float num2 = this.mRect.width;
		float num3 = this.mRect.height;
		if (num != 1f && num3 > 1f)
		{
			float num4 = (float)this.mRoot.activeHeight / num3;
			num2 *= num4;
			num3 *= num4;
		}
		Vector3 localScale2 = this.mTrans.localScale;
		if (this.style == Style.BasedOnHeight)
		{
			localScale2.x = this.relativeSize.x * num3;
			localScale2.y = this.relativeSize.y * num3;
		}
		else if (this.style == Style.FillKeepingRatio)
		{
			float num5 = num2 / num3;
			if (this.initialSize.x / this.initialSize.y < num5)
			{
				float num6 = num2 / this.initialSize.x;
				localScale2.x = num2;
				localScale2.y = this.initialSize.y * num6;
			}
			else
			{
				float num7 = num3 / this.initialSize.y;
				localScale2.x = this.initialSize.x * num7;
				localScale2.y = num3;
			}
		}
		else if (this.style == Style.FitInternalKeepingRatio)
		{
			float num8 = num2 / num3;
			if (this.initialSize.x / this.initialSize.y > num8)
			{
				float num9 = num2 / this.initialSize.x;
				localScale2.x = num2;
				localScale2.y = this.initialSize.y * num9;
			}
			else
			{
				float num10 = num3 / this.initialSize.y;
				localScale2.x = this.initialSize.x * num10;
				localScale2.y = num3;
			}
		}
		else
		{
			if (this.style == Style.Both || this.style == Style.Horizontal)
			{
				localScale2.x = this.relativeSize.x * num2;
			}
			if (this.style == Style.Both || this.style == Style.Vertical)
			{
				localScale2.y = this.relativeSize.y * num3;
			}
		}
		if (this.mTrans.localScale != localScale2)
		{
			this.mTrans.localScale = localScale2;
		}
	}
}
