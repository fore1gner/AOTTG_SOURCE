using UnityEngine;

namespace Weather;

internal class SnowWeatherEffect : BaseWeatherEffect
{
	protected override Vector3 _positionOffset => Vector3.up * 0f;

	public override void Randomize()
	{
		base._particleEmitters[0].rndVelocity = new Vector3(20f, 20f, 20f);
		ParticleEmitter obj = base._particleEmitters[0];
		float minEnergy = (base._particleEmitters[0].maxEnergy = 1.2f);
		obj.minEnergy = minEnergy;
		base._particleEmitters[1].rndVelocity = new Vector3(5f, 5f, 5f);
		base._particleEmitters[1].localVelocity = new Vector3(20f * Util.GetRandomSign(), 0f, 0f);
		ParticleEmitter obj2 = base._particleEmitters[1];
		minEnergy = (base._particleEmitters[0].maxEnergy = 1.2f);
		obj2.minEnergy = minEnergy;
	}

	public override void SetLevel(float level)
	{
		base.SetLevel(level);
		if (!(level <= 0f))
		{
			if (level <= 0.5f)
			{
				float num = level / 0.5f;
				this.SetActiveEmitter(0);
				this.SetActiveAudio(0, 0.25f + 0.25f * num);
				ParticleEmitter obj = base._particleEmitters[0];
				float minEmission = (base._particleEmitters[0].maxEmission = this.ClampParticles(100f + num * 300f));
				obj.minEmission = minEmission;
				ParticleEmitter obj2 = base._particleEmitters[0];
				minEmission = (base._particleEmitters[0].maxSize = 25f);
				obj2.minSize = minEmission;
			}
			else
			{
				float num4 = (level - 0.5f) / 0.5f;
				this.SetActiveEmitter(1);
				this.SetAudioVolume(1, 0.25f + 0.25f * num4);
				ParticleEmitter obj3 = base._particleEmitters[1];
				float minEmission = (base._particleEmitters[1].maxEmission = this.ClampParticles(200f + num4 * 200f));
				obj3.minEmission = minEmission;
				ParticleEmitter obj4 = base._particleEmitters[1];
				minEmission = (base._particleEmitters[1].maxSize = 12f);
				obj4.minSize = minEmission;
			}
		}
	}

	public override void Setup(Transform parent)
	{
		base.Setup(parent);
	}
}
