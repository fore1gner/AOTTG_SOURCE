using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ApplicationManagers;
using CustomSkins;
using ExitGames.Client.Photon;
using GameManagers;
using Photon;
using Settings;
using UI;
using UnityEngine;

internal class FengGameManagerMKII : Photon.MonoBehaviour
{
	private enum LoginStates
	{
		notlogged,
		loggingin,
		loginfailed,
		loggedin
	}

	public static bool JustLeftRoom = false;

	public Dictionary<int, CannonValues> allowedToCannon;

	public Dictionary<string, Texture2D> assetCacheTextures;

	public static ExitGames.Client.Photon.Hashtable banHash;

	public static ExitGames.Client.Photon.Hashtable boolVariables;

	public static Dictionary<string, GameObject> CachedPrefabs;

	private ArrayList chatContent;

	public InRoomChat chatRoom;

	public GameObject checkpoint;

	private ArrayList cT;

	public static string currentLevel;

	private float currentSpeed;

	public static bool customLevelLoaded;

	public int cyanKills;

	public int difficulty;

	public float distanceSlider;

	private bool endRacing;

	private ArrayList eT;

	public static ExitGames.Client.Photon.Hashtable floatVariables;

	private ArrayList fT;

	private float gameEndCD;

	private float gameEndTotalCDtime = 9f;

	public bool gameStart;

	private bool gameTimesUp;

	public static ExitGames.Client.Photon.Hashtable globalVariables;

	public List<GameObject> groundList;

	public static bool hasLogged;

	private ArrayList heroes;

	public static ExitGames.Client.Photon.Hashtable heroHash;

	private int highestwave = 1;

	private ArrayList hooks;

	private int humanScore;

	public static List<int> ignoreList;

	public static ExitGames.Client.Photon.Hashtable imatitan;

	public static FengGameManagerMKII instance;

	public static ExitGames.Client.Photon.Hashtable intVariables;

	public static bool isAssetLoaded;

	public bool isFirstLoad;

	private bool isLosing;

	private bool isPlayer1Winning;

	private bool isPlayer2Winning;

	public bool isRecompiling;

	public bool isRestarting;

	public bool isSpawning;

	public bool isUnloading;

	private bool isWinning;

	public bool justSuicide;

	private ArrayList kicklist;

	private ArrayList killInfoGO = new ArrayList();

	public static bool LAN;

	public static string level = string.Empty;

	public List<string[]> levelCache;

	public static ExitGames.Client.Photon.Hashtable[] linkHash;

	private string localRacingResult;

	public static bool logicLoaded;

	public static int loginstate;

	public int magentaKills;

	private IN_GAME_MAIN_CAMERA mainCamera;

	public static bool masterRC;

	public int maxPlayers;

	private float maxSpeed;

	public float mouseSlider;

	private string myLastHero;

	private string myLastRespawnTag = "playerRespawn";

	public float myRespawnTime;

	public new string name;

	public static string nameField;

	public bool needChooseSide;

	public static bool noRestart;

	public static string oldScript;

	public static string oldScriptLogic;

	public static string passwordField;

	public float pauseWaitTime;

	public string playerList;

	public List<Vector3> playerSpawnsC;

	public List<Vector3> playerSpawnsM;

	public List<PhotonPlayer> playersRPC;

	public static ExitGames.Client.Photon.Hashtable playerVariables;

	public Dictionary<string, int[]> PreservedPlayerKDR;

	public static string PrivateServerAuthPass;

	public static string privateServerField;

	public static string privateLobbyField;

	public int PVPhumanScore;

	private int PVPhumanScoreMax = 200;

	public int PVPtitanScore;

	private int PVPtitanScoreMax = 200;

	public float qualitySlider;

	public List<GameObject> racingDoors;

	private ArrayList racingResult;

	public Vector3 racingSpawnPoint;

	public bool racingSpawnPointSet;

	public static AssetBundle RCassets;

	public static ExitGames.Client.Photon.Hashtable RCEvents;

	public static ExitGames.Client.Photon.Hashtable RCRegions;

	public static ExitGames.Client.Photon.Hashtable RCRegionTriggers;

	public static ExitGames.Client.Photon.Hashtable RCVariableNames;

	public List<float> restartCount;

	public bool restartingBomb;

	public bool restartingEren;

	public bool restartingHorse;

	public bool restartingMC;

	public bool restartingTitan;

	public float retryTime;

	public float roundTime;

	public Vector2 scroll;

	public Vector2 scroll2;

	public GameObject selectedObj;

	public static object[] settingsOld;

	private int single_kills;

	private int single_maxDamage;

	private int single_totalDamage;

	public List<GameObject> spectateSprites;

	private bool startRacing;

	public static ExitGames.Client.Photon.Hashtable stringVariables;

	private int[] teamScores;

	private int teamWinner;

	public Texture2D textureBackgroundBlack;

	public Texture2D textureBackgroundBlue;

	public int time = 600;

	private float timeElapse;

	private float timeTotalServer;

	private ArrayList titans;

	private int titanScore;

	public List<TitanSpawner> titanSpawners;

	public List<Vector3> titanSpawns;

	public static ExitGames.Client.Photon.Hashtable titanVariables;

	public float transparencySlider;

	public GameObject ui;

	public float updateTime;

	public static string usernameField;

	public int wave = 1;

	public Dictionary<string, Material> customMapMaterials;

	public float LastRoomPropertyCheckTime;

	private SkyboxCustomSkinLoader _skyboxCustomSkinLoader;

	private ForestCustomSkinLoader _forestCustomSkinLoader;

	private CityCustomSkinLoader _cityCustomSkinLoader;

	private CustomLevelCustomSkinLoader _customLevelCustomSkinLoader;

