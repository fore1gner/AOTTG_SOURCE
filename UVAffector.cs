using UnityEngine;

public class UVAffector : Affector
{
	protected float ElapsedTime;

	protected UVAnimation Frames;

	protected float UVTime;

	public UVAffector(UVAnimation frame, float time, EffectNode node)
		: base(node)
	{
		this.Frames = frame;
		this.UVTime = time;
	}

	public override void Reset()
	{
		this.ElapsedTime = 0f;
		this.Frames.curFrame = 0;
	}

	public override void Update()
	{
		this.ElapsedTime += Time.deltaTime;
		float num = ((!(this.UVTime <= 0f)) ? (this.UVTime / (float)this.Frames.frames.Length) : (base.Node.GetLifeTime() / (float)this.Frames.frames.Length));
		if (this.ElapsedTime >= num)
		{
			Vector2 uv = Vector2.zero;
			Vector2 dm = Vector2.zero;
			this.Frames.GetNextFrame(ref uv, ref dm);
			base.Node.LowerLeftUV = uv;
			base.Node.UVDimensions = dm;
			this.ElapsedTime -= num;
		}
	}
}
