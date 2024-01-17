using UnityEngine;

namespace Weather;

internal class RainWeatherEffect : BaseWeatherEffect
{
	protected override Vector3 _positionOffset => Vector3.up * 30f;

	public override void Randomize()
	{
		float num = Random.Range(0f, 20f);
		num = Random.Range(0f - num, num);
		foreach (ParticleEmitter particleEmitter in base._particleEmitters)
		{
			particleEmitter.transform.localPosition = this._positionOffset;
			particleEmitter.transform.localRotation = Quaternion.identity;
			particleEmitter.transform.RotateAround(base._transform.position, Vector3.forward, num);
			particleEmitter.transform.RotateAround(base._transform.position, Vector3.up, Random.Range(0f, 360f));
		}
	}

	public override void SetLevel(float level)
	{
		base.SetLevel(level);
		if (!(level <= 0f))
		{
			if (level < 0.5f)
			{
				float num = level / 0.5f;
				this.SetActiveEmitter(0);
				ParticleEmitter obj = base._particleEmitters[0];
				float minEmission = (base._particleEmitters[0].maxEmission = this.ClampParticles(50f + 150f * num));
				obj.minEmission = minEmission;
				ParticleEmitter obj2 = base._particleEmitters[0];
				minEmission = (base._particleEmitters[0].maxSize = 30f + 30f * num);
				obj2.minSize = minEmission;
				this.SetActiveAudio(0, 0.25f + 0.25f * num);
			}
			else
			{
				float num4 = (level - 0.5f) / 0.5f;
				this.SetActiveEmitter(1);
				ParticleEmitter obj3 = base._particleEmitters[1];
				float minEmission = (base._particleEmitters[1].maxEmission = this.ClampParticles(100f + 150f * num4));
				obj3.minEmission = minEmission;
				ParticleEmitter obj4 = base._particleEmitters[1];
				minEmission = (base._particleEmitters[1].maxSize = 50f + num4 * 10f);
				obj4.minSize = minEmission;
				this.SetActiveAudio(1, 0.25f + 0.25f * num4);
			}
		}
	}

	public override void Setup(Transform parent)
	{
		base.Setup(parent);
		base._particleEmitters[0].localVelocity = Vector3.down * 100f;
		base._particleEmitters[1].localVelocity = Vector3.down * 100f;
		base._particleEmitters[0].rndVelocity = new Vector3(10f, 0f, 10f);
		base._particleEmitters[1].rndVelocity = new Vector3(10f, 0f, 10f);
	}
}
