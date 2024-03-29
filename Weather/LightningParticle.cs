using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weather;

public class LightningParticle : MonoBehaviour
{
	private const float FadeInTime = 0.5f;

	private const float StayTime = 0.3f;

	private const float FadeOutTime = 1f;

	private const float ChaosFactor = 0.2f;

	private const float StartWidth = 2f;

	private const float EndWidth = 2f;

	protected Color LightningColor = new Color(228f, 245f, 255f);

	private static System.Random _random = new System.Random();

	protected LineRenderer _lineRenderer;

	protected int _startIndex;

	protected List<AudioSource> _audioSources = new List<AudioSource>();

	private static void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
	{
		if (directionNormalized == Vector3.zero)
		{
			side = Vector3.right;
			return;
		}
		float x = directionNormalized.x;
		float y = directionNormalized.y;
		float z = directionNormalized.z;
		float num = Mathf.Abs(x);
		float num2 = Mathf.Abs(y);
		float num3 = Mathf.Abs(z);
		float num4;
		float num5;
		float num6;
		if (num >= num2 && num2 >= num3)
		{
			num4 = 1f;
			num5 = 1f;
			num6 = (0f - (y * num4 + z * num5)) / x;
		}
		else if (num2 >= num3)
		{
			num6 = 1f;
			num5 = 1f;
			num4 = (0f - (x * num6 + z * num5)) / y;
		}
		else
		{
			num6 = 1f;
			num4 = 1f;
			num5 = (0f - (x * num6 + y * num4)) / z;
		}
		side = new Vector3(num6, num4, num5).normalized;
	}

	public static List<Vector3> GenerateLightningBoltPositions(Vector3 start, Vector3 end, int generation, float offsetAmount = 0f)
	{
		int num = 0;
		List<KeyValuePair<Vector3, Vector3>> list = new List<KeyValuePair<Vector3, Vector3>>();
		list.Add(new KeyValuePair<Vector3, Vector3>(start, end));
		if (offsetAmount <= 0f)
		{
			offsetAmount = (end - start).magnitude * 0.2f;
		}
		while (generation-- > 0)
		{
			int num2 = num;
			num = list.Count;
			for (int i = num2; i < num; i++)
			{
				start = list[i].Key;
				end = list[i].Value;
				Vector3 vector = (start + end) * 0.5f;
				LightningParticle.RandomVector(ref start, ref end, offsetAmount, out var result);
				vector += result;
				list.Add(new KeyValuePair<Vector3, Vector3>(start, vector));
				list.Add(new KeyValuePair<Vector3, Vector3>(vector, end));
			}
			offsetAmount *= 0.5f;
		}
		List<Vector3> list2 = new List<Vector3>();
		list2.Add(list[num].Key);
		for (int j = num; j < list.Count; j++)
		{
			list2.Add(list[j].Value);
		}
		return list2;
	}

	private static void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
	{
		Vector3 directionNormalized = (end - start).normalized;
		LightningParticle.GetPerpendicularVector(ref directionNormalized, out var side);
		float num = ((float)LightningParticle._random.NextDouble() + 0.1f) * offsetAmount;
		float angle = (float)LightningParticle._random.NextDouble() * 360f;
		result = Quaternion.AngleAxis(angle, directionNormalized) * side * num;
	}

	private void Awake()
	{
		this._lineRenderer = base.GetComponent<LineRenderer>();
		this._lineRenderer.SetVertexCount(0);
		this._audioSources = (from x in base.GetComponentsInChildren<AudioSource>()
			orderby x.gameObject.name
			select x).ToList();
	}

	public void Disable()
	{
		foreach (AudioSource audioSource in this._audioSources)
		{
			audioSource.Stop();
		}
		this._lineRenderer.SetColors(Color.clear, Color.clear);
		base.gameObject.SetActive(value: false);
	}

	public void Enable()
	{
		this._lineRenderer.SetColors(Color.clear, Color.clear);
		base.gameObject.SetActive(value: true);
	}

	public void Strike(bool sound)
	{
		base.StartCoroutine(this.StrikeCoroutine(sound));
	}

	public void PlayAudio()
	{
		this.SetVolume(0.3f);
		int index = UnityEngine.Random.Range(0, 2);
		this._audioSources[index].Play();
	}

	public void Setup(Vector3 start, Vector3 end, int generation)
	{
		List<Vector3> list = LightningParticle.GenerateLightningBoltPositions(start, end, generation);
		this._lineRenderer.SetVertexCount(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			this._lineRenderer.SetPosition(i, list[i]);
		}
	}

	private IEnumerator StrikeCoroutine(bool sound)
	{
		Color color = this.LightningColor;
		float maxAlpha = ((Application.loadedLevel == 0) ? 0.3f : 1f);
		color.a = 0f;
		this._lineRenderer.SetColors(color, color);
		this._lineRenderer.SetWidth(2f, 2f);
		float startTime2 = Time.time;
		while (Time.time - startTime2 < 0.5f)
		{
			float num = Mathf.Clamp((Time.time - startTime2) / 0.5f, 0f, 1f);
			color.a = num * maxAlpha;
			this._lineRenderer.SetColors(color, color);
			yield return new WaitForEndOfFrame();
		}
		if (sound)
		{
			this.PlayAudio();
		}
		color.a = maxAlpha;
		this._lineRenderer.SetColors(color, color);
		yield return new WaitForSeconds(0.3f);
		startTime2 = Time.time;
		while (Time.time - startTime2 < 1f)
		{
			float num2 = Mathf.Clamp((Time.time - startTime2) / 1f, 0f, 1f);
			color.a = (1f - num2) * (1f - num2) * maxAlpha;
			this._lineRenderer.SetColors(color, color);
			this.SetVolume(0.3f * (1f - num2));
			yield return new WaitForEndOfFrame();
		}
		this.Disable();
	}

	private void SetVolume(float volume)
	{
		foreach (AudioSource audioSource in this._audioSources)
		{
			audioSource.volume = volume;
		}
	}
}
