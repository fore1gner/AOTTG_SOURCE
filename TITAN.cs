using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Constants;
using CustomSkins;
using ExitGames.Client.Photon;
using Photon;
using Settings;
using UI;
using UnityEngine;

internal class TITAN : Photon.MonoBehaviour
{
	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap5;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap6;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap7;

	private Vector3 abnorma_jump_bite_horizon_v;

	public AbnormalType abnormalType;

	public int activeRad = int.MaxValue;

	private float angle;

	public bool asClientLookTarget;

	private string attackAnimation;

	private float attackCheckTime;

	private float attackCheckTimeA;

	private float attackCheckTimeB;

	private int attackCount;

	public float attackDistance = 13f;

	private bool attacked;

	private float attackEndWait;

	public float attackWait = 1f;

	public Animation baseAnimation;

	public AudioSource baseAudioSource;

	public List<Collider> baseColliders;

	public Transform baseGameObjectTransform;

	public Rigidbody baseRigidBody;

	public Transform baseTransform;

	private float between2;

	public float chaseDistance = 80f;

	public ArrayList checkPoints = new ArrayList();

	public bool colliderEnabled;

	public TITAN_CONTROLLER controller;

	public GameObject currentCamera;

	private Transform currentGrabHand;

	public int currentHealth;

	private float desDeg;

	private float dieTime;

	public bool eye;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchmap5;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchmap6;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchmap7;

	private string fxName;

	private Vector3 fxPosition;

	private Quaternion fxRotation;

	private float getdownTime;

	private GameObject grabbedTarget;

	public GameObject grabTF;

	private float gravity = 120f;

	private bool grounded;

	public bool hasDie;

	private bool hasDieSteam;

	public bool hasExplode;

	public bool hasload;

	public bool hasSetLevel;

	public bool hasSpawn;

	private Transform head;

	private Vector3 headscale = Vector3.one;

	public GameObject healthLabel;

	public bool healthLabelEnabled;

	public float healthTime;

	private string hitAnimation;

	private float hitPause;

	public bool isAlarm;

	private bool isAttackMoveByCore;

	private bool isGrabHandLeft;

	public bool isHooked;

	public bool isLook;

	public bool isThunderSpear;

	public float lagMax;

	private bool leftHandAttack;

	public GameObject mainMaterial;

	public int maxHealth;

	private float maxStamina = 320f;

	public float maxVelocityChange = 10f;

	public static float minusDistance = 99999f;

	public static GameObject minusDistanceEnemy;

	public FengGameManagerMKII MultiplayerManager;

	public int myDifficulty;

	public float myDistance;

	public GROUP myGroup = GROUP.T;

	public GameObject myHero;

	public float myLevel = 1f;

	public TitanTrigger myTitanTrigger;

	private Transform neck;

	private bool needFreshCorePosition;

	private string nextAttackAnimation;

	public bool nonAI;

	private bool nonAIcombo;

	private Vector3 oldCorePosition;

	private Quaternion oldHeadRotation;

	public PVPcheckPoint PVPfromCheckPt;

	private float random_run_time;

	private float rockInterval;

	public bool rockthrow;

	private string runAnimation;

	private float sbtime;

	public int skin;

	private Vector3 spawnPt;

	public float speed = 7f;

	private float stamina = 320f;

	private TitanState state;

	private int stepSoundPhase = 2;

	private bool stuck;

	private float stuckTime;

	private float stuckTurnAngle;

	private Vector3 targetCheckPt;

	private Quaternion targetHeadRotation;

	private float targetR;

	private float tauntTime;

	private GameObject throwRock;

	private string turnAnimation;

	private float turnDeg;

	private GameObject whoHasTauntMe;

	private TitanCustomSkinLoader _customSkinLoader;

	private bool _hasRunStart;

	private HashSet<string> _ignoreLookTargetAnimations;

	private HashSet<string> _fastHeadRotationAnimations;

	private bool _ignoreLookTarget;

	private bool _fastHeadRotation;