	public void OnJoinedLobby()
	{
		if (FengGameManagerMKII.JustLeftRoom)
		{
			PhotonNetwork.Disconnect();
			FengGameManagerMKII.JustLeftRoom = false;
		}
		else if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<MainMenu>() != null)
		{
			UIManager.CurrentMenu.GetComponent<MainMenu>().ShowMultiplayerRoomListPopup();
		}
	}

	private void Awake()
	{
		this._skyboxCustomSkinLoader = base.gameObject.AddComponent<SkyboxCustomSkinLoader>();
		this._forestCustomSkinLoader = base.gameObject.AddComponent<ForestCustomSkinLoader>();
		this._cityCustomSkinLoader = base.gameObject.AddComponent<CityCustomSkinLoader>();
		this._customLevelCustomSkinLoader = base.gameObject.AddComponent<CustomLevelCustomSkinLoader>();
		base.gameObject.AddComponent<CustomRPCManager>();
	}

	private string getMaterialHash(string material, string x, string y)
	{
		return material + "," + x + "," + y;
	}

	public void addCamera(IN_GAME_MAIN_CAMERA c)
	{
		this.mainCamera = c;
	}

	public void addCT(COLOSSAL_TITAN titan)
	{
		this.cT.Add(titan);
	}

	public void addET(TITAN_EREN hero)
	{
		this.eT.Add(hero);
	}

	public void addFT(FEMALE_TITAN titan)
	{
		this.fT.Add(titan);
	}

	public void addHero(HERO hero)
	{
		this.heroes.Add(hero);
	}

	public void addHook(Bullet h)
	{
		this.hooks.Add(h);
	}

	public void addTime(float time)
	{
		this.timeTotalServer -= time;
	}

	public void addTitan(TITAN titan)
	{
		this.titans.Add(titan);
	}

	private void cache()
	{
		ClothFactory.ClearClothCache();
		this.chatRoom = GameObject.Find("Chatroom").GetComponent<InRoomChat>();
		this.playersRPC.Clear();
		this.titanSpawners.Clear();
		this.groundList.Clear();
		this.PreservedPlayerKDR = new Dictionary<string, int[]>();
		FengGameManagerMKII.noRestart = false;
		this.isSpawning = false;
		this.retryTime = 0f;
		FengGameManagerMKII.logicLoaded = false;
		FengGameManagerMKII.customLevelLoaded = true;
		this.isUnloading = false;
		this.isRecompiling = false;
		Time.timeScale = 1f;
		Camera.main.farClipPlane = 1500f;
		this.pauseWaitTime = 0f;
		this.spectateSprites = new List<GameObject>();
		this.isRestarting = false;
		if (PhotonNetwork.isMasterClient)
		{
			base.StartCoroutine(this.WaitAndResetRestarts());
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			this.roundTime = 0f;
			if (FengGameManagerMKII.level.StartsWith("Custom"))
			{
				FengGameManagerMKII.customLevelLoaded = false;
			}
			if (PhotonNetwork.isMasterClient)
			{
				if (this.isFirstLoad)
				{
					this.setGameSettings(this.checkGameGUI());
				}
				if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
				{
					base.StartCoroutine(this.respawnE(SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value));
				}
			}
		}
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round Start.");
		}
		this.isFirstLoad = false;
		this.RecompilePlayerList(0.5f);
	}

	[RPC]
	private void Chat(string content, string sender, PhotonMessageInfo info)
	{
		if (sender != string.Empty)
		{
			content = sender + ":" + content;
		}
		content = "<color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
		this.chatRoom.addLINE(content);
	}

	[RPC]
	private void ChatPM(string sender, string content, PhotonMessageInfo info)
	{
		content = sender + ":" + content;
		content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
		this.chatRoom.addLINE(content);
	}

	private ExitGames.Client.Photon.Hashtable checkGameGUI()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		LegacyGameSettings legacyGameSettingsUI = SettingsManager.LegacyGameSettingsUI;
		if (legacyGameSettingsUI.InfectionModeEnabled.Value)
		{
			legacyGameSettingsUI.BombModeEnabled.Value = false;
			legacyGameSettingsUI.TeamMode.Value = 0;
			legacyGameSettingsUI.PointModeEnabled.Value = false;
			legacyGameSettingsUI.BladePVP.Value = 0;
			if (legacyGameSettingsUI.InfectionModeAmount.Value > PhotonNetwork.countOfPlayers)
			{
				legacyGameSettingsUI.InfectionModeAmount.Value = 1;
			}
			hashtable.Add("infection", legacyGameSettingsUI.InfectionModeAmount.Value);
			if (!SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value || SettingsManager.LegacyGameSettings.InfectionModeAmount.Value != legacyGameSettingsUI.InfectionModeAmount.Value)
			{
				FengGameManagerMKII.imatitan.Clear();
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
					ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
					hashtable2.Add(PhotonPlayerProperty.isTitan, 1);
					photonPlayer.SetCustomProperties(hashtable2);
				}
				int num = PhotonNetwork.playerList.Length;
				int num2 = legacyGameSettingsUI.InfectionModeAmount.Value;
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer photonPlayer2 = PhotonNetwork.playerList[i];
					if (num > 0 && UnityEngine.Random.Range(0f, 1f) <= (float)num2 / (float)num)
					{
						ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
						hashtable3.Add(PhotonPlayerProperty.isTitan, 2);
						photonPlayer2.SetCustomProperties(hashtable3);
						FengGameManagerMKII.imatitan.Add(photonPlayer2.ID, 2);
						num2--;
					}
					num--;
				}
			}
		}
		if (legacyGameSettingsUI.BombModeEnabled.Value)
		{
			hashtable.Add("bomb", 1);
		}
		if (legacyGameSettingsUI.BombModeCeiling.Value)
		{
			hashtable.Add("bombCeiling", 1);
		}
		else
		{
			hashtable.Add("bombCeiling", 0);
		}
		if (legacyGameSettingsUI.BombModeInfiniteGas.Value)
		{
			hashtable.Add("bombInfiniteGas", 1);
		}
		else
		{
			hashtable.Add("bombInfiniteGas", 0);
		}
		if (legacyGameSettingsUI.GlobalHideNames.Value)
		{
			hashtable.Add("globalHideNames", 1);
		}
		if (legacyGameSettingsUI.GlobalMinimapDisable.Value)
		{
			hashtable.Add("globalDisableMinimap", 1);
		}
		if (legacyGameSettingsUI.TeamMode.Value > 0)
		{
			hashtable.Add("team", legacyGameSettingsUI.TeamMode.Value);
			if (SettingsManager.LegacyGameSettings.TeamMode.Value != legacyGameSettingsUI.TeamMode.Value)
			{
				int num2 = 1;
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
					switch (num2)
					{
					case 1:
						base.photonView.RPC("setTeamRPC", photonPlayer, 1);
						num2 = 2;
						break;
					case 2:
						base.photonView.RPC("setTeamRPC", photonPlayer, 2);
						num2 = 1;
						break;
					}
				}
			}
		}
		if (legacyGameSettingsUI.PointModeEnabled.Value)
		{
			hashtable.Add("point", legacyGameSettingsUI.PointModeAmount.Value);
		}
		if (!legacyGameSettingsUI.RockThrowEnabled.Value)
		{
			hashtable.Add("rock", 1);
		}
		if (legacyGameSettingsUI.TitanExplodeEnabled.Value)
		{
			hashtable.Add("explode", legacyGameSettingsUI.TitanExplodeRadius.Value);
		}
		if (legacyGameSettingsUI.TitanHealthMode.Value > 0)
		{
			hashtable.Add("healthMode", legacyGameSettingsUI.TitanHealthMode.Value);
			hashtable.Add("healthLower", legacyGameSettingsUI.TitanHealthMin.Value);
			hashtable.Add("healthUpper", legacyGameSettingsUI.TitanHealthMax.Value);
		}
		if (legacyGameSettingsUI.KickShifters.Value)
		{
			hashtable.Add("eren", 1);
		}
		if (legacyGameSettingsUI.TitanNumberEnabled.Value)
		{
			hashtable.Add("titanc", legacyGameSettingsUI.TitanNumber.Value);
		}
		if (legacyGameSettingsUI.TitanArmorEnabled.Value)
		{
			hashtable.Add("damage", legacyGameSettingsUI.TitanArmor.Value);
		}
		if (legacyGameSettingsUI.TitanSizeEnabled.Value)
		{
			hashtable.Add("sizeMode", 1);
			hashtable.Add("sizeLower", legacyGameSettingsUI.TitanSizeMin.Value);
			hashtable.Add("sizeUpper", legacyGameSettingsUI.TitanSizeMax.Value);
		}
		if (legacyGameSettingsUI.TitanSpawnEnabled.Value)
		{
			if (legacyGameSettingsUI.TitanSpawnNormal.Value + legacyGameSettingsUI.TitanSpawnAberrant.Value + legacyGameSettingsUI.TitanSpawnCrawler.Value + legacyGameSettingsUI.TitanSpawnJumper.Value + legacyGameSettingsUI.TitanSpawnPunk.Value > 100f)
			{
				legacyGameSettingsUI.TitanSpawnNormal.Value = 20f;
				legacyGameSettingsUI.TitanSpawnAberrant.Value = 20f;
				legacyGameSettingsUI.TitanSpawnCrawler.Value = 20f;
				legacyGameSettingsUI.TitanSpawnJumper.Value = 20f;
				legacyGameSettingsUI.TitanSpawnPunk.Value = 20f;
			}
			hashtable.Add("spawnMode", 1);
			hashtable.Add("nRate", legacyGameSettingsUI.TitanSpawnNormal.Value);
			hashtable.Add("aRate", legacyGameSettingsUI.TitanSpawnAberrant.Value);
			hashtable.Add("jRate", legacyGameSettingsUI.TitanSpawnJumper.Value);
			hashtable.Add("cRate", legacyGameSettingsUI.TitanSpawnCrawler.Value);
			hashtable.Add("pRate", legacyGameSettingsUI.TitanSpawnPunk.Value);
		}
		if (legacyGameSettingsUI.AllowHorses.Value)
		{
			hashtable.Add("horse", 1);
		}
		if (legacyGameSettingsUI.TitanPerWavesEnabled.Value)
		{
			hashtable.Add("waveModeOn", 1);
			hashtable.Add("waveModeNum", legacyGameSettingsUI.TitanPerWaves.Value);
		}
		if (legacyGameSettingsUI.FriendlyMode.Value)
		{
			hashtable.Add("friendly", 1);
		}
		if (legacyGameSettingsUI.BladePVP.Value > 0)
		{
			hashtable.Add("pvp", legacyGameSettingsUI.BladePVP.Value);
		}
		if (legacyGameSettingsUI.TitanMaxWavesEnabled.Value)
		{
			hashtable.Add("maxwave", legacyGameSettingsUI.TitanMaxWaves.Value);
		}
		if (legacyGameSettingsUI.EndlessRespawnEnabled.Value)
		{
			hashtable.Add("endless", legacyGameSettingsUI.EndlessRespawnTime.Value);
		}
		if (legacyGameSettingsUI.Motd.Value != string.Empty)
		{
			hashtable.Add("motd", legacyGameSettingsUI.Motd.Value);
		}
		if (!legacyGameSettingsUI.AHSSAirReload.Value)
		{
			hashtable.Add("ahssReload", 1);
		}
		if (!legacyGameSettingsUI.PunksEveryFive.Value)
		{
			hashtable.Add("punkWaves", 1);
		}
		if (legacyGameSettingsUI.CannonsFriendlyFire.Value)
		{
			hashtable.Add("deadlycannons", 1);
		}
		if (legacyGameSettingsUI.RacingEndless.Value)
		{
			hashtable.Add("asoracing", 1);
		}
		hashtable.Add("racingStartTime", legacyGameSettingsUI.RacingStartTime.Value);
		LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
		legacyGameSettings.PreserveKDR.Value = legacyGameSettingsUI.PreserveKDR.Value;
		legacyGameSettings.TitanSpawnCap.Value = legacyGameSettingsUI.TitanSpawnCap.Value;
		legacyGameSettings.GameType.Value = legacyGameSettingsUI.GameType.Value;
		legacyGameSettings.LevelScript.Value = legacyGameSettingsUI.LevelScript.Value;
		legacyGameSettings.LogicScript.Value = legacyGameSettingsUI.LogicScript.Value;
		return hashtable;
	}

	private bool checkIsTitanAllDie()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TITAN>() != null && !gameObject.GetComponent<TITAN>().hasDie)
			{
				return false;
			}
			if (gameObject.GetComponent<FEMALE_TITAN>() != null)
			{
				return false;
			}
		}
		return true;
	}

	public void checkPVPpts()
	{
		if (this.PVPtitanScore >= this.PVPtitanScoreMax)
		{
			this.PVPtitanScore = this.PVPtitanScoreMax;
			this.gameLose2();
		}
		else if (this.PVPhumanScore >= this.PVPhumanScoreMax)
		{
			this.PVPhumanScore = this.PVPhumanScoreMax;
			this.gameWin2();
		}
	}

	[RPC]
	private void clearlevel(string[] link, int gametype, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			switch (gametype)
			{
			case 0:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.KILL_TITAN;
				break;
			case 1:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.SURVIVE_MODE;
				break;
			case 2:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.PVP_AHSS;
				break;
			case 3:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.RACING;
				break;
			case 4:
				IN_GAME_MAIN_CAMERA.gamemode = GAMEMODE.None;
				break;
			}
			if (info.sender.isMasterClient && link.Length > 6)
			{
				base.StartCoroutine(this.clearlevelE(link));
			}
		}
	}

	private IEnumerator clearlevelE(string[] skybox)
	{
		if (this.IsValidSkybox(skybox))
		{
			yield return base.StartCoroutine(this._skyboxCustomSkinLoader.LoadSkinsFromRPC(skybox));
			yield return base.StartCoroutine(this._customLevelCustomSkinLoader.LoadSkinsFromRPC(skybox));
		}
		else
		{
			SkyboxCustomSkinLoader.SkyboxMaterial = null;
		}
		base.StartCoroutine(this.reloadSky());
	}

	public void compileScript(string str)
	{
		string[] array = str.Replace(" ", string.Empty).Split(new string[2] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == "{")
			{
				num++;
				continue;
			}
			if (array[i] == "}")
			{
				num2++;
				continue;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			string text = array[i];
			for (int j = 0; j < text.Length; j++)
			{
				switch (text[j])
				{
				case '(':
					num3++;
					break;
				case ')':
					num4++;
					break;
				case '"':
					num5++;
					break;
				}
			}
			if (num3 != num4)
			{
				this.chatRoom.addLINE("Script Error: Parentheses not equal! (line " + (i + 1) + ")");
				flag = true;
			}
			if (num5 % 2 != 0)
			{
				this.chatRoom.addLINE("Script Error: Quotations not equal! (line " + (i + 1) + ")");
				flag = true;
			}
		}
		if (num != num2)
		{
			this.chatRoom.addLINE("Script Error: Bracket count not equivalent!");
			flag = true;
		}
		if (flag)
		{
			return;
		}
		try
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].StartsWith("On") || !(array[i + 1] == "{"))
				{
					continue;
				}
				int num6 = i;
				int num7 = i + 2;
				int num8 = 0;
				for (int k = i + 2; k < array.Length; k++)
				{
					if (array[k] == "{")
					{
						num8++;
					}
					if (array[k] == "}")
					{
						if (num8 > 0)
						{
							num8--;
							continue;
						}
						num7 = k - 1;
						k = array.Length;
					}
				}
				hashtable.Add(num6, num7);
				i = num7;
			}
			foreach (int key in hashtable.Keys)
			{
				string text2 = array[key];
				int num7 = (int)hashtable[key];
				string[] array2 = new string[num7 - key + 1];
				int num10 = 0;
				for (int i = key; i <= num7; i++)
				{
					array2[num10] = array[i];
					num10++;
				}
				RCEvent rCEvent = this.parseBlock(array2, 0, 0, null);
				if (text2.StartsWith("OnPlayerEnterRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (FengGameManagerMKII.RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)FengGameManagerMKII.RCRegionTriggers[text3];
						regionTrigger.playerEventEnter = rCEvent;
						regionTrigger.myName = text3;
						FengGameManagerMKII.RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							playerEventEnter = rCEvent,
							myName = text3
						};
						FengGameManagerMKII.RCRegionTriggers.Add(text3, regionTrigger);
					}
					FengGameManagerMKII.RCVariableNames.Add("OnPlayerEnterRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnPlayerLeaveRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (FengGameManagerMKII.RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)FengGameManagerMKII.RCRegionTriggers[text3];
						regionTrigger.playerEventExit = rCEvent;
						regionTrigger.myName = text3;
						FengGameManagerMKII.RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							playerEventExit = rCEvent,
							myName = text3
						};
						FengGameManagerMKII.RCRegionTriggers.Add(text3, regionTrigger);
					}
					FengGameManagerMKII.RCVariableNames.Add("OnPlayerExitRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnTitanEnterRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (FengGameManagerMKII.RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)FengGameManagerMKII.RCRegionTriggers[text3];
						regionTrigger.titanEventEnter = rCEvent;
						regionTrigger.myName = text3;
						FengGameManagerMKII.RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							titanEventEnter = rCEvent,
							myName = text3
						};
						FengGameManagerMKII.RCRegionTriggers.Add(text3, regionTrigger);
					}
					FengGameManagerMKII.RCVariableNames.Add("OnTitanEnterRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnTitanLeaveRegion"))
				{
					int num11 = text2.IndexOf('[');
					int num12 = text2.IndexOf(']');
					string text3 = text2.Substring(num11 + 2, num12 - num11 - 3);
					num11 = text2.IndexOf('(');
					num12 = text2.IndexOf(')');
					string value = text2.Substring(num11 + 2, num12 - num11 - 3);
					if (FengGameManagerMKII.RCRegionTriggers.ContainsKey(text3))
					{
						RegionTrigger regionTrigger = (RegionTrigger)FengGameManagerMKII.RCRegionTriggers[text3];
						regionTrigger.titanEventExit = rCEvent;
						regionTrigger.myName = text3;
						FengGameManagerMKII.RCRegionTriggers[text3] = regionTrigger;
					}
					else
					{
						RegionTrigger regionTrigger = new RegionTrigger
						{
							titanEventExit = rCEvent,
							myName = text3
						};
						FengGameManagerMKII.RCRegionTriggers.Add(text3, regionTrigger);
					}
					FengGameManagerMKII.RCVariableNames.Add("OnTitanExitRegion[" + text3 + "]", value);
				}
				else if (text2.StartsWith("OnFirstLoad()"))
				{
					FengGameManagerMKII.RCEvents.Add("OnFirstLoad", rCEvent);
				}
				else if (text2.StartsWith("OnRoundStart()"))
				{
					FengGameManagerMKII.RCEvents.Add("OnRoundStart", rCEvent);
				}
				else if (text2.StartsWith("OnUpdate()"))
				{
					FengGameManagerMKII.RCEvents.Add("OnUpdate", rCEvent);
				}
				else if (text2.StartsWith("OnTitanDie"))
				{
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					FengGameManagerMKII.RCVariableNames.Add("OnTitanDie", array3);
					FengGameManagerMKII.RCEvents.Add("OnTitanDie", rCEvent);
				}
				else if (text2.StartsWith("OnPlayerDieByTitan"))
				{
					FengGameManagerMKII.RCEvents.Add("OnPlayerDieByTitan", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					FengGameManagerMKII.RCVariableNames.Add("OnPlayerDieByTitan", array3);
				}
				else if (text2.StartsWith("OnPlayerDieByPlayer"))
				{
					FengGameManagerMKII.RCEvents.Add("OnPlayerDieByPlayer", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string[] array3 = text2.Substring(num11 + 1, num12 - num11 - 1).Split(',');
					array3[0] = array3[0].Substring(1, array3[0].Length - 2);
					array3[1] = array3[1].Substring(1, array3[1].Length - 2);
					FengGameManagerMKII.RCVariableNames.Add("OnPlayerDieByPlayer", array3);
				}
				else if (text2.StartsWith("OnChatInput"))
				{
					FengGameManagerMKII.RCEvents.Add("OnChatInput", rCEvent);
					int num11 = text2.IndexOf('(');
					int num12 = text2.LastIndexOf(')');
					string value = text2.Substring(num11 + 1, num12 - num11 - 1);
					FengGameManagerMKII.RCVariableNames.Add("OnChatInput", value.Substring(1, value.Length - 2));
				}
			}
		}
		catch (UnityException ex)
		{
			this.chatRoom.addLINE(ex.Message);
		}
	}

	public int conditionType(string str)
	{
		if (!str.StartsWith("Int"))
		{
			if (str.StartsWith("Bool"))
			{
				return 1;
			}
			if (str.StartsWith("String"))
			{
				return 2;
			}
			if (str.StartsWith("Float"))
			{
				return 3;
			}
			if (str.StartsWith("Titan"))
			{
				return 5;
			}
			if (str.StartsWith("Player"))
			{
				return 4;
			}
		}
		return 0;
	}

	private void core2()
	{
		if ((int)FengGameManagerMKII.settingsOld[64] >= 100)
		{
			this.coreeditor();
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && this.needChooseSide)
		{
			if (SettingsManager.InputSettings.Human.Flare1.GetKeyDown())
			{
				if (NGUITools.GetActive(this.ui.GetComponent<UIReferArray>().panels[3]))
				{
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], state: true);
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], state: false);
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], state: false);
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], state: false);
					Camera.main.GetComponent<SpectatorMovement>().disable = false;
					Camera.main.GetComponent<MouseLook>().disable = false;
				}
				else
				{
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], state: false);
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], state: false);
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], state: false);
					NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], state: true);
					Camera.main.GetComponent<SpectatorMovement>().disable = true;
					Camera.main.GetComponent<MouseLook>().disable = true;
				}
			}
			if (SettingsManager.InputSettings.General.Pause.GetKeyDown() && !GameMenu.Paused)
			{
				Camera.main.GetComponent<SpectatorMovement>().disable = true;
				Camera.main.GetComponent<MouseLook>().disable = true;
				GameMenu.TogglePause(pause: true);
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			this.coreadd();
			this.ShowHUDInfoTopLeft(this.playerList);
			if (Camera.main != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING && Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
			{
				this.ShowHUDInfoCenter("Press [F7D358]" + SettingsManager.InputSettings.General.SpectateNextPlayer.ToString() + "[-] to spectate the next player. \nPress [F7D358]" + SettingsManager.InputSettings.General.SpectatePreviousPlayer.ToString() + "[-] to spectate the previous player.\nPress [F7D358]" + SettingsManager.InputSettings.Human.AttackSpecial.ToString() + "[-] to enter the spectator mode.\n\n\n\n");
				if (LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.DEATHMATCH || SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value || ((SettingsManager.LegacyGameSettings.BombModeEnabled.Value || SettingsManager.LegacyGameSettings.BladePVP.Value > 0) && SettingsManager.LegacyGameSettings.PointModeEnabled.Value))
				{
					this.myRespawnTime += Time.deltaTime;
					int num = 5;
					if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
					{
						num = 10;
					}
					if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
					{
						num = SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value;
					}
					this.ShowHUDInfoCenterADD("Respawn in " + (num - (int)this.myRespawnTime) + "s.");
					if (this.myRespawnTime > (float)num)
					{
						this.myRespawnTime = 0f;
						Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
						if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
						{
							this.SpawnNonAITitan2(this.myLastHero);
						}
						else
						{
							base.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
						}
						Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
						this.ShowHUDInfoCenter(string.Empty);
					}
				}
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
			{
				if (!this.isLosing)
				{
					this.currentSpeed = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.rigidbody.velocity.magnitude;
					this.maxSpeed = Mathf.Max(this.maxSpeed, this.currentSpeed);
					this.ShowHUDInfoTopLeft("Current Speed : " + (int)this.currentSpeed + "\nMax Speed:" + this.maxSpeed);
				}
			}
			else
			{
				this.ShowHUDInfoTopLeft("Kills:" + this.single_kills + "\nMax Damage:" + this.single_maxDamage + "\nTotal Damage:" + this.single_totalDamage);
			}
		}
		if (this.isLosing && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					this.ShowHUDInfoCenter("Survive " + this.wave + " Waves!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
				else
				{
					this.ShowHUDInfoCenter("Humanity Fail!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
			}
			else
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					this.ShowHUDInfoCenter("Survive " + this.wave + " Waves!\nGame Restart in " + (int)this.gameEndCD + "s\n\n");
				}
				else
				{
					this.ShowHUDInfoCenter("Humanity Fail!\nAgain!\nGame Restart in " + (int)this.gameEndCD + "s\n\n");
				}
				if (this.gameEndCD <= 0f)
				{
					this.gameEndCD = 0f;
					if (PhotonNetwork.isMasterClient)
					{
						this.restartRC();
					}
					this.ShowHUDInfoCenter(string.Empty);
				}
				else
				{
					this.gameEndCD -= Time.deltaTime;
				}
			}
		}
		if (this.isWinning)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
				{
					this.ShowHUDInfoCenter((float)(int)(this.timeTotalServer * 10f) * 0.1f - 5f + "s !\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					this.ShowHUDInfoCenter("Survive All Waves!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
				else
				{
					this.ShowHUDInfoCenter("Humanity Win!\n Press " + SettingsManager.InputSettings.General.RestartGame.ToString() + " to Restart.\n\n\n");
				}
			}
			else
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
				{
					this.ShowHUDInfoCenter(this.localRacingResult + "\n\nGame Restart in " + (int)this.gameEndCD + "s");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					this.ShowHUDInfoCenter("Survive All Waves!\nGame Restart in " + (int)this.gameEndCD + "s\n\n");
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
				{
					if (SettingsManager.LegacyGameSettings.BladePVP.Value == 0 && !SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
					{
						this.ShowHUDInfoCenter("Team " + this.teamWinner + " Win!\nGame Restart in " + (int)this.gameEndCD + "s\n\n");
					}
					else
					{
						this.ShowHUDInfoCenter(string.Concat(new object[3]
						{
							"Round Ended!\nGame Restart in ",
							(int)this.gameEndCD,
							"s\n\n"
						}));
					}
				}
				else
				{
					this.ShowHUDInfoCenter("Humanity Win!\nGame Restart in " + (int)this.gameEndCD + "s\n\n");
				}
				if (this.gameEndCD <= 0f)
				{
					this.gameEndCD = 0f;
					if (PhotonNetwork.isMasterClient)
					{
						this.restartRC();
					}
					this.ShowHUDInfoCenter(string.Empty);
				}
				else
				{
					this.gameEndCD -= Time.deltaTime;
				}
			}
		}
		this.timeElapse += Time.deltaTime;
		this.roundTime += Time.deltaTime;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
			{
				if (!this.isWinning)
				{
					this.timeTotalServer += Time.deltaTime;
				}
			}
			else if (!this.isLosing && !this.isWinning)
			{
				this.timeTotalServer += Time.deltaTime;
			}
		}
		else
		{
			this.timeTotalServer += Time.deltaTime;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (!this.isWinning)
				{
					this.ShowHUDInfoTopCenter("Time : " + (this.timeTotalServer - 5f).ToString("0.00"));
				}
				if (this.timeTotalServer < 5f)
				{
					this.ShowHUDInfoCenter("RACE START IN " + (5f - this.timeTotalServer).ToString("0.00"));
				}
				else if (!this.startRacing)
				{
					this.ShowHUDInfoCenter(string.Empty);
					this.startRacing = true;
					this.endRacing = false;
					GameObject.Find("door").SetActive(value: false);
				}
			}
			else
			{
				float value = SettingsManager.LegacyGameSettings.RacingStartTime.Value;
				this.ShowHUDInfoTopCenter("Time : " + ((this.roundTime >= value) ? (this.roundTime - value).ToString("0.00") : "WAITING"));
				if (this.roundTime < value)
				{
					this.ShowHUDInfoCenter("RACE START IN " + (value - this.roundTime).ToString("0.00") + ((!(this.localRacingResult == string.Empty)) ? ("\nLast Round\n" + this.localRacingResult) : "\n\n"));
				}
				else if (!this.startRacing)
				{
					this.ShowHUDInfoCenter(string.Empty);
					this.startRacing = true;
					this.endRacing = false;
					GameObject gameObject = GameObject.Find("door");
					if (gameObject != null)
					{
						gameObject.SetActive(value: false);
					}
					if (this.racingDoors != null && FengGameManagerMKII.customLevelLoaded)
					{
						foreach (GameObject racingDoor in this.racingDoors)
						{
							racingDoor.SetActive(value: false);
						}
						this.racingDoors = null;
					}
				}
				else if (this.racingDoors != null && FengGameManagerMKII.customLevelLoaded)
				{
					foreach (GameObject racingDoor2 in this.racingDoors)
					{
						racingDoor2.SetActive(value: false);
					}
					this.racingDoors = null;
				}
				if (this.needChooseSide)
				{
					string text = SettingsManager.InputSettings.Human.Flare1.ToString();
					this.ShowHUDInfoTopCenterADD("\n\nPRESS " + text + " TO ENTER GAME");
				}
			}
			if (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide && FengGameManagerMKII.customLevelLoaded && !SettingsManager.LegacyGeneralSettings.SpecMode.Value)
			{
				this.myRespawnTime += Time.deltaTime;
				if (this.myRespawnTime > 1.5f)
				{
					this.myRespawnTime = 0f;
					Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
					if (this.checkpoint != null)
					{
						base.StartCoroutine(this.WaitAndRespawn2(0.1f, this.checkpoint));
					}
					else
					{
						base.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
					}
					Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
					this.ShowHUDInfoCenter(string.Empty);
				}
			}
		}
		if (this.timeElapse > 1f)
		{
			this.timeElapse -= 1f;
			string text2 = string.Empty;
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
			{
				text2 = text2 + "Time : " + (this.time - (int)this.timeTotalServer);
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.None)
			{
				text2 = "Titan Left: ";
				text2 = text2 + GameObject.FindGameObjectsWithTag("titan").Length + "  Time : ";
				text2 = ((IN_GAME_MAIN_CAMERA.gametype != 0) ? (text2 + (this.time - (int)this.timeTotalServer)) : (text2 + (int)this.timeTotalServer));
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
			{
				text2 = "Titan Left: ";
				text2 = text2 + GameObject.FindGameObjectsWithTag("titan").Length.ToString() + " Wave : " + this.wave;
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
			{
				text2 = "Time : ";
				text2 = text2 + (this.time - (int)this.timeTotalServer) + "\nDefeat the Colossal Titan.\nPrevent abnormal titan from running to the north gate";
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
			{
				string text3 = "| ";
				for (int i = 0; i < PVPcheckPoint.chkPts.Count; i++)
				{
					text3 = text3 + (PVPcheckPoint.chkPts[i] as PVPcheckPoint).getStateString() + " ";
				}
				text3 += "|";
				int num2 = this.time - (int)this.timeTotalServer;
				text2 = string.Concat(this.PVPtitanScoreMax - this.PVPtitanScore + "  " + text3 + "  " + (this.PVPhumanScoreMax - this.PVPhumanScore) + "\n", "Time : ", num2.ToString());
			}
			if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
			{
				text2 = text2 + "\n[00FFFF]Cyan:" + Convert.ToString(this.cyanKills) + "       [FF00FF]Magenta:" + Convert.ToString(this.magentaKills) + "[ffffff]";
			}
			if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
			{
				this.ShowHUDInfoTopCenter(text2);
			}
			text2 = string.Empty;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					text2 = "Time : ";
					text2 += (int)this.timeTotalServer;
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
			{
				text2 = "Humanity " + this.humanScore + " : Titan " + this.titanScore + " ";
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
			{
				text2 = "Humanity " + this.humanScore + " : Titan " + this.titanScore + " ";
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.CAGE_FIGHT)
			{
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					text2 = "Time : ";
					text2 += this.time - (int)this.timeTotalServer;
				}
				else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
				{
					for (int j = 0; j < this.teamScores.Length; j++)
					{
						string text4 = text2;
						text2 = text4 + ((j == 0) ? string.Empty : " : ") + "Team" + (j + 1) + " " + this.teamScores[j] + string.Empty;
					}
					text2 = text2 + "\nTime : " + (this.time - (int)this.timeTotalServer);
				}
			}
			this.ShowHUDInfoTopRight(text2);
			string text5 = ((IN_GAME_MAIN_CAMERA.difficulty < 0) ? "Trainning" : ((IN_GAME_MAIN_CAMERA.difficulty == 0) ? "Normal" : ((IN_GAME_MAIN_CAMERA.difficulty != 1) ? "Abnormal" : "Hard")));
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.CAGE_FIGHT)
			{
				this.ShowHUDInfoTopRightMAPNAME((int)this.roundTime + "s\n" + FengGameManagerMKII.level + " : " + text5);
			}
			else
			{
				this.ShowHUDInfoTopRightMAPNAME("\n" + FengGameManagerMKII.level + " : " + text5);
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				char[] separator = new char[1] { "`"[0] };
				string text6 = PhotonNetwork.room.name.Split(separator)[0];
				if (text6.Length > 20)
				{
					text6 = text6.Remove(19) + "...";
				}
				this.ShowHUDInfoTopRightMAPNAME("\n" + text6 + " [FFC000](" + Convert.ToString(PhotonNetwork.room.playerCount) + "/" + Convert.ToString(PhotonNetwork.room.maxPlayers) + ")");
				if (this.needChooseSide && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
				{
					string text7 = SettingsManager.InputSettings.Human.Flare1.ToString();
					this.ShowHUDInfoTopCenterADD("\n\nPRESS " + text7 + " TO ENTER GAME");
				}
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && this.killInfoGO.Count > 0 && this.killInfoGO[0] == null)
		{
			this.killInfoGO.RemoveAt(0);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !PhotonNetwork.isMasterClient || !(this.timeTotalServer > (float)this.time))
		{
			return;
		}
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
		this.gameStart = false;
		string text8 = string.Empty;
		string text9 = string.Empty;
		string text10 = string.Empty;
		string text11 = string.Empty;
		string text12 = string.Empty;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer != null)
			{
				text8 = text8 + photonPlayer.customProperties[PhotonPlayerProperty.name]?.ToString() + "\n";
				text9 = text9 + photonPlayer.customProperties[PhotonPlayerProperty.kills]?.ToString() + "\n";
				text10 = text10 + photonPlayer.customProperties[PhotonPlayerProperty.deaths]?.ToString() + "\n";
				text11 = text11 + photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]?.ToString() + "\n";
				text12 = text12 + photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]?.ToString() + "\n";
			}
		}
		string text13;
		if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_AHSS)
		{
			text13 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.SURVIVE_MODE) ? ("Humanity " + this.humanScore + " : Titan " + this.titanScore) : ("Highest Wave : " + this.highestwave));
		}
		else
		{
			text13 = string.Empty;
			for (int l = 0; l < this.teamScores.Length; l++)
			{
				text13 += ((l == 0) ? ("Team" + (l + 1) + " " + this.teamScores[l] + " ") : " : ");
			}
		}
		object[] parameters = new object[6] { text8, text9, text10, text11, text12, text13 };
		base.photonView.RPC("showResult", PhotonTargets.AllBuffered, parameters);
	}

	private void coreadd()
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.OnUpdate();
			if (FengGameManagerMKII.customLevelLoaded)
			{
				for (int i = 0; i < this.titanSpawners.Count; i++)
				{
					TitanSpawner titanSpawner = this.titanSpawners[i];
					titanSpawner.time -= Time.deltaTime;
					if (!(titanSpawner.time <= 0f) || this.titans.Count + this.fT.Count >= Math.Min(SettingsManager.LegacyGameSettings.TitanSpawnCap.Value, 80))
					{
						continue;
					}
					string text = titanSpawner.name;
					if (text == "spawnAnnie")
					{
						PhotonNetwork.Instantiate("FEMALE_TITAN", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);
					}
					else
					{
						GameObject gameObject = PhotonNetwork.Instantiate("TITAN_VER3.1", titanSpawner.location, new Quaternion(0f, 0f, 0f, 1f), 0);
						switch (text)
						{
						case "spawnAbnormal":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
							break;
						case "spawnJumper":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
							break;
						case "spawnCrawler":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
							break;
						case "spawnPunk":
							gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
							break;
						}
					}
					if (titanSpawner.endless)
					{
						titanSpawner.time = titanSpawner.delay;
					}
					else
					{
						this.titanSpawners.Remove(titanSpawner);
					}
				}
			}
		}
		if (!(Time.timeScale <= 0.1f))
		{
			return;
		}
		if (this.pauseWaitTime <= 3f)
		{
			this.pauseWaitTime -= Time.deltaTime * 1000000f;
			if (this.pauseWaitTime <= 1f)
			{
				Camera.main.farClipPlane = 1500f;
			}
			if (this.pauseWaitTime <= 0f)
			{
				this.pauseWaitTime = 0f;
				Time.timeScale = 1f;
			}
		}
		this.justRecompileThePlayerList();
	}

	private void coreeditor()
	{
		if (Input.GetKey(KeyCode.Tab))
		{
			GUI.FocusControl(null);
		}
		if (this.selectedObj != null)
		{
			float num = 0.2f;
			if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
			{
				num = 0.04f;
			}
			else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
			{
				num = 0.6f;
			}
			if (SettingsManager.InputSettings.General.Forward.GetKey())
			{
				this.selectedObj.transform.position += num * new Vector3(Camera.mainCamera.transform.forward.x, 0f, Camera.mainCamera.transform.forward.z);
			}
			else if (SettingsManager.InputSettings.General.Back.GetKey())
			{
				this.selectedObj.transform.position -= num * new Vector3(Camera.mainCamera.transform.forward.x, 0f, Camera.mainCamera.transform.forward.z);
			}
			if (SettingsManager.InputSettings.General.Left.GetKey())
			{
				this.selectedObj.transform.position -= num * new Vector3(Camera.mainCamera.transform.right.x, 0f, Camera.mainCamera.transform.right.z);
			}
			else if (SettingsManager.InputSettings.General.Right.GetKey())
			{
				this.selectedObj.transform.position += num * new Vector3(Camera.mainCamera.transform.right.x, 0f, Camera.mainCamera.transform.right.z);
			}
			if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
			{
				this.selectedObj.transform.position -= Vector3.up * num;
			}
			else if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
			{
				this.selectedObj.transform.position += Vector3.up * num;
			}
			if (!this.selectedObj.name.StartsWith("misc,region"))
			{
				if (SettingsManager.InputSettings.RCEditor.RotateRight.GetKey())
				{
					this.selectedObj.transform.Rotate(Vector3.up * num);
				}
				else if (SettingsManager.InputSettings.RCEditor.RotateLeft.GetKey())
				{
					this.selectedObj.transform.Rotate(Vector3.down * num);
				}
				if (SettingsManager.InputSettings.RCEditor.RotateCCW.GetKey())
				{
					this.selectedObj.transform.Rotate(Vector3.forward * num);
				}
				else if (SettingsManager.InputSettings.RCEditor.RotateCW.GetKey())
				{
					this.selectedObj.transform.Rotate(Vector3.back * num);
				}
				if (SettingsManager.InputSettings.RCEditor.RotateBack.GetKey())
				{
					this.selectedObj.transform.Rotate(Vector3.left * num);
				}
				else if (SettingsManager.InputSettings.RCEditor.RotateForward.GetKey())
				{
					this.selectedObj.transform.Rotate(Vector3.right * num);
				}
			}
			if (SettingsManager.InputSettings.RCEditor.Place.GetKeyDown())
			{
				FengGameManagerMKII.linkHash[3].Add(this.selectedObj.GetInstanceID(), this.selectedObj.name + "," + Convert.ToString(this.selectedObj.transform.position.x) + "," + Convert.ToString(this.selectedObj.transform.position.y) + "," + Convert.ToString(this.selectedObj.transform.position.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.x) + "," + Convert.ToString(this.selectedObj.transform.rotation.y) + "," + Convert.ToString(this.selectedObj.transform.rotation.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.w));
				this.selectedObj = null;
				Camera.main.GetComponent<MouseLook>().enabled = true;
			}
			if (SettingsManager.InputSettings.RCEditor.Delete.GetKeyDown())
			{
				UnityEngine.Object.Destroy(this.selectedObj);
				this.selectedObj = null;
				Camera.main.GetComponent<MouseLook>().enabled = true;
				FengGameManagerMKII.linkHash[3].Remove(this.selectedObj.GetInstanceID());
			}
			return;
		}
		if (Camera.main.GetComponent<MouseLook>().enabled)
		{
			float num2 = 100f;
			if (SettingsManager.InputSettings.RCEditor.Slow.GetKey())
			{
				num2 = 20f;
			}
			else if (SettingsManager.InputSettings.RCEditor.Fast.GetKey())
			{
				num2 = 400f;
			}
			Transform transform = Camera.main.transform;
			if (SettingsManager.InputSettings.General.Forward.GetKey())
			{
				transform.position += transform.forward * num2 * Time.deltaTime;
			}
			else if (SettingsManager.InputSettings.General.Back.GetKey())
			{
				transform.position -= transform.forward * num2 * Time.deltaTime;
			}
			if (SettingsManager.InputSettings.General.Left.GetKey())
			{
				transform.position -= transform.right * num2 * Time.deltaTime;
			}
			else if (SettingsManager.InputSettings.General.Right.GetKey())
			{
				transform.position += transform.right * num2 * Time.deltaTime;
			}
			if (SettingsManager.InputSettings.RCEditor.Up.GetKey())
			{
				transform.position += transform.up * num2 * Time.deltaTime;
			}
			else if (SettingsManager.InputSettings.RCEditor.Down.GetKey())
			{
				transform.position -= transform.up * num2 * Time.deltaTime;
			}
		}
		if (SettingsManager.InputSettings.RCEditor.Cursor.GetKeyDown())
		{
			if (Camera.main.GetComponent<MouseLook>().enabled)
			{
				Camera.main.GetComponent<MouseLook>().enabled = false;
			}
			else
			{
				Camera.main.GetComponent<MouseLook>().enabled = true;
			}
		}
		if (!Input.GetKeyDown(KeyCode.Mouse0) || Screen.lockCursor || GUIUtility.hotControl != 0 || ((!(Input.mousePosition.x > 300f) || !(Input.mousePosition.x < (float)Screen.width - 300f)) && !((float)Screen.height - Input.mousePosition.y > 600f)))
		{
			return;
		}
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
		{
			Transform transform2 = hitInfo.transform;
			if (transform2.gameObject.name.StartsWith("custom") || transform2.gameObject.name.StartsWith("base") || transform2.gameObject.name.StartsWith("racing") || transform2.gameObject.name.StartsWith("photon") || transform2.gameObject.name.StartsWith("spawnpoint") || transform2.gameObject.name.StartsWith("misc"))
			{
				this.selectedObj = transform2.gameObject;
				Camera.main.GetComponent<MouseLook>().enabled = false;
				Screen.lockCursor = true;
				FengGameManagerMKII.linkHash[3].Remove(this.selectedObj.GetInstanceID());
			}
			else if (transform2.parent.gameObject.name.StartsWith("custom") || transform2.parent.gameObject.name.StartsWith("base") || transform2.parent.gameObject.name.StartsWith("racing") || transform2.parent.gameObject.name.StartsWith("photon"))
			{
				this.selectedObj = transform2.parent.gameObject;
				Camera.main.GetComponent<MouseLook>().enabled = false;
				Screen.lockCursor = true;
				FengGameManagerMKII.linkHash[3].Remove(this.selectedObj.GetInstanceID());
			}
		}
	}

	private IEnumerator customlevelcache()
	{
		for (int i = 0; i < this.levelCache.Count; i++)
		{
			this.customlevelclientE(this.levelCache[i], renewHash: false);
			yield return new WaitForEndOfFrame();
		}
	}

	private void customlevelclientE(string[] content, bool renewHash)
	{
		bool flag = false;
		bool flag2 = false;
		if (content[content.Length - 1].StartsWith("a"))
		{
			flag = true;
			this.customMapMaterials.Clear();
		}
		else if (content[content.Length - 1].StartsWith("z"))
		{
			flag2 = true;
			FengGameManagerMKII.customLevelLoaded = true;
			this.spawnPlayerCustomMap();
			Minimap.TryRecaptureInstance();
			this.unloadAssets();
			Camera.main.GetComponent<TiltShift>().enabled = false;
		}
		if (renewHash)
		{
			if (flag)
			{
				FengGameManagerMKII.currentLevel = string.Empty;
				this.levelCache.Clear();
				this.titanSpawns.Clear();
				this.playerSpawnsC.Clear();
				this.playerSpawnsM.Clear();
				for (int i = 0; i < content.Length; i++)
				{
					string[] array = content[i].Split(',');
					if (array[0] == "titan")
					{
						this.titanSpawns.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
					else if (array[0] == "playerC")
					{
						this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
					else if (array[0] == "playerM")
					{
						this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(array[1]), Convert.ToSingle(array[2]), Convert.ToSingle(array[3])));
					}
				}
				this.spawnPlayerCustomMap();
			}
			FengGameManagerMKII.currentLevel += content[content.Length - 1];
			this.levelCache.Add(content);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.currentLevel, FengGameManagerMKII.currentLevel);
			PhotonNetwork.player.SetCustomProperties(hashtable);
		}
		if (flag || flag2)
		{
			return;
		}
		for (int i = 0; i < content.Length; i++)
		{
			string[] array = content[i].Split(',');
			float result;
			if (array[0].StartsWith("custom"))
			{
				float a = 1f;
				GameObject gameObject = null;
				gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])));
				if (array[2] != "default")
				{
					if (array[2].StartsWith("transparent"))
					{
						if (float.TryParse(array[2].Substring(11), out result))
						{
							a = result;
						}
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer in componentsInChildren)
						{
							renderer.material = (Material)FengGameManagerMKII.RCassets.Load("transparent");
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash = this.getMaterialHash(array[2], array[10], array[11]);
								if (this.customMapMaterials.ContainsKey(materialHash))
								{
									renderer.material = this.customMapMaterials[materialHash];
									continue;
								}
								renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								this.customMapMaterials.Add(materialHash, renderer.material);
							}
						}
					}
					else
					{
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer2 in componentsInChildren)
						{
							renderer2.material = (Material)FengGameManagerMKII.RCassets.Load(array[2]);
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash2 = this.getMaterialHash(array[2], array[10], array[11]);
								if (this.customMapMaterials.ContainsKey(materialHash2))
								{
									renderer2.material = this.customMapMaterials[materialHash2];
									continue;
								}
								renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer2.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								this.customMapMaterials.Add(materialHash2, renderer2.material);
							}
						}
					}
				}
				float num = gameObject.transform.localScale.x * Convert.ToSingle(array[3]);
				num -= 0.001f;
				float y = gameObject.transform.localScale.y * Convert.ToSingle(array[4]);
				float z = gameObject.transform.localScale.z * Convert.ToSingle(array[5]);
				gameObject.transform.localScale = new Vector3(num, y, z);
				if (!(array[6] != "0"))
				{
					continue;
				}
				Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
				MeshFilter[] componentsInChildren2 = gameObject.GetComponentsInChildren<MeshFilter>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					Mesh mesh = componentsInChildren2[j].mesh;
					Color[] array2 = new Color[mesh.vertexCount];
					for (int k = 0; k < mesh.vertexCount; k++)
					{
						array2[k] = color;
					}
					mesh.colors = array2;
				}
			}
			else if (array[0].StartsWith("base"))
			{
				if (array.Length < 15)
				{
					UnityEngine.Object.Instantiate(Resources.Load(array[1]), new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])));
					continue;
				}
				float a = 1f;
				GameObject gameObject = null;
				gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array[1]), new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])));
				if (array[2] != "default")
				{
					if (array[2].StartsWith("transparent"))
					{
						if (float.TryParse(array[2].Substring(11), out result))
						{
							a = result;
						}
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer3 in componentsInChildren)
						{
							renderer3.material = (Material)FengGameManagerMKII.RCassets.Load("transparent");
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash3 = this.getMaterialHash(array[2], array[10], array[11]);
								if (this.customMapMaterials.ContainsKey(materialHash3))
								{
									renderer3.material = this.customMapMaterials[materialHash3];
									continue;
								}
								renderer3.material.mainTextureScale = new Vector2(renderer3.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer3.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								this.customMapMaterials.Add(materialHash3, renderer3.material);
							}
						}
					}
					else
					{
						Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
						foreach (Renderer renderer4 in componentsInChildren)
						{
							if (renderer4.name.Contains("Particle System") && gameObject.name.Contains("aot_supply"))
							{
								continue;
							}
							renderer4.material = (Material)FengGameManagerMKII.RCassets.Load(array[2]);
							if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
							{
								string materialHash4 = this.getMaterialHash(array[2], array[10], array[11]);
								if (this.customMapMaterials.ContainsKey(materialHash4))
								{
									renderer4.material = this.customMapMaterials[materialHash4];
									continue;
								}
								renderer4.material.mainTextureScale = new Vector2(renderer4.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer4.material.mainTextureScale.y * Convert.ToSingle(array[11]));
								this.customMapMaterials.Add(materialHash4, renderer4.material);
							}
						}
					}
				}
				float num = gameObject.transform.localScale.x * Convert.ToSingle(array[3]);
				num -= 0.001f;
				float y = gameObject.transform.localScale.y * Convert.ToSingle(array[4]);
				float z = gameObject.transform.localScale.z * Convert.ToSingle(array[5]);
				gameObject.transform.localScale = new Vector3(num, y, z);
				if (!(array[6] != "0"))
				{
					continue;
				}
				Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
				MeshFilter[] componentsInChildren2 = gameObject.GetComponentsInChildren<MeshFilter>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					Mesh mesh = componentsInChildren2[j].mesh;
					Color[] array2 = new Color[mesh.vertexCount];
					for (int k = 0; k < mesh.vertexCount; k++)
					{
						array2[k] = color;
					}
					mesh.colors = array2;
				}
			}
			else if (array[0].StartsWith("misc"))
			{
				if (array[1].StartsWith("barrier"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
				}
				else if (array[1].StartsWith("racingStart"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					if (this.racingDoors != null)
					{
						this.racingDoors.Add(gameObject);
					}
				}
				else if (array[1].StartsWith("racingEnd"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.AddComponent<LevelTriggerRacingEnd>();
				}
				else if (array[1].StartsWith("region") && PhotonNetwork.isMasterClient)
				{
					Vector3 vector = new Vector3(Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8]));
					RCRegion rCRegion = new RCRegion(vector, Convert.ToSingle(array[3]), Convert.ToSingle(array[4]), Convert.ToSingle(array[5]));
					string key = array[2];
					if (FengGameManagerMKII.RCRegionTriggers.ContainsKey(key))
					{
						GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load("region"));
						gameObject2.transform.position = vector;
						gameObject2.AddComponent<RegionTrigger>();
						gameObject2.GetComponent<RegionTrigger>().CopyTrigger((RegionTrigger)FengGameManagerMKII.RCRegionTriggers[key]);
						float num = gameObject2.transform.localScale.x * Convert.ToSingle(array[3]);
						num -= 0.001f;
						float y = gameObject2.transform.localScale.y * Convert.ToSingle(array[4]);
						float z = gameObject2.transform.localScale.z * Convert.ToSingle(array[5]);
						gameObject2.transform.localScale = new Vector3(num, y, z);
						rCRegion.myBox = gameObject2;
					}
					FengGameManagerMKII.RCRegions.Add(key, rCRegion);
				}
			}
			else if (array[0].StartsWith("racing"))
			{
				if (array[1].StartsWith("start"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					if (this.racingDoors != null)
					{
						this.racingDoors.Add(gameObject);
					}
				}
				else if (array[1].StartsWith("end"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.GetComponentInChildren<Collider>().gameObject.AddComponent<LevelTriggerRacingEnd>();
				}
				else if (array[1].StartsWith("kill"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingKillTrigger>();
				}
				else if (array[1].StartsWith("checkpoint"))
				{
					GameObject gameObject = null;
					gameObject = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array[1]), new Vector3(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7])), new Quaternion(Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), Convert.ToSingle(array[10]), Convert.ToSingle(array[11])));
					float num = gameObject.transform.localScale.x * Convert.ToSingle(array[2]);
					num -= 0.001f;
					float y = gameObject.transform.localScale.y * Convert.ToSingle(array[3]);
					float z = gameObject.transform.localScale.z * Convert.ToSingle(array[4]);
					gameObject.transform.localScale = new Vector3(num, y, z);
					gameObject.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingCheckpointTrigger>();
				}
			}
			else if (array[0].StartsWith("map"))
			{
				if (array[1].StartsWith("disablebounds"))
				{
					UnityEngine.Object.Destroy(GameObject.Find("gameobjectOutSide"));
					UnityEngine.Object.Instantiate(FengGameManagerMKII.RCassets.Load("outside"));
				}
			}
			else
			{
				if (!PhotonNetwork.isMasterClient || !array[0].StartsWith("photon"))
				{
					continue;
				}
				if (array[1].StartsWith("Cannon"))
				{
					if (array.Length > 15)
					{
						GameObject obj = PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
						obj.GetComponent<CannonPropRegion>().settings = content[i];
						obj.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, content[i]);
					}
					else
					{
						PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0).GetComponent<CannonPropRegion>().settings = content[i];
					}
					continue;
				}
				TitanSpawner titanSpawner = new TitanSpawner();
				float num = 30f;
				if (float.TryParse(array[2], out result))
				{
					num = Mathf.Max(Convert.ToSingle(array[2]), 1f);
				}
				titanSpawner.time = num;
				titanSpawner.delay = num;
				titanSpawner.name = array[1];
				if (array[3] == "1")
				{
					titanSpawner.endless = true;
				}
				else
				{
					titanSpawner.endless = false;
				}
				titanSpawner.location = new Vector3(Convert.ToSingle(array[4]), Convert.ToSingle(array[5]), Convert.ToSingle(array[6]));
				this.titanSpawners.Add(titanSpawner);
			}
		}
	}

	private IEnumerator customlevelE(List<PhotonPlayer> players)
	{
		string[] array;
		if (!(FengGameManagerMKII.currentLevel == string.Empty))
		{
			for (int i = 0; i < this.levelCache.Count; i++)
			{
				foreach (PhotonPlayer player in players)
				{
					if (player.customProperties[PhotonPlayerProperty.currentLevel] != null && FengGameManagerMKII.currentLevel != string.Empty && RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.currentLevel]) == FengGameManagerMKII.currentLevel)
					{
						if (i == 0)
						{
							array = new string[1] { "loadcached" };
							base.photonView.RPC("customlevelRPC", player, new object[1] { array });
						}
					}
					else
					{
						base.photonView.RPC("customlevelRPC", player, new object[1] { this.levelCache[i] });
					}
				}
				if (i > 0)
				{
					yield return new WaitForSeconds(0.75f);
				}
				else
				{
					yield return new WaitForSeconds(0.25f);
				}
			}
			yield break;
		}
		array = new string[1] { "loadempty" };
		foreach (PhotonPlayer player2 in players)
		{
			base.photonView.RPC("customlevelRPC", player2, new object[1] { array });
		}
		FengGameManagerMKII.customLevelLoaded = true;
	}

	[RPC]
	private void customlevelRPC(string[] content, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			if (content.Length == 1 && content[0] == "loadcached")
			{
				base.StartCoroutine(this.customlevelcache());
			}
			else if (content.Length == 1 && content[0] == "loadempty")
			{
				FengGameManagerMKII.currentLevel = string.Empty;
				this.levelCache.Clear();
				this.titanSpawns.Clear();
				this.playerSpawnsC.Clear();
				this.playerSpawnsM.Clear();
				this.customMapMaterials.Clear();
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.currentLevel, FengGameManagerMKII.currentLevel);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				FengGameManagerMKII.customLevelLoaded = true;
				this.spawnPlayerCustomMap();
			}
			else
			{
				this.customlevelclientE(content, renewHash: true);
			}
		}
	}

	public void debugChat(string str)
	{
		this.chatRoom.addLINE(str);
	}

	public void DestroyAllExistingCloths()
	{
		Cloth[] array = UnityEngine.Object.FindObjectsOfType<Cloth>();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				ClothFactory.DisposeObject(array[i].gameObject);
			}
		}
	}

	private void endGameInfectionRC()
	{
		FengGameManagerMKII.imatitan.Clear();
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonNetwork.playerList[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				PhotonPlayerProperty.isTitan,
				1
			} });
		}
		int num = PhotonNetwork.playerList.Length;
		int num2 = SettingsManager.LegacyGameSettings.InfectionModeAmount.Value;
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			if (num > 0 && UnityEngine.Random.Range(0f, 1f) <= (float)num2 / (float)num)
			{
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.isTitan, 2);
				photonPlayer.SetCustomProperties(hashtable);
				FengGameManagerMKII.imatitan.Add(photonPlayer.ID, 2);
				num2--;
			}
			num--;
		}
		this.gameEndCD = 0f;
		this.restartGame2();
	}

	private void endGameRC()
	{
		if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
		{
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonNetwork.playerList[i].SetCustomProperties(new ExitGames.Client.Photon.Hashtable
				{
					{
						PhotonPlayerProperty.kills,
						0
					},
					{
						PhotonPlayerProperty.deaths,
						0
					},
					{
						PhotonPlayerProperty.max_dmg,
						0
					},
					{
						PhotonPlayerProperty.total_dmg,
						0
					}
				});
			}
		}
		this.gameEndCD = 0f;
		this.restartGame2();
	}

	public void EnterSpecMode(bool enter)
	{
		if (enter)
		{
			this.spectateSprites = new List<GameObject>();
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				if (!(gameObject.GetComponent<UISprite>() != null) || !gameObject.activeInHierarchy)
				{
					continue;
				}
				string text = gameObject.name;
				if (text.Contains("blade") || text.Contains("bullet") || text.Contains("gas") || text.Contains("flare") || text.Contains("skill_cd"))
				{
					if (!this.spectateSprites.Contains(gameObject))
					{
						this.spectateSprites.Add(gameObject);
					}
					gameObject.SetActive(value: false);
				}
			}
			string[] array2 = new string[2] { "Flare", "LabelInfoBottomRight" };
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject gameObject2 = GameObject.Find(array2[i]);
				if (gameObject2 != null)
				{
					if (!this.spectateSprites.Contains(gameObject2))
					{
						this.spectateSprites.Add(gameObject2);
					}
					gameObject2.SetActive(value: false);
				}
			}
			foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
			{
				if (player.photonView.isMine)
				{
					PhotonNetwork.Destroy(player.photonView);
				}
			}
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.dead]))
			{
				foreach (TITAN titan in FengGameManagerMKII.instance.getTitans())
				{
					if (titan.photonView.isMine)
					{
						PhotonNetwork.Destroy(titan.photonView);
					}
				}
			}
			NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
			NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
			FengGameManagerMKII.instance.needChooseSide = false;
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("Player");
			if (gameObject3 != null && gameObject3.GetComponent<HERO>() != null)
			{
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject3);
			}
			else
			{
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
			}
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: false);
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			base.StartCoroutine(this.reloadSky(specmode: true));
			return;
		}
		if (GameObject.Find("cross1") != null)
		{
			GameObject.Find("cross1").transform.localPosition = Vector3.up * 5000f;
		}
		if (this.spectateSprites != null)
		{
			foreach (GameObject spectateSprite in this.spectateSprites)
			{
				if (spectateSprite != null)
				{
					spectateSprite.SetActive(value: true);
				}
			}
		}
		this.spectateSprites = new List<GameObject>();
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
		FengGameManagerMKII.instance.needChooseSide = true;
		Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void gameLose2()
	{
		if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || PhotonNetwork.isMasterClient) && !this.isWinning && !this.isLosing)
		{
			this.isLosing = true;
			this.titanScore++;
			this.gameEndCD = this.gameEndTotalCDtime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters = new object[1] { this.titanScore };
				base.photonView.RPC("netGameLose", PhotonTargets.Others, parameters);
			}
			if (SettingsManager.UISettings.GameFeed.Value)
			{
				this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
			}
		}
	}

	public void gameWin2()
	{
		if (this.isLosing || this.isWinning)
		{
			return;
		}
		this.isWinning = true;
		this.humanScore++;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
			{
				this.gameEndCD = 1000f;
			}
			else
			{
				this.gameEndCD = 20f;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters = new object[1] { 0 };
				base.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			this.gameEndCD = this.gameEndTotalCDtime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters2 = new object[1] { this.teamWinner };
				base.photonView.RPC("netGameWin", PhotonTargets.Others, parameters2);
			}
			this.teamScores[this.teamWinner - 1]++;
		}
		else
		{
			this.gameEndCD = this.gameEndTotalCDtime;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters3 = new object[1] { this.humanScore };
				base.photonView.RPC("netGameWin", PhotonTargets.Others, parameters3);
			}
		}
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
		}
	}

	public ArrayList getPlayers()
	{
		return this.heroes;
	}

	public ArrayList getErens()
	{
		return this.eT;
	}

	[RPC]
	private void getRacingResult(string player, float time, PhotonMessageInfo info)
	{
		if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.RACING)
		{
			if (info != null)
			{
				this.kickPlayerRCIfMC(info.sender, ban: true, "racing exploit");
			}
		}
		else
		{
			RacingResult value = new RacingResult
			{
				name = player,
				time = time
			};
			this.racingResult.Add(value);
			this.refreshRacingResult2();
		}
	}

	public ArrayList getTitans()
	{
		return this.titans;
	}

	private string hairtype(int lol)
	{
		if (lol < 0)
		{
			return "Random";
		}
		return "Male " + lol;
	}

	[RPC]
	private void ignorePlayer(int ID, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
			if (photonPlayer != null && !FengGameManagerMKII.ignoreList.Contains(ID))
			{
				for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
				{
					if (PhotonNetwork.playerList[i] == photonPlayer)
					{
						FengGameManagerMKII.ignoreList.Add(ID);
						RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
						raiseEventOptions.TargetActors = new int[1] { ID };
						RaiseEventOptions options = raiseEventOptions;
						PhotonNetwork.RaiseEvent(254, null, sendReliable: true, options);
					}
				}
			}
		}
		this.RecompilePlayerList(0.1f);
	}

	[RPC]
	private void ignorePlayerArray(int[] IDS, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			foreach (int num in IDS)
			{
				PhotonPlayer photonPlayer = PhotonPlayer.Find(num);
				if (photonPlayer == null || FengGameManagerMKII.ignoreList.Contains(num))
				{
					continue;
				}
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					if (PhotonNetwork.playerList[j] == photonPlayer)
					{
						FengGameManagerMKII.ignoreList.Add(num);
						RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
						raiseEventOptions.TargetActors = new int[1] { num };
						RaiseEventOptions options = raiseEventOptions;
						PhotonNetwork.RaiseEvent(254, null, sendReliable: true, options);
					}
				}
			}
		}
		this.RecompilePlayerList(0.1f);
	}

	public static GameObject InstantiateCustomAsset(string key)
	{
		key = key.Substring(8);
		return (GameObject)FengGameManagerMKII.RCassets.Load(key);
	}

	public bool isPlayerAllDead()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if ((int)photonPlayer.customProperties[PhotonPlayerProperty.isTitan] == 1)
			{
				num++;
				if ((bool)photonPlayer.customProperties[PhotonPlayerProperty.dead])
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public bool isPlayerAllDead2()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 1)
			{
				num++;
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public bool isTeamAllDead(int team)
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if ((int)photonPlayer.customProperties[PhotonPlayerProperty.isTitan] == 1 && (int)photonPlayer.customProperties[PhotonPlayerProperty.team] == team)
			{
				num++;
				if ((bool)photonPlayer.customProperties[PhotonPlayerProperty.dead])
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public bool isTeamAllDead2(int team)
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer.customProperties[PhotonPlayerProperty.isTitan] != null && photonPlayer.customProperties[PhotonPlayerProperty.team] != null && RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 1 && RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.team]) == team)
			{
				num++;
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public void justRecompileThePlayerList()
	{
		string text = string.Empty;
		if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in array)
			{
				if (photonPlayer.customProperties[PhotonPlayerProperty.dead] != null && !FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
				{
					switch (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.RCteam]))
					{
					case 0:
						dictionary3.Add(photonPlayer.ID, photonPlayer);
						break;
					case 1:
						dictionary.Add(photonPlayer.ID, photonPlayer);
						num += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
						num3 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
						num5 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
						num7 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					case 2:
						dictionary2.Add(photonPlayer.ID, photonPlayer);
						num2 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
						num4 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
						num6 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
						num8 += RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					}
				}
			}
			this.cyanKills = num;
			this.magentaKills = num2;
			if (PhotonNetwork.isMasterClient)
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value == 2)
				{
					array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer2 in array)
					{
						int num9 = 0;
						if (dictionary.Count > dictionary2.Count + 1)
						{
							num9 = 2;
							if (dictionary.ContainsKey(photonPlayer2.ID))
							{
								dictionary.Remove(photonPlayer2.ID);
							}
							if (!dictionary2.ContainsKey(photonPlayer2.ID))
							{
								dictionary2.Add(photonPlayer2.ID, photonPlayer2);
							}
						}
						else if (dictionary2.Count > dictionary.Count + 1)
						{
							num9 = 1;
							if (!dictionary.ContainsKey(photonPlayer2.ID))
							{
								dictionary.Add(photonPlayer2.ID, photonPlayer2);
							}
							if (dictionary2.ContainsKey(photonPlayer2.ID))
							{
								dictionary2.Remove(photonPlayer2.ID);
							}
						}
						if (num9 > 0)
						{
							base.photonView.RPC("setTeamRPC", photonPlayer2, num9);
						}
					}
				}
				else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
				{
					array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer3 in array)
					{
						int num10 = 0;
						int num11 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.RCteam]);
						if (num11 <= 0)
						{
							continue;
						}
						switch (num11)
						{
						case 1:
						{
							int num13 = 0;
							num13 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
							if (num2 + num13 + 7 < num - num13)
							{
								num10 = 2;
								num2 += num13;
								num -= num13;
							}
							break;
						}
						case 2:
						{
							int num12 = 0;
							num12 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
							if (num + num12 + 7 < num2 - num12)
							{
								num10 = 1;
								num += num12;
								num2 -= num12;
							}
							break;
						}
						}
						if (num10 > 0)
						{
							base.photonView.RPC("setTeamRPC", photonPlayer3, num10);
						}
					}
				}
			}
			text = text + "[00FFFF]TEAM CYAN" + "[ffffff]:" + this.cyanKills + "/" + num3 + "/" + num5 + "/" + num7 + "\n";
			foreach (PhotonPlayer value in dictionary.Values)
			{
				int num11 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.RCteam]);
				if (value.customProperties[PhotonPlayerProperty.dead] == null || num11 != 1)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(value.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value.ID) + "] ";
				if (value.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(value.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text2 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = text + " \n" + "[FF00FF]TEAM MAGENTA" + "[ffffff]:" + this.magentaKills + "/" + num4 + "/" + num6 + "/" + num8 + "\n";
			foreach (PhotonPlayer value2 in dictionary2.Values)
			{
				int num11 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.RCteam]);
				if (value2.customProperties[PhotonPlayerProperty.dead] == null || num11 != 2)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(value2.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value2.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value2.ID) + "] ";
				if (value2.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(value2.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text2 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = string.Concat(new object[3] { text, " \n", "[00FF00]INDIVIDUAL\n" });
			foreach (PhotonPlayer value3 in dictionary3.Values)
			{
				int num11 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.RCteam]);
				if (value3.customProperties[PhotonPlayerProperty.dead] == null || num11 != 0)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(value3.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value3.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value3.ID) + "] ";
				if (value3.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(value3.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text2 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		else
		{
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer4 in array)
			{
				if (photonPlayer4.customProperties[PhotonPlayerProperty.dead] == null)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(photonPlayer4.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!photonPlayer4.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(photonPlayer4.ID) + "] ";
				if (photonPlayer4.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num14 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.team]);
					if (num14 < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num14 == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text3 = text;
				string empty = string.Empty;
				empty = RCextensions.returnStringFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.name]);
				int num15 = 0;
				num15 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.kills]);
				int num16 = 0;
				num16 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.deaths]);
				int num17 = 0;
				num17 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.max_dmg]);
				int num18 = 0;
				num18 = RCextensions.returnIntFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text3 + string.Empty + empty + "[ffffff]:" + num15 + "/" + num16 + "/" + num17 + "/" + num18;
				if (RCextensions.returnBoolFromObject(photonPlayer4.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		this.playerList = text;
		if (!PhotonNetwork.isMasterClient || this.isWinning || this.isLosing || !(this.roundTime >= 5f))
		{
			return;
		}
		if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
		{
			int num19 = 0;
			for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
			{
				PhotonPlayer photonPlayer5 = PhotonNetwork.playerList[j];
				if (FengGameManagerMKII.ignoreList.Contains(photonPlayer5.ID) || photonPlayer5.customProperties[PhotonPlayerProperty.dead] == null || photonPlayer5.customProperties[PhotonPlayerProperty.isTitan] == null)
				{
					continue;
				}
				if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 1)
				{
					if (RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.deaths]) > 0)
					{
						if (!FengGameManagerMKII.imatitan.ContainsKey(photonPlayer5.ID))
						{
							FengGameManagerMKII.imatitan.Add(photonPlayer5.ID, 2);
						}
						ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
						hashtable.Add(PhotonPlayerProperty.isTitan, 2);
						photonPlayer5.SetCustomProperties(hashtable);
						base.photonView.RPC("spawnTitanRPC", photonPlayer5);
					}
					else
					{
						if (!FengGameManagerMKII.imatitan.ContainsKey(photonPlayer5.ID))
						{
							continue;
						}
						for (int k = 0; k < this.heroes.Count; k++)
						{
							HERO hERO = (HERO)this.heroes[k];
							if (hERO.photonView.owner == photonPlayer5)
							{
								hERO.markDie();
								hERO.photonView.RPC("netDie2", PhotonTargets.All, -1, "no switching in infection");
							}
						}
					}
				}
				else if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]))
				{
					num19++;
				}
			}
			if (num19 <= 0 && IN_GAME_MAIN_CAMERA.gamemode != 0)
			{
				this.gameWin2();
			}
		}
		else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
		{
			if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
			{
				if (this.cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
				{
					object[] parameters = new object[2]
					{
						"<color=#00FFFF>Team Cyan wins! </color>",
						string.Empty
					};
					base.photonView.RPC("Chat", PhotonTargets.All, parameters);
					this.gameWin2();
				}
				else if (this.magentaKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
				{
					object[] parameters2 = new object[2]
					{
						"<color=#FF00FF>Team Magenta wins! </color>",
						string.Empty
					};
					base.photonView.RPC("Chat", PhotonTargets.All, parameters2);
					this.gameWin2();
				}
			}
			else
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0)
				{
					return;
				}
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer6 = PhotonNetwork.playerList[j];
					if (RCextensions.returnIntFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
					{
						object[] parameters3 = new object[2]
						{
							"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters3);
						this.gameWin2();
					}
				}
			}
		}
		else
		{
			if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value || (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value && SettingsManager.LegacyGameSettings.BladePVP.Value <= 0))
			{
				return;
			}
			if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.playerList.Length > 1)
			{
				int num20 = 0;
				int num21 = 0;
				int num22 = 0;
				int num23 = 0;
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer7 = PhotonNetwork.playerList[j];
					if (FengGameManagerMKII.ignoreList.Contains(photonPlayer7.ID) || photonPlayer7.customProperties[PhotonPlayerProperty.RCteam] == null || photonPlayer7.customProperties[PhotonPlayerProperty.dead] == null)
					{
						continue;
					}
					if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 1)
					{
						num22++;
						if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
						{
							num20++;
						}
					}
					else if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 2)
					{
						num23++;
						if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
						{
							num21++;
						}
					}
				}
				if (num22 > 0 && num23 > 0)
				{
					if (num20 == 0)
					{
						object[] parameters4 = new object[2]
						{
							"<color=#FF00FF>Team Magenta wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters4);
						this.gameWin2();
					}
					else if (num21 == 0)
					{
						object[] parameters5 = new object[2]
						{
							"<color=#00FFFF>Team Cyan wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters5);
						this.gameWin2();
					}
				}
			}
			else
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value != 0 || PhotonNetwork.playerList.Length <= 1)
				{
					return;
				}
				int num24 = 0;
				string text4 = "Nobody";
				PhotonPlayer player = PhotonNetwork.playerList[0];
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer8 = PhotonNetwork.playerList[j];
					if (photonPlayer8.customProperties[PhotonPlayerProperty.dead] != null && !RCextensions.returnBoolFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.dead]))
					{
						text4 = RCextensions.returnStringFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.name]).hexColor();
						player = photonPlayer8;
						num24++;
					}
				}
				if (num24 > 1)
				{
					return;
				}
				string text5 = " 5 points added.";
				if (text4 == "Nobody")
				{
					text5 = string.Empty;
				}
				else
				{
					for (int j = 0; j < 5; j++)
					{
						this.playerKillInfoUpdate(player, 0);
					}
				}
				object[] parameters6 = new object[2]
				{
					"<color=#FFCC00>" + text4.hexColor() + " wins." + text5 + "</color>",
					string.Empty
				};
				base.photonView.RPC("Chat", PhotonTargets.All, parameters6);
				this.gameWin2();
			}
		}
	}

	private void kickPhotonPlayer(string name)
	{
		UnityEngine.MonoBehaviour.print("KICK " + name + "!!!");
		PhotonPlayer[] array = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in array)
		{
			if (photonPlayer.ID.ToString() == name && !photonPlayer.isMasterClient)
			{
				PhotonNetwork.CloseConnection(photonPlayer);
				break;
			}
		}
	}

	private void kickPlayer(string kickPlayer, string kicker)
	{
		bool flag = false;
		for (int i = 0; i < this.kicklist.Count; i++)
		{
			if (((KickState)this.kicklist[i]).name == kickPlayer)
			{
				KickState kickState = (KickState)this.kicklist[i];
				kickState.addKicker(kicker);
				this.tryKick(kickState);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			KickState kickState = new KickState();
			kickState.init(kickPlayer);
			kickState.addKicker(kicker);
			this.kicklist.Add(kickState);
			this.tryKick(kickState);
		}
	}

	public void kickPlayerRCIfMC(PhotonPlayer player, bool ban, string reason)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.kickPlayerRC(player, ban, reason);
		}
	}

	public void kickPlayerRC(PhotonPlayer player, bool ban, string reason)
	{
		if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
		{
			string empty = string.Empty;
			empty = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
			FengGameManagerMKII.ServerCloseConnection(player, ban, empty);
			return;
		}
		if (PhotonNetwork.isMasterClient && player == PhotonNetwork.player && reason != string.Empty)
		{
			this.chatRoom.addLINE("Attempting to ban myself for:" + reason + ", please report this to the devs.");
			return;
		}
		PhotonNetwork.DestroyPlayerObjects(player);
		PhotonNetwork.CloseConnection(player);
		base.photonView.RPC("ignorePlayer", PhotonTargets.Others, player.ID);
		if (!FengGameManagerMKII.ignoreList.Contains(player.ID))
		{
			FengGameManagerMKII.ignoreList.Add(player.ID);
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.TargetActors = new int[1] { player.ID };
			RaiseEventOptions options = raiseEventOptions;
			PhotonNetwork.RaiseEvent(254, null, sendReliable: true, options);
		}
		if (ban && !FengGameManagerMKII.banHash.ContainsKey(player.ID))
		{
			string empty = string.Empty;
			empty = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
			FengGameManagerMKII.banHash.Add(player.ID, empty);
		}
		if (reason != string.Empty)
		{
			this.chatRoom.addLINE("Player " + player.ID + " was autobanned. Reason:" + reason);
		}
		this.RecompilePlayerList(0.1f);
	}

	[RPC]
	private void labelRPC(int setting, PhotonMessageInfo info)
	{
		if (!(PhotonView.Find(setting) != null))
		{
			return;
		}
		PhotonPlayer owner = PhotonView.Find(setting).owner;
		if (owner != info.sender)
		{
			return;
		}
		string text = RCextensions.returnStringFromObject(owner.customProperties[PhotonPlayerProperty.guildName]);
		string text2 = RCextensions.returnStringFromObject(owner.customProperties[PhotonPlayerProperty.name]);
		GameObject gameObject = PhotonView.Find(setting).gameObject;
		if (!(gameObject != null))
		{
			return;
		}
		HERO component = gameObject.GetComponent<HERO>();
		if (component != null)
		{
			if (text != string.Empty)
			{
				component.myNetWorkName.GetComponent<UILabel>().text = "[FFFF00]" + text + "\n[FFFFFF]" + text2;
			}
			else
			{
				component.myNetWorkName.GetComponent<UILabel>().text = text2;
			}
		}
	}

	private void LateUpdate()
	{
		if (!this.gameStart)
		{
			return;
		}
		foreach (HERO hero in this.heroes)
		{
			hero.lateUpdate2();
		}
		foreach (TITAN_EREN item in this.eT)
		{
			item.lateUpdate();
		}
		foreach (TITAN titan in this.titans)
		{
			titan.lateUpdate2();
		}
		foreach (FEMALE_TITAN item2 in this.fT)
		{
			item2.lateUpdate2();
		}
		this.core2();
	}

	private void loadconfig()
	{
		object[] array = new object[500];
		array[31] = 0;
		array[64] = 0;
		array[68] = 100;
		array[69] = "default";
		array[70] = "1";
		array[71] = "1";
		array[72] = "1";
		array[73] = 1f;
		array[74] = 1f;
		array[75] = 1f;
		array[76] = 0;
		array[77] = string.Empty;
		array[78] = 0;
		array[79] = "1.0";
		array[80] = "1.0";
		array[81] = 0;
		array[83] = "30";
		array[84] = 0;
		array[91] = 0;
		array[100] = 0;
		array[185] = 0;
		array[186] = 0;
		array[187] = 0;
		array[188] = 0;
		array[190] = 0;
		array[191] = string.Empty;
		array[230] = 0;
		array[263] = 0;
		FengGameManagerMKII.linkHash = new ExitGames.Client.Photon.Hashtable[5]
		{
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable(),
			new ExitGames.Client.Photon.Hashtable()
		};
		FengGameManagerMKII.settingsOld = array;
		this.scroll = Vector2.zero;
		this.scroll2 = Vector2.zero;
		this.transparencySlider = 1f;
		SettingsManager.LegacyGeneralSettings.SetDefault();
		MaterialCache.Clear();
	}

	private void loadskin()
	{
		if ((int)FengGameManagerMKII.settingsOld[64] >= 100)
		{
			string[] array = new string[5] { "Flare", "LabelInfoBottomRight", "LabelNetworkStatus", "skill_cd_bottom", "GasUI" };
			GameObject[] array2 = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject gameObject in array2)
			{
				if (gameObject.name.Contains("TREE") || gameObject.name.Contains("aot_supply") || gameObject.name.Contains("gameobjectOutSide"))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			GameObject.Find("Cube_001").renderer.material.mainTexture = ((Material)FengGameManagerMKII.RCassets.Load("grass")).mainTexture;
			UnityEngine.Object.Instantiate(FengGameManagerMKII.RCassets.Load("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0f, 0f, 0f, 1f));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject2 = GameObject.Find(array[i]);
				if (gameObject2 != null)
				{
					UnityEngine.Object.Destroy(gameObject2);
				}
			}
			Camera.main.GetComponent<SpectatorMovement>().disable = true;
			return;
		}
		InstantiateTracker.instance.Dispose();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			this.updateTime = 1f;
			if (FengGameManagerMKII.oldScriptLogic != SettingsManager.LegacyGameSettings.LogicScript.Value)
			{
				FengGameManagerMKII.intVariables.Clear();
				FengGameManagerMKII.boolVariables.Clear();
				FengGameManagerMKII.stringVariables.Clear();
				FengGameManagerMKII.floatVariables.Clear();
				FengGameManagerMKII.globalVariables.Clear();
				FengGameManagerMKII.RCEvents.Clear();
				FengGameManagerMKII.RCVariableNames.Clear();
				FengGameManagerMKII.playerVariables.Clear();
				FengGameManagerMKII.titanVariables.Clear();
				FengGameManagerMKII.RCRegionTriggers.Clear();
				FengGameManagerMKII.oldScriptLogic = SettingsManager.LegacyGameSettings.LogicScript.Value;
				this.compileScript(SettingsManager.LegacyGameSettings.LogicScript.Value);
				if (FengGameManagerMKII.RCEvents.ContainsKey("OnFirstLoad"))
				{
					((RCEvent)FengGameManagerMKII.RCEvents["OnFirstLoad"]).checkEvent();
				}
			}
			if (FengGameManagerMKII.RCEvents.ContainsKey("OnRoundStart"))
			{
				((RCEvent)FengGameManagerMKII.RCEvents["OnRoundStart"]).checkEvent();
			}
			base.photonView.RPC("setMasterRC", PhotonTargets.All);
		}
		FengGameManagerMKII.logicLoaded = true;
		this.racingSpawnPoint = new Vector3(0f, 0f, 0f);
		this.racingSpawnPointSet = false;
		this.racingDoors = new List<GameObject>();
		this.allowedToCannon = new Dictionary<int, CannonValues>();
		bool flag = false;
		string[] array3 = new string[6]
		{
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty
		};
		if (SettingsManager.CustomSkinSettings.Skybox.SkinsEnabled.Value)
		{
			SkyboxCustomSkinSet skyboxCustomSkinSet = (SkyboxCustomSkinSet)SettingsManager.CustomSkinSettings.Skybox.GetSelectedSet();
			array3 = new string[6]
			{
				skyboxCustomSkinSet.Front.Value,
				skyboxCustomSkinSet.Back.Value,
				skyboxCustomSkinSet.Left.Value,
				skyboxCustomSkinSet.Right.Value,
				skyboxCustomSkinSet.Up.Value,
				skyboxCustomSkinSet.Down.Value
			};
			flag = true;
		}
		if (!FengGameManagerMKII.level.StartsWith("Custom") && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.isMasterClient))
		{
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("City") && SettingsManager.CustomSkinSettings.City.SkinsEnabled.Value)
			{
				CityCustomSkinSet cityCustomSkinSet = (CityCustomSkinSet)SettingsManager.CustomSkinSettings.City.GetSelectedSet();
				List<string> list = new List<string>();
				foreach (StringSetting ıtem in cityCustomSkinSet.Houses.GetItems())
				{
					list.Add(ıtem.Value);
				}
				text2 = string.Join(",", list.ToArray());
				for (int j = 0; j < 250; j++)
				{
					text += Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
				}
				text3 = string.Join(",", new string[3]
				{
					cityCustomSkinSet.Ground.Value,
					cityCustomSkinSet.Wall.Value,
					cityCustomSkinSet.Gate.Value
				});
				flag = true;
			}
			else if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("Forest") && SettingsManager.CustomSkinSettings.Forest.SkinsEnabled.Value)
			{
				ForestCustomSkinSet forestCustomSkinSet = (ForestCustomSkinSet)SettingsManager.CustomSkinSettings.Forest.GetSelectedSet();
				List<string> list2 = new List<string>();
				foreach (StringSetting ıtem2 in forestCustomSkinSet.TreeTrunks.GetItems())
				{
					list2.Add(ıtem2.Value);
				}
				text2 = string.Join(",", list2.ToArray());
				List<string> list3 = new List<string>();
				foreach (StringSetting ıtem3 in forestCustomSkinSet.TreeLeafs.GetItems())
				{
					list3.Add(ıtem3.Value);
				}
				list3.Add(forestCustomSkinSet.Ground.Value);
				text3 = string.Join(",", list3.ToArray());
				for (int k = 0; k < 150; k++)
				{
					string text4 = Convert.ToString((int)UnityEngine.Random.Range(0f, 8f));
					text += text4;
					text = (forestCustomSkinSet.RandomizedPairs.Value ? (text + Convert.ToString((int)UnityEngine.Random.Range(0f, 8f))) : (text + text4));
				}
				flag = true;
			}
			if (flag)
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					base.StartCoroutine(this.loadskinE(text, text2, text3, array3));
				}
				else if (PhotonNetwork.isMasterClient)
				{
					base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, text, text2, text3, array3);
				}
			}
		}
		else
		{
			if (!FengGameManagerMKII.level.StartsWith("Custom") || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				return;
			}
			GameObject[] array4 = GameObject.FindGameObjectsWithTag("playerRespawn");
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f));
			}
			GameObject[] array2 = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject gameObject in array2)
			{
				if (gameObject.name.Contains("TREE") || gameObject.name.Contains("aot_supply"))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				else if (gameObject.name == "Cube_001" && gameObject.transform.parent.gameObject.tag != "player" && gameObject.renderer != null)
				{
					this.groundList.Add(gameObject);
					gameObject.renderer.material.mainTexture = ((Material)FengGameManagerMKII.RCassets.Load("grass")).mainTexture;
				}
			}
			if (!PhotonNetwork.isMasterClient)
			{
				return;
			}
			string[] array5 = new string[7];
			for (int l = 0; l < 6; l++)
			{
				array5[l] = array3[l];
			}
			array5[6] = ((CustomLevelCustomSkinSet)SettingsManager.CustomSkinSettings.CustomLevel.GetSelectedSet()).Ground.Value;
			SettingsManager.LegacyGameSettings.TitanSpawnCap.Value = Math.Min(100, SettingsManager.LegacyGameSettings.TitanSpawnCap.Value);
			base.photonView.RPC("clearlevel", PhotonTargets.AllBuffered, array5, SettingsManager.LegacyGameSettings.GameType.Value);
			FengGameManagerMKII.RCRegions.Clear();
			if (FengGameManagerMKII.oldScript != SettingsManager.LegacyGameSettings.LevelScript.Value)
			{
				this.levelCache.Clear();
				this.titanSpawns.Clear();
				this.playerSpawnsC.Clear();
				this.playerSpawnsM.Clear();
				this.titanSpawners.Clear();
				FengGameManagerMKII.currentLevel = string.Empty;
				if (SettingsManager.LegacyGameSettings.LevelScript.Value == string.Empty)
				{
					ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add(PhotonPlayerProperty.currentLevel, FengGameManagerMKII.currentLevel);
					PhotonNetwork.player.SetCustomProperties(hashtable);
					FengGameManagerMKII.oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
				}
				else
				{
					string[] array6 = Regex.Replace(SettingsManager.LegacyGameSettings.LevelScript.Value, "\\s+", "").Replace("\r\n", "").Replace("\n", "")
						.Replace("\r", "")
						.Split(';');
					for (int i = 0; i < Mathf.FloorToInt((array6.Length - 1) / 100) + 1; i++)
					{
						string[] array7;
						int num;
						if (i < Mathf.FloorToInt(array6.Length / 100))
						{
							array7 = new string[101];
							num = 0;
							for (int m = 100 * i; m < 100 * i + 100; m++)
							{
								if (array6[m].StartsWith("spawnpoint"))
								{
									string[] array8 = array6[m].Split(',');
									if (array8[1] == "titan")
									{
										this.titanSpawns.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
									}
									else if (array8[1] == "playerC")
									{
										this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
									}
									else if (array8[1] == "playerM")
									{
										this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
									}
								}
								array7[num] = array6[m];
								num++;
							}
							FengGameManagerMKII.currentLevel += (array7[100] = UnityEngine.Random.Range(10000, 99999).ToString());
							this.levelCache.Add(array7);
							continue;
						}
						array7 = new string[array6.Length % 100 + 1];
						num = 0;
						for (int m = 100 * i; m < 100 * i + array6.Length % 100; m++)
						{
							if (array6[m].StartsWith("spawnpoint"))
							{
								string[] array8 = array6[m].Split(',');
								if (array8[1] == "titan")
								{
									this.titanSpawns.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
								}
								else if (array8[1] == "playerC")
								{
									this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
								}
								else if (array8[1] == "playerM")
								{
									this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(array8[2]), Convert.ToSingle(array8[3]), Convert.ToSingle(array8[4])));
								}
							}
							array7[num] = array6[m];
							num++;
						}
						string text5 = UnityEngine.Random.Range(10000, 99999).ToString();
						array7[array6.Length % 100] = text5;
						FengGameManagerMKII.currentLevel += text5;
						this.levelCache.Add(array7);
					}
					List<string> list4 = new List<string>();
					foreach (Vector3 titanSpawn in this.titanSpawns)
					{
						string[] obj = new string[6] { "titan,", null, null, null, null, null };
						float x = titanSpawn.x;
						obj[1] = x.ToString();
						obj[2] = ",";
						x = titanSpawn.y;
						obj[3] = x.ToString();
						obj[4] = ",";
						x = titanSpawn.z;
						obj[5] = x.ToString();
						list4.Add(string.Concat(obj));
					}
					foreach (Vector3 item in this.playerSpawnsC)
					{
						string[] obj2 = new string[6] { "playerC,", null, null, null, null, null };
						float x = item.x;
						obj2[1] = x.ToString();
						obj2[2] = ",";
						x = item.y;
						obj2[3] = x.ToString();
						obj2[4] = ",";
						x = item.z;
						obj2[5] = x.ToString();
						list4.Add(string.Concat(obj2));
					}
					foreach (Vector3 item2 in this.playerSpawnsM)
					{
						string[] obj3 = new string[6] { "playerM,", null, null, null, null, null };
						float x = item2.x;
						obj3[1] = x.ToString();
						obj3[2] = ",";
						x = item2.y;
						obj3[3] = x.ToString();
						obj3[4] = ",";
						x = item2.z;
						obj3[5] = x.ToString();
						list4.Add(string.Concat(obj3));
					}
					string text6 = "a" + UnityEngine.Random.Range(10000, 99999);
					list4.Add(text6);
					FengGameManagerMKII.currentLevel = text6 + FengGameManagerMKII.currentLevel;
					this.levelCache.Insert(0, list4.ToArray());
					string text7 = "z" + UnityEngine.Random.Range(10000, 99999);
					this.levelCache.Add(new string[1] { text7 });
					FengGameManagerMKII.currentLevel += text7;
					ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add(PhotonPlayerProperty.currentLevel, FengGameManagerMKII.currentLevel);
					PhotonNetwork.player.SetCustomProperties(hashtable);
					FengGameManagerMKII.oldScript = SettingsManager.LegacyGameSettings.LevelScript.Value;
				}
			}
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
				if (!photonPlayer.isMasterClient)
				{
					this.playersRPC.Add(photonPlayer);
				}
			}
			base.StartCoroutine(this.customlevelE(this.playersRPC));
			base.StartCoroutine(this.customlevelcache());
		}
	}

	private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
	{
		if (this.IsValidSkybox(skybox))
		{
			yield return base.StartCoroutine(this._skyboxCustomSkinLoader.LoadSkinsFromRPC(skybox));
		}
		else
		{
			SkyboxCustomSkinLoader.SkyboxMaterial = null;
		}
		if (n != string.Empty)
		{
			if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("Forest"))
			{
				yield return base.StartCoroutine(this._forestCustomSkinLoader.LoadSkinsFromRPC(new object[3] { n, url, url2 }));
			}
			else if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("City"))
			{
				yield return base.StartCoroutine(this._cityCustomSkinLoader.LoadSkinsFromRPC(new object[3] { n, url, url2 }));
			}
		}
		Minimap.TryRecaptureInstance();
		base.StartCoroutine(this.reloadSky());
		yield return null;
	}

	private bool IsValidSkybox(string[] skybox)
	{
		for (int i = 0; i < skybox.Length; i++)
		{
			if (TextureDownloader.ValidTextureURL(skybox[i]))
			{
				return true;
			}
		}
		return false;
	}

	[RPC]
	private void loadskinRPC(string n, string url1, string url2, string[] skybox, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient)
		{
			return;
		}
		if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("Forest"))
		{
			BaseCustomSkinSettings<ForestCustomSkinSet> forest = SettingsManager.CustomSkinSettings.Forest;
			if (forest.SkinsEnabled.Value && (!forest.SkinsLocal.Value || PhotonNetwork.isMasterClient))
			{
				base.StartCoroutine(this.loadskinE(n, url1, url2, skybox));
			}
		}
		else if (LevelInfo.getInfo(FengGameManagerMKII.level).mapName.Contains("City"))
		{
			BaseCustomSkinSettings<CityCustomSkinSet> city = SettingsManager.CustomSkinSettings.City;
			if (city.SkinsEnabled.Value && (!city.SkinsLocal.Value || PhotonNetwork.isMasterClient))
			{
				base.StartCoroutine(this.loadskinE(n, url1, url2, skybox));
			}
		}
	}

	private IEnumerator loginFeng()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("userid", FengGameManagerMKII.usernameField);
		wWWForm.AddField("password", FengGameManagerMKII.passwordField);
		WWW iteratorVariable1 = ((!Application.isWebPlayer) ? new WWW("http://fenglee.com/game/aog/require_user_info.php", wWWForm) : new WWW("http://aotskins.com/version/getinfo.php", wWWForm));
		yield return iteratorVariable1;
		if (iteratorVariable1.error == null && !iteratorVariable1.text.Contains("Error,please sign in again."))
		{
			char[] separator = new char[1] { '|' };
			string[] array = iteratorVariable1.text.Split(separator);
			LoginFengKAI.player.name = FengGameManagerMKII.usernameField;
			LoginFengKAI.player.guildname = array[0];
			FengGameManagerMKII.loginstate = 3;
		}
		else
		{
			FengGameManagerMKII.loginstate = 2;
		}
	}

	private string mastertexturetype(int lol)
	{
		return lol switch
		{
			0 => "High", 
			1 => "Med", 
			_ => "Low", 
		};
	}

	public void multiplayerRacingFinsih()
	{
		float num = this.roundTime - SettingsManager.LegacyGameSettings.RacingStartTime.Value;
		if (PhotonNetwork.isMasterClient)
		{
			this.getRacingResult(LoginFengKAI.player.name, num, null);
		}
		else
		{
			object[] parameters = new object[2]
			{
				LoginFengKAI.player.name,
				num
			};
			base.photonView.RPC("getRacingResult", PhotonTargets.MasterClient, parameters);
		}
		this.gameWin2();
	}

	[RPC]
	private void netGameLose(int score, PhotonMessageInfo info)
	{
		this.isLosing = true;
		this.titanScore = score;
		this.gameEndCD = this.gameEndTotalCDtime;
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
		}
		if (info.sender != PhotonNetwork.masterClient && !info.sender.isLocal && PhotonNetwork.isMasterClient)
		{
			this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID + "</color>");
		}
	}

	[RPC]
	private void netGameWin(int score, PhotonMessageInfo info)
	{
		this.humanScore = score;
		this.isWinning = true;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			this.teamWinner = score;
			this.teamScores[this.teamWinner - 1]++;
			this.gameEndCD = this.gameEndTotalCDtime;
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.RACING)
		{
			if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
			{
				this.gameEndCD = 1000f;
			}
			else
			{
				this.gameEndCD = 20f;
			}
		}
		else
		{
			this.gameEndCD = this.gameEndTotalCDtime;
		}
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
		}
		if (info.sender != PhotonNetwork.masterClient && !info.sender.isLocal)
		{
			this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID + "</color>");
		}
	}

	[RPC]
	private void netRefreshRacingResult(string tmp)
	{
		this.localRacingResult = tmp;
	}

	[RPC]
	public void netShowDamage(int speed)
	{
		GameObject.Find("Stylish").GetComponent<StylishComponent>().Style(speed);
		GameObject gameObject = GameObject.Find("LabelScore");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = speed.ToString();
			gameObject.transform.localScale = Vector3.zero;
			speed = (int)((float)speed * 0.1f);
			speed = Mathf.Max(40, speed);
			speed = Mathf.Min(150, speed);
			iTween.Stop(gameObject);
			object[] args = new object[10]
			{
				"x",
				speed,
				"y",
				speed,
				"z",
				speed,
				"easetype",
				iTween.EaseType.easeOutElastic,
				"time",
				1f
			};
			iTween.ScaleTo(gameObject, iTween.Hash(args));
			object[] args2 = new object[12]
			{
				"x",
				0,
				"y",
				0,
				"z",
				0,
				"easetype",
				iTween.EaseType.easeInBounce,
				"time",
				0.5f,
				"delay",
				2f
			};
			iTween.ScaleTo(gameObject, iTween.Hash(args2));
		}
	}

	public void NOTSpawnNonAITitan(string id)
	{
		this.myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			2
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		this.ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void NOTSpawnNonAITitanRC(string id)
	{
		this.myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			2
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		this.ShowHUDInfoCenter("Syncing spawn locations...");
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void NOTSpawnPlayer(string id)
	{
		this.myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			1
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		this.ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void NOTSpawnPlayerRC(string id)
	{
		this.myLastHero = id.ToUpper();
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", true } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			1
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
	}

	public void OnConnectedToMaster()
	{
		UnityEngine.MonoBehaviour.print("OnConnectedToMaster");
	}

	public void OnConnectedToPhoton()
	{
		UnityEngine.MonoBehaviour.print("OnConnectedToPhoton");
	}

	public void OnConnectionFail(DisconnectCause cause)
	{
		UnityEngine.MonoBehaviour.print("OnConnectionFail : " + cause);
		IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
		this.gameStart = false;
		NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], state: false);
		NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], state: false);
		NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], state: false);
		NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], state: false);
		NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[4], state: true);
		GameObject.Find("LabelDisconnectInfo").GetComponent<UILabel>().text = "OnConnectionFail : " + cause;
	}

	public void OnCreatedRoom()
	{
		this.kicklist = new ArrayList();
		this.racingResult = new ArrayList();
		this.teamScores = new int[2];
		UnityEngine.MonoBehaviour.print("OnCreatedRoom");
	}

	public void OnCustomAuthenticationFailed()
	{
		UnityEngine.MonoBehaviour.print("OnCustomAuthenticationFailed");
	}

	public void OnDisconnectedFromPhoton()
	{
		UnityEngine.MonoBehaviour.print("OnDisconnectedFromPhoton");
	}

	[RPC]
	public void oneTitanDown(string name1, bool onPlayerLeave)
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !PhotonNetwork.isMasterClient)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (name1 != string.Empty)
			{
				switch (name1)
				{
				case "Titan":
					this.PVPhumanScore++;
					break;
				case "Aberrant":
					this.PVPhumanScore += 2;
					break;
				case "Jumper":
					this.PVPhumanScore += 3;
					break;
				case "Crawler":
					this.PVPhumanScore += 4;
					break;
				case "Female Titan":
					this.PVPhumanScore += 10;
					break;
				default:
					this.PVPhumanScore += 3;
					break;
				}
			}
			this.checkPVPpts();
			object[] parameters = new object[2] { this.PVPhumanScore, this.PVPtitanScore };
			base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
		}
		else
		{
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.CAGE_FIGHT)
			{
				return;
			}
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN)
			{
				if (this.checkIsTitanAllDie())
				{
					this.gameWin2();
					Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
			{
				if (!this.checkIsTitanAllDie())
				{
					return;
				}
				this.wave++;
				if ((LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.NEWROUND || (FengGameManagerMKII.level.StartsWith("Custom") && SettingsManager.LegacyGameSettings.GameType.Value == 1)) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					PhotonPlayer[] array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer in array)
					{
						if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) != 2)
						{
							base.photonView.RPC("respawnHeroInNewRound", photonPlayer);
						}
					}
				}
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					this.sendChatContentInfo("<color=#A8FF24>Wave : " + this.wave + "</color>");
				}
				if (this.wave > this.highestwave)
				{
					this.highestwave = this.wave;
				}
				if (PhotonNetwork.isMasterClient)
				{
					this.RequireStatus();
				}
				if ((!SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value && this.wave > 20) || (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value && this.wave > SettingsManager.LegacyGameSettings.TitanMaxWaves.Value))
				{
					this.gameWin2();
					return;
				}
				int abnormal = 90;
				if (this.difficulty == 1)
				{
					abnormal = 70;
				}
				if (!LevelInfo.getInfo(FengGameManagerMKII.level).punk)
				{
					this.spawnTitanCustom("titanRespawn", abnormal, this.wave + 2, punk: false);
				}
				else if (this.wave == 5)
				{
					this.spawnTitanCustom("titanRespawn", abnormal, 1, punk: true);
				}
				else if (this.wave == 10)
				{
					this.spawnTitanCustom("titanRespawn", abnormal, 2, punk: true);
				}
				else if (this.wave == 15)
				{
					this.spawnTitanCustom("titanRespawn", abnormal, 3, punk: true);
				}
				else if (this.wave == 20)
				{
					this.spawnTitanCustom("titanRespawn", abnormal, 4, punk: true);
				}
				else
				{
					this.spawnTitanCustom("titanRespawn", abnormal, this.wave + 2, punk: false);
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
			{
				if (!onPlayerLeave)
				{
					this.humanScore++;
					int abnormal2 = 90;
					if (this.difficulty == 1)
					{
						abnormal2 = 70;
					}
					this.spawnTitanCustom("titanRespawn", abnormal2, 1, punk: false);
				}
			}
			else
			{
				_ = LevelInfo.getInfo(FengGameManagerMKII.level).enemyNumber;
				_ = -1;
			}
		}
	}

	public void OnFailedToConnectToPhoton()
	{
		UnityEngine.MonoBehaviour.print("OnFailedToConnectToPhoton");
	}

	private void DrawBackgroundIfLoading()
	{
		if (AssetBundleManager.Status == AssetBundleStatus.Loading || AutoUpdateManager.Status == AutoUpdateStatus.Updating)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), this.textureBackgroundBlue);
		}
	}

	public void OnGUI()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP && Application.loadedLevelName != "characterCreation")
		{
			LegacyPopupTemplate legacyPopupTemplate = new LegacyPopupTemplate(new Color(0f, 0f, 0f, 1f), this.textureBackgroundBlack, new Color(1f, 1f, 1f, 1f), (float)Screen.width / 2f, (float)Screen.height / 2f, 230f, 140f, 2f);
			this.DrawBackgroundIfLoading();
			if (AutoUpdateManager.Status == AutoUpdateStatus.Updating)
			{
				legacyPopupTemplate.DrawPopup("Auto-updating mod...", 130f, 22f);
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.NeedRestart && !AutoUpdateManager.CloseFailureBox)
			{
				bool[] array = legacyPopupTemplate.DrawPopupWithTwoButtons("Mod has been updated and requires a restart.", 190f, 44f, "Restart Now", 90f, "Ignore", 90f, 25f);
				if (array[0])
				{
					if (Application.platform == RuntimePlatform.WindowsPlayer)
					{
						Process.Start(Application.dataPath.Replace("_Data", ".exe"));
					}
					else if (Application.platform == RuntimePlatform.OSXPlayer)
					{
						Process.Start(Application.dataPath + "/MacOS/MacTest");
					}
					Application.Quit();
				}
				else if (array[1])
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.LauncherOutdated && !AutoUpdateManager.CloseFailureBox)
			{
				if (legacyPopupTemplate.DrawPopupWithButton("Game launcher is outdated, visit aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.FailedUpdate && !AutoUpdateManager.CloseFailureBox)
			{
				if (legacyPopupTemplate.DrawPopupWithButton("Auto-update failed, check internet connection or aotrc.weebly.com for a new game version.", 190f, 66f, "Continue", 80f, 25f))
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AutoUpdateManager.Status == AutoUpdateStatus.MacTranslocated && !AutoUpdateManager.CloseFailureBox)
			{
				if (legacyPopupTemplate.DrawPopupWithButton("Your game is not in the Applications folder, cannot auto-update and some bugs may occur.", 190f, 66f, "Continue", 80f, 25f))
				{
					AutoUpdateManager.CloseFailureBox = true;
				}
			}
			else if (AssetBundleManager.Status == AssetBundleStatus.Loading)
			{
				legacyPopupTemplate.DrawPopup("Downloading asset bundle...", 170f, 22f);
			}
			else if (AssetBundleManager.Status == AssetBundleStatus.Failed && !AssetBundleManager.CloseFailureBox && legacyPopupTemplate.DrawPopupWithButton("Failed to load asset bundle, check your internet connection.", 190f, 44f, "Continue", 80f, 25f))
			{
				AssetBundleManager.CloseFailureBox = true;
			}
		}
		else
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP)
			{
				return;
			}
			if ((int)FengGameManagerMKII.settingsOld[64] >= 100)
			{
				float num = (float)Screen.width - 300f;
				GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
				GUI.DrawTexture(new Rect(7f, 7f, 291f, 586f), this.textureBackgroundBlue);
				GUI.DrawTexture(new Rect(num + 2f, 7f, 291f, 586f), this.textureBackgroundBlue);
				bool flag = false;
				bool flag2 = false;
				GUI.Box(new Rect(5f, 5f, 295f, 590f), string.Empty);
				GUI.Box(new Rect(num, 5f, 295f, 590f), string.Empty);
				if (GUI.Button(new Rect(10f, 10f, 60f, 25f), "Script", "box"))
				{
					FengGameManagerMKII.settingsOld[68] = 100;
				}
				if (GUI.Button(new Rect(75f, 10f, 80f, 25f), "Full Screen", "box"))
				{
					FullscreenHandler.ToggleFullscreen();
				}
				if ((int)FengGameManagerMKII.settingsOld[68] == 100 || (int)FengGameManagerMKII.settingsOld[68] == 102)
				{
					GUI.Label(new Rect(115f, 40f, 100f, 20f), "Level Script:", "Label");
					GUI.Label(new Rect(115f, 115f, 100f, 20f), "Import Data", "Label");
					GUI.Label(new Rect(12f, 535f, 280f, 60f), "Warning: your current level will be lost if you quit or import data. Make sure to save the level to a text document.", "Label");
					FengGameManagerMKII.settingsOld[77] = GUI.TextField(new Rect(10f, 140f, 285f, 350f), (string)FengGameManagerMKII.settingsOld[77]);
					if (GUI.Button(new Rect(35f, 500f, 60f, 30f), "Apply"))
					{
						UnityEngine.Object[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
						for (int i = 0; i < array2.Length; i++)
						{
							GameObject gameObject = (GameObject)array2[i];
							if (gameObject.name.StartsWith("custom") || gameObject.name.StartsWith("base") || gameObject.name.StartsWith("photon") || gameObject.name.StartsWith("spawnpoint") || gameObject.name.StartsWith("misc") || gameObject.name.StartsWith("racing"))
							{
								UnityEngine.Object.Destroy(gameObject);
							}
						}
						FengGameManagerMKII.linkHash[3].Clear();
						FengGameManagerMKII.settingsOld[186] = 0;
						string[] array3 = Regex.Replace((string)FengGameManagerMKII.settingsOld[77], "\\s+", "").Replace("\r\n", "").Replace("\n", "")
							.Replace("\r", "")
							.Split(';');
						for (int j = 0; j < array3.Length; j++)
						{
							string[] array4 = array3[j].Split(',');
							if (array4[0].StartsWith("custom") || array4[0].StartsWith("base") || array4[0].StartsWith("photon") || array4[0].StartsWith("spawnpoint") || array4[0].StartsWith("misc") || array4[0].StartsWith("racing"))
							{
								GameObject gameObject2 = null;
								if (array4[0].StartsWith("custom"))
								{
									gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[12]), Convert.ToSingle(array4[13]), Convert.ToSingle(array4[14])), new Quaternion(Convert.ToSingle(array4[15]), Convert.ToSingle(array4[16]), Convert.ToSingle(array4[17]), Convert.ToSingle(array4[18])));
								}
								else if (array4[0].StartsWith("photon"))
								{
									gameObject2 = ((!array4[1].StartsWith("Cannon")) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[4]), Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6])), new Quaternion(Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10])))) : ((array4.Length >= 15) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array4[1] + "Prop"), new Vector3(Convert.ToSingle(array4[12]), Convert.ToSingle(array4[13]), Convert.ToSingle(array4[14])), new Quaternion(Convert.ToSingle(array4[15]), Convert.ToSingle(array4[16]), Convert.ToSingle(array4[17]), Convert.ToSingle(array4[18])))) : ((GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array4[1] + "Prop"), new Vector3(Convert.ToSingle(array4[2]), Convert.ToSingle(array4[3]), Convert.ToSingle(array4[4])), new Quaternion(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]))))));
								}
								else if (array4[0].StartsWith("spawnpoint"))
								{
									gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[2]), Convert.ToSingle(array4[3]), Convert.ToSingle(array4[4])), new Quaternion(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8])));
								}
								else if (array4[0].StartsWith("base"))
								{
									gameObject2 = ((array4.Length >= 15) ? ((GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array4[1]), new Vector3(Convert.ToSingle(array4[12]), Convert.ToSingle(array4[13]), Convert.ToSingle(array4[14])), new Quaternion(Convert.ToSingle(array4[15]), Convert.ToSingle(array4[16]), Convert.ToSingle(array4[17]), Convert.ToSingle(array4[18])))) : ((GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(array4[1]), new Vector3(Convert.ToSingle(array4[2]), Convert.ToSingle(array4[3]), Convert.ToSingle(array4[4])), new Quaternion(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8])))));
								}
								else if (array4[0].StartsWith("misc"))
								{
									if (array4[1].StartsWith("barrier"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load("barrierEditor"), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
									}
									else if (array4[1].StartsWith("region"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load("regionEditor"));
										gameObject2.transform.position = new Vector3(Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]));
										GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
										gameObject3.name = "RegionLabel";
										gameObject3.transform.parent = gameObject2.transform;
										float y = 1f;
										if (Convert.ToSingle(array4[4]) > 100f)
										{
											y = 0.8f;
										}
										else if (Convert.ToSingle(array4[4]) > 1000f)
										{
											y = 0.5f;
										}
										gameObject3.transform.localPosition = new Vector3(0f, y, 0f);
										gameObject3.transform.localScale = new Vector3(5f / Convert.ToSingle(array4[3]), 5f / Convert.ToSingle(array4[4]), 5f / Convert.ToSingle(array4[5]));
										gameObject3.GetComponent<UILabel>().text = array4[2];
										gameObject2.AddComponent<RCRegionLabel>();
										gameObject2.GetComponent<RCRegionLabel>().myLabel = gameObject3;
									}
									else if (array4[1].StartsWith("racingStart"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load("racingStart"), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
									}
									else if (array4[1].StartsWith("racingEnd"))
									{
										gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load("racingEnd"), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
									}
								}
								else if (array4[0].StartsWith("racing"))
								{
									gameObject2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)FengGameManagerMKII.RCassets.Load(array4[1]), new Vector3(Convert.ToSingle(array4[5]), Convert.ToSingle(array4[6]), Convert.ToSingle(array4[7])), new Quaternion(Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), Convert.ToSingle(array4[10]), Convert.ToSingle(array4[11])));
								}
								if (array4[2] != "default" && (array4[0].StartsWith("custom") || (array4[0].StartsWith("base") && array4.Length > 15) || (array4[0].StartsWith("photon") && array4.Length > 15)))
								{
									Renderer[] componentsInChildren = gameObject2.GetComponentsInChildren<Renderer>();
									foreach (Renderer renderer in componentsInChildren)
									{
										if (!renderer.name.Contains("Particle System") || !gameObject2.name.Contains("aot_supply"))
										{
											renderer.material = (Material)FengGameManagerMKII.RCassets.Load(array4[2]);
											renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array4[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array4[11]));
										}
									}
								}
								if (array4[0].StartsWith("custom") || (array4[0].StartsWith("base") && array4.Length > 15) || (array4[0].StartsWith("photon") && array4.Length > 15))
								{
									float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[3]);
									num2 -= 0.001f;
									float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[4]);
									float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[5]);
									gameObject2.transform.localScale = new Vector3(num2, y2, z);
									if (array4[6] != "0")
									{
										Color color = new Color(Convert.ToSingle(array4[7]), Convert.ToSingle(array4[8]), Convert.ToSingle(array4[9]), 1f);
										MeshFilter[] componentsInChildren2 = gameObject2.GetComponentsInChildren<MeshFilter>();
										for (int i = 0; i < componentsInChildren2.Length; i++)
										{
											Mesh mesh = componentsInChildren2[i].mesh;
											Color[] array5 = new Color[mesh.vertexCount];
											for (int k = 0; k < mesh.vertexCount; k++)
											{
												array5[k] = color;
											}
											mesh.colors = array5;
										}
									}
									gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4] + "," + array4[5] + "," + array4[6] + "," + array4[7] + "," + array4[8] + "," + array4[9] + "," + array4[10] + "," + array4[11];
								}
								else if (array4[0].StartsWith("misc"))
								{
									if (array4[1].StartsWith("barrier") || array4[1].StartsWith("racing"))
									{
										float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[2]);
										num2 -= 0.001f;
										float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[3]);
										float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[4]);
										gameObject2.transform.localScale = new Vector3(num2, y2, z);
										gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4];
									}
									else if (array4[1].StartsWith("region"))
									{
										float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[3]);
										num2 -= 0.001f;
										float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[4]);
										float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[5]);
										gameObject2.transform.localScale = new Vector3(num2, y2, z);
										gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4] + "," + array4[5];
									}
								}
								else if (array4[0].StartsWith("racing"))
								{
									float num2 = gameObject2.transform.localScale.x * Convert.ToSingle(array4[2]);
									num2 -= 0.001f;
									float y2 = gameObject2.transform.localScale.y * Convert.ToSingle(array4[3]);
									float z = gameObject2.transform.localScale.z * Convert.ToSingle(array4[4]);
									gameObject2.transform.localScale = new Vector3(num2, y2, z);
									gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3] + "," + array4[4];
								}
								else if (array4[0].StartsWith("photon") && !array4[1].StartsWith("Cannon"))
								{
									gameObject2.name = array4[0] + "," + array4[1] + "," + array4[2] + "," + array4[3];
								}
								else
								{
									gameObject2.name = array4[0] + "," + array4[1];
								}
								FengGameManagerMKII.linkHash[3].Add(gameObject2.GetInstanceID(), array3[j]);
							}
							else if (array4[0].StartsWith("map") && array4[1].StartsWith("disablebounds"))
							{
								FengGameManagerMKII.settingsOld[186] = 1;
								if (!FengGameManagerMKII.linkHash[3].ContainsKey("mapbounds"))
								{
									FengGameManagerMKII.linkHash[3].Add("mapbounds", "map,disablebounds");
								}
							}
						}
						this.unloadAssets();
						FengGameManagerMKII.settingsOld[77] = string.Empty;
					}
					else if (GUI.Button(new Rect(205f, 500f, 60f, 30f), "Exit"))
					{
						IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
						UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
						Application.LoadLevel("menu");
					}
					else if (GUI.Button(new Rect(15f, 70f, 115f, 30f), "Copy to Clipboard"))
					{
						string text = string.Empty;
						int num3 = 0;
						foreach (string value in FengGameManagerMKII.linkHash[3].Values)
						{
							num3++;
							text = text + value + ";\n";
						}
						TextEditor textEditor = new TextEditor();
						textEditor.content = new GUIContent(text);
						textEditor.SelectAll();
						textEditor.Copy();
					}
					else if (GUI.Button(new Rect(175f, 70f, 115f, 30f), "View Script"))
					{
						FengGameManagerMKII.settingsOld[68] = 102;
					}
					if ((int)FengGameManagerMKII.settingsOld[68] == 102)
					{
						string text = string.Empty;
						int num3 = 0;
						foreach (string value2 in FengGameManagerMKII.linkHash[3].Values)
						{
							num3++;
							text = text + value2 + ";\n";
						}
						float num4 = (float)(Screen.width / 2) - 110.5f;
						float num5 = (float)(Screen.height / 2) - 250f;
						GUI.DrawTexture(new Rect(num4 + 2f, num5 + 2f, 217f, 496f), this.textureBackgroundBlue);
						GUI.Box(new Rect(num4, num5, 221f, 500f), string.Empty);
						if (GUI.Button(new Rect(num4 + 10f, num5 + 460f, 60f, 30f), "Copy"))
						{
							TextEditor textEditor2 = new TextEditor();
							textEditor2.content = new GUIContent(text);
							textEditor2.SelectAll();
							textEditor2.Copy();
						}
						else if (GUI.Button(new Rect(num4 + 151f, num5 + 460f, 60f, 30f), "Done"))
						{
							FengGameManagerMKII.settingsOld[68] = 100;
						}
						GUI.TextArea(new Rect(num4 + 5f, num5 + 5f, 211f, 415f), text);
						GUI.Label(new Rect(num4 + 10f, num5 + 430f, 150f, 20f), "Object Count: " + Convert.ToString(num3), "Label");
					}
				}
				if ((int)FengGameManagerMKII.settingsOld[64] != 105 && (int)FengGameManagerMKII.settingsOld[64] != 106)
				{
					GUI.Label(new Rect(num + 13f, 445f, 125f, 20f), "Scale Multipliers:", "Label");
					GUI.Label(new Rect(num + 13f, 470f, 50f, 22f), "Length:", "Label");
					FengGameManagerMKII.settingsOld[72] = GUI.TextField(new Rect(num + 58f, 470f, 40f, 20f), (string)FengGameManagerMKII.settingsOld[72]);
					GUI.Label(new Rect(num + 13f, 495f, 50f, 20f), "Width:", "Label");
					FengGameManagerMKII.settingsOld[70] = GUI.TextField(new Rect(num + 58f, 495f, 40f, 20f), (string)FengGameManagerMKII.settingsOld[70]);
					GUI.Label(new Rect(num + 13f, 520f, 50f, 22f), "Height:", "Label");
					FengGameManagerMKII.settingsOld[71] = GUI.TextField(new Rect(num + 58f, 520f, 40f, 20f), (string)FengGameManagerMKII.settingsOld[71]);
					if ((int)FengGameManagerMKII.settingsOld[64] <= 106)
					{
						GUI.Label(new Rect(num + 155f, 554f, 50f, 22f), "Tiling:", "Label");
						FengGameManagerMKII.settingsOld[79] = GUI.TextField(new Rect(num + 200f, 554f, 40f, 20f), (string)FengGameManagerMKII.settingsOld[79]);
						FengGameManagerMKII.settingsOld[80] = GUI.TextField(new Rect(num + 245f, 554f, 40f, 20f), (string)FengGameManagerMKII.settingsOld[80]);
						GUI.Label(new Rect(num + 219f, 570f, 10f, 22f), "x:", "Label");
						GUI.Label(new Rect(num + 264f, 570f, 10f, 22f), "y:", "Label");
						GUI.Label(new Rect(num + 155f, 445f, 50f, 20f), "Color:", "Label");
						GUI.Label(new Rect(num + 155f, 470f, 10f, 20f), "R:", "Label");
						GUI.Label(new Rect(num + 155f, 495f, 10f, 20f), "G:", "Label");
						GUI.Label(new Rect(num + 155f, 520f, 10f, 20f), "B:", "Label");
						FengGameManagerMKII.settingsOld[73] = GUI.HorizontalSlider(new Rect(num + 170f, 475f, 100f, 20f), (float)FengGameManagerMKII.settingsOld[73], 0f, 1f);
						FengGameManagerMKII.settingsOld[74] = GUI.HorizontalSlider(new Rect(num + 170f, 500f, 100f, 20f), (float)FengGameManagerMKII.settingsOld[74], 0f, 1f);
						FengGameManagerMKII.settingsOld[75] = GUI.HorizontalSlider(new Rect(num + 170f, 525f, 100f, 20f), (float)FengGameManagerMKII.settingsOld[75], 0f, 1f);
						GUI.Label(new Rect(num + 13f, 554f, 57f, 22f), "Material:", "Label");
						if (GUI.Button(new Rect(num + 66f, 554f, 60f, 20f), (string)FengGameManagerMKII.settingsOld[69]))
						{
							FengGameManagerMKII.settingsOld[78] = 1;
						}
						if ((int)FengGameManagerMKII.settingsOld[78] == 1)
						{
							string[] item = new string[4] { "bark", "bark2", "bark3", "bark4" };
							string[] item2 = new string[4] { "wood1", "wood2", "wood3", "wood4" };
							string[] item3 = new string[4] { "grass", "grass2", "grass3", "grass4" };
							string[] item4 = new string[4] { "brick1", "brick2", "brick3", "brick4" };
							string[] item5 = new string[4] { "metal1", "metal2", "metal3", "metal4" };
							string[] item6 = new string[3] { "rock1", "rock2", "rock3" };
							string[] item7 = new string[10] { "stone1", "stone2", "stone3", "stone4", "stone5", "stone6", "stone7", "stone8", "stone9", "stone10" };
							string[] item8 = new string[7] { "earth1", "earth2", "ice1", "lava1", "crystal1", "crystal2", "empty" };
							_ = new string[0];
							List<string[]> list = new List<string[]> { item, item2, item3, item4, item5, item6, item7, item8 };
							string[] array6 = new string[9] { "bark", "wood", "grass", "brick", "metal", "rock", "stone", "misc", "transparent" };
							int num6 = 78;
							int num7 = 69;
							float num4 = (float)(Screen.width / 2) - 110.5f;
							float num5 = (float)(Screen.height / 2) - 220f;
							int num8 = (int)FengGameManagerMKII.settingsOld[185];
							float val = 10f + 104f * (float)(list[num8].Length / 3 + 1);
							val = Math.Max(val, 280f);
							GUI.DrawTexture(new Rect(num4 + 2f, num5 + 2f, 208f, 446f), this.textureBackgroundBlue);
							GUI.Box(new Rect(num4, num5, 212f, 450f), string.Empty);
							for (int j = 0; j < list.Count; j++)
							{
								int num9 = j / 3;
								int num10 = j % 3;
								if (GUI.Button(new Rect(num4 + 5f + 69f * (float)num10, num5 + 5f + (float)(30 * num9), 64f, 25f), array6[j], "box"))
								{
									FengGameManagerMKII.settingsOld[185] = j;
								}
							}
							this.scroll2 = GUI.BeginScrollView(new Rect(num4, num5 + 110f, 225f, 290f), this.scroll2, new Rect(num4, num5 + 110f, 212f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
							if (num8 != 8)
							{
								for (int j = 0; j < list[num8].Length; j++)
								{
									int num9 = j / 3;
									int num10 = j % 3;
									GUI.DrawTexture(new Rect(num4 + 5f + 69f * (float)num10, num5 + 115f + 104f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + list[num8][j]));
									if (GUI.Button(new Rect(num4 + 5f + 69f * (float)num10, num5 + 184f + 104f * (float)num9, 64f, 30f), list[num8][j]))
									{
										FengGameManagerMKII.settingsOld[num7] = list[num8][j];
										FengGameManagerMKII.settingsOld[num6] = 0;
									}
								}
							}
							GUI.EndScrollView();
							if (GUI.Button(new Rect(num4 + 24f, num5 + 410f, 70f, 30f), "Default"))
							{
								FengGameManagerMKII.settingsOld[num7] = "default";
								FengGameManagerMKII.settingsOld[num6] = 0;
							}
							else if (GUI.Button(new Rect(num4 + 118f, num5 + 410f, 70f, 30f), "Done"))
							{
								FengGameManagerMKII.settingsOld[num6] = 0;
							}
						}
						bool flag3 = false;
						if ((int)FengGameManagerMKII.settingsOld[76] == 1)
						{
							flag3 = true;
							Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
							texture2D.SetPixel(0, 0, new Color((float)FengGameManagerMKII.settingsOld[73], (float)FengGameManagerMKII.settingsOld[74], (float)FengGameManagerMKII.settingsOld[75], 1f));
							texture2D.Apply();
							GUI.DrawTexture(new Rect(num + 235f, 445f, 30f, 20f), texture2D, ScaleMode.StretchToFill);
							UnityEngine.Object.Destroy(texture2D);
						}
						bool flag4 = GUI.Toggle(new Rect(num + 193f, 445f, 40f, 20f), flag3, "On");
						if (flag3 != flag4)
						{
							if (flag4)
							{
								FengGameManagerMKII.settingsOld[76] = 1;
							}
							else
							{
								FengGameManagerMKII.settingsOld[76] = 0;
							}
						}
					}
				}
				if (GUI.Button(new Rect(num + 5f, 10f, 60f, 25f), "General", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 101;
				}
				else if (GUI.Button(new Rect(num + 70f, 10f, 70f, 25f), "Geometry", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 102;
				}
				else if (GUI.Button(new Rect(num + 145f, 10f, 65f, 25f), "Buildings", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 103;
				}
				else if (GUI.Button(new Rect(num + 215f, 10f, 50f, 25f), "Nature", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 104;
				}
				else if (GUI.Button(new Rect(num + 5f, 45f, 70f, 25f), "Spawners", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 105;
				}
				else if (GUI.Button(new Rect(num + 80f, 45f, 70f, 25f), "Racing", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 108;
				}
				else if (GUI.Button(new Rect(num + 155f, 45f, 40f, 25f), "Misc", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 107;
				}
				else if (GUI.Button(new Rect(num + 200f, 45f, 70f, 25f), "Credits", "box"))
				{
					FengGameManagerMKII.settingsOld[64] = 106;
				}
				float result;
				if ((int)FengGameManagerMKII.settingsOld[64] == 101)
				{
					this.scroll = GUI.BeginScrollView(new Rect(num, 80f, 305f, 350f), this.scroll, new Rect(num, 80f, 300f, 470f), alwaysShowHorizontal: true, alwaysShowVertical: true);
					GUI.Label(new Rect(num + 100f, 80f, 120f, 20f), "General Objects:", "Label");
					GUI.Label(new Rect(num + 108f, 245f, 120f, 20f), "Spawn Points:", "Label");
					GUI.Label(new Rect(num + 7f, 415f, 290f, 60f), "* The above titan spawn points apply only to randomly spawned titans specified by the Random Titan #.", "Label");
					GUI.Label(new Rect(num + 7f, 470f, 290f, 60f), "* If team mode is disabled both cyan and magenta spawn points will be randomly chosen for players.", "Label");
					GUI.DrawTexture(new Rect(num + 27f, 110f, 64f, 64f), this.RCLoadTexture("psupply"));
					GUI.DrawTexture(new Rect(num + 118f, 110f, 64f, 64f), this.RCLoadTexture("pcannonwall"));
					GUI.DrawTexture(new Rect(num + 209f, 110f, 64f, 64f), this.RCLoadTexture("pcannonground"));
					GUI.DrawTexture(new Rect(num + 27f, 275f, 64f, 64f), this.RCLoadTexture("pspawnt"));
					GUI.DrawTexture(new Rect(num + 118f, 275f, 64f, 64f), this.RCLoadTexture("pspawnplayerC"));
					GUI.DrawTexture(new Rect(num + 209f, 275f, 64f, 64f), this.RCLoadTexture("pspawnplayerM"));
					if (GUI.Button(new Rect(num + 27f, 179f, 64f, 60f), "Supply"))
					{
						flag = true;
						GameObject original = (GameObject)Resources.Load("aot_supply");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						this.selectedObj.name = "base,aot_supply";
					}
					else if (GUI.Button(new Rect(num + 118f, 179f, 64f, 60f), "Cannon \nWall"))
					{
						flag = true;
						GameObject original = (GameObject)FengGameManagerMKII.RCassets.Load("CannonWallProp");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						this.selectedObj.name = "photon,CannonWall";
					}
					else if (GUI.Button(new Rect(num + 209f, 179f, 64f, 60f), "Cannon\n Ground"))
					{
						flag = true;
						GameObject original = (GameObject)FengGameManagerMKII.RCassets.Load("CannonGroundProp");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						this.selectedObj.name = "photon,CannonGround";
					}
					else if (GUI.Button(new Rect(num + 27f, 344f, 64f, 60f), "Titan"))
					{
						flag = true;
						flag2 = true;
						GameObject original = (GameObject)FengGameManagerMKII.RCassets.Load("titan");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						this.selectedObj.name = "spawnpoint,titan";
					}
					else if (GUI.Button(new Rect(num + 118f, 344f, 64f, 60f), "Player \nCyan"))
					{
						flag = true;
						flag2 = true;
						GameObject original = (GameObject)FengGameManagerMKII.RCassets.Load("playerC");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						this.selectedObj.name = "spawnpoint,playerC";
					}
					else if (GUI.Button(new Rect(num + 209f, 344f, 64f, 60f), "Player \nMagenta"))
					{
						flag = true;
						flag2 = true;
						GameObject original = (GameObject)FengGameManagerMKII.RCassets.Load("playerM");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original);
						this.selectedObj.name = "spawnpoint,playerM";
					}
					GUI.EndScrollView();
				}
				else if ((int)FengGameManagerMKII.settingsOld[64] == 107)
				{
					GUI.DrawTexture(new Rect(num + 30f, 90f, 64f, 64f), this.RCLoadTexture("pbarrier"));
					GUI.DrawTexture(new Rect(num + 30f, 199f, 64f, 64f), this.RCLoadTexture("pregion"));
					GUI.Label(new Rect(num + 110f, 243f, 200f, 22f), "Region Name:", "Label");
					GUI.Label(new Rect(num + 110f, 179f, 200f, 22f), "Disable Map Bounds:", "Label");
					bool flag5 = false;
					if ((int)FengGameManagerMKII.settingsOld[186] == 1)
					{
						flag5 = true;
						if (!FengGameManagerMKII.linkHash[3].ContainsKey("mapbounds"))
						{
							FengGameManagerMKII.linkHash[3].Add("mapbounds", "map,disablebounds");
						}
					}
					else if (FengGameManagerMKII.linkHash[3].ContainsKey("mapbounds"))
					{
						FengGameManagerMKII.linkHash[3].Remove("mapbounds");
					}
					if (GUI.Button(new Rect(num + 30f, 159f, 64f, 30f), "Barrier"))
					{
						flag = true;
						flag2 = true;
						GameObject original2 = (GameObject)FengGameManagerMKII.RCassets.Load("barrierEditor");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
						this.selectedObj.name = "misc,barrier";
					}
					else if (GUI.Button(new Rect(num + 30f, 268f, 64f, 30f), "Region"))
					{
						if ((string)FengGameManagerMKII.settingsOld[191] == string.Empty)
						{
							FengGameManagerMKII.settingsOld[191] = "Region" + UnityEngine.Random.Range(10000, 99999);
						}
						flag = true;
						flag2 = true;
						GameObject original2 = (GameObject)FengGameManagerMKII.RCassets.Load("regionEditor");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
						GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
						gameObject3.name = "RegionLabel";
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[71], out result))
						{
							FengGameManagerMKII.settingsOld[71] = "1";
						}
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[70], out result))
						{
							FengGameManagerMKII.settingsOld[70] = "1";
						}
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[72], out result))
						{
							FengGameManagerMKII.settingsOld[72] = "1";
						}
						gameObject3.transform.parent = this.selectedObj.transform;
						float y = 1f;
						if (Convert.ToSingle((string)FengGameManagerMKII.settingsOld[71]) > 100f)
						{
							y = 0.8f;
						}
						else if (Convert.ToSingle((string)FengGameManagerMKII.settingsOld[71]) > 1000f)
						{
							y = 0.5f;
						}
						gameObject3.transform.localPosition = new Vector3(0f, y, 0f);
						gameObject3.transform.localScale = new Vector3(5f / Convert.ToSingle((string)FengGameManagerMKII.settingsOld[70]), 5f / Convert.ToSingle((string)FengGameManagerMKII.settingsOld[71]), 5f / Convert.ToSingle((string)FengGameManagerMKII.settingsOld[72]));
						gameObject3.GetComponent<UILabel>().text = (string)FengGameManagerMKII.settingsOld[191];
						this.selectedObj.AddComponent<RCRegionLabel>();
						this.selectedObj.GetComponent<RCRegionLabel>().myLabel = gameObject3;
						this.selectedObj.name = "misc,region," + (string)FengGameManagerMKII.settingsOld[191];
					}
					FengGameManagerMKII.settingsOld[191] = GUI.TextField(new Rect(num + 200f, 243f, 75f, 20f), (string)FengGameManagerMKII.settingsOld[191]);
					bool flag6 = GUI.Toggle(new Rect(num + 240f, 179f, 40f, 20f), flag5, "On");
					if (flag6 != flag5)
					{
						if (flag6)
						{
							FengGameManagerMKII.settingsOld[186] = 1;
						}
						else
						{
							FengGameManagerMKII.settingsOld[186] = 0;
						}
					}
				}
				else if ((int)FengGameManagerMKII.settingsOld[64] == 105)
				{
					GUI.Label(new Rect(num + 95f, 85f, 130f, 20f), "Custom Spawners:", "Label");
					GUI.DrawTexture(new Rect(num + 7.8f, 110f, 64f, 64f), this.RCLoadTexture("ptitan"));
					GUI.DrawTexture(new Rect(num + 79.6f, 110f, 64f, 64f), this.RCLoadTexture("pabnormal"));
					GUI.DrawTexture(new Rect(num + 151.4f, 110f, 64f, 64f), this.RCLoadTexture("pjumper"));
					GUI.DrawTexture(new Rect(num + 223.2f, 110f, 64f, 64f), this.RCLoadTexture("pcrawler"));
					GUI.DrawTexture(new Rect(num + 7.8f, 224f, 64f, 64f), this.RCLoadTexture("ppunk"));
					GUI.DrawTexture(new Rect(num + 79.6f, 224f, 64f, 64f), this.RCLoadTexture("pannie"));
					float result2;
					if (GUI.Button(new Rect(num + 7.8f, 179f, 64f, 30f), "Titan"))
					{
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[83], out result2))
						{
							FengGameManagerMKII.settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)FengGameManagerMKII.RCassets.Load("spawnTitan");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)FengGameManagerMKII.settingsOld[84];
						this.selectedObj.name = "photon,spawnTitan," + (string)FengGameManagerMKII.settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 79.6f, 179f, 64f, 30f), "Aberrant"))
					{
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[83], out result2))
						{
							FengGameManagerMKII.settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)FengGameManagerMKII.RCassets.Load("spawnAbnormal");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)FengGameManagerMKII.settingsOld[84];
						this.selectedObj.name = "photon,spawnAbnormal," + (string)FengGameManagerMKII.settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 151.4f, 179f, 64f, 30f), "Jumper"))
					{
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[83], out result2))
						{
							FengGameManagerMKII.settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)FengGameManagerMKII.RCassets.Load("spawnJumper");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)FengGameManagerMKII.settingsOld[84];
						this.selectedObj.name = "photon,spawnJumper," + (string)FengGameManagerMKII.settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 223.2f, 179f, 64f, 30f), "Crawler"))
					{
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[83], out result2))
						{
							FengGameManagerMKII.settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)FengGameManagerMKII.RCassets.Load("spawnCrawler");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)FengGameManagerMKII.settingsOld[84];
						this.selectedObj.name = "photon,spawnCrawler," + (string)FengGameManagerMKII.settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 7.8f, 293f, 64f, 30f), "Punk"))
					{
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[83], out result2))
						{
							FengGameManagerMKII.settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)FengGameManagerMKII.RCassets.Load("spawnPunk");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)FengGameManagerMKII.settingsOld[84];
						this.selectedObj.name = "photon,spawnPunk," + (string)FengGameManagerMKII.settingsOld[83] + "," + num11;
					}
					else if (GUI.Button(new Rect(num + 79.6f, 293f, 64f, 30f), "Annie"))
					{
						if (!float.TryParse((string)FengGameManagerMKII.settingsOld[83], out result2))
						{
							FengGameManagerMKII.settingsOld[83] = "30";
						}
						flag = true;
						flag2 = true;
						GameObject original3 = (GameObject)FengGameManagerMKII.RCassets.Load("spawnAnnie");
						this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original3);
						int num11 = (int)FengGameManagerMKII.settingsOld[84];
						this.selectedObj.name = "photon,spawnAnnie," + (string)FengGameManagerMKII.settingsOld[83] + "," + num11;
					}
					GUI.Label(new Rect(num + 7f, 379f, 140f, 22f), "Spawn Timer:", "Label");
					FengGameManagerMKII.settingsOld[83] = GUI.TextField(new Rect(num + 100f, 379f, 50f, 20f), (string)FengGameManagerMKII.settingsOld[83]);
					GUI.Label(new Rect(num + 7f, 356f, 140f, 22f), "Endless spawn:", "Label");
					GUI.Label(new Rect(num + 7f, 405f, 290f, 80f), "* The above settingsOld apply only to the next placed spawner. You can have unique spawn times and settingsOld for each individual titan spawner.", "Label");
					bool flag7 = false;
					if ((int)FengGameManagerMKII.settingsOld[84] == 1)
					{
						flag7 = true;
					}
					bool flag8 = GUI.Toggle(new Rect(num + 100f, 356f, 40f, 20f), flag7, "On");
					if (flag7 != flag8)
					{
						if (flag8)
						{
							FengGameManagerMKII.settingsOld[84] = 1;
						}
						else
						{
							FengGameManagerMKII.settingsOld[84] = 0;
						}
					}
				}
				else if ((int)FengGameManagerMKII.settingsOld[64] == 102)
				{
					string[] array7 = new string[12]
					{
						"cuboid", "plane", "sphere", "cylinder", "capsule", "pyramid", "cone", "prism", "arc90", "arc180",
						"torus", "tube"
					};
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, 90f + 114f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, 159f + 114f * (float)num9, 64f, 30f), array7[j]))
						{
							flag = true;
							GameObject original2 = (GameObject)FengGameManagerMKII.RCassets.Load(array7[j]);
							this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original2);
							this.selectedObj.name = "custom," + array7[j];
						}
					}
				}
				else if ((int)FengGameManagerMKII.settingsOld[64] == 103)
				{
					List<string> list2 = new List<string> { "arch1", "house1" };
					string[] array7 = new string[44]
					{
						"tower1", "tower2", "tower3", "tower4", "tower5", "house1", "house2", "house3", "house4", "house5",
						"house6", "house7", "house8", "house9", "house10", "house11", "house12", "house13", "house14", "pillar1",
						"pillar2", "village1", "village2", "windmill1", "arch1", "canal1", "castle1", "church1", "cannon1", "statue1",
						"statue2", "wagon1", "elevator1", "bridge1", "dummy1", "spike1", "wall1", "wall2", "wall3", "wall4",
						"arena1", "arena2", "arena3", "arena4"
					};
					float val = 110f + 114f * (float)((array7.Length - 1) / 4);
					this.scroll = GUI.BeginScrollView(new Rect(num, 90f, 303f, 350f), this.scroll, new Rect(num, 90f, 300f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, 90f + 114f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, 159f + 114f * (float)num9, 64f, 30f), array7[j]))
						{
							flag = true;
							GameObject original4 = (GameObject)FengGameManagerMKII.RCassets.Load(array7[j]);
							this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							if (list2.Contains(array7[j]))
							{
								this.selectedObj.name = "customb," + array7[j];
							}
							else
							{
								this.selectedObj.name = "custom," + array7[j];
							}
						}
					}
					GUI.EndScrollView();
				}
				else if ((int)FengGameManagerMKII.settingsOld[64] == 104)
				{
					List<string> list2 = new List<string> { "tree0" };
					string[] array7 = new string[23]
					{
						"leaf0", "leaf1", "leaf2", "field1", "field2", "tree0", "tree1", "tree2", "tree3", "tree4",
						"tree5", "tree6", "tree7", "log1", "log2", "trunk1", "boulder1", "boulder2", "boulder3", "boulder4",
						"boulder5", "cave1", "cave2"
					};
					float val = 110f + 114f * (float)((array7.Length - 1) / 4);
					this.scroll = GUI.BeginScrollView(new Rect(num, 90f, 303f, 350f), this.scroll, new Rect(num, 90f, 300f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, 90f + 114f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, 159f + 114f * (float)num9, 64f, 30f), array7[j]))
						{
							flag = true;
							GameObject original4 = (GameObject)FengGameManagerMKII.RCassets.Load(array7[j]);
							this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							if (list2.Contains(array7[j]))
							{
								this.selectedObj.name = "customb," + array7[j];
							}
							else
							{
								this.selectedObj.name = "custom," + array7[j];
							}
						}
					}
					GUI.EndScrollView();
				}
				else if ((int)FengGameManagerMKII.settingsOld[64] == 108)
				{
					string[] array8 = new string[12]
					{
						"Cuboid", "Plane", "Sphere", "Cylinder", "Capsule", "Pyramid", "Cone", "Prism", "Arc90", "Arc180",
						"Torus", "Tube"
					};
					string[] array7 = new string[12];
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "start" + array8[j];
					}
					float val = 110f + 114f * (float)((array7.Length - 1) / 4);
					val *= 4f;
					val += 200f;
					this.scroll = GUI.BeginScrollView(new Rect(num, 90f, 303f, 350f), this.scroll, new Rect(num, 90f, 300f, val), alwaysShowHorizontal: true, alwaysShowVertical: true);
					GUI.Label(new Rect(num + 90f, 90f, 200f, 22f), "Racing Start Barrier");
					int num12 = 125;
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)FengGameManagerMKII.RCassets.Load(array7[j]);
							this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							this.selectedObj.name = "racing," + array7[j];
						}
					}
					num12 += 114 * (array7.Length / 4) + 10;
					GUI.Label(new Rect(num + 93f, num12, 200f, 22f), "Racing End Trigger");
					num12 += 35;
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "end" + array8[j];
					}
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)FengGameManagerMKII.RCassets.Load(array7[j]);
							this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							this.selectedObj.name = "racing," + array7[j];
						}
					}
					num12 += 114 * (array7.Length / 4) + 10;
					GUI.Label(new Rect(num + 113f, num12, 200f, 22f), "Kill Trigger");
					num12 += 35;
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "kill" + array8[j];
					}
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)FengGameManagerMKII.RCassets.Load(array7[j]);
							this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							this.selectedObj.name = "racing," + array7[j];
						}
					}
					num12 += 114 * (array7.Length / 4) + 10;
					GUI.Label(new Rect(num + 95f, num12, 200f, 22f), "Checkpoint Trigger");
					num12 += 35;
					for (int j = 0; j < array7.Length; j++)
					{
						array7[j] = "checkpoint" + array8[j];
					}
					for (int j = 0; j < array7.Length; j++)
					{
						int num10 = j % 4;
						int num9 = j / 4;
						GUI.DrawTexture(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 114f * (float)num9, 64f, 64f), this.RCLoadTexture("p" + array7[j]));
						if (GUI.Button(new Rect(num + 7.8f + 71.8f * (float)num10, (float)num12 + 69f + 114f * (float)num9, 64f, 30f), array8[j]))
						{
							flag = true;
							flag2 = true;
							GameObject original4 = (GameObject)FengGameManagerMKII.RCassets.Load(array7[j]);
							this.selectedObj = (GameObject)UnityEngine.Object.Instantiate(original4);
							this.selectedObj.name = "racing," + array7[j];
						}
					}
					GUI.EndScrollView();
				}
				else if ((int)FengGameManagerMKII.settingsOld[64] == 106)
				{
					GUI.Label(new Rect(num + 10f, 80f, 200f, 22f), "- Tree 2 designed by Ken P.", "Label");
					GUI.Label(new Rect(num + 10f, 105f, 250f, 22f), "- Tower 2, House 5 designed by Matthew Santos", "Label");
					GUI.Label(new Rect(num + 10f, 130f, 200f, 22f), "- Cannon retextured by Mika", "Label");
					GUI.Label(new Rect(num + 10f, 155f, 200f, 22f), "- Arena 1,2,3 & 4 created by Gun", "Label");
					GUI.Label(new Rect(num + 10f, 180f, 250f, 22f), "- Cannon Wall/Ground textured by Bellfox", "Label");
					GUI.Label(new Rect(num + 10f, 205f, 250f, 120f), "- House 7 - 14, Statue1, Statue2, Wagon1, Wall 1, Wall 2, Wall 3, Wall 4, CannonWall, CannonGround, Tower5, Bridge1, Dummy1, Spike1 created by meecube", "Label");
				}
				if (!flag || !(this.selectedObj != null))
				{
					return;
				}
				if (!float.TryParse((string)FengGameManagerMKII.settingsOld[70], out result))
				{
					FengGameManagerMKII.settingsOld[70] = "1";
				}
				if (!float.TryParse((string)FengGameManagerMKII.settingsOld[71], out result))
				{
					FengGameManagerMKII.settingsOld[71] = "1";
				}
				if (!float.TryParse((string)FengGameManagerMKII.settingsOld[72], out result))
				{
					FengGameManagerMKII.settingsOld[72] = "1";
				}
				if (!float.TryParse((string)FengGameManagerMKII.settingsOld[79], out result))
				{
					FengGameManagerMKII.settingsOld[79] = "1";
				}
				if (!float.TryParse((string)FengGameManagerMKII.settingsOld[80], out result))
				{
					FengGameManagerMKII.settingsOld[80] = "1";
				}
				if (!flag2)
				{
					float a = 1f;
					if ((string)FengGameManagerMKII.settingsOld[69] != "default")
					{
						if (((string)FengGameManagerMKII.settingsOld[69]).StartsWith("transparent"))
						{
							if (float.TryParse(((string)FengGameManagerMKII.settingsOld[69]).Substring(11), out var result3))
							{
								a = result3;
							}
							Renderer[] componentsInChildren = this.selectedObj.GetComponentsInChildren<Renderer>();
							foreach (Renderer renderer2 in componentsInChildren)
							{
								renderer2.material = (Material)FengGameManagerMKII.RCassets.Load("transparent");
								renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[79]), renderer2.material.mainTextureScale.y * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[80]));
							}
						}
						else
						{
							Renderer[] componentsInChildren = this.selectedObj.GetComponentsInChildren<Renderer>();
							foreach (Renderer renderer3 in componentsInChildren)
							{
								if (!renderer3.name.Contains("Particle System") || !this.selectedObj.name.Contains("aot_supply"))
								{
									renderer3.material = (Material)FengGameManagerMKII.RCassets.Load((string)FengGameManagerMKII.settingsOld[69]);
									renderer3.material.mainTextureScale = new Vector2(renderer3.material.mainTextureScale.x * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[79]), renderer3.material.mainTextureScale.y * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[80]));
								}
							}
						}
					}
					float num13 = 1f;
					MeshFilter[] componentsInChildren2 = this.selectedObj.GetComponentsInChildren<MeshFilter>();
					foreach (MeshFilter meshFilter in componentsInChildren2)
					{
						if (this.selectedObj.name.StartsWith("customb"))
						{
							if (num13 < meshFilter.mesh.bounds.size.y)
							{
								num13 = meshFilter.mesh.bounds.size.y;
							}
						}
						else if (num13 < meshFilter.mesh.bounds.size.z)
						{
							num13 = meshFilter.mesh.bounds.size.z;
						}
					}
					float num14 = this.selectedObj.transform.localScale.x * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[70]);
					num14 -= 0.001f;
					float y3 = this.selectedObj.transform.localScale.y * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[71]);
					float z2 = this.selectedObj.transform.localScale.z * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[72]);
					this.selectedObj.transform.localScale = new Vector3(num14, y3, z2);
					if ((int)FengGameManagerMKII.settingsOld[76] == 1)
					{
						Color color = new Color((float)FengGameManagerMKII.settingsOld[73], (float)FengGameManagerMKII.settingsOld[74], (float)FengGameManagerMKII.settingsOld[75], a);
						componentsInChildren2 = this.selectedObj.GetComponentsInChildren<MeshFilter>();
						for (int i = 0; i < componentsInChildren2.Length; i++)
						{
							Mesh mesh = componentsInChildren2[i].mesh;
							Color[] array5 = new Color[mesh.vertexCount];
							for (int k = 0; k < mesh.vertexCount; k++)
							{
								array5[k] = color;
							}
							mesh.colors = array5;
						}
					}
					float num15 = this.selectedObj.transform.localScale.z;
					if (this.selectedObj.name.Contains("boulder2") || this.selectedObj.name.Contains("boulder3") || this.selectedObj.name.Contains("field2"))
					{
						num15 *= 0.01f;
					}
					float num16 = 10f + num15 * num13 * 1.2f / 2f;
					this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num16, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num16);
					this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
					string text4 = this.selectedObj.name;
					string[] array9 = new string[21]
					{
						text4,
						",",
						(string)FengGameManagerMKII.settingsOld[69],
						",",
						(string)FengGameManagerMKII.settingsOld[70],
						",",
						(string)FengGameManagerMKII.settingsOld[71],
						",",
						(string)FengGameManagerMKII.settingsOld[72],
						",",
						FengGameManagerMKII.settingsOld[76].ToString(),
						",",
						((float)FengGameManagerMKII.settingsOld[73]).ToString(),
						",",
						((float)FengGameManagerMKII.settingsOld[74]).ToString(),
						",",
						((float)FengGameManagerMKII.settingsOld[75]).ToString(),
						",",
						(string)FengGameManagerMKII.settingsOld[79],
						",",
						(string)FengGameManagerMKII.settingsOld[80]
					};
					this.selectedObj.name = string.Concat(array9);
					this.unloadAssetsEditor();
				}
				else if (this.selectedObj.name.StartsWith("misc"))
				{
					if (this.selectedObj.name.Contains("barrier") || this.selectedObj.name.Contains("region") || this.selectedObj.name.Contains("racing"))
					{
						float num13 = 1f;
						float num14 = this.selectedObj.transform.localScale.x * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[70]);
						num14 -= 0.001f;
						float y3 = this.selectedObj.transform.localScale.y * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[71]);
						float z2 = this.selectedObj.transform.localScale.z * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[72]);
						this.selectedObj.transform.localScale = new Vector3(num14, y3, z2);
						float num15 = this.selectedObj.transform.localScale.z;
						float num16 = 10f + num15 * num13 * 1.2f / 2f;
						this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num16, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num16);
						if (!this.selectedObj.name.Contains("region"))
						{
							this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
						}
						string text4 = this.selectedObj.name;
						this.selectedObj.name = text4 + "," + (string)FengGameManagerMKII.settingsOld[70] + "," + (string)FengGameManagerMKII.settingsOld[71] + "," + (string)FengGameManagerMKII.settingsOld[72];
					}
				}
				else if (this.selectedObj.name.StartsWith("racing"))
				{
					float num13 = 1f;
					float num14 = this.selectedObj.transform.localScale.x * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[70]);
					num14 -= 0.001f;
					float y3 = this.selectedObj.transform.localScale.y * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[71]);
					float z2 = this.selectedObj.transform.localScale.z * Convert.ToSingle((string)FengGameManagerMKII.settingsOld[72]);
					this.selectedObj.transform.localScale = new Vector3(num14, y3, z2);
					float num15 = this.selectedObj.transform.localScale.z;
					float num16 = 10f + num15 * num13 * 1.2f / 2f;
					this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * num16, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * num16);
					this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
					string text4 = this.selectedObj.name;
					this.selectedObj.name = text4 + "," + (string)FengGameManagerMKII.settingsOld[70] + "," + (string)FengGameManagerMKII.settingsOld[71] + "," + (string)FengGameManagerMKII.settingsOld[72];
				}
				else
				{
					this.selectedObj.transform.position = new Vector3(Camera.main.transform.position.x + Camera.main.transform.forward.x * 10f, Camera.main.transform.position.y + Camera.main.transform.forward.y * 10f, Camera.main.transform.position.z + Camera.main.transform.forward.z * 10f);
					this.selectedObj.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
				}
				Screen.lockCursor = true;
				GUI.FocusControl(null);
			}
			else
			{
				if (GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
				{
					return;
				}
				if (Time.timeScale <= 0.1f)
				{
					float num17 = (float)Screen.width / 2f;
					float num18 = (float)Screen.height / 2f;
					GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
					GUI.DrawTexture(new Rect(num17 - 98f, num18 - 48f, 196f, 96f), this.textureBackgroundBlue);
					GUI.Box(new Rect(num17 - 100f, num18 - 50f, 200f, 100f), string.Empty);
					if (this.pauseWaitTime <= 3f)
					{
						GUI.Label(new Rect(num17 - 43f, num18 - 15f, 200f, 22f), "Unpausing in:");
						GUI.Label(new Rect(num17 - 8f, num18 + 5f, 200f, 22f), this.pauseWaitTime.ToString("F1"));
					}
					else
					{
						GUI.Label(new Rect(num17 - 43f, num18 - 10f, 200f, 22f), "Game Paused.");
					}
				}
				else if (!FengGameManagerMKII.logicLoaded || !FengGameManagerMKII.customLevelLoaded)
				{
					float num17 = (float)Screen.width / 2f;
					float num18 = (float)Screen.height / 2f;
					GUI.backgroundColor = new Color(0.08f, 0.3f, 0.4f, 1f);
					GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), this.textureBackgroundBlack);
					GUI.DrawTexture(new Rect(num17 - 98f, num18 - 48f, 196f, 146f), this.textureBackgroundBlue);
					GUI.Box(new Rect(num17 - 100f, num18 - 50f, 200f, 150f), string.Empty);
					int length = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.currentLevel]).Length;
					int length2 = RCextensions.returnStringFromObject(PhotonNetwork.masterClient.customProperties[PhotonPlayerProperty.currentLevel]).Length;
					GUI.Label(new Rect(num17 - 60f, num18 - 30f, 200f, 22f), "Loading Level (" + length + "/" + length2 + ")");
					this.retryTime += Time.deltaTime;
					if (GUI.Button(new Rect(num17 - 20f, num18 + 50f, 40f, 30f), "Quit"))
					{
						PhotonNetwork.Disconnect();
						IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
						GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameStart = false;
						this.DestroyAllExistingCloths();
						UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
						Application.LoadLevel("menu");
					}
				}
			}
		}
	}

	public void OnJoinedRoom()
	{
		this.maxPlayers = PhotonNetwork.room.maxPlayers;
		this.playerList = string.Empty;
		char[] separator = new char[1] { "`"[0] };
		UnityEngine.MonoBehaviour.print("OnJoinedRoom " + PhotonNetwork.room.name + "    >>>>   " + LevelInfo.getInfo(PhotonNetwork.room.name.Split(separator)[1]).mapName);
		this.gameTimesUp = false;
		char[] separator2 = new char[1] { "`"[0] };
		string[] array = PhotonNetwork.room.name.Split(separator2);
		FengGameManagerMKII.level = array[1];
		if (array[2] == "normal")
		{
			this.difficulty = 0;
		}
		else if (array[2] == "hard")
		{
			this.difficulty = 1;
		}
		else if (array[2] == "abnormal")
		{
			this.difficulty = 2;
		}
		IN_GAME_MAIN_CAMERA.difficulty = this.difficulty;
		this.time = int.Parse(array[3]);
		this.time *= 60;
		IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
		PhotonNetwork.LoadLevel(LevelInfo.getInfo(FengGameManagerMKII.level).mapName);
		this.name = SettingsManager.ProfileSettings.Name.Value;
		LoginFengKAI.player.name = this.name;
		LoginFengKAI.player.guildname = SettingsManager.ProfileSettings.Guild.Value;
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable
		{
			{
				PhotonPlayerProperty.name,
				LoginFengKAI.player.name
			},
			{
				PhotonPlayerProperty.guildName,
				LoginFengKAI.player.guildname
			},
			{
				PhotonPlayerProperty.kills,
				0
			},
			{
				PhotonPlayerProperty.max_dmg,
				0
			},
			{
				PhotonPlayerProperty.total_dmg,
				0
			},
			{
				PhotonPlayerProperty.deaths,
				0
			},
			{
				PhotonPlayerProperty.dead,
				true
			},
			{
				PhotonPlayerProperty.isTitan,
				0
			},
			{
				PhotonPlayerProperty.RCteam,
				0
			},
			{
				PhotonPlayerProperty.currentLevel,
				string.Empty
			}
		};
		PhotonNetwork.player.SetCustomProperties(customProperties);
		this.humanScore = 0;
		this.titanScore = 0;
		this.PVPtitanScore = 0;
		this.PVPhumanScore = 0;
		this.wave = 1;
		this.highestwave = 1;
		this.localRacingResult = string.Empty;
		this.needChooseSide = true;
		this.chatContent = new ArrayList();
		this.killInfoGO = new ArrayList();
		InRoomChat.messages.Clear();
		if (!PhotonNetwork.isMasterClient)
		{
			base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient);
		}
		this.assetCacheTextures = new Dictionary<string, Texture2D>();
		this.customMapMaterials = new Dictionary<string, Material>();
		this.isFirstLoad = true;
		if (SettingsManager.MultiplayerSettings.CurrentMultiplayerServerType == MultiplayerServerType.LAN)
		{
			FengGameManagerMKII.ServerRequestAuthentication(FengGameManagerMKII.PrivateServerAuthPass);
		}
	}

	public void OnLeftLobby()
	{
		UnityEngine.MonoBehaviour.print("OnLeftLobby");
	}

	public void OnLeftRoom()
	{
		PhotonPlayer.CleanProperties();
		InRoomChat.messages.Clear();
		if (Application.loadedLevel != 0)
		{
			Time.timeScale = 1f;
			if (PhotonNetwork.connected)
			{
				PhotonNetwork.Disconnect();
			}
			this.resetSettings(isLeave: true);
			this.loadconfig();
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
			this.gameStart = false;
			this.DestroyAllExistingCloths();
			FengGameManagerMKII.JustLeftRoom = true;
			UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
			Application.LoadLevel("menu");
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		SkyboxCustomSkinLoader.SkyboxMaterial = null;
		if (level != 0 && Application.loadedLevelName != "characterCreation" && Application.loadedLevelName != "SnapShot")
		{
			UIManager.SetMenu(MenuType.Game);
			GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
			foreach (GameObject gameObject in array)
			{
				if (!(gameObject.GetPhotonView() != null) || !gameObject.GetPhotonView().owner.isMasterClient)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			this.isWinning = false;
			this.gameStart = true;
			this.ShowHUDInfoCenter(string.Empty);
			GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
			UnityEngine.Object.Destroy(GameObject.Find("cameraDefaultPosition"));
			obj.name = "MainCamera";
			this.ui = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI_IN_GAME"));
			this.ui.name = "UI_IN_GAME";
			this.ui.SetActive(value: true);
			NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[0], state: true);
			NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[2], state: false);
			NGUITools.SetActive(this.ui.GetComponent<UIReferArray>().panels[3], state: false);
			LevelInfo ınfo = LevelInfo.getInfo(FengGameManagerMKII.level);
			this.cache();
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
			this.loadskin();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.single_kills = 0;
				this.single_maxDamage = 0;
				this.single_totalDamage = 0;
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
				Camera.main.GetComponent<SpectatorMovement>().disable = true;
				Camera.main.GetComponent<MouseLook>().disable = true;
				IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
				this.SpawnPlayer(IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper());
				int abnormal = 90;
				if (this.difficulty == 1)
				{
					abnormal = 70;
				}
				this.spawnTitanCustom("titanRespawn", abnormal, ınfo.enemyNumber, punk: false);
			}
			else
			{
				PVPcheckPoint.chkPts = new ArrayList();
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = false;
				Camera.main.GetComponent<CameraShake>().enabled = false;
				IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.MULTIPLAYER;
				if (ınfo.type == GAMEMODE.TROST)
				{
					GameObject.Find("playerRespawn").SetActive(value: false);
					UnityEngine.Object.Destroy(GameObject.Find("playerRespawn"));
					GameObject.Find("rock").animation["lift"].speed = 0f;
					GameObject.Find("door_fine").SetActive(value: false);
					GameObject.Find("door_broke").SetActive(value: true);
					UnityEngine.Object.Destroy(GameObject.Find("ppl"));
				}
				else if (ınfo.type == GAMEMODE.BOSS_FIGHT_CT)
				{
					GameObject.Find("playerRespawnTrost").SetActive(value: false);
					UnityEngine.Object.Destroy(GameObject.Find("playerRespawnTrost"));
				}
				if (this.needChooseSide)
				{
					string text = SettingsManager.InputSettings.Human.Flare1.ToString();
					this.ShowHUDInfoTopCenterADD("\n\nPRESS " + text + " TO ENTER GAME");
				}
				else if (!SettingsManager.LegacyGeneralSettings.SpecMode.Value)
				{
					if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
					{
						Screen.lockCursor = true;
					}
					else
					{
						Screen.lockCursor = false;
					}
					if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
					{
						if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
						{
							this.checkpoint = GameObject.Find("PVPchkPtT");
						}
						else
						{
							this.checkpoint = GameObject.Find("PVPchkPtH");
						}
					}
					if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
					{
						this.SpawnNonAITitan2(this.myLastHero);
					}
					else
					{
						this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
					}
				}
				if (ınfo.type == GAMEMODE.BOSS_FIGHT_CT)
				{
					UnityEngine.Object.Destroy(GameObject.Find("rock"));
				}
				if (PhotonNetwork.isMasterClient)
				{
					if (ınfo.type == GAMEMODE.TROST)
					{
						if (!this.isPlayerAllDead2())
						{
							PhotonNetwork.Instantiate("TITAN_EREN_trost", new Vector3(-200f, 0f, -194f), Quaternion.Euler(0f, 180f, 0f), 0).GetComponent<TITAN_EREN>().rockLift = true;
							int rate = 90;
							if (this.difficulty == 1)
							{
								rate = 70;
							}
							GameObject[] array2 = GameObject.FindGameObjectsWithTag("titanRespawn");
							GameObject gameObject2 = GameObject.Find("titanRespawnTrost");
							if (gameObject2 != null)
							{
								array = array2;
								foreach (GameObject gameObject3 in array)
								{
									if (gameObject3.transform.parent.gameObject == gameObject2)
									{
										this.spawnTitan(rate, gameObject3.transform.position, gameObject3.transform.rotation);
									}
								}
							}
						}
					}
					else if (ınfo.type == GAMEMODE.BOSS_FIGHT_CT)
					{
						if (!this.isPlayerAllDead2())
						{
							PhotonNetwork.Instantiate("COLOSSAL_TITAN", -Vector3.up * 10000f, Quaternion.Euler(0f, 180f, 0f), 0);
						}
					}
					else if (ınfo.type == GAMEMODE.KILL_TITAN || ınfo.type == GAMEMODE.ENDLESS_TITAN || ınfo.type == GAMEMODE.SURVIVE_MODE)
					{
						if (ınfo.name == "Annie" || ınfo.name == "Annie II")
						{
							PhotonNetwork.Instantiate("FEMALE_TITAN", GameObject.Find("titanRespawn").transform.position, GameObject.Find("titanRespawn").transform.rotation, 0);
						}
						else
						{
							int abnormal2 = 90;
							if (this.difficulty == 1)
							{
								abnormal2 = 70;
							}
							this.spawnTitanCustom("titanRespawn", abnormal2, ınfo.enemyNumber, punk: false);
						}
					}
					else if (ınfo.type != GAMEMODE.TROST && ınfo.type == GAMEMODE.PVP_CAPTURE && LevelInfo.getInfo(FengGameManagerMKII.level).mapName == "OutSide")
					{
						GameObject[] array3 = GameObject.FindGameObjectsWithTag("titanRespawn");
						if (array3.Length == 0)
						{
							return;
						}
						for (int j = 0; j < array3.Length; j++)
						{
							this.spawnTitanRaw(array3[j].transform.position, array3[j].transform.rotation).GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
						}
					}
				}
				if (!ınfo.supply)
				{
					UnityEngine.Object.Destroy(GameObject.Find("aot_supply"));
				}
				if (!PhotonNetwork.isMasterClient)
				{
					base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient);
				}
				if (LevelInfo.getInfo(FengGameManagerMKII.level).lavaMode)
				{
					UnityEngine.Object.Instantiate(Resources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
					GameObject.Find("aot_supply").transform.position = GameObject.Find("aot_supply_lava_position").transform.position;
					GameObject.Find("aot_supply").transform.rotation = GameObject.Find("aot_supply_lava_position").transform.rotation;
				}
				if (SettingsManager.LegacyGeneralSettings.SpecMode.Value)
				{
					this.EnterSpecMode(enter: true);
				}
			}
		}
		this.unloadAssets(immediate: true);
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!FengGameManagerMKII.noRestart)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.restartingMC = true;
				if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
				{
					this.restartingTitan = true;
				}
				if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
				{
					this.restartingBomb = true;
				}
				if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
				{
					this.restartingHorse = true;
				}
				if (!SettingsManager.LegacyGameSettings.KickShifters.Value)
				{
					this.restartingEren = true;
				}
			}
			this.resetSettings(isLeave: false);
			if (!LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan)
			{
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.isTitan, 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
			if (!this.gameTimesUp && PhotonNetwork.isMasterClient)
			{
				this.restartGame2(masterclientSwitched: true);
				base.photonView.RPC("setMasterRC", PhotonTargets.All);
			}
		}
		FengGameManagerMKII.noRestart = false;
	}

	public void OnPhotonCreateRoomFailed()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonCreateRoomFailed");
	}

	public void OnPhotonCustomRoomPropertiesChanged()
	{
	}

	public void OnPhotonInstantiate()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonInstantiate");
	}

	public void OnPhotonJoinRoomFailed()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonJoinRoomFailed");
	}

	public void OnPhotonMaxCccuReached()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonMaxCccuReached");
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonView photonView = base.photonView;
			if (FengGameManagerMKII.banHash.ContainsValue(RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name])))
			{
				this.kickPlayerRC(player, ban: false, "banned.");
			}
			else
			{
				int num = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statACL]);
				int num2 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statBLA]);
				int num3 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statGAS]);
				int num4 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statSPD]);
				if (num > 150 || num2 > 125 || num3 > 150 || num4 > 140)
				{
					this.kickPlayerRC(player, ban: true, "excessive stats.");
					return;
				}
				if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
				{
					base.StartCoroutine(this.WaitAndReloadKDR(player));
				}
				if (FengGameManagerMKII.level.StartsWith("Custom"))
				{
					base.StartCoroutine(this.customlevelE(new List<PhotonPlayer> { player }));
				}
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
				{
					hashtable.Add("bomb", 1);
				}
				if (SettingsManager.LegacyGameSettings.BombModeCeiling.Value)
				{
					hashtable.Add("bombCeiling", 1);
				}
				else
				{
					hashtable.Add("bombCeiling", 0);
				}
				if (SettingsManager.LegacyGameSettings.BombModeInfiniteGas.Value)
				{
					hashtable.Add("bombInfiniteGas", 1);
				}
				else
				{
					hashtable.Add("bombInfiniteGas", 0);
				}
				if (SettingsManager.LegacyGameSettings.GlobalHideNames.Value)
				{
					hashtable.Add("globalHideNames", 1);
				}
				if (SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
				{
					hashtable.Add("globalDisableMinimap", 1);
				}
				if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
				{
					hashtable.Add("team", SettingsManager.LegacyGameSettings.TeamMode.Value);
				}
				if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
				{
					hashtable.Add("point", SettingsManager.LegacyGameSettings.PointModeAmount.Value);
				}
				if (!SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
				{
					hashtable.Add("rock", 1);
				}
				if (SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value)
				{
					hashtable.Add("explode", SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
				{
					hashtable.Add("healthMode", SettingsManager.LegacyGameSettings.TitanHealthMode.Value);
					hashtable.Add("healthLower", SettingsManager.LegacyGameSettings.TitanHealthMin.Value);
					hashtable.Add("healthUpper", SettingsManager.LegacyGameSettings.TitanHealthMax.Value);
				}
				if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
				{
					hashtable.Add("infection", SettingsManager.LegacyGameSettings.InfectionModeAmount.Value);
				}
				if (SettingsManager.LegacyGameSettings.KickShifters.Value)
				{
					hashtable.Add("eren", 1);
				}
				if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
				{
					hashtable.Add("titanc", SettingsManager.LegacyGameSettings.TitanNumber.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value)
				{
					hashtable.Add("damage", SettingsManager.LegacyGameSettings.TitanArmor.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
				{
					hashtable.Add("sizeMode", SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value);
					hashtable.Add("sizeLower", SettingsManager.LegacyGameSettings.TitanSizeMin.Value);
					hashtable.Add("sizeUpper", SettingsManager.LegacyGameSettings.TitanSizeMax.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
				{
					hashtable.Add("spawnMode", 1);
					hashtable.Add("nRate", SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value);
					hashtable.Add("aRate", SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value);
					hashtable.Add("jRate", SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value);
					hashtable.Add("cRate", SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value);
					hashtable.Add("pRate", SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
				{
					hashtable.Add("waveModeOn", 1);
					hashtable.Add("waveModeNum", SettingsManager.LegacyGameSettings.TitanPerWaves.Value);
				}
				if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
				{
					hashtable.Add("friendly", 1);
				}
				if (SettingsManager.LegacyGameSettings.BladePVP.Value > 0)
				{
					hashtable.Add("pvp", SettingsManager.LegacyGameSettings.BladePVP.Value);
				}
				if (SettingsManager.LegacyGameSettings.TitanMaxWavesEnabled.Value)
				{
					hashtable.Add("maxwave", SettingsManager.LegacyGameSettings.TitanMaxWaves.Value);
				}
				if (SettingsManager.LegacyGameSettings.EndlessRespawnEnabled.Value)
				{
					hashtable.Add("endless", SettingsManager.LegacyGameSettings.EndlessRespawnTime.Value);
				}
				if (SettingsManager.LegacyGameSettings.Motd.Value != string.Empty)
				{
					hashtable.Add("motd", SettingsManager.LegacyGameSettings.Motd.Value);
				}
				if (SettingsManager.LegacyGameSettings.AllowHorses.Value)
				{
					hashtable.Add("horse", 1);
				}
				if (!SettingsManager.LegacyGameSettings.AHSSAirReload.Value)
				{
					hashtable.Add("ahssReload", 1);
				}
				if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
				{
					hashtable.Add("punkWaves", 1);
				}
				if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
				{
					hashtable.Add("deadlycannons", 1);
				}
				if (SettingsManager.LegacyGameSettings.RacingEndless.Value)
				{
					hashtable.Add("asoracing", 1);
				}
				hashtable.Add("racingStartTime", SettingsManager.LegacyGameSettings.RacingStartTime.Value);
				if (FengGameManagerMKII.ignoreList != null && FengGameManagerMKII.ignoreList.Count > 0)
				{
					photonView.RPC("ignorePlayerArray", player, FengGameManagerMKII.ignoreList.ToArray());
				}
				photonView.RPC("settingRPC", player, hashtable);
				photonView.RPC("setMasterRC", player);
				if (Time.timeScale <= 0.1f && this.pauseWaitTime > 3f)
				{
					photonView.RPC("pauseRPC", player, true);
					object[] parameters = new object[2] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
					photonView.RPC("Chat", player, parameters);
				}
			}
		}
		this.RecompilePlayerList(0.1f);
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		if (!this.gameTimesUp)
		{
			this.oneTitanDown(string.Empty, onPlayerLeave: true);
			this.someOneIsDead(0);
		}
		if (FengGameManagerMKII.ignoreList.Contains(player.ID))
		{
			FengGameManagerMKII.ignoreList.Remove(player.ID);
		}
		InstantiateTracker.instance.TryRemovePlayer(player.ID);
		if (PhotonNetwork.isMasterClient)
		{
			base.photonView.RPC("verifyPlayerHasLeft", PhotonTargets.All, player.ID);
			if (SettingsManager.LegacyGameSettings.PreserveKDR.Value)
			{
				string key = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
				if (this.PreservedPlayerKDR.ContainsKey(key))
				{
					this.PreservedPlayerKDR.Remove(key);
				}
				int[] value = new int[4]
				{
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.kills]),
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.deaths]),
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.max_dmg]),
					RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.total_dmg])
				};
				this.PreservedPlayerKDR.Add(key, value);
			}
		}
		this.RecompilePlayerList(0.1f);
	}

	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		this.RecompilePlayerList(0.1f);
		if (playerAndUpdatedProps == null || playerAndUpdatedProps.Length < 2 || (PhotonPlayer)playerAndUpdatedProps[0] != PhotonNetwork.player)
		{
			return;
		}
		ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];
		if (hashtable.ContainsKey("name") && RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]) != this.name)
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add(PhotonPlayerProperty.name, this.name);
			PhotonNetwork.player.SetCustomProperties(hashtable2);
		}
		if (hashtable.ContainsKey("statACL") || hashtable.ContainsKey("statBLA") || hashtable.ContainsKey("statGAS") || hashtable.ContainsKey("statSPD"))
		{
			PhotonPlayer player = PhotonNetwork.player;
			int num = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statACL]);
			int num2 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statBLA]);
			int num3 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statGAS]);
			int num4 = RCextensions.returnIntFromObject(player.customProperties[PhotonPlayerProperty.statSPD]);
			if (num > 150)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { 
				{
					PhotonPlayerProperty.statACL,
					100
				} };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num = 100;
			}
			if (num2 > 125)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { 
				{
					PhotonPlayerProperty.statBLA,
					100
				} };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num2 = 100;
			}
			if (num3 > 150)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { 
				{
					PhotonPlayerProperty.statGAS,
					100
				} };
				PhotonNetwork.player.SetCustomProperties(hashtable2);
				num3 = 100;
			}
			if (num4 > 140)
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
				hashtable2.Add(PhotonPlayerProperty.statSPD, 100);
				PhotonNetwork.player.SetCustomProperties(hashtable2);
			}
		}
	}

	public void OnPhotonRandomJoinFailed()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonRandomJoinFailed");
	}

	public void OnPhotonSerializeView()
	{
		UnityEngine.MonoBehaviour.print("OnPhotonSerializeView");
	}

	public void OnReceivedRoomListUpdate()
	{
	}

	public void OnUpdate()
	{
		if (FengGameManagerMKII.RCEvents.ContainsKey("OnUpdate"))
		{
			if (this.updateTime > 0f)
			{
				this.updateTime -= Time.deltaTime;
				return;
			}
			((RCEvent)FengGameManagerMKII.RCEvents["OnUpdate"]).checkEvent();
			this.updateTime = 1f;
		}
	}

	public void OnUpdatedFriendList()
	{
		UnityEngine.MonoBehaviour.print("OnUpdatedFriendList");
	}

	public int operantType(string str, int condition)
	{
		switch (condition)
		{
		case 0:
		case 3:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 5;
				}
				if (!str.StartsWith("LessThan"))
				{
					if (str.StartsWith("LessThanOrEquals"))
					{
						return 1;
					}
					if (str.StartsWith("GreaterThanOrEquals"))
					{
						return 3;
					}
					if (str.StartsWith("GreaterThan"))
					{
						return 4;
					}
				}
				return 0;
			}
			return 2;
		case 1:
		case 4:
		case 5:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 5;
				}
				return 0;
			}
			return 2;
		case 2:
			if (!str.StartsWith("Equals"))
			{
				if (str.StartsWith("NotEquals"))
				{
					return 1;
				}
				if (str.StartsWith("Contains"))
				{
					return 2;
				}
				if (str.StartsWith("NotContains"))
				{
					return 3;
				}
				if (str.StartsWith("StartsWith"))
				{
					return 4;
				}
				if (str.StartsWith("NotStartsWith"))
				{
					return 5;
				}
				if (str.StartsWith("EndsWith"))
				{
					return 6;
				}
				if (str.StartsWith("NotEndsWith"))
				{
					return 7;
				}
				return 0;
			}
			return 0;
		default:
			return 0;
		}
	}

	public RCEvent parseBlock(string[] stringArray, int eventClass, int eventType, RCCondition condition)
	{
		List<RCAction> list = new List<RCAction>();
		RCEvent rCEvent = new RCEvent(null, null, 0, 0);
		for (int i = 0; i < stringArray.Length; i++)
		{
			if (stringArray[i].StartsWith("If") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
				int num7 = this.conditionType(text);
				int num8 = text.IndexOf('.');
				text = text.Substring(num8 + 1);
				int sentOperand = this.operantType(text, num7);
				num5 = text.IndexOf('(');
				num6 = text.LastIndexOf(")");
				string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
				RCCondition condition2 = new RCCondition(sentOperand, num7, this.returnHelper(array2[0]), this.returnHelper(array2[1]));
				RCEvent rCEvent2 = this.parseBlock(array, 1, 0, condition2);
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				rCEvent = rCEvent2;
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("While") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
				int num7 = this.conditionType(text);
				int num8 = text.IndexOf('.');
				text = text.Substring(num8 + 1);
				int sentOperand = this.operantType(text, num7);
				num5 = text.IndexOf('(');
				num6 = text.LastIndexOf(")");
				string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
				RCCondition condition2 = new RCCondition(sentOperand, num7, this.returnHelper(array2[0]), this.returnHelper(array2[1]));
				RCEvent rCEvent2 = this.parseBlock(array, 3, 0, condition2);
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("ForeachTitan") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
				int num7 = 0;
				RCEvent rCEvent2 = this.parseBlock(array, 2, num7, null);
				rCEvent2.foreachVariableName = text;
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("ForeachPlayer") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				int num5 = stringArray[i].IndexOf("(");
				int num6 = stringArray[i].LastIndexOf(")");
				string text = stringArray[i].Substring(num5 + 2, num6 - num5 - 3);
				int num7 = 1;
				RCEvent rCEvent2 = this.parseBlock(array, 2, num7, null);
				rCEvent2.foreachVariableName = text;
				RCAction item = new RCAction(0, 0, rCEvent2, null);
				list.Add(item);
				i = num2;
			}
			else if (stringArray[i].StartsWith("Else") && stringArray[i + 1] == "{")
			{
				int num = i + 2;
				int num2 = i + 2;
				int num3 = 0;
				for (int j = i + 2; j < stringArray.Length; j++)
				{
					if (stringArray[j] == "{")
					{
						num3++;
					}
					if (stringArray[j] == "}")
					{
						if (num3 > 0)
						{
							num3--;
							continue;
						}
						num2 = j - 1;
						j = stringArray.Length;
					}
				}
				string[] array = new string[num2 - num + 1];
				int num4 = 0;
				for (int k = num; k <= num2; k++)
				{
					array[num4] = stringArray[k];
					num4++;
				}
				if (stringArray[i] == "Else")
				{
					RCEvent rCEvent2 = this.parseBlock(array, 0, 0, null);
					RCAction item = new RCAction(0, 0, rCEvent2, null);
					rCEvent.setElse(item);
					i = num2;
				}
				else if (stringArray[i].StartsWith("Else If"))
				{
					int num5 = stringArray[i].IndexOf("(");
					int num6 = stringArray[i].LastIndexOf(")");
					string text = stringArray[i].Substring(num5 + 1, num6 - num5 - 1);
					int num7 = this.conditionType(text);
					int num8 = text.IndexOf('.');
					text = text.Substring(num8 + 1);
					int sentOperand = this.operantType(text, num7);
					num5 = text.IndexOf('(');
					num6 = text.LastIndexOf(")");
					string[] array2 = text.Substring(num5 + 1, num6 - num5 - 1).Split(',');
					RCCondition condition2 = new RCCondition(sentOperand, num7, this.returnHelper(array2[0]), this.returnHelper(array2[1]));
					RCEvent rCEvent2 = this.parseBlock(array, 1, 0, condition2);
					RCAction item = new RCAction(0, 0, rCEvent2, null);
					rCEvent.setElse(item);
					i = num2;
				}
			}
			else if (stringArray[i].StartsWith("VariableInt"))
			{
				int category = 1;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Add"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Subtract"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Multiply"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Divide"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Modulo"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Power"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableBool"))
			{
				int category = 2;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetToOpposite"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCAction item = new RCAction(category, 11, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableString"))
			{
				int category = 3;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Concat"))
				{
					RCActionHelper[] array4 = new RCActionHelper[array3.Length];
					for (int j = 0; j < array3.Length; j++)
					{
						array4[j] = this.returnHelper(array3[j]);
					}
					RCAction item = new RCAction(category, 7, null, array4);
					list.Add(item);
				}
				else if (text2.StartsWith("Append"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 8, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Replace"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCAction item = new RCAction(category, 10, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Remove"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 9, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableFloat"))
			{
				int category = 4;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("SetRandom"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Add"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Subtract"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Multiply"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Divide"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Modulo"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("Power"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariablePlayer"))
			{
				int category = 5;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("VariableTitan"))
			{
				int category = 6;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("Set"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Player"))
			{
				int category = 7;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("KillPlayer"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnPlayerAt"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = this.returnHelper(array3[3]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnPlayer"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("MovePlayer"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = this.returnHelper(array3[3]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetKills"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetDeaths"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 5, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetMaxDmg"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 6, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetTotalDmg"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 7, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetName"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 8, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetGuildName"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 9, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetTeam"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 10, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomInt"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 11, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomBool"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 12, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomString"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 13, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetCustomFloat"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 14, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Titan"))
			{
				int category = 8;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("KillTitan"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[3] { rCActionHelper, rCActionHelper2, rCActionHelper3 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnTitanAt"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = this.returnHelper(array3[3]);
					RCActionHelper rCActionHelper5 = this.returnHelper(array3[4]);
					RCActionHelper rCActionHelper6 = this.returnHelper(array3[5]);
					RCActionHelper rCActionHelper7 = this.returnHelper(array3[6]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[7] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4, rCActionHelper5, rCActionHelper6, rCActionHelper7 });
					list.Add(item);
				}
				else if (text2.StartsWith("SpawnTitan"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = this.returnHelper(array3[3]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
				else if (text2.StartsWith("SetHealth"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[2] { rCActionHelper, rCActionHelper2 });
					list.Add(item);
				}
				else if (text2.StartsWith("MoveTitan"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCActionHelper rCActionHelper2 = this.returnHelper(array3[1]);
					RCActionHelper rCActionHelper3 = this.returnHelper(array3[2]);
					RCActionHelper rCActionHelper4 = this.returnHelper(array3[3]);
					RCAction item = new RCAction(category, 4, null, new RCActionHelper[4] { rCActionHelper, rCActionHelper2, rCActionHelper3, rCActionHelper4 });
					list.Add(item);
				}
			}
			else if (stringArray[i].StartsWith("Game"))
			{
				int category = 9;
				int num9 = stringArray[i].IndexOf('.');
				int num10 = stringArray[i].IndexOf('(');
				int num11 = stringArray[i].LastIndexOf(')');
				string text2 = stringArray[i].Substring(num9 + 1, num10 - num9 - 1);
				string[] array3 = stringArray[i].Substring(num10 + 1, num11 - num10 - 1).Split(',');
				if (text2.StartsWith("PrintMessage"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCAction item = new RCAction(category, 0, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("LoseGame"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCAction item = new RCAction(category, 2, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("WinGame"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCAction item = new RCAction(category, 1, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
				else if (text2.StartsWith("Restart"))
				{
					RCActionHelper rCActionHelper = this.returnHelper(array3[0]);
					RCAction item = new RCAction(category, 3, null, new RCActionHelper[1] { rCActionHelper });
					list.Add(item);
				}
			}
		}
		return new RCEvent(condition, list, eventClass, eventType);
	}

	[RPC]
	public void pauseRPC(bool pause, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			if (pause)
			{
				this.pauseWaitTime = 100000f;
				Time.timeScale = 1E-06f;
			}
			else
			{
				this.pauseWaitTime = 3f;
			}
		}
	}

	public void playerKillInfoSingleUpdate(int dmg)
	{
		this.single_kills++;
		this.single_maxDamage = Mathf.Max(dmg, this.single_maxDamage);
		this.single_totalDamage += dmg;
	}

	public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.kills, (int)player.customProperties[PhotonPlayerProperty.kills] + 1);
		player.SetCustomProperties(hashtable);
		hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.max_dmg, Mathf.Max(dmg, (int)player.customProperties[PhotonPlayerProperty.max_dmg]));
		player.SetCustomProperties(hashtable);
		hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.total_dmg, (int)player.customProperties[PhotonPlayerProperty.total_dmg] + dmg);
		player.SetCustomProperties(hashtable);
	}

	public GameObject randomSpawnOneTitan(string place, int rate)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(place);
		int num = UnityEngine.Random.Range(0, array.Length);
		GameObject gameObject = array[num];
		return this.spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation);
	}

	public void randomSpawnTitan(string place, int rate, int num, bool punk = false)
	{
		if (num == -1)
		{
			num = 1;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag(place);
		List<GameObject> list = new List<GameObject>(array);
		if (array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			if (list.Count <= 0)
			{
				list = new List<GameObject>(array);
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			GameObject gameObject = list[index];
			list.RemoveAt(index);
			this.spawnTitan(rate, gameObject.transform.position, gameObject.transform.rotation, punk);
		}
	}

	public Texture2D RCLoadTexture(string tex)
	{
		if (this.assetCacheTextures == null)
		{
			this.assetCacheTextures = new Dictionary<string, Texture2D>();
		}
		if (this.assetCacheTextures.ContainsKey(tex))
		{
			return this.assetCacheTextures[tex];
		}
		Texture2D texture2D = (Texture2D)FengGameManagerMKII.RCassets.Load(tex);
		this.assetCacheTextures.Add(tex, texture2D);
		return texture2D;
	}

	public void RecompilePlayerList(float time)
	{
		if (!this.isRecompiling)
		{
			this.isRecompiling = true;
			base.StartCoroutine(this.WaitAndRecompilePlayerList(time));
		}
	}

	[RPC]
	private void refreshPVPStatus(int score1, int score2)
	{
		this.PVPhumanScore = score1;
		this.PVPtitanScore = score2;
	}

	[RPC]
	private void refreshPVPStatus_AHSS(int[] score1)
	{
		this.teamScores = score1;
	}

	private void refreshRacingResult()
	{
		this.localRacingResult = "Result\n";
		IComparer comparer = new IComparerRacingResult();
		this.racingResult.Sort(comparer);
		int num = Mathf.Min(this.racingResult.Count, 6);
		for (int i = 0; i < num; i++)
		{
			string text = this.localRacingResult;
			object[] array = new object[4]
			{
				text,
				"Rank ",
				i + 1,
				" : "
			};
			this.localRacingResult = string.Concat(array);
			this.localRacingResult += (this.racingResult[i] as RacingResult).name;
			this.localRacingResult = this.localRacingResult + "   " + (float)(int)((this.racingResult[i] as RacingResult).time * 100f) * 0.01f + "s";
			this.localRacingResult += "\n";
		}
		object[] parameters = new object[1] { this.localRacingResult };
		base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
	}

	private void refreshRacingResult2()
	{
		this.localRacingResult = "Result\n";
		IComparer comparer = new IComparerRacingResult();
		this.racingResult.Sort(comparer);
		int num = Mathf.Min(this.racingResult.Count, 10);
		for (int i = 0; i < num; i++)
		{
			string text = this.localRacingResult;
			object[] array = new object[4]
			{
				text,
				"Rank ",
				i + 1,
				" : "
			};
			this.localRacingResult = string.Concat(array);
			this.localRacingResult += (this.racingResult[i] as RacingResult).name;
			this.localRacingResult = this.localRacingResult + "   " + (float)(int)((this.racingResult[i] as RacingResult).time * 100f) * 0.01f + "s";
			this.localRacingResult += "\n";
		}
		object[] parameters = new object[1] { this.localRacingResult };
		base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
	}

	[RPC]
	private void refreshStatus(int score1, int score2, int wav, int highestWav, float time1, float time2, bool startRacin, bool endRacin, PhotonMessageInfo info)
	{
		if (info.sender == PhotonNetwork.masterClient && !PhotonNetwork.isMasterClient)
		{
			this.humanScore = score1;
			this.titanScore = score2;
			this.wave = wav;
			this.highestwave = highestWav;
			this.roundTime = time1;
			this.timeTotalServer = time2;
			this.startRacing = startRacin;
			this.endRacing = endRacin;
			if (this.startRacing && GameObject.Find("door") != null)
			{
				GameObject.Find("door").SetActive(value: false);
			}
		}
	}

	public IEnumerator reloadSky(bool specmode = false)
	{
		yield return new WaitForSeconds(0.5f);
		Material skyboxMaterial = SkyboxCustomSkinLoader.SkyboxMaterial;
		if (skyboxMaterial != null && Camera.main.GetComponent<Skybox>().material != skyboxMaterial)
		{
			Camera.main.GetComponent<Skybox>().material = skyboxMaterial;
		}
	}

	public void removeCT(COLOSSAL_TITAN titan)
	{
		this.cT.Remove(titan);
	}

	public void removeET(TITAN_EREN hero)
	{
		this.eT.Remove(hero);
	}

	public void removeFT(FEMALE_TITAN titan)
	{
		this.fT.Remove(titan);
	}

	public void removeHero(HERO hero)
	{
		this.heroes.Remove(hero);
	}

	public void removeHook(Bullet h)
	{
		this.hooks.Remove(h);
	}

	public void removeTitan(TITAN titan)
	{
		this.titans.Remove(titan);
	}

	[RPC]
	private void RequireStatus()
	{
		object[] parameters = new object[8] { this.humanScore, this.titanScore, this.wave, this.highestwave, this.roundTime, this.timeTotalServer, this.startRacing, this.endRacing };
		base.photonView.RPC("refreshStatus", PhotonTargets.Others, parameters);
		object[] parameters2 = new object[2] { this.PVPhumanScore, this.PVPtitanScore };
		base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters2);
		object[] parameters3 = new object[1] { this.teamScores };
		base.photonView.RPC("refreshPVPStatus_AHSS", PhotonTargets.Others, parameters3);
	}

	private void resetGameSettings()
	{
		SettingsManager.LegacyGameSettings.SetDefault();
	}

	private void resetSettings(bool isLeave)
	{
		this.name = LoginFengKAI.player.name;
		FengGameManagerMKII.masterRC = false;
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add(PhotonPlayerProperty.RCteam, 0);
		if (isLeave)
		{
			FengGameManagerMKII.currentLevel = string.Empty;
			hashtable.Add(PhotonPlayerProperty.currentLevel, string.Empty);
			this.levelCache = new List<string[]>();
			this.titanSpawns.Clear();
			this.playerSpawnsC.Clear();
			this.playerSpawnsM.Clear();
			this.titanSpawners.Clear();
			FengGameManagerMKII.intVariables.Clear();
			FengGameManagerMKII.boolVariables.Clear();
			FengGameManagerMKII.stringVariables.Clear();
			FengGameManagerMKII.floatVariables.Clear();
			FengGameManagerMKII.globalVariables.Clear();
			FengGameManagerMKII.RCRegions.Clear();
			FengGameManagerMKII.RCEvents.Clear();
			FengGameManagerMKII.RCVariableNames.Clear();
			FengGameManagerMKII.playerVariables.Clear();
			FengGameManagerMKII.titanVariables.Clear();
			FengGameManagerMKII.RCRegionTriggers.Clear();
			hashtable.Add(PhotonPlayerProperty.statACL, 100);
			hashtable.Add(PhotonPlayerProperty.statBLA, 100);
			hashtable.Add(PhotonPlayerProperty.statGAS, 100);
			hashtable.Add(PhotonPlayerProperty.statSPD, 100);
			this.restartingTitan = false;
			this.restartingMC = false;
			this.restartingHorse = false;
			this.restartingEren = false;
			this.restartingBomb = false;
		}
		PhotonNetwork.player.SetCustomProperties(hashtable);
		this.resetGameSettings();
		FengGameManagerMKII.banHash = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.imatitan = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.oldScript = string.Empty;
		FengGameManagerMKII.ignoreList = new List<int>();
		this.restartCount = new List<float>();
		FengGameManagerMKII.heroHash = new ExitGames.Client.Photon.Hashtable();
	}

	private IEnumerator respawnE(float seconds)
	{
		while (true)
		{
			yield return new WaitForSeconds(seconds);
			if (this.isLosing || this.isWinning)
			{
				continue;
			}
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
			{
				PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
				if (photonPlayer.customProperties[PhotonPlayerProperty.RCteam] == null && RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) != 2)
				{
					base.photonView.RPC("respawnHeroInNewRound", photonPlayer);
				}
			}
		}
	}

	[RPC]
	private void respawnHeroInNewRound()
	{
		if (!this.needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
		{
			this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
			this.ShowHUDInfoCenter(string.Empty);
		}
	}

	public IEnumerator restartE(float time)
	{
		yield return new WaitForSeconds(time);
		this.restartGame2();
	}

	public void restartGame2(bool masterclientSwitched = false)
	{
		if (!this.gameTimesUp)
		{
			this.PVPtitanScore = 0;
			this.PVPhumanScore = 0;
			this.startRacing = false;
			this.endRacing = false;
			this.checkpoint = null;
			this.timeElapse = 0f;
			this.roundTime = 0f;
			this.isWinning = false;
			this.isLosing = false;
			this.isPlayer1Winning = false;
			this.isPlayer2Winning = false;
			this.wave = 1;
			this.myRespawnTime = 0f;
			this.kicklist = new ArrayList();
			this.killInfoGO = new ArrayList();
			this.racingResult = new ArrayList();
			this.ShowHUDInfoCenter(string.Empty);
			this.isRestarting = true;
			this.DestroyAllExistingCloths();
			PhotonNetwork.DestroyAll();
			ExitGames.Client.Photon.Hashtable hashtable = this.checkGameGUI();
			base.photonView.RPC("settingRPC", PhotonTargets.Others, hashtable);
			base.photonView.RPC("RPCLoadLevel", PhotonTargets.All);
			this.setGameSettings(hashtable);
			if (masterclientSwitched)
			{
				this.sendChatContentInfo("<color=#A8FF24>MasterClient has switched to </color>" + ((string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor());
			}
		}
	}

	[RPC]
	private void restartGameByClient()
	{
	}

	public void restartGameSingle2()
	{
		this.startRacing = false;
		this.endRacing = false;
		this.checkpoint = null;
		this.single_kills = 0;
		this.single_maxDamage = 0;
		this.single_totalDamage = 0;
		this.timeElapse = 0f;
		this.roundTime = 0f;
		this.timeTotalServer = 0f;
		this.isWinning = false;
		this.isLosing = false;
		this.isPlayer1Winning = false;
		this.isPlayer2Winning = false;
		this.wave = 1;
		this.myRespawnTime = 0f;
		this.ShowHUDInfoCenter(string.Empty);
		this.DestroyAllExistingCloths();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void restartRC()
	{
		FengGameManagerMKII.intVariables.Clear();
		FengGameManagerMKII.boolVariables.Clear();
		FengGameManagerMKII.stringVariables.Clear();
		FengGameManagerMKII.floatVariables.Clear();
		FengGameManagerMKII.playerVariables.Clear();
		FengGameManagerMKII.titanVariables.Clear();
		if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
		{
			this.endGameInfectionRC();
		}
		else
		{
			this.endGameRC();
		}
	}

	public RCActionHelper returnHelper(string str)
	{
		string[] array = str.Split('.');
		if (float.TryParse(str, out var _))
		{
			array = new string[1] { str };
		}
		List<RCActionHelper> list = new List<RCActionHelper>();
		int sentType = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (list.Count == 0)
			{
				string text = array[i];
				int result2;
				float result3;
				if (text.StartsWith("\"") && text.EndsWith("\""))
				{
					RCActionHelper item = new RCActionHelper(0, 0, text.Substring(1, text.Length - 2));
					list.Add(item);
					sentType = 2;
				}
				else if (int.TryParse(text, out result2))
				{
					RCActionHelper item = new RCActionHelper(0, 0, result2);
					list.Add(item);
					sentType = 0;
				}
				else if (float.TryParse(text, out result3))
				{
					RCActionHelper item = new RCActionHelper(0, 0, result3);
					list.Add(item);
					sentType = 3;
				}
				else if (text.ToLower() == "true" || text.ToLower() == "false")
				{
					RCActionHelper item = new RCActionHelper(0, 0, Convert.ToBoolean(text.ToLower()));
					list.Add(item);
					sentType = 1;
				}
				else if (text.StartsWith("Variable"))
				{
					int num = text.IndexOf('(');
					int num2 = text.LastIndexOf(')');
					if (text.StartsWith("VariableInt"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 0, this.returnHelper(text));
						list.Add(item);
						sentType = 0;
					}
					else if (text.StartsWith("VariableBool"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 1, this.returnHelper(text));
						list.Add(item);
						sentType = 1;
					}
					else if (text.StartsWith("VariableString"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 2, this.returnHelper(text));
						list.Add(item);
						sentType = 2;
					}
					else if (text.StartsWith("VariableFloat"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 3, this.returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("VariablePlayer"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 4, this.returnHelper(text));
						list.Add(item);
						sentType = 4;
					}
					else if (text.StartsWith("VariableTitan"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(1, 5, this.returnHelper(text));
						list.Add(item);
						sentType = 5;
					}
				}
				else if (text.StartsWith("Region"))
				{
					int num = text.IndexOf('(');
					int num2 = text.LastIndexOf(')');
					if (text.StartsWith("RegionRandomX"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 0, this.returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("RegionRandomY"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 1, this.returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
					else if (text.StartsWith("RegionRandomZ"))
					{
						text = text.Substring(num + 1, num2 - num - 1);
						RCActionHelper item = new RCActionHelper(4, 2, this.returnHelper(text));
						list.Add(item);
						sentType = 3;
					}
				}
			}
			else
			{
				if (list.Count <= 0)
				{
					continue;
				}
				string text = array[i];
				if (list[list.Count - 1].helperClass == 1)
				{
					switch (list[list.Count - 1].helperType)
					{
					case 4:
						if (text.StartsWith("GetTeam()"))
						{
							RCActionHelper item = new RCActionHelper(2, 1, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetType()"))
						{
							RCActionHelper item = new RCActionHelper(2, 0, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetIsAlive()"))
						{
							RCActionHelper item = new RCActionHelper(2, 2, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("GetTitan()"))
						{
							RCActionHelper item = new RCActionHelper(2, 3, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetKills()"))
						{
							RCActionHelper item = new RCActionHelper(2, 4, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetDeaths()"))
						{
							RCActionHelper item = new RCActionHelper(2, 5, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetMaxDmg()"))
						{
							RCActionHelper item = new RCActionHelper(2, 6, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetTotalDmg()"))
						{
							RCActionHelper item = new RCActionHelper(2, 7, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetCustomInt()"))
						{
							RCActionHelper item = new RCActionHelper(2, 8, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetCustomBool()"))
						{
							RCActionHelper item = new RCActionHelper(2, 9, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("GetCustomString()"))
						{
							RCActionHelper item = new RCActionHelper(2, 10, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetCustomFloat()"))
						{
							RCActionHelper item = new RCActionHelper(2, 11, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionX()"))
						{
							RCActionHelper item = new RCActionHelper(2, 14, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionY()"))
						{
							RCActionHelper item = new RCActionHelper(2, 15, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionZ()"))
						{
							RCActionHelper item = new RCActionHelper(2, 16, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetName()"))
						{
							RCActionHelper item = new RCActionHelper(2, 12, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetGuildName()"))
						{
							RCActionHelper item = new RCActionHelper(2, 13, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("GetSpeed()"))
						{
							RCActionHelper item = new RCActionHelper(2, 17, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					case 5:
						if (text.StartsWith("GetType()"))
						{
							RCActionHelper item = new RCActionHelper(3, 0, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetSize()"))
						{
							RCActionHelper item = new RCActionHelper(3, 1, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetHealth()"))
						{
							RCActionHelper item = new RCActionHelper(3, 2, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("GetPositionX()"))
						{
							RCActionHelper item = new RCActionHelper(3, 3, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionY()"))
						{
							RCActionHelper item = new RCActionHelper(3, 4, null);
							list.Add(item);
							sentType = 3;
						}
						else if (text.StartsWith("GetPositionZ()"))
						{
							RCActionHelper item = new RCActionHelper(3, 5, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					default:
						if (text.StartsWith("ConvertToInt()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 0;
						}
						else if (text.StartsWith("ConvertToBool()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 1;
						}
						else if (text.StartsWith("ConvertToString()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 2;
						}
						else if (text.StartsWith("ConvertToFloat()"))
						{
							RCActionHelper item = new RCActionHelper(5, sentType, null);
							list.Add(item);
							sentType = 3;
						}
						break;
					}
				}
				else if (text.StartsWith("ConvertToInt()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 0;
				}
				else if (text.StartsWith("ConvertToBool()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 1;
				}
				else if (text.StartsWith("ConvertToString()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 2;
				}
				else if (text.StartsWith("ConvertToFloat()"))
				{
					RCActionHelper item = new RCActionHelper(5, sentType, null);
					list.Add(item);
					sentType = 3;
				}
			}
		}
		for (int i = list.Count - 1; i > 0; i--)
		{
			list[i - 1].setNextHelper(list[i]);
		}
		return list[0];
	}

	public static PeerStates returnPeerState(int peerstate)
	{
		return peerstate switch
		{
			0 => PeerStates.Authenticated, 
			1 => PeerStates.ConnectedToMaster, 
			2 => PeerStates.DisconnectingFromMasterserver, 
			3 => PeerStates.DisconnectingFromGameserver, 
			4 => PeerStates.DisconnectingFromNameServer, 
			_ => PeerStates.ConnectingToMasterserver, 
		};
	}

	[RPC]
	private void RPCLoadLevel(PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			this.DestroyAllExistingCloths();
			PhotonNetwork.LoadLevel(LevelInfo.getInfo(FengGameManagerMKII.level).mapName);
		}
		else if (PhotonNetwork.isMasterClient)
		{
			this.kickPlayerRC(info.sender, ban: true, "false restart.");
		}
		else
		{
			if (FengGameManagerMKII.masterRC)
			{
				return;
			}
			this.restartCount.Add(Time.time);
			foreach (float item in this.restartCount)
			{
				if (Time.time - item > 60f)
				{
					this.restartCount.Remove(item);
				}
			}
			if (this.restartCount.Count < 6)
			{
				this.DestroyAllExistingCloths();
				PhotonNetwork.LoadLevel(LevelInfo.getInfo(FengGameManagerMKII.level).mapName);
			}
		}
	}

	public void sendChatContentInfo(string content)
	{
		object[] parameters = new object[2]
		{
			content,
			string.Empty
		};
		base.photonView.RPC("Chat", PhotonTargets.All, parameters);
	}

	public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg = 0)
	{
		object[] parameters = new object[5] { t1, killer, t2, victim, dmg };
		base.photonView.RPC("updateKillInfo", PhotonTargets.All, parameters);
	}

	public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName = null)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
		raiseEventOptions.TargetActors = new int[1] { targetPlayer.ID };
		RaiseEventOptions options = raiseEventOptions;
		if (requestIpBan)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = true;
			if (inGameName != null && inGameName.Length > 0)
			{
				hashtable[(byte)1] = inGameName;
			}
			PhotonNetwork.RaiseEvent(203, hashtable, sendReliable: true, options);
		}
		else
		{
			PhotonNetwork.RaiseEvent(203, null, sendReliable: true, options);
		}
	}

	public static void ServerRequestAuthentication(string authPassword)
	{
		if (!string.IsNullOrEmpty(authPassword))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = authPassword;
			PhotonNetwork.RaiseEvent(198, hashtable, sendReliable: true, new RaiseEventOptions());
		}
	}

	public static void ServerRequestUnban(string bannedAddress)
	{
		if (!string.IsNullOrEmpty(bannedAddress))
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[(byte)0] = bannedAddress;
			PhotonNetwork.RaiseEvent(199, hashtable, sendReliable: true, new RaiseEventOptions());
		}
	}

	private void setGameSettings(ExitGames.Client.Photon.Hashtable hash)
	{
		this.restartingEren = false;
		this.restartingBomb = false;
		this.restartingHorse = false;
		this.restartingTitan = false;
		LegacyGameSettings legacyGameSettings = SettingsManager.LegacyGameSettings;
		if (hash.ContainsKey("bomb"))
		{
			if (!legacyGameSettings.BombModeEnabled.Value)
			{
				legacyGameSettings.BombModeEnabled.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode enabled.</color>");
			}
		}
		else if (legacyGameSettings.BombModeEnabled.Value)
		{
			legacyGameSettings.BombModeEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>PVP Bomb Mode disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				this.restartingBomb = true;
			}
		}
		if (legacyGameSettings.BombModeEnabled.Value && (!hash.ContainsKey("bombCeiling") || (int)hash["bombCeiling"] == 1))
		{
			MapCeiling.CreateMapCeiling();
		}
		if (!hash.ContainsKey("bombInfiniteGas") || (int)hash["bombInfiniteGas"] == 1)
		{
			legacyGameSettings.BombModeInfiniteGas.Value = true;
		}
		else
		{
			legacyGameSettings.BombModeInfiniteGas.Value = false;
		}
		legacyGameSettings.GlobalHideNames.Value = hash.ContainsKey("globalHideNames");
		if (hash.ContainsKey("globalDisableMinimap"))
		{
			if (!legacyGameSettings.GlobalMinimapDisable.Value)
			{
				legacyGameSettings.GlobalMinimapDisable.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>Minimaps are not allowed.</color>");
			}
		}
		else if (legacyGameSettings.GlobalMinimapDisable.Value)
		{
			legacyGameSettings.GlobalMinimapDisable.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Minimaps are allowed.</color>");
		}
		if (hash.ContainsKey("globalDisableMinimap"))
		{
			if (!legacyGameSettings.GlobalMinimapDisable.Value)
			{
				legacyGameSettings.GlobalMinimapDisable.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>Minimaps are not allowed.</color>");
			}
		}
		else if (legacyGameSettings.GlobalMinimapDisable.Value)
		{
			legacyGameSettings.GlobalMinimapDisable.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Minimaps are allowed.</color>");
		}
		if (hash.ContainsKey("horse"))
		{
			if (!legacyGameSettings.AllowHorses.Value)
			{
				legacyGameSettings.AllowHorses.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>Horses enabled.</color>");
			}
		}
		else if (legacyGameSettings.AllowHorses.Value)
		{
			legacyGameSettings.AllowHorses.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Horses disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				this.restartingHorse = true;
			}
		}
		if (hash.ContainsKey("punkWaves"))
		{
			if (legacyGameSettings.PunksEveryFive.Value)
			{
				legacyGameSettings.PunksEveryFive.Value = false;
				this.chatRoom.addLINE("<color=#FFCC00>Punks every 5 waves disabled.</color>");
			}
		}
		else if (!legacyGameSettings.PunksEveryFive.Value)
		{
			legacyGameSettings.PunksEveryFive.Value = true;
			this.chatRoom.addLINE("<color=#FFCC00>Punks ever 5 waves enabled.</color>");
		}
		if (hash.ContainsKey("ahssReload"))
		{
			if (legacyGameSettings.AHSSAirReload.Value)
			{
				legacyGameSettings.AHSSAirReload.Value = false;
				this.chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
			}
		}
		else if (!legacyGameSettings.AHSSAirReload.Value)
		{
			legacyGameSettings.AHSSAirReload.Value = true;
			this.chatRoom.addLINE("<color=#FFCC00>AHSS Air-Reload allowed.</color>");
		}
		if (hash.ContainsKey("team"))
		{
			if (legacyGameSettings.TeamMode.Value != (int)hash["team"])
			{
				legacyGameSettings.TeamMode.Value = (int)hash["team"];
				string text = string.Empty;
				if (legacyGameSettings.TeamMode.Value == 1)
				{
					text = "no sort";
				}
				else if (legacyGameSettings.TeamMode.Value == 2)
				{
					text = "locked by size";
				}
				else if (legacyGameSettings.TeamMode.Value == 3)
				{
					text = "locked by skill";
				}
				this.chatRoom.addLINE("<color=#FFCC00>Team Mode enabled (" + text + ").</color>");
				if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 0)
				{
					this.setTeam(3);
				}
			}
		}
		else if (legacyGameSettings.TeamMode.Value != 0)
		{
			legacyGameSettings.TeamMode.Value = 0;
			this.setTeam(0);
			this.chatRoom.addLINE("<color=#FFCC00>Team mode disabled.</color>");
		}
		if (hash.ContainsKey("point"))
		{
			if (!legacyGameSettings.PointModeEnabled.Value || legacyGameSettings.PointModeAmount.Value != (int)hash["point"])
			{
				legacyGameSettings.PointModeEnabled.Value = true;
				legacyGameSettings.PointModeAmount.Value = (int)hash["point"];
				this.chatRoom.addLINE("<color=#FFCC00>Point limit enabled (" + Convert.ToString(legacyGameSettings.PointModeAmount.Value) + ").</color>");
			}
		}
		else if (legacyGameSettings.PointModeEnabled.Value)
		{
			legacyGameSettings.PointModeEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Point limit disabled.</color>");
		}
		if (hash.ContainsKey("rock"))
		{
			if (legacyGameSettings.RockThrowEnabled.Value)
			{
				legacyGameSettings.RockThrowEnabled.Value = false;
				this.chatRoom.addLINE("<color=#FFCC00>Punk rock throwing disabled.</color>");
			}
		}
		else if (!legacyGameSettings.RockThrowEnabled.Value)
		{
			legacyGameSettings.RockThrowEnabled.Value = true;
			this.chatRoom.addLINE("<color=#FFCC00>Punk rock throwing enabled.</color>");
		}
		if (hash.ContainsKey("explode"))
		{
			if (!legacyGameSettings.TitanExplodeEnabled.Value || legacyGameSettings.TitanExplodeRadius.Value != (int)hash["explode"])
			{
				legacyGameSettings.TitanExplodeEnabled.Value = true;
				legacyGameSettings.TitanExplodeRadius.Value = (int)hash["explode"];
				this.chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode enabled (Radius " + Convert.ToString(legacyGameSettings.TitanExplodeRadius.Value) + ").</color>");
			}
		}
		else if (legacyGameSettings.TitanExplodeEnabled.Value)
		{
			legacyGameSettings.TitanExplodeEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Titan Explode Mode disabled.</color>");
		}
		if (hash.ContainsKey("healthMode") && hash.ContainsKey("healthLower") && hash.ContainsKey("healthUpper"))
		{
			if (legacyGameSettings.TitanHealthMode.Value != (int)hash["healthMode"] || legacyGameSettings.TitanHealthMin.Value != (int)hash["healthLower"] || legacyGameSettings.TitanHealthMax.Value != (int)hash["healthUpper"])
			{
				legacyGameSettings.TitanHealthMode.Value = (int)hash["healthMode"];
				legacyGameSettings.TitanHealthMin.Value = (int)hash["healthLower"];
				legacyGameSettings.TitanHealthMax.Value = (int)hash["healthUpper"];
				string text = "Static";
				if (legacyGameSettings.TitanHealthMode.Value == 2)
				{
					text = "Scaled";
				}
				this.chatRoom.addLINE("<color=#FFCC00>Titan Health (" + text + ", " + legacyGameSettings.TitanHealthMin.Value + " to " + legacyGameSettings.TitanHealthMax.Value + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanHealthMode.Value > 0)
		{
			legacyGameSettings.TitanHealthMode.Value = 0;
			this.chatRoom.addLINE("<color=#FFCC00>Titan Health disabled.</color>");
		}
		if (hash.ContainsKey("infection"))
		{
			if (!legacyGameSettings.InfectionModeEnabled.Value)
			{
				legacyGameSettings.InfectionModeEnabled.Value = true;
				legacyGameSettings.InfectionModeAmount.Value = (int)hash["infection"];
				this.name = LoginFengKAI.player.name;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.RCteam, 0);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				this.chatRoom.addLINE("<color=#FFCC00>Infection mode (" + Convert.ToString(legacyGameSettings.InfectionModeAmount.Value) + ") enabled. Make sure your first character is human.</color>");
			}
		}
		else if (legacyGameSettings.InfectionModeEnabled.Value)
		{
			legacyGameSettings.InfectionModeEnabled.Value = false;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.isTitan, 1);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			this.chatRoom.addLINE("<color=#FFCC00>Infection Mode disabled.</color>");
			if (PhotonNetwork.isMasterClient)
			{
				this.restartingTitan = true;
			}
		}
		if (hash.ContainsKey("eren"))
		{
			if (!legacyGameSettings.KickShifters.Value)
			{
				legacyGameSettings.KickShifters.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>Anti-Eren enabled. Using eren transform will get you kicked.</color>");
				if (PhotonNetwork.isMasterClient)
				{
					this.restartingEren = true;
				}
			}
		}
		else if (legacyGameSettings.KickShifters.Value)
		{
			legacyGameSettings.KickShifters.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Anti-Eren disabled. Eren transform is allowed.</color>");
		}
		if (hash.ContainsKey("titanc"))
		{
			if (!legacyGameSettings.TitanNumberEnabled.Value || legacyGameSettings.TitanNumber.Value != (int)hash["titanc"])
			{
				legacyGameSettings.TitanNumberEnabled.Value = true;
				legacyGameSettings.TitanNumber.Value = (int)hash["titanc"];
				this.chatRoom.addLINE("<color=#FFCC00>" + Convert.ToString(legacyGameSettings.TitanNumber.Value) + " titans will spawn each round.</color>");
			}
		}
		else if (legacyGameSettings.TitanNumberEnabled.Value)
		{
			legacyGameSettings.TitanNumberEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Default titans will spawn each round.</color>");
		}
		if (hash.ContainsKey("damage"))
		{
			if (!legacyGameSettings.TitanArmorEnabled.Value || legacyGameSettings.TitanArmor.Value != (int)hash["damage"])
			{
				legacyGameSettings.TitanArmorEnabled.Value = true;
				legacyGameSettings.TitanArmor.Value = (int)hash["damage"];
				this.chatRoom.addLINE("<color=#FFCC00>Nape minimum damage (" + Convert.ToString(legacyGameSettings.TitanArmor.Value) + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanArmorEnabled.Value)
		{
			legacyGameSettings.TitanArmorEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Nape minimum damage disabled.</color>");
		}
		if (hash.ContainsKey("sizeMode") && hash.ContainsKey("sizeLower") && hash.ContainsKey("sizeUpper"))
		{
			if (!legacyGameSettings.TitanSizeEnabled.Value || legacyGameSettings.TitanSizeMin.Value != (float)hash["sizeLower"] || legacyGameSettings.TitanSizeMax.Value != (float)hash["sizeUpper"])
			{
				legacyGameSettings.TitanSizeEnabled.Value = true;
				legacyGameSettings.TitanSizeMin.Value = (float)hash["sizeLower"];
				legacyGameSettings.TitanSizeMax.Value = (float)hash["sizeUpper"];
				this.chatRoom.addLINE("<color=#FFCC00>Custom titan size (" + legacyGameSettings.TitanSizeMin.Value.ToString("F2") + "," + legacyGameSettings.TitanSizeMax.Value.ToString("F2") + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanSizeEnabled.Value)
		{
			legacyGameSettings.TitanSizeEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Custom titan size disabled.</color>");
		}
		if (hash.ContainsKey("spawnMode") && hash.ContainsKey("nRate") && hash.ContainsKey("aRate") && hash.ContainsKey("jRate") && hash.ContainsKey("cRate") && hash.ContainsKey("pRate"))
		{
			if (!legacyGameSettings.TitanSpawnEnabled.Value || legacyGameSettings.TitanSpawnNormal.Value != (float)hash["nRate"] || legacyGameSettings.TitanSpawnAberrant.Value != (float)hash["aRate"] || legacyGameSettings.TitanSpawnJumper.Value != (float)hash["jRate"] || legacyGameSettings.TitanSpawnCrawler.Value != (float)hash["cRate"] || legacyGameSettings.TitanSpawnPunk.Value != (float)hash["pRate"])
			{
				legacyGameSettings.TitanSpawnEnabled.Value = true;
				legacyGameSettings.TitanSpawnNormal.Value = (float)hash["nRate"];
				legacyGameSettings.TitanSpawnAberrant.Value = (float)hash["aRate"];
				legacyGameSettings.TitanSpawnJumper.Value = (float)hash["jRate"];
				legacyGameSettings.TitanSpawnCrawler.Value = (float)hash["cRate"];
				legacyGameSettings.TitanSpawnPunk.Value = (float)hash["pRate"];
				this.chatRoom.addLINE("<color=#FFCC00>Custom spawn rate enabled (" + legacyGameSettings.TitanSpawnNormal.Value.ToString("F2") + "% Normal, " + legacyGameSettings.TitanSpawnAberrant.Value.ToString("F2") + "% Abnormal, " + legacyGameSettings.TitanSpawnJumper.Value.ToString("F2") + "% Jumper, " + legacyGameSettings.TitanSpawnCrawler.Value.ToString("F2") + "% Crawler, " + legacyGameSettings.TitanSpawnPunk.Value.ToString("F2") + "% Punk </color>");
			}
		}
		else if (legacyGameSettings.TitanSpawnEnabled.Value)
		{
			legacyGameSettings.TitanSpawnEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Custom spawn rate disabled.</color>");
		}
		if (hash.ContainsKey("waveModeOn") && hash.ContainsKey("waveModeNum"))
		{
			if (!legacyGameSettings.TitanPerWavesEnabled.Value || legacyGameSettings.TitanPerWaves.Value != (int)hash["waveModeNum"])
			{
				legacyGameSettings.TitanPerWavesEnabled.Value = true;
				legacyGameSettings.TitanPerWaves.Value = (int)hash["waveModeNum"];
				this.chatRoom.addLINE("<color=#FFCC00>Custom wave mode (" + legacyGameSettings.TitanPerWaves.Value + ") enabled.</color>");
			}
		}
		else if (legacyGameSettings.TitanPerWavesEnabled.Value)
		{
			legacyGameSettings.TitanPerWavesEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Custom wave mode disabled.</color>");
		}
		if (hash.ContainsKey("friendly"))
		{
			if (!legacyGameSettings.FriendlyMode.Value)
			{
				legacyGameSettings.FriendlyMode.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>PVP is prohibited.</color>");
			}
		}
		else if (legacyGameSettings.FriendlyMode.Value)
		{
			legacyGameSettings.FriendlyMode.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>PVP is allowed.</color>");
		}
		if (hash.ContainsKey("pvp"))
		{
			if (legacyGameSettings.BladePVP.Value != (int)hash["pvp"])
			{
				legacyGameSettings.BladePVP.Value = (int)hash["pvp"];
				string text = string.Empty;
				if (legacyGameSettings.BladePVP.Value == 1)
				{
					text = "Team-Based";
				}
				else if (legacyGameSettings.BladePVP.Value == 2)
				{
					text = "FFA";
				}
				this.chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP enabled (" + text + ").</color>");
			}
		}
		else if (legacyGameSettings.BladePVP.Value != 0)
		{
			legacyGameSettings.BladePVP.Value = 0;
			this.chatRoom.addLINE("<color=#FFCC00>Blade/AHSS PVP disabled.</color>");
		}
		if (hash.ContainsKey("maxwave"))
		{
			if (!legacyGameSettings.TitanMaxWavesEnabled.Value || legacyGameSettings.TitanMaxWaves.Value != (int)hash["maxwave"])
			{
				legacyGameSettings.TitanMaxWavesEnabled.Value = true;
				legacyGameSettings.TitanMaxWaves.Value = (int)hash["maxwave"];
				this.chatRoom.addLINE("<color=#FFCC00>Max wave is " + legacyGameSettings.TitanMaxWaves.Value + ".</color>");
			}
		}
		else if (legacyGameSettings.TitanMaxWavesEnabled.Value)
		{
			legacyGameSettings.TitanMaxWavesEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Max wave set to default.</color>");
		}
		if (hash.ContainsKey("endless"))
		{
			if (!legacyGameSettings.EndlessRespawnEnabled.Value || legacyGameSettings.EndlessRespawnTime.Value != (int)hash["endless"])
			{
				legacyGameSettings.EndlessRespawnEnabled.Value = true;
				legacyGameSettings.EndlessRespawnTime.Value = (int)hash["endless"];
				this.chatRoom.addLINE("<color=#FFCC00>Endless respawn enabled (" + legacyGameSettings.EndlessRespawnTime.Value + " seconds).</color>");
			}
		}
		else if (legacyGameSettings.EndlessRespawnEnabled.Value)
		{
			legacyGameSettings.EndlessRespawnEnabled.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Endless respawn disabled.</color>");
		}
		if (hash.ContainsKey("motd"))
		{
			if (legacyGameSettings.Motd.Value != (string)hash["motd"])
			{
				legacyGameSettings.Motd.Value = (string)hash["motd"];
				this.chatRoom.addLINE("<color=#FFCC00>MOTD:" + legacyGameSettings.Motd.Value + "</color>");
			}
		}
		else if (legacyGameSettings.Motd.Value != string.Empty)
		{
			legacyGameSettings.Motd.Value = string.Empty;
		}
		if (hash.ContainsKey("deadlycannons"))
		{
			if (!legacyGameSettings.CannonsFriendlyFire.Value)
			{
				legacyGameSettings.CannonsFriendlyFire.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>Cannons will now kill players.</color>");
			}
		}
		else if (legacyGameSettings.CannonsFriendlyFire.Value)
		{
			legacyGameSettings.CannonsFriendlyFire.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Cannons will no longer kill players.</color>");
		}
		if (hash.ContainsKey("asoracing"))
		{
			if (!legacyGameSettings.RacingEndless.Value)
			{
				legacyGameSettings.RacingEndless.Value = true;
				this.chatRoom.addLINE("<color=#FFCC00>Racing will not restart on win.</color>");
			}
		}
		else if (legacyGameSettings.RacingEndless.Value)
		{
			legacyGameSettings.RacingEndless.Value = false;
			this.chatRoom.addLINE("<color=#FFCC00>Racing will restart on win.</color>");
		}
		if (hash.ContainsKey("racingStartTime"))
		{
			legacyGameSettings.RacingStartTime.Value = (float)hash["racingStartTime"];
		}
		else
		{
			legacyGameSettings.RacingStartTime.Value = 20f;
		}
		foreach (HERO hero in this.heroes)
		{
			if (hero != null)
			{
				hero.SetName();
			}
		}
	}

	private IEnumerator setGuildFeng()
	{
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("name", LoginFengKAI.player.name);
		wWWForm.AddField("guildname", LoginFengKAI.player.guildname);
		yield return (!Application.isWebPlayer) ? new WWW("http://fenglee.com/game/aog/change_guild_name.php", wWWForm) : new WWW("http://aotskins.com/version/guild.php", wWWForm);
	}

	[RPC]
	private void setMasterRC(PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			FengGameManagerMKII.masterRC = true;
		}
	}

	private void setTeam(int setting)
	{
		switch (setting)
		{
		case 0:
		{
			this.name = LoginFengKAI.player.name;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.RCteam, 0);
			hashtable.Add(PhotonPlayerProperty.name, this.name);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			break;
		}
		case 1:
		{
			ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
			hashtable3.Add(PhotonPlayerProperty.RCteam, 1);
			string text2 = LoginFengKAI.player.name;
			while (text2.Contains("[") && text2.Length >= text2.IndexOf("[") + 8)
			{
				int startIndex2 = text2.IndexOf("[");
				text2 = text2.Remove(startIndex2, 8);
			}
			if (!text2.StartsWith("[00FFFF]"))
			{
				text2 = "[00FFFF]" + text2;
			}
			this.name = text2;
			hashtable3.Add(PhotonPlayerProperty.name, this.name);
			PhotonNetwork.player.SetCustomProperties(hashtable3);
			break;
		}
		case 2:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
			hashtable2.Add(PhotonPlayerProperty.RCteam, 2);
			string text = LoginFengKAI.player.name;
			while (text.Contains("[") && text.Length >= text.IndexOf("[") + 8)
			{
				int startIndex = text.IndexOf("[");
				text = text.Remove(startIndex, 8);
			}
			if (!text.StartsWith("[FF00FF]"))
			{
				text = "[FF00FF]" + text;
			}
			this.name = text;
			hashtable2.Add(PhotonPlayerProperty.name, this.name);
			PhotonNetwork.player.SetCustomProperties(hashtable2);
			break;
		}
		case 3:
		{
			int num = 0;
			int num2 = 0;
			int team = 1;
			PhotonPlayer[] array = PhotonNetwork.playerList;
			for (int i = 0; i < array.Length; i++)
			{
				switch (RCextensions.returnIntFromObject(array[i].customProperties[PhotonPlayerProperty.RCteam]))
				{
				case 1:
					num++;
					break;
				case 2:
					num2++;
					break;
				}
			}
			if (num > num2)
			{
				team = 2;
			}
			this.setTeam(team);
			break;
		}
		}
		if (setting != 0 && setting != 1 && setting != 2)
		{
			return;
		}
		GameObject[] array2 = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject go in array2)
		{
			if (go.GetPhotonView().isMine)
			{
				base.photonView.RPC("labelRPC", PhotonTargets.All, go.GetPhotonView().viewID);
			}
		}
	}

	[RPC]
	private void setTeamRPC(int setting, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient || info.sender.isLocal)
		{
			this.setTeam(setting);
		}
	}

	[RPC]
	private void settingRPC(ExitGames.Client.Photon.Hashtable hash, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			this.setGameSettings(hash);
		}
	}

	private void showChatContent(string content)
	{
		this.chatContent.Add(content);
		if (this.chatContent.Count > 10)
		{
			this.chatContent.RemoveAt(0);
		}
		GameObject.Find("LabelChatContent").GetComponent<UILabel>().text = string.Empty;
		for (int i = 0; i < this.chatContent.Count; i++)
		{
			GameObject.Find("LabelChatContent").GetComponent<UILabel>().text += this.chatContent[i];
		}
	}

	public void ShowHUDInfoCenter(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	public void ShowHUDInfoCenterADD(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	private void ShowHUDInfoTopCenter(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void ShowHUDInfoTopCenterADD(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	private void ShowHUDInfoTopLeft(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopLeft");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void ShowHUDInfoTopRight(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopRight");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void ShowHUDInfoTopRightMAPNAME(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopRight");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text += content;
		}
	}

	[RPC]
	private void showResult(string text0, string text1, string text2, string text3, string text4, string text6, PhotonMessageInfo t)
	{
		if (!this.gameTimesUp && t.sender.isMasterClient)
		{
			this.gameTimesUp = true;
			GameObject obj = GameObject.Find("UI_IN_GAME");
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[0], state: false);
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[1], state: false);
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[2], state: true);
			NGUITools.SetActive(obj.GetComponent<UIReferArray>().panels[3], state: false);
			GameObject.Find("LabelName").GetComponent<UILabel>().text = text0;
			GameObject.Find("LabelKill").GetComponent<UILabel>().text = text1;
			GameObject.Find("LabelDead").GetComponent<UILabel>().text = text2;
			GameObject.Find("LabelMaxDmg").GetComponent<UILabel>().text = text3;
			GameObject.Find("LabelTotalDmg").GetComponent<UILabel>().text = text4;
			GameObject.Find("LabelResultTitle").GetComponent<UILabel>().text = text6;
			IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
			this.gameStart = false;
		}
		else if (!t.sender.isMasterClient && PhotonNetwork.player.isMasterClient)
		{
			this.kickPlayerRC(t.sender, ban: true, "false game end.");
		}
	}

	private void SingleShowHUDInfoTopCenter(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopCenter");
		if (gameObject != null)
		{
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	private void SingleShowHUDInfoTopLeft(string content)
	{
		GameObject gameObject = GameObject.Find("LabelInfoTopLeft");
		if (gameObject != null)
		{
			content = content.Replace("[0]", "[*^_^*]");
			gameObject.GetComponent<UILabel>().text = content;
		}
	}

	[RPC]
	public void someOneIsDead(int id = -1)
	{
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (id != 0)
			{
				this.PVPtitanScore += 2;
			}
			this.checkPVPpts();
			object[] parameters = new object[2] { this.PVPhumanScore, this.PVPtitanScore };
			base.photonView.RPC("refreshPVPStatus", PhotonTargets.Others, parameters);
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN)
		{
			this.titanScore++;
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.KILL_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST)
		{
			if (this.isPlayerAllDead2())
			{
				this.gameLose2();
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS && SettingsManager.LegacyGameSettings.BladePVP.Value == 0 && !SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
		{
			if (this.isPlayerAllDead2())
			{
				this.gameLose2();
				this.teamWinner = 0;
			}
			if (this.isTeamAllDead2(1))
			{
				this.teamWinner = 2;
				this.gameWin2();
			}
			if (this.isTeamAllDead2(2))
			{
				this.teamWinner = 1;
				this.gameWin2();
			}
		}
	}

	public void SpawnNonAITitan(string id, string tag = "titanRespawn")
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject gameObject = array[UnityEngine.Random.Range(0, array.Length)];
		this.myLastHero = id.ToUpper();
		GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE) ? PhotonNetwork.Instantiate("TITAN_VER3.1", gameObject.transform.position, gameObject.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", this.checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20)), this.checkpoint.transform.rotation, 0));
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(gameObject2);
		gameObject2.GetComponent<TITAN>().nonAI = true;
		gameObject2.GetComponent<TITAN>().speed = 30f;
		gameObject2.GetComponent<TITAN_CONTROLLER>().enabled = true;
		if (id == "RANDOM" && UnityEngine.Random.Range(0, 100) < 7)
		{
			gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
		}
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
		GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
		GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			2
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		this.ShowHUDInfoCenter(string.Empty);
	}

	public void SpawnNonAITitan2(string id, string tag = "titanRespawn")
	{
		if (FengGameManagerMKII.logicLoaded && FengGameManagerMKII.customLevelLoaded)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
			GameObject gameObject = array[UnityEngine.Random.Range(0, array.Length)];
			Vector3 position = gameObject.transform.position;
			if (FengGameManagerMKII.level.StartsWith("Custom") && this.titanSpawns.Count > 0)
			{
				position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
			}
			this.myLastHero = id.ToUpper();
			GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE) ? PhotonNetwork.Instantiate("TITAN_VER3.1", position, gameObject.transform.rotation, 0) : PhotonNetwork.Instantiate("TITAN_VER3.1", this.checkpoint.transform.position + new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20)), this.checkpoint.transform.rotation, 0));
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(gameObject2);
			gameObject2.GetComponent<TITAN>().nonAI = true;
			gameObject2.GetComponent<TITAN>().speed = 30f;
			gameObject2.GetComponent<TITAN_CONTROLLER>().enabled = true;
			if (id == "RANDOM" && UnityEngine.Random.Range(0, 100) < 7)
			{
				gameObject2.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
			}
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
			GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
			GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
			GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
			PhotonNetwork.player.SetCustomProperties(customProperties);
			customProperties = new ExitGames.Client.Photon.Hashtable { 
			{
				PhotonPlayerProperty.isTitan,
				2
			} };
			PhotonNetwork.player.SetCustomProperties(customProperties);
			this.ShowHUDInfoCenter(string.Empty);
		}
		else
		{
			this.NOTSpawnNonAITitanRC(id);
		}
	}

	public void SpawnPlayer(string id, string tag = "playerRespawn")
	{
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			this.SpawnPlayerAt2(id, this.checkpoint);
			return;
		}
		this.myLastRespawnTag = tag;
		GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
		GameObject pos = array[UnityEngine.Random.Range(0, array.Length)];
		this.SpawnPlayerAt2(id, pos);
	}

	public void SpawnPlayerAt2(string id, GameObject pos)
	{
		if (!FengGameManagerMKII.logicLoaded || !FengGameManagerMKII.customLevelLoaded)
		{
			this.NOTSpawnPlayerRC(id);
			return;
		}
		Vector3 position = pos.transform.position;
		if (this.racingSpawnPointSet)
		{
			position = this.racingSpawnPoint;
		}
		else if (FengGameManagerMKII.level.StartsWith("Custom"))
		{
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 0)
			{
				List<Vector3> list = new List<Vector3>();
				foreach (Vector3 item in this.playerSpawnsC)
				{
					list.Add(item);
				}
				foreach (Vector3 item2 in this.playerSpawnsM)
				{
					list.Add(item2);
				}
				if (list.Count > 0)
				{
					position = list[UnityEngine.Random.Range(0, list.Count)];
				}
			}
			else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
			{
				if (this.playerSpawnsC.Count > 0)
				{
					position = this.playerSpawnsC[UnityEngine.Random.Range(0, this.playerSpawnsC.Count)];
				}
			}
			else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2 && this.playerSpawnsM.Count > 0)
			{
				position = this.playerSpawnsM[UnityEngine.Random.Range(0, this.playerSpawnsM.Count)];
			}
		}
		IN_GAME_MAIN_CAMERA component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
		this.myLastHero = id.ToUpper();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (IN_GAME_MAIN_CAMERA.singleCharacter == "TITAN_EREN")
			{
				component.setMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation));
			}
			else
			{
				component.setMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation));
				if (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 1" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 2" || IN_GAME_MAIN_CAMERA.singleCharacter == "SET 3")
				{
					HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(IN_GAME_MAIN_CAMERA.singleCharacter);
					heroCostume.checkstat();
					CostumeConeveter.HeroCostumeToLocalData(heroCostume, IN_GAME_MAIN_CAMERA.singleCharacter);
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
					if (heroCostume != null)
					{
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = heroCostume.stat;
					}
					else
					{
						heroCostume = HeroCostume.costumeOption[3];
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume.name.ToUpper());
					}
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
					component.main_object.GetComponent<HERO>().setStat2();
					component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
				}
				else
				{
					for (int i = 0; i < HeroCostume.costume.Length; i++)
					{
						if (HeroCostume.costume[i].name.ToUpper() == IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper())
						{
							int num = HeroCostume.costume[i].id + CheckBoxCostume.costumeSet - 1;
							if (HeroCostume.costume[num].name != HeroCostume.costume[i].name)
							{
								num = HeroCostume.costume[i].id + 1;
							}
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num];
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num].name.ToUpper());
							component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
							component.main_object.GetComponent<HERO>().setStat2();
							component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
							break;
						}
					}
				}
			}
		}
		else
		{
			component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, pos.transform.rotation, 0));
			id = id.ToUpper();
			switch (id)
			{
			case "SET 1":
			case "SET 2":
			case "SET 3":
			{
				HeroCostume heroCostume2 = CostumeConeveter.LocalDataToHeroCostume(id);
				heroCostume2.checkstat();
				CostumeConeveter.HeroCostumeToLocalData(heroCostume2, id);
				if (heroCostume2.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
				{
					heroCostume2 = HeroCostume.costume[6];
				}
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
				if (heroCostume2 != null)
				{
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume2;
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = heroCostume2.stat;
				}
				else
				{
					heroCostume2 = HeroCostume.costumeOption[3];
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume2;
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume2.name.ToUpper());
				}
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
				component.main_object.GetComponent<HERO>().setStat2();
				component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
				break;
			}
			default:
			{
				for (int j = 0; j < HeroCostume.costume.Length; j++)
				{
					if (HeroCostume.costume[j].name.ToUpper() == id.ToUpper())
					{
						int num2 = HeroCostume.costume[j].id;
						if (id.ToUpper() != "AHSS")
						{
							num2 += CheckBoxCostume.costumeSet - 1;
						}
						if (HeroCostume.costume[num2].name != HeroCostume.costume[j].name)
						{
							num2 = HeroCostume.costume[j].id + 1;
						}
						if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && id.ToUpper() == "AHSS")
						{
							num2 = 6;
						}
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num2];
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num2].name.ToUpper());
						component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
						component.main_object.GetComponent<HERO>().setStat2();
						component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
						break;
					}
				}
				break;
			}
			}
			CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
			if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
			{
				component.main_object.transform.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
			}
			ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
			PhotonNetwork.player.SetCustomProperties(customProperties);
			customProperties = new ExitGames.Client.Photon.Hashtable { 
			{
				PhotonPlayerProperty.isTitan,
				1
			} };
			PhotonNetwork.player.SetCustomProperties(customProperties);
		}
		component.enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
		GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
		GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
		component.gameOver = false;
		this.isLosing = false;
		this.ShowHUDInfoCenter(string.Empty);
	}

	[RPC]
	public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient || !FengGameManagerMKII.logicLoaded || !FengGameManagerMKII.customLevelLoaded || this.needChooseSide || !Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
		{
			return;
		}
		Vector3 position = new Vector3(posX, posY, posZ);
		IN_GAME_MAIN_CAMERA component = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
		component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0f, 0f, 0f, 1f), 0));
		string text = this.myLastHero.ToUpper();
		switch (text)
		{
		case "SET 1":
		case "SET 2":
		case "SET 3":
		{
			HeroCostume heroCostume = CostumeConeveter.LocalDataToHeroCostume(text);
			heroCostume.checkstat();
			CostumeConeveter.HeroCostumeToLocalData(heroCostume, text);
			if (heroCostume.uniform_type == UNIFORM_TYPE.CasualAHSS && SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
			{
				heroCostume = HeroCostume.costume[6];
			}
			component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
			if (heroCostume != null)
			{
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = heroCostume.stat;
			}
			else
			{
				heroCostume = HeroCostume.costumeOption[3];
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = heroCostume;
				component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(heroCostume.name.ToUpper());
			}
			component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
			component.main_object.GetComponent<HERO>().setStat2();
			component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
			break;
		}
		default:
		{
			for (int i = 0; i < HeroCostume.costume.Length; i++)
			{
				if (HeroCostume.costume[i].name.ToUpper() == text.ToUpper())
				{
					int num = HeroCostume.costume[i].id;
					if (text.ToUpper() != "AHSS")
					{
						num += CheckBoxCostume.costumeSet - 1;
					}
					if (HeroCostume.costume[num].name != HeroCostume.costume[i].name)
					{
						num = HeroCostume.costume[i].id + 1;
					}
					if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && text.ToUpper() == "AHSS")
					{
						num = 6;
					}
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().init();
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num];
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num].name.ToUpper());
					component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().setCharacterComponent();
					component.main_object.GetComponent<HERO>().setStat2();
					component.main_object.GetComponent<HERO>().setSkillHUDPosition2();
					break;
				}
			}
			break;
		}
		}
		CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<HERO>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			component.main_object.transform.position += new Vector3(UnityEngine.Random.Range(-20, 20), 2f, UnityEngine.Random.Range(-20, 20));
		}
		ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable { { "dead", false } };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		customProperties = new ExitGames.Client.Photon.Hashtable { 
		{
			PhotonPlayerProperty.isTitan,
			1
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
		component.enabled = true;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
		GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
		GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
		component.gameOver = false;
		this.isLosing = false;
		this.ShowHUDInfoCenter(string.Empty);
	}

	private void spawnPlayerCustomMap()
	{
		if (!this.needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
		{
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
			if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.isTitan]) == 2)
			{
				this.SpawnNonAITitan2(this.myLastHero);
			}
			else
			{
				this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
			}
			this.ShowHUDInfoCenter(string.Empty);
		}
	}

	public GameObject spawnTitan(int rate, Vector3 position, Quaternion rotation, bool punk = false)
	{
		GameObject gameObject = this.spawnTitanRaw(position, rotation);
		if (punk)
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
		}
		else if (UnityEngine.Random.Range(0, 100) < rate)
		{
			if (IN_GAME_MAIN_CAMERA.difficulty == 2)
			{
				if (UnityEngine.Random.Range(0f, 1f) < 0.7f || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
				{
					gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				}
				else
				{
					gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
				}
			}
		}
		else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.7f || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
			}
			else
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
			}
		}
		else if (UnityEngine.Random.Range(0, 100) < rate)
		{
			if (UnityEngine.Random.Range(0f, 1f) < 0.8f || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
			}
			else
			{
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
			}
		}
		else if (UnityEngine.Random.Range(0f, 1f) < 0.8f || LevelInfo.getInfo(FengGameManagerMKII.level).noCrawler)
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
		}
		else
		{
			gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
		}
		GameObject gameObject2 = ((IN_GAME_MAIN_CAMERA.gametype != 0) ? PhotonNetwork.Instantiate("FX/FXtitanSpawn", gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0) : ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanSpawn"), gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f))));
		gameObject2.transform.localScale = gameObject.transform.localScale;
		return gameObject;
	}

	public void spawnTitanAction(int type, float size, int health, int number)
	{
		Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
		Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
		if (this.titanSpawns.Count > 0)
		{
			position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
		}
		else
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
			if (array.Length != 0)
			{
				int num = UnityEngine.Random.Range(0, array.Length);
				GameObject obj = array[num];
				position = obj.transform.position;
				rotation = obj.transform.rotation;
			}
		}
		for (int i = 0; i < number; i++)
		{
			GameObject gameObject = this.spawnTitanRaw(position, rotation);
			gameObject.GetComponent<TITAN>().resetLevel(size);
			gameObject.GetComponent<TITAN>().hasSetLevel = true;
			if ((float)health > 0f)
			{
				gameObject.GetComponent<TITAN>().currentHealth = health;
				gameObject.GetComponent<TITAN>().maxHealth = health;
			}
			switch (type)
			{
			case 0:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
				break;
			case 1:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
				break;
			case 2:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				break;
			case 3:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
				break;
			case 4:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
				break;
			}
		}
	}

	public void spawnTitanAtAction(int type, float size, int health, int number, float posX, float posY, float posZ)
	{
		Vector3 position = new Vector3(posX, posY, posZ);
		Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
		for (int i = 0; i < number; i++)
		{
			GameObject gameObject = this.spawnTitanRaw(position, rotation);
			gameObject.GetComponent<TITAN>().resetLevel(size);
			gameObject.GetComponent<TITAN>().hasSetLevel = true;
			if ((float)health > 0f)
			{
				gameObject.GetComponent<TITAN>().currentHealth = health;
				gameObject.GetComponent<TITAN>().maxHealth = health;
			}
			switch (type)
			{
			case 0:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
				break;
			case 1:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
				break;
			case 2:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				break;
			case 3:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
				break;
			case 4:
				gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
				break;
			}
		}
	}

	public void spawnTitanCustom(string type, int abnormal, int rate, bool punk)
	{
		int num = rate;
		if (!SettingsManager.LegacyGameSettings.PunksEveryFive.Value)
		{
			punk = false;
		}
		if (FengGameManagerMKII.level.StartsWith("Custom"))
		{
			num = 5;
			if (SettingsManager.LegacyGameSettings.GameType.Value == 1)
			{
				num = 3;
			}
			else if (SettingsManager.LegacyGameSettings.GameType.Value == 2 || SettingsManager.LegacyGameSettings.GameType.Value == 3)
			{
				num = 0;
			}
		}
		if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value || (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value && FengGameManagerMKII.level.StartsWith("Custom") && SettingsManager.LegacyGameSettings.GameType.Value >= 2))
		{
			num = SettingsManager.LegacyGameSettings.TitanNumber.Value;
			if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
			{
				num = 0;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
		{
			if (punk)
			{
				num = rate;
			}
			else if (!SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
			{
				int num2 = 1;
				if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
				{
					num2 = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
				}
				num += (this.wave - 1) * (num2 - 1);
			}
			else if (SettingsManager.LegacyGameSettings.TitanNumberEnabled.Value)
			{
				int num2 = 1;
				if (SettingsManager.LegacyGameSettings.TitanPerWavesEnabled.Value)
				{
					num2 = SettingsManager.LegacyGameSettings.TitanPerWaves.Value;
				}
				num += (this.wave - 1) * num2;
			}
		}
		num = Math.Min(100, num);
		if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
		{
			float num3 = SettingsManager.LegacyGameSettings.TitanSpawnNormal.Value;
			float num4 = SettingsManager.LegacyGameSettings.TitanSpawnAberrant.Value;
			float num5 = SettingsManager.LegacyGameSettings.TitanSpawnJumper.Value;
			float num6 = SettingsManager.LegacyGameSettings.TitanSpawnCrawler.Value;
			float num7 = SettingsManager.LegacyGameSettings.TitanSpawnPunk.Value;
			if (punk)
			{
				num3 = 0f;
				num4 = 0f;
				num5 = 0f;
				num6 = 0f;
				num7 = 100f;
				num = rate;
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
			List<GameObject> list = new List<GameObject>(array);
			for (int i = 0; i < num; i++)
			{
				Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
				Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
				if (this.titanSpawns.Count > 0)
				{
					position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
				}
				else if (array.Length != 0)
				{
					if (list.Count <= 0)
					{
						list = new List<GameObject>(array);
					}
					int index = UnityEngine.Random.Range(0, list.Count);
					GameObject obj = list[index];
					position = obj.transform.position;
					rotation = obj.transform.rotation;
					list.RemoveAt(index);
				}
				float num8 = UnityEngine.Random.Range(0f, 100f);
				if (num8 <= num3 + num4 + num5 + num6 + num7)
				{
					GameObject gameObject = this.spawnTitanRaw(position, rotation);
					if (num8 < num3)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
					}
					else if (num8 >= num3 && num8 < num3 + num4)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
					}
					else if (num8 >= num3 + num4 && num8 < num3 + num4 + num5)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
					}
					else if (num8 >= num3 + num4 + num5 && num8 < num3 + num4 + num5 + num6)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: true);
					}
					else if (num8 >= num3 + num4 + num5 + num6 && num8 < num3 + num4 + num5 + num6 + num7)
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_PUNK, forceCrawler: false);
					}
					else
					{
						gameObject.GetComponent<TITAN>().setAbnormalType2(AbnormalType.NORMAL, forceCrawler: false);
					}
				}
				else
				{
					this.spawnTitan(abnormal, position, rotation, punk);
				}
			}
		}
		else if (FengGameManagerMKII.level.StartsWith("Custom"))
		{
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("titanRespawn");
			List<GameObject> list2 = new List<GameObject>(array2);
			for (int i = 0; i < num; i++)
			{
				Vector3 position = new Vector3(UnityEngine.Random.Range(-400f, 400f), 0f, UnityEngine.Random.Range(-400f, 400f));
				Quaternion rotation = new Quaternion(0f, 0f, 0f, 1f);
				if (this.titanSpawns.Count > 0)
				{
					position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
				}
				else if (array2.Length != 0)
				{
					if (list2.Count <= 0)
					{
						list2 = new List<GameObject>(array2);
					}
					int index2 = UnityEngine.Random.Range(0, list2.Count);
					GameObject obj2 = list2[index2];
					position = obj2.transform.position;
					rotation = obj2.transform.rotation;
					list2.RemoveAt(index2);
				}
				this.spawnTitan(abnormal, position, rotation, punk);
			}
		}
		else
		{
			this.randomSpawnTitan("titanRespawn", abnormal, num, punk);
		}
	}

	private GameObject spawnTitanRaw(Vector3 position, Quaternion rotation)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_VER3.1"), position, rotation);
		}
		return PhotonNetwork.Instantiate("TITAN_VER3.1", position, rotation, 0);
	}

	[RPC]
	private void spawnTitanRPC(PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient)
		{
			return;
		}
		foreach (TITAN titan in this.titans)
		{
			if (titan.photonView.isMine && (!PhotonNetwork.isMasterClient || titan.nonAI))
			{
				PhotonNetwork.Destroy(titan.gameObject);
			}
		}
		this.SpawnNonAITitan2(this.myLastHero);
	}

	private void Start()
	{
		FengGameManagerMKII.instance = this;
		base.gameObject.name = "MultiplayerManager";
		HeroCostume.init2();
		CharacterMaterials.init();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.heroes = new ArrayList();
		this.eT = new ArrayList();
		this.titans = new ArrayList();
		this.fT = new ArrayList();
		this.cT = new ArrayList();
		this.hooks = new ArrayList();
		this.name = string.Empty;
		if (FengGameManagerMKII.nameField == null)
		{
			FengGameManagerMKII.nameField = "GUEST" + UnityEngine.Random.Range(0, 100000);
		}
		if (FengGameManagerMKII.privateServerField == null)
		{
			FengGameManagerMKII.privateServerField = string.Empty;
		}
		if (FengGameManagerMKII.privateLobbyField == null)
		{
			FengGameManagerMKII.privateLobbyField = string.Empty;
		}
		FengGameManagerMKII.usernameField = string.Empty;
		FengGameManagerMKII.passwordField = string.Empty;
		this.resetGameSettings();
		FengGameManagerMKII.banHash = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.imatitan = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.oldScript = string.Empty;
		FengGameManagerMKII.currentLevel = string.Empty;
		this.titanSpawns = new List<Vector3>();
		this.playerSpawnsC = new List<Vector3>();
		this.playerSpawnsM = new List<Vector3>();
		this.playersRPC = new List<PhotonPlayer>();
		this.levelCache = new List<string[]>();
		this.titanSpawners = new List<TitanSpawner>();
		this.restartCount = new List<float>();
		FengGameManagerMKII.ignoreList = new List<int>();
		this.groundList = new List<GameObject>();
		FengGameManagerMKII.noRestart = false;
		FengGameManagerMKII.masterRC = false;
		this.isSpawning = false;
		FengGameManagerMKII.intVariables = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.heroHash = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.boolVariables = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.stringVariables = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.floatVariables = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.globalVariables = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.RCRegions = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.RCEvents = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.RCVariableNames = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.RCRegionTriggers = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.playerVariables = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.titanVariables = new ExitGames.Client.Photon.Hashtable();
		FengGameManagerMKII.logicLoaded = false;
		FengGameManagerMKII.customLevelLoaded = false;
		FengGameManagerMKII.oldScriptLogic = string.Empty;
		this.customMapMaterials = new Dictionary<string, Material>();
		this.retryTime = 0f;
		this.playerList = string.Empty;
		this.updateTime = 0f;
		if (this.textureBackgroundBlack == null)
		{
			this.textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			this.textureBackgroundBlack.SetPixel(0, 0, new Color(0f, 0f, 0f, 1f));
			this.textureBackgroundBlack.Apply();
		}
		if (this.textureBackgroundBlue == null)
		{
			this.textureBackgroundBlue = new Texture2D(1, 1, TextureFormat.ARGB32, mipmap: false);
			this.textureBackgroundBlue.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 1f));
			this.textureBackgroundBlue.Apply();
		}
		this.loadconfig();
		List<string> list = new List<string> { "PanelLogin", "LOGIN", "VERSION", "LabelNetworkStatus" };
		List<string> collection = new List<string> { "AOTTG_HERO", "Colossal", "Icosphere", "Cube", "colossal", "CITY", "city", "rock" };
		if (!SettingsManager.GraphicsSettings.AnimatedIntro.Value)
		{
			list.AddRange(collection);
		}
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			foreach (string item in list)
			{
				if (gameObject.name.Contains(item))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
	}

	public void titanGetKill(PhotonPlayer player, int Damage, string name)
	{
		Damage = Mathf.Max(10, Damage);
		object[] parameters = new object[1] { Damage };
		base.photonView.RPC("netShowDamage", player, parameters);
		object[] parameters2 = new object[2] { name, false };
		base.photonView.RPC("oneTitanDown", PhotonTargets.MasterClient, parameters2);
		this.sendKillInfo(t1: false, (string)player.customProperties[PhotonPlayerProperty.name], t2: true, name, Damage);
		this.playerKillInfoUpdate(player, Damage);
	}

	public void titanGetKillbyServer(int Damage, string name)
	{
		Damage = Mathf.Max(10, Damage);
		this.sendKillInfo(t1: false, LoginFengKAI.player.name, t2: true, name, Damage);
		this.netShowDamage(Damage);
		this.oneTitanDown(name, onPlayerLeave: false);
		this.playerKillInfoUpdate(PhotonNetwork.player, Damage);
	}

	private void tryKick(KickState tmp)
	{
		this.sendChatContentInfo("kicking #" + tmp.name + ", " + tmp.getKickCount() + "/" + (int)((float)PhotonNetwork.playerList.Length * 0.5f) + "vote");
		if (tmp.getKickCount() >= (int)((float)PhotonNetwork.playerList.Length * 0.5f))
		{
			this.kickPhotonPlayer(tmp.name.ToString());
		}
	}

	public void unloadAssets(bool immediate = false)
	{
		if (immediate)
		{
			Resources.UnloadUnusedAssets();
		}
		else if (!this.isUnloading)
		{
			this.isUnloading = true;
			base.StartCoroutine(this.unloadAssetsE(10f));
		}
	}

	public IEnumerator unloadAssetsE(float time)
	{
		yield return new WaitForSeconds(time);
		Resources.UnloadUnusedAssets();
		this.isUnloading = false;
	}

	public void unloadAssetsEditor()
	{
		if (!this.isUnloading)
		{
			this.isUnloading = true;
			base.StartCoroutine(this.unloadAssetsE(30f));
		}
	}

	private void Update()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && GameObject.Find("LabelNetworkStatus") != null)
		{
			GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>().text = PhotonNetwork.connectionStateDetailed.ToString();
			if (PhotonNetwork.connected)
			{
				UILabel component = GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>();
				component.text = component.text + " ping:" + PhotonNetwork.GetPing();
			}
		}
		if (!this.gameStart)
		{
			return;
		}
		foreach (HERO hero in this.heroes)
		{
			hero.update2();
		}
		foreach (Bullet hook in this.hooks)
		{
			hook.update();
		}
		foreach (TITAN_EREN item in this.eT)
		{
			item.update();
		}
		foreach (TITAN titan in this.titans)
		{
			titan.update2();
		}
		foreach (FEMALE_TITAN item2 in this.fT)
		{
			item2.update();
		}
		foreach (COLOSSAL_TITAN item3 in this.cT)
		{
			item3.update2();
		}
		if (this.mainCamera != null)
		{
			this.mainCamera.update2();
		}
	}

	[RPC]
	private void updateKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
	{
		GameObject gameObject = GameObject.Find("UI_IN_GAME");
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/KillInfo"));
		for (int i = 0; i < this.killInfoGO.Count; i++)
		{
			GameObject gameObject3 = (GameObject)this.killInfoGO[i];
			if (gameObject3 != null)
			{
				gameObject3.GetComponent<KillInfoComponent>().moveOn();
			}
		}
		if (this.killInfoGO.Count > 4)
		{
			GameObject gameObject3 = (GameObject)this.killInfoGO[0];
			if (gameObject3 != null)
			{
				gameObject3.GetComponent<KillInfoComponent>().destory();
			}
			this.killInfoGO.RemoveAt(0);
		}
		gameObject2.transform.parent = gameObject.GetComponent<UIReferArray>().panels[0].transform;
		gameObject2.GetComponent<KillInfoComponent>().show(t1, killer, t2, victim, dmg);
		this.killInfoGO.Add(gameObject2);
		this.ReportKillToChatFeed(killer, victim, dmg);
	}

	public void ReportKillToChatFeed(string killer, string victim, int damage)
	{
		if (SettingsManager.UISettings.GameFeed.Value)
		{
			string text = "<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> " + killer.hexColor() + " killed ";
			string newLine = text + victim.hexColor() + " for " + damage + " damage.";
			this.chatRoom.addLINE(newLine);
		}
	}

	[RPC]
	public void verifyPlayerHasLeft(int ID, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient && PhotonPlayer.Find(ID) != null)
		{
			PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
			string empty = string.Empty;
			empty = RCextensions.returnStringFromObject(photonPlayer.customProperties[PhotonPlayerProperty.name]);
			FengGameManagerMKII.banHash.Add(ID, empty);
		}
	}

	public IEnumerator WaitAndRecompilePlayerList(float time)
	{
		yield return new WaitForSeconds(time);
		string text = string.Empty;
		if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
		{
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in array)
			{
				if (photonPlayer.customProperties[PhotonPlayerProperty.dead] == null)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(photonPlayer.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!photonPlayer.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(photonPlayer.ID) + "] ";
				if (photonPlayer.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text2 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(photonPlayer.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
				object[] array2 = new object[11]
				{
					text2,
					string.Empty,
					text3,
					"[ffffff]:",
					num2,
					"/",
					num3,
					"/",
					num4,
					"/",
					num5
				};
				text = string.Concat(array2);
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		else
		{
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
			Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
			PhotonPlayer[] array = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer2 in array)
			{
				if (photonPlayer2.customProperties[PhotonPlayerProperty.dead] != null && !FengGameManagerMKII.ignoreList.Contains(photonPlayer2.ID))
				{
					switch (RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.RCteam]))
					{
					case 0:
						dictionary3.Add(photonPlayer2.ID, photonPlayer2);
						break;
					case 1:
						dictionary.Add(photonPlayer2.ID, photonPlayer2);
						num6 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.kills]);
						num8 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.deaths]);
						num10 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.max_dmg]);
						num12 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					case 2:
						dictionary2.Add(photonPlayer2.ID, photonPlayer2);
						num7 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.kills]);
						num9 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.deaths]);
						num11 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.max_dmg]);
						num13 += RCextensions.returnIntFromObject(photonPlayer2.customProperties[PhotonPlayerProperty.total_dmg]);
						break;
					}
				}
			}
			this.cyanKills = num6;
			this.magentaKills = num7;
			if (PhotonNetwork.isMasterClient)
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value != 2)
				{
					if (SettingsManager.LegacyGameSettings.TeamMode.Value == 3)
					{
						array = PhotonNetwork.playerList;
						foreach (PhotonPlayer photonPlayer3 in array)
						{
							int num14 = 0;
							int num15 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.RCteam]);
							if (num15 <= 0)
							{
								continue;
							}
							switch (num15)
							{
							case 1:
							{
								int num17 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
								if (num7 + num17 + 7 < num6 - num17)
								{
									num14 = 2;
									num7 += num17;
									num6 -= num17;
								}
								break;
							}
							case 2:
							{
								int num16 = RCextensions.returnIntFromObject(photonPlayer3.customProperties[PhotonPlayerProperty.kills]);
								if (num6 + num16 + 7 < num7 - num16)
								{
									num14 = 1;
									num6 += num16;
									num7 -= num16;
								}
								break;
							}
							}
							if (num14 > 0)
							{
								base.photonView.RPC("setTeamRPC", photonPlayer3, num14);
							}
						}
					}
				}
				else
				{
					array = PhotonNetwork.playerList;
					foreach (PhotonPlayer photonPlayer4 in array)
					{
						int num18 = 0;
						if (dictionary.Count > dictionary2.Count + 1)
						{
							num18 = 2;
							if (dictionary.ContainsKey(photonPlayer4.ID))
							{
								dictionary.Remove(photonPlayer4.ID);
							}
							if (!dictionary2.ContainsKey(photonPlayer4.ID))
							{
								dictionary2.Add(photonPlayer4.ID, photonPlayer4);
							}
						}
						else if (dictionary2.Count > dictionary.Count + 1)
						{
							num18 = 1;
							if (!dictionary.ContainsKey(photonPlayer4.ID))
							{
								dictionary.Add(photonPlayer4.ID, photonPlayer4);
							}
							if (dictionary2.ContainsKey(photonPlayer4.ID))
							{
								dictionary2.Remove(photonPlayer4.ID);
							}
						}
						if (num18 > 0)
						{
							base.photonView.RPC("setTeamRPC", photonPlayer4, num18);
						}
					}
				}
			}
			text = text + "[00FFFF]TEAM CYAN" + "[ffffff]:" + this.cyanKills + "/" + num8 + "/" + num10 + "/" + num12 + "\n";
			foreach (PhotonPlayer value in dictionary.Values)
			{
				int num15 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.RCteam]);
				if (value.customProperties[PhotonPlayerProperty.dead] == null || num15 != 1)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(value.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value.ID) + "] ";
				if (value.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text4 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(value.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(value.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text4 + string.Empty + text3 + "[ffffff]:" + num2 + "/" + num3 + "/" + num4 + "/" + num5;
				if (RCextensions.returnBoolFromObject(value.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = text + " \n" + "[FF00FF]TEAM MAGENTA" + "[ffffff]:" + this.magentaKills + "/" + num9 + "/" + num11 + "/" + num13 + "\n";
			foreach (PhotonPlayer value2 in dictionary2.Values)
			{
				int num15 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.RCteam]);
				if (value2.customProperties[PhotonPlayerProperty.dead] == null || num15 != 2)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(value2.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value2.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value2.ID) + "] ";
				if (value2.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text4 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(value2.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(value2.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text4 + string.Empty + text3 + "[ffffff]:" + num2 + "/" + num3 + "/" + num4 + "/" + num5;
				if (RCextensions.returnBoolFromObject(value2.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
			text = string.Concat(new object[3] { text, " \n", "[00FF00]INDIVIDUAL\n" });
			foreach (PhotonPlayer value3 in dictionary3.Values)
			{
				int num15 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.RCteam]);
				if (value3.customProperties[PhotonPlayerProperty.dead] == null || num15 != 0)
				{
					continue;
				}
				if (FengGameManagerMKII.ignoreList.Contains(value3.ID))
				{
					text += "[FF0000][X] ";
				}
				text = ((!value3.isLocal) ? (text + "[FFCC00]") : (text + "[00CC00]"));
				text = text + "[" + Convert.ToString(value3.ID) + "] ";
				if (value3.isMasterClient)
				{
					text += "[ffffff][M] ";
				}
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text = text + "[" + ColorSet.color_red + "] *dead* ";
				}
				if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) < 2)
				{
					int num = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.team]);
					if (num < 2)
					{
						text = text + "[" + ColorSet.color_human + "] H ";
					}
					else if (num == 2)
					{
						text = text + "[" + ColorSet.color_human_1 + "] A ";
					}
				}
				else if (RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.isTitan]) == 2)
				{
					text = text + "[" + ColorSet.color_titan_player + "] <T> ";
				}
				string text4 = text;
				_ = string.Empty;
				string text3 = RCextensions.returnStringFromObject(value3.customProperties[PhotonPlayerProperty.name]);
				int num2 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.kills]);
				int num3 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.deaths]);
				int num4 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.max_dmg]);
				int num5 = RCextensions.returnIntFromObject(value3.customProperties[PhotonPlayerProperty.total_dmg]);
				text = text4 + string.Empty + text3 + "[ffffff]:" + num2 + "/" + num3 + "/" + num4 + "/" + num5;
				if (RCextensions.returnBoolFromObject(value3.customProperties[PhotonPlayerProperty.dead]))
				{
					text += "[-]";
				}
				text += "\n";
			}
		}
		this.playerList = text;
		if (PhotonNetwork.isMasterClient && !this.isWinning && !this.isLosing && this.roundTime >= 5f)
		{
			if (SettingsManager.LegacyGameSettings.InfectionModeEnabled.Value)
			{
				int num19 = 0;
				for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
				{
					PhotonPlayer photonPlayer5 = PhotonNetwork.playerList[j];
					if (FengGameManagerMKII.ignoreList.Contains(photonPlayer5.ID) || photonPlayer5.customProperties[PhotonPlayerProperty.dead] == null || photonPlayer5.customProperties[PhotonPlayerProperty.isTitan] == null)
					{
						continue;
					}
					if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 1)
					{
						if (RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]) && RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.deaths]) > 0)
						{
							if (!FengGameManagerMKII.imatitan.ContainsKey(photonPlayer5.ID))
							{
								FengGameManagerMKII.imatitan.Add(photonPlayer5.ID, 2);
							}
							ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
							hashtable.Add(PhotonPlayerProperty.isTitan, 2);
							photonPlayer5.SetCustomProperties(hashtable);
							base.photonView.RPC("spawnTitanRPC", photonPlayer5);
						}
						else
						{
							if (!FengGameManagerMKII.imatitan.ContainsKey(photonPlayer5.ID))
							{
								continue;
							}
							for (int k = 0; k < this.heroes.Count; k++)
							{
								HERO hERO = (HERO)this.heroes[k];
								if (hERO.photonView.owner == photonPlayer5)
								{
									hERO.markDie();
									hERO.photonView.RPC("netDie2", PhotonTargets.All, -1, "no switching in infection");
								}
							}
						}
					}
					else if (RCextensions.returnIntFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.isTitan]) == 2 && !RCextensions.returnBoolFromObject(photonPlayer5.customProperties[PhotonPlayerProperty.dead]))
					{
						num19++;
					}
				}
				if (num19 <= 0 && IN_GAME_MAIN_CAMERA.gamemode != 0)
				{
					this.gameWin2();
				}
			}
			else if (SettingsManager.LegacyGameSettings.PointModeEnabled.Value)
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0)
				{
					if (this.cyanKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
					{
						object[] parameters = new object[2]
						{
							"<color=#00FFFF>Team Cyan wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters);
						this.gameWin2();
					}
					else if (this.magentaKills >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
					{
						object[] array2 = new object[2]
						{
							"<color=#FF00FF>Team Magenta wins! </color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, array2);
						this.gameWin2();
					}
				}
				else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0)
				{
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer6 = PhotonNetwork.playerList[j];
						if (RCextensions.returnIntFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.kills]) >= SettingsManager.LegacyGameSettings.PointModeAmount.Value)
						{
							object[] parameters2 = new object[2]
							{
								"<color=#FFCC00>" + RCextensions.returnStringFromObject(photonPlayer6.customProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>",
								string.Empty
							};
							base.photonView.RPC("Chat", PhotonTargets.All, parameters2);
							this.gameWin2();
						}
					}
				}
			}
			else if (!SettingsManager.LegacyGameSettings.PointModeEnabled.Value && (SettingsManager.LegacyGameSettings.BombModeEnabled.Value || SettingsManager.LegacyGameSettings.BladePVP.Value > 0))
			{
				if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.playerList.Length > 1)
				{
					int num20 = 0;
					int num21 = 0;
					int num22 = 0;
					int num23 = 0;
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer7 = PhotonNetwork.playerList[j];
						if (FengGameManagerMKII.ignoreList.Contains(photonPlayer7.ID) || photonPlayer7.customProperties[PhotonPlayerProperty.RCteam] == null || photonPlayer7.customProperties[PhotonPlayerProperty.dead] == null)
						{
							continue;
						}
						if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 1)
						{
							num22++;
							if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
							{
								num20++;
							}
						}
						else if (RCextensions.returnIntFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.RCteam]) == 2)
						{
							num23++;
							if (!RCextensions.returnBoolFromObject(photonPlayer7.customProperties[PhotonPlayerProperty.dead]))
							{
								num21++;
							}
						}
					}
					if (num22 > 0 && num23 > 0)
					{
						if (num20 == 0)
						{
							object[] parameters3 = new object[2]
							{
								"<color=#FF00FF>Team Magenta wins! </color>",
								string.Empty
							};
							base.photonView.RPC("Chat", PhotonTargets.All, parameters3);
							this.gameWin2();
						}
						else if (num21 == 0)
						{
							object[] parameters4 = new object[2]
							{
								"<color=#00FFFF>Team Cyan wins! </color>",
								string.Empty
							};
							base.photonView.RPC("Chat", PhotonTargets.All, parameters4);
							this.gameWin2();
						}
					}
				}
				else if (SettingsManager.LegacyGameSettings.TeamMode.Value == 0 && PhotonNetwork.playerList.Length > 1)
				{
					int num24 = 0;
					string text5 = "Nobody";
					PhotonPlayer player = PhotonNetwork.playerList[0];
					for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
					{
						PhotonPlayer photonPlayer8 = PhotonNetwork.playerList[j];
						if (photonPlayer8.customProperties[PhotonPlayerProperty.dead] != null && !RCextensions.returnBoolFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.dead]))
						{
							text5 = RCextensions.returnStringFromObject(photonPlayer8.customProperties[PhotonPlayerProperty.name]).hexColor();
							player = photonPlayer8;
							num24++;
						}
					}
					if (num24 <= 1)
					{
						string text6 = " 5 points added.";
						if (text5 == "Nobody")
						{
							text6 = string.Empty;
						}
						else
						{
							for (int j = 0; j < 5; j++)
							{
								this.playerKillInfoUpdate(player, 0);
							}
						}
						object[] parameters5 = new object[2]
						{
							"<color=#FFCC00>" + text5.hexColor() + " wins." + text6 + "</color>",
							string.Empty
						};
						base.photonView.RPC("Chat", PhotonTargets.All, parameters5);
						this.gameWin2();
					}
				}
			}
		}
		this.isRecompiling = false;
	}

	public IEnumerator WaitAndReloadKDR(PhotonPlayer player)
	{
		yield return new WaitForSeconds(5f);
		string key = RCextensions.returnStringFromObject(player.customProperties[PhotonPlayerProperty.name]);
		if (this.PreservedPlayerKDR.ContainsKey(key))
		{
			int[] array = this.PreservedPlayerKDR[key];
			this.PreservedPlayerKDR.Remove(key);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.kills, array[0]);
			hashtable.Add(PhotonPlayerProperty.deaths, array[1]);
			hashtable.Add(PhotonPlayerProperty.max_dmg, array[2]);
			hashtable.Add(PhotonPlayerProperty.total_dmg, array[3]);
			player.SetCustomProperties(hashtable);
		}
	}

	public IEnumerator WaitAndResetRestarts()
	{
		yield return new WaitForSeconds(10f);
		this.restartingBomb = false;
		this.restartingEren = false;
		this.restartingHorse = false;
		this.restartingMC = false;
		this.restartingTitan = false;
	}

	public IEnumerator WaitAndRespawn1(float time, string str)
	{
		yield return new WaitForSeconds(time);
		this.SpawnPlayer(this.myLastHero, str);
	}

	public IEnumerator WaitAndRespawn2(float time, GameObject pos)
	{
		yield return new WaitForSeconds(time);
		this.SpawnPlayerAt2(this.myLastHero, pos);
	}
}
