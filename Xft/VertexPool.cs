using System.Collections.Generic;
using UnityEngine;

namespace Xft;

public class VertexPool
{
	public class VertexSegment
	{
		public int IndexCount;

		public int IndexStart;

		public VertexPool Pool;

		public int VertCount;

		public int VertStart;

		public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
		{
			this.VertStart = start;
			this.VertCount = count;
			this.IndexCount = icount;
			this.IndexStart = istart;
			this.Pool = pool;
		}

		public void ClearIndices()
		{
			for (int i = this.IndexStart; i < this.IndexStart + this.IndexCount; i++)
			{
				this.Pool.Indices[i] = 0;
			}
			this.Pool.IndiceChanged = true;
		}
	}

	public const int BlockSize = 108;

	public float BoundsScheduleTime = 1f;

	public bool ColorChanged;

	public Color[] Colors;

	public float ElapsedTime;

	public bool FirstUpdate = true;

	protected int IndexTotal;

	protected int IndexUsed;

	public bool IndiceChanged;

	public int[] Indices;

	public Material Material;

	public Mesh Mesh;

	protected List<VertexSegment> SegmentList = new List<VertexSegment>();

	public bool UV2Changed;

	public bool UVChanged;

	public Vector2[] UVs;

	public Vector2[] UVs2;

	public bool VertChanged;

	protected bool VertCountChanged;

	protected int VertexTotal;

	protected int VertexUsed;

	public Vector3[] Vertices;

	public VertexPool(Mesh mesh, Material material)
	{
		this.VertexTotal = (this.VertexUsed = 0);
		this.VertCountChanged = false;
		this.Mesh = mesh;
		this.Material = material;
		this.InitArrays();
		this.IndiceChanged = (this.ColorChanged = (this.UVChanged = (this.UV2Changed = (this.VertChanged = true))));
	}

	public void EnlargeArrays(int count, int icount)
	{
		Vector3[] vertices = this.Vertices;
		this.Vertices = new Vector3[this.Vertices.Length + count];
		vertices.CopyTo(this.Vertices, 0);
		Vector2[] uVs = this.UVs;
		this.UVs = new Vector2[this.UVs.Length + count];
		uVs.CopyTo(this.UVs, 0);
		Vector2[] uVs2 = this.UVs2;
		this.UVs2 = new Vector2[this.UVs2.Length + count];
		uVs2.CopyTo(this.UVs2, 0);
		this.InitDefaultShaderParam(this.UVs2);
		Color[] colors = this.Colors;
		this.Colors = new Color[this.Colors.Length + count];
		colors.CopyTo(this.Colors, 0);
		int[] ındices = this.Indices;
		this.Indices = new int[this.Indices.Length + icount];
		ındices.CopyTo(this.Indices, 0);
		this.VertCountChanged = true;
		this.IndiceChanged = true;
		this.ColorChanged = true;
		this.UVChanged = true;
		this.VertChanged = true;
		this.UV2Changed = true;
	}

	public Material GetMaterial()
	{
		return this.Material;
	}

	public VertexSegment GetRopeVertexSeg(int maxcount)
	{
		return this.GetVertices(maxcount * 2, (maxcount - 1) * 6);
	}

	public VertexSegment GetVertices(int vcount, int icount)
	{
		int num = 0;
		int num2 = 0;
		if (this.VertexUsed + vcount >= this.VertexTotal)
		{
			num = (vcount / 108 + 1) * 108;
		}
		if (this.IndexUsed + icount >= this.IndexTotal)
		{
			num2 = (icount / 108 + 1) * 108;
		}
		this.VertexUsed += vcount;
		this.IndexUsed += icount;
		if (num != 0 || num2 != 0)
		{
			this.EnlargeArrays(num, num2);
			this.VertexTotal += num;
			this.IndexTotal += num2;
		}
		return new VertexSegment(this.VertexUsed - vcount, vcount, this.IndexUsed - icount, icount, this);
	}

	protected void InitArrays()
	{
		this.Vertices = new Vector3[4];
		this.UVs = new Vector2[4];
		this.UVs2 = new Vector2[4];
		this.Colors = new Color[4];
		this.Indices = new int[6];
		this.VertexTotal = 4;
		this.IndexTotal = 6;
		this.InitDefaultShaderParam(this.UVs2);
	}

	private void InitDefaultShaderParam(Vector2[] uv2)
	{
		for (int i = 0; i < uv2.Length; i++)
		{
			uv2[i].x = 1f;
			uv2[i].y = 0f;
		}
	}

	public void LateUpdate()
	{
		if (this.VertCountChanged)
		{
			this.Mesh.Clear();
		}
		this.Mesh.vertices = this.Vertices;
		if (this.UVChanged)
		{
			this.Mesh.uv = this.UVs;
		}
		if (this.UV2Changed)
		{
			this.Mesh.uv2 = this.UVs2;
		}
		if (this.ColorChanged)
		{
			this.Mesh.colors = this.Colors;
		}
		if (this.IndiceChanged)
		{
			this.Mesh.triangles = this.Indices;
		}
		this.ElapsedTime += Time.deltaTime;
		if (this.ElapsedTime > this.BoundsScheduleTime || this.FirstUpdate)
		{
			this.RecalculateBounds();
			this.ElapsedTime = 0f;
		}
		if (this.ElapsedTime > this.BoundsScheduleTime)
		{
			this.FirstUpdate = false;
		}
		this.VertCountChanged = false;
		this.IndiceChanged = false;
		this.ColorChanged = false;
		this.UVChanged = false;
		this.UV2Changed = false;
		this.VertChanged = false;
	}

	public void RecalculateBounds()
	{
		this.Mesh.RecalculateBounds();
	}
}
