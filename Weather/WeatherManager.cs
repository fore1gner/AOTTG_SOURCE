using System.Collections;
using System.Collections.Generic;
using ApplicationManagers;
using CustomSkins;
using GameManagers;
using Settings;
using UnityEngine;
using Utility;

namespace Weather;

internal class WeatherManager : MonoBehaviour
{
	private static WeatherManager _instance;

	private const float LerpDelay = 0.05f;

	private const float SyncDelay = 5f;

	private HashSet<WeatherEffect> LowEffects = new HashSet<WeatherEffect>
	{
		WeatherEffect.Daylight,
		WeatherEffect.AmbientLight,
		WeatherEffect.Flashlight,
		WeatherEffect.Skybox
	};

	private static Dictionary<string, Material> SkyboxMaterials = new Dictionary<string, Material>();

	private static Dictionary<string, Dictionary<string, Material>> SkyboxBlendedMaterials = new Dictionary<string, Dictionary<string, Material>>();

	private static Shader _blendedShader;

	private List<WeatherScheduleRunner> _scheduleRunners = new List<WeatherScheduleRunner>();

	private Dictionary<WeatherEffect, BaseWeatherEffect> _effects = new Dictionary<WeatherEffect, BaseWeatherEffect>();

	public WeatherSet _currentWeather = new WeatherSet();

	public WeatherSet _targetWeather = new WeatherSet();

	public WeatherSet _startWeather = new WeatherSet();

	public Dictionary<int, float> _targetWeatherStartTimes = new Dictionary<int, float>();

	public Dictionary<int, float> _targetWeatherEndTimes = new Dictionary<int, float>();

	private List<WeatherEffect> _needApply = new List<WeatherEffect>();

	public float _currentTime;

	public bool _needSync;

	public Dictionary<WeatherScheduleRunner, float> _currentScheduleWait = new Dictionary<WeatherScheduleRunner, float>();

	private float _currentLerpWait;

	private float _currentSyncWait;

	private bool _finishedLoading;

	private Light _mainLight;

	private Skybox _skybox;

	public static void Init()
	{
		WeatherManager._instance = SingletonFactory.CreateSingleton(WeatherManager._instance);
	}

	public static void FinishLoadAssets()
	{
		WeatherManager.LoadSkyboxes();
		ThunderWeatherEffect.FinishLoadAssets();
		WeatherManager._instance.StartCoroutine(WeatherManager._instance.RestartWeather());
	}

	private static void LoadSkyboxes()
	{
		WeatherManager._blendedShader = AssetBundleManager.InstantiateAsset<Shader>("SkyboxBlendShader");
		string[] array = RCextensions.EnumToStringArray<WeatherSkybox>();
		string[] parts = RCextensions.EnumToStringArray<SkyboxCustomSkinPartId>();
		string[] array2 = array;
		foreach (string text in array2)
		{
			WeatherManager.SkyboxMaterials.Add(text, AssetBundleManager.InstantiateAsset<Material>(text.ToString() + "Skybox"));
		}
		array2 = array;
		foreach (string text2 in array2)
		{
			WeatherManager.SkyboxBlendedMaterials.Add(text2, new Dictionary<string, Material>());
			string[] array3 = array;
			foreach (string text3 in array3)
			{
				Material value = WeatherManager.CreateBlendedSkybox(WeatherManager._blendedShader, parts, text2, text3);
				WeatherManager.SkyboxBlendedMaterials[text2].Add(text3, value);
			}
		}
	}

	public static void TakeFlashlight(Transform parent)
	{
		if (WeatherManager._instance._effects.ContainsKey(WeatherEffect.Flashlight) && WeatherManager._instance._effects[WeatherEffect.Flashlight] != null)
		{
			WeatherManager._instance._effects[WeatherEffect.Flashlight].SetParent(parent);
		}
	}

