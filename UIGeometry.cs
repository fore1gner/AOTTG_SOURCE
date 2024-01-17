using UnityEngine;

public class UIGeometry
{
	public BetterList<Color32> cols = new BetterList<Color32>();

	private Vector3 mRtpNormal;

	private Vector4 mRtpTan;

	private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	public BetterList<Vector3> verts = new BetterList<Vector3>();

	public bool hasTransformed
	{
		get
		{
			if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
			{
				return this.mRtpVerts.size == this.verts.size;
			}
			return false;
		}
	}

	public bool hasVertices => this.verts.size > 0;

	public void ApplyOffset(Vector3 pivotOffset)
	{
		for (int i = 0; i < this.verts.size; i++)
		{
			this.verts.buffer[i] += pivotOffset;
		}
	}

	public void ApplyTransform(Matrix4x4 widgetToPanel, bool normals)
	{
		if (this.verts.size > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			for (int size = this.verts.size; i < size; i++)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
			}
			this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
			Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
			this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts == null || this.mRtpVerts.size <= 0)
		{
			return;
		}
		if (n == null)
		{
			for (int i = 0; i < this.mRtpVerts.size; i++)
			{
				v.Add(this.mRtpVerts.buffer[i]);
				u.Add(this.uvs.buffer[i]);
				c.Add(this.cols.buffer[i]);
			}
			return;
		}
		for (int j = 0; j < this.mRtpVerts.size; j++)
		{
			v.Add(this.mRtpVerts.buffer[j]);
			u.Add(this.uvs.buffer[j]);
			c.Add(this.cols.buffer[j]);
			n.Add(this.mRtpNormal);
			t.Add(this.mRtpTan);
		}
	}
}
