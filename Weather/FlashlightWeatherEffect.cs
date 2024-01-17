using UnityEngine;

namespace Weather;

internal class FlashlightWeatherEffect : BaseWeatherEffect
{
	private Light _light;

	protected override Vector3 _positionOffset => Vector3.up * 0f;

	public override void Randomize()
	{
	}

	public override void Setup(Transform parent)
	{
		base.Setup(parent);
		this._light = base.GetComponentInChildren<Light>();
		this._light.range = 120f;
		this._light.intensity = 1f;
		this._light.spotAngle = 60f;
		this.SetColor(Color.black);
	}

	public virtual void SetColor(Color color)
	{
		this._light.color = color;
	}

	protected override void LateUpdate()
	{
		if (base._parent != null)
		{
			if (!this._light.gameObject.activeSelf)
			{
				this._light.gameObject.SetActive(value: true);
			}
			base._transform.rotation = base._parent.rotation * Quaternion.Euler(353f, 0f, 0f);
			base._transform.position = base._parent.position;
		}
		else if (this._light.gameObject.activeSelf)
		{
			this._light.gameObject.SetActive(value: false);
		}
	}
}
