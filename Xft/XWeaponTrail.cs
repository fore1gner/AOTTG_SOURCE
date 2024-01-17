using System.Collections.Generic;
using UnityEngine;

namespace Xft;

public class XWeaponTrail : MonoBehaviour
{
	public class Element
	{
		public Vector3 PointEnd;

		public Vector3 PointStart;

		public Vector3 Pos => (this.PointStart + this.PointEnd) / 2f;

		public Element()
		{
		}

		public Element(Vector3 start, Vector3 end)
		{
			this.PointStart = start;
			this.PointEnd = end;
		}
	}

	public float Fps = 60f;

	public int Granularity = 60;

	public int MaxFrame = 14;

	protected float mElapsedTime;

	protected float mFadeElapsedime;

	protected float mFadeT = 1f;

	protected float mFadeTime = 1f;

	protected Element mHeadElem = new Element();

	protected bool mInited;

	protected bool mIsFading;

	protected GameObject mMeshObj;

	protected List<Element> mSnapshotList = new List<Element>();

	protected Spline mSpline = new Spline();

	protected float mTrailWidth;

	protected VertexPool mVertexPool;

	protected VertexPool.VertexSegment mVertexSegment;

	public Color MyColor = Color.white;

	public Material MyMaterial;

	public Transform PointEnd;

	public Transform PointStart;

	public static string Version = "1.0.1";

	public Vector3 CurHeadPos => (this.PointStart.position + this.PointEnd.position) / 2f;

	public float TrailWidth => this.mTrailWidth;

	public float UpdateInterval => 1f / this.Fps;

	public void Activate()
	{
		this.MaxFrame = 14;
		this.Init();
		if (this.mMeshObj == null)
		{
			this.InitMeshObj();
			return;
		}
		base.gameObject.SetActive(value: true);
		if (this.mMeshObj != null)
		{
			this.mMeshObj.SetActive(value: true);
		}
		this.mFadeT = 1f;
		this.mIsFading = false;
		this.mFadeTime = 1f;
		this.mFadeElapsedime = 0f;
		this.mElapsedTime = 0f;
		for (int i = 0; i < this.mSnapshotList.Count; i++)
		{
			this.mSnapshotList[i].PointStart = this.PointStart.position;
			this.mSnapshotList[i].PointEnd = this.PointEnd.position;
			this.mSpline.ControlPoints[i].Position = this.mSnapshotList[i].Pos;
			this.mSpline.ControlPoints[i].Normal = this.mSnapshotList[i].PointEnd - this.mSnapshotList[i].PointStart;
		}
		this.RefreshSpline();
		this.UpdateVertex();
	}

	public void Deactivate()
	{
		base.gameObject.SetActive(value: false);
		if (this.mMeshObj != null)
		{
			this.mMeshObj.SetActive(value: false);
		}
	}

	public void Init()
	{
		if (!this.mInited)
		{
			this.mTrailWidth = (this.PointStart.position - this.PointEnd.position).magnitude;
			this.InitMeshObj();
			this.InitOriginalElements();
			this.InitSpline();
			this.mInited = true;
		}
	}

	private void InitMeshObj()
	{
		this.mMeshObj = new GameObject("_XWeaponTrailMesh: " + base.gameObject.name);
		this.mMeshObj.layer = base.gameObject.layer;
		this.mMeshObj.SetActive(value: true);
		MeshFilter meshFilter = this.mMeshObj.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = this.mMeshObj.AddComponent<MeshRenderer>();
		meshRenderer.castShadows = false;
		meshRenderer.receiveShadows = false;
		meshRenderer.renderer.sharedMaterial = this.MyMaterial;
		meshFilter.sharedMesh = new Mesh();
		this.mVertexPool = new VertexPool(meshFilter.sharedMesh, this.MyMaterial);
		this.mVertexSegment = this.mVertexPool.GetVertices(this.Granularity * 3, (this.Granularity - 1) * 12);
		this.UpdateIndices();
	}

	private void InitOriginalElements()
	{
		this.mSnapshotList.Clear();
		this.mSnapshotList.Add(new Element(this.PointStart.position, this.PointEnd.position));
		this.mSnapshotList.Add(new Element(this.PointStart.position, this.PointEnd.position));
	}

	private void InitSpline()
	{
		this.mSpline.Granularity = this.Granularity;
		this.mSpline.Clear();
		for (int i = 0; i < this.MaxFrame; i++)
		{
			this.mSpline.AddControlPoint(this.CurHeadPos, this.PointStart.position - this.PointEnd.position);
		}
	}

	public void lateUpdate()
	{
		if (this.mInited)
		{
			this.mVertexPool.LateUpdate();
		}
	}