	private static Material CreateBlendedSkybox(Shader shader, string[] parts, string skybox1, string skybox2)
	{
		Material material = new Material(shader);
		foreach (string text in parts)
		{
			string text2 = "_" + text + "Tex";
			material.SetTexture(text2, WeatherManager.SkyboxMaterials[skybox1].GetTexture(text2));
			material.SetTexture(text2 + "2", WeatherManager.SkyboxMaterials[skybox2].GetTexture(text2));
		}
		WeatherManager.SetSkyboxBlend(material, 0f);
		return material;
	}

	private static void SetSkyboxBlend(Material skybox, float blend)
	{
		skybox.SetFloat("_Blend", blend);
	}

	private void Cache()
	{
		this._mainLight = GameObject.Find("mainLight").GetComponent<Light>();
		this._skybox = Camera.main.GetComponent<Skybox>();
	}

	private void ResetSkyboxColors()
	{
		foreach (string key in WeatherManager.SkyboxBlendedMaterials.Keys)
		{
			foreach (string key2 in WeatherManager.SkyboxBlendedMaterials[key].Keys)
			{
				WeatherManager.SkyboxBlendedMaterials[key][key2].SetColor("_Tint", new Color(0.5f, 0.5f, 0.5f));
			}
		}
	}

	private IEnumerator RestartWeather()
	{
		while (Camera.main == null)
		{
			yield return null;
		}
		this.Cache();
		this.ResetSkyboxColors();
		this._scheduleRunners.Clear();
		this._effects.Clear();
		this._currentWeather.SetDefault();
		this._startWeather.SetDefault();
		this._targetWeather.SetDefault();
		this._targetWeatherStartTimes.Clear();
		this._targetWeatherEndTimes.Clear();
		this._needApply.Clear();
		this._currentTime = 0f;
		this._currentScheduleWait.Clear();
		this.CreateEffects();
		if (Application.loadedLevel == 0 && SettingsManager.GraphicsSettings.AnimatedIntro.Value)
		{
			this.SetMainMenuWeather();
		}
		this.ApplyCurrentWeather(firstStart: true, applyAll: true);
		bool flag = SettingsManager.GraphicsSettings.WeatherEffects.Value == 0;
		if (Application.loadedLevel != 0 && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.isMasterClient))
		{
			if (!flag)
			{
				this._currentWeather.Copy(SettingsManager.WeatherSettings.WeatherSets.GetSelectedSet());
				this.CreateScheduleRunners(this._currentWeather.Schedule.Value);
				this._currentWeather.Schedule.SetDefault();
			}
			if (this._currentWeather.UseSchedule.Value)
			{
				foreach (WeatherScheduleRunner scheduleRunner in this._scheduleRunners)
				{
					scheduleRunner.ProcessSchedule();
					scheduleRunner.ConsumeSchedule();
				}
			}
			this.SyncWeather();
			this._currentSyncWait = 5f;
			this._needSync = false;
		}
		this._currentLerpWait = 0.05f;
		this._finishedLoading = true;
	}

	private void SetMainMenuWeather()
	{
		this._currentWeather.Rain.Value = 0.45f;
		this._currentWeather.Thunder.Value = 0.1f;
		this._currentWeather.Skybox.Value = "Storm";
		this._currentWeather.FogDensity.Value = 0.01f;
		this._currentWeather.Daylight.Value = new Color(0.1f, 0.1f, 0.1f);
		this._currentWeather.AmbientLight.Value = new Color(0.1f, 0.1f, 0.1f);
	}

	private void CreateScheduleRunners(string schedule)
	{
		WeatherScheduleRunner weatherScheduleRunner = new WeatherScheduleRunner(this);
		foreach (WeatherEvent @event in new WeatherSchedule(schedule).Events)
		{
			if (@event.Action == WeatherAction.BeginSchedule)
			{
				weatherScheduleRunner = new WeatherScheduleRunner(this);
				this._scheduleRunners.Add(weatherScheduleRunner);
				this._currentScheduleWait.Add(weatherScheduleRunner, 0f);
			}
			weatherScheduleRunner.Schedule.Events.Add(@event);
		}
	}

	private void CreateEffects()
	{
		this._effects.Add(WeatherEffect.Rain, AssetBundleManager.InstantiateAsset<GameObject>("RainEffect").AddComponent<RainWeatherEffect>());
		this._effects.Add(WeatherEffect.Snow, AssetBundleManager.InstantiateAsset<GameObject>("SnowEffect").AddComponent<SnowWeatherEffect>());
		this._effects.Add(WeatherEffect.Wind, AssetBundleManager.InstantiateAsset<GameObject>("WindEffect").AddComponent<WindWeatherEffect>());
		this._effects.Add(WeatherEffect.Thunder, AssetBundleManager.InstantiateAsset<GameObject>("ThunderEffect").AddComponent<ThunderWeatherEffect>());
		Transform parent = Camera.main.transform;
		foreach (BaseWeatherEffect value in this._effects.Values)
		{
			value.Setup(parent);
			value.Randomize();
			value.Disable();
		}
		this.CreateFlashlight();
	}

	private void CreateFlashlight()
	{
		this._effects.Add(WeatherEffect.Flashlight, AssetBundleManager.InstantiateAsset<GameObject>("FlashlightEffect").AddComponent<FlashlightWeatherEffect>());
		this._effects[WeatherEffect.Flashlight].Setup(null);
		this._effects[WeatherEffect.Flashlight].Disable();
		if (IN_GAME_MAIN_CAMERA.Instance != null)
		{
			WeatherManager.TakeFlashlight(IN_GAME_MAIN_CAMERA.Instance.transform);
		}
	}

	private void FixedUpdate()
	{
		if (!this._finishedLoading)
		{
			return;
		}
		this._currentTime += Time.fixedDeltaTime;
		if (this._targetWeatherStartTimes.Count > 0)
		{
			this._currentLerpWait -= Time.fixedDeltaTime;
			if (this._currentLerpWait <= 0f)
			{
				this.LerpCurrentWeatherToTarget();
				this.ApplyCurrentWeather(firstStart: false, applyAll: false);
				this._currentLerpWait = 0.05f;
			}
		}
		if ((IN_GAME_MAIN_CAMERA.gametype != 0 && !PhotonNetwork.isMasterClient) || !this._currentWeather.UseSchedule.Value)
		{
			return;
		}
		foreach (WeatherScheduleRunner item in new List<WeatherScheduleRunner>(this._currentScheduleWait.Keys))
		{
			this._currentScheduleWait[item] -= Time.fixedDeltaTime;
			if (this._currentScheduleWait[item] <= 0f)
			{
				item.ConsumeSchedule();
			}
		}
		this._currentSyncWait -= Time.fixedDeltaTime;
		if (this._currentSyncWait <= 0f && this._needSync)
		{
			this.LerpCurrentWeatherToTarget();
			this.SyncWeather();
			this._needSync = false;
			this._currentSyncWait = 5f;
		}
	}

	private void SyncWeather()
	{
		this.ApplyCurrentWeather(firstStart: false, applyAll: true);
		if (PhotonNetwork.isMasterClient && IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			CustomRPCManager.PhotonView.RPC("SetWeatherRPC", PhotonTargets.Others, this._currentWeather.SerializeToJsonString(), this._startWeather.SerializeToJsonString(), this._targetWeather.SerializeToJsonString(), this._targetWeatherStartTimes, this._targetWeatherEndTimes, this._currentTime);
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		WindWeatherEffect.WindEnabled = false;
		foreach (List<LightningParticle> item in ThunderWeatherEffect.LightningPool)
		{
			foreach (LightningParticle item2 in item)
			{
				item2.Disable();
			}
		}
		if (Application.loadedLevelName != "characterCreation" && Application.loadedLevelName != "SnapShot")
		{
			this._finishedLoading = false;
			base.StartCoroutine(this.RestartWeather());
		}
	}

	private void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (PhotonNetwork.isMasterClient)
		{
			CustomRPCManager.PhotonView.RPC("SetWeatherRPC", player, this._currentWeather.SerializeToJsonString(), this._startWeather.SerializeToJsonString(), this._targetWeather.SerializeToJsonString(), this._targetWeatherStartTimes, this._targetWeatherEndTimes, this._currentTime);
		}
	}

	private void LerpCurrentWeatherToTarget()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, float> targetWeatherEndTime in this._targetWeatherEndTimes)
		{
			float num;
			if (targetWeatherEndTime.Value <= this._currentTime)
			{
				list.Add(targetWeatherEndTime.Key);
				num = 1f;
			}
			else
			{
				float num2 = this._targetWeatherStartTimes[targetWeatherEndTime.Key];
				float value = targetWeatherEndTime.Value;
				num = (this._currentTime - num2) / Mathf.Max(value - num2, 1f);
				num = Mathf.Clamp(num, 0f, 1f);
			}
			string key = ((WeatherEffect)targetWeatherEndTime.Key).ToString();
			BaseSetting baseSetting = (BaseSetting)this._startWeather.Settings[key];
			BaseSetting baseSetting2 = (BaseSetting)this._currentWeather.Settings[key];
			BaseSetting baseSetting3 = (BaseSetting)this._targetWeather.Settings[key];
			switch ((WeatherEffect)targetWeatherEndTime.Key)
			{
			case WeatherEffect.Daylight:
			case WeatherEffect.AmbientLight:
			case WeatherEffect.SkyboxColor:
			case WeatherEffect.Flashlight:
			case WeatherEffect.FogColor:
				((ColorSetting)baseSetting2).Value = Color.Lerp(((ColorSetting)baseSetting).Value, ((ColorSetting)baseSetting3).Value, num);
				break;
			case WeatherEffect.FogDensity:
			case WeatherEffect.Rain:
			case WeatherEffect.Thunder:
			case WeatherEffect.Snow:
			case WeatherEffect.Wind:
				((FloatSetting)baseSetting2).Value = Mathf.Lerp(((FloatSetting)baseSetting).Value, ((FloatSetting)baseSetting3).Value, num);
				break;
			case WeatherEffect.Skybox:
			{
				Material blendedSkybox = this.GetBlendedSkybox(this._currentWeather.Skybox.Value, this._targetWeather.Skybox.Value);
				if (blendedSkybox != null)
				{
					if (num >= 1f)
					{
						((StringSetting)baseSetting2).Value = ((StringSetting)baseSetting3).Value;
					}
					WeatherManager.SetSkyboxBlend(blendedSkybox, num);
				}
				break;
			}
			}
			this._needApply.Add((WeatherEffect)targetWeatherEndTime.Key);
		}
		foreach (int item in list)
		{
			this._targetWeatherStartTimes.Remove(item);
			this._targetWeatherEndTimes.Remove(item);
		}
	}

	private void ApplyCurrentWeather(bool firstStart, bool applyAll)
	{
		if (applyAll)
		{
			this._needApply = RCextensions.EnumToList<WeatherEffect>();
		}
		WeatherEffectLevel value = (WeatherEffectLevel)SettingsManager.GraphicsSettings.WeatherEffects.Value;
		foreach (WeatherEffect item in this._needApply)
		{
			if (!firstStart && value == WeatherEffectLevel.Low && !this.LowEffects.Contains(item))
			{
				continue;
			}
			switch (item)
			{
			case WeatherEffect.Daylight:
				this._mainLight.color = this._currentWeather.Daylight.Value;
				break;
			case WeatherEffect.AmbientLight:
				RenderSettings.ambientLight = this._currentWeather.AmbientLight.Value;
				break;
			case WeatherEffect.FogColor:
				RenderSettings.fogColor = this._currentWeather.FogColor.Value;
				break;
			case WeatherEffect.FogDensity:
				if (this._currentWeather.FogDensity.Value > 0f)
				{
					RenderSettings.fog = true;
					RenderSettings.fogMode = FogMode.Exponential;
					RenderSettings.fogDensity = this._currentWeather.FogDensity.Value * 0.05f;
				}
				else
				{
					RenderSettings.fog = false;
				}
				break;
			case WeatherEffect.Flashlight:
				((FlashlightWeatherEffect)this._effects[WeatherEffect.Flashlight]).SetColor(this._currentWeather.Flashlight.Value);
				if (this._currentWeather.Flashlight.Value.a > 0f && this._currentWeather.Flashlight.Value != Color.black)
				{
					if (!this._effects[WeatherEffect.Flashlight].gameObject.activeSelf)
					{
						this._effects[WeatherEffect.Flashlight].Enable();
					}
				}
				else
				{
					this._effects[WeatherEffect.Flashlight].Disable();
				}
				break;
			case WeatherEffect.Skybox:
				base.StartCoroutine(this.WaitAndApplySkybox());
				break;
			case WeatherEffect.SkyboxColor:
			{
				Material blendedSkybox = this.GetBlendedSkybox(this._currentWeather.Skybox.Value, this._targetWeather.Skybox.Value);
				if (blendedSkybox != null)
				{
					blendedSkybox.SetColor("_Tint", this._currentWeather.SkyboxColor.Value);
				}
				break;
			}
			case WeatherEffect.Rain:
			case WeatherEffect.Thunder:
			case WeatherEffect.Snow:
			case WeatherEffect.Wind:
			{
				float value2 = ((FloatSetting)this._currentWeather.Settings[item.ToString()]).Value;
				this._effects[item].SetLevel(value2);
				if (value2 > 0f)
				{
					if (!this._effects[item].gameObject.activeSelf)
					{
						this._effects[item].Randomize();
						this._effects[item].Enable();
					}
				}
				else
				{
					this._effects[item].Disable(fadeOut: true);
				}
				break;
			}
			}
		}
		this._needApply.Clear();
	}

	private IEnumerator WaitAndApplySkybox()
	{
		yield return new WaitForEndOfFrame();
		Material blendedSkybox = this.GetBlendedSkybox(this._currentWeather.Skybox.Value, this._targetWeather.Skybox.Value);
		if (blendedSkybox != null && this._skybox.material != blendedSkybox && SkyboxCustomSkinLoader.SkyboxMaterial == null)
		{
			blendedSkybox.SetColor("_Tint", this._currentWeather.SkyboxColor.Value);
			this._skybox.material = blendedSkybox;
			if (IN_GAME_MAIN_CAMERA.Instance != null)
			{
				IN_GAME_MAIN_CAMERA.Instance.UpdateSnapshotSkybox();
			}
		}
	}

	private Material GetBlendedSkybox(string skybox1, string skybox2)
	{
		if (WeatherManager.SkyboxBlendedMaterials.ContainsKey(skybox1) && WeatherManager.SkyboxBlendedMaterials[skybox1].ContainsKey(skybox2))
		{
			return WeatherManager.SkyboxBlendedMaterials[skybox1][skybox2];
		}
		return null;
	}

	public static void OnSetWeatherRPC(string currentWeatherJson, string startWeatherJson, string targetWeatherJson, Dictionary<int, float> targetWeatherStartTimes, Dictionary<int, float> targetWeatherEndTimes, float currentTime, PhotonMessageInfo info)
	{
		if ((info == null || info.sender == PhotonNetwork.masterClient) && SettingsManager.GraphicsSettings.WeatherEffects.Value != 0)
		{
			WeatherManager._instance.StartCoroutine(WeatherManager._instance.WaitAndFinishOnSetWeather(currentWeatherJson, startWeatherJson, targetWeatherJson, targetWeatherStartTimes, targetWeatherEndTimes, currentTime));
		}
	}

	private IEnumerator WaitAndFinishOnSetWeather(string currentWeatherJson, string startWeatherJson, string targetWeatherJson, Dictionary<int, float> targetWeatherStartTimes, Dictionary<int, float> targetWeatherEndTimes, float currentTime)
	{
		while (!this._finishedLoading)
		{
			yield return null;
		}
		this._currentWeather.DeserializeFromJsonString(currentWeatherJson);
		this._startWeather.DeserializeFromJsonString(startWeatherJson);
		this._targetWeather.DeserializeFromJsonString(targetWeatherJson);
		this._targetWeatherStartTimes = targetWeatherStartTimes;
		this._targetWeatherEndTimes = targetWeatherEndTimes;
		this._currentTime = currentTime;
		this.LerpCurrentWeatherToTarget();
		this.ApplyCurrentWeather(firstStart: false, applyAll: true);
	}
}