	private void HideTitanIfBomb()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && PhotonNetwork.isMasterClient && SettingsManager.LegacyGameSettings.BombModeEnabled.Value && SettingsManager.LegacyGameSettings.BombModeDisableTitans.Value)
		{
			base.transform.position = new Vector3(-10000f, -10000f, -10000f);
		}
	}

	public bool WillDIe(int damage)
	{
		if (!this.hasDie)
		{
			if (!SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value || damage >= SettingsManager.LegacyGameSettings.TitanArmor.Value)
			{
				return (float)(this.currentHealth - damage) <= 0f;
			}
			if (this.abnormalType == AbnormalType.TYPE_CRAWLER && !SettingsManager.LegacyGameSettings.TitanArmorCrawlerEnabled.Value)
			{
				return (float)(this.currentHealth - damage) <= 0f;
			}
		}
		return false;
	}

	private void attack(string type)
	{
		this.state = TitanState.attack;
		this.attacked = false;
		this.isAlarm = true;
		if (this.attackAnimation == type)
		{
			this.attackAnimation = type;
			this.playAnimationAt("attack_" + type, 0f);
		}
		else
		{
			this.attackAnimation = type;
			this.playAnimationAt("attack_" + type, 0f);
		}
		this.nextAttackAnimation = null;
		this.fxName = null;
		this.isAttackMoveByCore = false;
		this.attackCheckTime = 0f;
		this.attackCheckTimeA = 0f;
		this.attackCheckTimeB = 0f;
		this.attackEndWait = 0f;
		this.fxRotation = Quaternion.Euler(270f, 0f, 0f);
		switch (type)
		{
		case "abnormal_getup":
			this.attackCheckTime = 0f;
			this.fxName = string.Empty;
			break;
		case "abnormal_jump":
			this.nextAttackAnimation = "abnormal_getup";
			if (!this.nonAI)
			{
				this.attackEndWait = ((this.myDifficulty <= 0) ? UnityEngine.Random.Range(1f, 4f) : UnityEngine.Random.Range(0f, 1f));
			}
			else
			{
				this.attackEndWait = 0f;
			}
			this.attackCheckTime = 0.75f;
			this.fxName = "boom4";
			this.fxRotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
			break;
		case "combo_1":
			this.nextAttackAnimation = "combo_2";
			this.attackCheckTimeA = 0.54f;
			this.attackCheckTimeB = 0.76f;
			this.nonAIcombo = false;
			this.isAttackMoveByCore = true;
			this.leftHandAttack = false;
			break;
		case "combo_2":
			if (this.abnormalType != AbnormalType.TYPE_PUNK)
			{
				this.nextAttackAnimation = "combo_3";
			}
			this.attackCheckTimeA = 0.37f;
			this.attackCheckTimeB = 0.57f;
			this.nonAIcombo = false;
			this.isAttackMoveByCore = true;
			this.leftHandAttack = true;
			break;
		case "combo_3":
			this.nonAIcombo = false;
			this.isAttackMoveByCore = true;
			this.attackCheckTime = 0.21f;
			this.fxName = "boom1";
			break;
		case "front_ground":
			this.fxName = "boom1";
			this.attackCheckTime = 0.45f;
			break;
		case "kick":
			this.fxName = "boom5";
			this.fxRotation = base.transform.rotation;
			this.attackCheckTime = 0.43f;
			break;
		case "slap_back":
			this.fxName = "boom3";
			this.attackCheckTime = 0.66f;
			break;
		case "slap_face":
			this.fxName = "boom3";
			this.attackCheckTime = 0.655f;
			break;
		case "stomp":
			this.fxName = "boom2";
			this.attackCheckTime = 0.42f;
			break;
		case "bite":
			this.fxName = "bite";
			this.attackCheckTime = 0.6f;
			break;
		case "bite_l":
			this.fxName = "bite";
			this.attackCheckTime = 0.4f;
			break;
		case "bite_r":
			this.fxName = "bite";
			this.attackCheckTime = 0.4f;
			break;
		case "jumper_0":
			this.abnorma_jump_bite_horizon_v = Vector3.zero;
			break;
		case "crawler_jump_0":
			this.abnorma_jump_bite_horizon_v = Vector3.zero;
			break;
		case "anti_AE_l":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = true;
			break;
		case "anti_AE_r":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = false;
			break;
		case "anti_AE_low_l":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = true;
			break;
		case "anti_AE_low_r":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = false;
			break;
		case "quick_turn_l":
			this.attackCheckTimeA = 2f;
			this.attackCheckTimeB = 2f;
			this.isAttackMoveByCore = true;
			break;
		case "quick_turn_r":
			this.attackCheckTimeA = 2f;
			this.attackCheckTimeB = 2f;
			this.isAttackMoveByCore = true;
			break;
		case "throw":
			this.isAlarm = true;
			this.chaseDistance = 99999f;
			break;
		}
		this.needFreshCorePosition = true;
	}

	private void attack2(string type)
	{
		this.state = TitanState.attack;
		this.attacked = false;
		this.isAlarm = true;
		if (this.attackAnimation == type)
		{
			this.attackAnimation = type;
			this.playAnimationAt("attack_" + type, 0f);
		}
		else
		{
			this.attackAnimation = type;
			this.playAnimationAt("attack_" + type, 0f);
		}
		this.nextAttackAnimation = null;
		this.fxName = null;
		this.isAttackMoveByCore = false;
		this.attackCheckTime = 0f;
		this.attackCheckTimeA = 0f;
		this.attackCheckTimeB = 0f;
		this.attackEndWait = 0f;
		this.fxRotation = Quaternion.Euler(270f, 0f, 0f);
		switch (type)
		{
		case "abnormal_getup":
			this.attackCheckTime = 0f;
			this.fxName = string.Empty;
			break;
		case "abnormal_jump":
			this.nextAttackAnimation = "abnormal_getup";
			if (this.nonAI)
			{
				this.attackEndWait = 0f;
			}
			else
			{
				this.attackEndWait = ((this.myDifficulty <= 0) ? UnityEngine.Random.Range(1f, 4f) : UnityEngine.Random.Range(0f, 1f));
			}
			this.attackCheckTime = 0.75f;
			this.fxName = "boom4";
			this.fxRotation = Quaternion.Euler(270f, this.baseTransform.rotation.eulerAngles.y, 0f);
			break;
		case "combo_1":
			this.nextAttackAnimation = "combo_2";
			this.attackCheckTimeA = 0.54f;
			this.attackCheckTimeB = 0.76f;
			this.nonAIcombo = false;
			this.isAttackMoveByCore = true;
			this.leftHandAttack = false;
			break;
		case "combo_2":
			if (this.abnormalType != AbnormalType.TYPE_PUNK && !this.nonAI)
			{
				this.nextAttackAnimation = "combo_3";
			}
			this.attackCheckTimeA = 0.37f;
			this.attackCheckTimeB = 0.57f;
			this.nonAIcombo = false;
			this.isAttackMoveByCore = true;
			this.leftHandAttack = true;
			break;
		case "combo_3":
			this.nonAIcombo = false;
			this.isAttackMoveByCore = true;
			this.attackCheckTime = 0.21f;
			this.fxName = "boom1";
			break;
		case "front_ground":
			this.fxName = "boom1";
			this.attackCheckTime = 0.45f;
			break;
		case "kick":
			this.fxName = "boom5";
			this.fxRotation = this.baseTransform.rotation;
			this.attackCheckTime = 0.43f;
			break;
		case "slap_back":
			this.fxName = "boom3";
			this.attackCheckTime = 0.66f;
			break;
		case "slap_face":
			this.fxName = "boom3";
			this.attackCheckTime = 0.655f;
			break;
		case "stomp":
			this.fxName = "boom2";
			this.attackCheckTime = 0.42f;
			break;
		case "bite":
			this.fxName = "bite";
			this.attackCheckTime = 0.6f;
			break;
		case "bite_l":
			this.fxName = "bite";
			this.attackCheckTime = 0.4f;
			break;
		case "bite_r":
			this.fxName = "bite";
			this.attackCheckTime = 0.4f;
			break;
		case "jumper_0":
			this.abnorma_jump_bite_horizon_v = Vector3.zero;
			break;
		case "crawler_jump_0":
			this.abnorma_jump_bite_horizon_v = Vector3.zero;
			break;
		case "anti_AE_l":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = true;
			break;
		case "anti_AE_r":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = false;
			break;
		case "anti_AE_low_l":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = true;
			break;
		case "anti_AE_low_r":
			this.attackCheckTimeA = 0.31f;
			this.attackCheckTimeB = 0.4f;
			this.leftHandAttack = false;
			break;
		case "quick_turn_l":
			this.attackCheckTimeA = 2f;
			this.attackCheckTimeB = 2f;
			this.isAttackMoveByCore = true;
			break;
		case "quick_turn_r":
			this.attackCheckTimeA = 2f;
			this.attackCheckTimeB = 2f;
			this.isAttackMoveByCore = true;
			break;
		case "throw":
			this.isAlarm = true;
			this.chaseDistance = 99999f;
			break;
		}
		this.needFreshCorePosition = true;
	}

	private void Awake()
	{
		this.cache();
		this.baseRigidBody.freezeRotation = true;
		this.baseRigidBody.useGravity = false;
		this._customSkinLoader = base.gameObject.AddComponent<TitanCustomSkinLoader>();
		this._ignoreLookTargetAnimations = new HashSet<string> { "sit_hunt_down", "hit_eren_L", "hit_eren_R", "idle_recovery", "eat_l", "eat_r", "sit_hit_eye", "hit_eye" };
		this._fastHeadRotationAnimations = new HashSet<string> { "hit_eren_L", "hit_eren_R", "sit_hit_eye", "hit_eye" };
		foreach (AnimationState item in base.animation)
		{
			if (item.name.StartsWith("attack_"))
			{
				this._ignoreLookTargetAnimations.Add(item.name);
				this._fastHeadRotationAnimations.Add(item.name);
			}
		}
		this.HideTitanIfBomb();
	}

	private void CheckAnimationLookTarget(string animation)
	{
		this._ignoreLookTarget = this._ignoreLookTargetAnimations.Contains(animation);
		this._fastHeadRotation = this._fastHeadRotationAnimations.Contains(animation);
	}

	private IEnumerator HandleSpawnCollisionCoroutine(float time, float maxSpeed)
	{
		while (time > 0f)
		{
			if (this.baseRigidBody.velocity.magnitude > maxSpeed)
			{
				this.baseRigidBody.velocity = this.baseRigidBody.velocity.normalized * maxSpeed;
			}
			time -= Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
	}

	public void beLaughAttacked()
	{
		if (!this.hasDie && this.abnormalType != AbnormalType.TYPE_CRAWLER)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				object[] parameters = new object[1] { 0f };
				base.photonView.RPC("laugh", PhotonTargets.All, parameters);
			}
			else if (this.state == TitanState.idle || this.state == TitanState.turn || this.state == TitanState.chase)
			{
				this.laugh();
			}
		}
	}

	public void beTauntedBy(GameObject target, float tauntTime)
	{
		this.whoHasTauntMe = target;
		this.tauntTime = tauntTime;
		this.isAlarm = true;
	}

	public void cache()
	{
		this.baseAudioSource = base.transform.Find("snd_titan_foot").GetComponent<AudioSource>();
		this.baseAnimation = base.animation;
		this.baseTransform = base.transform;
		this.baseRigidBody = base.rigidbody;
		this.baseColliders = new List<Collider>();
		Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			if (collider.name != "AABB")
			{
				this.baseColliders.Add(collider);
			}
		}
		GameObject gameObject = new GameObject
		{
			name = "PlayerDetectorRC"
		};
		CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
		CapsuleCollider component = this.baseTransform.Find("AABB").GetComponent<CapsuleCollider>();
		capsuleCollider.center = component.center;
		capsuleCollider.radius = Math.Abs(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position.y - this.baseTransform.position.y);
		capsuleCollider.height = component.height * 1.2f;
		capsuleCollider.material = component.material;
		capsuleCollider.isTrigger = true;
		capsuleCollider.name = "PlayerDetectorRC";
		this.myTitanTrigger = gameObject.AddComponent<TitanTrigger>();
		this.myTitanTrigger.isCollide = false;
		gameObject.layer = PhysicsLayer.PlayerAttackBox;
		gameObject.transform.parent = this.baseTransform.Find("AABB");
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.MultiplayerManager = FengGameManagerMKII.instance;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
		{
			this.baseGameObjectTransform = base.gameObject.transform;
		}
	}

	private void chase()
	{
		this.state = TitanState.chase;
		this.isAlarm = true;
		this.crossFade(this.runAnimation, 0.5f);
	}

	private GameObject checkIfHitCrawlerMouth(Transform head, float rad)
	{
		float num = rad * this.myLevel;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TITAN_EREN>() == null && (gameObject.GetComponent<HERO>() == null || !gameObject.GetComponent<HERO>().isInvincible()))
			{
				float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
				if (Vector3.Distance(gameObject.transform.position + Vector3.up * num2, head.position - Vector3.up * 1.5f * this.myLevel) < num + num2)
				{
					return gameObject;
				}
			}
		}
		return null;
	}

	private GameObject checkIfHitHand(Transform hand)
	{
		float num = 2.4f * this.myLevel;
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
			}
			else if (gameObject.GetComponent<HERO>() != null && !gameObject.GetComponent<HERO>().isInvincible())
			{
				return gameObject;
			}
		}
		return null;
	}

	private GameObject checkIfHitHead(Transform head, float rad)
	{
		float num = rad * this.myLevel;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TITAN_EREN>() == null && (gameObject.GetComponent<HERO>() == null || !gameObject.GetComponent<HERO>().isInvincible()))
			{
				float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
				if (Vector3.Distance(gameObject.transform.position + Vector3.up * num2, head.position + Vector3.up * 1.5f * this.myLevel) < num + num2)
				{
					return gameObject;
				}
			}
		}
		return null;
	}

	private void crossFadeIfNotPlaying(string aniName, float time)
	{
		if (!base.animation.IsPlaying(aniName) || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || PhotonNetwork.offlineMode)
		{
			this.crossFade(aniName, time);
		}
	}

	private void crossFade(string aniName, float time)
	{
		base.animation.CrossFade(aniName, time);
		this.CheckAnimationLookTarget(aniName);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			object[] parameters = new object[2] { aniName, time };
			base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
		}
	}

	public bool die()
	{
		if (this.hasDie)
		{
			return false;
		}
		this.hasDie = true;
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().oneTitanDown(string.Empty, onPlayerLeave: false);
		this.dieAnimation();
		return true;
	}

	private void dieAnimation()
	{
		if (base.animation.IsPlaying("sit_idle") || base.animation.IsPlaying("sit_hit_eye"))
		{
			this.crossFade("sit_die", 0.1f);
		}
		else if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			this.crossFade("crawler_die", 0.2f);
		}
		else if (this.abnormalType == AbnormalType.NORMAL)
		{
			this.crossFade("die_front", 0.05f);
		}
		else if ((base.animation.IsPlaying("attack_abnormal_jump") && base.animation["attack_abnormal_jump"].normalizedTime > 0.7f) || (base.animation.IsPlaying("attack_abnormal_getup") && base.animation["attack_abnormal_getup"].normalizedTime < 0.7f) || base.animation.IsPlaying("tired"))
		{
			this.crossFade("die_ground", 0.2f);
		}
		else
		{
			this.crossFade("die_back", 0.05f);
		}
	}

	public void dieBlow(Vector3 attacker, float hitPauseTime)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			this.dieBlowFunc(attacker, hitPauseTime);
			if (GameObject.FindGameObjectsWithTag("titan").Length <= 1)
			{
				GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			}
		}
		else
		{
			object[] parameters = new object[2] { attacker, hitPauseTime };
			base.photonView.RPC("dieBlowRPC", PhotonTargets.All, parameters);
		}
	}

	public void dieBlowFunc(Vector3 attacker, float hitPauseTime)
	{
		if (this.hasDie)
		{
			return;
		}
		base.transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - base.transform.position).eulerAngles.y, 0f);
		this.hasDie = true;
		this.hitAnimation = "die_blow";
		this.hitPause = hitPauseTime;
		this.playAnimation(this.hitAnimation);
		base.animation[this.hitAnimation].time = 0f;
		base.animation[this.hitAnimation].speed = 0f;
		this.needFreshCorePosition = true;
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().oneTitanDown(string.Empty, onPlayerLeave: false);
		if (base.photonView.isMine)
		{
			if (this.grabbedTarget != null)
			{
				this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
			}
			if (this.nonAI)
			{
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.dead, true);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths] + 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
		}
	}

	[RPC]
	private void dieBlowRPC(Vector3 attacker, float hitPauseTime)
	{
		if (base.photonView.isMine && (attacker - base.transform.position).magnitude < 80f)
		{
			this.dieBlowFunc(attacker, hitPauseTime);
		}
	}

	[RPC]
	public void DieByCannon(int viewID)
	{
		PhotonView photonView = PhotonView.Find(viewID);
		if (photonView != null)
		{
			int damage = 0;
			if (PhotonNetwork.isMasterClient)
			{
				this.OnTitanDie(photonView);
			}
			if (this.nonAI)
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, damage, (string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
			}
			else
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, damage, base.name);
			}
		}
		else
		{
			FengGameManagerMKII.instance.photonView.RPC("netShowDamage", photonView.owner, this.speed);
		}
	}

	public void dieHeadBlow(Vector3 attacker, float hitPauseTime)
	{
		if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			this.dieHeadBlowFunc(attacker, hitPauseTime);
			if (GameObject.FindGameObjectsWithTag("titan").Length <= 1)
			{
				GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			}
		}
		else
		{
			object[] parameters = new object[2] { attacker, hitPauseTime };
			base.photonView.RPC("dieHeadBlowRPC", PhotonTargets.All, parameters);
		}
	}

	public void dieHeadBlowFunc(Vector3 attacker, float hitPauseTime)
	{
		if (this.hasDie)
		{
			return;
		}
		this.playSound("snd_titan_head_blow");
		base.transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - base.transform.position).eulerAngles.y, 0f);
		this.hasDie = true;
		this.hitAnimation = "die_headOff";
		this.hitPause = hitPauseTime;
		this.playAnimation(this.hitAnimation);
		base.animation[this.hitAnimation].time = 0f;
		base.animation[this.hitAnimation].speed = 0f;
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().oneTitanDown(string.Empty, onPlayerLeave: false);
		this.needFreshCorePosition = true;
		GameObject gameObject = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("bloodExplore"), this.head.position + Vector3.up * 1f * this.myLevel, Quaternion.Euler(270f, 0f, 0f))) : PhotonNetwork.Instantiate("bloodExplore", this.head.position + Vector3.up * 1f * this.myLevel, Quaternion.Euler(270f, 0f, 0f), 0));
		gameObject.transform.localScale = base.transform.localScale;
		gameObject = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("bloodsplatter"), this.head.position, Quaternion.Euler(270f + this.neck.rotation.eulerAngles.x, this.neck.rotation.eulerAngles.y, this.neck.rotation.eulerAngles.z))) : PhotonNetwork.Instantiate("bloodsplatter", this.head.position, Quaternion.Euler(270f + this.neck.rotation.eulerAngles.x, this.neck.rotation.eulerAngles.y, this.neck.rotation.eulerAngles.z), 0));
		gameObject.transform.localScale = base.transform.localScale;
		gameObject.transform.parent = this.neck;
		gameObject = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/justSmoke"), this.neck.position, Quaternion.Euler(270f, 0f, 0f))) : PhotonNetwork.Instantiate("FX/justSmoke", this.neck.position, Quaternion.Euler(270f, 0f, 0f), 0));
		gameObject.transform.parent = this.neck;
		if (base.photonView.isMine)
		{
			if (this.grabbedTarget != null)
			{
				this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
			}
			if (this.nonAI)
			{
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.dead, true);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths] + 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
		}
	}

	[RPC]
	private void dieHeadBlowRPC(Vector3 attacker, float hitPauseTime)
	{
		if (base.photonView.isMine && (attacker - this.neck.position).magnitude < this.lagMax)
		{
			this.dieHeadBlowFunc(attacker, hitPauseTime);
		}
	}

	private void eat()
	{
		this.state = TitanState.eat;
		this.attacked = false;
		if (this.isGrabHandLeft)
		{
			this.attackAnimation = "eat_l";
			this.crossFade("eat_l", 0.1f);
		}
		else
		{
			this.attackAnimation = "eat_r";
			this.crossFade("eat_r", 0.1f);
		}
	}

	private void eatSet(GameObject grabTarget)
	{
		if ((IN_GAME_MAIN_CAMERA.gametype != 0 && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine)) || !grabTarget.GetComponent<HERO>().isGrabbed)
		{
			this.grabToRight(null);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
			{
				base.photonView.RPC("grabToRight", PhotonTargets.Others);
				object[] parameters = new object[1] { "grabbed" };
				grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters);
				object[] parameters2 = new object[2]
				{
					base.photonView.viewID,
					false
				};
				grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, parameters2);
			}
			else
			{
				grabTarget.GetComponent<HERO>().grabbed(base.gameObject, leftHand: false);
				grabTarget.GetComponent<HERO>().animation.Play("grabbed");
			}
		}
	}

	private void eatSetL(GameObject grabTarget)
	{
		if ((IN_GAME_MAIN_CAMERA.gametype != 0 && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine)) || !grabTarget.GetComponent<HERO>().isGrabbed)
		{
			this.grabToLeft(null);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
			{
				base.photonView.RPC("grabToLeft", PhotonTargets.Others);
				object[] parameters = new object[1] { "grabbed" };
				grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters);
				object[] parameters2 = new object[2]
				{
					base.photonView.viewID,
					true
				};
				grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, parameters2);
			}
			else
			{
				grabTarget.GetComponent<HERO>().grabbed(base.gameObject, leftHand: true);
				grabTarget.GetComponent<HERO>().animation.Play("grabbed");
			}
		}
	}

	private bool executeAttack(string decidedAction)
	{
		switch (decidedAction)
		{
		case "grab_ground_front_l":
			this.grab("ground_front_l");
			return true;
		case "grab_ground_front_r":
			this.grab("ground_front_r");
			return true;
		case "grab_ground_back_l":
			this.grab("ground_back_l");
			return true;
		case "grab_ground_back_r":
			this.grab("ground_back_r");
			return true;
		case "grab_head_front_l":
			this.grab("head_front_l");
			return true;
		case "grab_head_front_r":
			this.grab("head_front_r");
			return true;
		case "grab_head_back_l":
			this.grab("head_back_l");
			return true;
		case "grab_head_back_r":
			this.grab("head_back_r");
			return true;
		case "attack_abnormal_jump":
			this.attack("abnormal_jump");
			return true;
		case "attack_combo":
			this.attack("combo_1");
			return true;
		case "attack_front_ground":
			this.attack("front_ground");
			return true;
		case "attack_kick":
			this.attack("kick");
			return true;
		case "attack_slap_back":
			this.attack("slap_back");
			return true;
		case "attack_slap_face":
			this.attack("slap_face");
			return true;
		case "attack_stomp":
			this.attack("stomp");
			return true;
		case "attack_bite":
			this.attack("bite");
			return true;
		case "attack_bite_l":
			this.attack("bite_l");
			return true;
		case "attack_bite_r":
			this.attack("bite_r");
			return true;
		default:
			return false;
		}
	}

	private bool executeAttack2(string decidedAction)
	{
		switch (decidedAction)
		{
		case "grab_ground_front_l":
			this.grab("ground_front_l");
			return true;
		case "grab_ground_front_r":
			this.grab("ground_front_r");
			return true;
		case "grab_ground_back_l":
			this.grab("ground_back_l");
			return true;
		case "grab_ground_back_r":
			this.grab("ground_back_r");
			return true;
		case "grab_head_front_l":
			this.grab("head_front_l");
			return true;
		case "grab_head_front_r":
			this.grab("head_front_r");
			return true;
		case "grab_head_back_l":
			this.grab("head_back_l");
			return true;
		case "grab_head_back_r":
			this.grab("head_back_r");
			return true;
		case "attack_abnormal_jump":
			this.attack2("abnormal_jump");
			return true;
		case "attack_combo":
			this.attack2("combo_1");
			return true;
		case "attack_front_ground":
			this.attack2("front_ground");
			return true;
		case "attack_kick":
			this.attack2("kick");
			return true;
		case "attack_slap_back":
			this.attack2("slap_back");
			return true;
		case "attack_slap_face":
			this.attack2("slap_face");
			return true;
		case "attack_stomp":
			this.attack2("stomp");
			return true;
		case "attack_bite":
			this.attack2("bite");
			return true;
		case "attack_bite_l":
			this.attack2("bite_l");
			return true;
		case "attack_bite_r":
			this.attack2("bite_r");
			return true;
		default:
			return false;
		}
	}

	public void explode()
	{
		if (!SettingsManager.LegacyGameSettings.TitanExplodeEnabled.Value || !this.hasDie || !(this.dieTime >= 1f) || this.hasExplode)
		{
			return;
		}
		int num = 0;
		float num2 = this.myLevel * 10f;
		if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			if (this.dieTime >= 2f)
			{
				this.hasExplode = true;
				num2 = 0f;
				num = 1;
			}
		}
		else
		{
			num = 1;
			this.hasExplode = true;
		}
		if (num != 1)
		{
			return;
		}
		Vector3 vector = this.baseTransform.position + Vector3.up * num2;
		PhotonNetwork.Instantiate("FX/Thunder", vector, Quaternion.Euler(270f, 0f, 0f), 0);
		PhotonNetwork.Instantiate("FX/boom1", vector, Quaternion.Euler(270f, 0f, 0f), 0);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject gameObject in array)
		{
			if (Vector3.Distance(gameObject.transform.position, vector) < (float)SettingsManager.LegacyGameSettings.TitanExplodeRadius.Value)
			{
				gameObject.GetComponent<HERO>().markDie();
				gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, "Server ");
			}
		}
	}

	private void findNearestFacingHero2()
	{
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		Vector3 position = this.baseTransform.position;
		float num2 = ((this.abnormalType != 0) ? 180f : 100f);
		foreach (HERO player in this.MultiplayerManager.getPlayers())
		{
			float num3 = Vector3.Distance(player.transform.position, position);
			if (num3 < num)
			{
				Vector3 vector = player.transform.position - this.baseTransform.position;
				if (Mathf.Abs(0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f)) < num2)
				{
					gameObject = player.gameObject;
					num = num3;
				}
			}
		}
		foreach (TITAN_EREN eren in this.MultiplayerManager.getErens())
		{
			float num4 = Vector3.Distance(eren.transform.position, position);
			if (num4 < num)
			{
				Vector3 vector2 = eren.transform.position - this.baseTransform.position;
				if (Mathf.Abs(0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f)) < num2)
				{
					gameObject = eren.gameObject;
					num = num4;
				}
			}
		}
		if (!(gameObject != null))
		{
			return;
		}
		GameObject obj = this.myHero;
		this.myHero = gameObject;
		if (obj != this.myHero && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			if (this.myHero == null)
			{
				object[] parameters = new object[1] { -1 };
				base.photonView.RPC("setMyTarget", PhotonTargets.Others, parameters);
			}
			else
			{
				object[] parameters2 = new object[1] { this.myHero.GetPhotonView().viewID };
				base.photonView.RPC("setMyTarget", PhotonTargets.Others, parameters2);
			}
		}
		this.tauntTime = 5f;
	}

	private void findNearestHero2()
	{
		GameObject gameObject = this.myHero;
		this.myHero = this.getNearestHero2();
		if (this.myHero != gameObject && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			if (this.myHero == null)
			{
				object[] parameters = new object[1] { -1 };
				base.photonView.RPC("setMyTarget", PhotonTargets.Others, parameters);
			}
			else
			{
				object[] parameters2 = new object[1] { this.myHero.GetPhotonView().viewID };
				base.photonView.RPC("setMyTarget", PhotonTargets.Others, parameters2);
			}
		}
		this.oldHeadRotation = this.head.rotation;
	}

	private void FixedUpdate()
	{
		if ((GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine))
		{
			return;
		}
		this.baseRigidBody.AddForce(new Vector3(0f, (0f - this.gravity) * this.baseRigidBody.mass, 0f));
		if (this.needFreshCorePosition)
		{
			this.oldCorePosition = this.baseTransform.position - this.baseTransform.Find("Amarture/Core").position;
			this.needFreshCorePosition = false;
		}
		if (this.hasDie)
		{
			if (this.hitPause <= 0f && this.baseAnimation.IsPlaying("die_headOff"))
			{
				Vector3 vector = this.baseTransform.position - this.baseTransform.Find("Amarture/Core").position - this.oldCorePosition;
				this.baseRigidBody.velocity = vector / Time.deltaTime + Vector3.up * this.baseRigidBody.velocity.y;
			}
			this.oldCorePosition = this.baseTransform.position - this.baseTransform.Find("Amarture/Core").position;
		}
		else if ((this.state == TitanState.attack && this.isAttackMoveByCore) || this.state == TitanState.hit)
		{
			Vector3 vector2 = this.baseTransform.position - this.baseTransform.Find("Amarture/Core").position - this.oldCorePosition;
			this.baseRigidBody.velocity = vector2 / Time.deltaTime + Vector3.up * this.baseRigidBody.velocity.y;
			this.oldCorePosition = this.baseTransform.position - this.baseTransform.Find("Amarture/Core").position;
		}
		if (this.hasDie)
		{
			if (this.hitPause > 0f)
			{
				this.hitPause -= Time.deltaTime;
				if (this.hitPause <= 0f)
				{
					this.baseAnimation[this.hitAnimation].speed = 1f;
					this.hitPause = 0f;
				}
			}
			else if (this.baseAnimation.IsPlaying("die_blow"))
			{
				if (this.baseAnimation["die_blow"].normalizedTime < 0.55f)
				{
					this.baseRigidBody.velocity = -this.baseTransform.forward * 300f + Vector3.up * this.baseRigidBody.velocity.y;
				}
				else if (this.baseAnimation["die_blow"].normalizedTime < 0.83f)
				{
					this.baseRigidBody.velocity = -this.baseTransform.forward * 100f + Vector3.up * this.baseRigidBody.velocity.y;
				}
				else
				{
					this.baseRigidBody.velocity = Vector3.up * this.baseRigidBody.velocity.y;
				}
			}
			return;
		}
		if (this.nonAI && !GameMenu.Paused && (this.state == TitanState.idle || (this.state == TitanState.attack && this.attackAnimation == "jumper_1")))
		{
			Vector3 vector3 = Vector3.zero;
			if (this.controller.targetDirection != -874f)
			{
				bool flag = false;
				if (this.stamina < 5f)
				{
					flag = true;
				}
				else if (!(this.stamina >= 40f) && !this.baseAnimation.IsPlaying("run_abnormal") && !this.baseAnimation.IsPlaying("crawler_run"))
				{
					flag = true;
				}
				vector3 = ((!(this.controller.isWALKDown || flag)) ? (this.baseTransform.forward * this.speed * Mathf.Sqrt(this.myLevel)) : (this.baseTransform.forward * this.speed * Mathf.Sqrt(this.myLevel) * 0.2f));
				this.baseGameObjectTransform.rotation = Quaternion.Lerp(this.baseGameObjectTransform.rotation, Quaternion.Euler(0f, this.controller.targetDirection, 0f), this.speed * 0.15f * Time.deltaTime);
				if (this.state == TitanState.idle)
				{
					if (this.controller.isWALKDown || flag)
					{
						if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
						{
							if (!this.baseAnimation.IsPlaying("crawler_run"))
							{
								this.crossFade("crawler_run", 0.1f);
							}
						}
						else if (!this.baseAnimation.IsPlaying("run_walk"))
						{
							this.crossFade("run_walk", 0.1f);
						}
					}
					else if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						if (!this.baseAnimation.IsPlaying("crawler_run"))
						{
							this.crossFade("crawler_run", 0.1f);
						}
						GameObject gameObject = this.checkIfHitCrawlerMouth(this.head, 2.2f);
						if (gameObject != null)
						{
							Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
							if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
							{
								gameObject.GetComponent<HERO>().die((gameObject.transform.position - position) * 15f * this.myLevel, isBite: false);
							}
							else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
							{
								gameObject.GetComponent<HERO>().markDie();
								object[] parameters = new object[5]
								{
									(gameObject.transform.position - position) * 15f * this.myLevel,
									true,
									(!this.nonAI) ? (-1) : base.photonView.viewID,
									base.name,
									true
								};
								gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters);
							}
						}
					}
					else if (!this.baseAnimation.IsPlaying("run_abnormal"))
					{
						this.crossFade("run_abnormal", 0.1f);
					}
				}
			}
			else if (this.state == TitanState.idle)
			{
				if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					if (!this.baseAnimation.IsPlaying("crawler_idle"))
					{
						this.crossFade("crawler_idle", 0.1f);
					}
				}
				else if (!this.baseAnimation.IsPlaying("idle"))
				{
					this.crossFade("idle", 0.1f);
				}
				vector3 = Vector3.zero;
			}
			if (this.state == TitanState.idle)
			{
				Vector3 velocity = this.baseRigidBody.velocity;
				Vector3 force = vector3 - velocity;
				force.x = Mathf.Clamp(force.x, 0f - this.maxVelocityChange, this.maxVelocityChange);
				force.z = Mathf.Clamp(force.z, 0f - this.maxVelocityChange, this.maxVelocityChange);
				force.y = 0f;
				this.baseRigidBody.AddForce(force, ForceMode.VelocityChange);
			}
			else if (this.state == TitanState.attack && this.attackAnimation == "jumper_0")
			{
				Vector3 velocity2 = this.baseRigidBody.velocity;
				Vector3 force2 = vector3 * 0.8f - velocity2;
				force2.x = Mathf.Clamp(force2.x, 0f - this.maxVelocityChange, this.maxVelocityChange);
				force2.z = Mathf.Clamp(force2.z, 0f - this.maxVelocityChange, this.maxVelocityChange);
				force2.y = 0f;
				this.baseRigidBody.AddForce(force2, ForceMode.VelocityChange);
			}
		}
		if ((this.abnormalType == AbnormalType.TYPE_I || this.abnormalType == AbnormalType.TYPE_JUMPER) && !this.nonAI && this.state == TitanState.attack && this.attackAnimation == "jumper_0")
		{
			Vector3 vector4 = this.baseTransform.forward * this.speed * this.myLevel * 0.5f;
			Vector3 velocity3 = this.baseRigidBody.velocity;
			if (this.baseAnimation["attack_jumper_0"].normalizedTime <= 0.28f || this.baseAnimation["attack_jumper_0"].normalizedTime >= 0.8f)
			{
				vector4 = Vector3.zero;
			}
			Vector3 force3 = vector4 - velocity3;
			force3.x = Mathf.Clamp(force3.x, 0f - this.maxVelocityChange, this.maxVelocityChange);
			force3.z = Mathf.Clamp(force3.z, 0f - this.maxVelocityChange, this.maxVelocityChange);
			force3.y = 0f;
			this.baseRigidBody.AddForce(force3, ForceMode.VelocityChange);
		}
		if (this.state != TitanState.chase && this.state != TitanState.wander && this.state != TitanState.to_check_point && this.state != TitanState.to_pvp_pt && this.state != TitanState.random_run)
		{
			return;
		}
		Vector3 vector5 = this.baseTransform.forward * this.speed;
		Vector3 velocity4 = this.baseRigidBody.velocity;
		Vector3 force4 = vector5 - velocity4;
		force4.x = Mathf.Clamp(force4.x, 0f - this.maxVelocityChange, this.maxVelocityChange);
		force4.z = Mathf.Clamp(force4.z, 0f - this.maxVelocityChange, this.maxVelocityChange);
		force4.y = 0f;
		this.baseRigidBody.AddForce(force4, ForceMode.VelocityChange);
		if (!this.stuck && this.abnormalType != AbnormalType.TYPE_CRAWLER && !this.nonAI)
		{
			if (this.baseAnimation.IsPlaying(this.runAnimation) && this.baseRigidBody.velocity.magnitude < this.speed * 0.5f)
			{
				this.stuck = true;
				this.stuckTime = 2f;
				this.stuckTurnAngle = (float)UnityEngine.Random.Range(0, 2) * 140f - 70f;
			}
			if (this.state == TitanState.chase && this.myHero != null && this.myDistance > this.attackDistance && this.myDistance < 150f)
			{
				float num = 0.05f;
				if (this.myDifficulty > 1)
				{
					num += 0.05f;
				}
				if (this.abnormalType != 0)
				{
					num += 0.1f;
				}
				if (UnityEngine.Random.Range(0f, 1f) < num)
				{
					this.stuck = true;
					this.stuckTime = 1f;
					float num2 = UnityEngine.Random.Range(20f, 50f);
					this.stuckTurnAngle = (float)UnityEngine.Random.Range(0, 2) * num2 * 2f - num2;
				}
			}
		}
		float num3 = 0f;
		if (this.state == TitanState.wander)
		{
			num3 = this.baseTransform.rotation.eulerAngles.y - 90f;
		}
		else if (this.state == TitanState.to_check_point || this.state == TitanState.to_pvp_pt || this.state == TitanState.random_run)
		{
			Vector3 vector6 = this.targetCheckPt - this.baseTransform.position;
			num3 = (0f - Mathf.Atan2(vector6.z, vector6.x)) * 57.29578f;
		}
		else
		{
			if (this.myHero == null)
			{
				return;
			}
			Vector3 vector7 = this.myHero.transform.position - this.baseTransform.position;
			num3 = (0f - Mathf.Atan2(vector7.z, vector7.x)) * 57.29578f;
		}
		if (this.stuck)
		{
			this.stuckTime -= Time.deltaTime;
			if (this.stuckTime < 0f)
			{
				this.stuck = false;
			}
			if (this.stuckTurnAngle > 0f)
			{
				this.stuckTurnAngle -= Time.deltaTime * 10f;
			}
			else
			{
				this.stuckTurnAngle += Time.deltaTime * 10f;
			}
			num3 += this.stuckTurnAngle;
		}
		float num4 = 0f - Mathf.DeltaAngle(num3, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f);
		if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			this.baseGameObjectTransform.rotation = Quaternion.Lerp(this.baseGameObjectTransform.rotation, Quaternion.Euler(0f, this.baseGameObjectTransform.rotation.eulerAngles.y + num4, 0f), this.speed * 0.3f * Time.deltaTime / this.myLevel);
		}
		else
		{
			this.baseGameObjectTransform.rotation = Quaternion.Lerp(this.baseGameObjectTransform.rotation, Quaternion.Euler(0f, this.baseGameObjectTransform.rotation.eulerAngles.y + num4, 0f), this.speed * 0.5f * Time.deltaTime / this.myLevel);
		}
	}

	private string[] GetAttackStrategy()
	{
		string[] array = null;
		int num = 0;
		if (this.isAlarm || this.myHero.transform.position.y + 3f <= this.neck.position.y + 10f * this.myLevel)
		{
			if (this.myHero.transform.position.y > this.neck.position.y - 3f * this.myLevel)
			{
				if (this.myDistance < this.attackDistance * 0.5f)
				{
					if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("chkOverHead").position) < 3.6f * this.myLevel)
					{
						array = ((!(this.between2 > 0f)) ? new string[1] { "grab_head_front_l" } : new string[1] { "grab_head_front_r" });
					}
					else if (Mathf.Abs(this.between2) < 90f)
					{
						if (Mathf.Abs(this.between2) < 30f)
						{
							if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("chkFront").position) < 2.5f * this.myLevel)
							{
								array = new string[3] { "attack_bite", "attack_bite", "attack_slap_face" };
							}
						}
						else if (this.between2 > 0f)
						{
							if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("chkFrontRight").position) < 2.5f * this.myLevel)
							{
								array = new string[1] { "attack_bite_r" };
							}
						}
						else if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("chkFrontLeft").position) < 2.5f * this.myLevel)
						{
							array = new string[1] { "attack_bite_l" };
						}
					}
					else if (this.between2 > 0f)
					{
						if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("chkBackRight").position) < 2.8f * this.myLevel)
						{
							array = new string[3] { "grab_head_back_r", "grab_head_back_r", "attack_slap_back" };
						}
					}
					else if (Vector3.Distance(this.myHero.transform.position, base.transform.Find("chkBackLeft").position) < 2.8f * this.myLevel)
					{
						array = new string[3] { "grab_head_back_l", "grab_head_back_l", "attack_slap_back" };
					}
				}
				if (array != null)
				{
					return array;
				}
				if (this.abnormalType == AbnormalType.NORMAL || this.abnormalType == AbnormalType.TYPE_PUNK)
				{
					if (this.myDifficulty <= 0 && UnityEngine.Random.Range(0, 1000) >= 3)
					{
						return array;
					}
					if (Mathf.Abs(this.between2) >= 60f)
					{
						return array;
					}
					return new string[1] { "attack_combo" };
				}
				if (this.abnormalType != AbnormalType.TYPE_I && this.abnormalType != AbnormalType.TYPE_JUMPER)
				{
					return array;
				}
				if (this.myDifficulty > 0 || UnityEngine.Random.Range(0, 100) < 50)
				{
					return new string[1] { "attack_abnormal_jump" };
				}
				return array;
			}
			switch ((Mathf.Abs(this.between2) < 90f) ? ((this.between2 > 0f) ? 1 : 2) : ((!(this.between2 > 0f)) ? 3 : 4))
			{
			case 1:
				if (this.myDistance >= this.attackDistance * 0.25f)
				{
					if (this.myDistance < this.attackDistance * 0.5f)
					{
						if (this.abnormalType != AbnormalType.TYPE_PUNK && this.abnormalType == AbnormalType.NORMAL)
						{
							return new string[3] { "grab_ground_front_r", "grab_ground_front_r", "attack_stomp" };
						}
						return new string[3] { "grab_ground_front_r", "grab_ground_front_r", "attack_abnormal_jump" };
					}
					if (this.abnormalType != AbnormalType.TYPE_PUNK)
					{
						if (this.abnormalType == AbnormalType.NORMAL)
						{
							if (this.myDifficulty <= 0)
							{
								return new string[5] { "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_combo" };
							}
							return new string[3] { "attack_front_ground", "attack_combo", "attack_combo" };
						}
						return new string[1] { "attack_abnormal_jump" };
					}
					return new string[3] { "attack_combo", "attack_combo", "attack_abnormal_jump" };
				}
				if (this.abnormalType != AbnormalType.TYPE_PUNK)
				{
					if (this.abnormalType != 0)
					{
						return new string[1] { "attack_kick" };
					}
					return new string[2] { "attack_front_ground", "attack_stomp" };
				}
				return new string[2] { "attack_kick", "attack_stomp" };
			case 2:
				if (this.myDistance >= this.attackDistance * 0.25f)
				{
					if (this.myDistance < this.attackDistance * 0.5f)
					{
						if (this.abnormalType != AbnormalType.TYPE_PUNK && this.abnormalType == AbnormalType.NORMAL)
						{
							return new string[3] { "grab_ground_front_l", "grab_ground_front_l", "attack_stomp" };
						}
						return new string[3] { "grab_ground_front_l", "grab_ground_front_l", "attack_abnormal_jump" };
					}
					if (this.abnormalType != AbnormalType.TYPE_PUNK)
					{
						if (this.abnormalType == AbnormalType.NORMAL)
						{
							if (this.myDifficulty <= 0)
							{
								return new string[5] { "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_front_ground", "attack_combo" };
							}
							return new string[3] { "attack_front_ground", "attack_combo", "attack_combo" };
						}
						return new string[1] { "attack_abnormal_jump" };
					}
					return new string[3] { "attack_combo", "attack_combo", "attack_abnormal_jump" };
				}
				if (this.abnormalType != AbnormalType.TYPE_PUNK)
				{
					if (this.abnormalType != 0)
					{
						return new string[1] { "attack_kick" };
					}
					return new string[2] { "attack_front_ground", "attack_stomp" };
				}
				return new string[2] { "attack_kick", "attack_stomp" };
			case 3:
				if (this.myDistance >= this.attackDistance * 0.5f)
				{
					return array;
				}
				_ = this.abnormalType;
				return new string[1] { "grab_ground_back_l" };
			case 4:
				if (this.myDistance >= this.attackDistance * 0.5f)
				{
					return array;
				}
				_ = this.abnormalType;
				return new string[1] { "grab_ground_back_r" };
			}
		}
		return array;
	}

	private void getDown()
	{
		this.state = TitanState.down;
		this.isAlarm = true;
		this.playAnimation("sit_hunt_down");
		this.getdownTime = UnityEngine.Random.Range(3f, 5f);
	}

	private GameObject getNearestHero2()
	{
		GameObject result = null;
		float num = float.PositiveInfinity;
		Vector3 position = this.baseTransform.position;
		foreach (HERO player in this.MultiplayerManager.getPlayers())
		{
			float num2 = Vector3.Distance(base.gameObject.transform.position, position);
			if (num2 < num)
			{
				result = player.gameObject;
				num = num2;
			}
		}
		foreach (TITAN_EREN eren in this.MultiplayerManager.getErens())
		{
			float num3 = Vector3.Distance(base.gameObject.transform.position, position);
			if (num3 < num)
			{
				result = eren.gameObject;
				num = num3;
			}
		}
		return result;
	}

	private int getPunkNumber()
	{
		int num = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TITAN>() != null && gameObject.GetComponent<TITAN>().name == "Punk")
			{
				num++;
			}
		}
		return num;
	}

	private void grab(string type)
	{
		this.state = TitanState.grab;
		this.attacked = false;
		this.isAlarm = true;
		this.attackAnimation = type;
		this.crossFade("grab_" + type, 0.1f);
		this.isGrabHandLeft = true;
		this.grabbedTarget = null;
		switch (type)
		{
		case "ground_back_l":
			this.attackCheckTimeA = 0.34f;
			this.attackCheckTimeB = 0.49f;
			break;
		case "ground_back_r":
			this.attackCheckTimeA = 0.34f;
			this.attackCheckTimeB = 0.49f;
			this.isGrabHandLeft = false;
			break;
		case "ground_front_l":
			this.attackCheckTimeA = 0.37f;
			this.attackCheckTimeB = 0.6f;
			break;
		case "ground_front_r":
			this.attackCheckTimeA = 0.37f;
			this.attackCheckTimeB = 0.6f;
			this.isGrabHandLeft = false;
			break;
		case "head_back_l":
			this.attackCheckTimeA = 0.45f;
			this.attackCheckTimeB = 0.5f;
			this.isGrabHandLeft = false;
			break;
		case "head_back_r":
			this.attackCheckTimeA = 0.45f;
			this.attackCheckTimeB = 0.5f;
			break;
		case "head_front_l":
			this.attackCheckTimeA = 0.38f;
			this.attackCheckTimeB = 0.55f;
			break;
		case "head_front_r":
			this.attackCheckTimeA = 0.38f;
			this.attackCheckTimeB = 0.55f;
			this.isGrabHandLeft = false;
			break;
		}
		if (this.isGrabHandLeft)
		{
			this.currentGrabHand = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
		}
		else
		{
			this.currentGrabHand = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
		}
	}

	[RPC]
	public void grabbedTargetEscape(PhotonMessageInfo info)
	{
		if (info != null && info.sender != this.grabbedTarget.GetComponent<PhotonView>().owner && PhotonNetwork.isMasterClient)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "titan grabbedTargetEscape");
		}
		else
		{
			this.grabbedTarget = null;
		}
	}

	[RPC]
	public void grabToLeft(PhotonMessageInfo info)
	{
		if (PhotonNetwork.isMasterClient && info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "titan grabToLeft");
			return;
		}
		Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
		this.grabTF.transform.parent = transform;
		this.grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
		this.grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
		this.grabTF.transform.localPosition -= Vector3.right * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localPosition -= Vector3.up * transform.GetComponent<SphereCollider>().radius * 0.51f;
		this.grabTF.transform.localPosition -= Vector3.forward * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localRotation = Quaternion.Euler(this.grabTF.transform.localRotation.eulerAngles.x, this.grabTF.transform.localRotation.eulerAngles.y + 180f, this.grabTF.transform.localRotation.eulerAngles.z + 180f);
	}

	[RPC]
	public void grabToRight(PhotonMessageInfo info)
	{
		if (PhotonNetwork.isMasterClient && info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "titan grabToLeft");
			return;
		}
		Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
		this.grabTF.transform.parent = transform;
		this.grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
		this.grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
		this.grabTF.transform.localPosition -= Vector3.right * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localPosition += Vector3.up * transform.GetComponent<SphereCollider>().radius * 0.51f;
		this.grabTF.transform.localPosition -= Vector3.forward * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localRotation = Quaternion.Euler(this.grabTF.transform.localRotation.eulerAngles.x, this.grabTF.transform.localRotation.eulerAngles.y + 180f, this.grabTF.transform.localRotation.eulerAngles.z);
	}

	public void headMovement()
	{
		if (!this.hasDie)
		{
			if (IN_GAME_MAIN_CAMERA.gametype != 0)
			{
				if (base.photonView.isMine)
				{
					this.targetHeadRotation = this.head.rotation;
					bool flag = false;
					if (this.abnormalType != AbnormalType.TYPE_CRAWLER && this.state != TitanState.attack && this.state != TitanState.down && this.state != TitanState.hit && this.state != TitanState.recover && this.state != TitanState.eat && this.state != TitanState.hit_eye && !this.hasDie && this.myDistance < 100f && this.myHero != null)
					{
						Vector3 vector = this.myHero.transform.position - base.transform.position;
						this.angle = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
						float value = 0f - Mathf.DeltaAngle(this.angle, base.transform.rotation.eulerAngles.y - 90f);
						value = Mathf.Clamp(value, -40f, 40f);
						float value2 = Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f;
						value2 = Mathf.Clamp(value2, -40f, 30f);
						this.targetHeadRotation = Quaternion.Euler(this.head.rotation.eulerAngles.x + value2, this.head.rotation.eulerAngles.y + value, this.head.rotation.eulerAngles.z);
						if (!this.asClientLookTarget)
						{
							this.asClientLookTarget = true;
							object[] parameters = new object[1] { true };
							base.photonView.RPC("setIfLookTarget", PhotonTargets.Others, parameters);
						}
						flag = true;
					}
					if (!flag && this.asClientLookTarget)
					{
						this.asClientLookTarget = false;
						object[] parameters2 = new object[1] { false };
						base.photonView.RPC("setIfLookTarget", PhotonTargets.Others, parameters2);
					}
					if (this.state == TitanState.attack || this.state == TitanState.hit || this.state == TitanState.hit_eye)
					{
						this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 20f);
					}
					else
					{
						this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f);
					}
				}
				else
				{
					this.targetHeadRotation = this.head.rotation;
					if (this.asClientLookTarget && this.myHero != null)
					{
						Vector3 vector2 = this.myHero.transform.position - base.transform.position;
						this.angle = (0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f;
						float value3 = 0f - Mathf.DeltaAngle(this.angle, base.transform.rotation.eulerAngles.y - 90f);
						value3 = Mathf.Clamp(value3, -40f, 40f);
						float value4 = Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f;
						value4 = Mathf.Clamp(value4, -40f, 30f);
						this.targetHeadRotation = Quaternion.Euler(this.head.rotation.eulerAngles.x + value4, this.head.rotation.eulerAngles.y + value3, this.head.rotation.eulerAngles.z);
					}
					if (!this.hasDie)
					{
						this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f);
					}
				}
			}
			else
			{
				this.targetHeadRotation = this.head.rotation;
				if (this.abnormalType != AbnormalType.TYPE_CRAWLER && this.state != TitanState.attack && this.state != TitanState.down && this.state != TitanState.hit && this.state != TitanState.recover && this.state != TitanState.hit_eye && !this.hasDie && this.myDistance < 100f && this.myHero != null)
				{
					Vector3 vector3 = this.myHero.transform.position - base.transform.position;
					this.angle = (0f - Mathf.Atan2(vector3.z, vector3.x)) * 57.29578f;
					float value5 = 0f - Mathf.DeltaAngle(this.angle, base.transform.rotation.eulerAngles.y - 90f);
					value5 = Mathf.Clamp(value5, -40f, 40f);
					float value6 = Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f;
					value6 = Mathf.Clamp(value6, -40f, 30f);
					this.targetHeadRotation = Quaternion.Euler(this.head.rotation.eulerAngles.x + value6, this.head.rotation.eulerAngles.y + value5, this.head.rotation.eulerAngles.z);
				}
				if (this.state == TitanState.attack || this.state == TitanState.hit || this.state == TitanState.hit_eye)
				{
					this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 20f);
				}
				else
				{
					this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f);
				}
			}
			this.head.rotation = this.oldHeadRotation;
		}
		if (!base.animation.IsPlaying("die_headOff"))
		{
			this.head.localScale = this.headscale;
		}
	}

	public void headMovement2()
	{
		if (!this.hasDie)
		{
			this.targetHeadRotation = this.head.rotation;
			if (!this._ignoreLookTarget && this.abnormalType != AbnormalType.TYPE_CRAWLER && !this.hasDie && this.myDistance < 100f && this.myHero != null)
			{
				Vector3 vector = this.myHero.transform.position - base.transform.position;
				this.angle = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
				float value = 0f - Mathf.DeltaAngle(this.angle, base.transform.rotation.eulerAngles.y - 90f);
				value = Mathf.Clamp(value, -40f, 40f);
				float value2 = Mathf.Atan2(this.neck.position.y + this.myLevel * 2f - this.myHero.transform.position.y, this.myDistance) * 57.29578f;
				value2 = Mathf.Clamp(value2, -40f, 30f);
				this.targetHeadRotation = Quaternion.Euler(this.head.rotation.eulerAngles.x + value2, this.head.rotation.eulerAngles.y + value, this.head.rotation.eulerAngles.z);
			}
			if (this._fastHeadRotation)
			{
				this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 20f);
			}
			else
			{
				this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 10f);
			}
			this.head.rotation = this.oldHeadRotation;
		}
		if (!base.animation.IsPlaying("die_headOff"))
		{
			this.head.localScale = this.headscale;
		}
	}

	private void hit(string animationName, Vector3 attacker, float hitPauseTime)
	{
		this.state = TitanState.hit;
		this.hitAnimation = animationName;
		this.hitPause = hitPauseTime;
		this.playAnimation(this.hitAnimation);
		base.animation[this.hitAnimation].time = 0f;
		base.animation[this.hitAnimation].speed = 0f;
		base.transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - base.transform.position).eulerAngles.y, 0f);
		this.needFreshCorePosition = true;
		if (base.photonView.isMine && this.grabbedTarget != null)
		{
			this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
		}
	}

	public void hitAnkle()
	{
		if (!this.hasDie && this.state != TitanState.down)
		{
			if (this.grabbedTarget != null)
			{
				this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
			}
			this.getDown();
		}
	}

	[RPC]
	public void hitAnkleRPC(int viewID)
	{
		if (this.hasDie || this.state == TitanState.down)
		{
			return;
		}
		PhotonView photonView = PhotonView.Find(viewID);
		if (photonView != null && (photonView.gameObject.transform.position - base.transform.position).magnitude < 20f)
		{
			if (base.photonView.isMine && this.grabbedTarget != null)
			{
				this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
			}
			this.getDown();
		}
	}

	public void hitEye()
	{
		if (!this.hasDie)
		{
			this.justHitEye();
		}
	}

	[RPC]
	public void hitEyeRPC(int viewID)
	{
		if (!this.hasDie && (PhotonView.Find(viewID).gameObject.transform.position - this.neck.position).magnitude < 20f)
		{
			if (base.photonView.isMine && this.grabbedTarget != null)
			{
				this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
			}
			if (!this.hasDie)
			{
				this.justHitEye();
			}
		}
	}

	public void hitL(Vector3 attacker, float hitPauseTime)
	{
		if (this.abnormalType != AbnormalType.TYPE_CRAWLER)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.hit("hit_eren_L", attacker, hitPauseTime);
				return;
			}
			object[] parameters = new object[2] { attacker, hitPauseTime };
			base.photonView.RPC("hitLRPC", PhotonTargets.All, parameters);
		}
	}

	[RPC]
	private void hitLRPC(Vector3 attacker, float hitPauseTime)
	{
		if (base.photonView.isMine && (attacker - base.transform.position).magnitude < 80f)
		{
			this.hit("hit_eren_L", attacker, hitPauseTime);
		}
	}

	public void hitR(Vector3 attacker, float hitPauseTime)
	{
		if (this.abnormalType != AbnormalType.TYPE_CRAWLER)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.hit("hit_eren_R", attacker, hitPauseTime);
				return;
			}
			object[] parameters = new object[2] { attacker, hitPauseTime };
			base.photonView.RPC("hitRRPC", PhotonTargets.All, parameters);
		}
	}

	[RPC]
	private void hitRRPC(Vector3 attacker, float hitPauseTime)
	{
		if (base.photonView.isMine && !this.hasDie && (attacker - base.transform.position).magnitude < 80f)
		{
			this.hit("hit_eren_R", attacker, hitPauseTime);
		}
	}

	private void idle(float sbtime = 0f)
	{
		this.stuck = false;
		this.sbtime = sbtime;
		if (this.myDifficulty == 2 && (this.abnormalType == AbnormalType.TYPE_JUMPER || this.abnormalType == AbnormalType.TYPE_I))
		{
			this.sbtime = UnityEngine.Random.Range(0f, 1.5f);
		}
		else if (this.myDifficulty >= 1)
		{
			this.sbtime = 0f;
		}
		this.sbtime = Mathf.Max(0.5f, this.sbtime);
		if (this.abnormalType == AbnormalType.TYPE_PUNK)
		{
			this.sbtime = 0.1f;
			if (this.myDifficulty == 1)
			{
				this.sbtime += 0.4f;
			}
		}
		this.state = TitanState.idle;
		if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			this.crossFadeIfNotPlaying("crawler_idle", 0.2f);
		}
		else
		{
			this.crossFadeIfNotPlaying("idle", 0.2f);
		}
	}

	public bool IsGrounded()
	{
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = (int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyAABB")) | (int)layerMask;
		return Physics.Raycast(base.gameObject.transform.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, layerMask2.value);
	}

	private void justEatHero(GameObject target, Transform hand)
	{
		if (!(target != null))
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			if (!target.GetComponent<HERO>().HasDied())
			{
				target.GetComponent<HERO>().markDie();
				if (this.nonAI)
				{
					object[] parameters = new object[2]
					{
						base.photonView.viewID,
						base.name
					};
					target.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, parameters);
				}
				else
				{
					object[] parameters2 = new object[2] { -1, base.name };
					target.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, parameters2);
				}
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			target.GetComponent<HERO>().die2(hand);
		}
	}

	private void justHitEye()
	{
		if (this.state != TitanState.hit_eye)
		{
			if (this.state == TitanState.down || this.state == TitanState.sit)
			{
				this.playAnimation("sit_hit_eye");
			}
			else
			{
				this.playAnimation("hit_eye");
			}
			this.state = TitanState.hit_eye;
		}
	}

	[RPC]
	public void labelRPC(int health, int maxHealth)
	{
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
			this.healthLabel.transform.localPosition = new Vector3(0f, 20f + 1f / this.myLevel, 0f);
			if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
			{
				this.healthLabel.transform.localPosition = new Vector3(0f, 10f + 1f / this.myLevel, 0f);
			}
			float num = 1f;
			if (this.myLevel < 1f)
			{
				num = 1f / this.myLevel;
			}
			this.healthLabel.transform.localScale = new Vector3(num, num, num);
			this.healthLabelEnabled = true;
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

	public void lateUpdate()
	{
		if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return;
		}
		if (base.animation.IsPlaying("run_walk"))
		{
			if (base.animation["run_walk"].normalizedTime % 1f > 0.1f && base.animation["run_walk"].normalizedTime % 1f < 0.6f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				Transform obj = base.transform.Find("snd_titan_foot");
				obj.GetComponent<AudioSource>().Stop();
				obj.GetComponent<AudioSource>().Play();
			}
			if (base.animation["run_walk"].normalizedTime % 1f > 0.6f && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				Transform obj2 = base.transform.Find("snd_titan_foot");
				obj2.GetComponent<AudioSource>().Stop();
				obj2.GetComponent<AudioSource>().Play();
			}
		}
		if (base.animation.IsPlaying("crawler_run"))
		{
			if (base.animation["crawler_run"].normalizedTime % 1f > 0.1f && base.animation["crawler_run"].normalizedTime % 1f < 0.56f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				Transform obj3 = base.transform.Find("snd_titan_foot");
				obj3.GetComponent<AudioSource>().Stop();
				obj3.GetComponent<AudioSource>().Play();
			}
			if (base.animation["crawler_run"].normalizedTime % 1f > 0.56f && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				Transform obj4 = base.transform.Find("snd_titan_foot");
				obj4.GetComponent<AudioSource>().Stop();
				obj4.GetComponent<AudioSource>().Play();
			}
		}
		if (base.animation.IsPlaying("run_abnormal"))
		{
			if (base.animation["run_abnormal"].normalizedTime % 1f > 0.47f && base.animation["run_abnormal"].normalizedTime % 1f < 0.95f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				Transform obj5 = base.transform.Find("snd_titan_foot");
				obj5.GetComponent<AudioSource>().Stop();
				obj5.GetComponent<AudioSource>().Play();
			}
			if ((base.animation["run_abnormal"].normalizedTime % 1f > 0.95f || base.animation["run_abnormal"].normalizedTime % 1f < 0.47f) && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				Transform obj6 = base.transform.Find("snd_titan_foot");
				obj6.GetComponent<AudioSource>().Stop();
				obj6.GetComponent<AudioSource>().Play();
			}
		}
		this.headMovement();
		this.grounded = false;
	}

	public void lateUpdate2()
	{
		if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return;
		}
		if (this.baseAnimation.IsPlaying("run_walk"))
		{
			if (this.baseAnimation["run_walk"].normalizedTime % 1f > 0.1f && this.baseAnimation["run_walk"].normalizedTime % 1f < 0.6f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				this.baseAudioSource.Stop();
				this.baseAudioSource.Play();
			}
			else if (this.baseAnimation["run_walk"].normalizedTime % 1f > 0.6f && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				this.baseAudioSource.Stop();
				this.baseAudioSource.Play();
			}
		}
		else if (this.baseAnimation.IsPlaying("crawler_run"))
		{
			if (this.baseAnimation["crawler_run"].normalizedTime % 1f > 0.1f && this.baseAnimation["crawler_run"].normalizedTime % 1f < 0.56f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				this.baseAudioSource.Stop();
				this.baseAudioSource.Play();
			}
			else if (this.baseAnimation["crawler_run"].normalizedTime % 1f > 0.56f && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				this.baseAudioSource.Stop();
				this.baseAudioSource.Play();
			}
		}
		else if (this.baseAnimation.IsPlaying("run_abnormal"))
		{
			if (this.baseAnimation["run_abnormal"].normalizedTime % 1f > 0.47f && this.baseAnimation["run_abnormal"].normalizedTime % 1f < 0.95f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				this.baseAudioSource.Stop();
				this.baseAudioSource.Play();
			}
			else if ((this.baseAnimation["run_abnormal"].normalizedTime % 1f > 0.95f || this.baseAnimation["run_abnormal"].normalizedTime % 1f < 0.47f) && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				this.baseAudioSource.Stop();
				this.baseAudioSource.Play();
			}
		}
		this.headMovement2();
		this.grounded = false;
		this.updateLabel();
		this.updateCollider();
	}

	[RPC]
	private void laugh(float sbtime = 0f)
	{
		if (this.state == TitanState.idle || this.state == TitanState.turn || this.state == TitanState.chase)
		{
			this.sbtime = sbtime;
			this.state = TitanState.laugh;
			this.crossFade("laugh", 0.2f);
		}
	}

	public void loadskin()
	{
		this.skin = 0;
		this.eye = false;
		BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
		if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine) && titan.SkinsEnabled.Value)
		{
			int num = (int)UnityEngine.Random.Range(0f, 5f);
			int index = num;
			TitanCustomSkinSet obj = (TitanCustomSkinSet)titan.GetSelectedSet();
			if (obj.RandomizedPairs.Value)
			{
				index = (int)UnityEngine.Random.Range(0f, 5f);
			}
			string value = ((StringSetting)obj.Bodies.GetItemAt(num)).Value;
			string value2 = ((StringSetting)obj.Eyes.GetItemAt(index)).Value;
			this.skin = num;
			if (value2.EndsWith(".jpg") || value2.EndsWith(".png") || value2.EndsWith(".jpeg"))
			{
				this.eye = true;
			}
			base.GetComponent<TITAN_SETUP>().setVar(this.skin, this.eye);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				base.StartCoroutine(this.loadskinE(value, value2));
				return;
			}
			base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, value, value2);
		}
	}

	public IEnumerator loadskinE(string body, string eye)
	{
		while (!this.hasSpawn)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this._customSkinLoader.LoadSkinsFromRPC(new object[3] { false, body, eye }));
	}

	[RPC]
	public void loadskinRPC(string body, string eye, PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
			if (titan.SkinsEnabled.Value && (!titan.SkinsLocal.Value || base.photonView.isMine))
			{
				base.StartCoroutine(this.loadskinE(body, eye));
			}
		}
	}

	private bool longRangeAttackCheck()
	{
		if (this.abnormalType == AbnormalType.TYPE_PUNK && this.myHero != null && this.myHero.rigidbody != null)
		{
			Vector3 vector = this.myHero.rigidbody.velocity * Time.deltaTime * 30f;
			if (vector.sqrMagnitude > 10f)
			{
				if (this.simpleHitTestLineAndBall(vector, base.transform.Find("chkAeLeft").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack("anti_AE_l");
					return true;
				}
				if (this.simpleHitTestLineAndBall(vector, base.transform.Find("chkAeLLeft").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack("anti_AE_low_l");
					return true;
				}
				if (this.simpleHitTestLineAndBall(vector, base.transform.Find("chkAeRight").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack("anti_AE_r");
					return true;
				}
				if (this.simpleHitTestLineAndBall(vector, base.transform.Find("chkAeLRight").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack("anti_AE_low_r");
					return true;
				}
			}
			Vector3 vector2 = this.myHero.transform.position - base.transform.position;
			float f = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f, base.gameObject.transform.rotation.eulerAngles.y - 90f);
			if (this.rockInterval > 0f)
			{
				this.rockInterval -= Time.deltaTime;
			}
			else if (Mathf.Abs(f) < 5f)
			{
				float sqrMagnitude = (this.myHero.transform.position + vector - base.transform.position).sqrMagnitude;
				if (sqrMagnitude > 8000f && sqrMagnitude < 90000f)
				{
					this.attack("throw");
					this.rockInterval = 2f;
					return true;
				}
			}
		}
		return false;
	}

	private bool longRangeAttackCheck2()
	{
		if (this.abnormalType == AbnormalType.TYPE_PUNK && this.myHero != null)
		{
			Vector3 vector = this.myHero.rigidbody.velocity * Time.deltaTime * 30f;
			if (vector.sqrMagnitude > 10f)
			{
				if (this.simpleHitTestLineAndBall(vector, this.baseTransform.Find("chkAeLeft").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack2("anti_AE_l");
					return true;
				}
				if (this.simpleHitTestLineAndBall(vector, this.baseTransform.Find("chkAeLLeft").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack2("anti_AE_low_l");
					return true;
				}
				if (this.simpleHitTestLineAndBall(vector, this.baseTransform.Find("chkAeRight").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack2("anti_AE_r");
					return true;
				}
				if (this.simpleHitTestLineAndBall(vector, this.baseTransform.Find("chkAeLRight").position - this.myHero.transform.position, 5f * this.myLevel))
				{
					this.attack2("anti_AE_low_r");
					return true;
				}
			}
			Vector3 vector2 = this.myHero.transform.position - this.baseTransform.position;
			float f = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector2.z, vector2.x)) * 57.29578f, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f);
			if (this.rockInterval > 0f)
			{
				this.rockInterval -= Time.deltaTime;
			}
			else if (Mathf.Abs(f) < 5f)
			{
				float sqrMagnitude = (this.myHero.transform.position + vector - this.baseTransform.position).sqrMagnitude;
				if (sqrMagnitude > 8000f && sqrMagnitude < 90000f && SettingsManager.LegacyGameSettings.RockThrowEnabled.Value)
				{
					this.attack2("throw");
					this.rockInterval = 2f;
					return true;
				}
			}
		}
		return false;
	}

	public void moveTo(float posX, float posY, float posZ)
	{
		base.transform.position = new Vector3(posX, posY, posZ);
	}

	[RPC]
	public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient)
		{
			base.transform.position = new Vector3(posX, posY, posZ);
		}
	}

	[RPC]
	private void netCrossFade(string aniName, float time)
	{
		base.animation.CrossFade(aniName, time);
		this.CheckAnimationLookTarget(aniName);
	}

	[RPC]
	private void netDie()
	{
		this.asClientLookTarget = false;
		if (!this.hasDie)
		{
			this.hasDie = true;
			if (this.nonAI)
			{
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
				ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.dead, true);
				PhotonNetwork.player.SetCustomProperties(hashtable);
				hashtable = new ExitGames.Client.Photon.Hashtable();
				hashtable.Add(PhotonPlayerProperty.deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths] + 1);
				PhotonNetwork.player.SetCustomProperties(hashtable);
			}
			this.dieAnimation();
		}
	}

	[RPC]
	private void netPlayAnimation(string aniName)
	{
		base.animation.Play(aniName);
		this.CheckAnimationLookTarget(aniName);
	}

	[RPC]
	private void netPlayAnimationAt(string aniName, float normalizedTime)
	{
		base.animation.Play(aniName);
		this.CheckAnimationLookTarget(aniName);
		base.animation[aniName].normalizedTime = normalizedTime;
	}

	[RPC]
	private void netSetAbnormalType(int type)
	{
		if (!this.hasload)
		{
			this.hasload = true;
			this.loadskin();
		}
		switch (type)
		{
		case 0:
			this.abnormalType = AbnormalType.NORMAL;
			base.name = "Titan";
			this.runAnimation = "run_walk";
			base.GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 1:
			this.abnormalType = AbnormalType.TYPE_I;
			base.name = "Aberrant";
			this.runAnimation = "run_abnormal";
			base.GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 2:
			this.abnormalType = AbnormalType.TYPE_JUMPER;
			base.name = "Jumper";
			this.runAnimation = "run_abnormal";
			base.GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 3:
			this.abnormalType = AbnormalType.TYPE_CRAWLER;
			base.name = "Crawler";
			this.runAnimation = "crawler_run";
			base.GetComponent<TITAN_SETUP>().setHair2();
			break;
		case 4:
			this.abnormalType = AbnormalType.TYPE_PUNK;
			base.name = "Punk";
			this.runAnimation = "run_abnormal_1";
			base.GetComponent<TITAN_SETUP>().setHair2();
			break;
		}
		if (this.abnormalType == AbnormalType.TYPE_I || this.abnormalType == AbnormalType.TYPE_JUMPER || this.abnormalType == AbnormalType.TYPE_PUNK)
		{
			this.speed = 18f;
			if (this.myLevel > 1f)
			{
				this.speed *= Mathf.Sqrt(this.myLevel);
			}
			if (this.myDifficulty == 1)
			{
				this.speed *= 1.4f;
			}
			if (this.myDifficulty == 2)
			{
				this.speed *= 1.6f;
			}
			this.baseAnimation["turnaround1"].speed = 2f;
			this.baseAnimation["turnaround2"].speed = 2f;
		}
		if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			this.chaseDistance += 50f;
			this.speed = 25f;
			if (this.myLevel > 1f)
			{
				this.speed *= Mathf.Sqrt(this.myLevel);
			}
			if (this.myDifficulty == 1)
			{
				this.speed *= 2f;
			}
			if (this.myDifficulty == 2)
			{
				this.speed *= 2.2f;
			}
			this.baseTransform.Find("AABB").gameObject.GetComponent<CapsuleCollider>().height = 10f;
			this.baseTransform.Find("AABB").gameObject.GetComponent<CapsuleCollider>().radius = 5f;
			this.baseTransform.Find("AABB").gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, 5.05f, 0f);
		}
		if (this.nonAI)
		{
			if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
			{
				this.speed = Mathf.Min(70f, this.speed);
			}
			else
			{
				this.speed = Mathf.Min(60f, this.speed);
			}
			this.baseAnimation["attack_jumper_0"].speed = 7f;
			this.baseAnimation["attack_crawler_jump_0"].speed = 4f;
		}
		this.baseAnimation["attack_combo_1"].speed = 1f;
		this.baseAnimation["attack_combo_2"].speed = 1f;
		this.baseAnimation["attack_combo_3"].speed = 1f;
		this.baseAnimation["attack_quick_turn_l"].speed = 1f;
		this.baseAnimation["attack_quick_turn_r"].speed = 1f;
		this.baseAnimation["attack_anti_AE_l"].speed = 1.1f;
		this.baseAnimation["attack_anti_AE_low_l"].speed = 1.1f;
		this.baseAnimation["attack_anti_AE_r"].speed = 1.1f;
		this.baseAnimation["attack_anti_AE_low_r"].speed = 1.1f;
		this.idle();
	}

	[RPC]
	private void netSetLevel(float level, int AI, int skinColor, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient && !info.sender.isLocal && base.photonView.owner != info.sender)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "titan netSetLevel");
		}
		this.setLevel2(level, AI, skinColor);
		if (level > 5f)
		{
			this.headscale = new Vector3(1f, 1f, 1f);
		}
		else if (level < 1f && FengGameManagerMKII.level.StartsWith("Custom"))
		{
			this.myTitanTrigger.GetComponent<CapsuleCollider>().radius *= 2.5f - level;
		}
	}

	private void OnCollisionStay()
	{
		this.grounded = true;
	}

	private void OnDestroy()
	{
		if (GameObject.Find("MultiplayerManager") != null)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeTitan(this);
		}
	}

	public void OnTitanDie(PhotonView view)
	{
		if (FengGameManagerMKII.logicLoaded && FengGameManagerMKII.RCEvents.ContainsKey("OnTitanDie"))
		{
			RCEvent obj = (RCEvent)FengGameManagerMKII.RCEvents["OnTitanDie"];
			string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnTitanDie"];
			if (FengGameManagerMKII.titanVariables.ContainsKey(array[0]))
			{
				FengGameManagerMKII.titanVariables[array[0]] = this;
			}
			else
			{
				FengGameManagerMKII.titanVariables.Add(array[0], this);
			}
			if (FengGameManagerMKII.playerVariables.ContainsKey(array[1]))
			{
				FengGameManagerMKII.playerVariables[array[1]] = view.owner;
			}
			else
			{
				FengGameManagerMKII.playerVariables.Add(array[1], view.owner);
			}
			obj.checkEvent();
		}
	}

	private void playAnimation(string aniName)
	{
		base.animation.Play(aniName);
		this.CheckAnimationLookTarget(aniName);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			object[] parameters = new object[1] { aniName };
			base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
		}
	}

	private void playAnimationAt(string aniName, float normalizedTime)
	{
		base.animation.Play(aniName);
		this.CheckAnimationLookTarget(aniName);
		base.animation[aniName].normalizedTime = normalizedTime;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			object[] parameters = new object[2] { aniName, normalizedTime };
			base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
		}
	}

	private void playSound(string sndname)
	{
		this.playsoundRPC(sndname);
		if (base.photonView.isMine)
		{
			object[] parameters = new object[1] { sndname };
			base.photonView.RPC("playsoundRPC", PhotonTargets.Others, parameters);
		}
	}

	[RPC]
	private void playsoundRPC(string sndname)
	{
		base.transform.Find(sndname).GetComponent<AudioSource>().Play();
	}

	public void pt()
	{
		if (this.controller.bite)
		{
			this.attack2("bite");
		}
		if (this.controller.bitel)
		{
			this.attack2("bite_l");
		}
		if (this.controller.biter)
		{
			this.attack2("bite_r");
		}
		if (this.controller.chopl)
		{
			this.attack2("anti_AE_low_l");
		}
		if (this.controller.chopr)
		{
			this.attack2("anti_AE_low_r");
		}
		if (this.controller.choptl)
		{
			this.attack2("anti_AE_l");
		}
		if (this.controller.choptr)
		{
			this.attack2("anti_AE_r");
		}
		if (this.controller.cover && this.stamina > 75f)
		{
			this.recoverpt();
			this.stamina -= 75f;
		}
		if (this.controller.grabbackl)
		{
			this.grab("ground_back_l");
		}
		if (this.controller.grabbackr)
		{
			this.grab("ground_back_r");
		}
		if (this.controller.grabfrontl)
		{
			this.grab("ground_front_l");
		}
		if (this.controller.grabfrontr)
		{
			this.grab("ground_front_r");
		}
		if (this.controller.grabnapel)
		{
			this.grab("head_back_l");
		}
		if (this.controller.grabnaper)
		{
			this.grab("head_back_r");
		}
	}

	public void randomRun(Vector3 targetPt, float r)
	{
		this.state = TitanState.random_run;
		this.targetCheckPt = targetPt;
		this.targetR = r;
		this.random_run_time = UnityEngine.Random.Range(1f, 2f);
		this.crossFade(this.runAnimation, 0.5f);
	}

	private void recover()
	{
		this.state = TitanState.recover;
		this.playAnimation("idle_recovery");
		this.getdownTime = UnityEngine.Random.Range(2f, 5f);
	}

	private void recoverpt()
	{
		this.state = TitanState.recover;
		this.playAnimation("idle_recovery");
		this.getdownTime = UnityEngine.Random.Range(1.8f, 2.5f);
	}

	private void remainSitdown()
	{
		this.state = TitanState.sit;
		this.playAnimation("sit_idle");
		this.getdownTime = UnityEngine.Random.Range(10f, 30f);
	}

	public void resetLevel(float level)
	{
		this.myLevel = level;
		this.setmyLevel();
	}

	public void setAbnormalType(AbnormalType type, bool forceCrawler = false)
	{
		int num = 0;
		float num2 = 0.02f * (float)(IN_GAME_MAIN_CAMERA.difficulty + 1);
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			num2 = 100f;
		}
		switch (type)
		{
		case AbnormalType.NORMAL:
			num = ((UnityEngine.Random.Range(0f, 1f) < num2) ? 4 : 0);
			break;
		case AbnormalType.TYPE_I:
			num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 1 : 4);
			break;
		case AbnormalType.TYPE_JUMPER:
			num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 2 : 4);
			break;
		case AbnormalType.TYPE_CRAWLER:
			num = 3;
			if (GameObject.Find("Crawler") != null && UnityEngine.Random.Range(0, 1000) > 5)
			{
				num = 2;
			}
			break;
		case AbnormalType.TYPE_PUNK:
			num = 4;
			break;
		}
		if (forceCrawler)
		{
			num = 3;
		}
		if (num == 4)
		{
			if (!LevelInfo.getInfo(FengGameManagerMKII.level).punk)
			{
				num = 1;
			}
			else
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && this.getPunkNumber() >= 3)
				{
					num = 1;
				}
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					int wave = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().wave;
					if (wave != 5 && wave != 10 && wave != 15 && wave != 20)
					{
						num = 1;
					}
				}
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
		{
			object[] parameters = new object[1] { num };
			base.photonView.RPC("netSetAbnormalType", PhotonTargets.AllBuffered, parameters);
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			this.netSetAbnormalType(num);
		}
	}

	public void setAbnormalType2(AbnormalType type, bool forceCrawler)
	{
		bool flag = false;
		if (SettingsManager.LegacyGameSettings.TitanSpawnEnabled.Value)
		{
			flag = true;
		}
		if (FengGameManagerMKII.level.StartsWith("Custom"))
		{
			flag = true;
		}
		int num = 0;
		float num2 = 0.02f * (float)(IN_GAME_MAIN_CAMERA.difficulty + 1);
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_AHSS)
		{
			num2 = 100f;
		}
		switch (type)
		{
		case AbnormalType.NORMAL:
			num = ((UnityEngine.Random.Range(0f, 1f) < num2) ? 4 : 0);
			if (flag)
			{
				num = 0;
			}
			break;
		case AbnormalType.TYPE_I:
			num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 1 : 4);
			if (flag)
			{
				num = 1;
			}
			break;
		case AbnormalType.TYPE_JUMPER:
			num = ((!(UnityEngine.Random.Range(0f, 1f) < num2)) ? 2 : 4);
			if (flag)
			{
				num = 2;
			}
			break;
		case AbnormalType.TYPE_CRAWLER:
			num = 3;
			if (GameObject.Find("Crawler") != null && UnityEngine.Random.Range(0, 1000) > 5)
			{
				num = 2;
			}
			if (flag)
			{
				num = 3;
			}
			break;
		case AbnormalType.TYPE_PUNK:
			num = 4;
			break;
		}
		if (forceCrawler)
		{
			num = 3;
		}
		if (num == 4)
		{
			if (!LevelInfo.getInfo(FengGameManagerMKII.level).punk)
			{
				num = 1;
			}
			else
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && this.getPunkNumber() >= 3)
				{
					num = 1;
				}
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
				{
					int wave = FengGameManagerMKII.instance.wave;
					if (wave != 5 && wave != 10 && wave != 15 && wave != 20)
					{
						num = 1;
					}
				}
			}
			if (flag)
			{
				num = 4;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
		{
			object[] parameters = new object[1] { num };
			base.photonView.RPC("netSetAbnormalType", PhotonTargets.AllBuffered, parameters);
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			this.netSetAbnormalType(num);
		}
	}

	[RPC]
	private void setIfLookTarget(bool bo)
	{
		this.asClientLookTarget = bo;
	}

	private void setLevel(float level, int AI, int skinColor)
	{
		this.myLevel = level;
		this.myLevel = Mathf.Clamp(this.myLevel, 0.7f, 3f);
		this.attackWait += UnityEngine.Random.Range(0f, 2f);
		this.chaseDistance += this.myLevel * 10f;
		base.transform.localScale = new Vector3(this.myLevel, this.myLevel, this.myLevel);
		float num = Mathf.Min(Mathf.Pow(2f / this.myLevel, 0.35f), 1.25f);
		this.headscale = new Vector3(num, num, num);
		this.head = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
		this.head.localScale = this.headscale;
		if (skinColor != 0)
		{
			this.mainMaterial.GetComponent<SkinnedMeshRenderer>().material.color = skinColor switch
			{
				1 => FengColor.titanSkin1, 
				2 => FengColor.titanSkin2, 
				_ => FengColor.titanSkin3, 
			};
		}
		float value = 1.4f - (this.myLevel - 0.7f) * 0.15f;
		value = Mathf.Clamp(value, 0.9f, 1.5f);
		foreach (AnimationState item in base.animation)
		{
			item.speed = value;
		}
		base.rigidbody.mass *= this.myLevel;
		base.rigidbody.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f);
		if (this.myLevel > 1f)
		{
			this.speed *= Mathf.Sqrt(this.myLevel);
		}
		this.myDifficulty = AI;
		if (this.myDifficulty == 1 || this.myDifficulty == 2)
		{
			foreach (AnimationState item2 in base.animation)
			{
				item2.speed = value * 1.05f;
			}
			if (this.nonAI)
			{
				this.speed *= 1.1f;
			}
			else
			{
				this.speed *= 1.4f;
			}
			this.chaseDistance *= 1.15f;
		}
		if (this.myDifficulty == 2)
		{
			foreach (AnimationState item3 in base.animation)
			{
				item3.speed = value * 1.05f;
			}
			if (this.nonAI)
			{
				this.speed *= 1.1f;
			}
			else
			{
				this.speed *= 1.5f;
			}
			this.chaseDistance *= 1.3f;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
		{
			this.chaseDistance = 999999f;
		}
		if (this.nonAI)
		{
			if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
			{
				this.speed = Mathf.Min(70f, this.speed);
			}
			else
			{
				this.speed = Mathf.Min(60f, this.speed);
			}
		}
		this.attackDistance = Vector3.Distance(base.transform.position, base.transform.Find("ap_front_ground").position) * 1.65f;
	}

	private void setLevel2(float level, int AI, int skinColor)
	{
		this.myLevel = level;
		this.myLevel = Mathf.Clamp(this.myLevel, 0.1f, 50f);
		this.attackWait += UnityEngine.Random.Range(0f, 2f);
		this.chaseDistance += this.myLevel * 10f;
		base.transform.localScale = new Vector3(this.myLevel, this.myLevel, this.myLevel);
		float num = Mathf.Min(Mathf.Pow(2f / this.myLevel, 0.35f), 1.25f);
		this.headscale = new Vector3(num, num, num);
		this.head = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
		this.head.localScale = this.headscale;
		if (skinColor != 0)
		{
			this.mainMaterial.GetComponent<SkinnedMeshRenderer>().material.color = skinColor switch
			{
				1 => FengColor.titanSkin1, 
				2 => FengColor.titanSkin2, 
				_ => FengColor.titanSkin3, 
			};
		}
		float value = 1.4f - (this.myLevel - 0.7f) * 0.15f;
		value = Mathf.Clamp(value, 0.9f, 1.5f);
		foreach (AnimationState item in base.animation)
		{
			item.speed = value;
		}
		base.rigidbody.mass *= this.myLevel;
		base.rigidbody.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0, 360), 0f);
		if (this.myLevel > 1f)
		{
			this.speed *= Mathf.Sqrt(this.myLevel);
		}
		this.myDifficulty = AI;
		if (this.myDifficulty == 1 || this.myDifficulty == 2)
		{
			foreach (AnimationState item2 in base.animation)
			{
				item2.speed = value * 1.05f;
			}
			if (this.nonAI)
			{
				this.speed *= 1.1f;
			}
			else
			{
				this.speed *= 1.4f;
			}
			this.chaseDistance *= 1.15f;
		}
		if (this.myDifficulty == 2)
		{
			foreach (AnimationState item3 in base.animation)
			{
				item3.speed = value * 1.05f;
			}
			if (this.nonAI)
			{
				this.speed *= 1.1f;
			}
			else
			{
				this.speed *= 1.5f;
			}
			this.chaseDistance *= 1.3f;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.ENDLESS_TITAN || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.SURVIVE_MODE)
		{
			this.chaseDistance = 999999f;
		}
		if (this.nonAI)
		{
			if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
			{
				this.speed = Mathf.Min(70f, this.speed);
			}
			else
			{
				this.speed = Mathf.Min(60f, this.speed);
			}
		}
		this.attackDistance = Vector3.Distance(base.transform.position, base.transform.Find("ap_front_ground").position) * 1.65f;
	}

	private void setmyLevel()
	{
		base.animation.cullingType = AnimationCullingType.BasedOnRenderers;
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
		{
			object[] parameters = new object[3]
			{
				this.myLevel,
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().difficulty,
				UnityEngine.Random.Range(0, 4)
			};
			base.photonView.RPC("netSetLevel", PhotonTargets.AllBuffered, parameters);
			base.animation.cullingType = AnimationCullingType.AlwaysAnimate;
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			this.setLevel2(this.myLevel, IN_GAME_MAIN_CAMERA.difficulty, UnityEngine.Random.Range(0, 4));
		}
	}

	[RPC]
	private void setMyTarget(int ID)
	{
		if (ID == -1)
		{
			this.myHero = null;
		}
		PhotonView photonView = PhotonView.Find(ID);
		if (photonView != null)
		{
			this.myHero = photonView.gameObject;
		}
	}

	public void setRoute(GameObject route)
	{
		this.checkPoints = new ArrayList();
		for (int i = 1; i <= 10; i++)
		{
			this.checkPoints.Add(route.transform.Find("r" + i).position);
		}
		this.checkPoints.Add("end");
	}

	private bool simpleHitTestLineAndBall(Vector3 line, Vector3 ball, float R)
	{
		Vector3 vector = Vector3.Project(ball, line);
		if ((ball - vector).magnitude > R)
		{
			return false;
		}
		if (Vector3.Dot(line, vector) < 0f)
		{
			return false;
		}
		if (vector.sqrMagnitude > line.sqrMagnitude)
		{
			return false;
		}
		return true;
	}

	private void sitdown()
	{
		this.state = TitanState.sit;
		this.playAnimation("sit_down");
		this.getdownTime = UnityEngine.Random.Range(10f, 30f);
	}

	private void Start()
	{
		this.MultiplayerManager.addTitan(this);
		if (Minimap.instance != null)
		{
			Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.yellow, trackOrientation: false, depthAboveAll: true);
		}
		this.currentCamera = GameObject.Find("MainCamera");
		this.runAnimation = "run_walk";
		this.grabTF = new GameObject();
		this.grabTF.name = "titansTmpGrabTF";
		this.head = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
		this.neck = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
		this.oldHeadRotation = this.head.rotation;
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || base.photonView.isMine)
		{
			if (!this.hasSetLevel)
			{
				this.myLevel = UnityEngine.Random.Range(0.7f, 3f);
				if (SettingsManager.LegacyGameSettings.TitanSizeEnabled.Value)
				{
					float value = SettingsManager.LegacyGameSettings.TitanSizeMin.Value;
					float value2 = SettingsManager.LegacyGameSettings.TitanSizeMax.Value;
					this.myLevel = UnityEngine.Random.Range(value, value2);
				}
				this.hasSetLevel = true;
			}
			this.spawnPt = this.baseTransform.position;
			this.setmyLevel();
			this.setAbnormalType2(this.abnormalType, forceCrawler: false);
			if (this.myHero == null)
			{
				this.findNearestHero2();
			}
			this.controller = base.gameObject.GetComponent<TITAN_CONTROLLER>();
			base.StartCoroutine(this.HandleSpawnCollisionCoroutine(2f, 20f));
		}
		if (this.maxHealth == 0 && SettingsManager.LegacyGameSettings.TitanHealthMode.Value > 0)
		{
			if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value == 1)
			{
				this.maxHealth = (this.currentHealth = UnityEngine.Random.Range(SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value + 1));
			}
			else if (SettingsManager.LegacyGameSettings.TitanHealthMode.Value == 2)
			{
				this.maxHealth = (this.currentHealth = Mathf.Clamp(Mathf.RoundToInt(this.myLevel / 4f * (float)UnityEngine.Random.Range(SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value + 1)), SettingsManager.LegacyGameSettings.TitanHealthMin.Value, SettingsManager.LegacyGameSettings.TitanHealthMax.Value));
			}
		}
		this.lagMax = 150f + this.myLevel * 3f;
		this.healthTime = Time.time;
		if (this.currentHealth > 0 && base.photonView.isMine)
		{
			base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, this.currentHealth, this.maxHealth);
		}
		this.hasExplode = false;
		this.colliderEnabled = true;
		this.isHooked = false;
		this.isLook = false;
		this.isThunderSpear = false;
		this.hasSpawn = true;
		this._hasRunStart = true;
	}

	public void suicide()
	{
		this.netDie();
		if (this.nonAI)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(t1: false, string.Empty, t2: true, (string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
		}
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = true;
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().justSuicide = true;
	}

	public void testVisual(bool setCollider)
	{
		if (setCollider)
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.color = Color.white;
			}
		}
		else
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material.color = Color.black;
			}
		}
	}

	[RPC]
	public void titanGetHit(int viewID, int speed)
	{
		PhotonView photonView = PhotonView.Find(viewID);
		if (!(photonView != null) || !((photonView.gameObject.transform.position - this.neck.position).magnitude < this.lagMax) || this.hasDie || !(Time.time - this.healthTime > 0.2f))
		{
			return;
		}
		this.healthTime = Time.time;
		if (!SettingsManager.LegacyGameSettings.TitanArmorEnabled.Value || speed >= SettingsManager.LegacyGameSettings.TitanArmor.Value)
		{
			this.currentHealth -= speed;
		}
		else if (this.abnormalType == AbnormalType.TYPE_CRAWLER && !SettingsManager.LegacyGameSettings.TitanArmorCrawlerEnabled.Value)
		{
			this.currentHealth -= speed;
		}
		if ((float)this.maxHealth > 0f)
		{
			base.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, this.currentHealth, this.maxHealth);
		}
		if ((float)this.currentHealth < 0f)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.OnTitanDie(photonView);
			}
			base.photonView.RPC("netDie", PhotonTargets.OthersBuffered);
			if (this.grabbedTarget != null)
			{
				this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
			}
			this.netDie();
			if (this.nonAI)
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, speed, (string)PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
			}
			else
			{
				FengGameManagerMKII.instance.titanGetKill(photonView.owner, speed, base.name);
			}
		}
		else
		{
			FengGameManagerMKII.instance.photonView.RPC("netShowDamage", photonView.owner, speed);
		}
	}

	public void toCheckPoint(Vector3 targetPt, float r)
	{
		this.state = TitanState.to_check_point;
		this.targetCheckPt = targetPt;
		this.targetR = r;
		this.crossFade(this.runAnimation, 0.5f);
	}

	public void toPVPCheckPoint(Vector3 targetPt, float r)
	{
		this.state = TitanState.to_pvp_pt;
		this.targetCheckPt = targetPt;
		this.targetR = r;
		this.crossFade(this.runAnimation, 0.5f);
	}

	private void turn(float d)
	{
		if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
		{
			if (d > 0f)
			{
				this.turnAnimation = "crawler_turnaround_R";
			}
			else
			{
				this.turnAnimation = "crawler_turnaround_L";
			}
		}
		else if (d > 0f)
		{
			this.turnAnimation = "turnaround2";
		}
		else
		{
			this.turnAnimation = "turnaround1";
		}
		this.playAnimation(this.turnAnimation);
		base.animation[this.turnAnimation].time = 0f;
		d = Mathf.Clamp(d, -120f, 120f);
		this.turnDeg = d;
		this.desDeg = base.gameObject.transform.rotation.eulerAngles.y + this.turnDeg;
		this.state = TitanState.turn;
	}

	public void UpdateHeroDistance()
	{
		if ((!GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && this.myDifficulty >= 0 && !this.nonAI)
		{
			if (this.myHero == null)
			{
				this.myDistance = float.MaxValue;
				return;
			}
			Vector2 a = new Vector2(this.myHero.transform.position.x, this.myHero.transform.position.z);
			Vector2 b = new Vector2(this.baseTransform.position.x, this.baseTransform.position.z);
			this.myDistance = Vector2.Distance(a, b);
		}
	}

	public void update2()
	{
		this.UpdateHeroDistance();
		if ((GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || this.myDifficulty < 0 || (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine))
		{
			return;
		}
		this.explode();
		if (!this.nonAI)
		{
			if (this.activeRad < int.MaxValue && (this.state == TitanState.idle || this.state == TitanState.wander || this.state == TitanState.chase))
			{
				if (this.checkPoints.Count > 1)
				{
					if (Vector3.Distance((Vector3)this.checkPoints[0], this.baseTransform.position) > (float)this.activeRad)
					{
						this.toCheckPoint((Vector3)this.checkPoints[0], 10f);
					}
				}
				else if (Vector3.Distance(this.spawnPt, this.baseTransform.position) > (float)this.activeRad)
				{
					this.toCheckPoint(this.spawnPt, 10f);
				}
			}
			if (this.whoHasTauntMe != null)
			{
				this.tauntTime -= Time.deltaTime;
				if (this.tauntTime <= 0f)
				{
					this.whoHasTauntMe = null;
				}
				this.myHero = this.whoHasTauntMe;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
				{
					object[] parameters = new object[1] { this.myHero.GetPhotonView().viewID };
					base.photonView.RPC("setMyTarget", PhotonTargets.Others, parameters);
				}
			}
		}
		if (this.hasDie)
		{
			this.dieTime += Time.deltaTime;
			if (this.dieTime > 2f && !this.hasDieSteam)
			{
				this.hasDieSteam = true;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie1"));
					obj.transform.position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position;
					obj.transform.localScale = this.baseTransform.localScale;
				}
				else if (base.photonView.isMine)
				{
					PhotonNetwork.Instantiate("FX/FXtitanDie1", this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = this.baseTransform.localScale;
				}
			}
			if (this.dieTime > 5f)
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie"));
					obj2.transform.position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position;
					obj2.transform.localScale = this.baseTransform.localScale;
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else if (base.photonView.isMine)
				{
					PhotonNetwork.Instantiate("FX/FXtitanDie", this.baseTransform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = this.baseTransform.localScale;
					PhotonNetwork.Destroy(base.gameObject);
					this.myDifficulty = -1;
				}
			}
			return;
		}
		if (this.state == TitanState.hit)
		{
			if (this.hitPause > 0f)
			{
				this.hitPause -= Time.deltaTime;
				if (this.hitPause <= 0f)
				{
					this.baseAnimation[this.hitAnimation].speed = 1f;
					this.hitPause = 0f;
				}
			}
			if (this.baseAnimation[this.hitAnimation].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
		if (!this.nonAI)
		{
			if (this.myHero == null)
			{
				this.findNearestHero2();
			}
			if ((this.state == TitanState.idle || this.state == TitanState.chase || this.state == TitanState.wander) && this.whoHasTauntMe == null && UnityEngine.Random.Range(0, 100) < 10)
			{
				this.findNearestFacingHero2();
			}
		}
		else
		{
			if (this.stamina < this.maxStamina)
			{
				if (this.baseAnimation.IsPlaying("idle"))
				{
					this.stamina += Time.deltaTime * 30f;
				}
				if (this.baseAnimation.IsPlaying("crawler_idle"))
				{
					this.stamina += Time.deltaTime * 35f;
				}
				if (this.baseAnimation.IsPlaying("run_walk"))
				{
					this.stamina += Time.deltaTime * 10f;
				}
			}
			if (this.baseAnimation.IsPlaying("run_abnormal_1"))
			{
				this.stamina -= Time.deltaTime * 5f;
			}
			if (this.baseAnimation.IsPlaying("crawler_run"))
			{
				this.stamina -= Time.deltaTime * 15f;
			}
			if (this.stamina < 0f)
			{
				this.stamina = 0f;
			}
			if (!GameMenu.Paused)
			{
				GameObject.Find("stamina_titan").transform.localScale = new Vector3(this.stamina, 16f);
			}
		}
		if (this.state == TitanState.laugh)
		{
			if (this.baseAnimation["laugh"].normalizedTime >= 1f)
			{
				this.idle(2f);
			}
		}
		else if (this.state == TitanState.idle)
		{
			if (this.nonAI)
			{
				if (GameMenu.Paused)
				{
					return;
				}
				this.pt();
				if (this.abnormalType != AbnormalType.TYPE_CRAWLER)
				{
					if (this.controller.isAttackDown && this.stamina > 25f)
					{
						this.stamina -= 25f;
						this.attack2("combo_1");
					}
					else if (this.controller.isAttackIIDown && this.stamina > 50f)
					{
						this.stamina -= 50f;
						this.attack2("abnormal_jump");
					}
					else if (this.controller.isJumpDown && this.stamina > 15f)
					{
						this.stamina -= 15f;
						this.attack2("jumper_0");
					}
				}
				else if (this.controller.isAttackDown && this.stamina > 40f)
				{
					this.stamina -= 40f;
					this.attack2("crawler_jump_0");
				}
				if (this.controller.isSuicide)
				{
					this.suicide();
				}
				return;
			}
			if (this.sbtime > 0f)
			{
				this.sbtime -= Time.deltaTime;
				return;
			}
			if (!this.isAlarm)
			{
				if (this.abnormalType != AbnormalType.TYPE_PUNK && this.abnormalType != AbnormalType.TYPE_CRAWLER && UnityEngine.Random.Range(0f, 1f) < 0.005f)
				{
					this.sitdown();
					return;
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.02f)
				{
					this.wander();
					return;
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
				{
					this.turn(UnityEngine.Random.Range(30, 120));
					return;
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
				{
					this.turn(UnityEngine.Random.Range(-30, -120));
					return;
				}
			}
			this.angle = 0f;
			this.between2 = 0f;
			if (this.myDistance < this.chaseDistance || this.whoHasTauntMe != null)
			{
				Vector3 vector = this.myHero.transform.position - this.baseTransform.position;
				this.angle = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
				this.between2 = 0f - Mathf.DeltaAngle(this.angle, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f);
				if (this.myDistance >= this.attackDistance)
				{
					if (this.isAlarm || Mathf.Abs(this.between2) < 90f)
					{
						this.chase();
						return;
					}
					if (!this.isAlarm && !(this.myDistance >= this.chaseDistance * 0.1f))
					{
						this.chase();
						return;
					}
				}
			}
			if (this.longRangeAttackCheck2())
			{
				return;
			}
			if (this.myDistance < this.chaseDistance)
			{
				if (this.abnormalType == AbnormalType.TYPE_JUMPER && (this.myDistance > this.attackDistance || this.myHero.transform.position.y > this.head.position.y + 4f * this.myLevel) && Mathf.Abs(this.between2) < 120f && Vector3.Distance(this.baseTransform.position, this.myHero.transform.position) < 1.5f * this.myHero.transform.position.y)
				{
					this.attack2("jumper_0");
					return;
				}
				if (this.abnormalType == AbnormalType.TYPE_CRAWLER && this.myDistance < this.attackDistance * 3f && Mathf.Abs(this.between2) < 90f && this.myHero.transform.position.y < this.neck.position.y + 30f * this.myLevel && this.myHero.transform.position.y > this.neck.position.y + 10f * this.myLevel)
				{
					this.attack2("crawler_jump_0");
					return;
				}
			}
			if (this.abnormalType == AbnormalType.TYPE_PUNK && this.myDistance < 90f && Mathf.Abs(this.between2) > 90f)
			{
				if (UnityEngine.Random.Range(0f, 1f) < 0.4f)
				{
					this.randomRun(this.baseTransform.position + new Vector3(UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f), UnityEngine.Random.Range(-50f, 50f)), 10f);
				}
				if (UnityEngine.Random.Range(0f, 1f) < 0.2f)
				{
					this.recover();
				}
				else if (UnityEngine.Random.Range(0, 2) == 0)
				{
					this.attack2("quick_turn_l");
				}
				else
				{
					this.attack2("quick_turn_r");
				}
				return;
			}
			if (this.myDistance < this.attackDistance)
			{
				if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					if (this.myHero.transform.position.y + 3f <= this.neck.position.y + 20f * this.myLevel && UnityEngine.Random.Range(0f, 1f) < 0.1f)
					{
						this.chase();
					}
					return;
				}
				string text = string.Empty;
				string[] attackStrategy = this.GetAttackStrategy();
				if (attackStrategy != null)
				{
					text = attackStrategy[UnityEngine.Random.Range(0, attackStrategy.Length)];
				}
				if ((this.abnormalType == AbnormalType.TYPE_JUMPER || this.abnormalType == AbnormalType.TYPE_I) && Mathf.Abs(this.between2) > 40f)
				{
					if (text.Contains("grab") || text.Contains("kick") || text.Contains("slap") || text.Contains("bite"))
					{
						if (UnityEngine.Random.Range(0, 100) < 30)
						{
							this.turn(this.between2);
							return;
						}
					}
					else if (UnityEngine.Random.Range(0, 100) < 90)
					{
						this.turn(this.between2);
						return;
					}
				}
				if (this.executeAttack2(text))
				{
					return;
				}
				if (this.abnormalType == AbnormalType.NORMAL)
				{
					if (UnityEngine.Random.Range(0, 100) < 30 && Mathf.Abs(this.between2) > 45f)
					{
						this.turn(this.between2);
						return;
					}
				}
				else if (Mathf.Abs(this.between2) > 45f)
				{
					this.turn(this.between2);
					return;
				}
			}
			if (!(this.PVPfromCheckPt != null))
			{
				return;
			}
			if (this.PVPfromCheckPt.state == CheckPointState.Titan)
			{
				if (UnityEngine.Random.Range(0, 100) > 48)
				{
					GameObject chkPtNext = this.PVPfromCheckPt.chkPtNext;
					if (chkPtNext != null && (chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan || UnityEngine.Random.Range(0, 100) < 20))
					{
						this.toPVPCheckPoint(chkPtNext.transform.position, 5 + UnityEngine.Random.Range(0, 10));
						this.PVPfromCheckPt = chkPtNext.GetComponent<PVPcheckPoint>();
					}
				}
				else
				{
					GameObject chkPtNext = this.PVPfromCheckPt.chkPtPrevious;
					if (chkPtNext != null && (chkPtNext.GetComponent<PVPcheckPoint>().state != CheckPointState.Titan || UnityEngine.Random.Range(0, 100) < 5))
					{
						this.toPVPCheckPoint(chkPtNext.transform.position, 5 + UnityEngine.Random.Range(0, 10));
						this.PVPfromCheckPt = chkPtNext.GetComponent<PVPcheckPoint>();
					}
				}
			}
			else
			{
				this.toPVPCheckPoint(this.PVPfromCheckPt.transform.position, 5 + UnityEngine.Random.Range(0, 10));
			}
		}
		else if (this.state == TitanState.attack)
		{
			if (this.attackAnimation == "combo")
			{
				if (this.nonAI)
				{
					if (this.controller.isAttackDown)
					{
						this.nonAIcombo = true;
					}
					if (!this.nonAIcombo && !(this.baseAnimation["attack_" + this.attackAnimation].normalizedTime < 0.385f))
					{
						this.idle();
						return;
					}
				}
				if (this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.11f && this.baseAnimation["attack_" + this.attackAnimation].normalizedTime <= 0.16f)
				{
					GameObject gameObject = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001"));
					if (gameObject != null)
					{
						Vector3 position = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							gameObject.GetComponent<HERO>().die((gameObject.transform.position - position) * 15f * this.myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject.GetComponent<HERO>().HasDied())
						{
							gameObject.GetComponent<HERO>().markDie();
							object[] parameters2 = new object[5]
							{
								(gameObject.transform.position - position) * 15f * this.myLevel,
								false,
								(!this.nonAI) ? (-1) : base.photonView.viewID,
								base.name,
								true
							};
							gameObject.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters2);
						}
					}
				}
				if (this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.27f && this.baseAnimation["attack_" + this.attackAnimation].normalizedTime <= 0.32f)
				{
					GameObject gameObject2 = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001"));
					if (gameObject2 != null)
					{
						Vector3 position2 = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							gameObject2.GetComponent<HERO>().die((gameObject2.transform.position - position2) * 15f * this.myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject2.GetComponent<HERO>().HasDied())
						{
							gameObject2.GetComponent<HERO>().markDie();
							object[] parameters3 = new object[5]
							{
								(gameObject2.transform.position - position2) * 15f * this.myLevel,
								false,
								(!this.nonAI) ? (-1) : base.photonView.viewID,
								base.name,
								true
							};
							gameObject2.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters3);
						}
					}
				}
			}
			if (this.attackCheckTimeA != 0f && this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA && this.baseAnimation["attack_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB)
			{
				if (this.leftHandAttack)
				{
					GameObject gameObject3 = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001"));
					if (gameObject3 != null)
					{
						Vector3 position3 = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							gameObject3.GetComponent<HERO>().die((gameObject3.transform.position - position3) * 15f * this.myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject3.GetComponent<HERO>().HasDied())
						{
							gameObject3.GetComponent<HERO>().markDie();
							object[] parameters4 = new object[5]
							{
								(gameObject3.transform.position - position3) * 15f * this.myLevel,
								false,
								(!this.nonAI) ? (-1) : base.photonView.viewID,
								base.name,
								true
							};
							gameObject3.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters4);
						}
					}
				}
				else
				{
					GameObject gameObject4 = this.checkIfHitHand(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001"));
					if (gameObject4 != null)
					{
						Vector3 position4 = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							gameObject4.GetComponent<HERO>().die((gameObject4.transform.position - position4) * 15f * this.myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject4.GetComponent<HERO>().HasDied())
						{
							gameObject4.GetComponent<HERO>().markDie();
							object[] parameters5 = new object[5]
							{
								(gameObject4.transform.position - position4) * 15f * this.myLevel,
								false,
								(!this.nonAI) ? (-1) : base.photonView.viewID,
								base.name,
								true
							};
							gameObject4.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters5);
						}
					}
				}
			}
			if (!this.attacked && this.attackCheckTime != 0f && this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTime)
			{
				this.attacked = true;
				this.fxPosition = this.baseTransform.Find("ap_" + this.attackAnimation).position;
				GameObject gameObject5 = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/" + this.fxName), this.fxPosition, this.fxRotation)) : PhotonNetwork.Instantiate("FX/" + this.fxName, this.fxPosition, this.fxRotation, 0));
				if (this.nonAI)
				{
					gameObject5.transform.localScale = this.baseTransform.localScale * 1.5f;
					if (gameObject5.GetComponent<EnemyfxIDcontainer>() != null)
					{
						gameObject5.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
					}
				}
				else
				{
					gameObject5.transform.localScale = this.baseTransform.localScale;
				}
				if (gameObject5.GetComponent<EnemyfxIDcontainer>() != null)
				{
					gameObject5.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
				}
				float b = 1f - Vector3.Distance(this.currentCamera.transform.position, gameObject5.transform.position) * 0.05f;
				b = Mathf.Min(1f, b);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b, b);
			}
			if (this.attackAnimation == "throw")
			{
				if (!this.attacked && this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.11f)
				{
					this.attacked = true;
					Transform transform = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
					{
						this.throwRock = PhotonNetwork.Instantiate("FX/rockThrow", transform.position, transform.rotation, 0);
					}
					else
					{
						this.throwRock = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/rockThrow"), transform.position, transform.rotation);
					}
					this.throwRock.transform.localScale = this.baseTransform.localScale;
					this.throwRock.transform.position -= this.throwRock.transform.forward * 2.5f * this.myLevel;
					if (this.throwRock.GetComponent<EnemyfxIDcontainer>() != null)
					{
						if (this.nonAI)
						{
							this.throwRock.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
						}
						this.throwRock.GetComponent<EnemyfxIDcontainer>().titanName = base.name;
					}
					this.throwRock.transform.parent = transform;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
					{
						object[] parameters6 = new object[4]
						{
							base.photonView.viewID,
							this.baseTransform.localScale,
							this.throwRock.transform.localPosition,
							this.myLevel
						};
						this.throwRock.GetPhotonView().RPC("initRPC", PhotonTargets.Others, parameters6);
					}
				}
				if (this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.11f)
				{
					float y = Mathf.Atan2(this.myHero.transform.position.x - this.baseTransform.position.x, this.myHero.transform.position.z - this.baseTransform.position.z) * 57.29578f;
					this.baseGameObjectTransform.rotation = Quaternion.Euler(0f, y, 0f);
				}
				if (this.throwRock != null && this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.62f)
				{
					float num = 1f;
					float num2 = -20f;
					Vector3 vector2;
					if (this.myHero != null)
					{
						vector2 = (this.myHero.transform.position - this.throwRock.transform.position) / num + this.myHero.rigidbody.velocity;
						float num3 = this.myHero.transform.position.y + 2f * this.myLevel - this.throwRock.transform.position.y;
						vector2 = new Vector3(vector2.x, num3 / num - 0.5f * num2 * num, vector2.z);
					}
					else
					{
						vector2 = this.baseTransform.forward * 60f + Vector3.up * 10f;
					}
					this.throwRock.GetComponent<RockThrow>().launch(vector2);
					this.throwRock.transform.parent = null;
					this.throwRock = null;
				}
			}
			if (this.attackAnimation == "jumper_0" || this.attackAnimation == "crawler_jump_0")
			{
				if (!this.attacked)
				{
					if (this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 0.68f)
					{
						this.attacked = true;
						if (this.myHero == null || this.nonAI)
						{
							float num4 = 120f;
							Vector3 velocity = this.baseTransform.forward * this.speed + Vector3.up * num4;
							if (this.nonAI && this.abnormalType == AbnormalType.TYPE_CRAWLER)
							{
								num4 = 100f;
								float a = this.speed * 2.5f;
								a = Mathf.Min(a, 100f);
								velocity = this.baseTransform.forward * a + Vector3.up * num4;
							}
							this.baseRigidBody.velocity = velocity;
						}
						else
						{
							float y2 = this.myHero.rigidbody.velocity.y;
							float num5 = -20f;
							float num6 = this.gravity;
							float y3 = this.neck.position.y;
							float num7 = (num5 - num6) * 0.5f;
							float num8 = y2;
							float num9 = this.myHero.transform.position.y - y3;
							float num10 = Mathf.Abs((Mathf.Sqrt(num8 * num8 - 4f * num7 * num9) - num8) / (2f * num7));
							Vector3 vector3 = this.myHero.transform.position + this.myHero.rigidbody.velocity * num10 + Vector3.up * 0.5f * num5 * num10 * num10;
							float y4 = vector3.y;
							float num11;
							if (num9 < 0f || y4 - y3 < 0f)
							{
								num11 = 60f;
								float a2 = this.speed * 2.5f;
								a2 = Mathf.Min(a2, 100f);
								Vector3 velocity2 = this.baseTransform.forward * a2 + Vector3.up * num11;
								this.baseRigidBody.velocity = velocity2;
								return;
							}
							float num12 = y4 - y3;
							float num13 = Mathf.Sqrt(2f * num12 / this.gravity);
							num11 = this.gravity * num13;
							num11 = Mathf.Max(30f, num11);
							Vector3 vector4 = (vector3 - this.baseTransform.position) / num10;
							this.abnorma_jump_bite_horizon_v = new Vector3(vector4.x, 0f, vector4.z);
							Vector3 velocity3 = this.baseRigidBody.velocity;
							Vector3 force = new Vector3(this.abnorma_jump_bite_horizon_v.x, velocity3.y, this.abnorma_jump_bite_horizon_v.z) - velocity3;
							this.baseRigidBody.AddForce(force, ForceMode.VelocityChange);
							this.baseRigidBody.AddForce(Vector3.up * num11, ForceMode.VelocityChange);
							float num14 = Vector2.Angle(new Vector2(this.baseTransform.position.x, this.baseTransform.position.z), new Vector2(this.myHero.transform.position.x, this.myHero.transform.position.z));
							num14 = Mathf.Atan2(this.myHero.transform.position.x - this.baseTransform.position.x, this.myHero.transform.position.z - this.baseTransform.position.z) * 57.29578f;
							this.baseGameObjectTransform.rotation = Quaternion.Euler(0f, num14, 0f);
						}
					}
					else
					{
						this.baseRigidBody.velocity = Vector3.zero;
					}
				}
				if (!(this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 1f))
				{
					return;
				}
				Debug.DrawLine(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position + Vector3.up * 1.5f * this.myLevel, this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position + Vector3.up * 1.5f * this.myLevel + Vector3.up * 3f * this.myLevel, Color.green);
				Debug.DrawLine(this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position + Vector3.up * 1.5f * this.myLevel, this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").position + Vector3.up * 1.5f * this.myLevel + Vector3.forward * 3f * this.myLevel, Color.green);
				GameObject gameObject6 = this.checkIfHitHead(this.head, 3f);
				if (gameObject6 != null)
				{
					Vector3 position5 = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						gameObject6.GetComponent<HERO>().die((gameObject6.transform.position - position5) * 15f * this.myLevel, isBite: false);
					}
					else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine && !gameObject6.GetComponent<HERO>().HasDied())
					{
						gameObject6.GetComponent<HERO>().markDie();
						object[] parameters7 = new object[5]
						{
							(gameObject6.transform.position - position5) * 15f * this.myLevel,
							true,
							(!this.nonAI) ? (-1) : base.photonView.viewID,
							base.name,
							true
						};
						gameObject6.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters7);
					}
					if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						this.attackAnimation = "crawler_jump_1";
					}
					else
					{
						this.attackAnimation = "jumper_1";
					}
					this.playAnimation("attack_" + this.attackAnimation);
				}
				if (Mathf.Abs(this.baseRigidBody.velocity.y) < 0.5f || this.baseRigidBody.velocity.y < 0f || this.IsGrounded())
				{
					if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						this.attackAnimation = "crawler_jump_1";
					}
					else
					{
						this.attackAnimation = "jumper_1";
					}
					this.playAnimation("attack_" + this.attackAnimation);
				}
			}
			else if (this.attackAnimation == "jumper_1" || this.attackAnimation == "crawler_jump_1")
			{
				if (this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 1f && this.grounded)
				{
					if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
					{
						this.attackAnimation = "crawler_jump_2";
					}
					else
					{
						this.attackAnimation = "jumper_2";
					}
					this.crossFade("attack_" + this.attackAnimation, 0.1f);
					this.fxPosition = this.baseTransform.position;
					GameObject gameObject7 = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/boom2"), this.fxPosition, this.fxRotation)) : PhotonNetwork.Instantiate("FX/boom2", this.fxPosition, this.fxRotation, 0));
					gameObject7.transform.localScale = this.baseTransform.localScale * 1.6f;
					float b2 = 1f - Vector3.Distance(this.currentCamera.transform.position, gameObject7.transform.position) * 0.05f;
					b2 = Mathf.Min(1f, b2);
					this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b2, b2);
				}
			}
			else if (this.attackAnimation == "jumper_2" || this.attackAnimation == "crawler_jump_2")
			{
				if (this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 1f)
				{
					this.idle();
				}
			}
			else if (this.baseAnimation.IsPlaying("tired"))
			{
				if (this.baseAnimation["tired"].normalizedTime >= 1f + Mathf.Max(this.attackEndWait * 2f, 3f))
				{
					this.idle(UnityEngine.Random.Range(this.attackWait - 1f, 3f));
				}
			}
			else
			{
				if (!(this.baseAnimation["attack_" + this.attackAnimation].normalizedTime >= 1f + this.attackEndWait))
				{
					return;
				}
				if (this.nextAttackAnimation != null)
				{
					this.attack2(this.nextAttackAnimation);
				}
				else if (this.attackAnimation == "quick_turn_l" || this.attackAnimation == "quick_turn_r")
				{
					this.baseTransform.rotation = Quaternion.Euler(this.baseTransform.rotation.eulerAngles.x, this.baseTransform.rotation.eulerAngles.y + 180f, this.baseTransform.rotation.eulerAngles.z);
					this.idle(UnityEngine.Random.Range(0.5f, 1f));
					this.playAnimation("idle");
				}
				else if (this.abnormalType == AbnormalType.TYPE_I || this.abnormalType == AbnormalType.TYPE_JUMPER)
				{
					this.attackCount++;
					if (this.attackCount > 3 && this.attackAnimation == "abnormal_getup")
					{
						this.attackCount = 0;
						this.crossFade("tired", 0.5f);
					}
					else
					{
						this.idle(UnityEngine.Random.Range(this.attackWait - 1f, 3f));
					}
				}
				else
				{
					this.idle(UnityEngine.Random.Range(this.attackWait - 1f, 3f));
				}
			}
		}
		else if (this.state == TitanState.grab)
		{
			if (this.baseAnimation["grab_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA && this.baseAnimation["grab_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB && this.grabbedTarget == null)
			{
				GameObject gameObject8 = this.checkIfHitHand(this.currentGrabHand);
				if (gameObject8 != null)
				{
					if (this.isGrabHandLeft)
					{
						this.eatSetL(gameObject8);
						this.grabbedTarget = gameObject8;
					}
					else
					{
						this.eatSet(gameObject8);
						this.grabbedTarget = gameObject8;
					}
				}
			}
			if (this.baseAnimation["grab_" + this.attackAnimation].normalizedTime >= 1f)
			{
				if (this.grabbedTarget != null)
				{
					this.eat();
				}
				else
				{
					this.idle(UnityEngine.Random.Range(this.attackWait - 1f, 2f));
				}
			}
		}
		else if (this.state == TitanState.eat)
		{
			if (!this.attacked && !(this.baseAnimation[this.attackAnimation].normalizedTime < 0.48f))
			{
				this.attacked = true;
				this.justEatHero(this.grabbedTarget, this.currentGrabHand);
			}
			_ = this.grabbedTarget == null;
			if (this.baseAnimation[this.attackAnimation].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.chase)
		{
			if (this.myHero == null)
			{
				this.idle();
			}
			else
			{
				if (this.longRangeAttackCheck2())
				{
					return;
				}
				if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE && this.PVPfromCheckPt != null && this.myDistance > this.chaseDistance)
				{
					this.idle();
				}
				else if (this.abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					Vector3 vector5 = this.myHero.transform.position - this.baseTransform.position;
					float f = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector5.z, vector5.x)) * 57.29578f, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f);
					if (this.myDistance < this.attackDistance * 3f && UnityEngine.Random.Range(0f, 1f) < 0.1f && Mathf.Abs(f) < 90f && this.myHero.transform.position.y < this.neck.position.y + 30f * this.myLevel && this.myHero.transform.position.y > this.neck.position.y + 10f * this.myLevel)
					{
						this.attack2("crawler_jump_0");
						return;
					}
					GameObject gameObject9 = this.checkIfHitCrawlerMouth(this.head, 2.2f);
					if (gameObject9 != null)
					{
						Vector3 position6 = this.baseTransform.Find("Amarture/Core/Controller_Body/hip/spine/chest").position;
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
						{
							gameObject9.GetComponent<HERO>().die((gameObject9.transform.position - position6) * 15f * this.myLevel, isBite: false);
						}
						else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
						{
							if (gameObject9.GetComponent<TITAN_EREN>() != null)
							{
								gameObject9.GetComponent<TITAN_EREN>().hitByTitan();
							}
							else if (!gameObject9.GetComponent<HERO>().HasDied())
							{
								gameObject9.GetComponent<HERO>().markDie();
								object[] parameters8 = new object[5]
								{
									(gameObject9.transform.position - position6) * 15f * this.myLevel,
									true,
									(!this.nonAI) ? (-1) : base.photonView.viewID,
									base.name,
									true
								};
								gameObject9.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters8);
							}
						}
					}
					if (this.myDistance < this.attackDistance && UnityEngine.Random.Range(0f, 1f) < 0.02f)
					{
						this.idle(UnityEngine.Random.Range(0.05f, 0.2f));
					}
				}
				else if (this.abnormalType == AbnormalType.TYPE_JUMPER && ((this.myDistance > this.attackDistance && this.myHero.transform.position.y > this.head.position.y + 4f * this.myLevel) || this.myHero.transform.position.y > this.head.position.y + 4f * this.myLevel) && Vector3.Distance(this.baseTransform.position, this.myHero.transform.position) < 1.5f * this.myHero.transform.position.y)
				{
					this.attack2("jumper_0");
				}
				else if (this.myDistance < this.attackDistance)
				{
					this.idle(UnityEngine.Random.Range(0.05f, 0.2f));
				}
			}
		}
		else if (this.state == TitanState.wander)
		{
			float num15 = 0f;
			if (this.myDistance < this.chaseDistance || this.whoHasTauntMe != null)
			{
				Vector3 vector6 = this.myHero.transform.position - this.baseTransform.position;
				num15 = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector6.z, vector6.x)) * 57.29578f, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f);
				if (this.isAlarm || Mathf.Abs(num15) < 90f)
				{
					this.chase();
					return;
				}
				if (!this.isAlarm && !(this.myDistance >= this.chaseDistance * 0.1f))
				{
					this.chase();
					return;
				}
			}
			if (UnityEngine.Random.Range(0f, 1f) < 0.01f)
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.turn)
		{
			this.baseGameObjectTransform.rotation = Quaternion.Lerp(this.baseGameObjectTransform.rotation, Quaternion.Euler(0f, this.desDeg, 0f), Time.deltaTime * Mathf.Abs(this.turnDeg) * 0.015f);
			if (this.baseAnimation[this.turnAnimation].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.hit_eye)
		{
			if (this.baseAnimation.IsPlaying("sit_hit_eye") && this.baseAnimation["sit_hit_eye"].normalizedTime >= 1f)
			{
				this.remainSitdown();
			}
			else if (this.baseAnimation.IsPlaying("hit_eye") && this.baseAnimation["hit_eye"].normalizedTime >= 1f)
			{
				if (this.nonAI)
				{
					this.idle();
				}
				else
				{
					this.attack2("combo_1");
				}
			}
		}
		else if (this.state == TitanState.to_check_point)
		{
			if (this.checkPoints.Count <= 0 && this.myDistance < this.attackDistance)
			{
				string decidedAction = string.Empty;
				string[] attackStrategy2 = this.GetAttackStrategy();
				if (attackStrategy2 != null)
				{
					decidedAction = attackStrategy2[UnityEngine.Random.Range(0, attackStrategy2.Length)];
				}
				if (this.executeAttack2(decidedAction))
				{
					return;
				}
			}
			if (!(Vector3.Distance(this.baseTransform.position, this.targetCheckPt) < this.targetR))
			{
				return;
			}
			if (this.checkPoints.Count > 0)
			{
				if (this.checkPoints.Count == 1)
				{
					if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT)
					{
						this.MultiplayerManager.gameLose2();
						this.checkPoints = new ArrayList();
						this.idle();
					}
					return;
				}
				if (this.checkPoints.Count == 4)
				{
					this.MultiplayerManager.sendChatContentInfo("<color=#A8FF24>*WARNING!* An abnormal titan is approaching the north gate!</color>");
				}
				Vector3 vector7 = (Vector3)this.checkPoints[0];
				this.targetCheckPt = vector7;
				this.checkPoints.RemoveAt(0);
			}
			else
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.to_pvp_pt)
		{
			if (this.myDistance < this.chaseDistance * 0.7f)
			{
				this.chase();
			}
			if (Vector3.Distance(this.baseTransform.position, this.targetCheckPt) < this.targetR)
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.random_run)
		{
			this.random_run_time -= Time.deltaTime;
			if (Vector3.Distance(this.baseTransform.position, this.targetCheckPt) < this.targetR || this.random_run_time <= 0f)
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.down)
		{
			this.getdownTime -= Time.deltaTime;
			if (this.baseAnimation.IsPlaying("sit_hunt_down") && this.baseAnimation["sit_hunt_down"].normalizedTime >= 1f)
			{
				this.playAnimation("sit_idle");
			}
			if (this.getdownTime <= 0f)
			{
				this.crossFadeIfNotPlaying("sit_getup", 0.1f);
			}
			if (this.baseAnimation.IsPlaying("sit_getup") && this.baseAnimation["sit_getup"].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.sit)
		{
			this.getdownTime -= Time.deltaTime;
			this.angle = 0f;
			this.between2 = 0f;
			if (this.myDistance < this.chaseDistance || this.whoHasTauntMe != null)
			{
				if (this.myDistance < 50f)
				{
					this.isAlarm = true;
				}
				else
				{
					Vector3 vector8 = this.myHero.transform.position - this.baseTransform.position;
					this.angle = (0f - Mathf.Atan2(vector8.z, vector8.x)) * 57.29578f;
					this.between2 = 0f - Mathf.DeltaAngle(this.angle, this.baseGameObjectTransform.rotation.eulerAngles.y - 90f);
					if (Mathf.Abs(this.between2) < 100f)
					{
						this.isAlarm = true;
					}
				}
			}
			if (this.baseAnimation.IsPlaying("sit_down") && this.baseAnimation["sit_down"].normalizedTime >= 1f)
			{
				this.playAnimation("sit_idle");
			}
			if ((this.getdownTime <= 0f || this.isAlarm) && this.baseAnimation.IsPlaying("sit_idle"))
			{
				this.crossFadeIfNotPlaying("sit_getup", 0.1f);
			}
			if (this.baseAnimation.IsPlaying("sit_getup") && this.baseAnimation["sit_getup"].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
		else if (this.state == TitanState.recover)
		{
			this.getdownTime -= Time.deltaTime;
			if (this.getdownTime <= 0f)
			{
				this.idle();
			}
			if (this.baseAnimation.IsPlaying("idle_recovery") && this.baseAnimation["idle_recovery"].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
	}

	public void updateCollider()
	{
		if (this.colliderEnabled)
		{
			if (this.isHooked || this.myTitanTrigger.isCollide || this.isLook || this.isThunderSpear)
			{
				return;
			}
			foreach (Collider baseCollider in this.baseColliders)
			{
				if (baseCollider != null)
				{
					baseCollider.enabled = false;
				}
			}
			this.colliderEnabled = false;
		}
		else
		{
			if (!this.isHooked && !this.myTitanTrigger.isCollide && !this.isLook && !this.isThunderSpear)
			{
				return;
			}
			foreach (Collider baseCollider2 in this.baseColliders)
			{
				if (baseCollider2 != null)
				{
					baseCollider2.enabled = true;
				}
			}
			this.colliderEnabled = true;
		}
	}

	public void updateLabel()
	{
		if (this.healthLabel != null && this.healthLabel.GetComponent<UILabel>().isVisible)
		{
			this.healthLabel.transform.LookAt(2f * this.healthLabel.transform.position - Camera.main.transform.position);
		}
	}

	private void wander(float sbtime = 0f)
	{
		this.state = TitanState.wander;
		this.crossFade(this.runAnimation, 0.5f);
	}
}
