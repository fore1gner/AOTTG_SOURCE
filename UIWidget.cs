using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class UIWidget : MonoBehaviour
{
	public enum Pivot
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight
	}

	[CompilerGenerated]
	private static Comparison<UIWidget> famScache14;

	protected bool mChanged = true;

	[SerializeField]
	[HideInInspector]
	private Color mColor = Color.white;

	[SerializeField]
	[HideInInspector]
	private int mDepth;

	private Vector3 mDiffPos;

	private Quaternion mDiffRot;

	private Vector3 mDiffScale;

	private bool mForceVisible;

	private UIGeometry mGeom = new UIGeometry();

	protected GameObject mGo;

	private float mLastAlpha;

	private Matrix4x4 mLocalToPanel;

	[HideInInspector]
	[SerializeField]
	protected Material mMat;

	private Vector3 mOldV0;

	private Vector3 mOldV1;

	protected UIPanel mPanel;

	[HideInInspector]
	[SerializeField]
	private Pivot mPivot = Pivot.Center;

	protected bool mPlayMode = true;

	[SerializeField]
	[HideInInspector]
	protected Texture mTex;

	protected Transform mTrans;

	private bool mVisibleByPanel = true;

	public float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			Color color = this.mColor;
			color.a = value;
			this.color = color;
		}
	}

	public virtual Vector4 border => Vector4.zero;

	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

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

	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (!this.mColor.Equals(value))
			{
				this.mColor = value;
				this.mChanged = true;
			}
		}
	}

	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				this.mDepth = value;
				if (this.mPanel != null)
				{
					this.mPanel.MarkMaterialAsChanged(this.material, sort: true);
				}
			}
		}
	}

	public float finalAlpha
	{
		get
		{
			if (this.mPanel == null)
			{
				this.CreatePanel();
			}
			if (!(this.mPanel == null))
			{
				return this.mColor.a * this.mPanel.alpha;
			}
			return this.mColor.a;
		}
	}

	public bool isVisible
	{
		get
		{
			if (this.mVisibleByPanel)
			{
				return this.finalAlpha > 0.001f;
			}
			return false;
		}
	}

	public virtual bool keepMaterial => false;

	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			if (material != null)
			{
				if (material.mainTexture != null)
				{
					this.mTex = material.mainTexture;
				}
				else if (this.mTex != null)
				{
					if (this.mPanel != null)
					{
						this.mPanel.RemoveWidget(this);
					}
					this.mPanel = null;
					this.mMat.mainTexture = this.mTex;
					if (base.enabled)
					{
						this.CreatePanel();
					}
				}
			}
			return this.mTex;
		}
		set
		{
			Material material = this.material;
			if (!(material == null) && !(material.mainTexture != value))
			{
				return;
			}
			if (this.mPanel != null)
			{
				this.mPanel.RemoveWidget(this);
			}
			this.mPanel = null;
			this.mTex = value;
			material = this.material;
			if (material != null)
			{
				material.mainTexture = value;
				if (base.enabled)
				{
					this.CreatePanel();
				}
			}
		}
	}

	public virtual Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				if (this.mMat != null && this.mPanel != null)
				{
					this.mPanel.RemoveWidget(this);
				}
				this.mPanel = null;
				this.mMat = value;
				this.mTex = null;
				if (this.mMat != null)
				{
					this.CreatePanel();
				}
			}
		}
	}

	public UIPanel panel
	{
		get
		{
			if (this.mPanel == null)
			{
				this.CreatePanel();
			}
			return this.mPanel;
		}
		set
		{
			this.mPanel = value;
		}
	}

	public Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				Vector3 vector = NGUIMath.CalculateWidgetCorners(this)[0];
				this.mPivot = value;
				this.mChanged = true;
				Vector3 vector2 = NGUIMath.CalculateWidgetCorners(this)[0];
				Transform obj = this.cachedTransform;
				Vector3 position = obj.position;
				float z = obj.localPosition.z;
				position.x += vector.x - vector2.x;
				position.y += vector.y - vector2.y;
				this.cachedTransform.position = position;
				position = this.cachedTransform.localPosition;
				position.x = Mathf.Round(position.x);
				position.y = Mathf.Round(position.y);
				position.z = z;
				this.cachedTransform.localPosition = position;
			}
		}
	}

	public Vector2 pivotOffset
	{
		get
		{
			Vector2 zero = Vector2.zero;
			Vector4 vector = this.relativePadding;
			Pivot pivot = this.pivot;
			switch (pivot)
			{
			case Pivot.Top:
			case Pivot.Center:
			case Pivot.Bottom:
				zero.x = (vector.x - vector.z - 1f) * 0.5f;
				break;
			case Pivot.TopRight:
			case Pivot.Right:
			case Pivot.BottomRight:
				zero.x = -1f - vector.z;
				break;
			default:
				zero.x = vector.x;
				break;
			}
			switch (pivot)
			{
			case Pivot.Left:
			case Pivot.Center:
			case Pivot.Right:
				zero.y = (vector.w - vector.y + 1f) * 0.5f;
				return zero;
			case Pivot.BottomLeft:
			case Pivot.Bottom:
			case Pivot.BottomRight:
				zero.y = 1f + vector.w;
				return zero;
			default:
				zero.y = 0f - vector.y;
				return zero;
			}
		}
	}

	public virtual bool pixelPerfectAfterResize => false;

	public virtual Vector4 relativePadding => Vector4.zero;

	public virtual Vector2 relativeSize => Vector2.one;

	protected virtual void Awake()
	{
		this.mGo = base.gameObject;
		this.mPlayMode = Application.isPlaying;
	}

	public void CheckLayer()
	{
		if (this.mPanel != null && this.mPanel.gameObject.layer != base.gameObject.layer)
		{
			Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.mPanel.gameObject.layer;
		}
	}

	[Obsolete("Use ParentHasChanged() instead")]
	public void CheckParent()
	{
		this.ParentHasChanged();
	}

	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		return 0;
	}

	public void CreatePanel()
	{
		if (this.mPanel == null && base.enabled && NGUITools.GetActive(base.gameObject) && this.material != null)
		{
			this.mPanel = UIPanel.Find(this.cachedTransform);
			if (this.mPanel != null)
			{
				this.CheckLayer();
				this.mPanel.AddWidget(this);
				this.mChanged = true;
			}
		}
	}

	public virtual void MakePixelPerfect()
	{
		Vector3 localScale = this.cachedTransform.localScale;
		int num = Mathf.RoundToInt(localScale.x);
		int num2 = Mathf.RoundToInt(localScale.y);
		localScale.x = num;
		localScale.y = num2;
		localScale.z = 1f;
		Vector3 localPosition = this.cachedTransform.localPosition;
		localPosition.z = Mathf.RoundToInt(localPosition.z);
		if (num % 2 == 1 && (this.pivot == Pivot.Top || this.pivot == Pivot.Center || this.pivot == Pivot.Bottom))
		{
			localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
		}
		else
		{
			localPosition.x = Mathf.Round(localPosition.x);
		}
		if (num2 % 2 == 1 && (this.pivot == Pivot.Left || this.pivot == Pivot.Center || this.pivot == Pivot.Right))
		{
			localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
		}
		else
		{
			localPosition.y = Mathf.Round(localPosition.y);
		}
		this.cachedTransform.localPosition = localPosition;
		this.cachedTransform.localScale = localScale;
	}

	public virtual void MarkAsChanged()
	{
		this.mChanged = true;
		if (this.mPanel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !Application.isPlaying && this.material != null)
		{
			this.mPanel.AddWidget(this);
			this.CheckLayer();
		}
	}

	public void MarkAsChangedLite()
	{
		this.mChanged = true;
	}

	private void OnDestroy()
	{
		if (this.mPanel != null)
		{
			this.mPanel.RemoveWidget(this);
			this.mPanel = null;
		}
	}

	private void OnDisable()
	{
		if (!this.keepMaterial)
		{
			this.material = null;
		}
		else if (this.mPanel != null)
		{
			this.mPanel.RemoveWidget(this);
		}
		this.mPanel = null;
	}

	protected virtual void OnEnable()
	{
		this.mChanged = true;
		if (!this.keepMaterial)
		{
			this.mMat = null;
			this.mTex = null;
		}
		this.mPanel = null;
	}

	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}

	protected virtual void OnStart()
	{
	}

	public void ParentHasChanged()
	{
		if (!(this.mPanel != null))
		{
			return;
		}
		UIPanel uIPanel = UIPanel.Find(this.cachedTransform);
		if (this.mPanel != uIPanel)
		{
			this.mPanel.RemoveWidget(this);
			if (!this.keepMaterial || Application.isPlaying)
			{
				this.material = null;
			}
			this.mPanel = null;
			this.CreatePanel();
		}
	}

	public static BetterList<UIWidget> Raycast(GameObject root, Vector2 mousePos)
	{
		BetterList<UIWidget> betterList = new BetterList<UIWidget>();
		UICamera uICamera = UICamera.FindCameraForLayer(root.layer);
		if (uICamera != null)
		{
			Camera cachedCamera = uICamera.cachedCamera;
			UIWidget[] componentsInChildren = root.GetComponentsInChildren<UIWidget>();
			foreach (UIWidget uIWidget in componentsInChildren)
			{
				if (NGUIMath.DistanceToRectangle(NGUIMath.CalculateWidgetCorners(uIWidget), mousePos, cachedCamera) == 0f)
				{
					betterList.Add(uIWidget);
				}
			}
			betterList.Sort((UIWidget w1, UIWidget w2) => w2.mDepth.CompareTo(w1.mDepth));
		}
		return betterList;
	}

	private void Start()
	{
		this.OnStart();
		this.CreatePanel();
	}

	public virtual void Update()
	{
		if (this.mPanel == null)
		{
			this.CreatePanel();
		}
	}

	public bool UpdateGeometry(UIPanel p, bool forceVisible)
	{
		if (this.material != null && p != null)
		{
			this.mPanel = p;
			bool flag = false;
			float num = this.finalAlpha;
			bool flag2 = num > 0.001f;
			bool flag3 = forceVisible || this.mVisibleByPanel;
			if (this.cachedTransform.hasChanged)
			{
				this.mTrans.hasChanged = false;
				if (!this.mPanel.widgetsAreStatic)
				{
					Vector2 vector = this.relativeSize;
					Vector2 vector2 = this.pivotOffset;
					Vector4 vector3 = this.relativePadding;
					float num2 = vector2.x * vector.x - vector3.x;
					float num3 = vector2.y * vector.y + vector3.y;
					float x = num2 + vector.x + vector3.x + vector3.z;
					float y = num3 - vector.y - vector3.y - vector3.w;
					this.mLocalToPanel = p.worldToLocal * this.cachedTransform.localToWorldMatrix;
					flag = true;
					Vector3 v = new Vector3(num2, num3, 0f);
					Vector3 v2 = new Vector3(x, y, 0f);
					v = this.mLocalToPanel.MultiplyPoint3x4(v);
					v2 = this.mLocalToPanel.MultiplyPoint3x4(v2);
					if (Vector3.SqrMagnitude(this.mOldV0 - v) > 1E-06f || Vector3.SqrMagnitude(this.mOldV1 - v2) > 1E-06f)
					{
						this.mChanged = true;
						this.mOldV0 = v;
						this.mOldV1 = v2;
					}
				}
				if (flag2 || this.mForceVisible != forceVisible)
				{
					this.mForceVisible = forceVisible;
					flag3 = forceVisible || this.mPanel.IsVisible(this);
				}
			}
			else if (flag2 && this.mForceVisible != forceVisible)
			{
				this.mForceVisible = forceVisible;
				flag3 = this.mPanel.IsVisible(this);
			}
			if (this.mVisibleByPanel != flag3)
			{
				this.mVisibleByPanel = flag3;
				this.mChanged = true;
			}
			if (this.mVisibleByPanel && this.mLastAlpha != num)
			{
				this.mChanged = true;
			}
			this.mLastAlpha = num;
			if (this.mChanged)
			{
				this.mChanged = false;
				if (this.isVisible)
				{
					this.mGeom.Clear();
					this.OnFill(this.mGeom.verts, this.mGeom.uvs, this.mGeom.cols);
					if (this.mGeom.hasVertices)
					{
						Vector3 vector4 = this.pivotOffset;
						Vector2 vector5 = this.relativeSize;
						vector4.x *= vector5.x;
						vector4.y *= vector5.y;
						if (!flag)
						{
							this.mLocalToPanel = p.worldToLocal * this.cachedTransform.localToWorldMatrix;
						}
						this.mGeom.ApplyOffset(vector4);
						this.mGeom.ApplyTransform(this.mLocalToPanel, p.generateNormals);
					}
					return true;
				}
				if (this.mGeom.hasVertices)
				{
					this.mGeom.Clear();
					return true;
				}
			}
		}
		return false;
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		this.mGeom.WriteToBuffers(v, u, c, n, t);
	}
}
