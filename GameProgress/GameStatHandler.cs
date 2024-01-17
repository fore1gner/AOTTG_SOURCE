using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameProgress;

internal class GameStatHandler : BaseGameProgressHandler
{
	private const int ExpPerKill = 10;

	private const int ExpPerLevelBase = 500;

	private const float ExpPerLevelMultiplier = 1.2f;

	private const int MaxLevel = 20;

	private List<int> _expPerLevel = new List<int>();

	private GameStatContainer _gameStat;

	public GameStatHandler(GameStatContainer gameStat)
	{
		this._gameStat = gameStat;
		this._expPerLevel.Add(500);
		for (int i = 1; i < 20; i++)
		{
			this._expPerLevel.Add((int)((float)this._expPerLevel[i - 1] * 1.2f));
		}
	}

	public int GetExpToNext()
	{
		if (this._gameStat.Level.Value >= 20)
		{
			return 0;
		}
		return this._expPerLevel[this._gameStat.Level.Value];
	}

	public void AddExp(int exp)
	{
		this._gameStat.Exp.Value += exp;
		this.CheckLevelUp();
	}

	private void CheckLevelUp()
	{
		if (this._gameStat.Level.Value >= 20)
		{
			this._gameStat.Exp.Value = 0;
			this._gameStat.Level.Value = 20;
		}
		else if (this._gameStat.Exp.Value > 0 && this._gameStat.Exp.Value >= this._expPerLevel[this._gameStat.Level.Value])
		{
			this._gameStat.Level.Value++;
			this._gameStat.Exp.Value -= this._expPerLevel[this._gameStat.Level.Value];
			this._gameStat.Exp.Value = Math.Max(this._gameStat.Exp.Value, 0);
			this.CheckLevelUp();
		}
	}

	public override void RegisterTitanKill(GameObject character, TITAN victim, KillWeapon weapon)
	{
		switch (weapon)
		{
		case KillWeapon.Blade:
			this._gameStat.TitansKilledBlade.Value++;
			break;
		case KillWeapon.Gun:
			this._gameStat.TitansKilledGun.Value++;
			break;
		case KillWeapon.ThunderSpear:
			this._gameStat.TitansKilledThunderSpear.Value++;
			break;
		default:
			this._gameStat.TitansKilledOther.Value++;
			break;
		}
		this._gameStat.TitansKilledTotal.Value++;
		this.AddExp(10);
	}

	public override void RegisterHumanKill(GameObject character, HERO victim, KillWeapon weapon)
	{
		switch (weapon)
		{
		case KillWeapon.Blade:
			this._gameStat.HumansKilledBlade.Value++;
			break;
		case KillWeapon.Gun:
			this._gameStat.HumansKilledGun.Value++;
			break;
		case KillWeapon.ThunderSpear:
			this._gameStat.HumansKilledThunderSpear.Value++;
			break;
		case KillWeapon.Titan:
			this._gameStat.HumansKilledTitan.Value++;
			break;
		default:
			this._gameStat.HumansKilledOther.Value++;
			break;
		}
		this._gameStat.HumansKilledTotal.Value++;
		this.AddExp(10);
	}

	public override void RegisterDamage(GameObject character, GameObject victim, KillWeapon weapon, int damage)
	{
		if (weapon == KillWeapon.Blade || weapon == KillWeapon.Gun)
		{
			this._gameStat.DamageHighestOverall.Value = Math.Max(this._gameStat.DamageHighestOverall.Value, damage);
			this._gameStat.DamageTotalOverall.Value += damage;
			switch (weapon)
			{
			case KillWeapon.Blade:
				this._gameStat.DamageHighestBlade.Value = Math.Max(this._gameStat.DamageHighestBlade.Value, damage);
				this._gameStat.DamageTotalBlade.Value += damage;
				break;
			case KillWeapon.Gun:
				this._gameStat.DamageHighestGun.Value = Math.Max(this._gameStat.DamageHighestGun.Value, damage);
				this._gameStat.DamageTotalGun.Value += damage;
				break;
			}
		}
	}

	public override void RegisterSpeed(GameObject character, float speed)
	{
		this._gameStat.HighestSpeed.Value = Mathf.Max(this._gameStat.HighestSpeed.Value, speed);
	}

	public override void RegisterInteraction(GameObject character, GameObject interact, InteractionType type)
	{
	}
}
