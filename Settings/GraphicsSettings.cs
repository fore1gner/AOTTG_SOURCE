using System;
using ApplicationManagers;
using UnityEngine;

namespace Settings;

internal class GraphicsSettings : SaveableSettingsContainer
{
	public IntSetting OverallQuality = new IntSetting(QualitySettings.GetQualityLevel());

	public IntSetting TextureQuality = new IntSetting(3);

	public BoolSetting VSync = new BoolSetting(defaultValue: false);

	public IntSetting FPSCap = new IntSetting(0, 0);

	public BoolSetting ExclusiveFullscreen = new BoolSetting(defaultValue: false);

	public BoolSetting ShowFPS = new BoolSetting(defaultValue: false);

	public BoolSetting MipmapEnabled = new BoolSetting(defaultValue: true);

	public BoolSetting WeaponTrailEnabled = new BoolSetting(defaultValue: true);

	public BoolSetting WindEffectEnabled = new BoolSetting(defaultValue: false);

	public BoolSetting InterpolationEnabled = new BoolSetting(defaultValue: true);

	public IntSetting RenderDistance = new IntSetting(1500, 10, 1000000);

	public IntSetting WeatherEffects = new IntSetting(3);

	public BoolSetting AnimatedIntro = new BoolSetting(defaultValue: true);

	public BoolSetting BlurEnabled = new BoolSetting(defaultValue: false);

	public IntSetting AntiAliasing = new IntSetting(0);

	protected override string FileName => "Graphics.json";

	public override void Save()
	{
		base.Save();
		FullscreenHandler.SetMainData(this.ExclusiveFullscreen.Value);
	}

	public override void Load()
	{
		base.Load();
		FullscreenHandler.SetMainData(this.ExclusiveFullscreen.Value);
	}

	public override void Apply()
	{
		QualitySettings.SetQualityLevel(this.OverallQuality.Value, applyExpensiveChanges: true);
		QualitySettings.vSyncCount = Convert.ToInt32(this.VSync.Value);
		Application.targetFrameRate = ((this.FPSCap.Value > 0) ? this.FPSCap.Value : (-1));
		QualitySettings.masterTextureLimit = 3 - this.TextureQuality.Value;
		QualitySettings.antiAliasing = ((this.AntiAliasing.Value != 0) ? ((int)Mathf.Pow(2f, this.AntiAliasing.Value)) : 0);
		this.ApplyShadows();
		IN_GAME_MAIN_CAMERA.ApplyGraphicsSettings();
	}

	private void ApplyShadows()
	{
	}
}
