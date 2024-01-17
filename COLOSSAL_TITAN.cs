using System;
using System.Collections;
using CustomSkins;
using Photon;
using Settings;
using UnityEngine;

public class COLOSSAL_TITAN : Photon.MonoBehaviour
{
	private string actionName;

	private string attackAnimation;

	private float attackCheckTime;

	private float attackCheckTimeA;

	private float attackCheckTimeB;

	private bool attackChkOnce;

	private int attackCount;

	private int attackPattern = -1;

	public GameObject bottomObject;

	private Transform checkHitCapsuleEnd;

	private Vector3 checkHitCapsuleEndOld;

	private float checkHitCapsuleR;

	private Transform checkHitCapsuleStart;

	public GameObject door_broken;

	public GameObject door_closed;

	public bool hasDie;

	public bool hasspawn;

	public GameObject healthLabel;

	public float healthTime;

	private bool isSteamNeed;

	public float lagMax;

	public int maxHealth;

	public static float minusDistance = 99999f;

	public static GameObject minusDistanceEnemy;

	public float myDistance;

	public GameObject myHero;

	public int NapeArmor = 10000;

	public int NapeArmorTotal = 10000;

	public GameObject neckSteamObject;

	public float size;

	private string state = "idle";

	public GameObject sweepSmokeObject;

	private float tauntTime;

	private float waitTime = 2f;

	private ColossalCustomSkinLoader _customSkinLoader;

