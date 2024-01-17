using UnityEngine;

public class VortexAffector : Affector
{
	protected Vector3 Direction;

	private float Magnitude;

	private bool UseCurve;

	private AnimationCurve VortexCurve;

	public VortexAffector(float mag, Vector3 dir, EffectNode node)
		: base(node)
	{
		this.Magnitude = mag;
		this.Direction = dir;
		this.UseCurve = false;
	}

	public VortexAffector(AnimationCurve vortexCurve, Vector3 dir, EffectNode node)
		: base(node)
	{
		this.VortexCurve = vortexCurve;
		this.Direction = dir;
		this.UseCurve = true;
	}

	public override void Update()
	{
		Vector3 vector = base.Node.GetLocalPosition() - base.Node.Owner.EmitPoint;
		if (vector.magnitude != 0f)
		{
			float num = Vector3.Dot(this.Direction, vector);
			vector -= num * this.Direction;
			Vector3 zero = Vector3.zero;
			zero = ((!(vector == Vector3.zero)) ? Vector3.Cross(this.Direction, vector).normalized : vector);
			float elapsedTime = base.Node.GetElapsedTime();
			float num2 = ((!this.UseCurve) ? this.Magnitude : this.VortexCurve.Evaluate(elapsedTime));
			zero *= num2 * Time.deltaTime;
			base.Node.Position += zero;
		}
	}
}
