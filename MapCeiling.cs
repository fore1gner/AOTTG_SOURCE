using System;
using UnityEngine;

public class MapCeiling : MonoBehaviour
{
	private GameObject _barrierRef;

	private Color _color;

	private float _minAlpha;

	private float _maxAlpha = 0.6f;

	private float _minimumHeight = 3f;

	private static float _forestHeight = 280f;

	private static float _cityHeight = 210f;

	private static float _forestWidth = 1320f;

	private static float _cityWidth = 1400f;

	private static float _depth = 20f;

	public static void CreateMapCeiling()
	{
		if (FengGameManagerMKII.level.StartsWith("The Forest"))
		{
			MapCeiling.CreateMapCeilingWithDimensions(MapCeiling._forestHeight, MapCeiling._forestWidth, MapCeiling._depth);
		}
		else if (FengGameManagerMKII.level.StartsWith("The City"))
		{
			MapCeiling.CreateMapCeilingWithDimensions(MapCeiling._cityHeight, MapCeiling._cityWidth, MapCeiling._depth);
		}
	}

	private static void CreateMapCeilingWithDimensions(float height, float width, float depth)
	{
		GameObject obj = new GameObject();
		obj.AddComponent<MapCeiling>();
		obj.transform.position = new Vector3(0f, height, 0f);
		obj.transform.rotation = Quaternion.identity;
		obj.transform.localScale = new Vector3(width, depth, width);
	}

	private void Start()
	{
		this.CreateCeilingPart("barrier");
		this._barrierRef = this.CreateCeilingPart("killcuboid");
		this._color = new Color(1f, 0f, 0f, this._maxAlpha);
		this.UpdateTransparency();
	}

	private GameObject CreateCeilingPart(string asset)
	{
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(FengGameManagerMKII.RCassets.Load(asset), Vector3.zero, Quaternion.identity);
		obj.transform.position = base.transform.position;
		obj.transform.rotation = base.transform.rotation;
		obj.transform.localScale = base.transform.localScale;
		return obj;
	}

	private void Update()
	{
		this.UpdateTransparency();
	}

	private float getMinAlpha()
	{
		return this._minAlpha;
	}

	private void setMinAlpha(float newMinAlpha)
	{
		if (newMinAlpha > 1f || newMinAlpha < 0f)
		{
			throw new Exception("Error: _minAlpha must in range (0 <= _minAlpha <= 1)");
		}
		this._minAlpha = newMinAlpha;
	}

	public float getMaxAlpha()
	{
		return this._maxAlpha;
	}

	public void setMaxAlpha(float newMaxAlpha)
	{
		if (newMaxAlpha > 1f || newMaxAlpha < 0f)
		{
			throw new Exception("Error: _minAlpha must in range (0 <= _minAlpha <= 1)");
		}
		this._maxAlpha = newMaxAlpha;
	}

	public void UpdateTransparency()
	{
		if (Camera.main != null && this._barrierRef != null && this._barrierRef.renderer != null)
		{
			float num = this._maxAlpha;
			try
			{
				float num2 = this._barrierRef.transform.position.y / this._minimumHeight;
				num = ((!(Camera.main.transform.position.y < num2)) ? this.Map(Camera.main.transform.position.y, num2, this._barrierRef.transform.position.y, this._minAlpha, this._maxAlpha) : this._minAlpha);
				num = this.fadeByGradient(num);
			}
			catch
			{
			}
			this._color.a = num;
			this._barrierRef.renderer.material.color = this._color;
		}
	}

	public float fadeByGradient(float x)
	{
		return Mathf.Clamp(10f * x * x, this._minAlpha, this._maxAlpha);
	}

	public float Map(float x, float inMin, float inMax, float outMin, float outMax)
	{
		if (x > inMax || x < inMin)
		{
			throw new Exception("Error,\npublic float map(float x, float inMin, float inMax, float outMin, float outMax)\nis not defined for values (x > inMax || x < inMin)");
		}
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
