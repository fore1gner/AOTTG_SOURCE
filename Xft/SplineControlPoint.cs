using UnityEngine;

namespace Xft;

public class SplineControlPoint
{
	public int ControlPointIndex = -1;

	public float Dist;

	protected Spline mSpline;

	public Vector3 Normal;

	public Vector3 Position;

	public int SegmentIndex = -1;

	public bool IsValid => this.NextControlPoint != null;

	public SplineControlPoint NextControlPoint => this.mSpline.NextControlPoint(this);

	public Vector3 NextNormal => this.mSpline.NextNormal(this);

	public Vector3 NextPosition => this.mSpline.NextPosition(this);

	public SplineControlPoint PreviousControlPoint => this.mSpline.PreviousControlPoint(this);

	public Vector3 PreviousNormal => this.mSpline.PreviousNormal(this);

	public Vector3 PreviousPosition => this.mSpline.PreviousPosition(this);

	private Vector3 GetNext2Normal()
	{
		return this.NextControlPoint?.NextNormal ?? this.Normal;
	}

	private Vector3 GetNext2Position()
	{
		return this.NextControlPoint?.NextPosition ?? this.NextPosition;
	}

	public void Init(Spline owner)
	{
		this.mSpline = owner;
		this.SegmentIndex = -1;
	}

	public Vector3 Interpolate(float localF)
	{
		localF = Mathf.Clamp01(localF);
		return Spline.CatmulRom(this.PreviousPosition, this.Position, this.NextPosition, this.GetNext2Position(), localF);
	}

	public Vector3 InterpolateNormal(float localF)
	{
		localF = Mathf.Clamp01(localF);
		return Spline.CatmulRom(this.PreviousNormal, this.Normal, this.NextNormal, this.GetNext2Normal(), localF);
	}
}
