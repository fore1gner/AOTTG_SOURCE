using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Panel")]
public class UIPanel : MonoBehaviour
{
	public enum DebugInfo
	{
		None,
		Gizmos,
		Geometry
	}

	public delegate void OnChangeDelegate();

	public bool cullWhileDragging;

	public bool depthPass;

	public bool generateNormals;

	[SerializeField]
	[HideInInspector]
	private float mAlpha = 1f;

	private Camera mCam;

	private BetterList<Material> mChanged = new BetterList<Material>();

	private UIPanel[] mChildPanels;

	[SerializeField]
	[HideInInspector]
	private UIDrawCall.Clipping mClipping;

	[HideInInspector]
	[SerializeField]
	private Vector4 mClipRange = Vector4.zero;

	[HideInInspector]
	[SerializeField]
	private Vector2 mClipSoftness = new Vector2(40f, 40f);

	private BetterList<Color32> mCols = new BetterList<Color32>();

	private float mCullTime;

	[SerializeField]
	[HideInInspector]
	private DebugInfo mDebugInfo = DebugInfo.Gizmos;

	private bool mDepthChanged;

	private BetterList<UIDrawCall> mDrawCalls = new BetterList<UIDrawCall>();

	private GameObject mGo;

	private int mLayer = -1;

	private float mMatrixTime;

	private Vector2 mMax = Vector2.zero;

	private Vector2 mMin = Vector2.zero;

	private BetterList<Vector3> mNorms = new BetterList<Vector3>();

	private BetterList<Vector4> mTans = new BetterList<Vector4>();

	private static float[] mTemp = new float[4];

	private Transform mTrans;

	private float mUpdateTime;

	private BetterList<Vector2> mUvs = new BetterList<Vector2>();

	private BetterList<Vector3> mVerts = new BetterList<Vector3>();

	private BetterList<UIWidget> mWidgets = new BetterList<UIWidget>();

	public OnChangeDelegate onChange;

	public bool showInPanelTool = true;

	public bool widgetsAreStatic;

	[HideInInspector]
	public Matrix4x4 worldToLocal = Matrix4x4.identity;

