using UnityEngine;

public class LevelInfo
{
	public string desc;

	public int enemyNumber;

	public bool hint;

	public bool horse;

	private static bool init;

	public bool lavaMode;

	public static LevelInfo[] levels;

	public string mapName;

	public Minimap.Preset minimapPreset;

	public string name;

	public bool noCrawler;

	public bool punk = true;

	public bool pvp;

	public RespawnMode respawnMode;

	public bool supply = true;

	public bool teamTitan;

	public GAMEMODE type;

	public static LevelInfo getInfo(string name)
	{
		LevelInfo.Init();
		LevelInfo[] array = LevelInfo.levels;
		foreach (LevelInfo levelInfo in array)
		{
			if (levelInfo.name == name)
			{
				return levelInfo;
			}
		}
		return null;
	}

	public static void Init()
	{
		if (!LevelInfo.init)
		{
			LevelInfo.init = true;
			LevelInfo.levels = new LevelInfo[27]
			{
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo(),
				new LevelInfo()
			};
			LevelInfo.levels[0].name = "The City";
			LevelInfo.levels[0].mapName = "The City I";
			LevelInfo.levels[0].desc = "kill all the titans with your friends.(No RESPAWN/SUPPLY/PLAY AS TITAN)";
			LevelInfo.levels[0].enemyNumber = 10;
			LevelInfo.levels[0].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[0].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[0].supply = true;
			LevelInfo.levels[0].teamTitan = true;
			LevelInfo.levels[0].pvp = true;
			LevelInfo.levels[1].name = "The City II";
			LevelInfo.levels[1].mapName = "The City I";
			LevelInfo.levels[1].desc = "Fight the titans with your friends.(RESPAWN AFTER 10 SECONDS/SUPPLY/TEAM TITAN)";
			LevelInfo.levels[1].enemyNumber = 10;
			LevelInfo.levels[1].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[1].respawnMode = RespawnMode.DEATHMATCH;
			LevelInfo.levels[1].supply = true;
			LevelInfo.levels[1].teamTitan = true;
			LevelInfo.levels[1].pvp = true;
			LevelInfo.levels[2].name = "Cage Fighting";
			LevelInfo.levels[2].mapName = "Cage Fighting";
			LevelInfo.levels[2].desc = "2 players in different cages. when you kill a titan,  one or more titan will spawn to your opponent's cage.";
			LevelInfo.levels[2].enemyNumber = 1;
			LevelInfo.levels[2].type = GAMEMODE.CAGE_FIGHT;
			LevelInfo.levels[2].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[3].name = "The Forest";
			LevelInfo.levels[3].mapName = "The Forest";
			LevelInfo.levels[3].desc = "The Forest Of Giant Trees.(No RESPAWN/SUPPLY/PLAY AS TITAN)";
			LevelInfo.levels[3].enemyNumber = 5;
			LevelInfo.levels[3].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[3].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[3].supply = true;
			LevelInfo.levels[3].teamTitan = true;
			LevelInfo.levels[3].pvp = true;
			LevelInfo.levels[4].name = "The Forest II";
			LevelInfo.levels[4].mapName = "The Forest";
			LevelInfo.levels[4].desc = "Survive for 20 waves.";
			LevelInfo.levels[4].enemyNumber = 3;
			LevelInfo.levels[4].type = GAMEMODE.SURVIVE_MODE;
			LevelInfo.levels[4].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[4].supply = true;
			LevelInfo.levels[5].name = "The Forest III";
			LevelInfo.levels[5].mapName = "The Forest";
			LevelInfo.levels[5].desc = "Survive for 20 waves.player will respawn in every new wave";
			LevelInfo.levels[5].enemyNumber = 3;
			LevelInfo.levels[5].type = GAMEMODE.SURVIVE_MODE;
			LevelInfo.levels[5].respawnMode = RespawnMode.NEWROUND;
			LevelInfo.levels[5].supply = true;
			LevelInfo.levels[6].name = "Annie";
			LevelInfo.levels[6].mapName = "The Forest";
			LevelInfo.levels[6].desc = "Nape Armor/ Ankle Armor:\nNormal:1000/50\nHard:2500/100\nAbnormal:4000/200\nYou only have 1 life.Don't do this alone.";
			LevelInfo.levels[6].enemyNumber = 15;
			LevelInfo.levels[6].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[6].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[6].punk = false;
			LevelInfo.levels[6].pvp = true;
			LevelInfo.levels[7].name = "Annie II";
			LevelInfo.levels[7].mapName = "The Forest";
			LevelInfo.levels[7].desc = "Nape Armor/ Ankle Armor:\nNormal:1000/50\nHard:3000/200\nAbnormal:6000/1000\n(RESPAWN AFTER 10 SECONDS)";
			LevelInfo.levels[7].enemyNumber = 15;
			LevelInfo.levels[7].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[7].respawnMode = RespawnMode.DEATHMATCH;
			LevelInfo.levels[7].punk = false;
			LevelInfo.levels[7].pvp = true;
			LevelInfo.levels[8].name = "Colossal Titan";
			LevelInfo.levels[8].mapName = "Colossal Titan";
			LevelInfo.levels[8].desc = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\n Nape Armor:\n Normal:2000\nHard:3500\nAbnormal:5000\n";
			LevelInfo.levels[8].enemyNumber = 2;
			LevelInfo.levels[8].type = GAMEMODE.BOSS_FIGHT_CT;
			LevelInfo.levels[8].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[9].name = "Colossal Titan II";
			LevelInfo.levels[9].mapName = "Colossal Titan";
			LevelInfo.levels[9].desc = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\n Nape Armor:\n Normal:5000\nHard:8000\nAbnormal:12000\n(RESPAWN AFTER 10 SECONDS)";
			LevelInfo.levels[9].enemyNumber = 2;
			LevelInfo.levels[9].type = GAMEMODE.BOSS_FIGHT_CT;
			LevelInfo.levels[9].respawnMode = RespawnMode.DEATHMATCH;
			LevelInfo.levels[10].name = "Trost";
			LevelInfo.levels[10].mapName = "Colossal Titan";
			LevelInfo.levels[10].desc = "Escort Titan Eren";
			LevelInfo.levels[10].enemyNumber = 2;
			LevelInfo.levels[10].type = GAMEMODE.TROST;
			LevelInfo.levels[10].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[10].punk = false;
			LevelInfo.levels[11].name = "Trost II";
			LevelInfo.levels[11].mapName = "Colossal Titan";
			LevelInfo.levels[11].desc = "Escort Titan Eren(RESPAWN AFTER 10 SECONDS)";
			LevelInfo.levels[11].enemyNumber = 2;
			LevelInfo.levels[11].type = GAMEMODE.TROST;
			LevelInfo.levels[11].respawnMode = RespawnMode.DEATHMATCH;
			LevelInfo.levels[11].punk = false;
			LevelInfo.levels[12].name = "[S]City";
			LevelInfo.levels[12].mapName = "The City I";
			LevelInfo.levels[12].desc = "Kill all 15 Titans";
			LevelInfo.levels[12].enemyNumber = 15;
			LevelInfo.levels[12].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[12].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[12].supply = true;
			LevelInfo.levels[13].name = "[S]Forest";
			LevelInfo.levels[13].mapName = "The Forest";
			LevelInfo.levels[13].desc = string.Empty;
			LevelInfo.levels[13].enemyNumber = 15;
			LevelInfo.levels[13].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[13].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[13].supply = true;
			LevelInfo.levels[14].name = "[S]Forest Survive(no crawler)";
			LevelInfo.levels[14].mapName = "The Forest";
			LevelInfo.levels[14].desc = string.Empty;
			LevelInfo.levels[14].enemyNumber = 3;
			LevelInfo.levels[14].type = GAMEMODE.SURVIVE_MODE;
			LevelInfo.levels[14].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[14].supply = true;
			LevelInfo.levels[14].noCrawler = true;
			LevelInfo.levels[14].punk = true;
			LevelInfo.levels[15].name = "[S]Tutorial";
			LevelInfo.levels[15].mapName = "tutorial";
			LevelInfo.levels[15].desc = string.Empty;
			LevelInfo.levels[15].enemyNumber = 1;
			LevelInfo.levels[15].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[15].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[15].supply = true;
			LevelInfo.levels[15].hint = true;
			LevelInfo.levels[15].punk = false;
			LevelInfo.levels[16].name = "[S]Battle training";
			LevelInfo.levels[16].mapName = "tutorial 1";
			LevelInfo.levels[16].desc = string.Empty;
			LevelInfo.levels[16].enemyNumber = 7;
			LevelInfo.levels[16].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[16].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[16].supply = true;
			LevelInfo.levels[16].punk = false;
			LevelInfo.levels[17].name = "The Forest IV  - LAVA";
			LevelInfo.levels[17].mapName = "The Forest";
			LevelInfo.levels[17].desc = "Survive for 20 waves.player will respawn in every new wave.\nNO CRAWLERS\n***YOU CAN'T TOUCH THE GROUND!***";
			LevelInfo.levels[17].enemyNumber = 3;
			LevelInfo.levels[17].type = GAMEMODE.SURVIVE_MODE;
			LevelInfo.levels[17].respawnMode = RespawnMode.NEWROUND;
			LevelInfo.levels[17].supply = true;
			LevelInfo.levels[17].noCrawler = true;
			LevelInfo.levels[17].lavaMode = true;
			LevelInfo.levels[18].name = "[S]Racing - Akina";
			LevelInfo.levels[18].mapName = "track - akina";
			LevelInfo.levels[18].desc = string.Empty;
			LevelInfo.levels[18].enemyNumber = 0;
			LevelInfo.levels[18].type = GAMEMODE.RACING;
			LevelInfo.levels[18].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[18].supply = false;
			LevelInfo.levels[19].name = "Racing - Akina";
			LevelInfo.levels[19].mapName = "track - akina";
			LevelInfo.levels[19].desc = string.Empty;
			LevelInfo.levels[19].enemyNumber = 0;
			LevelInfo.levels[19].type = GAMEMODE.RACING;
			LevelInfo.levels[19].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[19].supply = false;
			LevelInfo.levels[19].pvp = true;
			LevelInfo.levels[20].name = "Outside The Walls";
			LevelInfo.levels[20].mapName = "OutSide";
			LevelInfo.levels[20].desc = "Capture Checkpoint mode.";
			LevelInfo.levels[20].enemyNumber = 0;
			LevelInfo.levels[20].type = GAMEMODE.PVP_CAPTURE;
			LevelInfo.levels[20].respawnMode = RespawnMode.DEATHMATCH;
			LevelInfo.levels[20].supply = true;
			LevelInfo.levels[20].horse = true;
			LevelInfo.levels[20].teamTitan = true;
			LevelInfo.levels[21].name = "The City III";
			LevelInfo.levels[21].mapName = "The City I";
			LevelInfo.levels[21].desc = "Capture Checkpoint mode.";
			LevelInfo.levels[21].enemyNumber = 0;
			LevelInfo.levels[21].type = GAMEMODE.PVP_CAPTURE;
			LevelInfo.levels[21].respawnMode = RespawnMode.DEATHMATCH;
			LevelInfo.levels[21].supply = true;
			LevelInfo.levels[21].horse = false;
			LevelInfo.levels[21].teamTitan = true;
			LevelInfo.levels[22].name = "Cave Fight";
			LevelInfo.levels[22].mapName = "CaveFight";
			LevelInfo.levels[22].desc = "***Spoiler Alarm!***";
			LevelInfo.levels[22].enemyNumber = -1;
			LevelInfo.levels[22].type = GAMEMODE.PVP_AHSS;
			LevelInfo.levels[22].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[22].supply = true;
			LevelInfo.levels[22].horse = false;
			LevelInfo.levels[22].teamTitan = true;
			LevelInfo.levels[22].pvp = true;
			LevelInfo.levels[23].name = "House Fight";
			LevelInfo.levels[23].mapName = "HouseFight";
			LevelInfo.levels[23].desc = "***Spoiler Alarm!***";
			LevelInfo.levels[23].enemyNumber = -1;
			LevelInfo.levels[23].type = GAMEMODE.PVP_AHSS;
			LevelInfo.levels[23].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[23].supply = true;
			LevelInfo.levels[23].horse = false;
			LevelInfo.levels[23].teamTitan = true;
			LevelInfo.levels[23].pvp = true;
			LevelInfo.levels[24].name = "[S]Forest Survive(no crawler no punk)";
			LevelInfo.levels[24].mapName = "The Forest";
			LevelInfo.levels[24].desc = string.Empty;
			LevelInfo.levels[24].enemyNumber = 3;
			LevelInfo.levels[24].type = GAMEMODE.SURVIVE_MODE;
			LevelInfo.levels[24].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[24].supply = true;
			LevelInfo.levels[24].noCrawler = true;
			LevelInfo.levels[24].punk = false;
			LevelInfo.levels[25].name = "Custom";
			LevelInfo.levels[25].mapName = "The Forest";
			LevelInfo.levels[25].desc = "Custom Map.";
			LevelInfo.levels[25].enemyNumber = 1;
			LevelInfo.levels[25].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[25].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[25].supply = true;
			LevelInfo.levels[25].teamTitan = true;
			LevelInfo.levels[25].pvp = true;
			LevelInfo.levels[25].punk = true;
			LevelInfo.levels[26].name = "Custom (No PT)";
			LevelInfo.levels[26].mapName = "The Forest";
			LevelInfo.levels[26].desc = "Custom Map (No Player Titans).";
			LevelInfo.levels[26].enemyNumber = 1;
			LevelInfo.levels[26].type = GAMEMODE.KILL_TITAN;
			LevelInfo.levels[26].respawnMode = RespawnMode.NEVER;
			LevelInfo.levels[26].pvp = true;
			LevelInfo.levels[26].punk = true;
			LevelInfo.levels[26].supply = true;
			LevelInfo.levels[26].teamTitan = false;
			LevelInfo.levels[0].minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 731.9738f);
			LevelInfo.levels[8].minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f);
			LevelInfo.levels[9].minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f);
			LevelInfo.levels[18].minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f);
			LevelInfo.levels[19].minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f);
			LevelInfo.levels[20].minimapPreset = new Minimap.Preset(new Vector3(2549.4f, 0f, 3042.4f), 3697.16f);
			LevelInfo.levels[21].minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 734.9738f);
		}
	}
}
