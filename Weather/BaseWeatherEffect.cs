using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Settings;
using UnityEngine;

namespace Weather;

internal class BaseWeatherEffect : MonoBehaviour
{
	protected Transform _parent;

	protected Transform _transform;

	protected float _level;

	protected float _maxParticles;

	protected float _particleMultiplier;

	protected List<ParticleEmitter> _particleEmitters = new List<ParticleEmitter>();

	protected List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

	protected List<AudioSource> _audioSources = new List<AudioSource>();

	protected Dictionary<AudioSource, float> _audioTargetVolumes = new Dictionary<AudioSource, float>();

	protected Dictionary<AudioSource, float> _audioStartTimes = new Dictionary<AudioSource, float>();

	protected Dictionary<AudioSource, float> _audioStartVolumes = new Dictionary<AudioSource, float>();

	protected bool _isDisabling;

	protected virtual Vector3 _positionOffset => Vector3.zero;

	protected virtual float _audioFadeTime => 2f;

	public virtual void Disable(bool fadeOut = false)
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (fadeOut)
		{
			if (!this._isDisabling)
			{
				base.StartCoroutine(this.WaitAndDisable());
			}
			return;
		}
		base.StopAllCoroutines();
		this.StopAllAudio();
		this.StopAllEmitters();
		this.StopAllParticleSystems();
		base.gameObject.SetActive(value: false);
		this._isDisabling = false;
	}

	public virtual void Enable()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
			this._isDisabling = false;
		}
	}

	private IEnumerator WaitAndDisable()
	{
		this._isDisabling = true;
		this.StopAllAudio(fadeOut: true);
		this.StopAllEmitters();
		this.StopAllParticleSystems();
		yield return new WaitForSeconds(this._audioFadeTime);
		base.gameObject.SetActive(value: false);
		this._isDisabling = false;
	}

	public virtual void Randomize()
	{
	}

	public virtual void SetParent(Transform parent)
	{
		this._parent = parent;
	}

	public virtual void SetLevel(float level)
	{
		this._level = level;
	}

	public virtual void Setup(Transform parent)
	{
		this._transform = base.transform;
		this._parent = parent;
		if (SettingsManager.GraphicsSettings.WeatherEffects.Value == 3)
		{
			this._maxParticles = 500f;
			this._particleMultiplier = 1f;
		}
		else
		{
			this._maxParticles = 200f;
			this._particleMultiplier = 0.7f;
		}
		this._particleEmitters = (from x in base.GetComponentsInChildren<ParticleEmitter>()
			orderby x.gameObject.name
			select x).ToList();
		this._particleSystems = (from x in base.GetComponentsInChildren<ParticleSystem>()
			orderby x.gameObject.name
			select x).ToList();
		this._audioSources = (from x in base.GetComponentsInChildren<AudioSource>()
			orderby x.gameObject.name
			select x).ToList();
		foreach (ParticleEmitter particleEmitter in this._particleEmitters)
		{
			particleEmitter.emit = false;
			particleEmitter.transform.localPosition = this._positionOffset;
			particleEmitter.transform.localRotation = Quaternion.identity;
		}
		foreach (ParticleSystem particleSystem in this._particleSystems)
		{
			particleSystem.Stop();
			particleSystem.transform.localPosition = this._positionOffset;
			particleSystem.transform.localRotation = Quaternion.identity;
		}
		this.StopAllAudio();
	}

	protected virtual void SetActiveEmitter(int index)
	{
		this.StopAllEmitters();
		this.StopAllParticleSystems();
		this._particleEmitters[index].emit = true;
	}

	protected virtual void StopAllEmitters()
	{
		foreach (ParticleEmitter particleEmitter in this._particleEmitters)
		{
			particleEmitter.emit = false;
		}
	}

	protected virtual void SetActiveParticleSystem(int index)
	{
		this.StopAllEmitters();
		this.StopAllParticleSystems();
		if (!this._particleSystems[index].isPlaying)
		{
			this._particleSystems[index].Play();
		}
	}

	protected virtual void StopAllParticleSystems()
	{
		foreach (ParticleSystem particleSystem in this._particleSystems)
		{
			particleSystem.Stop();
		}
	}

	protected virtual void SetActiveAudio(int index, float volume)
	{
		for (int i = 0; i < this._audioSources.Count; i++)
		{
			if (i == index)
			{
				this.SetAudioVolume(i, volume);
			}
			else
			{
				this.SetAudioVolume(i, 0f);
			}
		}
	}

	protected virtual void SetAudioVolume(int index, float volume)
	{
		this.SetAudioVolume(this._audioSources[index], volume);
	}

	protected virtual void SetAudioVolume(AudioSource audio, float volume)
	{
		volume = Mathf.Clamp(volume, 0f, 1f);
		if (this._audioTargetVolumes[audio] != volume)
		{
			this._audioTargetVolumes[audio] = volume;
			if (volume == 0f)
			{
				this._audioStartTimes[audio] = Time.time;
				this._audioStartVolumes[audio] = audio.volume;
			}
		}
	}

	protected virtual void StopAllAudio(bool fadeOut = false)
	{
		if (fadeOut)
		{
			foreach (AudioSource audioSource in this._audioSources)
			{
				this.SetAudioVolume(audioSource, 0f);
			}
			return;
		}
		foreach (AudioSource audioSource2 in this._audioSources)
		{
			audioSource2.Stop();
			this._audioTargetVolumes[audioSource2] = 0f;
			this._audioStartTimes[audioSource2] = 0f;
			this._audioStartVolumes[audioSource2] = 0f;
		}
	}

	protected virtual float ClampParticles(float count)
	{
		return Mathf.Min(count * this._particleMultiplier, this._maxParticles);
	}

	protected virtual void LateUpdate()
	{
		this._transform.position = this._parent.position;
		this.UpdateAudio();
	}

	protected virtual void UpdateAudio()
	{
		foreach (AudioSource audioSource in this._audioSources)
		{
			if (this._audioTargetVolumes[audioSource] == 0f)
			{
				if (audioSource.isPlaying)
				{
					audioSource.volume = this.GetLerpedVolume(audioSource);
					if (audioSource.volume == 0f)
					{
						audioSource.Pause();
					}
				}
			}
			else if (audioSource.isPlaying)
			{
				if (audioSource.volume != this._audioTargetVolumes[audioSource])
				{
					audioSource.volume = this.GetLerpedVolume(audioSource);
				}
			}
			else
			{
				this._audioStartTimes[audioSource] = Time.time;
				this._audioStartVolumes[audioSource] = 0f;
				audioSource.volume = this.GetLerpedVolume(audioSource);
				audioSource.Play();
			}
		}
	}

	protected virtual float GetLerpedVolume(AudioSource audio)
	{
		float value = (Time.time - this._audioStartTimes[audio]) / this._audioFadeTime;
		value = Mathf.Clamp(value, 0f, 1f);
		return Mathf.Lerp(audio.volume, this._audioTargetVolumes[audio], value);
	}
}
