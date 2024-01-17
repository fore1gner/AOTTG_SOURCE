using UnityEngine;

public class AttractionForceAffector : Affector
{
	private AnimationCurve AttractionCurve;

	private float Magnitude;

	protected Vector3 Position;

	private bool UseCurve;

	public AttractionForceAffector(float magnitude, Vector3 pos, EffectNode node)
		: base(node)
	{
		this.Magnitude = magnitude;
		this.Position = pos;
		this.UseCurve = false;
	}

	public AttractionForceAffector(AnimationCurve curve, Vector3 pos, EffectNode node)
		: base(node)
	{
		this.AttractionCurve = curve;
		this.Position = pos;
		this.UseCurve = true;
	}

	public override void Update()
	{
		Vector3 vector = ((!base.Node.SyncClient) ? (base.Node.ClientTrans.position + this.Position - base.Node.GetLocalPosition()) : (this.Position - base.Node.GetLocalPosition()));
		float elapsedTime = base.Node.GetElapsedTime();
		float num = ((!this.UseCurve) ? this.Magnitude : this.AttractionCurve.Evaluate(elapsedTime));
		float num2 = num;
		base.Node.Velocity += vector.normalized * num2 * Time.deltaTime;
	}
}