	private void OnDrawGizmos()
	{
		if (this.PointEnd != null && this.PointStart != null)
		{
			float magnitude = (this.PointStart.position - this.PointEnd.position).magnitude;
			if (magnitude >= float.Epsilon)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(this.PointStart.position, magnitude * 0.04f);
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(this.PointEnd.position, magnitude * 0.04f);
			}
		}
	}

	private void RecordCurElem()
	{
		Element item = new Element(this.PointStart.position, this.PointEnd.position);
		if (this.mSnapshotList.Count < this.MaxFrame)
		{
			this.mSnapshotList.Insert(1, item);
			return;
		}
		this.mSnapshotList.RemoveAt(this.mSnapshotList.Count - 1);
		this.mSnapshotList.Insert(1, item);
	}

	private void RefreshSpline()
	{
		for (int i = 0; i < this.mSnapshotList.Count; i++)
		{
			this.mSpline.ControlPoints[i].Position = this.mSnapshotList[i].Pos;
			this.mSpline.ControlPoints[i].Normal = this.mSnapshotList[i].PointEnd - this.mSnapshotList[i].PointStart;
		}
		this.mSpline.RefreshSpline();
	}

	private void Start()
	{
		this.Init();
	}

	public void StopSmoothly(float fadeTime)
	{
		this.mIsFading = true;
		this.mFadeTime = fadeTime;
	}

	public void update()
	{
		if (!this.mInited)
		{
			return;
		}
		if (this.mMeshObj == null)
		{
			this.InitMeshObj();
			return;
		}
		this.UpdateHeadElem();
		this.mElapsedTime += Time.deltaTime;
		if (this.mElapsedTime >= this.UpdateInterval)
		{
			this.mElapsedTime -= this.UpdateInterval;
			this.RecordCurElem();
			this.RefreshSpline();
			this.UpdateFade();
			this.UpdateVertex();
		}
	}

	private void UpdateFade()
	{
		if (this.mIsFading)
		{
			this.mFadeElapsedime += Time.deltaTime;
			float num = this.mFadeElapsedime / this.mFadeTime;
			this.mFadeT = 1f - num;
			if (this.mFadeT < 0f)
			{
				this.Deactivate();
			}
		}
	}

	private void UpdateHeadElem()
	{
		this.mSnapshotList[0].PointStart = this.PointStart.position;
		this.mSnapshotList[0].PointEnd = this.PointEnd.position;
	}

	private void UpdateIndices()
	{
		VertexPool pool = this.mVertexSegment.Pool;
		for (int i = 0; i < this.Granularity - 1; i++)
		{
			int num = this.mVertexSegment.VertStart + i * 3;
			int num2 = this.mVertexSegment.VertStart + (i + 1) * 3;
			int num3 = this.mVertexSegment.IndexStart + i * 12;
			pool.Indices[num3] = num2;
			pool.Indices[num3 + 1] = num2 + 1;
			pool.Indices[num3 + 2] = num;
			pool.Indices[num3 + 3] = num2 + 1;
			pool.Indices[num3 + 4] = num + 1;
			pool.Indices[num3 + 5] = num;
			pool.Indices[num3 + 6] = num2 + 1;
			pool.Indices[num3 + 7] = num2 + 2;
			pool.Indices[num3 + 8] = num + 1;
			pool.Indices[num3 + 9] = num2 + 2;
			pool.Indices[num3 + 10] = num + 2;
			pool.Indices[num3 + 11] = num + 1;
		}
		pool.IndiceChanged = true;
	}

	private void UpdateVertex()
	{
		VertexPool pool = this.mVertexSegment.Pool;
		for (int i = 0; i < this.Granularity; i++)
		{
			int num = this.mVertexSegment.VertStart + i * 3;
			float num2 = (float)i / (float)this.Granularity;
			float tl = num2 * this.mFadeT;
			Vector2 zero = Vector2.zero;
			Vector3 vector = this.mSpline.InterpolateByLen(tl);
			Vector3 vector2 = this.mSpline.InterpolateNormalByLen(tl);
			Vector3 vector3 = vector + vector2.normalized * this.mTrailWidth * 0.5f;
			Vector3 vector4 = vector - vector2.normalized * this.mTrailWidth * 0.5f;
			pool.Vertices[num] = vector3;
			pool.Colors[num] = this.MyColor;
			zero.x = 0f;
			zero.y = num2;
			pool.UVs[num] = zero;
			pool.Vertices[num + 1] = vector;
			pool.Colors[num + 1] = this.MyColor;
			zero.x = 0.5f;
			zero.y = num2;
			pool.UVs[num + 1] = zero;
			pool.Vertices[num + 2] = vector4;
			pool.Colors[num + 2] = this.MyColor;
			zero.x = 1f;
			zero.y = num2;
			pool.UVs[num + 2] = zero;
		}
		this.mVertexSegment.Pool.UVChanged = true;
		this.mVertexSegment.Pool.VertChanged = true;
		this.mVertexSegment.Pool.ColorChanged = true;
	}
}