	private void attack_sweep(string type = "")
	{
		this.callTitanHAHA();
		this.state = "attack_sweep";
		this.attackAnimation = "sweep" + type;
		this.attackCheckTimeA = 0.4f;
		this.attackCheckTimeB = 0.57f;
		this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
		this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
		this.checkHitCapsuleR = 20f;
		this.crossFade("attack_" + this.attackAnimation, 0.1f);
		this.attackChkOnce = false;
		this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
		this.sweepSmokeObject.GetComponent<ParticleSystem>().Play();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (FengGameManagerMKII.LAN)
			{
				_ = Network.peerType;
				_ = 1;
			}
			else if (PhotonNetwork.isMasterClient)
			{
				base.photonView.RPC("startSweepSmoke", PhotonTargets.Others);
			}
		}
	}

	private void Awake()
	{
		base.rigidbody.freezeRotation = true;
		base.rigidbody.useGravity = false;
		base.rigidbody.isKinematic = true;
		this._customSkinLoader = base.gameObject.AddComponent<ColossalCustomSkinLoader>();
	}

	public void beTauntedBy(GameObject target, float tauntTime)
	{
	}

	public void blowPlayer(GameObject player, Transform neck)
	{
		Vector3 vector = -(neck.position + base.transform.forward * 50f - player.transform.position);
		float num = 20f;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			player.GetComponent<HERO>().blowAway(vector.normalized * num + Vector3.up * 1f, null);
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[1] { vector.normalized * num + Vector3.up * 1f };
			player.GetComponent<HERO>().photonView.RPC("blowAway", PhotonTargets.All, parameters);
		}
	}

	private void callTitan(bool special = false)
	{
		if (!special && GameObject.FindGameObjectsWithTag("titan").Length > 6)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("titanRespawn");
		ArrayList arrayList = new ArrayList();
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if (gameObject.transform.parent.name == "titanRespawnCT")
			{
				arrayList.Add(gameObject);
			}
		}
		GameObject gameObject2 = (GameObject)arrayList[UnityEngine.Random.Range(0, arrayList.Count)];
		string[] array3 = new string[1] { "TITAN_VER3.1" };
		GameObject gameObject3 = ((!FengGameManagerMKII.LAN) ? PhotonNetwork.Instantiate(array3[UnityEngine.Random.Range(0, array3.Length)], gameObject2.transform.position, gameObject2.transform.rotation, 0) : ((GameObject)Network.Instantiate(Resources.Load(array3[UnityEngine.Random.Range(0, array3.Length)]), gameObject2.transform.position, gameObject2.transform.rotation, 0)));
		if (special)
		{
			GameObject[] array4 = GameObject.FindGameObjectsWithTag("route");
			GameObject gameObject4 = array4[UnityEngine.Random.Range(0, array4.Length)];
			while (gameObject4.name != "routeCT")
			{
				gameObject4 = array4[UnityEngine.Random.Range(0, array4.Length)];
			}
			gameObject3.GetComponent<TITAN>().setRoute(gameObject4);
			gameObject3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_I, forceCrawler: false);
			gameObject3.GetComponent<TITAN>().activeRad = 0;
			gameObject3.GetComponent<TITAN>().toCheckPoint((Vector3)gameObject3.GetComponent<TITAN>().checkPoints[0], 10f);
		}
		else
		{
			float num = 0.7f;
			float num2 = 0.7f;
			if (IN_GAME_MAIN_CAMERA.difficulty != 0)
			{
				if (IN_GAME_MAIN_CAMERA.difficulty == 1)
				{
					num = 0.4f;
					num2 = 0.7f;
				}
				else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
				{
					num = -1f;
					num2 = 0.7f;
				}
			}
			if (GameObject.FindGameObjectsWithTag("titan").Length == 5)
			{
				gameObject3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
			}
			else if (UnityEngine.Random.Range(0f, 1f) >= num)
			{
				if (UnityEngine.Random.Range(0f, 1f) < num2)
				{
					gameObject3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_JUMPER, forceCrawler: false);
				}
				else
				{
					gameObject3.GetComponent<TITAN>().setAbnormalType2(AbnormalType.TYPE_CRAWLER, forceCrawler: false);
				}
			}
			gameObject3.GetComponent<TITAN>().activeRad = 200;
		}
		if (FengGameManagerMKII.LAN)
		{
			((GameObject)Network.Instantiate(Resources.Load("FX/FXtitanSpawn"), gameObject3.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0)).transform.localScale = gameObject3.transform.localScale;
		}
		else
		{
			PhotonNetwork.Instantiate("FX/FXtitanSpawn", gameObject3.transform.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = gameObject3.transform.localScale;
		}
	}

	private void callTitanHAHA()
	{
		this.attackCount++;
		int num = 4;
		int num2 = 7;
		if (IN_GAME_MAIN_CAMERA.difficulty != 0)
		{
			if (IN_GAME_MAIN_CAMERA.difficulty == 1)
			{
				num = 4;
				num2 = 6;
			}
			else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
			{
				num = 3;
				num2 = 5;
			}
		}
		if (this.attackCount % num == 0)
		{
			this.callTitan();
		}
		if ((double)this.NapeArmor < (double)this.NapeArmorTotal * 0.3)
		{
			if (this.attackCount % (int)((float)num2 * 0.5f) == 0)
			{
				this.callTitan(special: true);
			}
		}
		else if (this.attackCount % num2 == 0)
		{
			this.callTitan(special: true);
		}
	}

	[RPC]
	private void changeDoor()
	{
		this.door_broken.SetActive(value: true);
		this.door_closed.SetActive(value: false);
	}

	private RaycastHit[] checkHitCapsule(Vector3 start, Vector3 end, float r)
	{
		return Physics.SphereCastAll(start, r, end - start, Vector3.Distance(start, end));
	}

	private GameObject checkIfHitHand(Transform hand)
	{
		float num = 30f;
		Collider[] array = Physics.OverlapSphere(hand.GetComponent<SphereCollider>().transform.position, num + 1f);
		foreach (Collider collider in array)
		{
			if (!(collider.transform.root.tag == "Player"))
			{
				continue;
			}
			GameObject gameObject = collider.transform.root.gameObject;
			if (gameObject.GetComponent<TITAN_EREN>() != null)
			{
				if (!gameObject.GetComponent<TITAN_EREN>().isHit)
				{
					gameObject.GetComponent<TITAN_EREN>().hitByTitan();
				}
				return gameObject;
			}
			if (gameObject.GetComponent<HERO>() != null && !gameObject.GetComponent<HERO>().isInvincible())
			{
				return gameObject;
			}
		}
		return null;
	}

	private void crossFade(string aniName, float time)
	{
		base.animation.CrossFade(aniName, time);
		if (!FengGameManagerMKII.LAN && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[2] { aniName, time };
			base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
		}
	}

	private void findNearestHero()
	{
		this.myHero = this.getNearestHero();
	}

	private GameObject getNearestHero()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject result = null;
		float num = float.PositiveInfinity;
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((gameObject.GetComponent<HERO>() == null || !gameObject.GetComponent<HERO>().HasDied()) && (gameObject.GetComponent<TITAN_EREN>() == null || !gameObject.GetComponent<TITAN_EREN>().hasDied))
			{
				float num2 = Mathf.Sqrt((gameObject.transform.position.x - base.transform.position.x) * (gameObject.transform.position.x - base.transform.position.x) + (gameObject.transform.position.z - base.transform.position.z) * (gameObject.transform.position.z - base.transform.position.z));
				if (gameObject.transform.position.y - base.transform.position.y < 450f && num2 < num)
				{
					result = gameObject;
					num = num2;
				}
			}
		}
		return result;
	}

	private void idle()
	{
		this.state = "idle";
		this.crossFade("idle", 0.2f);
	}

	private void kick()
	{
		this.state = "kick";
		this.actionName = "attack_kick_wall";
		this.attackCheckTime = 0.64f;
		this.attackChkOnce = false;
		this.crossFade(this.actionName, 0.1f);
	}

	private void killPlayer(GameObject hitHero)
	{
		if (!(hitHero != null))
		{
			return;
		}
		Vector3 position = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (!hitHero.GetComponent<HERO>().HasDied())
			{
				hitHero.GetComponent<HERO>().die((hitHero.transform.position - position) * 15f * 4f, isBite: false);
			}
		}
		else
		{
			if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
			{
				return;
			}
			if (FengGameManagerMKII.LAN)
			{
				if (!hitHero.GetComponent<HERO>().HasDied())
				{
					hitHero.GetComponent<HERO>().markDie();
				}
			}
			else if (!hitHero.GetComponent<HERO>().HasDied())
			{
				hitHero.GetComponent<HERO>().markDie();
				object[] parameters = new object[5]
				{
					(hitHero.transform.position - position) * 15f * 4f,
					false,
					-1,
					"Colossal Titan",
					true
				};
				hitHero.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters);
			}
		}
	}

	[RPC]
	public void labelRPC(int health, int maxHealth, PhotonMessageInfo info)
	{
		if (info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "colossal labelRPC");
			return;
		}
		if (health < 0)
		{
			if (this.healthLabel != null)
			{
				UnityEngine.Object.Destroy(this.healthLabel);
			}
			return;
		}
		if (this.healthLabel == null)
		{
			this.healthLabel = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
			this.healthLabel.name = "LabelNameOverHead";
			this.healthLabel.transform.parent = base.transform;
			this.healthLabel.transform.localPosition = new Vector3(0f, 430f, 0f);
			float num = 15f;
			if (this.size > 0f && this.size < 1f)
			{
				num = 15f / this.size;
				num = Mathf.Min(num, 100f);
			}
			this.healthLabel.transform.localScale = new Vector3(num, num, num);
		}
		string text = "[7FFF00]";
		float num2 = (float)health / (float)maxHealth;
		if (num2 < 0.75f && num2 >= 0.5f)
		{
			text = "[f2b50f]";
		}
		else if (num2 < 0.5f && num2 >= 0.25f)
		{
			text = "[ff8100]";
		}
		else if (num2 < 0.25f)
		{
			text = "[ff3333]";
		}
		this.healthLabel.GetComponent<UILabel>().text = text + Convert.ToString(health);
	}

	public void loadskin()
	{
		if (PhotonNetwork.isMasterClient && SettingsManager.CustomSkinSettings.Shifter.SkinsEnabled.Value)
		{
			string value = ((ShifterCustomSkinSet)SettingsManager.CustomSkinSettings.Shifter.GetSelectedSet()).Colossal.Value;
			if (TextureDownloader.ValidTextureURL(value))
			{
				base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, value);
			}
		}
	}

	public IEnumerator loadskinE(string url)
	{
		while (!this.hasspawn)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this._customSkinLoader.LoadSkinsFromRPC(new object[1] { url }));
	}

	[RPC]
	public void loadskinRPC(string url, PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			BaseCustomSkinSettings<ShifterCustomSkinSet> shifter = SettingsManager.CustomSkinSettings.Shifter;
			if (shifter.SkinsEnabled.Value && (!shifter.SkinsLocal.Value || base.photonView.isMine))
			{
				base.StartCoroutine(this.loadskinE(url));
			}
		}
	}

	private void neckSteam()
	{
		this.neckSteamObject.GetComponent<ParticleSystem>().Stop();
		this.neckSteamObject.GetComponent<ParticleSystem>().Play();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (FengGameManagerMKII.LAN)
			{
				if (Network.peerType == NetworkPeerType.Server)
				{
				}
			}
			else if (PhotonNetwork.isMasterClient)
			{
				base.photonView.RPC("startNeckSteam", PhotonTargets.Others);
			}
		}
		this.isSteamNeed = true;
		Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
		float radius = 30f;
		Collider[] array = Physics.OverlapSphere(transform.transform.position - base.transform.forward * 10f, radius);
		foreach (Collider collider in array)
		{
			if (collider.transform.root.tag == "Player")
			{
				GameObject gameObject = collider.transform.root.gameObject;
				if (gameObject.GetComponent<TITAN_EREN>() == null && gameObject.GetComponent<HERO>() != null)
				{
					this.blowPlayer(gameObject, transform);
				}
			}
		}
	}

	[RPC]
	private void netCrossFade(string aniName, float time)
	{
		base.animation.CrossFade(aniName, time);
	}

	[RPC]
	public void netDie()
	{
		if (!this.hasDie)
		{
			this.hasDie = true;
		}
	}

	[RPC]
	private void netPlayAnimation(string aniName)
	{
		base.animation.Play(aniName);
	}

	[RPC]
	private void netPlayAnimationAt(string aniName, float normalizedTime)
	{
		base.animation.Play(aniName);
		base.animation[aniName].normalizedTime = normalizedTime;
	}

	private void OnDestroy()
	{
		if (GameObject.Find("MultiplayerManager") != null)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeCT(this);
		}
	}

	private void playAnimation(string aniName)
	{
		base.animation.Play(aniName);
		if (!FengGameManagerMKII.LAN && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[1] { aniName };
			base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
		}
	}

	private void playAnimationAt(string aniName, float normalizedTime)
	{
		base.animation.Play(aniName);
		base.animation[aniName].normalizedTime = normalizedTime;
		if (!FengGameManagerMKII.LAN && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[2] { aniName, normalizedTime };
			base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
		}
	}

	private void playSound(string sndname)
	{
		this.playsoundRPC(sndname);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (FengGameManagerMKII.LAN)
			{
				_ = Network.peerType;
				_ = 1;
			}
			else if (PhotonNetwork.isMasterClient)
			{
				object[] parameters = new object[1] { sndname };
				base.photonView.RPC("playsoundRPC", PhotonTargets.Others, parameters);
			}
		}
	}

	[RPC]
	private void playsoundRPC(string sndname)
	{
		base.transform.Find(sndname).GetComponent<AudioSource>().Play();
	}

	[RPC]
	private void removeMe(PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "colossal remove");
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	[RPC]
	public void setSize(float size, PhotonMessageInfo info)
	{
		size = Mathf.Clamp(size, 0.1f, 50f);
		if (info != null && info.sender.isMasterClient)
		{
			base.transform.localScale *= size * 0.05f;
			this.size = size;
		}
	}

	private void slap(string type)
	{
		this.callTitanHAHA();
		this.state = "slap";
		this.attackAnimation = type;
		if (type == "r1" || type == "r2")
		{
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
		}
		if (type == "l1" || type == "l2")
		{
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
		}
		this.attackCheckTime = 0.57f;
		this.attackChkOnce = false;
		this.crossFade("attack_slap_" + this.attackAnimation, 0.1f);
	}

	private void Start()
	{
		this.startMain();
		this.size = 20f;
		if (Minimap.instance != null)
		{
			Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.black, trackOrientation: false, depthAboveAll: true);
		}
		if (base.photonView.isMine)
		{
			if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
			{
				float value = SettingsManager.LegacyGameSettings.TitanSizeMin.Value;
				float value2 = SettingsManager.LegacyGameSettings.TitanSizeMax.Value;
				this.size = UnityEngine.Random.Range(value, value2);
				base.photonView.RPC("setSize", PhotonTargets.AllBuffered, this.size);
			}
			this.lagMax = 150f + this.size * 3f;
			this.healthTime = 0f;
			this.maxHealth = this.NapeArmor;
			if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
			{
				this.maxHealth = (this.NapeArmor = UnityEngine.Random.Range(SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value));
			}
			if (this.NapeArmor > 0)
			{
				base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, this.NapeArmor, this.maxHealth);
			}
			this.loadskin();
		}
		this.hasspawn = true;
	}

	private void startMain()
	{
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCT(this);
		if (this.myHero == null)
		{
			this.findNearestHero();
		}
		base.name = "COLOSSAL_TITAN";
		this.NapeArmor = 1000;
		bool flag = false;
		if (LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.NEVER)
		{
			flag = true;
		}
		if (IN_GAME_MAIN_CAMERA.difficulty == 0)
		{
			this.NapeArmor = ((!flag) ? 5000 : 2000);
		}
		else if (IN_GAME_MAIN_CAMERA.difficulty == 1)
		{
			this.NapeArmor = ((!flag) ? 8000 : 3500);
			foreach (AnimationState item in base.animation)
			{
				item.speed = 1.02f;
			}
		}
		else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
		{
			this.NapeArmor = ((!flag) ? 12000 : 5000);
			foreach (AnimationState item2 in base.animation)
			{
				item2.speed = 1.05f;
			}
		}
		this.NapeArmorTotal = this.NapeArmor;
		this.state = "wait";
		base.transform.position += -Vector3.up * 10000f;
		if (FengGameManagerMKII.LAN)
		{
			base.GetComponent<PhotonView>().enabled = false;
		}
		else
		{
			base.GetComponent<NetworkView>().enabled = false;
		}
		this.door_broken = GameObject.Find("door_broke");
		this.door_closed = GameObject.Find("door_fine");
		this.door_broken.SetActive(value: false);
		this.door_closed.SetActive(value: true);
	}

	[RPC]
	private void startNeckSteam()
	{
		this.neckSteamObject.GetComponent<ParticleSystem>().Stop();
		this.neckSteamObject.GetComponent<ParticleSystem>().Play();
	}

	[RPC]
	private void startSweepSmoke()
	{
		this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = true;
		this.sweepSmokeObject.GetComponent<ParticleSystem>().Play();
	}

	private void steam()
	{
		this.callTitanHAHA();
		this.state = "steam";
		this.actionName = "attack_steam";
		this.attackCheckTime = 0.45f;
		this.crossFade(this.actionName, 0.1f);
		this.attackChkOnce = false;
	}

	[RPC]
	private void stopSweepSmoke()
	{
		this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
		this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
	}

	[RPC]
	public void titanGetHit(int viewID, int speed)
	{
		Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
		PhotonView photonView = PhotonView.Find(viewID);
		if (!(photonView != null) || !((photonView.gameObject.transform.position - transform.transform.position).magnitude < this.lagMax) || !(this.healthTime <= 0f))
		{
			return;
		}
		if (!SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value || speed >= SettingsManager.LegacyGameSettings.TitanArmor.Value)
		{
			this.NapeArmor -= speed;
		}
		if ((float)this.maxHealth > 0f)
		{
			base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, this.NapeArmor, this.maxHealth);
		}
		this.neckSteam();
		if (this.NapeArmor <= 0)
		{
			this.NapeArmor = 0;
			if (!this.hasDie)
			{
				if (FengGameManagerMKII.LAN)
				{
					this.netDie();
				}
				else
				{
					base.photonView.RPC("netDie", PhotonTargets.OthersBuffered);
					this.netDie();
					GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().titanGetKill(photonView.owner, speed, base.name);
				}
			}
		}
		else
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(t1: false, (string)photonView.owner.customProperties[PhotonPlayerProperty.name], t2: true, "Colossal Titan's neck", speed);
			object[] parameters = new object[1] { speed };
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, parameters);
		}
		this.healthTime = 0.2f;
	}

	public void update()
	{
		if (!(this.state != "null"))
		{
			return;
		}
		if (this.state == "wait")
		{
			this.waitTime -= Time.deltaTime;
			if (this.waitTime <= 0f)
			{
				base.transform.position = new Vector3(30f, 0f, 784f);
				UnityEngine.Object.Instantiate(Resources.Load("FX/ThunderCT"), base.transform.position + Vector3.up * 350f, Quaternion.Euler(270f, 0f, 0f));
				GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					this.idle();
				}
				else if ((!FengGameManagerMKII.LAN) ? base.photonView.isMine : base.networkView.isMine)
				{
					this.idle();
				}
				else
				{
					this.state = "null";
				}
			}
			return;
		}
		if (!(this.state == "idle"))
		{
			if (this.state == "attack_sweep")
			{
				if (this.attackCheckTimeA != 0f && ((base.animation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA && base.animation["attack_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB) || (!this.attackChkOnce && base.animation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA)))
				{
					if (!this.attackChkOnce)
					{
						this.attackChkOnce = true;
					}
					RaycastHit[] array = this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR);
					foreach (RaycastHit raycastHit in array)
					{
						GameObject gameObject = raycastHit.collider.gameObject;
						if (gameObject.tag == "Player")
						{
							this.killPlayer(gameObject);
						}
						if (gameObject.tag == "erenHitbox" && this.attackAnimation == "combo_3" && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && ((!FengGameManagerMKII.LAN) ? PhotonNetwork.isMasterClient : Network.isServer))
						{
							gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
						}
					}
					array = this.checkHitCapsule(this.checkHitCapsuleEndOld, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR);
					foreach (RaycastHit raycastHit2 in array)
					{
						GameObject gameObject2 = raycastHit2.collider.gameObject;
						if (gameObject2.tag == "Player")
						{
							this.killPlayer(gameObject2);
						}
					}
					this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.position;
				}
				if (base.animation["attack_" + this.attackAnimation].normalizedTime >= 1f)
				{
					this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
					this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
					{
						base.photonView.RPC("stopSweepSmoke", PhotonTargets.Others);
					}
					this.findNearestHero();
					this.idle();
					this.playAnimation("idle");
				}
			}
			else if (this.state == "kick")
			{
				if (!this.attackChkOnce && base.animation[this.actionName].normalizedTime >= this.attackCheckTime)
				{
					this.attackChkOnce = true;
					this.door_broken.SetActive(value: true);
					this.door_closed.SetActive(value: false);
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
					{
						base.photonView.RPC("changeDoor", PhotonTargets.OthersBuffered);
					}
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						if (FengGameManagerMKII.LAN)
						{
							Network.Instantiate(Resources.Load("FX/boom1_CT_KICK"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(270f, 0f, 0f), 0);
							Network.Instantiate(Resources.Load("rock"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(0f, 0f, 0f), 0);
						}
						else
						{
							PhotonNetwork.Instantiate("FX/boom1_CT_KICK", base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(270f, 0f, 0f), 0);
							PhotonNetwork.Instantiate("rock", base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(0f, 0f, 0f), 0);
						}
					}
					else
					{
						UnityEngine.Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(270f, 0f, 0f));
						UnityEngine.Object.Instantiate(Resources.Load("rock"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(0f, 0f, 0f));
					}
				}
				if (base.animation[this.actionName].normalizedTime >= 1f)
				{
					this.findNearestHero();
					this.idle();
					this.playAnimation("idle");
				}
			}
			else if (this.state == "slap")
			{
				if (!this.attackChkOnce && base.animation["attack_slap_" + this.attackAnimation].normalizedTime >= this.attackCheckTime)
				{
					this.attackChkOnce = true;
					GameObject gameObject3;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						gameObject3 = ((!FengGameManagerMKII.LAN) ? PhotonNetwork.Instantiate("FX/boom1", this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0) : ((GameObject)Network.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0)));
						if (gameObject3.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject3.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
					}
					else
					{
						gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f));
					}
					gameObject3.transform.localScale = new Vector3(5f, 5f, 5f);
				}
				if (base.animation["attack_slap_" + this.attackAnimation].normalizedTime >= 1f)
				{
					this.findNearestHero();
					this.idle();
					this.playAnimation("idle");
				}
			}
			else if (this.state == "steam")
			{
				if (!this.attackChkOnce && base.animation[this.actionName].normalizedTime >= this.attackCheckTime)
				{
					this.attackChkOnce = true;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						if (FengGameManagerMKII.LAN)
						{
							Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
							Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
							Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
						}
						else
						{
							PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
							PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
							PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
						}
					}
					else
					{
						UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.forward * 185f, Quaternion.Euler(270f, 0f, 0f));
						UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.forward * 303f, Quaternion.Euler(270f, 0f, 0f));
						UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.forward * 50f, Quaternion.Euler(270f, 0f, 0f));
					}
				}
				if (!(base.animation[this.actionName].normalizedTime >= 1f))
				{
					return;
				}
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					if (FengGameManagerMKII.LAN)
					{
						Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
						Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
						Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
					}
					else
					{
						GameObject gameObject4 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
						if (gameObject4.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
						gameObject4 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
						if (gameObject4.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
						gameObject4 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
						if (gameObject4.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
					}
				}
				else
				{
					UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.forward * 185f, Quaternion.Euler(270f, 0f, 0f));
					UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.forward * 303f, Quaternion.Euler(270f, 0f, 0f));
					UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.forward * 50f, Quaternion.Euler(270f, 0f, 0f));
				}
				if (this.hasDie)
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					else if (FengGameManagerMKII.LAN)
					{
						if (base.networkView.isMine)
						{
						}
					}
					else if (PhotonNetwork.isMasterClient)
					{
						PhotonNetwork.Destroy(base.photonView);
					}
					GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
				}
				this.findNearestHero();
				this.idle();
				this.playAnimation("idle");
			}
			else
			{
				_ = this.state == string.Empty;
			}
			return;
		}
		if (this.attackPattern == -1)
		{
			this.slap("r1");
			this.attackPattern++;
			return;
		}
		if (this.attackPattern == 0)
		{
			this.attack_sweep(string.Empty);
			this.attackPattern++;
			return;
		}
		if (this.attackPattern == 1)
		{
			this.steam();
			this.attackPattern++;
			return;
		}
		if (this.attackPattern == 2)
		{
			this.kick();
			this.attackPattern++;
			return;
		}
		if (this.isSteamNeed || this.hasDie)
		{
			this.steam();
			this.isSteamNeed = false;
			return;
		}
		if (this.myHero == null)
		{
			this.findNearestHero();
			return;
		}
		Vector3 vector = this.myHero.transform.position - base.transform.position;
		float f = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, base.gameObject.transform.rotation.eulerAngles.y - 90f);
		this.myDistance = Mathf.Sqrt((this.myHero.transform.position.x - base.transform.position.x) * (this.myHero.transform.position.x - base.transform.position.x) + (this.myHero.transform.position.z - base.transform.position.z) * (this.myHero.transform.position.z - base.transform.position.z));
		float num = this.myHero.transform.position.y - base.transform.position.y;
		if (this.myDistance < 85f && UnityEngine.Random.Range(0, 100) < 5)
		{
			this.steam();
			return;
		}
		if (num > 310f && num < 350f)
		{
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APL1").position) < 40f)
			{
				this.slap("l1");
				return;
			}
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APL2").position) < 40f)
			{
				this.slap("l2");
				return;
			}
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APR1").position) < 40f)
			{
				this.slap("r1");
				return;
			}
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APR2").position) < 40f)
			{
				this.slap("r2");
				return;
			}
			if (this.myDistance < 150f && Mathf.Abs(f) < 80f)
			{
				this.attack_sweep(string.Empty);
				return;
			}
		}
		if (num < 300f && Mathf.Abs(f) < 80f && this.myDistance < 85f)
		{
			this.attack_sweep("_vertical");
			return;
		}
		switch (UnityEngine.Random.Range(0, 7))
		{
		case 0:
			this.slap("l1");
			break;
		case 1:
			this.slap("l2");
			break;
		case 2:
			this.slap("r1");
			break;
		case 3:
			this.slap("r2");
			break;
		case 4:
			this.attack_sweep(string.Empty);
			break;
		case 5:
			this.attack_sweep("_vertical");
			break;
		case 6:
			this.steam();
			break;
		}
	}

	public void update2()
	{
		this.healthTime -= Time.deltaTime;
		this.updateLabel();
		if (!(this.state != "null"))
		{
			return;
		}
		if (this.state == "wait")
		{
			this.waitTime -= Time.deltaTime;
			if (this.waitTime <= 0f)
			{
				base.transform.position = new Vector3(30f, 0f, 784f);
				UnityEngine.Object.Instantiate(Resources.Load("FX/ThunderCT"), base.transform.position + Vector3.up * 350f, Quaternion.Euler(270f, 0f, 0f));
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					this.idle();
				}
				else if ((!FengGameManagerMKII.LAN) ? base.photonView.isMine : base.networkView.isMine)
				{
					this.idle();
				}
				else
				{
					this.state = "null";
				}
			}
			return;
		}
		if (this.state != "idle")
		{
			if (this.state == "attack_sweep")
			{
				if (this.attackCheckTimeA != 0f && ((base.animation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA && base.animation["attack_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB) || (!this.attackChkOnce && base.animation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA)))
				{
					if (!this.attackChkOnce)
					{
						this.attackChkOnce = true;
					}
					RaycastHit[] array = this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR);
					foreach (RaycastHit raycastHit in array)
					{
						GameObject gameObject = raycastHit.collider.gameObject;
						if (gameObject.tag == "Player")
						{
							this.killPlayer(gameObject);
						}
						if (gameObject.tag == "erenHitbox" && this.attackAnimation == "combo_3" && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && ((!FengGameManagerMKII.LAN) ? PhotonNetwork.isMasterClient : Network.isServer))
						{
							gameObject.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
						}
					}
					array = this.checkHitCapsule(this.checkHitCapsuleEndOld, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR);
					foreach (RaycastHit raycastHit2 in array)
					{
						GameObject gameObject2 = raycastHit2.collider.gameObject;
						if (gameObject2.tag == "Player")
						{
							this.killPlayer(gameObject2);
						}
					}
					this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.position;
				}
				if (base.animation["attack_" + this.attackAnimation].normalizedTime >= 1f)
				{
					this.sweepSmokeObject.GetComponent<ParticleSystem>().enableEmission = false;
					this.sweepSmokeObject.GetComponent<ParticleSystem>().Stop();
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
					{
						base.photonView.RPC("stopSweepSmoke", PhotonTargets.Others);
					}
					this.findNearestHero();
					this.idle();
					this.playAnimation("idle");
				}
			}
			else if (this.state == "kick")
			{
				if (!this.attackChkOnce && base.animation[this.actionName].normalizedTime >= this.attackCheckTime)
				{
					this.attackChkOnce = true;
					this.door_broken.SetActive(value: true);
					this.door_closed.SetActive(value: false);
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !FengGameManagerMKII.LAN)
					{
						base.photonView.RPC("changeDoor", PhotonTargets.OthersBuffered);
					}
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						if (FengGameManagerMKII.LAN)
						{
							Network.Instantiate(Resources.Load("FX/boom1_CT_KICK"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(270f, 0f, 0f), 0);
							Network.Instantiate(Resources.Load("rock"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(0f, 0f, 0f), 0);
						}
						else
						{
							PhotonNetwork.Instantiate("FX/boom1_CT_KICK", base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(270f, 0f, 0f), 0);
							PhotonNetwork.Instantiate("rock", base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(0f, 0f, 0f), 0);
						}
					}
					else
					{
						UnityEngine.Object.Instantiate(Resources.Load("FX/boom1_CT_KICK"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(270f, 0f, 0f));
						UnityEngine.Object.Instantiate(Resources.Load("rock"), base.transform.position + base.transform.forward * 120f + base.transform.right * 30f, Quaternion.Euler(0f, 0f, 0f));
					}
				}
				if (base.animation[this.actionName].normalizedTime >= 1f)
				{
					this.findNearestHero();
					this.idle();
					this.playAnimation("idle");
				}
			}
			else if (this.state == "slap")
			{
				if (!this.attackChkOnce && base.animation["attack_slap_" + this.attackAnimation].normalizedTime >= this.attackCheckTime)
				{
					this.attackChkOnce = true;
					GameObject gameObject3;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						gameObject3 = ((!FengGameManagerMKII.LAN) ? PhotonNetwork.Instantiate("FX/boom1", this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0) : ((GameObject)Network.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f), 0)));
						if (gameObject3.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject3.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
					}
					else
					{
						gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/boom1"), this.checkHitCapsuleStart.position, Quaternion.Euler(270f, 0f, 0f));
					}
					gameObject3.transform.localScale = new Vector3(5f, 5f, 5f);
				}
				if (base.animation["attack_slap_" + this.attackAnimation].normalizedTime >= 1f)
				{
					this.findNearestHero();
					this.idle();
					this.playAnimation("idle");
				}
			}
			else if (this.state == "steam")
			{
				if (!this.attackChkOnce && base.animation[this.actionName].normalizedTime >= this.attackCheckTime)
				{
					this.attackChkOnce = true;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						if (FengGameManagerMKII.LAN)
						{
							Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
							Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
							Network.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
						}
						else
						{
							PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
							PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
							PhotonNetwork.Instantiate("FX/colossal_steam", base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
						}
					}
					else
					{
						UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.forward * 185f, Quaternion.Euler(270f, 0f, 0f));
						UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.forward * 303f, Quaternion.Euler(270f, 0f, 0f));
						UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam"), base.transform.position + base.transform.forward * 50f, Quaternion.Euler(270f, 0f, 0f));
					}
				}
				if (!(base.animation[this.actionName].normalizedTime >= 1f))
				{
					return;
				}
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					if (FengGameManagerMKII.LAN)
					{
						Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
						Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
						Network.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
					}
					else
					{
						GameObject gameObject4 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + base.transform.up * 185f, Quaternion.Euler(270f, 0f, 0f), 0);
						if (gameObject4.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
						gameObject4 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + base.transform.up * 303f, Quaternion.Euler(270f, 0f, 0f), 0);
						if (gameObject4.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
						gameObject4 = PhotonNetwork.Instantiate("FX/colossal_steam_dmg", base.transform.position + base.transform.up * 50f, Quaternion.Euler(270f, 0f, 0f), 0);
						if (gameObject4.GetComponent<EnemyfxIDcontainer>() != null)
						{
							gameObject4.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
						}
					}
				}
				else
				{
					UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.forward * 185f, Quaternion.Euler(270f, 0f, 0f));
					UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.forward * 303f, Quaternion.Euler(270f, 0f, 0f));
					UnityEngine.Object.Instantiate(Resources.Load("FX/colossal_steam_dmg"), base.transform.position + base.transform.forward * 50f, Quaternion.Euler(270f, 0f, 0f));
				}
				if (this.hasDie)
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					else if (FengGameManagerMKII.LAN)
					{
						if (base.networkView.isMine)
						{
						}
					}
					else if (PhotonNetwork.isMasterClient)
					{
						PhotonNetwork.Destroy(base.photonView);
					}
					GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameWin2();
				}
				this.findNearestHero();
				this.idle();
				this.playAnimation("idle");
			}
			else
			{
				_ = this.state == string.Empty;
			}
			return;
		}
		if (this.attackPattern == -1)
		{
			this.slap("r1");
			this.attackPattern++;
			return;
		}
		if (this.attackPattern == 0)
		{
			this.attack_sweep(string.Empty);
			this.attackPattern++;
			return;
		}
		if (this.attackPattern == 1)
		{
			this.steam();
			this.attackPattern++;
			return;
		}
		if (this.attackPattern == 2)
		{
			this.kick();
			this.attackPattern++;
			return;
		}
		if (this.isSteamNeed || this.hasDie)
		{
			this.steam();
			this.isSteamNeed = false;
			return;
		}
		if (this.myHero == null)
		{
			this.findNearestHero();
			return;
		}
		Vector3 vector = this.myHero.transform.position - base.transform.position;
		float f = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, base.gameObject.transform.rotation.eulerAngles.y - 90f);
		this.myDistance = Mathf.Sqrt((this.myHero.transform.position.x - base.transform.position.x) * (this.myHero.transform.position.x - base.transform.position.x) + (this.myHero.transform.position.z - base.transform.position.z) * (this.myHero.transform.position.z - base.transform.position.z));
		float num = this.myHero.transform.position.y - base.transform.position.y;
		if (this.myDistance < 85f && UnityEngine.Random.Range(0, 100) < 5)
		{
			this.steam();
			return;
		}
		if (num > 310f && num < 350f)
		{
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APL1").position) < 40f)
			{
				this.slap("l1");
				return;
			}
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APL2").position) < 40f)
			{
				this.slap("l2");
				return;
			}
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APR1").position) < 40f)
			{
				this.slap("r1");
				return;
			}
			if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("APR2").position) < 40f)
			{
				this.slap("r2");
				return;
			}
			if (this.myDistance < 150f && Mathf.Abs(f) < 80f)
			{
				this.attack_sweep(string.Empty);
				return;
			}
		}
		if (num < 300f && Mathf.Abs(f) < 80f && this.myDistance < 85f)
		{
			this.attack_sweep("_vertical");
			return;
		}
		switch (UnityEngine.Random.Range(0, 7))
		{
		case 0:
			this.slap("l1");
			break;
		case 1:
			this.slap("l2");
			break;
		case 2:
			this.slap("r1");
			break;
		case 3:
			this.slap("r2");
			break;
		case 4:
			this.attack_sweep(string.Empty);
			break;
		case 5:
			this.attack_sweep("_vertical");
			break;
		case 6:
			this.steam();
			break;
		}
	}

	public void updateLabel()
	{
		if (this.healthLabel != null && this.healthLabel.GetComponent<UILabel>().isVisible)
		{
			this.healthLabel.transform.LookAt(2f * this.healthLabel.transform.position - Camera.main.transform.position);
		}
	}
}
