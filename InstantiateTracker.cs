using Settings;
using UnityEngine;

public class InstantiateTracker
{
	private class AhssShots : ThingToCheck
	{
		private float lastShot = Time.time;

		private int shots = 1;

		public AhssShots()
		{
			base.type = GameResource.shotGun;
		}

		public override bool KickWorthy()
		{
			if (Time.time - this.lastShot < 1f)
			{
				this.shots++;
				if (this.shots > 2)
				{
					return true;
				}
			}
			else
			{
				this.shots = 0;
			}
			this.lastShot = Time.time;
			return false;
		}

		public override void reset()
		{
		}
	}

	private class BladeHitEffect : ThingToCheck
	{
		private float accumTime;

		private float lastHit = Time.time;

		public BladeHitEffect()
		{
			base.type = GameResource.bladeHit;
		}

		public override bool KickWorthy()
		{
			float num = Time.time - this.lastHit;
			this.lastHit = Time.time;
			if (num <= 0.3f)
			{
				this.accumTime += num;
				return this.accumTime >= 1.25f;
			}
			this.accumTime = 0f;
			return false;
		}

		public override void reset()
		{
		}
	}

	private class BloodEffect : ThingToCheck
	{
		private float accumTime;

		private float lastHit = Time.time;

		public BloodEffect()
		{
			base.type = GameResource.bloodEffect;
		}

		public override bool KickWorthy()
		{
			float num = Time.time - this.lastHit;
			this.lastHit = Time.time;
			if (num <= 0.3f)
			{
				this.accumTime += num;
				return this.accumTime >= 2f;
			}
			this.accumTime = 0f;
			return false;
		}

		public override void reset()
		{
		}
	}

	private class ExcessiveBombs : ThingToCheck
	{
		private int count = 1;

		private float lastClear = Time.time;

		public ExcessiveBombs()
		{
			base.type = GameResource.bomb;
		}

		public override bool KickWorthy()
		{
			if (Time.time - this.lastClear > 5f)
			{
				this.count = 0;
				this.lastClear = Time.time;
			}
			this.count++;
			return this.count > 20;
		}

		public override void reset()
		{
		}
	}

	private class ExcessiveEffect : ThingToCheck
	{
		private int effectCounter = 1;

		private float lastEffectTime = Time.time;

		public ExcessiveEffect()
		{
			base.type = GameResource.effect;
		}

		public override bool KickWorthy()
		{
			if (Time.time - this.lastEffectTime >= 2f)
			{
				this.effectCounter = 0;
				this.lastEffectTime = Time.time;
			}
			this.effectCounter++;
			return this.effectCounter > 8;
		}

		public override void reset()
		{
		}
	}

	private class ExcessiveFlares : ThingToCheck
	{
		private int flares = 1;

		private float lastFlare = Time.time;

		public ExcessiveFlares()
		{
			base.type = GameResource.flare;
		}

		public override bool KickWorthy()
		{
			if (Time.time - this.lastFlare >= 10f)
			{
				this.flares = 0;
				this.lastFlare = Time.time;
			}
			this.flares++;
			return this.flares > 4;
		}

		public override void reset()
		{
		}
	}

	private class ExcessiveNameChange : ThingToCheck
	{
		private float lastNameChange = Time.time;

		private int nameChanges = 1;

		public ExcessiveNameChange()
		{
			base.type = GameResource.name;
		}

		public override bool KickWorthy()
		{
			float num = Time.time - this.lastNameChange;
			this.lastNameChange = Time.time;
			if (num >= 5f)
			{
				this.nameChanges = 0;
			}
			this.nameChanges++;
			return this.nameChanges > 5;
		}

		public override void reset()
		{
			this.nameChanges = 0;
			this.lastNameChange = Time.time;
		}
	}

	public enum GameResource
	{
		none,
		shotGun,
		effect,
		flare,
		bladeHit,
		bloodEffect,
		general,
		name,
		bomb
	}

	private class GenerallyExcessive : ThingToCheck
	{
		private int count = 1;

		private float lastClear = Time.time;

		public GenerallyExcessive()
		{
			base.type = GameResource.general;
		}

		public override bool KickWorthy()
		{
			if (Time.time - this.lastClear > 5f)
			{
				this.count = 0;
				this.lastClear = Time.time;
			}
			this.count++;
			return this.count > 35;
		}

		public override void reset()
		{
		}
	}

	private class Player
	{
		public int id;

		private ThingToCheck[] thingsToCheck;

		public Player(int id)
		{
			this.id = id;
			this.thingsToCheck = new ThingToCheck[0];
		}

		private ThingToCheck GameResourceToThing(GameResource gr)
		{
			return gr switch
			{
				GameResource.shotGun => new AhssShots(), 
				GameResource.effect => new ExcessiveEffect(), 
				GameResource.flare => new ExcessiveFlares(), 
				GameResource.bladeHit => new BladeHitEffect(), 
				GameResource.bloodEffect => new BloodEffect(), 
				GameResource.general => new GenerallyExcessive(), 
				GameResource.name => new ExcessiveNameChange(), 
				GameResource.bomb => new ExcessiveBombs(), 
				_ => null, 
			};
		}

