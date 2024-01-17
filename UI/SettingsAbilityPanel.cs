using System;
using Settings;
using UnityEngine.UI;

namespace UI;

internal class SettingsAbilityPanel : SettingsCategoryPanel
{
	protected Text _pointsLeftLabel;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsPopup settingsPopup = (SettingsPopup)parent;
		_ = settingsPopup.LocaleCategory;
		AbilitySettings settings = SettingsManager.AbilitySettings;
		ElementStyle style = new ElementStyle(24, 200f, this.ThemePanel);
		ElementFactory.CreateColorSetting(base.DoublePanelRight, style, settings.BombColor, "Bomb color", settingsPopup.ColorPickPopup);
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, settings.ShowBombColors, "Show bomb colors");
		ElementFactory.CreateToggleSetting(base.DoublePanelRight, style, settings.UseOldEffect, "Use old effect");
		this._pointsLeftLabel = ElementFactory.CreateDefaultLabel(base.DoublePanelLeft, style, "Points Left").GetComponent<Text>();
		ElementFactory.CreateIncrementSetting(base.DoublePanelLeft, style, settings.BombRadius, "Bomb radius (0-10)", "", 33f, 30f, null, delegate
		{
			this.OnStatChanged(settings.BombRadius);
		});
		ElementFactory.CreateIncrementSetting(base.DoublePanelLeft, style, settings.BombRange, "Bomb range (0-3)", "", 33f, 30f, null, delegate
		{
			this.OnStatChanged(settings.BombRange);
		});
		ElementFactory.CreateIncrementSetting(base.DoublePanelLeft, style, settings.BombSpeed, "Bomb speed (0-10)", "", 33f, 30f, null, delegate
		{
			this.OnStatChanged(settings.BombSpeed);
		});
		ElementFactory.CreateIncrementSetting(base.DoublePanelLeft, style, settings.BombCooldown, "Bomb cooldown (0-6)", "", 33f, 30f, null, delegate
		{
			this.OnStatChanged(settings.BombCooldown);
		});
		this.OnStatChanged(settings.BombRadius);
	}

	protected void OnStatChanged(IntSetting setting)
	{
		int num = 16;
		AbilitySettings abilitySettings = SettingsManager.AbilitySettings;
		int num2 = abilitySettings.BombRadius.Value + abilitySettings.BombRange.Value + abilitySettings.BombSpeed.Value + abilitySettings.BombCooldown.Value;
		if (num2 > num)
		{
			int num3 = num2 - num;
			setting.Value -= num3;
			if (setting.Value < 0)
			{
				abilitySettings.BombRadius.SetDefault();
				abilitySettings.BombRange.SetDefault();
				abilitySettings.BombSpeed.SetDefault();
				abilitySettings.BombCooldown.SetDefault();
			}
			this.SyncSettingElements();
		}
		num2 = abilitySettings.BombRadius.Value + abilitySettings.BombRange.Value + abilitySettings.BombSpeed.Value + abilitySettings.BombCooldown.Value;
		this._pointsLeftLabel.text = "Points left: " + Math.Max(0, num - num2);
	}
}