	public float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mAlpha != num)
			{
				this.mAlpha = num;
				for (int i = 0; i < this.mDrawCalls.size; i++)
				{
					UIDrawCall uIDrawCall = this.mDrawCalls[i];
					this.MarkMaterialAsChanged(uIDrawCall.material, sort: false);
				}
				for (int j = 0; j < this.mWidgets.size; j++)
				{
					this.mWidgets[j].MarkAsChangedLite();
				}
			}
		}
	}

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

	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mClipping = value;
				this.mMatrixTime = 0f;
				this.UpdateDrawcalls();
			}
		}
	}

	public Vector4 clipRange
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (this.mClipRange != value)
			{
				this.mCullTime = ((this.mCullTime != 0f) ? (Time.realtimeSinceStartup + 0.15f) : 0.001f);
				this.mClipRange = value;
				this.mMatrixTime = 0f;
				this.UpdateDrawcalls();
			}
		}
	}

	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
				this.UpdateDrawcalls();
			}
		}
	}

	public DebugInfo debugInfo
	{
		get
		{
			return this.mDebugInfo;
		}
		set
		{
			if (this.mDebugInfo != value)
			{
				this.mDebugInfo = value;
				BetterList<UIDrawCall> betterList = this.drawCalls;
				HideFlags hideFlags = ((this.mDebugInfo != DebugInfo.Geometry) ? HideFlags.HideAndDontSave : (HideFlags.DontSave | HideFlags.NotEditable));
				int i = 0;
				for (int size = betterList.size; i < size; i++)
				{
					GameObject obj = betterList[i].gameObject;
					NGUITools.SetActiveSelf(obj, state: false);
					obj.hideFlags = hideFlags;
					NGUITools.SetActiveSelf(obj, state: true);
				}
			}
		}
	}

	public BetterList<UIDrawCall> drawCalls
	{
		get
		{
			int num = this.mDrawCalls.size;
			while (num > 0)
			{
				if (this.mDrawCalls[--num] == null)
				{
					this.mDrawCalls.RemoveAt(num);
				}
			}
			return this.mDrawCalls;
		}
	}

	public BetterList<UIWidget> widgets => this.mWidgets;

	public void AddWidget(UIWidget w)
	{
		if (w != null && !this.mWidgets.Contains(w))
		{
			this.mWidgets.Add(w);
			if (!this.mChanged.Contains(w.material))
			{
				this.mChanged.Add(w.material);
			}
			this.mDepthChanged = true;
		}
	}

	private void Awake()
	{
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
	}

	public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		float num = this.clipRange.z * 0.5f;
		float num2 = this.clipRange.w * 0.5f;
		Vector2 minRect = new Vector2(min.x, min.y);
		Vector2 maxRect = new Vector2(max.x, max.y);
		Vector2 minArea = new Vector2(this.clipRange.x - num, this.clipRange.y - num2);
		Vector2 maxArea = new Vector2(this.clipRange.x + num, this.clipRange.y + num2);
		if (this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += this.clipSoftness.x;
			minArea.y += this.clipSoftness.y;
			maxArea.x -= this.clipSoftness.x;
			maxArea.y -= this.clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(this.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref targetBounds, immediate);
	}

	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = this.CalculateConstrainOffset(targetBounds.min, targetBounds.max);
		if (vector.magnitude <= 0f)
		{
			return false;
		}
		if (immediate)
		{
			target.localPosition += vector;
			targetBounds.center += vector;
			SpringPosition component = target.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
		}
		else
		{
			SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + vector, 13f);
			springPosition.ignoreTimeScale = true;
			springPosition.worldSpace = false;
		}
		return true;
	}

	private void Fill(Material mat)
	{
		int num = 0;
		while (num < this.mWidgets.size)
		{
			UIWidget uIWidget = this.mWidgets.buffer[num];
			if (uIWidget == null)
			{
				this.mWidgets.RemoveAt(num);
				continue;
			}
			if (uIWidget.material == mat && uIWidget.isVisible)
			{
				if (!(uIWidget.panel == this))
				{
					this.mWidgets.RemoveAt(num);
					continue;
				}
				if (this.generateNormals)
				{
					uIWidget.WriteToBuffers(this.mVerts, this.mUvs, this.mCols, this.mNorms, this.mTans);
				}
				else
				{
					uIWidget.WriteToBuffers(this.mVerts, this.mUvs, this.mCols, null, null);
				}
			}
			num++;
		}
		if (this.mVerts.size > 0)
		{
			UIDrawCall drawCall = this.GetDrawCall(mat, createIfMissing: true);
			drawCall.depthPass = this.depthPass && this.mClipping == UIDrawCall.Clipping.None;
			drawCall.Set(this.mVerts, (!this.generateNormals) ? null : this.mNorms, (!this.generateNormals) ? null : this.mTans, this.mUvs, this.mCols);
		}
		else
		{
			UIDrawCall drawCall2 = this.GetDrawCall(mat, createIfMissing: false);
			if (drawCall2 != null)
			{
				this.mDrawCalls.Remove(drawCall2);
				NGUITools.DestroyImmediate(drawCall2.gameObject);
			}
		}
		this.mVerts.Clear();
		this.mNorms.Clear();
		this.mTans.Clear();
		this.mUvs.Clear();
		this.mCols.Clear();
	}

	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, createIfMissing: true);
	}

	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		Transform transform = trans;
		UIPanel uIPanel = null;
		while (uIPanel == null && trans != null)
		{
			uIPanel = trans.GetComponent<UIPanel>();
			if (uIPanel != null || trans.parent == null)
			{
				break;
			}
			trans = trans.parent;
		}
		if (createIfMissing && uIPanel == null && trans != transform)
		{
			uIPanel = trans.gameObject.AddComponent<UIPanel>();
			UIPanel.SetChildLayer(uIPanel.cachedTransform, uIPanel.cachedGameObject.layer);
		}
		return uIPanel;
	}

	private UIDrawCall GetDrawCall(Material mat, bool createIfMissing)
	{
		int i = 0;
		for (int size = this.drawCalls.size; i < size; i++)
		{
			UIDrawCall uIDrawCall = this.drawCalls.buffer[i];
			if (uIDrawCall.material == mat)
			{
				return uIDrawCall;
			}
		}
		UIDrawCall uIDrawCall2 = null;
		if (createIfMissing)
		{
			GameObject obj = new GameObject("_UIDrawCall [" + mat.name + "]");
			Object.DontDestroyOnLoad(obj);
			obj.layer = this.cachedGameObject.layer;
			uIDrawCall2 = obj.AddComponent<UIDrawCall>();
			uIDrawCall2.material = mat;
			this.mDrawCalls.Add(uIDrawCall2);
		}
		return uIDrawCall2;
	}

	public bool IsVisible(UIWidget w)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (!w.enabled || !NGUITools.GetActive(w.cachedGameObject) || w.alpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		Vector2 relativeSize = w.relativeSize;
		Vector2 vector = Vector2.Scale(w.pivotOffset, relativeSize);
		Vector2 vector2 = vector;
		vector.x += relativeSize.x;
		vector.y -= relativeSize.y;
		Transform obj = w.cachedTransform;
		Vector3 a = obj.TransformPoint(vector);
		Vector3 b = obj.TransformPoint(new Vector2(vector.x, vector2.y));
		Vector3 c = obj.TransformPoint(new Vector2(vector2.x, vector.y));
		Vector3 d = obj.TransformPoint(vector2);
		return this.IsVisible(a, b, c, d);
	}

	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping != 0)
		{
			this.UpdateTransformMatrix();
			Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
			if (vector.x < this.mMin.x)
			{
				return false;
			}
			if (vector.y < this.mMin.y)
			{
				return false;
			}
			if (vector.x > this.mMax.x)
			{
				return false;
			}
			if (vector.y > this.mMax.y)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float num = Mathf.Min(UIPanel.mTemp);
		float num2 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float num3 = Mathf.Min(UIPanel.mTemp);
		float num4 = Mathf.Max(UIPanel.mTemp);
		if (num2 < this.mMin.x)
		{
			return false;
		}
		if (num4 < this.mMin.y)
		{
			return false;
		}
		if (num > this.mMax.x)
		{
			return false;
		}
		if (num3 > this.mMax.y)
		{
			return false;
		}
		return true;
	}

	private void LateUpdate()
	{
		this.mUpdateTime = Time.realtimeSinceStartup;
		this.UpdateTransformMatrix();
		if (this.mLayer != this.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			UICamera uICamera = UICamera.FindCameraForLayer(this.mLayer);
			this.mCam = ((uICamera == null) ? NGUITools.FindCameraForLayer(this.mLayer) : uICamera.cachedCamera);
			UIPanel.SetChildLayer(this.cachedTransform, this.mLayer);
			int i = 0;
			for (int size = this.drawCalls.size; i < size; i++)
			{
				this.mDrawCalls.buffer[i].gameObject.layer = this.mLayer;
			}
		}
		bool forceVisible = !this.cullWhileDragging && (this.clipping == UIDrawCall.Clipping.None || this.mCullTime > this.mUpdateTime);
		int j = 0;
		for (int size2 = this.mWidgets.size; j < size2; j++)
		{
			UIWidget uIWidget = this.mWidgets[j];
			if (uIWidget.UpdateGeometry(this, forceVisible) && !this.mChanged.Contains(uIWidget.material))
			{
				this.mChanged.Add(uIWidget.material);
			}
		}
		if (this.mChanged.size != 0 && this.onChange != null)
		{
			this.onChange();
		}
		if (this.mDepthChanged)
		{
			this.mDepthChanged = false;
			this.mWidgets.Sort(UIWidget.CompareFunc);
		}
		int k = 0;
		for (int size3 = this.mChanged.size; k < size3; k++)
		{
			this.Fill(this.mChanged.buffer[k]);
		}
		this.UpdateDrawcalls();
		this.mChanged.Clear();
	}

	public void MarkMaterialAsChanged(Material mat, bool sort)
	{
		if (mat != null)
		{
			if (sort)
			{
				this.mDepthChanged = true;
			}
			if (!this.mChanged.Contains(mat))
			{
				this.mChanged.Add(mat);
			}
		}
	}

	private void OnDisable()
	{
		int num = this.mDrawCalls.size;
		while (num > 0)
		{
			UIDrawCall uIDrawCall = this.mDrawCalls.buffer[--num];
			if (uIDrawCall != null)
			{
				NGUITools.DestroyImmediate(uIDrawCall.gameObject);
			}
		}
		this.mDrawCalls.Clear();
		this.mChanged.Clear();
	}

	private void OnEnable()
	{
		int num = 0;
		while (num < this.mWidgets.size)
		{
			UIWidget uIWidget = this.mWidgets.buffer[num];
			if (uIWidget != null)
			{
				this.MarkMaterialAsChanged(uIWidget.material, sort: true);
				num++;
			}
			else
			{
				this.mWidgets.RemoveAt(num);
			}
		}
	}

	public void Refresh()
	{
		UIWidget[] componentsInChildren = base.GetComponentsInChildren<UIWidget>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].Update();
		}
		this.LateUpdate();
	}

	public void RemoveWidget(UIWidget w)
	{
		if (w != null && w != null && this.mWidgets.Remove(w) && w.material != null)
		{
			this.mChanged.Add(w.material);
		}
	}

	public void SetAlphaRecursive(float val, bool rebuildList)
	{
		if (rebuildList || this.mChildPanels == null)
		{
			this.mChildPanels = base.GetComponentsInChildren<UIPanel>(includeInactive: true);
		}
		int i = 0;
		for (int num = this.mChildPanels.Length; i < num; i++)
		{
			this.mChildPanels[i].alpha = val;
		}
	}

	private static void SetChildLayer(Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if (child.GetComponent<UIPanel>() == null)
			{
				if (child.GetComponent<UIWidget>() != null)
				{
					child.gameObject.layer = layer;
				}
				UIPanel.SetChildLayer(child, layer);
			}
		}
	}

	private void Start()
	{
		this.mLayer = this.mGo.layer;
		UICamera uICamera = UICamera.FindCameraForLayer(this.mLayer);
		this.mCam = ((uICamera == null) ? NGUITools.FindCameraForLayer(this.mLayer) : uICamera.cachedCamera);
	}

	public void UpdateDrawcalls()
	{
		Vector4 vector = Vector4.zero;
		if (this.mClipping != 0)
		{
			vector = new Vector4(this.mClipRange.x, this.mClipRange.y, this.mClipRange.z * 0.5f, this.mClipRange.w * 0.5f);
		}
		if (vector.z == 0f)
		{
			vector.z = (float)Screen.width * 0.5f;
		}
		if (vector.w == 0f)
		{
			vector.w = (float)Screen.height * 0.5f;
		}
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsWebPlayer || platform == RuntimePlatform.WindowsEditor)
		{
			vector.x -= 0.5f;
			vector.y += 0.5f;
		}
		Transform transform = this.cachedTransform;
		int i = 0;
		for (int size = this.mDrawCalls.size; i < size; i++)
		{
			UIDrawCall obj = this.mDrawCalls.buffer[i];
			obj.clipping = this.mClipping;
			obj.clipRange = vector;
			obj.clipSoftness = this.mClipSoftness;
			obj.depthPass = this.depthPass && this.mClipping == UIDrawCall.Clipping.None;
			Transform obj2 = obj.transform;
			obj2.position = transform.position;
			obj2.rotation = transform.rotation;
			obj2.localScale = transform.lossyScale;
		}
	}

	private void UpdateTransformMatrix()
	{
		if (this.mUpdateTime != 0f && this.mMatrixTime == this.mUpdateTime)
		{
			return;
		}
		this.mMatrixTime = this.mUpdateTime;
		this.worldToLocal = this.cachedTransform.worldToLocalMatrix;
		if (this.mClipping != 0)
		{
			Vector2 vector = new Vector2(this.mClipRange.z, this.mClipRange.w);
			if (vector.x == 0f)
			{
				vector.x = ((this.mCam != null) ? this.mCam.pixelWidth : ((float)Screen.width));
			}
			if (vector.y == 0f)
			{
				vector.y = ((this.mCam != null) ? this.mCam.pixelHeight : ((float)Screen.height));
			}
			vector *= 0.5f;
			this.mMin.x = this.mClipRange.x - vector.x;
			this.mMin.y = this.mClipRange.y - vector.y;
			this.mMax.x = this.mClipRange.x + vector.x;
			this.mMax.y = this.mClipRange.y + vector.y;
		}
	}
}