		private int GetThingToCheck(GameResource type)
		{
			for (int i = 0; i < this.thingsToCheck.Length; i++)
			{
				if (this.thingsToCheck[i].type == type)
				{
					return i;
				}
			}
			return -1;
		}

		public bool IsThingExcessive(GameResource gr)
		{
			int thingToCheck = this.GetThingToCheck(gr);
			if (thingToCheck > -1)
			{
				return this.thingsToCheck[thingToCheck].KickWorthy();
			}
			RCextensions.Add(ref this.thingsToCheck, this.GameResourceToThing(gr));
			return false;
		}

		public void resetNameTracking()
		{
			int thingToCheck = this.GetThingToCheck(GameResource.name);
			if (thingToCheck > -1)
			{
				this.thingsToCheck[thingToCheck].reset();
			}
		}
	}

	private abstract class ThingToCheck
	{
		public GameResource type;

		public abstract bool KickWorthy();

		public abstract void reset();
	}

	public static readonly InstantiateTracker instance = new InstantiateTracker();

	private Player[] players = new Player[0];

	public bool checkObj(string key, PhotonPlayer photonPlayer, int[] viewIDS)
	{
		if (photonPlayer.isMasterClient || photonPlayer.isLocal)
		{
			return true;
		}
		int num = photonPlayer.ID * PhotonNetwork.MAX_VIEW_IDS;
		int num2 = num + PhotonNetwork.MAX_VIEW_IDS;
		foreach (int num3 in viewIDS)
		{
			if (num3 <= num || num3 >= num2)
			{
				if (PhotonNetwork.isMasterClient)
				{
					FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning invalid photon view.");
				}
				return false;
			}
		}
		key = key.ToLower();
		switch (key)
		{
		case "rcasset/bombmain":
		case "rcasset/bombexplodemain":
			if (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
			{
				if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingBomb)
				{
					FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning bomb item (" + key + ").");
				}
				return false;
			}
			return this.Instantiated(photonPlayer, GameResource.bomb);
		case "hook":
		case "aottg_hero 1":
			return this.Instantiated(photonPlayer, GameResource.general);
		case "hitmeat2":
			return this.Instantiated(photonPlayer, GameResource.bloodEffect);
		case "hitmeat":
		case "redcross":
		case "redcross1":
			return this.Instantiated(photonPlayer, GameResource.bladeHit);
		case "fx/flarebullet1":
		case "fx/flarebullet2":
		case "fx/flarebullet3":
			return this.Instantiated(photonPlayer, GameResource.flare);
		case "fx/shotgun":
		case "fx/shotgun 1":
			return this.Instantiated(photonPlayer, GameResource.shotGun);
		case "fx/fxtitanspawn":
		case "fx/boom1":
		case "fx/boom3":
		case "fx/boom5":
		case "fx/rockthrow":
		case "fx/bite":
			if (LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan || SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
			{
				return this.Instantiated(photonPlayer, GameResource.effect);
			}
			if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingTitan)
			{
				FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: false, "spawning titan effects.");
			}
			return false;
		case "fx/boom2":
		case "fx/boom4":
		case "fx/fxtitandie":
		case "fx/fxtitandie1":
		case "fx/boost_smoke":
		case "fx/thunder":
			return this.Instantiated(photonPlayer, GameResource.effect);
		case "rcasset/cannonballobject":
			return this.Instantiated(photonPlayer, GameResource.bomb);
		case "rcasset/cannonwall":
		case "rcasset/cannonground":
			if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.allowedToCannon.ContainsKey(photonPlayer.ID) && !FengGameManagerMKII.instance.restartingMC)
			{
				FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: false, "spawning cannon item (" + key + ").");
			}
			return this.Instantiated(photonPlayer, GameResource.general);
		case "rcasset/cannonwallprop":
		case "rcasset/cannongroundprop":
			if (PhotonNetwork.isMasterClient)
			{
				FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning MC item (" + key + ").");
			}
			return false;
		case "titan_eren":
			if (!(RCextensions.returnStringFromObject(photonPlayer.customProperties[PhotonPlayerProperty.character]).ToUpper() != "EREN"))
			{
				if (SettingsManager.LegacyGameSettings.KickShifters.Value)
				{
					if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingEren)
					{
						FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: false, "spawning titan eren (" + key + ").");
					}
					return false;
				}
				return this.Instantiated(photonPlayer, GameResource.general);
			}
			if (PhotonNetwork.isMasterClient)
			{
				FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning titan eren (" + key + ").");
			}
			return false;
		case "fx/justSmoke":
		case "bloodexplore":
		case "bloodsplatter":
			return this.Instantiated(photonPlayer, GameResource.effect);
		case "hitmeatbig":
			if (!(RCextensions.returnStringFromObject(photonPlayer.customProperties[PhotonPlayerProperty.character]).ToUpper() != "EREN"))
			{
				if (SettingsManager.LegacyGameSettings.KickShifters.Value)
				{
					if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingEren)
					{
						FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: false, "spawning eren effect (" + key + ").");
					}
					return false;
				}
				return this.Instantiated(photonPlayer, GameResource.effect);
			}
			if (PhotonNetwork.isMasterClient)
			{
				FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning eren effect (" + key + ").");
			}
			return false;
		case "fx/colossal_steam_dmg":
		case "fx/colossal_steam":
		case "fx/boom1_ct_kick":
			if (!PhotonNetwork.isMasterClient || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
			{
				return this.Instantiated(photonPlayer, GameResource.effect);
			}
			FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning colossal effect (" + key + ").");
			return false;
		case "rock":
			if (!PhotonNetwork.isMasterClient || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
			{
				return this.Instantiated(photonPlayer, GameResource.general);
			}
			FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning MC item (" + key + ").");
			return false;
		case "horse":
			if (LevelInfo.getInfo(FengGameManagerMKII.level).horse || SettingsManager.LegacyGameSettings.AllowHorses.Value)
			{
				return this.Instantiated(photonPlayer, GameResource.general);
			}
			if (PhotonNetwork.isMasterClient && !FengGameManagerMKII.instance.restartingHorse)
			{
				FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning horse (" + key + ").");
			}
			return false;
		case "titan_ver3.1":
			if (!PhotonNetwork.isMasterClient)
			{
				if (FengGameManagerMKII.masterRC && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
				{
					int num4 = 0;
					foreach (TITAN titan in FengGameManagerMKII.instance.getTitans())
					{
						if (titan.photonView.owner == photonPlayer)
						{
							num4++;
						}
					}
					if (num4 > 1)
					{
						return false;
					}
				}
			}
			else
			{
				if (!LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan && !SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT && !FengGameManagerMKII.instance.restartingTitan)
				{
					FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: false, "spawning titan (" + key + ").");
					return false;
				}
				if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
				{
					int num4 = 0;
					foreach (TITAN titan2 in FengGameManagerMKII.instance.getTitans())
					{
						if (titan2.photonView.owner == photonPlayer)
						{
							num4++;
						}
					}
					if (num4 > 1)
					{
						FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: false, "spawning titan (" + key + ").");
						return false;
					}
				}
			}
			return this.Instantiated(photonPlayer, GameResource.general);
		case "colossal_titan":
		case "female_titan":
		case "titan_eren_trost":
		case "aot_supply":
		case "monsterprefab":
		case "titan_new_1":
		case "titan_new_2":
			if (!PhotonNetwork.isMasterClient)
			{
				if (FengGameManagerMKII.masterRC)
				{
					return false;
				}
				return this.Instantiated(photonPlayer, GameResource.general);
			}
			FengGameManagerMKII.instance.kickPlayerRC(photonPlayer, ban: true, "spawning MC item (" + key + ").");
			return false;
		default:
			return false;
		}
	}

	public void Dispose()
	{
		this.players = null;
		this.players = new Player[0];
	}

	public bool Instantiated(PhotonPlayer owner, GameResource type)
	{
		if (this.TryGetPlayer(owner.ID, out var result))
		{
			if (this.players[result].IsThingExcessive(type))
			{
				if (owner != null && PhotonNetwork.isMasterClient)
				{
					FengGameManagerMKII.instance.kickPlayerRC(owner, ban: true, "spamming instantiate (" + type.ToString() + ").");
				}
				RCextensions.RemoveAt(ref this.players, result);
				return false;
			}
		}
		else
		{
			RCextensions.Add(ref this.players, new Player(owner.ID));
			this.players[this.players.Length - 1].IsThingExcessive(type);
		}
		return true;
	}

	public bool PropertiesChanged(PhotonPlayer owner)
	{
		if (this.TryGetPlayer(owner.ID, out var result))
		{
			if (this.players[result].IsThingExcessive(GameResource.name))
			{
				return false;
			}
		}
		else
		{
			RCextensions.Add(ref this.players, new Player(owner.ID));
			this.players[this.players.Length - 1].IsThingExcessive(GameResource.name);
		}
		return true;
	}

	public void resetPropertyTracking(int ID)
	{
		if (this.TryGetPlayer(ID, out var result))
		{
			this.players[result].resetNameTracking();
		}
	}

	private bool TryGetPlayer(int id, out int result)
	{
		for (int i = 0; i < this.players.Length; i++)
		{
			if (this.players[i].id == id)
			{
				result = i;
				return true;
			}
		}
		result = -1;
		return false;
	}

	public void TryRemovePlayer(int playerId)
	{
		for (int i = 0; i < this.players.Length; i++)
		{
			if (this.players[i].id == playerId)
			{
				RCextensions.RemoveAt(ref this.players, i);
				break;
			}
		}
	}
}
