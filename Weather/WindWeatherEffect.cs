using UnityEngine;

namespace Weather;

internal class WindWeatherEffect : BaseWeatherEffect
{
	public static bool WindEnabled = false;

	public static Vector3 WindDirection = Vector3.zero;

	protected override Vector3 _positionOffset => Vector3.up * 0f;

	public override void Setup(Transform parent)
	{
		base.Setup(parent);
	}

	public override void Randomize()
	{
		WindWeatherEffect.WindDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
	}

	public override void Disable(bool fadeOut = false)
	{
		WindWeatherEffect.WindEnabled = false;
		base.Disable(fadeOut);
	}

	public override void SetLevel(float level)
	{
		base.SetLevel(level);
		if (!(level <= 0f))
		{
			WindWeatherEffect.WindEnabled = true;
			if (level < 0.5f)
			{
				this.SetAudioVolume(0, 1f);
			}
			else
			{
				this.SetAudioVolume(1, 0.5f);
			}
		}
	}
}
