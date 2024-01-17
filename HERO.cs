using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using CustomSkins;
using ExitGames.Client.Photon;
using GameProgress;
using Photon;
using Settings;
using UI;
using UnityEngine;
using Weather;
using Xft;

internal class HERO : Photon.MonoBehaviour
{
	private HERO_STATE _state;

	private bool almostSingleHook;

	private string attackAnimation;

	private int attackLoop;

	private bool attackMove;

	private bool attackReleased;

	public AudioSource audio_ally;

	public AudioSource audio_hitwall;

	private GameObject badGuy;

	public Animation baseAnimation;

	public Rigidbody baseRigidBody;

	public Transform baseTransform;

	private bool bigLean;

	public float bombCD;

	public bool bombImmune;

	public float bombRadius;

	public float bombSpeed;

	public float bombTime;

	public float bombTimeMax;

	private float buffTime;

	public GameObject bulletLeft;

	private int bulletMAX = 7;

	public GameObject bulletRight;

	private bool buttonAttackRelease;

	public Dictionary<string, UISprite> cachedSprites;

	public float CameraMultiplier;

	public bool canJump = true;

	public GameObject checkBoxLeft;

	public GameObject checkBoxRight;

	public GameObject cross1;

	public GameObject cross2;

	public GameObject crossL1;

	public GameObject crossL2;

	public GameObject crossR1;

	public GameObject crossR2;

	public string currentAnimation;

	private int currentBladeNum = 5;

	private float currentBladeSta = 100f;

	private BUFF currentBuff;

	public Camera currentCamera;

	private float currentGas = 100f;

	public float currentSpeed;

	private bool dashD;

	private Vector3 dashDirection;

	private bool dashL;

	private bool dashR;

	private float dashTime;

	private bool dashU;

	private Vector3 dashV;

	public bool detonate;

	private float dTapTime = -1f;

	private bool EHold;

	private GameObject eren_titan;

	private int escapeTimes = 1;

	private float facingDirection;

	private float flare1CD;

	private float flare2CD;

	private float flare3CD;

	private float flareTotalCD = 30f;

	private Transform forearmL;

	private Transform forearmR;

	private float gravity = 20f;

	private bool grounded;

	private GameObject gunDummy;

	private Vector3 gunTarget;

	private Transform handL;

	private Transform handR;

	private bool hasDied;

	public bool hasspawn;

	private bool hookBySomeOne = true;

	public GameObject hookRefL1;

	public GameObject hookRefL2;

	public GameObject hookRefR1;

	public GameObject hookRefR2;

	private bool hookSomeOne;

	private GameObject hookTarget;

	private float invincible = 3f;

	public bool isCannon;

	private bool isLaunchLeft;

	private bool isLaunchRight;

	private bool isLeftHandHooked;

	private bool isMounted;

	public bool isPhotonCamera;

	private bool isRightHandHooked;

	public float jumpHeight = 2f;

	private bool justGrounded;

	public GameObject LabelDistance;

	public Transform lastHook;

	private float launchElapsedTimeL;

	private float launchElapsedTimeR;

	private Vector3 launchForce;

	private Vector3 launchPointLeft;

	private Vector3 launchPointRight;

	private bool leanLeft;

	private bool leftArmAim;

	public XWeaponTrail leftbladetrail;

	public XWeaponTrail leftbladetrail2;

	private int leftBulletLeft = 7;

	private bool leftGunHasBullet = true;

	private float lTapTime = -1f;

	public GameObject maincamera;

	public float maxVelocityChange = 10f;

	public AudioSource meatDie;

	public Bomb myBomb;

	public GameObject myCannon;

	public Transform myCannonBase;

	public Transform myCannonPlayer;

	public CannonPropRegion myCannonRegion;

	public GROUP myGroup;

	private GameObject myHorse;

	public GameObject myNetWorkName;

	public float myScale = 1f;

	public int myTeam = 1;

	public List<TITAN> myTitans;

	private bool needLean;

	private Quaternion oldHeadRotation;

	private float originVM;

	private bool QHold;

	private string reloadAnimation = string.Empty;

	private bool rightArmAim;

	public XWeaponTrail rightbladetrail;

	public XWeaponTrail rightbladetrail2;

	private int rightBulletLeft = 7;

	private bool rightGunHasBullet = true;

	public AudioSource rope;

	private float rTapTime = -1f;

	public HERO_SETUP setup;

	private GameObject skillCD;

	public float skillCDDuration;

	public float skillCDLast;

	public float skillCDLastCannon;

	private string skillId;

	public string skillIDHUD;

	public AudioSource slash;

	public AudioSource slashHit;

	private ParticleSystem smoke_3dmg;

	private ParticleSystem sparks;

	public float speed = 10f;

	public GameObject speedFX;

	public GameObject speedFX1;

	private ParticleSystem speedFXPS;

	private bool spinning;

	private string standAnimation = "stand";

	private Quaternion targetHeadRotation;

	private Quaternion targetRotation;

	private bool throwedBlades;

	public bool titanForm;

	private GameObject titanWhoGrabMe;

	private int titanWhoGrabMeID;

	private int totalBladeNum = 5;

	public float totalBladeSta = 100f;

	public float totalGas = 100f;

	private Transform upperarmL;

	private Transform upperarmR;

	private float useGasSpeed = 0.2f;

	public bool useGun;

	private float uTapTime = -1f;

	private bool wallJump;

	private float wallRunTime;

	private float _reelInAxis;

	private float _reelOutAxis;

	private float _reelOutScrollTimeLeft;

	private bool _animationStopped;

	private GameObject ThunderSpearL;

	private GameObject ThunderSpearR;

	public GameObject ThunderSpearLModel;

	public GameObject ThunderSpearRModel;

	private bool _hasRunStart;

	private bool _needSetupThunderspears;

	public HumanCustomSkinLoader _customSkinLoader;

	private bool _cancelGasDisable;

	private float _currentEmoteActionTime;

	public float _flareDelayAfterEmote;

	private float _dashCooldownLeft;

	public bool isGrabbed => this.state == HERO_STATE.Grab;

	private HERO_STATE state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state == HERO_STATE.AirDodge || this._state == HERO_STATE.GroundDodge)
			{
				this.dashTime = 0f;
			}
			this._state = value;
		}
	}

	public bool IsMine()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			return base.photonView.isMine;
		}
		return true;
	}

	public void EmoteAction(string animation)
	{
		if (this.state != HERO_STATE.Grab && this.state != HERO_STATE.AirDodge)
		{
			this.state = HERO_STATE.Salute;
			this.crossFade(animation, 0.1f);
			this._currentEmoteActionTime = this.baseAnimation[animation].length;
		}
	}

	private void UpdateInput()
	{
		if (!GameMenu.Paused)
		{
			if (SettingsManager.InputSettings.Interaction.EmoteMenu.GetKeyDown())
			{
				GameMenu.ToggleEmoteWheel(!GameMenu.WheelMenu);
			}
			if (SettingsManager.InputSettings.Interaction.MenuNext.GetKeyDown())
			{
				GameMenu.NextEmoteWheel();
			}
		}
		this.UpdateReelInput();
	}

	private void UpdateReelInput()
	{
		this._reelOutScrollTimeLeft -= Time.deltaTime;
		if (this._reelOutScrollTimeLeft <= 0f)
		{
			this._reelOutAxis = 0f;
		}
		if (SettingsManager.InputSettings.Human.ReelIn.GetKey())
		{
			this._reelInAxis = -1f;
		}
		foreach (InputKey ınputKey in SettingsManager.InputSettings.Human.ReelOut.InputKeys)
		{
			if (ınputKey.GetKey())
			{
				this._reelOutAxis = 1f;
				if (ınputKey.IsWheel())
				{
					this._reelOutScrollTimeLeft = SettingsManager.InputSettings.Human.ReelOutScrollSmoothing.Value;
				}
			}
		}
	}

	private float GetReelAxis()
	{
		if (this._reelInAxis != 0f)
		{
			return this._reelInAxis;
		}
		return this._reelOutAxis;
	}

	private void SetupThunderSpears()
	{
		if (base.photonView.isMine)
		{
			base.photonView.RPC("SetupThunderSpearsRPC", PhotonTargets.AllBuffered);
		}
	}

	[RPC]
	private void SetupThunderSpearsRPC(PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			if (!this._hasRunStart)
			{
				this._needSetupThunderspears = true;
			}
			else
			{
				this.CreateAndAttachThunderSpears();
			}
		}
	}

	private void CreateAndAttachThunderSpears()
	{
		this.ThunderSpearL = (GameObject)UnityEngine.Object.Instantiate(FengGameManagerMKII.RCassets.Load("ThunderSpearProp"));
		this.ThunderSpearR = (GameObject)UnityEngine.Object.Instantiate(FengGameManagerMKII.RCassets.Load("ThunderSpearProp"));
		this.ThunderSpearLModel = this.ThunderSpearL.transform.Find("ThunderSpearModel").gameObject;
		this.ThunderSpearRModel = this.ThunderSpearR.transform.Find("ThunderSpearModel").gameObject;
		this.AttachThunderSpear(this.ThunderSpearL, this.handL.transform, left: true);
		this.AttachThunderSpear(this.ThunderSpearR, this.handR.transform, left: false);
		this.currentBladeNum = (this.totalBladeNum = 0);
		this.totalBladeSta = (this.currentBladeSta = 0f);
		this.setup.part_blade_l.SetActive(value: false);
		this.setup.part_blade_r.SetActive(value: false);
	}

	private void AttachThunderSpear(GameObject thunderSpear, Transform mount, bool left)
	{
		thunderSpear.transform.parent = mount.parent;
		Vector3 localPosition = (left ? new Vector3(-0.001649f, 0.000775f, -0.000227f) : new Vector3(-0.001649f, -0.000775f, -0.000227f));
		Quaternion localRotation = (left ? Quaternion.Euler(5f, -85f, 10f) : Quaternion.Euler(-5f, -85f, -10f));
		thunderSpear.transform.localPosition = localPosition;
		thunderSpear.transform.localRotation = localRotation;
	}

	private void SetThunderSpears(bool hasLeft, bool hasRight)
	{
		base.photonView.RPC("SetThunderSpearsRPC", PhotonTargets.All, hasLeft, hasRight);
	}

	[RPC]
	private void SetThunderSpearsRPC(bool hasLeft, bool hasRight, PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			if (this.ThunderSpearLModel != null)
			{
				this.ThunderSpearLModel.SetActive(hasLeft);
			}
			if (this.ThunderSpearRModel != null)
			{
				this.ThunderSpearRModel.SetActive(hasRight);
			}
		}
	}

	private void applyForceToBody(GameObject GO, Vector3 v)
	{
		GO.rigidbody.AddForce(v);
		GO.rigidbody.AddTorque(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
	}

	public void attackAccordingToMouse()
	{
		if ((double)Input.mousePosition.x < (double)Screen.width * 0.5)
		{
			this.attackAnimation = "attack2";
		}
		else
		{
			this.attackAnimation = "attack1";
		}
	}

	public void attackAccordingToTarget(Transform a)
	{
		Vector3 vector = a.position - base.transform.position;
		float num = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, base.transform.rotation.eulerAngles.y - 90f);
		if (Mathf.Abs(num) < 90f && vector.magnitude < 6f && a.position.y <= base.transform.position.y + 2f && a.position.y >= base.transform.position.y - 5f)
		{
			this.attackAnimation = "attack4";
		}
		else if (num > 0f)
		{
			this.attackAnimation = "attack1";
		}
		else
		{
			this.attackAnimation = "attack2";
		}
	}

	private void Awake()
	{
		this.cache();
		this.setup = base.gameObject.GetComponent<HERO_SETUP>();
		this.baseRigidBody.freezeRotation = true;
		this.baseRigidBody.useGravity = false;
		this.handL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
		this.handR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
		this.forearmL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
		this.forearmR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
		this.upperarmL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
		this.upperarmR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
		this._customSkinLoader = base.gameObject.AddComponent<HumanCustomSkinLoader>();
	}

	public void backToHuman()
	{
		base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
		base.rigidbody.velocity = Vector3.zero;
		this.titanForm = false;
		this.ungrabbed();
		this.falseAttack();
		this.skillCDDuration = this.skillCDLast;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(base.gameObject);
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			base.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
		}
	}

	[RPC]
	private void backToHumanRPC()
	{
		this.titanForm = false;
		this.eren_titan = null;
		base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
	}

	[RPC]
	public void badGuyReleaseMe()
	{
		this.hookBySomeOne = false;
		this.badGuy = null;
	}

	[RPC]
	public void blowAway(Vector3 force, PhotonMessageInfo info)
	{
		if (info != null)
		{
			if (Math.Abs(force.x) > 500f || Math.Abs(force.y) > 500f || Math.Abs(force.z) > 500f)
			{
				FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero blowaway exploit");
				return;
			}
			if (!info.sender.isMasterClient && (Convert.ToInt32(info.sender.customProperties[PhotonPlayerProperty.isTitan]) == 1 || Convert.ToBoolean(info.sender.customProperties[PhotonPlayerProperty.dead])))
			{
				return;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
		{
			base.rigidbody.AddForce(force, ForceMode.Impulse);
			base.transform.LookAt(base.transform.position);
		}
	}

	private void bodyLean()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine)
		{
			return;
		}
		float z = 0f;
		this.needLean = false;
		if (!this.useGun && this.state == HERO_STATE.Attack && this.attackAnimation != "attack3_1" && this.attackAnimation != "attack3_2" && !this.IsFiringThunderSpear())
		{
			float y = base.rigidbody.velocity.y;
			float x = base.rigidbody.velocity.x;
			float z2 = base.rigidbody.velocity.z;
			float x2 = Mathf.Sqrt(x * x + z2 * z2);
			float num = Mathf.Atan2(y, x2) * 57.29578f;
			this.targetRotation = Quaternion.Euler((0f - num) * (1f - Vector3.Angle(base.rigidbody.velocity, base.transform.forward) / 90f), this.facingDirection, 0f);
			if ((this.isLeftHandHooked && this.bulletLeft != null) || (this.isRightHandHooked && this.bulletRight != null))
			{
				base.transform.rotation = this.targetRotation;
			}
			return;
		}
		if (this.isLeftHandHooked && this.bulletLeft != null && this.isRightHandHooked && this.bulletRight != null)
		{
			if (this.almostSingleHook)
			{
				this.needLean = true;
				z = this.getLeanAngle(this.bulletRight.transform.position, left: true);
			}
		}
		else if (this.isLeftHandHooked && this.bulletLeft != null)
		{
			this.needLean = true;
			z = this.getLeanAngle(this.bulletLeft.transform.position, left: true);
		}
		else if (this.isRightHandHooked && this.bulletRight != null)
		{
			this.needLean = true;
			z = this.getLeanAngle(this.bulletRight.transform.position, left: false);
		}
		if (this.needLean)
		{
			float num2 = 0f;
			if (!this.useGun && this.state != HERO_STATE.Attack)
			{
				num2 = this.currentSpeed * 0.1f;
				num2 = Mathf.Min(num2, 20f);
			}
			this.targetRotation = Quaternion.Euler(0f - num2, this.facingDirection, z);
		}
		else if (this.state != HERO_STATE.Attack)
		{
			this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
		}
	}

	public void bombInit()
	{
		this.skillIDHUD = this.skillId;
		this.skillCDDuration = this.skillCDLast;
		if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
		{
			int num = SettingsManager.AbilitySettings.BombRadius.Value;
			int num2 = SettingsManager.AbilitySettings.BombCooldown.Value;
			int num3 = SettingsManager.AbilitySettings.BombSpeed.Value;
			int num4 = SettingsManager.AbilitySettings.BombRange.Value;
			if (num + num2 + num3 + num4 > 16)
			{
				num = (num3 = 6);
				num4 = 3;
				num2 = 1;
			}
			this.bombTimeMax = ((float)num4 * 60f + 200f) / ((float)num3 * 60f + 200f);
			this.bombRadius = (float)num * 4f + 20f;
			this.bombCD = (float)(num2 + 4) * -0.4f + 5f;
			this.bombSpeed = (float)num3 * 60f + 200f;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.RCBombR, SettingsManager.AbilitySettings.BombColor.Value.r);
			hashtable.Add(PhotonPlayerProperty.RCBombG, SettingsManager.AbilitySettings.BombColor.Value.g);
			hashtable.Add(PhotonPlayerProperty.RCBombB, SettingsManager.AbilitySettings.BombColor.Value.b);
			hashtable.Add(PhotonPlayerProperty.RCBombA, SettingsManager.AbilitySettings.BombColor.Value.a);
			hashtable.Add(PhotonPlayerProperty.RCBombRadius, this.bombRadius);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			this.skillId = "bomb";
			this.skillIDHUD = "armin";
			this.skillCDLast = this.bombCD;
			this.skillCDDuration = 10f;
			if (FengGameManagerMKII.instance.roundTime > 10f)
			{
				this.skillCDDuration = 5f;
			}
		}
	}

	private void breakApart2(Vector3 v, bool isBite)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
		gameObject.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
		gameObject.GetComponent<HERO_SETUP>().isDeadBody = true;
		gameObject.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
		if (!isBite)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
			GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
			GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
			gameObject2.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
			gameObject3.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
			gameObject4.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
			gameObject2.GetComponent<HERO_SETUP>().isDeadBody = true;
			gameObject3.GetComponent<HERO_SETUP>().isDeadBody = true;
			gameObject4.GetComponent<HERO_SETUP>().isDeadBody = true;
			gameObject2.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.UPPER);
			gameObject3.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.LOWER);
			gameObject4.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.animation[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
			this.applyForceToBody(gameObject2, v);
			this.applyForceToBody(gameObject3, v);
			this.applyForceToBody(gameObject4, v);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
			{
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject2, resetRotation: false);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
		{
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject, resetRotation: false);
		}
		this.applyForceToBody(gameObject, v);
		Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
		Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
		GameObject gameObject5;
		GameObject gameObject6;
		GameObject gameObject7;
		GameObject gameObject8;
		GameObject gameObject9;
		if (this.useGun)
		{
			gameObject5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
			gameObject6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
			gameObject7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_2"), base.transform.position, base.transform.rotation);
			gameObject8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), base.transform.position, base.transform.rotation);
			gameObject9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), base.transform.position, base.transform.rotation);
		}
		else
		{
			gameObject5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
			gameObject6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
			gameObject7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg"), base.transform.position, base.transform.rotation);
			gameObject8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), base.transform.position, base.transform.rotation);
			gameObject9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), base.transform.position, base.transform.rotation);
		}
		gameObject5.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
		gameObject6.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
		gameObject7.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
		gameObject8.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
		gameObject9.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
		this.applyForceToBody(gameObject5, v);
		this.applyForceToBody(gameObject6, v);
		this.applyForceToBody(gameObject7, v);
		this.applyForceToBody(gameObject8, v);
		this.applyForceToBody(gameObject9, v);
	}

	private void bufferUpdate()
	{
		if (!(this.buffTime > 0f))
		{
			return;
		}
		this.buffTime -= Time.deltaTime;
		if (this.buffTime <= 0f)
		{
			this.buffTime = 0f;
			if (this.currentBuff == BUFF.SpeedUp && base.animation.IsPlaying("run_sasha"))
			{
				this.crossFade("run", 0.1f);
			}
			this.currentBuff = BUFF.NoBuff;
		}
	}

	public void cache()
	{
		this.baseTransform = base.transform;
		this.baseRigidBody = base.rigidbody;
		this.maincamera = GameObject.Find("MainCamera");
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine)
		{
			return;
		}
		this.baseAnimation = base.animation;
		this.cross1 = GameObject.Find("cross1");
		this.cross2 = GameObject.Find("cross2");
		this.crossL1 = GameObject.Find("crossL1");
		this.crossL2 = GameObject.Find("crossL2");
		this.crossR1 = GameObject.Find("crossR1");
		this.crossR2 = GameObject.Find("crossR2");
		this.LabelDistance = GameObject.Find("LabelDistance");
		this.cachedSprites = new Dictionary<string, UISprite>();
		UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = (GameObject)array[i];
			if (gameObject.GetComponent<UISprite>() != null && gameObject.activeInHierarchy)
			{
				string text = gameObject.name;
				if ((text.Contains("blade") || text.Contains("bullet") || text.Contains("gas") || text.Contains("flare") || text.Contains("skill_cd")) && !this.cachedSprites.ContainsKey(text))
				{
					this.cachedSprites.Add(text, gameObject.GetComponent<UISprite>());
				}
			}
		}
		this.SetupCrosshairs();
	}

	private void SetupCrosshairs()
	{
		this.cross1.transform.localPosition = Vector3.up * 10000f;
		this.cross2.transform.localPosition = Vector3.up * 10000f;
		this.LabelDistance.transform.localPosition = Vector3.up * 10000f;
	}

	private void calcFlareCD()
	{
		if (this.flare1CD > 0f)
		{
			this.flare1CD -= Time.deltaTime;
			if (this.flare1CD < 0f)
			{
				this.flare1CD = 0f;
			}
		}
		if (this.flare2CD > 0f)
		{
			this.flare2CD -= Time.deltaTime;
			if (this.flare2CD < 0f)
			{
				this.flare2CD = 0f;
			}
		}
		if (this.flare3CD > 0f)
		{
			this.flare3CD -= Time.deltaTime;
			if (this.flare3CD < 0f)
			{
				this.flare3CD = 0f;
			}
		}
	}

	private void calcSkillCD()
	{
		if (this.skillCDDuration > 0f)
		{
			this.skillCDDuration -= Time.deltaTime;
			if (this.skillCDDuration < 0f)
			{
				this.skillCDDuration = 0f;
			}
		}
	}

	private float CalculateJumpVerticalSpeed()
	{
		return Mathf.Sqrt(2f * this.jumpHeight * this.gravity);
	}

	private void changeBlade()
	{
		if (this.useGun && !this.grounded && LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_AHSS)
		{
			return;
		}
		this.state = HERO_STATE.ChangeBlade;
		this.throwedBlades = false;
		if (this.useGun)
		{
			if (!this.leftGunHasBullet && !this.rightGunHasBullet)
			{
				if (this.grounded)
				{
					this.reloadAnimation = "AHSS_gun_reload_both";
				}
				else
				{
					this.reloadAnimation = "AHSS_gun_reload_both_air";
				}
			}
			else if (!this.leftGunHasBullet)
			{
				if (this.grounded)
				{
					this.reloadAnimation = "AHSS_gun_reload_l";
				}
				else
				{
					this.reloadAnimation = "AHSS_gun_reload_l_air";
				}
			}
			else if (!this.rightGunHasBullet)
			{
				if (this.grounded)
				{
					this.reloadAnimation = "AHSS_gun_reload_r";
				}
				else
				{
					this.reloadAnimation = "AHSS_gun_reload_r_air";
				}
			}
			else
			{
				if (this.grounded)
				{
					this.reloadAnimation = "AHSS_gun_reload_both";
				}
				else
				{
					this.reloadAnimation = "AHSS_gun_reload_both_air";
				}
				this.leftGunHasBullet = (this.rightGunHasBullet = false);
			}
			this.crossFade(this.reloadAnimation, 0.05f);
		}
		else
		{
			if (!this.grounded)
			{
				this.reloadAnimation = "changeBlade_air";
			}
			else
			{
				this.reloadAnimation = "changeBlade";
			}
			this.crossFade(this.reloadAnimation, 0.1f);
		}
	}

	private void checkDashDoubleTap()
	{
		if (this.uTapTime >= 0f)
		{
			this.uTapTime += Time.deltaTime;
			if (this.uTapTime > 0.2f)
			{
				this.uTapTime = -1f;
			}
		}
		if (this.dTapTime >= 0f)
		{
			this.dTapTime += Time.deltaTime;
			if (this.dTapTime > 0.2f)
			{
				this.dTapTime = -1f;
			}
		}
		if (this.lTapTime >= 0f)
		{
			this.lTapTime += Time.deltaTime;
			if (this.lTapTime > 0.2f)
			{
				this.lTapTime = -1f;
			}
		}
		if (this.rTapTime >= 0f)
		{
			this.rTapTime += Time.deltaTime;
			if (this.rTapTime > 0.2f)
			{
				this.rTapTime = -1f;
			}
		}
		if (SettingsManager.InputSettings.General.Forward.GetKeyDown())
		{
			if (this.uTapTime == -1f)
			{
				this.uTapTime = 0f;
			}
			if (this.uTapTime != 0f)
			{
				this.dashU = true;
			}
		}
		if (SettingsManager.InputSettings.General.Back.GetKeyDown())
		{
			if (this.dTapTime == -1f)
			{
				this.dTapTime = 0f;
			}
			if (this.dTapTime != 0f)
			{
				this.dashD = true;
			}
		}
		if (SettingsManager.InputSettings.General.Left.GetKeyDown())
		{
			if (this.lTapTime == -1f)
			{
				this.lTapTime = 0f;
			}
			if (this.lTapTime != 0f)
			{
				this.dashL = true;
			}
		}
		if (SettingsManager.InputSettings.General.Right.GetKeyDown())
		{
			if (this.rTapTime == -1f)
			{
				this.rTapTime = 0f;
			}
			if (this.rTapTime != 0f)
			{
				this.dashR = true;
			}
		}
	}

	private void checkDashRebind()
	{
		if (SettingsManager.InputSettings.Human.Dash.GetKeyDown())
		{
			if (SettingsManager.InputSettings.General.Forward.GetKey())
			{
				this.dashU = true;
			}
			else if (SettingsManager.InputSettings.General.Back.GetKey())
			{
				this.dashD = true;
			}
			else if (SettingsManager.InputSettings.General.Left.GetKey())
			{
				this.dashL = true;
			}
			else if (SettingsManager.InputSettings.General.Right.GetKey())
			{
				this.dashR = true;
			}
		}
	}

	public void checkTitan()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask layerMask = 1 << PhysicsLayer.PlayerAttackBox;
		LayerMask layerMask2 = 1 << PhysicsLayer.Ground;
		LayerMask layerMask3 = 1 << PhysicsLayer.EnemyBox;
		RaycastHit[] array = Physics.RaycastAll(ray, 180f, ((LayerMask)((int)layerMask | (int)layerMask2 | (int)layerMask3)).value);
		List<RaycastHit> list = new List<RaycastHit>();
		List<TITAN> list2 = new List<TITAN>();
		foreach (RaycastHit item in array)
		{
			list.Add(item);
		}
		list.Sort((RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
		float num = 180f;
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i].collider.gameObject;
			if (gameObject.layer == 16)
			{
				if (!gameObject.name.Contains("PlayerDetectorRC"))
				{
					continue;
				}
				RaycastHit raycastHit;
				RaycastHit raycastHit2 = (raycastHit = list[i]);
				if (raycastHit2.distance < num)
				{
					num -= 60f;
					if (num <= 60f)
					{
						i = list.Count;
					}
					TITAN component = gameObject.transform.root.gameObject.GetComponent<TITAN>();
					if (component != null)
					{
						list2.Add(component);
					}
				}
			}
			else
			{
				i = list.Count;
			}
		}
		for (int i = 0; i < this.myTitans.Count; i++)
		{
			TITAN tITAN = this.myTitans[i];
			if (!list2.Contains(tITAN))
			{
				tITAN.isLook = false;
			}
		}
		for (int i = 0; i < list2.Count; i++)
		{
			list2[i].isLook = true;
		}
		this.myTitans = list2;
	}

	public void ClearPopup()
	{
		FengGameManagerMKII.instance.ShowHUDInfoCenter(string.Empty);
	}

	public void continueAnimation()
	{
		if (!this._animationStopped)
		{
			return;
		}
		this._animationStopped = false;
		foreach (AnimationState item in base.animation)
		{
			if (item.speed == 1f)
			{
				return;
			}
			item.speed = 1f;
		}
		this.customAnimationSpeed();
		this.playAnimation(this.currentPlayingClipName());
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
		{
			base.photonView.RPC("netContinueAnimation", PhotonTargets.Others);
		}
	}

	public void crossFade(string aniName, float time)
	{
		this.currentAnimation = aniName;
		base.animation.CrossFade(aniName, time);
		if (PhotonNetwork.connected && base.photonView.isMine)
		{
			object[] parameters = new object[2] { aniName, time };
			base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
		}
	}

	public string currentPlayingClipName()
	{
		foreach (AnimationState item in base.animation)
		{
			if (base.animation.IsPlaying(item.name))
			{
				return item.name;
			}
		}
		return string.Empty;
	}

	private void customAnimationSpeed()
	{
		base.animation["attack5"].speed = 1.85f;
		base.animation["changeBlade"].speed = 1.2f;
		base.animation["air_release"].speed = 0.6f;
		base.animation["changeBlade_air"].speed = 0.8f;
		base.animation["AHSS_gun_reload_both"].speed = 0.38f;
		base.animation["AHSS_gun_reload_both_air"].speed = 0.5f;
		base.animation["AHSS_gun_reload_l"].speed = 0.4f;
		base.animation["AHSS_gun_reload_l_air"].speed = 0.5f;
		base.animation["AHSS_gun_reload_r"].speed = 0.4f;
		base.animation["AHSS_gun_reload_r_air"].speed = 0.5f;
	}

	private void dash(float horizontal, float vertical)
	{
		if (this.dashTime <= 0f && this.currentGas > 0f && !this.isMounted && this._dashCooldownLeft <= 0f)
		{
			this.useGas(this.totalGas * 0.04f);
			this.facingDirection = this.getGlobalFacingDirection(horizontal, vertical);
			this.dashV = this.getGlobaleFacingVector3(this.facingDirection);
			this.originVM = this.currentSpeed;
			Quaternion rotation = Quaternion.Euler(0f, this.facingDirection, 0f);
			base.rigidbody.rotation = rotation;
			this.targetRotation = rotation;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				UnityEngine.Object.Instantiate(Resources.Load("FX/boost_smoke"), base.transform.position, base.transform.rotation);
			}
			else
			{
				PhotonNetwork.Instantiate("FX/boost_smoke", base.transform.position, base.transform.rotation, 0);
			}
			this.dashTime = 0.5f;
			this.crossFade("dash", 0.1f);
			base.animation["dash"].time = 0.1f;
			this.state = HERO_STATE.AirDodge;
			this.falseAttack();
			base.rigidbody.AddForce(this.dashV * 40f, ForceMode.VelocityChange);
			this._dashCooldownLeft = 0.2f;
		}
	}

	public void die(Vector3 v, bool isBite)
	{
		if (this.invincible <= 0f)
		{
			if (this.titanForm && this.eren_titan != null)
			{
				this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (this.bulletLeft != null)
			{
				this.bulletLeft.GetComponent<Bullet>().removeMe();
			}
			if (this.bulletRight != null)
			{
				this.bulletRight.GetComponent<Bullet>().removeMe();
			}
			this.meatDie.Play();
			if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine) && !this.useGun)
			{
				this.leftbladetrail.Deactivate();
				this.rightbladetrail.Deactivate();
				this.leftbladetrail2.Deactivate();
				this.rightbladetrail2.Deactivate();
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatFeed("Titan", "You", 0);
			}
			this.breakApart2(v, isBite);
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
			this.falseAttack();
			this.hasDied = true;
			Transform obj = base.transform.Find("audio_die");
			obj.parent = null;
			obj.GetComponent<AudioSource>().Play();
			if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
			{
				GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(base.transform.position, 0, null, 0.02f);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void die2(Transform tf)
	{
		if (this.invincible <= 0f)
		{
			if (this.titanForm && this.eren_titan != null)
			{
				this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (this.bulletLeft != null)
			{
				this.bulletLeft.GetComponent<Bullet>().removeMe();
			}
			if (this.bulletRight != null)
			{
				this.bulletRight.GetComponent<Bullet>().removeMe();
			}
			Transform obj = base.transform.Find("audio_die");
			obj.parent = null;
			obj.GetComponent<AudioSource>().Play();
			this.meatDie.Play();
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().ReportKillToChatFeed("Titan", "You", 0);
			}
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
			this.falseAttack();
			this.hasDied = true;
			((GameObject)UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"))).transform.position = base.transform.position;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void dodge2(bool offTheWall = false)
	{
		if (SettingsManager.InputSettings.Human.HorseMount.GetKey() && !(this.myHorse == null) && !this.isMounted && !(Vector3.Distance(this.myHorse.transform.position, base.transform.position) >= 15f))
		{
			return;
		}
		this.state = HERO_STATE.GroundDodge;
		if (!offTheWall)
		{
			float num = (SettingsManager.InputSettings.General.Forward.GetKey() ? 1f : ((!SettingsManager.InputSettings.General.Back.GetKey()) ? 0f : (-1f)));
			float num2 = (SettingsManager.InputSettings.General.Left.GetKey() ? (-1f) : ((!SettingsManager.InputSettings.General.Right.GetKey()) ? 0f : 1f));
			float globalFacingDirection = this.getGlobalFacingDirection(num2, num);
			if (num2 != 0f || num != 0f)
			{
				this.facingDirection = globalFacingDirection + 180f;
				this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
			}
			this.crossFade("dodge", 0.1f);
		}
		else
		{
			this.playAnimation("dodge");
			this.playAnimationAt("dodge", 0.2f);
		}
		this.sparks.enableEmission = false;
	}

	private void erenTransform()
	{
		this.skillCDDuration = this.skillCDLast;
		if (this.bulletLeft != null)
		{
			this.bulletLeft.GetComponent<Bullet>().removeMe();
		}
		if (this.bulletRight != null)
		{
			this.bulletRight.GetComponent<Bullet>().removeMe();
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			this.eren_titan = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), base.transform.position, base.transform.rotation);
		}
		else
		{
			this.eren_titan = PhotonNetwork.Instantiate("TITAN_EREN", base.transform.position, base.transform.rotation, 0);
		}
		this.eren_titan.GetComponent<TITAN_EREN>().realBody = base.gameObject;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(this.eren_titan);
		this.eren_titan.GetComponent<TITAN_EREN>().born();
		this.eren_titan.rigidbody.velocity = base.rigidbody.velocity;
		base.rigidbody.velocity = Vector3.zero;
		base.transform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
		this.titanForm = true;
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			object[] parameters = new object[1] { this.eren_titan.GetPhotonView().viewID };
			base.photonView.RPC("whoIsMyErenTitan", PhotonTargets.Others, parameters);
		}
		if (this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
		{
			object[] parameters2 = new object[1] { false };
			base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters2);
		}
		this.smoke_3dmg.enableEmission = false;
	}

	private void escapeFromGrab()
	{
	}

	public void falseAttack()
	{
		this.attackMove = false;
		if (this.useGun)
		{
			if (!this.attackReleased)
			{
				this.continueAnimation();
				this.attackReleased = true;
			}
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
		{
			this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
			this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
			this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
			this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
			this.leftbladetrail.StopSmoothly(0.2f);
			this.rightbladetrail.StopSmoothly(0.2f);
			this.leftbladetrail2.StopSmoothly(0.2f);
			this.rightbladetrail2.StopSmoothly(0.2f);
		}
		this.attackLoop = 0;
		if (!this.attackReleased)
		{
			this.continueAnimation();
			this.attackReleased = true;
		}
	}

	public void fillGas()
	{
		this.currentGas = this.totalGas;
	}

	private GameObject findNearestTitan()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		GameObject result = null;
		float num = float.PositiveInfinity;
		Vector3 position = base.transform.position;
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			float sqrMagnitude = (gameObject.transform.position - position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				result = gameObject;
				num = sqrMagnitude;
			}
		}
		return result;
	}

	private void FixedUpdate()
	{
		if (!this.titanForm && !this.isCannon && (!GameMenu.Paused || IN_GAME_MAIN_CAMERA.gametype != 0))
		{
			this.currentSpeed = this.baseRigidBody.velocity.magnitude;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
			{
				GameProgressManager.RegisterSpeed(base.gameObject, this.baseRigidBody.velocity.magnitude);
				if (!this.baseAnimation.IsPlaying("attack3_2") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra"))
				{
					this.baseRigidBody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, this.targetRotation, Time.deltaTime * 6f);
				}
				if (this.state == HERO_STATE.Grab)
				{
					this.baseRigidBody.AddForce(-this.baseRigidBody.velocity, ForceMode.VelocityChange);
				}
				else
				{
					if (this.IsGrounded())
					{
						if (!this.grounded)
						{
							this.justGrounded = true;
						}
						this.grounded = true;
					}
					else
					{
						this.grounded = false;
					}
					if (this.hookSomeOne)
					{
						if (this.hookTarget != null)
						{
							Vector3 vector = this.hookTarget.transform.position - this.baseTransform.position;
							float magnitude = vector.magnitude;
							if (magnitude > 2f)
							{
								this.baseRigidBody.AddForce(vector.normalized * Mathf.Pow(magnitude, 0.15f) * 30f - this.baseRigidBody.velocity * 0.95f, ForceMode.VelocityChange);
							}
						}
						else
						{
							this.hookSomeOne = false;
						}
					}
					else if (this.hookBySomeOne && this.badGuy != null)
					{
						if (this.badGuy != null)
						{
							Vector3 vector2 = this.badGuy.transform.position - this.baseTransform.position;
							float magnitude2 = vector2.magnitude;
							if (magnitude2 > 5f)
							{
								this.baseRigidBody.AddForce(vector2.normalized * Mathf.Pow(magnitude2, 0.15f) * 0.2f, ForceMode.Impulse);
							}
						}
						else
						{
							this.hookBySomeOne = false;
						}
					}
					float num = 0f;
					float num2 = 0f;
					if (!IN_GAME_MAIN_CAMERA.isTyping && !GameMenu.InMenu())
					{
						num2 = (SettingsManager.InputSettings.General.Forward.GetKey() ? 1f : ((!SettingsManager.InputSettings.General.Back.GetKey()) ? 0f : (-1f)));
						num = (SettingsManager.InputSettings.General.Left.GetKey() ? (-1f) : ((!SettingsManager.InputSettings.General.Right.GetKey()) ? 0f : 1f));
					}
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					this.isLeftHandHooked = false;
					this.isRightHandHooked = false;
					if (this.isLaunchLeft)
					{
						if (this.bulletLeft != null && this.bulletLeft.GetComponent<Bullet>().isHooked())
						{
							this.isLeftHandHooked = true;
							Vector3 vector3 = this.bulletLeft.transform.position - this.baseTransform.position;
							vector3.Normalize();
							vector3 *= 10f;
							if (!this.isLaunchRight)
							{
								vector3 *= 2f;
							}
							if (Vector3.Angle(this.baseRigidBody.velocity, vector3) > 90f && SettingsManager.InputSettings.Human.Jump.GetKey())
							{
								flag2 = true;
								flag = true;
							}
							if (!flag2)
							{
								this.baseRigidBody.AddForce(vector3);
								if (Vector3.Angle(this.baseRigidBody.velocity, vector3) > 90f)
								{
									this.baseRigidBody.AddForce(-this.baseRigidBody.velocity * 2f, ForceMode.Acceleration);
								}
							}
						}
						this.launchElapsedTimeL += Time.deltaTime;
						if (this.QHold && this.currentGas > 0f)
						{
							this.useGas(this.useGasSpeed * Time.deltaTime);
						}
						else if (this.launchElapsedTimeL > 0.3f)
						{
							this.isLaunchLeft = false;
							if (this.bulletLeft != null)
							{
								this.bulletLeft.GetComponent<Bullet>().disable();
								this.releaseIfIHookSb();
								this.bulletLeft = null;
								flag2 = false;
							}
						}
					}
					if (this.isLaunchRight)
					{
						if (this.bulletRight != null && this.bulletRight.GetComponent<Bullet>().isHooked())
						{
							this.isRightHandHooked = true;
							Vector3 vector4 = this.bulletRight.transform.position - this.baseTransform.position;
							vector4.Normalize();
							vector4 *= 10f;
							if (!this.isLaunchLeft)
							{
								vector4 *= 2f;
							}
							if (Vector3.Angle(this.baseRigidBody.velocity, vector4) > 90f && SettingsManager.InputSettings.Human.Jump.GetKey())
							{
								flag3 = true;
								flag = true;
							}
							if (!flag3)
							{
								this.baseRigidBody.AddForce(vector4);
								if (Vector3.Angle(this.baseRigidBody.velocity, vector4) > 90f)
								{
									this.baseRigidBody.AddForce(-this.baseRigidBody.velocity * 2f, ForceMode.Acceleration);
								}
							}
						}
						this.launchElapsedTimeR += Time.deltaTime;
						if (this.EHold && this.currentGas > 0f)
						{
							this.useGas(this.useGasSpeed * Time.deltaTime);
						}
						else if (this.launchElapsedTimeR > 0.3f)
						{
							this.isLaunchRight = false;
							if (this.bulletRight != null)
							{
								this.bulletRight.GetComponent<Bullet>().disable();
								this.releaseIfIHookSb();
								this.bulletRight = null;
								flag3 = false;
							}
						}
					}
					if (this.grounded)
					{
						Vector3 vector5 = Vector3.zero;
						if (this.state == HERO_STATE.Attack)
						{
							if (this.attackAnimation == "attack5")
							{
								if (this.baseAnimation[this.attackAnimation].normalizedTime > 0.4f && this.baseAnimation[this.attackAnimation].normalizedTime < 0.61f)
								{
									this.baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
								}
							}
							else if (this.attackAnimation == "special_petra")
							{
								if (this.baseAnimation[this.attackAnimation].normalizedTime > 0.35f && this.baseAnimation[this.attackAnimation].normalizedTime < 0.48f)
								{
									this.baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
								}
							}
							else if (this.baseAnimation.IsPlaying("attack3_2"))
							{
								vector5 = Vector3.zero;
							}
							else if (this.baseAnimation.IsPlaying("attack1") || this.baseAnimation.IsPlaying("attack2"))
							{
								this.baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
							}
							if (this.baseAnimation.IsPlaying("attack3_2"))
							{
								vector5 = Vector3.zero;
							}
						}
						if (this.justGrounded)
						{
							if (this.state != HERO_STATE.Attack || (this.attackAnimation != "attack3_1" && this.attackAnimation != "attack5" && this.attackAnimation != "special_petra"))
							{
								if (this.state != HERO_STATE.Attack && num == 0f && num2 == 0f && this.bulletLeft == null && this.bulletRight == null && this.state != HERO_STATE.FillGas)
								{
									this.state = HERO_STATE.Land;
									this.crossFade("dash_land", 0.01f);
								}
								else
								{
									this.buttonAttackRelease = true;
									if (this.state != HERO_STATE.Attack && this.baseRigidBody.velocity.x * this.baseRigidBody.velocity.x + this.baseRigidBody.velocity.z * this.baseRigidBody.velocity.z > this.speed * this.speed * 1.5f && this.state != HERO_STATE.FillGas)
									{
										this.state = HERO_STATE.Slide;
										this.crossFade("slide", 0.05f);
										this.facingDirection = Mathf.Atan2(this.baseRigidBody.velocity.x, this.baseRigidBody.velocity.z) * 57.29578f;
										this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
										this.sparks.enableEmission = true;
									}
								}
							}
							this.justGrounded = false;
							vector5 = this.baseRigidBody.velocity;
						}
						if (this.state == HERO_STATE.Attack && this.attackAnimation == "attack3_1" && this.baseAnimation[this.attackAnimation].normalizedTime >= 1f)
						{
							this.playAnimation("attack3_2");
							this.resetAnimationSpeed();
							Vector3 zero = Vector3.zero;
							this.baseRigidBody.velocity = zero;
							vector5 = zero;
							this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f);
						}
						if (this.state == HERO_STATE.GroundDodge)
						{
							if (this.baseAnimation["dodge"].normalizedTime >= 0.2f && this.baseAnimation["dodge"].normalizedTime < 0.8f)
							{
								vector5 = -this.baseTransform.forward * 2.4f * this.speed;
							}
							if (this.baseAnimation["dodge"].normalizedTime > 0.8f)
							{
								vector5 = this.baseRigidBody.velocity * 0.9f;
							}
						}
						else if (this.state == HERO_STATE.Idle)
						{
							Vector3 vector6 = new Vector3(num, 0f, num2);
							float num3 = this.getGlobalFacingDirection(num, num2);
							vector5 = this.getGlobaleFacingVector3(num3);
							float num4 = ((!(vector6.magnitude <= 0.95f)) ? 1f : ((vector6.magnitude >= 0.25f) ? vector6.magnitude : 0f));
							vector5 *= num4;
							vector5 *= this.speed;
							if (this.buffTime > 0f && this.currentBuff == BUFF.SpeedUp)
							{
								vector5 *= 4f;
							}
							if (num != 0f || num2 != 0f)
							{
								if (!this.baseAnimation.IsPlaying("run") && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("run_sasha") && (!this.baseAnimation.IsPlaying("horse_geton") || this.baseAnimation["horse_geton"].normalizedTime >= 0.5f))
								{
									if (this.buffTime > 0f && this.currentBuff == BUFF.SpeedUp)
									{
										this.crossFade("run_sasha", 0.1f);
									}
									else
									{
										this.crossFade("run", 0.1f);
									}
								}
							}
							else
							{
								if (!this.baseAnimation.IsPlaying(this.standAnimation) && this.state != HERO_STATE.Land && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton") && !this.baseAnimation.IsPlaying("grabbed"))
								{
									this.crossFade(this.standAnimation, 0.1f);
									vector5 *= 0f;
								}
								num3 = -874f;
							}
							if (num3 != -874f)
							{
								this.facingDirection = num3;
								this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
							}
						}
						else if (this.state == HERO_STATE.Land)
						{
							vector5 = this.baseRigidBody.velocity * 0.96f;
						}
						else if (this.state == HERO_STATE.Slide)
						{
							vector5 = this.baseRigidBody.velocity * 0.99f;
							if (this.currentSpeed < this.speed * 1.2f)
							{
								this.idle();
								this.sparks.enableEmission = false;
							}
						}
						Vector3 velocity = this.baseRigidBody.velocity;
						Vector3 force = vector5 - velocity;
						force.x = Mathf.Clamp(force.x, 0f - this.maxVelocityChange, this.maxVelocityChange);
						force.z = Mathf.Clamp(force.z, 0f - this.maxVelocityChange, this.maxVelocityChange);
						force.y = 0f;
						if (this.baseAnimation.IsPlaying("jump") && this.baseAnimation["jump"].normalizedTime > 0.18f)
						{
							force.y += 8f;
						}
						if (this.baseAnimation.IsPlaying("horse_geton") && this.baseAnimation["horse_geton"].normalizedTime > 0.18f && this.baseAnimation["horse_geton"].normalizedTime < 1f)
						{
							float num5 = 6f;
							force = -this.baseRigidBody.velocity;
							force.y = num5;
							float num6 = Vector3.Distance(this.myHorse.transform.position, this.baseTransform.position);
							float num7 = 0.6f * this.gravity * num6 / (2f * num5);
							force += num7 * (this.myHorse.transform.position - this.baseTransform.position).normalized;
						}
						if (this.state != HERO_STATE.Attack || !this.useGun)
						{
							this.baseRigidBody.AddForce(force, ForceMode.VelocityChange);
							this.baseRigidBody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.facingDirection, 0f), Time.deltaTime * 10f);
						}
					}
					else
					{
						if (this.sparks.enableEmission)
						{
							this.sparks.enableEmission = false;
						}
						if (this.myHorse != null && (this.baseAnimation.IsPlaying("horse_geton") || this.baseAnimation.IsPlaying("air_fall")) && this.baseRigidBody.velocity.y < 0f && Vector3.Distance(this.myHorse.transform.position + Vector3.up * 1.65f, this.baseTransform.position) < 0.5f)
						{
							this.baseTransform.position = this.myHorse.transform.position + Vector3.up * 1.65f;
							this.baseTransform.rotation = this.myHorse.transform.rotation;
							this.isMounted = true;
							if (!base.animation.IsPlaying("horse_idle"))
							{
								this.crossFade("horse_idle", 0.1f);
							}
							this.myHorse.GetComponent<Horse>().mounted();
						}
						if ((this.state == HERO_STATE.Idle && !this.baseAnimation.IsPlaying("dash") && !this.baseAnimation.IsPlaying("wallrun") && !this.baseAnimation.IsPlaying("toRoof") && !this.baseAnimation.IsPlaying("horse_geton") && !this.baseAnimation.IsPlaying("horse_getoff") && !this.baseAnimation.IsPlaying("air_release") && !this.isMounted && (!this.baseAnimation.IsPlaying("air_hook_l_just") || this.baseAnimation["air_hook_l_just"].normalizedTime >= 1f) && (!this.baseAnimation.IsPlaying("air_hook_r_just") || this.baseAnimation["air_hook_r_just"].normalizedTime >= 1f)) || this.baseAnimation["dash"].normalizedTime >= 0.99f)
						{
							if (!this.isLeftHandHooked && !this.isRightHandHooked && (this.baseAnimation.IsPlaying("air_hook_l") || this.baseAnimation.IsPlaying("air_hook_r") || this.baseAnimation.IsPlaying("air_hook")) && this.baseRigidBody.velocity.y > 20f)
							{
								this.baseAnimation.CrossFade("air_release");
							}
							else
							{
								bool num8 = Mathf.Abs(this.baseRigidBody.velocity.x) + Mathf.Abs(this.baseRigidBody.velocity.z) > 25f;
								bool flag4 = this.baseRigidBody.velocity.y < 0f;
								if (!num8)
								{
									if (flag4)
									{
										if (!this.baseAnimation.IsPlaying("air_fall"))
										{
											this.crossFade("air_fall", 0.2f);
										}
									}
									else if (!this.baseAnimation.IsPlaying("air_rise"))
									{
										this.crossFade("air_rise", 0.2f);
									}
								}
								else if (!this.isLeftHandHooked && !this.isRightHandHooked)
								{
									float num9 = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(this.baseRigidBody.velocity.z, this.baseRigidBody.velocity.x)) * 57.29578f, this.baseTransform.rotation.eulerAngles.y - 90f);
									if (Mathf.Abs(num9) < 45f)
									{
										if (!this.baseAnimation.IsPlaying("air2"))
										{
											this.crossFade("air2", 0.2f);
										}
									}
									else if (num9 < 135f && num9 > 0f)
									{
										if (!this.baseAnimation.IsPlaying("air2_right"))
										{
											this.crossFade("air2_right", 0.2f);
										}
									}
									else if (num9 > -135f && num9 < 0f)
									{
										if (!this.baseAnimation.IsPlaying("air2_left"))
										{
											this.crossFade("air2_left", 0.2f);
										}
									}
									else if (!this.baseAnimation.IsPlaying("air2_backward"))
									{
										this.crossFade("air2_backward", 0.2f);
									}
								}
								else if (this.useGun)
								{
									if (!this.isRightHandHooked)
									{
										if (!this.baseAnimation.IsPlaying("AHSS_hook_forward_l"))
										{
											this.crossFade("AHSS_hook_forward_l", 0.1f);
										}
									}
									else if (!this.isLeftHandHooked)
									{
										if (!this.baseAnimation.IsPlaying("AHSS_hook_forward_r"))
										{
											this.crossFade("AHSS_hook_forward_r", 0.1f);
										}
									}
									else if (!this.baseAnimation.IsPlaying("AHSS_hook_forward_both"))
									{
										this.crossFade("AHSS_hook_forward_both", 0.1f);
									}
								}
								else if (!this.isRightHandHooked)
								{
									if (!this.baseAnimation.IsPlaying("air_hook_l"))
									{
										this.crossFade("air_hook_l", 0.1f);
									}
								}
								else if (!this.isLeftHandHooked)
								{
									if (!this.baseAnimation.IsPlaying("air_hook_r"))
									{
										this.crossFade("air_hook_r", 0.1f);
									}
								}
								else if (!this.baseAnimation.IsPlaying("air_hook"))
								{
									this.crossFade("air_hook", 0.1f);
								}
							}
						}
						if (!this.baseAnimation.IsPlaying("air_rise"))
						{
							if (this.state == HERO_STATE.Idle && this.baseAnimation.IsPlaying("air_release") && this.baseAnimation["air_release"].normalizedTime >= 1f)
							{
								this.crossFade("air_rise", 0.2f);
							}
							if (this.baseAnimation.IsPlaying("horse_getoff") && this.baseAnimation["horse_getoff"].normalizedTime >= 1f)
							{
								this.crossFade("air_rise", 0.2f);
							}
						}
						if (this.baseAnimation.IsPlaying("toRoof"))
						{
							if (this.baseAnimation["toRoof"].normalizedTime < 0.22f)
							{
								this.baseRigidBody.velocity = Vector3.zero;
								this.baseRigidBody.AddForce(new Vector3(0f, this.gravity * this.baseRigidBody.mass, 0f));
							}
							else
							{
								if (!this.wallJump)
								{
									this.wallJump = true;
									this.baseRigidBody.AddForce(Vector3.up * 8f, ForceMode.Impulse);
								}
								this.baseRigidBody.AddForce(this.baseTransform.forward * 0.05f, ForceMode.Impulse);
							}
							if (this.baseAnimation["toRoof"].normalizedTime >= 1f)
							{
								this.playAnimation("air_rise");
							}
						}
						else if (this.state == HERO_STATE.Idle && this.isPressDirectionTowardsHero(num, num2) && !SettingsManager.InputSettings.Human.Jump.GetKey() && !SettingsManager.InputSettings.Human.HookLeft.GetKey() && !SettingsManager.InputSettings.Human.HookRight.GetKey() && !SettingsManager.InputSettings.Human.HookBoth.GetKey() && this.IsFrontGrounded() && !this.baseAnimation.IsPlaying("wallrun") && !this.baseAnimation.IsPlaying("dodge"))
						{
							this.crossFade("wallrun", 0.1f);
							this.wallRunTime = 0f;
						}
						else if (this.baseAnimation.IsPlaying("wallrun"))
						{
							this.baseRigidBody.AddForce(Vector3.up * this.speed - this.baseRigidBody.velocity, ForceMode.VelocityChange);
							this.wallRunTime += Time.deltaTime;
							if (this.wallRunTime > 1f || (num2 == 0f && num == 0f))
							{
								this.baseRigidBody.AddForce(-this.baseTransform.forward * this.speed * 0.75f, ForceMode.Impulse);
								this.dodge2(offTheWall: true);
							}
							else if (!this.IsUpFrontGrounded())
							{
								this.wallJump = false;
								this.crossFade("toRoof", 0.1f);
							}
							else if (!this.IsFrontGrounded())
							{
								this.crossFade("air_fall", 0.1f);
							}
						}
						else if (!this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && !this.baseAnimation.IsPlaying("dash") && !this.baseAnimation.IsPlaying("jump") && !this.IsFiringThunderSpear())
						{
							Vector3 vector7 = new Vector3(num, 0f, num2);
							float num10 = this.getGlobalFacingDirection(num, num2);
							Vector3 globaleFacingVector = this.getGlobaleFacingVector3(num10);
							float num11 = ((!(vector7.magnitude <= 0.95f)) ? 1f : ((vector7.magnitude >= 0.25f) ? vector7.magnitude : 0f));
							globaleFacingVector *= num11;
							globaleFacingVector *= (float)this.setup.myCostume.stat.ACL / 10f * 2f;
							if (num == 0f && num2 == 0f)
							{
								if (this.state == HERO_STATE.Attack)
								{
									globaleFacingVector *= 0f;
								}
								num10 = -874f;
							}
							if (num10 != -874f)
							{
								this.facingDirection = num10;
								this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
							}
							if (!flag2 && !flag3 && !this.isMounted && SettingsManager.InputSettings.Human.Jump.GetKey() && this.currentGas > 0f)
							{
								if (num != 0f || num2 != 0f)
								{
									this.baseRigidBody.AddForce(globaleFacingVector, ForceMode.Acceleration);
								}
								else
								{
									this.baseRigidBody.AddForce(this.baseTransform.forward * globaleFacingVector.magnitude, ForceMode.Acceleration);
								}
								flag = true;
							}
						}
						if (this.baseAnimation.IsPlaying("air_fall") && this.currentSpeed < 0.2f && this.IsFrontGrounded())
						{
							this.crossFade("onWall", 0.3f);
						}
					}
					this.spinning = false;
					if (flag2 && flag3)
					{
						float num12 = this.currentSpeed + 0.1f;
						this.baseRigidBody.AddForce(-this.baseRigidBody.velocity, ForceMode.VelocityChange);
						Vector3 current = (this.bulletRight.transform.position + this.bulletLeft.transform.position) * 0.5f - this.baseTransform.position;
						float reelAxis = this.GetReelAxis();
						reelAxis = Mathf.Clamp(reelAxis, -0.8f, 0.8f);
						float num13 = 1f + reelAxis;
						Vector3 vector8 = Vector3.RotateTowards(current, this.baseRigidBody.velocity, 1.53938f * num13, 1.53938f * num13);
						vector8.Normalize();
						this.spinning = true;
						this.baseRigidBody.velocity = vector8 * num12;
					}
					else if (flag2)
					{
						float num14 = this.currentSpeed + 0.1f;
						this.baseRigidBody.AddForce(-this.baseRigidBody.velocity, ForceMode.VelocityChange);
						Vector3 current2 = this.bulletLeft.transform.position - this.baseTransform.position;
						float reelAxis2 = this.GetReelAxis();
						reelAxis2 = Mathf.Clamp(reelAxis2, -0.8f, 0.8f);
						float num15 = 1f + reelAxis2;
						Vector3 vector9 = Vector3.RotateTowards(current2, this.baseRigidBody.velocity, 1.53938f * num15, 1.53938f * num15);
						vector9.Normalize();
						this.spinning = true;
						this.baseRigidBody.velocity = vector9 * num14;
					}
					else if (flag3)
					{
						float num16 = this.currentSpeed + 0.1f;
						this.baseRigidBody.AddForce(-this.baseRigidBody.velocity, ForceMode.VelocityChange);
						Vector3 current3 = this.bulletRight.transform.position - this.baseTransform.position;
						float reelAxis3 = this.GetReelAxis();
						reelAxis3 = Mathf.Clamp(reelAxis3, -0.8f, 0.8f);
						float num17 = 1f + reelAxis3;
						Vector3 vector10 = Vector3.RotateTowards(current3, this.baseRigidBody.velocity, 1.53938f * num17, 1.53938f * num17);
						vector10.Normalize();
						this.spinning = true;
						this.baseRigidBody.velocity = vector10 * num16;
					}
					if (this.state == HERO_STATE.Attack && (this.attackAnimation == "attack5" || this.attackAnimation == "special_petra") && this.baseAnimation[this.attackAnimation].normalizedTime > 0.4f && !this.attackMove)
					{
						this.attackMove = true;
						if (this.launchPointRight.magnitude > 0f)
						{
							Vector3 force2 = this.launchPointRight - this.baseTransform.position;
							force2.Normalize();
							force2 *= 13f;
							this.baseRigidBody.AddForce(force2, ForceMode.Impulse);
						}
						if (this.attackAnimation == "special_petra" && this.launchPointLeft.magnitude > 0f)
						{
							Vector3 force3 = this.launchPointLeft - this.baseTransform.position;
							force3.Normalize();
							force3 *= 13f;
							this.baseRigidBody.AddForce(force3, ForceMode.Impulse);
							if (this.bulletRight != null)
							{
								this.bulletRight.GetComponent<Bullet>().disable();
								this.releaseIfIHookSb();
							}
							if (this.bulletLeft != null)
							{
								this.bulletLeft.GetComponent<Bullet>().disable();
								this.releaseIfIHookSb();
							}
						}
						this.baseRigidBody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
					}
					bool flag5 = false;
					if (this.bulletLeft != null || this.bulletRight != null)
					{
						if (this.bulletLeft != null && this.bulletLeft.transform.position.y > base.gameObject.transform.position.y && this.isLaunchLeft && this.bulletLeft.GetComponent<Bullet>().isHooked())
						{
							flag5 = true;
						}
						if (this.bulletRight != null && this.bulletRight.transform.position.y > base.gameObject.transform.position.y && this.isLaunchRight && this.bulletRight.GetComponent<Bullet>().isHooked())
						{
							flag5 = true;
						}
					}
					if (flag5)
					{
						this.baseRigidBody.AddForce(new Vector3(0f, -10f * this.baseRigidBody.mass, 0f));
					}
					else
					{
						this.baseRigidBody.AddForce(new Vector3(0f, (0f - this.gravity) * this.baseRigidBody.mass, 0f));
					}
					if (this.currentSpeed > 10f)
					{
						this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min(100f, this.currentSpeed + 40f), 0.1f);
					}
					else
					{
						this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
					}
					if (!this._cancelGasDisable)
					{
						if (flag)
						{
							this.useGas(this.useGasSpeed * Time.deltaTime);
							if (!this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
							{
								object[] parameters = new object[1] { true };
								base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
							}
							this.smoke_3dmg.enableEmission = true;
						}
						else
						{
							if (this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
							{
								object[] parameters2 = new object[1] { false };
								base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters2);
							}
							this.smoke_3dmg.enableEmission = false;
						}
					}
					else
					{
						this._cancelGasDisable = false;
					}
					if (WindWeatherEffect.WindEnabled)
					{
						if (!this.speedFXPS.enableEmission)
						{
							this.speedFXPS.enableEmission = true;
						}
						this.speedFXPS.startSpeed = 100f;
						this.speedFX.transform.LookAt(this.baseTransform.position + WindWeatherEffect.WindDirection);
					}
					else if (this.currentSpeed > 80f && SettingsManager.GraphicsSettings.WindEffectEnabled.Value)
					{
						if (!this.speedFXPS.enableEmission)
						{
							this.speedFXPS.enableEmission = true;
						}
						this.speedFXPS.startSpeed = this.currentSpeed;
						this.speedFX.transform.LookAt(this.baseTransform.position + this.baseRigidBody.velocity);
					}
					else if (this.speedFXPS.enableEmission)
					{
						this.speedFXPS.enableEmission = false;
					}
				}
			}
			this.setHookedPplDirection();
			this.bodyLean();
		}
		this._reelInAxis = 0f;
	}

	public string getDebugInfo()
	{
		string text = "\n";
		text = "Left:" + this.isLeftHandHooked + " ";
		if (this.isLeftHandHooked && this.bulletLeft != null)
		{
			Vector3 vector = this.bulletLeft.transform.position - base.transform.position;
			text += (int)(Mathf.Atan2(vector.x, vector.z) * 57.29578f);
		}
		string text2 = text;
		text = text2 + "\nRight:" + this.isRightHandHooked + " ";
		if (this.isRightHandHooked && this.bulletRight != null)
		{
			Vector3 vector2 = this.bulletRight.transform.position - base.transform.position;
			text += (int)(Mathf.Atan2(vector2.x, vector2.z) * 57.29578f);
		}
		text = text + "\nfacingDirection:" + (int)this.facingDirection + "\nActual facingDirection:" + (int)base.transform.rotation.eulerAngles.y + "\nState:" + this.state.ToString() + "\n\n\n\n\n";
		if (this.state == HERO_STATE.Attack)
		{
			this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
		}
		return text;
	}

	private Vector3 getGlobaleFacingVector3(float resultAngle)
	{
		float num = 0f - resultAngle + 90f;
		return new Vector3(Mathf.Cos(num * 0.01745329f), 0f, Mathf.Sin(num * 0.01745329f));
	}

	private Vector3 getGlobaleFacingVector3(float horizontal, float vertical)
	{
		float num = 0f - this.getGlobalFacingDirection(horizontal, vertical) + 90f;
		return new Vector3(Mathf.Cos(num * 0.01745329f), 0f, Mathf.Sin(num * 0.01745329f));
	}

	private float getGlobalFacingDirection(float horizontal, float vertical)
	{
		if (vertical == 0f && horizontal == 0f)
		{
			return base.transform.rotation.eulerAngles.y;
		}
		float y = this.currentCamera.transform.rotation.eulerAngles.y;
		float num = Mathf.Atan2(vertical, horizontal) * 57.29578f;
		num = 0f - num + 90f;
		return y + num;
	}

	private float getLeanAngle(Vector3 p, bool left)
	{
		if (!this.useGun && this.state == HERO_STATE.Attack)
		{
			return 0f;
		}
		float num = p.y - base.transform.position.y;
		float num2 = Vector3.Distance(p, base.transform.position);
		float num3 = Mathf.Acos(num / num2) * 57.29578f;
		num3 *= 0.1f;
		num3 *= 1f + Mathf.Pow(base.rigidbody.velocity.magnitude, 0.2f);
		Vector3 vector = p - base.transform.position;
		float current = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
		float target = Mathf.Atan2(base.rigidbody.velocity.x, base.rigidbody.velocity.z) * 57.29578f;
		float num4 = Mathf.DeltaAngle(current, target);
		num3 += Mathf.Abs(num4 * 0.5f);
		if (this.state != HERO_STATE.Attack)
		{
			num3 = Mathf.Min(num3, 80f);
		}
		if (num4 > 0f)
		{
			this.leanLeft = true;
		}
		else
		{
			this.leanLeft = false;
		}
		if (this.useGun)
		{
			return num3 * ((num4 >= 0f) ? 1f : (-1f));
		}
		float num5 = 0f;
		num5 = (((!left || !(num4 < 0f)) && (left || !(num4 > 0f))) ? 0.5f : 0.1f);
		return num3 * ((num4 >= 0f) ? num5 : (0f - num5));
	}

	private void getOffHorse()
	{
		this.playAnimation("horse_getoff");
		base.rigidbody.AddForce(Vector3.up * 10f - base.transform.forward * 2f - base.transform.right * 1f, ForceMode.VelocityChange);
		this.unmounted();
	}

	private void getOnHorse()
	{
		this.playAnimation("horse_geton");
		this.facingDirection = this.myHorse.transform.rotation.eulerAngles.y;
		this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
	}

	public void getSupply()
	{
		if ((base.animation.IsPlaying(this.standAnimation) || base.animation.IsPlaying("run") || base.animation.IsPlaying("run_sasha")) && (this.currentBladeSta != this.totalBladeSta || this.currentBladeNum != this.totalBladeNum || this.currentGas != this.totalGas || this.leftBulletLeft != this.bulletMAX || this.rightBulletLeft != this.bulletMAX))
		{
			this.state = HERO_STATE.FillGas;
			this.crossFade("supply", 0.1f);
		}
	}

	public void grabbed(GameObject titan, bool leftHand)
	{
		if (this.isMounted)
		{
			this.unmounted();
		}
		this.state = HERO_STATE.Grab;
		base.GetComponent<CapsuleCollider>().isTrigger = true;
		this.falseAttack();
		this.titanWhoGrabMe = titan;
		if (this.titanForm && this.eren_titan != null)
		{
			this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
		}
		if (!this.useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
		{
			this.leftbladetrail.Deactivate();
			this.rightbladetrail.Deactivate();
			this.leftbladetrail2.Deactivate();
			this.rightbladetrail2.Deactivate();
		}
		this.smoke_3dmg.enableEmission = false;
		this.sparks.enableEmission = false;
	}

	public bool HasDied()
	{
		if (!this.hasDied)
		{
			return this.isInvincible();
		}
		return true;
	}

	private void headMovement()
	{
		Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
		Transform obj = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
		float x = Mathf.Sqrt((this.gunTarget.x - base.transform.position.x) * (this.gunTarget.x - base.transform.position.x) + (this.gunTarget.z - base.transform.position.z) * (this.gunTarget.z - base.transform.position.z));
		this.targetHeadRotation = transform.rotation;
		Vector3 vector = this.gunTarget - base.transform.position;
		float value = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, base.transform.rotation.eulerAngles.y - 90f);
		value = Mathf.Clamp(value, -40f, 40f);
		float value2 = Mathf.Atan2(obj.position.y - this.gunTarget.y, x) * 57.29578f;
		value2 = Mathf.Clamp(value2, -40f, 30f);
		this.targetHeadRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + value2, transform.rotation.eulerAngles.y + value, transform.rotation.eulerAngles.z);
		this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 60f);
		transform.rotation = this.oldHeadRotation;
	}

	public void hookedByHuman(int hooker, Vector3 hookPosition)
	{
		object[] parameters = new object[2] { hooker, hookPosition };
		base.photonView.RPC("RPCHookedByHuman", base.photonView.owner, parameters);
	}

	[RPC]
	public void hookFail()
	{
		this.hookTarget = null;
		this.hookSomeOne = false;
	}

	public void hookToHuman(GameObject target, Vector3 hookPosition)
	{
		this.releaseIfIHookSb();
		this.hookTarget = target;
		this.hookSomeOne = true;
		if (target.GetComponent<HERO>() != null)
		{
			target.GetComponent<HERO>().hookedByHuman(base.photonView.viewID, hookPosition);
		}
		this.launchForce = hookPosition - base.transform.position;
		float num = Mathf.Pow(this.launchForce.magnitude, 0.1f);
		if (this.grounded)
		{
			base.rigidbody.AddForce(Vector3.up * Mathf.Min(this.launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
		}
		base.rigidbody.AddForce(this.launchForce * num * 0.1f, ForceMode.Impulse);
	}

	private void idle()
	{
		if (this.state == HERO_STATE.Attack)
		{
			this.falseAttack();
		}
		this.state = HERO_STATE.Idle;
		this.crossFade(this.standAnimation, 0.1f);
	}

	private bool IsFrontGrounded()
	{
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = (int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask;
		return Physics.Raycast(base.gameObject.transform.position + base.gameObject.transform.up * 1f, base.gameObject.transform.forward, 1f, layerMask2.value);
	}

	public bool IsGrounded()
	{
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = (int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask;
		return Physics.Raycast(base.gameObject.transform.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, layerMask2.value);
	}

	public bool isInvincible()
	{
		return this.invincible > 0f;
	}

	private bool isPressDirectionTowardsHero(float h, float v)
	{
		if (h == 0f && v == 0f)
		{
			return false;
		}
		return Mathf.Abs(Mathf.DeltaAngle(this.getGlobalFacingDirection(h, v), base.transform.rotation.eulerAngles.y)) < 45f;
	}

	private bool IsUpFrontGrounded()
	{
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = (int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask;
		return Physics.Raycast(base.gameObject.transform.position + base.gameObject.transform.up * 3f, base.gameObject.transform.forward, 1.2f, layerMask2.value);
	}

	[RPC]
	private void killObject(PhotonMessageInfo info)
	{
		if (info != null)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero killObject exploit");
		}
	}

	public void lateUpdate2()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && this.myNetWorkName != null)
		{
			if (this.titanForm && this.eren_titan != null)
			{
				this.myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
			}
			Vector3 vector = new Vector3(this.baseTransform.position.x, this.baseTransform.position.y + 2f, this.baseTransform.position.z);
			GameObject gameObject = this.maincamera;
			LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
			LayerMask layerMask2 = (int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask;
			if (Vector3.Angle(gameObject.transform.forward, vector - gameObject.transform.position) > 90f || Physics.Linecast(vector, gameObject.transform.position, layerMask2))
			{
				this.myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
			}
			else
			{
				Vector2 vector2 = this.maincamera.GetComponent<Camera>().WorldToScreenPoint(vector);
				this.myNetWorkName.transform.localPosition = new Vector3((int)(vector2.x - (float)Screen.width * 0.5f), (int)(vector2.y - (float)Screen.height * 0.5f), 0f);
			}
		}
		if (this.titanForm || this.isCannon)
		{
			return;
		}
		if (SettingsManager.GeneralSettings.CameraTilt.Value && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
		{
			Vector3 vector3 = Vector3.zero;
			Vector3 vector4 = Vector3.zero;
			if (this.isLaunchLeft && this.bulletLeft != null && this.bulletLeft.GetComponent<Bullet>().isHooked())
			{
				vector3 = this.bulletLeft.transform.position;
			}
			if (this.isLaunchRight && this.bulletRight != null && this.bulletRight.GetComponent<Bullet>().isHooked())
			{
				vector4 = this.bulletRight.transform.position;
			}
			Vector3 vector5 = Vector3.zero;
			if (vector3.magnitude != 0f && vector4.magnitude == 0f)
			{
				vector5 = vector3;
			}
			else if (vector3.magnitude == 0f && vector4.magnitude != 0f)
			{
				vector5 = vector4;
			}
			else if (vector3.magnitude != 0f && vector4.magnitude != 0f)
			{
				vector5 = (vector3 + vector4) * 0.5f;
			}
			Vector3 vector6 = Vector3.Project(vector5 - this.baseTransform.position, this.maincamera.transform.up);
			Vector3 vector7 = Vector3.Project(vector5 - this.baseTransform.position, this.maincamera.transform.right);
			Quaternion to2;
			if (vector5.magnitude > 0f)
			{
				Vector3 to = vector6 + vector7;
				float num = Vector3.Angle(vector5 - this.baseTransform.position, this.baseRigidBody.velocity) * 0.005f;
				Vector3 vector8 = this.maincamera.transform.right + vector7.normalized;
				to2 = Quaternion.Euler(this.maincamera.transform.rotation.eulerAngles.x, this.maincamera.transform.rotation.eulerAngles.y, (vector8.magnitude >= 1f) ? ((0f - Vector3.Angle(vector6, to)) * num) : (Vector3.Angle(vector6, to) * num));
			}
			else
			{
				to2 = Quaternion.Euler(this.maincamera.transform.rotation.eulerAngles.x, this.maincamera.transform.rotation.eulerAngles.y, 0f);
			}
			this.maincamera.transform.rotation = Quaternion.Lerp(this.maincamera.transform.rotation, to2, Time.deltaTime * 2f);
		}
		if (this.state == HERO_STATE.Grab && this.titanWhoGrabMe != null)
		{
			if (this.titanWhoGrabMe.GetComponent<TITAN>() != null)
			{
				this.baseTransform.position = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
				this.baseTransform.rotation = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
			}
			else if (this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
			{
				this.baseTransform.position = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
				this.baseTransform.rotation = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
			}
		}
		if (!this.useGun)
		{
			return;
		}
		if (this.leftArmAim || this.rightArmAim)
		{
			Vector3 vector9 = this.gunTarget - this.baseTransform.position;
			float num2 = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector9.z, vector9.x)) * 57.29578f, this.baseTransform.rotation.eulerAngles.y - 90f);
			this.headMovement();
			if (!this.isLeftHandHooked && this.leftArmAim && num2 < 40f && num2 > -90f)
			{
				this.leftArmAimTo(this.gunTarget);
			}
			if (!this.isRightHandHooked && this.rightArmAim && num2 > -40f && num2 < 90f)
			{
				this.rightArmAimTo(this.gunTarget);
			}
		}
		else if (!this.grounded)
		{
			this.handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
			this.handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		}
		if (this.isLeftHandHooked && this.bulletLeft != null)
		{
			this.leftArmAimTo(this.bulletLeft.transform.position);
		}
		if (this.isRightHandHooked && this.bulletRight != null)
		{
			this.rightArmAimTo(this.bulletRight.transform.position);
		}
	}

	public void launch(Vector3 des, bool left = true, bool leviMode = false)
	{
		if (left)
		{
			this.isLaunchLeft = true;
			this.launchElapsedTimeL = 0f;
		}
		else
		{
			this.isLaunchRight = true;
			this.launchElapsedTimeR = 0f;
		}
		if (this.state == HERO_STATE.Grab)
		{
			return;
		}
		if (this.isMounted)
		{
			this.unmounted();
		}
		if (this.state != HERO_STATE.Attack)
		{
			this.idle();
		}
		Vector3 vector = des - base.transform.position;
		if (left)
		{
			this.launchPointLeft = des;
		}
		else
		{
			this.launchPointRight = des;
		}
		vector.Normalize();
		vector *= 20f;
		if (this.bulletLeft != null && this.bulletRight != null && this.bulletLeft.GetComponent<Bullet>().isHooked() && this.bulletRight.GetComponent<Bullet>().isHooked())
		{
			vector *= 0.8f;
		}
		leviMode = ((base.animation.IsPlaying("attack5") || base.animation.IsPlaying("special_petra")) ? true : false);
		if (!leviMode)
		{
			this.falseAttack();
			this.idle();
			if (this.useGun)
			{
				this.crossFade("AHSS_hook_forward_both", 0.1f);
			}
			else if (left && !this.isRightHandHooked)
			{
				this.crossFade("air_hook_l_just", 0.1f);
			}
			else if (!left && !this.isLeftHandHooked)
			{
				this.crossFade("air_hook_r_just", 0.1f);
			}
			else
			{
				this.crossFade("dash", 0.1f);
				base.animation["dash"].time = 0f;
			}
		}
		this.launchForce = vector;
		if (!leviMode)
		{
			if (vector.y < 30f)
			{
				this.launchForce += Vector3.up * (30f - vector.y);
			}
			if (des.y >= base.transform.position.y)
			{
				this.launchForce += Vector3.up * (des.y - base.transform.position.y) * 10f;
			}
			base.rigidbody.AddForce(this.launchForce);
		}
		this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
		Quaternion rotation = Quaternion.Euler(0f, this.facingDirection, 0f);
		base.gameObject.transform.rotation = rotation;
		base.rigidbody.rotation = rotation;
		this.targetRotation = rotation;
		if (leviMode)
		{
			this.launchElapsedTimeR = -100f;
		}
		if (base.animation.IsPlaying("special_petra"))
		{
			this.launchElapsedTimeR = -100f;
			this.launchElapsedTimeL = -100f;
			if (this.bulletRight != null)
			{
				this.bulletRight.GetComponent<Bullet>().disable();
				this.releaseIfIHookSb();
			}
			if (this.bulletLeft != null)
			{
				this.bulletLeft.GetComponent<Bullet>().disable();
				this.releaseIfIHookSb();
			}
		}
		this._cancelGasDisable = true;
		this.sparks.enableEmission = false;
	}

	private void launchLeftRope(RaycastHit hit, bool single, int mode = 0)
	{
		if (this.currentGas != 0f)
		{
			this.useGas();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.bulletLeft = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hook"));
			}
			else if (base.photonView.isMine)
			{
				this.bulletLeft = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
			}
			GameObject gameObject = ((!this.useGun) ? this.hookRefL1 : this.hookRefL2);
			string launcher_ref = ((!this.useGun) ? "hookRefL1" : "hookRefL2");
			this.bulletLeft.transform.position = gameObject.transform.position;
			Bullet component = this.bulletLeft.GetComponent<Bullet>();
			float num = (single ? 0f : ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)));
			Vector3 vector = hit.point - base.transform.right * num - this.bulletLeft.transform.position;
			vector.Normalize();
			if (mode == 1)
			{
				component.launch(vector * 3f, base.rigidbody.velocity, launcher_ref, isLeft: true, base.gameObject, leviMode: true);
			}
			else
			{
				component.launch(vector * 3f, base.rigidbody.velocity, launcher_ref, isLeft: true, base.gameObject);
			}
			this.launchPointLeft = Vector3.zero;
		}
	}

	private void launchRightRope(RaycastHit hit, bool single, int mode = 0)
	{
		if (this.currentGas != 0f)
		{
			this.useGas();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.bulletRight = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hook"));
			}
			else if (base.photonView.isMine)
			{
				this.bulletRight = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
			}
			GameObject gameObject = ((!this.useGun) ? this.hookRefR1 : this.hookRefR2);
			string launcher_ref = ((!this.useGun) ? "hookRefR1" : "hookRefR2");
			this.bulletRight.transform.position = gameObject.transform.position;
			Bullet component = this.bulletRight.GetComponent<Bullet>();
			float num = (single ? 0f : ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)));
			Vector3 vector = hit.point + base.transform.right * num - this.bulletRight.transform.position;
			vector.Normalize();
			if (mode == 1)
			{
				component.launch(vector * 5f, base.rigidbody.velocity, launcher_ref, isLeft: false, base.gameObject, leviMode: true);
			}
			else
			{
				component.launch(vector * 3f, base.rigidbody.velocity, launcher_ref, isLeft: false, base.gameObject);
			}
			this.launchPointRight = Vector3.zero;
		}
	}

	private void leftArmAimTo(Vector3 target)
	{
		float num = target.x - this.upperarmL.transform.position.x;
		float y = target.y - this.upperarmL.transform.position.y;
		float num2 = target.z - this.upperarmL.transform.position.z;
		float x = Mathf.Sqrt(num * num + num2 * num2);
		this.handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
		this.forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		this.upperarmL.rotation = Quaternion.Euler(0f, 90f + Mathf.Atan2(num, num2) * 57.29578f, (0f - Mathf.Atan2(y, x)) * 57.29578f);
	}

	public void loadskin()
	{
		if ((IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine) || !SettingsManager.CustomSkinSettings.Human.SkinsEnabled.Value)
		{
			return;
		}
		HumanCustomSkinSet humanCustomSkinSet = (HumanCustomSkinSet)SettingsManager.CustomSkinSettings.Human.GetSelectedSet();
		string text = string.Join(",", new string[19]
		{
			humanCustomSkinSet.Horse.Value,
			humanCustomSkinSet.Hair.Value,
			humanCustomSkinSet.Eye.Value,
			humanCustomSkinSet.Glass.Value,
			humanCustomSkinSet.Face.Value,
			humanCustomSkinSet.Skin.Value,
			humanCustomSkinSet.Costume.Value,
			humanCustomSkinSet.Logo.Value,
			humanCustomSkinSet.GearL.Value,
			humanCustomSkinSet.GearR.Value,
			humanCustomSkinSet.Gas.Value,
			humanCustomSkinSet.Hoodie.Value,
			humanCustomSkinSet.WeaponTrail.Value,
			humanCustomSkinSet.ThunderSpearL.Value,
			humanCustomSkinSet.ThunderSpearR.Value,
			humanCustomSkinSet.HookL.Value,
			humanCustomSkinSet.HookLTiling.Value.ToString(),
			humanCustomSkinSet.HookR.Value,
			humanCustomSkinSet.HookRTiling.Value.ToString()
		});
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			base.StartCoroutine(this.loadskinE(-1, text));
			return;
		}
		int num = -1;
		if (this.myHorse != null)
		{
			num = this.myHorse.GetPhotonView().viewID;
		}
		base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, num, text);
	}

	public IEnumerator loadskinE(int horse, string url)
	{
		while (!this._hasRunStart)
		{
			yield return null;
		}
		this._customSkinLoader.StartCoroutine(this._customSkinLoader.LoadSkinsFromRPC(new object[2] { horse, url }));
	}

	[RPC]
	public void loadskinRPC(int horse, string url, PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			HumanCustomSkinSettings human = SettingsManager.CustomSkinSettings.Human;
			if (human.SkinsEnabled.Value && (!human.SkinsLocal.Value || base.photonView.isMine))
			{
				base.StartCoroutine(this.loadskinE(horse, url));
			}
		}
	}

	public void markDie()
	{
		this.hasDied = true;
		this.state = HERO_STATE.Die;
	}

	[RPC]
	public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
	{
		if (info != null && info.sender.isMasterClient)
		{
			base.transform.position = new Vector3(posX, posY, posZ);
		}
	}

	[RPC]
	private void net3DMGSMOKE(bool ifON, PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero net3dmgsmoke exploit");
		}
		else if (this.smoke_3dmg != null)
		{
			this.smoke_3dmg.enableEmission = ifON;
		}
	}

	[RPC]
	private void netContinueAnimation(PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero continueanimation exploit");
		}
		foreach (AnimationState item in base.animation)
		{
			if (item.speed == 1f)
			{
				return;
			}
			item.speed = 1f;
		}
		this.playAnimation(this.currentPlayingClipName());
	}

	[RPC]
	private void netCrossFade(string aniName, float time, PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netCrossFade exploit");
			return;
		}
		this.currentAnimation = aniName;
		if (base.animation != null)
		{
			base.animation.CrossFade(aniName, time);
		}
	}

	[RPC]
	public void netDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = null)
	{
		if (base.photonView.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
		{
			if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
			{
				base.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
				return;
			}
			if (!info.sender.isLocal && !info.sender.isMasterClient)
			{
				if (info.sender.customProperties[PhotonPlayerProperty.name] == null || info.sender.customProperties[PhotonPlayerProperty.isTitan] == null)
				{
					FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
				}
				else if (viewID < 0)
				{
					if (titanName == "")
					{
						FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + " (possibly valid).</color>");
					}
					else
					{
						FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
					}
				}
				else if (PhotonView.Find(viewID) == null)
				{
					FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
				}
				else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
				{
					FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
				}
			}
		}
		if (PhotonNetwork.isMasterClient)
		{
			this.onDeathEvent(viewID, killByTitan);
			int ıD = base.photonView.owner.ID;
			if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
			{
				FengGameManagerMKII.heroHash.Remove(ıD);
			}
		}
		if (base.photonView.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			if (this.myBomb != null)
			{
				this.myBomb.DestroySelf();
			}
			if (this.myCannon != null)
			{
				PhotonNetwork.Destroy(this.myCannon);
			}
			if (this.titanForm && this.eren_titan != null)
			{
				this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (this.skillCD != null)
			{
				this.skillCD.transform.localPosition = localPosition;
			}
		}
		if (this.bulletLeft != null)
		{
			this.bulletLeft.GetComponent<Bullet>().removeMe();
		}
		if (this.bulletRight != null)
		{
			this.bulletRight.GetComponent<Bullet>().removeMe();
		}
		this.meatDie.Play();
		if (!this.useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
		{
			this.leftbladetrail.Deactivate();
			this.rightbladetrail.Deactivate();
			this.leftbladetrail2.Deactivate();
			this.rightbladetrail2.Deactivate();
		}
		this.falseAttack();
		this.breakApart2(v, isBite);
		if (base.photonView.isMine)
		{
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: false);
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			FengGameManagerMKII.instance.myRespawnTime = 0f;
		}
		this.hasDied = true;
		Transform transform = base.transform.Find("audio_die");
		if (transform != null)
		{
			transform.parent = null;
			transform.GetComponent<AudioSource>().Play();
		}
		base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
		if (base.photonView.isMine)
		{
			PhotonNetwork.RemoveRPCs(base.photonView);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.dead, true);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths]) + 1);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			object[] parameters = new object[1] { (!(titanName == string.Empty)) ? 1 : 0 };
			FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
			if (viewID != -1)
			{
				PhotonView photonView = PhotonView.Find(viewID);
				if (photonView != null)
				{
					FengGameManagerMKII.instance.sendKillInfo(killByTitan, "[FFC000][" + info.sender.ID + "][FFFFFF]" + RCextensions.returnStringFromObject(photonView.owner.customProperties[PhotonPlayerProperty.name]), t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]));
					hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(photonView.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
					photonView.owner.SetCustomProperties(hashtable);
				}
			}
			else
			{
				FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), "[FFC000][" + info.sender.ID + "][FFFFFF]" + titanName, t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]));
			}
		}
		if (base.photonView.isMine)
		{
			PhotonNetwork.Destroy(base.photonView);
		}
		if (viewID != -1)
		{
			PhotonView photonView2 = PhotonView.Find(viewID);
			if (photonView2 != null && photonView2.isMine && photonView2.GetComponent<TITAN>() != null)
			{
				GameProgressManager.RegisterHumanKill(photonView2.gameObject, this, KillWeapon.Titan);
			}
		}
	}

	[RPC]
	private void netDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = null)
	{
		if (base.photonView.isMine && info != null && IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.BOSS_FIGHT_CT)
		{
			if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
			{
				base.photonView.RPC("backToHumanRPC", PhotonTargets.Others);
				return;
			}
			if (!info.sender.isLocal && !info.sender.isMasterClient)
			{
				if (info.sender.customProperties[PhotonPlayerProperty.name] == null || info.sender.customProperties[PhotonPlayerProperty.isTitan] == null)
				{
					FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
				}
				else if (viewID < 0)
				{
					if (titanName == "")
					{
						FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + " (possibly valid).</color>");
					}
					else if (!SettingsManager.LegacyGameSettings.BombModeEnabled.Value && !SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
					{
						FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
					}
				}
				else if (PhotonView.Find(viewID) == null)
				{
					FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
				}
				else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
				{
					FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
				}
			}
		}
		if (base.photonView.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			if (this.myBomb != null)
			{
				this.myBomb.DestroySelf();
			}
			if (this.myCannon != null)
			{
				PhotonNetwork.Destroy(this.myCannon);
			}
			PhotonNetwork.RemoveRPCs(base.photonView);
			if (this.titanForm && this.eren_titan != null)
			{
				this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (this.skillCD != null)
			{
				this.skillCD.transform.localPosition = localPosition;
			}
		}
		this.meatDie.Play();
		if (this.bulletLeft != null)
		{
			this.bulletLeft.GetComponent<Bullet>().removeMe();
		}
		if (this.bulletRight != null)
		{
			this.bulletRight.GetComponent<Bullet>().removeMe();
		}
		Transform obj = base.transform.Find("audio_die");
		obj.parent = null;
		obj.GetComponent<AudioSource>().Play();
		if (base.photonView.isMine)
		{
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: true);
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			FengGameManagerMKII.instance.myRespawnTime = 0f;
		}
		this.falseAttack();
		this.hasDied = true;
		base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			PhotonNetwork.RemoveRPCs(base.photonView);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.dead, true);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.deaths, (int)PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths] + 1);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			if (viewID != -1)
			{
				PhotonView photonView = PhotonView.Find(viewID);
				if (photonView != null)
				{
					FengGameManagerMKII.instance.sendKillInfo(t1: true, "[FFC000][" + info.sender.ID + "][FFFFFF]" + RCextensions.returnStringFromObject(photonView.owner.customProperties[PhotonPlayerProperty.name]), t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]));
					hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(photonView.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
					photonView.owner.SetCustomProperties(hashtable);
				}
			}
			else
			{
				FengGameManagerMKII.instance.sendKillInfo(t1: true, "[FFC000][" + info.sender.ID + "][FFFFFF]" + titanName, t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]));
			}
			object[] parameters = new object[1] { (!(titanName == string.Empty)) ? 1 : 0 };
			FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
		}
		GameObject gameObject = ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || !base.photonView.isMine) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"))) : PhotonNetwork.Instantiate("hitMeat2", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0));
		gameObject.transform.position = base.transform.position;
		if (base.photonView.isMine)
		{
			PhotonNetwork.Destroy(base.photonView);
		}
		if (PhotonNetwork.isMasterClient)
		{
			this.onDeathEvent(viewID, isTitan: true);
			int ıD = base.photonView.owner.ID;
			if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
			{
				FengGameManagerMKII.heroHash.Remove(ıD);
			}
		}
		if (viewID != -1)
		{
			PhotonView photonView2 = PhotonView.Find(viewID);
			if (photonView2 != null && photonView2.isMine && photonView2.GetComponent<TITAN>() != null)
			{
				GameProgressManager.RegisterHumanKill(photonView2.gameObject, this, KillWeapon.Titan);
			}
		}
	}

	public void netDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
	{
		if (base.photonView.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			if (this.titanForm && this.eren_titan != null)
			{
				this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
			}
			if (this.myBomb != null)
			{
				this.myBomb.DestroySelf();
			}
			if (this.myCannon != null)
			{
				PhotonNetwork.Destroy(this.myCannon);
			}
			if (this.skillCD != null)
			{
				this.skillCD.transform.localPosition = localPosition;
			}
		}
		if (this.bulletLeft != null)
		{
			this.bulletLeft.GetComponent<Bullet>().removeMe();
		}
		if (this.bulletRight != null)
		{
			this.bulletRight.GetComponent<Bullet>().removeMe();
		}
		this.meatDie.Play();
		if (!this.useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
		{
			this.leftbladetrail.Deactivate();
			this.rightbladetrail.Deactivate();
			this.leftbladetrail2.Deactivate();
			this.rightbladetrail2.Deactivate();
		}
		this.falseAttack();
		this.breakApart2(v, isBite);
		if (base.photonView.isMine)
		{
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(val: false);
			this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
			FengGameManagerMKII.instance.myRespawnTime = 0f;
		}
		this.hasDied = true;
		Transform obj = base.transform.Find("audio_die");
		obj.parent = null;
		obj.GetComponent<AudioSource>().Play();
		base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
		if (base.photonView.isMine)
		{
			PhotonNetwork.RemoveRPCs(base.photonView);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.dead, true);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.deaths]) + 1);
			PhotonNetwork.player.SetCustomProperties(hashtable);
			object[] parameters = new object[1] { (!(titanName == string.Empty)) ? 1 : 0 };
			FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
			if (viewID != -1)
			{
				PhotonView photonView = PhotonView.Find(viewID);
				if (photonView != null)
				{
					FengGameManagerMKII.instance.sendKillInfo(killByTitan, RCextensions.returnStringFromObject(photonView.owner.customProperties[PhotonPlayerProperty.name]), t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]));
					hashtable = new ExitGames.Client.Photon.Hashtable();
					hashtable.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(photonView.owner.customProperties[PhotonPlayerProperty.kills]) + 1);
					photonView.owner.SetCustomProperties(hashtable);
				}
			}
			else
			{
				FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), titanName, t2: false, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]));
			}
		}
		if (base.photonView.isMine)
		{
			PhotonNetwork.Destroy(base.photonView);
		}
		if (PhotonNetwork.isMasterClient)
		{
			this.onDeathEvent(viewID, killByTitan);
			int ıD = base.photonView.owner.ID;
			if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
			{
				FengGameManagerMKII.heroHash.Remove(ıD);
			}
		}
	}

	[RPC]
	private void netGrabbed(int id, bool leftHand, PhotonMessageInfo info)
	{
		if (info != null && !info.sender.isMasterClient && (RCextensions.returnIntFromObject(info.sender.customProperties[PhotonPlayerProperty.isTitan]) != 2 || RCextensions.returnBoolFromObject(info.sender.customProperties[PhotonPlayerProperty.dead])))
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netGrabbed exploit");
			return;
		}
		this.titanWhoGrabMeID = id;
		this.grabbed(PhotonView.Find(id).gameObject, leftHand);
	}

	[RPC]
	private void netlaughAttack(PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netlaughattack exploit");
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (Vector3.Distance(gameObject.transform.position, base.transform.position) < 50f && Vector3.Angle(gameObject.transform.forward, base.transform.position - gameObject.transform.position) < 90f && gameObject.GetComponent<TITAN>() != null)
			{
				gameObject.GetComponent<TITAN>().beLaughAttacked();
			}
		}
	}

	[RPC]
	private void netPauseAnimation(PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netPauseAniamtion");
			return;
		}
		foreach (AnimationState item in base.animation)
		{
			item.speed = 0f;
		}
	}

	[RPC]
	private void netPlayAnimation(string aniName, PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner && aniName != "grabbed")
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netPlayAnimation exploit");
			return;
		}
		this.currentAnimation = aniName;
		if (base.animation != null)
		{
			base.animation.Play(aniName);
		}
	}

	[RPC]
	private void netPlayAnimationAt(string aniName, float normalizedTime, PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netPlayAnimationAt exploit");
			return;
		}
		this.currentAnimation = aniName;
		if (base.animation != null)
		{
			base.animation.Play(aniName);
			base.animation[aniName].normalizedTime = normalizedTime;
		}
	}

	[RPC]
	private void netSetIsGrabbedFalse(PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netSetIsGrabbedFalse");
		}
		else
		{
			this.state = HERO_STATE.Idle;
		}
	}

	[RPC]
	private void netTauntAttack(float tauntTime, float distance, PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero netTauntAttack");
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		foreach (GameObject gameObject in array)
		{
			if (Vector3.Distance(gameObject.transform.position, base.transform.position) < distance && gameObject.GetComponent<TITAN>() != null)
			{
				gameObject.GetComponent<TITAN>().beTauntedBy(base.gameObject, tauntTime);
			}
		}
	}

	[RPC]
	public void netUngrabbed()
	{
		this.ungrabbed();
		this.netPlayAnimation(this.standAnimation, null);
		this.falseAttack();
	}

	public void onDeathEvent(int viewID, bool isTitan)
	{
		if (isTitan)
		{
			if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByTitan"))
			{
				RCEvent obj = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByTitan"];
				string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByTitan"];
				if (FengGameManagerMKII.playerVariables.ContainsKey(array[0]))
				{
					FengGameManagerMKII.playerVariables[array[0]] = base.photonView.owner;
				}
				else
				{
					FengGameManagerMKII.playerVariables.Add(array[0], base.photonView.owner);
				}
				if (FengGameManagerMKII.titanVariables.ContainsKey(array[1]))
				{
					FengGameManagerMKII.titanVariables[array[1]] = PhotonView.Find(viewID).gameObject.GetComponent<TITAN>();
				}
				else
				{
					FengGameManagerMKII.titanVariables.Add(array[1], PhotonView.Find(viewID).gameObject.GetComponent<TITAN>());
				}
				obj.checkEvent();
			}
		}
		else if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByPlayer"))
		{
			RCEvent obj2 = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByPlayer"];
			string[] array = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByPlayer"];
			if (FengGameManagerMKII.playerVariables.ContainsKey(array[0]))
			{
				FengGameManagerMKII.playerVariables[array[0]] = base.photonView.owner;
			}
			else
			{
				FengGameManagerMKII.playerVariables.Add(array[0], base.photonView.owner);
			}
			if (FengGameManagerMKII.playerVariables.ContainsKey(array[1]))
			{
				FengGameManagerMKII.playerVariables[array[1]] = PhotonView.Find(viewID).owner;
			}
			else
			{
				FengGameManagerMKII.playerVariables.Add(array[1], PhotonView.Find(viewID).owner);
			}
			obj2.checkEvent();
		}
	}

	private void OnDestroy()
	{
		if (this.myNetWorkName != null)
		{
			UnityEngine.Object.Destroy(this.myNetWorkName);
		}
		if (this.gunDummy != null)
		{
			UnityEngine.Object.Destroy(this.gunDummy);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			this.releaseIfIHookSb();
		}
		if (GameObject.Find("MultiplayerManager") != null)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeHero(this);
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			Vector3 localPosition = Vector3.up * 5000f;
			this.cross1.transform.localPosition = localPosition;
			this.cross2.transform.localPosition = localPosition;
			this.crossL1.transform.localPosition = localPosition;
			this.crossL2.transform.localPosition = localPosition;
			this.crossR1.transform.localPosition = localPosition;
			this.crossR2.transform.localPosition = localPosition;
			this.LabelDistance.transform.localPosition = localPosition;
		}
		if (this.setup.part_cape != null)
		{
			ClothFactory.DisposeObject(this.setup.part_cape);
		}
		if (this.setup.part_hair_1 != null)
		{
			ClothFactory.DisposeObject(this.setup.part_hair_1);
		}
		if (this.setup.part_hair_2 != null)
		{
			ClothFactory.DisposeObject(this.setup.part_hair_2);
		}
		if (this.IsMine())
		{
			GameMenu.ToggleEmoteWheel(enable: false);
		}
	}

	public void pauseAnimation()
	{
		if (this._animationStopped)
		{
			return;
		}
		this._animationStopped = true;
		foreach (AnimationState item in base.animation)
		{
			item.speed = 0f;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
		{
			base.photonView.RPC("netPauseAnimation", PhotonTargets.Others);
		}
	}

	public void playAnimation(string aniName)
	{
		this.currentAnimation = aniName;
		base.animation.Play(aniName);
		if (PhotonNetwork.connected && base.photonView.isMine)
		{
			object[] parameters = new object[1] { aniName };
			base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
		}
	}

	private void playAnimationAt(string aniName, float normalizedTime)
	{
		this.currentAnimation = aniName;
		base.animation.Play(aniName);
		base.animation[aniName].normalizedTime = normalizedTime;
		if (PhotonNetwork.connected && base.photonView.isMine)
		{
			object[] parameters = new object[2] { aniName, normalizedTime };
			base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
		}
	}

	private void releaseIfIHookSb()
	{
		if (this.hookSomeOne && this.hookTarget != null)
		{
			this.hookTarget.GetPhotonView().RPC("badGuyReleaseMe", this.hookTarget.GetPhotonView().owner);
			this.hookTarget = null;
			this.hookSomeOne = false;
		}
	}

	public void resetAnimationSpeed()
	{
		foreach (AnimationState item in base.animation)
		{
			item.speed = 1f;
		}
		this.customAnimationSpeed();
	}

	[RPC]
	public void ReturnFromCannon(PhotonMessageInfo info)
	{
		if (info != null && info.sender == base.photonView.owner)
		{
			this.isCannon = false;
			base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
		}
	}

	private void rightArmAimTo(Vector3 target)
	{
		float num = target.x - this.upperarmR.transform.position.x;
		float y = target.y - this.upperarmR.transform.position.y;
		float num2 = target.z - this.upperarmR.transform.position.z;
		float x = Mathf.Sqrt(num * num + num2 * num2);
		this.handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		this.forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
		this.upperarmR.rotation = Quaternion.Euler(180f, 90f + Mathf.Atan2(num, num2) * 57.29578f, Mathf.Atan2(y, x) * 57.29578f);
	}

	[RPC]
	private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
	{
		this.hookBySomeOne = true;
		this.badGuy = PhotonView.Find(hooker).gameObject;
		if (Vector3.Distance(hookPosition, base.transform.position) < 15f)
		{
			this.launchForce = PhotonView.Find(hooker).gameObject.transform.position - base.transform.position;
			base.rigidbody.AddForce(-base.rigidbody.velocity * 0.9f, ForceMode.VelocityChange);
			float num = Mathf.Pow(this.launchForce.magnitude, 0.1f);
			if (this.grounded)
			{
				base.rigidbody.AddForce(Vector3.up * Mathf.Min(this.launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
			}
			base.rigidbody.AddForce(this.launchForce * num * 0.1f, ForceMode.Impulse);
			if (this.state != HERO_STATE.Grab)
			{
				this.dashTime = 1f;
				this.crossFade("dash", 0.05f);
				base.animation["dash"].time = 0.1f;
				this.state = HERO_STATE.AirDodge;
				this.falseAttack();
				this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
				Quaternion rotation = Quaternion.Euler(0f, this.facingDirection, 0f);
				base.gameObject.transform.rotation = rotation;
				base.rigidbody.rotation = rotation;
				this.targetRotation = rotation;
			}
		}
		else
		{
			this.hookBySomeOne = false;
			this.badGuy = null;
			PhotonView.Find(hooker).RPC("hookFail", PhotonView.Find(hooker).owner);
		}
	}

	private void setHookedPplDirection()
	{
		this.almostSingleHook = false;
		float num = this.facingDirection;
		if (this.isRightHandHooked && this.isLeftHandHooked)
		{
			if (this.bulletLeft != null && this.bulletRight != null)
			{
				Vector3 normal = this.bulletLeft.transform.position - this.bulletRight.transform.position;
				if (normal.sqrMagnitude < 4f)
				{
					Vector3 vector = (this.bulletLeft.transform.position + this.bulletRight.transform.position) * 0.5f - base.transform.position;
					this.facingDirection = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
					if (this.useGun && this.state != HERO_STATE.Attack)
					{
						float current = (0f - Mathf.Atan2(base.rigidbody.velocity.z, base.rigidbody.velocity.x)) * 57.29578f;
						float target = (0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f;
						float num2 = 0f - Mathf.DeltaAngle(current, target);
						this.facingDirection += num2;
					}
					this.almostSingleHook = true;
				}
				else
				{
					Vector3 to = base.transform.position - this.bulletLeft.transform.position;
					Vector3 to2 = base.transform.position - this.bulletRight.transform.position;
					Vector3 vector2 = (this.bulletLeft.transform.position + this.bulletRight.transform.position) * 0.5f;
					Vector3 from = base.transform.position - vector2;
					if (Vector3.Angle(from, to) < 30f && Vector3.Angle(from, to2) < 30f)
					{
						this.almostSingleHook = true;
						Vector3 vector3 = vector2 - base.transform.position;
						this.facingDirection = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
					}
					else
					{
						this.almostSingleHook = false;
						Vector3 tangent = base.transform.forward;
						Vector3.OrthoNormalize(ref normal, ref tangent);
						this.facingDirection = Mathf.Atan2(tangent.x, tangent.z) * 57.29578f;
						if (Mathf.DeltaAngle(Mathf.Atan2(to.x, to.z) * 57.29578f, this.facingDirection) > 0f)
						{
							this.facingDirection += 180f;
						}
					}
				}
			}
		}
		else
		{
			this.almostSingleHook = true;
			Vector3 zero = Vector3.zero;
			if (this.isRightHandHooked && this.bulletRight != null)
			{
				zero = this.bulletRight.transform.position - base.transform.position;
			}
			else
			{
				if (!this.isLeftHandHooked || !(this.bulletLeft != null))
				{
					return;
				}
				zero = this.bulletLeft.transform.position - base.transform.position;
			}
			this.facingDirection = Mathf.Atan2(zero.x, zero.z) * 57.29578f;
			if (this.state != HERO_STATE.Attack)
			{
				float current2 = (0f - Mathf.Atan2(base.rigidbody.velocity.z, base.rigidbody.velocity.x)) * 57.29578f;
				float target2 = (0f - Mathf.Atan2(zero.z, zero.x)) * 57.29578f;
				float num3 = 0f - Mathf.DeltaAngle(current2, target2);
				if (this.useGun)
				{
					this.facingDirection += num3;
				}
				else
				{
					float num4 = 0f;
					num4 = (((!this.isLeftHandHooked || !(num3 < 0f)) && (!this.isRightHandHooked || !(num3 > 0f))) ? 0.1f : (-0.1f));
					this.facingDirection += num3 * num4;
				}
			}
		}
		if (this.IsFiringThunderSpear())
		{
			this.facingDirection = num;
		}
	}

	[RPC]
	public void SetMyCannon(int viewID, PhotonMessageInfo info)
	{
		if (info.sender != base.photonView.owner || viewID < 0)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero setcannon exploit");
		}
		else if (PhotonView.Find(viewID).owner != info.sender)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero setcannon exploit");
		}
		else
		{
			if (info.sender != base.photonView.owner)
			{
				return;
			}
			PhotonView photonView = PhotonView.Find(viewID);
			if (photonView != null)
			{
				this.myCannon = photonView.gameObject;
				if (this.myCannon != null)
				{
					this.myCannonBase = this.myCannon.transform;
					this.myCannonPlayer = this.myCannonBase.Find("PlayerPoint");
					this.isCannon = true;
				}
			}
		}
	}

	[RPC]
	public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
	{
		if (info != null && base.photonView.owner == info.sender)
		{
			this.CameraMultiplier = offset;
			base.GetComponent<SmoothSyncMovement>().PhotonCamera = true;
			this.isPhotonCamera = true;
		}
	}

	[RPC]
	private void setMyTeam(int val)
	{
		this.myTeam = val;
		if (this.checkBoxLeft != null)
		{
			this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
		}
		if (this.checkBoxRight != null)
		{
			this.checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient)
		{
			return;
		}
		if (SettingsManager.LegacyGameSettings.FriendlyMode.Value)
		{
			if (val != 1)
			{
				object[] parameters = new object[1] { 1 };
				base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
			}
		}
		else if (SettingsManager.LegacyGameSettings.BladePVP.Value == 1)
		{
			int num = 0;
			if (base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam] != null)
			{
				num = RCextensions.returnIntFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam]);
			}
			if (val != num)
			{
				object[] parameters = new object[1] { num };
				base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
			}
		}
		else if (SettingsManager.LegacyGameSettings.BladePVP.Value == 2 && val != base.photonView.owner.ID)
		{
			object[] parameters = new object[1] { base.photonView.owner.ID };
			base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
		}
	}

	public void setSkillHUDPosition2()
	{
		this.skillCD = GameObject.Find("skill_cd_" + this.skillIDHUD);
		if (this.skillCD != null)
		{
			this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
		}
		if (this.useGun && !SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
		{
			this.skillCD.transform.localPosition = Vector3.up * 5000f;
		}
	}

	public void setStat2()
	{
		this.skillCDLast = 1.5f;
		this.skillId = this.setup.myCostume.stat.skillId;
		if (this.skillId == "levi")
		{
			this.skillCDLast = 3.5f;
		}
		this.customAnimationSpeed();
		if (this.skillId == "armin")
		{
			this.skillCDLast = 5f;
		}
		if (this.skillId == "marco")
		{
			this.skillCDLast = 10f;
		}
		if (this.skillId == "jean")
		{
			this.skillCDLast = 0.001f;
		}
		if (this.skillId == "eren")
		{
			this.skillCDLast = 120f;
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && (LevelInfo.getInfo(FengGameManagerMKII.level).teamTitan || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.RACING || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.PVP_CAPTURE || LevelInfo.getInfo(FengGameManagerMKII.level).type == GAMEMODE.TROST))
			{
				this.skillId = "petra";
				this.skillCDLast = 1f;
			}
		}
		if (this.skillId == "sasha")
		{
			this.skillCDLast = 20f;
		}
		if (this.skillId == "petra")
		{
			this.skillCDLast = 3.5f;
		}
		this.bombInit();
		this.speed = (float)this.setup.myCostume.stat.SPD / 10f;
		this.totalGas = (this.currentGas = this.setup.myCostume.stat.GAS);
		this.totalBladeSta = (this.currentBladeSta = this.setup.myCostume.stat.BLA);
		this.baseRigidBody.mass = 0.5f - (float)(this.setup.myCostume.stat.ACL - 100) * 0.001f;
		GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (float)(-Screen.height) * 0.5f + 5f, 0f);
		this.skillCD = GameObject.Find("skill_cd_" + this.skillIDHUD);
		this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
		GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
		{
			GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = false;
			GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = false;
		}
		if (this.setup.myCostume.uniform_type == UNIFORM_TYPE.CasualAHSS)
		{
			this.standAnimation = "AHSS_stand_gun";
			this.useGun = true;
			this.gunDummy = new GameObject();
			this.gunDummy.name = "gunDummy";
			this.gunDummy.transform.position = this.baseTransform.position;
			this.gunDummy.transform.rotation = this.baseTransform.rotation;
			this.myGroup = GROUP.A;
			this.setTeam2(2);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
			{
				GameObject.Find("bladeCL").GetComponent<UISprite>().enabled = false;
				GameObject.Find("bladeCR").GetComponent<UISprite>().enabled = false;
				GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
				GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
				GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
				GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
				GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
				GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = true;
				GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = true;
				if (this.skillId != "bomb")
				{
					this.skillCD.transform.localPosition = Vector3.up * 5000f;
				}
			}
		}
		else if (this.setup.myCostume.sex == SEX.FEMALE)
		{
			this.standAnimation = "stand";
			this.setTeam2(1);
		}
		else
		{
			this.standAnimation = "stand_levi";
			this.setTeam2(1);
		}
	}

	public void setTeam2(int team)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			object[] parameters = new object[1] { team };
			base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable.Add(PhotonPlayerProperty.team, team);
			PhotonNetwork.player.SetCustomProperties(hashtable);
		}
		else
		{
			this.setMyTeam(team);
		}
	}

	public void shootFlare(int type)
	{
		bool flag = false;
		if (type == 1 && this.flare1CD == 0f)
		{
			this.flare1CD = this.flareTotalCD;
			flag = true;
		}
		if (type == 2 && this.flare2CD == 0f)
		{
			this.flare2CD = this.flareTotalCD;
			flag = true;
		}
		if (type == 3 && this.flare3CD == 0f)
		{
			this.flare3CD = this.flareTotalCD;
			flag = true;
		}
		if (flag)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/flareBullet" + type), base.transform.position, base.transform.rotation);
				obj.GetComponent<FlareMovement>().dontShowHint();
				UnityEngine.Object.Destroy(obj, 25f);
			}
			else
			{
				PhotonNetwork.Instantiate("FX/flareBullet" + type, base.transform.position, base.transform.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
			}
		}
	}

	private void showAimUI2()
	{
		if (CursorManager.State == CursorState.Pointer || GameMenu.HideCrosshair)
		{
			Vector3 localPosition = Vector3.up * 10000f;
			this.crossL1.transform.localPosition = localPosition;
			this.crossL2.transform.localPosition = localPosition;
			this.crossR1.transform.localPosition = localPosition;
			this.crossR2.transform.localPosition = localPosition;
			return;
		}
		this.checkTitan();
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask layerMask = 1 << PhysicsLayer.Ground;
		LayerMask layerMask2 = (int)(LayerMask)(1 << PhysicsLayer.EnemyBox) | (int)layerMask;
		if (!Physics.Raycast(ray, out var hitInfo, 10000000f, layerMask2.value))
		{
			return;
		}
		float magnitude = (hitInfo.point - this.baseTransform.position).magnitude;
		string text = string.Empty;
		if (SettingsManager.UISettings.ShowCrosshairDistance.Value)
		{
			text = ((magnitude <= 1000f) ? ((int)magnitude).ToString() : "???");
		}
		if (SettingsManager.UISettings.Speedometer.Value == 1)
		{
			if (text != string.Empty)
			{
				text += "\n";
			}
			text = text + this.currentSpeed.ToString("F1") + " u/s";
		}
		else if (SettingsManager.UISettings.Speedometer.Value == 2)
		{
			if (text != string.Empty)
			{
				text += "\n";
			}
			text = text + (this.currentSpeed / 100f).ToString("F1") + "K";
		}
		CursorManager.SetCrosshairText(text);
		if (magnitude > 120f)
		{
			CursorManager.SetCrosshairColor(white: false);
		}
		else
		{
			CursorManager.SetCrosshairColor(white: true);
		}
		if (SettingsManager.UISettings.ShowCrosshairArrows.Value)
		{
			Vector3 vector = new Vector3(0f, 0.4f, 0f);
			vector -= this.baseTransform.right * 0.3f;
			Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
			vector2 += this.baseTransform.right * 0.3f;
			float num = ((hitInfo.distance <= 50f) ? (hitInfo.distance * 0.05f) : (hitInfo.distance * 0.3f));
			Vector3 vector3 = hitInfo.point - this.baseTransform.right * num - (this.baseTransform.position + vector);
			Vector3 vector4 = hitInfo.point + this.baseTransform.right * num - (this.baseTransform.position + vector2);
			vector3.Normalize();
			vector4.Normalize();
			vector3 *= 1000000f;
			vector4 *= 1000000f;
			if (Physics.Linecast(this.baseTransform.position + vector, this.baseTransform.position + vector + vector3, out var hitInfo2, layerMask2.value))
			{
				Transform transform = this.crossL1.transform;
				transform.localPosition = this.currentCamera.WorldToScreenPoint(hitInfo2.point);
				transform.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
				transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(transform.localPosition.y - (Input.mousePosition.y - (float)Screen.height * 0.5f), transform.localPosition.x - (Input.mousePosition.x - (float)Screen.width * 0.5f)) * 57.29578f + 180f);
				Transform transform2 = this.crossL2.transform;
				transform2.localPosition = transform.localPosition;
				transform2.localRotation = transform.localRotation;
				if (hitInfo2.distance > 120f)
				{
					transform.localPosition += Vector3.up * 10000f;
				}
				else
				{
					transform2.localPosition += Vector3.up * 10000f;
				}
			}
			if (Physics.Linecast(this.baseTransform.position + vector2, this.baseTransform.position + vector2 + vector4, out hitInfo2, layerMask2.value))
			{
				Transform transform3 = this.crossR1.transform;
				transform3.localPosition = this.currentCamera.WorldToScreenPoint(hitInfo2.point);
				transform3.localPosition -= new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f);
				transform3.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(transform3.localPosition.y - (Input.mousePosition.y - (float)Screen.height * 0.5f), transform3.localPosition.x - (Input.mousePosition.x - (float)Screen.width * 0.5f)) * 57.29578f);
				Transform transform4 = this.crossR2.transform;
				transform4.localPosition = transform3.localPosition;
				transform4.localRotation = transform3.localRotation;
				if (hitInfo2.distance > 120f)
				{
					transform3.localPosition += Vector3.up * 10000f;
				}
				else
				{
					transform4.localPosition += Vector3.up * 10000f;
				}
			}
		}
		else
		{
			Vector3 localPosition = Vector3.up * 10000f;
			this.crossL1.transform.localPosition = localPosition;
			this.crossL2.transform.localPosition = localPosition;
			this.crossR1.transform.localPosition = localPosition;
			this.crossR2.transform.localPosition = localPosition;
		}
	}

	private void showFlareCD2()
	{
		if (this.cachedSprites["UIflare1"] != null)
		{
			this.cachedSprites["UIflare1"].fillAmount = (this.flareTotalCD - this.flare1CD) / this.flareTotalCD;
			this.cachedSprites["UIflare2"].fillAmount = (this.flareTotalCD - this.flare2CD) / this.flareTotalCD;
			this.cachedSprites["UIflare3"].fillAmount = (this.flareTotalCD - this.flare3CD) / this.flareTotalCD;
		}
	}

	private void showGas()
	{
		float num = this.currentGas / this.totalGas;
		float num2 = this.currentBladeSta / this.totalBladeSta;
		GameObject.Find("gasL1").GetComponent<UISprite>().fillAmount = this.currentGas / this.totalGas;
		GameObject.Find("gasR1").GetComponent<UISprite>().fillAmount = this.currentGas / this.totalGas;
		if (!this.useGun)
		{
			GameObject.Find("bladeCL").GetComponent<UISprite>().fillAmount = this.currentBladeSta / this.totalBladeSta;
			GameObject.Find("bladeCR").GetComponent<UISprite>().fillAmount = this.currentBladeSta / this.totalBladeSta;
			if (num <= 0f)
			{
				GameObject.Find("gasL").GetComponent<UISprite>().color = Color.red;
				GameObject.Find("gasR").GetComponent<UISprite>().color = Color.red;
			}
			else if (num < 0.3f)
			{
				GameObject.Find("gasL").GetComponent<UISprite>().color = Color.yellow;
				GameObject.Find("gasR").GetComponent<UISprite>().color = Color.yellow;
			}
			else
			{
				GameObject.Find("gasL").GetComponent<UISprite>().color = Color.white;
				GameObject.Find("gasR").GetComponent<UISprite>().color = Color.white;
			}
			if (num2 <= 0f)
			{
				GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.red;
				GameObject.Find("blader1").GetComponent<UISprite>().color = Color.red;
			}
			else if (num2 < 0.3f)
			{
				GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.yellow;
				GameObject.Find("blader1").GetComponent<UISprite>().color = Color.yellow;
			}
			else
			{
				GameObject.Find("bladel1").GetComponent<UISprite>().color = Color.white;
				GameObject.Find("blader1").GetComponent<UISprite>().color = Color.white;
			}
			if (this.currentBladeNum <= 4)
			{
				GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
			}
			else
			{
				GameObject.Find("bladel5").GetComponent<UISprite>().enabled = true;
				GameObject.Find("blader5").GetComponent<UISprite>().enabled = true;
			}
			if (this.currentBladeNum <= 3)
			{
				GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
			}
			else
			{
				GameObject.Find("bladel4").GetComponent<UISprite>().enabled = true;
				GameObject.Find("blader4").GetComponent<UISprite>().enabled = true;
			}
			if (this.currentBladeNum <= 2)
			{
				GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
			}
			else
			{
				GameObject.Find("bladel3").GetComponent<UISprite>().enabled = true;
				GameObject.Find("blader3").GetComponent<UISprite>().enabled = true;
			}
			if (this.currentBladeNum <= 1)
			{
				GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
			}
			else
			{
				GameObject.Find("bladel2").GetComponent<UISprite>().enabled = true;
				GameObject.Find("blader2").GetComponent<UISprite>().enabled = true;
			}
			if (this.currentBladeNum <= 0)
			{
				GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
				GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
			}
			else
			{
				GameObject.Find("bladel1").GetComponent<UISprite>().enabled = true;
				GameObject.Find("blader1").GetComponent<UISprite>().enabled = true;
			}
		}
		else
		{
			if (this.leftGunHasBullet)
			{
				GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
			}
			else
			{
				GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
			}
			if (this.rightGunHasBullet)
			{
				GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
			}
			else
			{
				GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
			}
		}
	}

	private void showGas2()
	{
		float num = this.currentGas / this.totalGas;
		float num2 = this.currentBladeSta / this.totalBladeSta;
		this.cachedSprites["gasL1"].fillAmount = this.currentGas / this.totalGas;
		this.cachedSprites["gasR1"].fillAmount = this.currentGas / this.totalGas;
		if (!this.useGun)
		{
			this.cachedSprites["bladeCL"].fillAmount = this.currentBladeSta / this.totalBladeSta;
			this.cachedSprites["bladeCR"].fillAmount = this.currentBladeSta / this.totalBladeSta;
			if (num <= 0f)
			{
				this.cachedSprites["gasL"].color = Color.red;
				this.cachedSprites["gasR"].color = Color.red;
			}
			else if (num < 0.3f)
			{
				this.cachedSprites["gasL"].color = Color.yellow;
				this.cachedSprites["gasR"].color = Color.yellow;
			}
			else
			{
				this.cachedSprites["gasL"].color = Color.white;
				this.cachedSprites["gasR"].color = Color.white;
			}
			if (num2 <= 0f)
			{
				this.cachedSprites["bladel1"].color = Color.red;
				this.cachedSprites["blader1"].color = Color.red;
			}
			else if (num2 < 0.3f)
			{
				this.cachedSprites["bladel1"].color = Color.yellow;
				this.cachedSprites["blader1"].color = Color.yellow;
			}
			else
			{
				this.cachedSprites["bladel1"].color = Color.white;
				this.cachedSprites["blader1"].color = Color.white;
			}
			if (this.currentBladeNum <= 4)
			{
				this.cachedSprites["bladel5"].enabled = false;
				this.cachedSprites["blader5"].enabled = false;
			}
			else
			{
				this.cachedSprites["bladel5"].enabled = true;
				this.cachedSprites["blader5"].enabled = true;
			}
			if (this.currentBladeNum <= 3)
			{
				this.cachedSprites["bladel4"].enabled = false;
				this.cachedSprites["blader4"].enabled = false;
			}
			else
			{
				this.cachedSprites["bladel4"].enabled = true;
				this.cachedSprites["blader4"].enabled = true;
			}
			if (this.currentBladeNum <= 2)
			{
				this.cachedSprites["bladel3"].enabled = false;
				this.cachedSprites["blader3"].enabled = false;
			}
			else
			{
				this.cachedSprites["bladel3"].enabled = true;
				this.cachedSprites["blader3"].enabled = true;
			}
			if (this.currentBladeNum <= 1)
			{
				this.cachedSprites["bladel2"].enabled = false;
				this.cachedSprites["blader2"].enabled = false;
			}
			else
			{
				this.cachedSprites["bladel2"].enabled = true;
				this.cachedSprites["blader2"].enabled = true;
			}
			if (this.currentBladeNum <= 0)
			{
				this.cachedSprites["bladel1"].enabled = false;
				this.cachedSprites["blader1"].enabled = false;
			}
			else
			{
				this.cachedSprites["bladel1"].enabled = true;
				this.cachedSprites["blader1"].enabled = true;
			}
		}
		else
		{
			if (this.leftGunHasBullet)
			{
				this.cachedSprites["bulletL"].enabled = true;
			}
			else
			{
				this.cachedSprites["bulletL"].enabled = false;
			}
			if (this.rightGunHasBullet)
			{
				this.cachedSprites["bulletR"].enabled = true;
			}
			else
			{
				this.cachedSprites["bulletR"].enabled = false;
			}
		}
	}

	[RPC]
	private void showHitDamage(PhotonMessageInfo info)
	{
		if (info != null)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero showHitDamage exploit");
		}
	}

	private void showSkillCD()
	{
		if (this.skillCD != null)
		{
			this.skillCD.GetComponent<UISprite>().fillAmount = (this.skillCDLast - this.skillCDDuration) / this.skillCDLast;
		}
	}

	[RPC]
	public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
	{
		if (info.sender.isMasterClient && base.photonView.isMine && this.myCannon == null)
		{
			if (this.myHorse != null && this.isMounted)
			{
				this.getOffHorse();
			}
			this.idle();
			if (this.bulletLeft != null)
			{
				this.bulletLeft.GetComponent<Bullet>().removeMe();
			}
			if (this.bulletRight != null)
			{
				this.bulletRight.GetComponent<Bullet>().removeMe();
			}
			if (this.smoke_3dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
			{
				object[] parameters = new object[1] { false };
				base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
			}
			this.smoke_3dmg.enableEmission = false;
			base.rigidbody.velocity = Vector3.zero;
			string[] array = settings.Split(',');
			if (array.Length > 15)
			{
				this.myCannon = PhotonNetwork.Instantiate("RCAsset/" + array[1], new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
			}
			else
			{
				this.myCannon = PhotonNetwork.Instantiate("RCAsset/" + array[1], new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0);
			}
			this.myCannonBase = this.myCannon.transform;
			this.myCannonPlayer = this.myCannon.transform.Find("PlayerPoint");
			this.isCannon = true;
			this.myCannon.GetComponent<Cannon>().myHero = this;
			this.myCannonRegion = null;
			Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(this.myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject);
			Camera.main.fieldOfView = 55f;
			base.photonView.RPC("SetMyCannon", PhotonTargets.OthersBuffered, this.myCannon.GetPhotonView().viewID);
			this.skillCDLastCannon = this.skillCDLast;
			this.skillCDLast = 3.5f;
			this.skillCDDuration = 3.5f;
		}
	}

	private void Start()
	{
		FengGameManagerMKII.instance.addHero(this);
		if ((LevelInfo.getInfo(FengGameManagerMKII.level).horse || SettingsManager.LegacyGameSettings.AllowHorses.Value) && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			this.myHorse = PhotonNetwork.Instantiate("horse", this.baseTransform.position + Vector3.up * 5f, this.baseTransform.rotation, 0);
			this.myHorse.GetComponent<Horse>().myHero = base.gameObject;
			this.myHorse.GetComponent<TITAN_CONTROLLER>().isHorse = true;
		}
		this.sparks = this.baseTransform.Find("slideSparks").GetComponent<ParticleSystem>();
		this.smoke_3dmg = this.baseTransform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
		this.baseTransform.localScale = new Vector3(this.myScale, this.myScale, this.myScale);
		this.facingDirection = this.baseTransform.rotation.eulerAngles.y;
		this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
		this.smoke_3dmg.enableEmission = false;
		this.sparks.enableEmission = false;
		this.speedFXPS = this.speedFX1.GetComponent<ParticleSystem>();
		this.speedFXPS.enableEmission = false;
		if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER)
		{
			if (Minimap.instance != null)
			{
				Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.green, trackOrientation: false, depthAboveAll: true);
			}
		}
		else
		{
			if (PhotonNetwork.isMasterClient)
			{
				int ıD = base.photonView.owner.ID;
				if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
				{
					FengGameManagerMKII.heroHash[ıD] = this;
				}
				else
				{
					FengGameManagerMKII.heroHash.Add(ıD, this);
				}
			}
			GameObject gameObject = GameObject.Find("UI_IN_GAME");
			this.myNetWorkName = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
			this.myNetWorkName.name = "LabelNameOverHead";
			this.myNetWorkName.transform.parent = gameObject.GetComponent<UIReferArray>().panels[0].transform;
			this.myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
			this.myNetWorkName.GetComponent<UILabel>().text = string.Empty;
			if (base.photonView.isMine)
			{
				if (Minimap.instance != null)
				{
					Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.green, trackOrientation: false, depthAboveAll: true);
				}
				base.GetComponent<SmoothSyncMovement>().PhotonCamera = true;
				base.photonView.RPC("SetMyPhotonCamera", PhotonTargets.OthersBuffered, SettingsManager.GeneralSettings.CameraDistance.Value + 0.3f);
			}
			else
			{
				bool flag = false;
				if (base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam] != null)
				{
					switch (RCextensions.returnIntFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.RCteam]))
					{
					case 1:
						flag = true;
						if (Minimap.instance != null)
						{
							Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.cyan, trackOrientation: false, depthAboveAll: true);
						}
						break;
					case 2:
						flag = true;
						if (Minimap.instance != null)
						{
							Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.magenta, trackOrientation: false, depthAboveAll: true);
						}
						break;
					}
				}
				if (RCextensions.returnIntFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.team]) == 2)
				{
					this.myNetWorkName.GetComponent<UILabel>().text = "[FF0000]AHSS\n[FFFFFF]";
					if (!flag && Minimap.instance != null)
					{
						Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.red, trackOrientation: false, depthAboveAll: true);
					}
				}
				else if (!flag && Minimap.instance != null)
				{
					Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.blue, trackOrientation: false, depthAboveAll: true);
				}
			}
			string text = RCextensions.returnStringFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.guildName]);
			if (text != string.Empty)
			{
				UILabel component = this.myNetWorkName.GetComponent<UILabel>();
				string text2 = component.text;
				string[] array = new string[5]
				{
					text2,
					"[FFFF00]",
					text,
					"\n[FFFFFF]",
					RCextensions.returnStringFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.name])
				};
				component.text = string.Concat(array);
			}
			else
			{
				this.myNetWorkName.GetComponent<UILabel>().text += RCextensions.returnStringFromObject(base.photonView.owner.customProperties[PhotonPlayerProperty.name]);
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine)
		{
			base.gameObject.layer = LayerMask.NameToLayer("NetworkObject");
			this.setup.init();
			this.setup.myCostume = new HeroCostume();
			this.setup.myCostume = CostumeConeveter.PhotonDataToHeroCostume2(base.photonView.owner);
			this.setup.setCharacterComponent();
			UnityEngine.Object.Destroy(this.checkBoxLeft);
			UnityEngine.Object.Destroy(this.checkBoxRight);
			UnityEngine.Object.Destroy(this.leftbladetrail);
			UnityEngine.Object.Destroy(this.rightbladetrail);
			UnityEngine.Object.Destroy(this.leftbladetrail2);
			UnityEngine.Object.Destroy(this.rightbladetrail2);
			this.hasspawn = true;
		}
		else
		{
			this.SetInterpolationIfEnabled(interpolate: true);
			this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
			this.loadskin();
			this.hasspawn = true;
		}
		this.bombImmune = false;
		if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value)
		{
			this.bombImmune = true;
			base.StartCoroutine(this.stopImmunity());
			this.SetupThunderSpears();
		}
		if (this._needSetupThunderspears)
		{
			this.CreateAndAttachThunderSpears();
		}
		this._hasRunStart = true;
		this.SetName();
	}

	public void SetName()
	{
		if (!(this.myNetWorkName == null) && !(this.myNetWorkName.GetComponent<UILabel>() == null))
		{
			if (SettingsManager.UISettings.DisableNameColors.Value)
			{
				this.ForceWhiteName();
			}
			if (SettingsManager.LegacyGameSettings.GlobalHideNames.Value || SettingsManager.UISettings.HideNames.Value)
			{
				this.HideName();
			}
		}
	}

	public void HideName()
	{
		this.myNetWorkName.GetComponent<UILabel>().text = string.Empty;
	}

	public void ForceWhiteName()
	{
		UILabel component = this.myNetWorkName.GetComponent<UILabel>();
		component.text = component.text.StripHex();
	}

	public void SetInterpolationIfEnabled(bool interpolate)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
		{
			if (interpolate && SettingsManager.GraphicsSettings.InterpolationEnabled.Value)
			{
				base.rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			}
			else
			{
				base.rigidbody.interpolation = RigidbodyInterpolation.None;
			}
		}
	}

	public IEnumerator stopImmunity()
	{
		yield return new WaitForSeconds(5f);
		this.bombImmune = false;
	}

	private void suicide()
	{
	}

	private void suicide2()
	{
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			this.netDieLocal(base.rigidbody.velocity * 50f, isBite: false, -1, string.Empty);
			FengGameManagerMKII.instance.needChooseSide = true;
			FengGameManagerMKII.instance.justSuicide = true;
		}
	}

	private void throwBlades()
	{
		Transform transform = this.setup.part_blade_l.transform;
		Transform transform2 = this.setup.part_blade_r.transform;
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
		GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
		gameObject.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
		obj.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
		Vector3 force = base.transform.forward + base.transform.up * 2f - base.transform.right;
		gameObject.rigidbody.AddForce(force, ForceMode.Impulse);
		Vector3 force2 = base.transform.forward + base.transform.up * 2f + base.transform.right;
		obj.rigidbody.AddForce(force2, ForceMode.Impulse);
		Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
		torque.Normalize();
		gameObject.rigidbody.AddTorque(torque);
		torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
		torque.Normalize();
		obj.rigidbody.AddTorque(torque);
		this.setup.part_blade_l.SetActive(value: false);
		this.setup.part_blade_r.SetActive(value: false);
		this.currentBladeNum--;
		if (this.currentBladeNum == 0)
		{
			this.currentBladeSta = 0f;
		}
		if (this.state == HERO_STATE.Attack)
		{
			this.falseAttack();
		}
	}

	public void ungrabbed()
	{
		this.facingDirection = 0f;
		this.targetRotation = Quaternion.Euler(0f, 0f, 0f);
		base.transform.parent = null;
		base.GetComponent<CapsuleCollider>().isTrigger = false;
		this.state = HERO_STATE.Idle;
	}

	private void unmounted()
	{
		this.myHorse.GetComponent<Horse>().unmounted();
		this.isMounted = false;
	}

	public void update2()
	{
		if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return;
		}
		if (this.invincible > 0f)
		{
			this.invincible -= Time.deltaTime;
		}
		if (this.hasDied)
		{
			return;
		}
		if (this.titanForm && this.eren_titan != null)
		{
			this.baseTransform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
			base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
		}
		else if (this.isCannon && this.myCannon != null)
		{
			this.updateCannon();
			base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine)
		{
			return;
		}
		this.UpdateInput();
		this._dashCooldownLeft -= Time.deltaTime;
		if (this._dashCooldownLeft < 0f)
		{
			this._dashCooldownLeft = 0f;
		}
		if (this.myCannonRegion != null)
		{
			FengGameManagerMKII.instance.ShowHUDInfoCenter($"Press {SettingsManager.InputSettings.Interaction.Interact.ToString()} to use Cannon.");
			if (SettingsManager.InputSettings.Interaction.Interact.GetKeyDown())
			{
				this.myCannonRegion.photonView.RPC("RequestControlRPC", PhotonTargets.MasterClient, base.photonView.viewID);
			}
		}
		if (this.state == HERO_STATE.Grab && !this.useGun)
		{
			if (this.skillId == "jean")
			{
				if (this.state != HERO_STATE.Attack && (SettingsManager.InputSettings.Human.AttackDefault.GetKeyDown() || SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown()) && this.escapeTimes > 0 && !this.baseAnimation.IsPlaying("grabbed_jean"))
				{
					this.playAnimation("grabbed_jean");
					this.baseAnimation["grabbed_jean"].time = 0f;
					this.escapeTimes--;
				}
				if (!this.baseAnimation.IsPlaying("grabbed_jean") || !(this.baseAnimation["grabbed_jean"].normalizedTime > 0.64f) || !(this.titanWhoGrabMe.GetComponent<TITAN>() != null))
				{
					return;
				}
				this.ungrabbed();
				this.baseRigidBody.velocity = Vector3.up * 30f;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape(null);
					return;
				}
				base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
				if (PhotonNetwork.isMasterClient)
				{
					this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape(null);
				}
				else
				{
					PhotonView.Find(this.titanWhoGrabMeID).RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
				}
			}
			else
			{
				if (!(this.skillId == "eren"))
				{
					return;
				}
				this.showSkillCD();
				if (IN_GAME_MAIN_CAMERA.gametype != 0 || (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !GameMenu.Paused))
				{
					this.calcSkillCD();
					this.calcFlareCD();
				}
				if (!SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown())
				{
					return;
				}
				bool flag = false;
				if (this.skillCDDuration > 0f || flag)
				{
					flag = true;
					return;
				}
				this.skillCDDuration = this.skillCDLast;
				if (!(this.skillId == "eren") || !(this.titanWhoGrabMe.GetComponent<TITAN>() != null))
				{
					return;
				}
				this.ungrabbed();
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape(null);
				}
				else
				{
					base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All);
					if (PhotonNetwork.isMasterClient)
					{
						this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape(null);
					}
					else
					{
						PhotonView.Find(this.titanWhoGrabMeID).photonView.RPC("grabbedTargetEscape", PhotonTargets.MasterClient);
					}
				}
				this.erenTransform();
			}
		}
		else if (!this.titanForm && !this.isCannon)
		{
			this.bufferUpdate();
			this.UpdateThunderSpear();
			if (!GameMenu.InMenu())
			{
				if (!this.grounded && this.state != HERO_STATE.AirDodge && (!this.isMounted || !(this.myHorse != null)))
				{
					this.checkDashRebind();
					if (SettingsManager.InputSettings.Human.DashDoubleTap.Value)
					{
						this.checkDashDoubleTap();
					}
					if (this.dashD)
					{
						this.dashD = false;
						this.dash(0f, -1f);
						return;
					}
					if (this.dashU)
					{
						this.dashU = false;
						this.dash(0f, 1f);
						return;
					}
					if (this.dashL)
					{
						this.dashL = false;
						this.dash(-1f, 0f);
						return;
					}
					if (this.dashR)
					{
						this.dashR = false;
						this.dash(1f, 0f);
						return;
					}
				}
				if (this.grounded && (this.state == HERO_STATE.Idle || this.state == HERO_STATE.Slide))
				{
					if (SettingsManager.InputSettings.Human.Jump.GetKeyDown() && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton"))
					{
						this.idle();
						this.crossFade("jump", 0.1f);
						this.sparks.enableEmission = false;
					}
					if (SettingsManager.InputSettings.Human.HorseMount.GetKeyDown() && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton") && this.myHorse != null && !this.isMounted && Vector3.Distance(this.myHorse.transform.position, base.transform.position) < 15f)
					{
						this.getOnHorse();
					}
					if (SettingsManager.InputSettings.Human.Dodge.GetKeyDown() && !this.baseAnimation.IsPlaying("jump") && !this.baseAnimation.IsPlaying("horse_geton"))
					{
						this.dodge2();
						return;
					}
				}
			}
			if (this.state == HERO_STATE.Idle && !GameMenu.InMenu())
			{
				this._flareDelayAfterEmote -= Time.deltaTime;
				if (this._flareDelayAfterEmote <= 0f)
				{
					if (SettingsManager.InputSettings.Human.Flare1.GetKeyDown())
					{
						this.shootFlare(1);
					}
					if (SettingsManager.InputSettings.Human.Flare2.GetKeyDown())
					{
						this.shootFlare(2);
					}
					if (SettingsManager.InputSettings.Human.Flare3.GetKeyDown())
					{
						this.shootFlare(3);
					}
				}
				if (SettingsManager.InputSettings.General.ChangeCharacter.GetKeyDown())
				{
					this.suicide2();
				}
				if (this.myHorse != null && this.isMounted && SettingsManager.InputSettings.Human.HorseMount.GetKeyDown())
				{
					this.getOffHorse();
				}
				if ((base.animation.IsPlaying(this.standAnimation) || !this.grounded) && SettingsManager.InputSettings.Human.Reload.GetKeyDown() && (!this.useGun || SettingsManager.LegacyGameSettings.AHSSAirReload.Value || this.grounded))
				{
					this.changeBlade();
					return;
				}
				if (!this.isMounted && (SettingsManager.InputSettings.Human.AttackDefault.GetKeyDown() || SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown()) && !this.useGun)
				{
					bool flag2 = false;
					if (SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown())
					{
						if (this.skillCDDuration > 0f || flag2)
						{
							flag2 = true;
						}
						else
						{
							this.skillCDDuration = this.skillCDLast;
							if (this.skillId == "eren")
							{
								this.erenTransform();
								return;
							}
							if (this.skillId == "marco")
							{
								if (this.IsGrounded())
								{
									this.attackAnimation = ((UnityEngine.Random.Range(0, 2) != 0) ? "special_marco_1" : "special_marco_0");
									this.playAnimation(this.attackAnimation);
								}
								else
								{
									flag2 = true;
									this.skillCDDuration = 0f;
								}
							}
							else if (this.skillId == "armin")
							{
								if (this.IsGrounded())
								{
									this.attackAnimation = "special_armin";
									this.playAnimation("special_armin");
								}
								else
								{
									flag2 = true;
									this.skillCDDuration = 0f;
								}
							}
							else if (this.skillId == "sasha")
							{
								if (this.IsGrounded())
								{
									this.attackAnimation = "special_sasha";
									this.playAnimation("special_sasha");
									this.currentBuff = BUFF.SpeedUp;
									this.buffTime = 10f;
								}
								else
								{
									flag2 = true;
									this.skillCDDuration = 0f;
								}
							}
							else if (this.skillId == "mikasa")
							{
								this.attackAnimation = "attack3_1";
								this.playAnimation("attack3_1");
								this.baseRigidBody.velocity = Vector3.up * 10f;
							}
							else if (this.skillId == "levi")
							{
								this.attackAnimation = "attack5";
								this.playAnimation("attack5");
								this.baseRigidBody.velocity += Vector3.up * 5f;
								Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
								LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
								if (Physics.Raycast(ray, out var hitInfo, 10000000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask)).value))
								{
									if (this.bulletRight != null)
									{
										this.bulletRight.GetComponent<Bullet>().disable();
										this.releaseIfIHookSb();
									}
									this.dashDirection = hitInfo.point - this.baseTransform.position;
									this.launchRightRope(hitInfo, single: true, 1);
									this.rope.Play();
								}
								this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
								this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
								this.attackLoop = 3;
							}
							else if (this.skillId == "petra")
							{
								this.attackAnimation = "special_petra";
								this.playAnimation("special_petra");
								this.baseRigidBody.velocity += Vector3.up * 5f;
								Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
								LayerMask layerMask2 = 1 << LayerMask.NameToLayer("Ground");
								if (Physics.Raycast(ray2, out var hitInfo2, 10000000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask2)).value))
								{
									if (this.bulletRight != null)
									{
										this.bulletRight.GetComponent<Bullet>().disable();
										this.releaseIfIHookSb();
									}
									if (this.bulletLeft != null)
									{
										this.bulletLeft.GetComponent<Bullet>().disable();
										this.releaseIfIHookSb();
									}
									this.dashDirection = hitInfo2.point - this.baseTransform.position;
									this.launchLeftRope(hitInfo2, single: true);
									this.launchRightRope(hitInfo2, single: true);
									this.rope.Play();
								}
								this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
								this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
								this.attackLoop = 3;
							}
							else
							{
								if (this.needLean)
								{
									if (this.leanLeft)
									{
										this.attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2");
									}
									else
									{
										this.attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2");
									}
								}
								else
								{
									this.attackAnimation = "attack1";
								}
								this.playAnimation(this.attackAnimation);
							}
						}
					}
					else if (SettingsManager.InputSettings.Human.AttackDefault.GetKeyDown())
					{
						if (this.needLean)
						{
							if (SettingsManager.InputSettings.General.Left.GetKey())
							{
								this.attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2");
							}
							else if (SettingsManager.InputSettings.General.Right.GetKey())
							{
								this.attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2");
							}
							else if (this.leanLeft)
							{
								this.attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2");
							}
							else
							{
								this.attackAnimation = ((UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2");
							}
						}
						else if (SettingsManager.InputSettings.General.Left.GetKey())
						{
							this.attackAnimation = "attack2";
						}
						else if (SettingsManager.InputSettings.General.Right.GetKey())
						{
							this.attackAnimation = "attack1";
						}
						else if (this.lastHook != null)
						{
							if (this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck") != null)
							{
								this.attackAccordingToTarget(this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
							}
							else
							{
								flag2 = true;
							}
						}
						else if (this.bulletLeft != null && this.bulletLeft.transform.parent != null)
						{
							Transform transform = this.bulletLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
							if (transform != null)
							{
								this.attackAccordingToTarget(transform);
							}
							else
							{
								this.attackAccordingToMouse();
							}
						}
						else if (this.bulletRight != null && this.bulletRight.transform.parent != null)
						{
							Transform transform2 = this.bulletRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
							if (transform2 != null)
							{
								this.attackAccordingToTarget(transform2);
							}
							else
							{
								this.attackAccordingToMouse();
							}
						}
						else
						{
							GameObject gameObject = this.findNearestTitan();
							if (gameObject != null)
							{
								Transform transform3 = gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
								if (transform3 != null)
								{
									this.attackAccordingToTarget(transform3);
								}
								else
								{
									this.attackAccordingToMouse();
								}
							}
							else
							{
								this.attackAccordingToMouse();
							}
						}
					}
					if (!flag2)
					{
						this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
						this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
						if (this.grounded)
						{
							this.baseRigidBody.AddForce(base.gameObject.transform.forward * 200f);
						}
						this.playAnimation(this.attackAnimation);
						this.baseAnimation[this.attackAnimation].time = 0f;
						this.buttonAttackRelease = false;
						this.state = HERO_STATE.Attack;
						if (this.grounded || this.attackAnimation == "attack3_1" || this.attackAnimation == "attack5" || this.attackAnimation == "special_petra")
						{
							this.attackReleased = true;
							this.buttonAttackRelease = true;
						}
						else
						{
							this.attackReleased = false;
						}
						this.sparks.enableEmission = false;
					}
				}
				if (this.useGun)
				{
					if (SettingsManager.InputSettings.Human.AttackSpecial.GetKey())
					{
						this.leftArmAim = true;
						this.rightArmAim = true;
					}
					else if (SettingsManager.InputSettings.Human.AttackDefault.GetKey())
					{
						if (this.leftGunHasBullet)
						{
							this.leftArmAim = true;
							this.rightArmAim = false;
						}
						else
						{
							this.leftArmAim = false;
							if (this.rightGunHasBullet)
							{
								this.rightArmAim = true;
							}
							else
							{
								this.rightArmAim = false;
							}
						}
					}
					else
					{
						this.leftArmAim = false;
						this.rightArmAim = false;
					}
					if (this.leftArmAim || this.rightArmAim)
					{
						Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
						LayerMask layerMask3 = 1 << LayerMask.NameToLayer("Ground");
						if (Physics.Raycast(ray3, out var hitInfo3, 10000000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask3)).value))
						{
							this.gunTarget = hitInfo3.point;
						}
					}
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					if (SettingsManager.InputSettings.Human.AttackSpecial.GetKeyUp() && this.skillId != "bomb")
					{
						if (this.leftGunHasBullet && this.rightGunHasBullet)
						{
							if (this.grounded)
							{
								this.attackAnimation = "AHSS_shoot_both";
							}
							else
							{
								this.attackAnimation = "AHSS_shoot_both_air";
							}
							flag3 = true;
						}
						else if (!this.leftGunHasBullet && !this.rightGunHasBullet)
						{
							flag4 = true;
						}
						else
						{
							flag5 = true;
						}
					}
					if (flag5 || SettingsManager.InputSettings.Human.AttackDefault.GetKeyUp())
					{
						if (this.grounded)
						{
							if (this.leftGunHasBullet && this.rightGunHasBullet)
							{
								if (this.isLeftHandHooked)
								{
									this.attackAnimation = "AHSS_shoot_r";
								}
								else
								{
									this.attackAnimation = "AHSS_shoot_l";
								}
							}
							else if (this.leftGunHasBullet)
							{
								this.attackAnimation = "AHSS_shoot_l";
							}
							else if (this.rightGunHasBullet)
							{
								this.attackAnimation = "AHSS_shoot_r";
							}
						}
						else if (this.leftGunHasBullet && this.rightGunHasBullet)
						{
							if (this.isLeftHandHooked)
							{
								this.attackAnimation = "AHSS_shoot_r_air";
							}
							else
							{
								this.attackAnimation = "AHSS_shoot_l_air";
							}
						}
						else if (this.leftGunHasBullet)
						{
							this.attackAnimation = "AHSS_shoot_l_air";
						}
						else if (this.rightGunHasBullet)
						{
							this.attackAnimation = "AHSS_shoot_r_air";
						}
						if (this.leftGunHasBullet || this.rightGunHasBullet)
						{
							flag3 = true;
						}
						else
						{
							flag4 = true;
						}
					}
					if (flag3)
					{
						this.state = HERO_STATE.Attack;
						this.crossFade(this.attackAnimation, 0.05f);
						this.gunDummy.transform.position = this.baseTransform.position;
						this.gunDummy.transform.rotation = this.baseTransform.rotation;
						this.gunDummy.transform.LookAt(this.gunTarget);
						this.attackReleased = false;
						this.facingDirection = this.gunDummy.transform.rotation.eulerAngles.y;
						this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
					}
					else if (flag4 && (this.grounded || (LevelInfo.getInfo(FengGameManagerMKII.level).type != GAMEMODE.PVP_AHSS && SettingsManager.LegacyGameSettings.AHSSAirReload.Value)))
					{
						this.changeBlade();
					}
				}
			}
			else if (this.state == HERO_STATE.Attack)
			{
				if (!this.useGun)
				{
					if (!SettingsManager.InputSettings.Human.AttackDefault.GetKey())
					{
						this.buttonAttackRelease = true;
					}
					if (!this.attackReleased)
					{
						if (this.buttonAttackRelease)
						{
							this.continueAnimation();
							this.attackReleased = true;
						}
						else if (this.baseAnimation[this.attackAnimation].normalizedTime >= 0.32f)
						{
							this.pauseAnimation();
						}
					}
					if (this.attackAnimation == "attack3_1" && this.currentBladeSta > 0f)
					{
						if (this.baseAnimation[this.attackAnimation].normalizedTime >= 0.8f)
						{
							if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
							{
								this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
								if (SettingsManager.GraphicsSettings.WeaponTrailEnabled.Value)
								{
									this.leftbladetrail2.Activate();
									this.rightbladetrail2.Activate();
									this.leftbladetrail.Activate();
									this.rightbladetrail.Activate();
								}
								this.baseRigidBody.velocity = -Vector3.up * 30f;
							}
							if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
							{
								this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
								this.slash.Play();
							}
						}
						else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
						{
							this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
							this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
							this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
							this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
							this.leftbladetrail.StopSmoothly(0.1f);
							this.rightbladetrail.StopSmoothly(0.1f);
							this.leftbladetrail2.StopSmoothly(0.1f);
							this.rightbladetrail2.StopSmoothly(0.1f);
						}
					}
					else
					{
						float num2;
						float num;
						if (this.currentBladeSta == 0f)
						{
							num2 = (num = -1f);
						}
						else if (this.attackAnimation == "attack5")
						{
							num2 = 0.35f;
							num = 0.5f;
						}
						else if (this.attackAnimation == "special_petra")
						{
							num2 = 0.35f;
							num = 0.48f;
						}
						else if (this.attackAnimation == "special_armin")
						{
							num2 = 0.25f;
							num = 0.35f;
						}
						else if (this.attackAnimation == "attack4")
						{
							num2 = 0.6f;
							num = 0.9f;
						}
						else if (this.attackAnimation == "special_sasha")
						{
							num2 = (num = -1f);
						}
						else
						{
							num2 = 0.5f;
							num = 0.85f;
						}
						if (this.baseAnimation[this.attackAnimation].normalizedTime > num2 && this.baseAnimation[this.attackAnimation].normalizedTime < num)
						{
							if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
							{
								this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
								this.slash.Play();
								if (SettingsManager.GraphicsSettings.WeaponTrailEnabled.Value)
								{
									this.leftbladetrail2.Activate();
									this.rightbladetrail2.Activate();
									this.leftbladetrail.Activate();
									this.rightbladetrail.Activate();
								}
							}
							if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
							{
								this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
							}
						}
						else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
						{
							this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
							this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
							this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
							this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
							this.leftbladetrail2.StopSmoothly(0.1f);
							this.rightbladetrail2.StopSmoothly(0.1f);
							this.leftbladetrail.StopSmoothly(0.1f);
							this.rightbladetrail.StopSmoothly(0.1f);
						}
						if (this.attackLoop > 0 && this.baseAnimation[this.attackAnimation].normalizedTime > num)
						{
							this.attackLoop--;
							this.playAnimationAt(this.attackAnimation, num2);
						}
					}
					if (this.baseAnimation[this.attackAnimation].normalizedTime >= 1f)
					{
						if (this.attackAnimation == "special_marco_0" || this.attackAnimation == "special_marco_1")
						{
							if (IN_GAME_MAIN_CAMERA.gametype != 0)
							{
								if (!PhotonNetwork.isMasterClient)
								{
									object[] parameters = new object[2] { 5f, 100f };
									base.photonView.RPC("netTauntAttack", PhotonTargets.MasterClient, parameters);
								}
								else
								{
									this.netTauntAttack(5f, 100f, null);
								}
							}
							else
							{
								this.netTauntAttack(5f, 100f, null);
							}
							this.falseAttack();
							this.idle();
						}
						else if (this.attackAnimation == "special_armin")
						{
							if (IN_GAME_MAIN_CAMERA.gametype != 0)
							{
								if (!PhotonNetwork.isMasterClient)
								{
									base.photonView.RPC("netlaughAttack", PhotonTargets.MasterClient);
								}
								else
								{
									this.netlaughAttack(null);
								}
							}
							else
							{
								GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
								foreach (GameObject gameObject2 in array)
								{
									if (Vector3.Distance(gameObject2.transform.position, this.baseTransform.position) < 50f && Vector3.Angle(gameObject2.transform.forward, this.baseTransform.position - gameObject2.transform.position) < 90f && gameObject2.GetComponent<TITAN>() != null)
									{
										gameObject2.GetComponent<TITAN>().beLaughAttacked();
									}
								}
							}
							this.falseAttack();
							this.idle();
						}
						else if (this.attackAnimation == "attack3_1")
						{
							this.baseRigidBody.velocity -= Vector3.up * Time.deltaTime * 30f;
						}
						else
						{
							this.falseAttack();
							this.idle();
						}
					}
					if (this.baseAnimation.IsPlaying("attack3_2") && this.baseAnimation["attack3_2"].normalizedTime >= 1f)
					{
						this.falseAttack();
						this.idle();
					}
				}
				else
				{
					this.baseTransform.rotation = Quaternion.Lerp(this.baseTransform.rotation, this.gunDummy.transform.rotation, Time.deltaTime * 30f);
					if (!this.attackReleased && this.baseAnimation[this.attackAnimation].normalizedTime > 0.167f)
					{
						this.attackReleased = true;
						bool flag6 = false;
						if (this.attackAnimation == "AHSS_shoot_both" || this.attackAnimation == "AHSS_shoot_both_air")
						{
							flag6 = true;
							this.leftGunHasBullet = false;
							this.rightGunHasBullet = false;
							this.baseRigidBody.AddForce(-this.baseTransform.forward * 1000f, ForceMode.Acceleration);
						}
						else
						{
							if (this.attackAnimation == "AHSS_shoot_l" || this.attackAnimation == "AHSS_shoot_l_air")
							{
								this.leftGunHasBullet = false;
							}
							else
							{
								this.rightGunHasBullet = false;
							}
							this.baseRigidBody.AddForce(-this.baseTransform.forward * 600f, ForceMode.Acceleration);
						}
						this.baseRigidBody.AddForce(Vector3.up * 200f, ForceMode.Acceleration);
						string text = "FX/shotGun";
						if (flag6)
						{
							text = "FX/shotGun 1";
						}
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
						{
							GameObject gameObject3 = PhotonNetwork.Instantiate(text, this.baseTransform.position + this.baseTransform.up * 0.8f - this.baseTransform.right * 0.1f, this.baseTransform.rotation, 0);
							if (gameObject3.GetComponent<EnemyfxIDcontainer>() != null)
							{
								gameObject3.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
							}
						}
						else
						{
							GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(text), this.baseTransform.position + this.baseTransform.up * 0.8f - this.baseTransform.right * 0.1f, this.baseTransform.rotation);
						}
					}
					if (this.baseAnimation[this.attackAnimation].normalizedTime >= 1f)
					{
						this.falseAttack();
						this.idle();
					}
					if (!this.baseAnimation.IsPlaying(this.attackAnimation))
					{
						this.falseAttack();
						this.idle();
					}
				}
			}
			else if (this.state == HERO_STATE.ChangeBlade)
			{
				if (this.useGun)
				{
					if (this.baseAnimation[this.reloadAnimation].normalizedTime > 0.22f)
					{
						if (!this.leftGunHasBullet && this.setup.part_blade_l.activeSelf)
						{
							this.setup.part_blade_l.SetActive(value: false);
							Transform transform4 = this.setup.part_blade_l.transform;
							GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform4.position, transform4.rotation);
							obj.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
							Vector3 force = -this.baseTransform.forward * 10f + this.baseTransform.up * 5f - this.baseTransform.right;
							obj.rigidbody.AddForce(force, ForceMode.Impulse);
							Vector3 torque = new Vector3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));
							obj.rigidbody.AddTorque(torque, ForceMode.Acceleration);
						}
						if (!this.rightGunHasBullet && this.setup.part_blade_r.activeSelf)
						{
							this.setup.part_blade_r.SetActive(value: false);
							Transform transform5 = this.setup.part_blade_r.transform;
							GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform5.position, transform5.rotation);
							obj2.renderer.material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
							Vector3 force2 = -this.baseTransform.forward * 10f + this.baseTransform.up * 5f + this.baseTransform.right;
							obj2.rigidbody.AddForce(force2, ForceMode.Impulse);
							Vector3 torque2 = new Vector3(UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300), UnityEngine.Random.Range(-300, 300));
							obj2.rigidbody.AddTorque(torque2, ForceMode.Acceleration);
						}
					}
					if (this.baseAnimation[this.reloadAnimation].normalizedTime > 0.62f && !this.throwedBlades)
					{
						this.throwedBlades = true;
						if (this.leftBulletLeft > 0 && !this.leftGunHasBullet)
						{
							this.leftBulletLeft--;
							this.setup.part_blade_l.SetActive(value: true);
							this.leftGunHasBullet = true;
						}
						if (this.rightBulletLeft > 0 && !this.rightGunHasBullet)
						{
							this.setup.part_blade_r.SetActive(value: true);
							this.rightBulletLeft--;
							this.rightGunHasBullet = true;
						}
						this.updateRightMagUI();
						this.updateLeftMagUI();
					}
					if (this.baseAnimation[this.reloadAnimation].normalizedTime > 1f)
					{
						this.idle();
					}
				}
				else
				{
					if (!this.grounded)
					{
						if (base.animation[this.reloadAnimation].normalizedTime >= 0.2f && !this.throwedBlades)
						{
							this.throwedBlades = true;
							if (this.setup.part_blade_l.activeSelf)
							{
								this.throwBlades();
							}
						}
						if (base.animation[this.reloadAnimation].normalizedTime >= 0.56f && this.currentBladeNum > 0)
						{
							this.setup.part_blade_l.SetActive(value: true);
							this.setup.part_blade_r.SetActive(value: true);
							this.currentBladeSta = this.totalBladeSta;
						}
					}
					else
					{
						if (this.baseAnimation[this.reloadAnimation].normalizedTime >= 0.13f && !this.throwedBlades)
						{
							this.throwedBlades = true;
							if (this.setup.part_blade_l.activeSelf)
							{
								this.throwBlades();
							}
						}
						if (this.baseAnimation[this.reloadAnimation].normalizedTime >= 0.37f && this.currentBladeNum > 0)
						{
							this.setup.part_blade_l.SetActive(value: true);
							this.setup.part_blade_r.SetActive(value: true);
							this.currentBladeSta = this.totalBladeSta;
						}
					}
					if (this.baseAnimation[this.reloadAnimation].normalizedTime >= 1f)
					{
						this.idle();
					}
				}
			}
			else if (this.state == HERO_STATE.Salute)
			{
				this._currentEmoteActionTime -= Time.deltaTime;
				if (this._currentEmoteActionTime <= 0f)
				{
					this.idle();
				}
			}
			else if (this.state == HERO_STATE.GroundDodge)
			{
				if (this.baseAnimation.IsPlaying("dodge"))
				{
					if (!this.grounded && !(this.baseAnimation["dodge"].normalizedTime <= 0.6f))
					{
						this.idle();
					}
					if (this.baseAnimation["dodge"].normalizedTime >= 1f)
					{
						this.idle();
					}
				}
			}
			else if (this.state == HERO_STATE.Land)
			{
				if (this.baseAnimation.IsPlaying("dash_land") && this.baseAnimation["dash_land"].normalizedTime >= 1f)
				{
					this.idle();
				}
			}
			else if (this.state == HERO_STATE.FillGas)
			{
				if (this.baseAnimation.IsPlaying("supply") && this.baseAnimation["supply"].normalizedTime >= 1f)
				{
					if (this.skillId != "bomb")
					{
						this.currentBladeSta = this.totalBladeSta;
						this.currentBladeNum = this.totalBladeNum;
						if (!this.useGun)
						{
							this.setup.part_blade_l.SetActive(value: true);
							this.setup.part_blade_r.SetActive(value: true);
						}
						else
						{
							this.leftBulletLeft = (this.rightBulletLeft = this.bulletMAX);
							this.leftGunHasBullet = (this.rightGunHasBullet = true);
							this.setup.part_blade_l.SetActive(value: true);
							this.setup.part_blade_r.SetActive(value: true);
							this.updateRightMagUI();
							this.updateLeftMagUI();
						}
					}
					this.currentGas = this.totalGas;
					this.idle();
				}
			}
			else if (this.state == HERO_STATE.Slide)
			{
				if (!this.grounded)
				{
					this.idle();
				}
			}
			else if (this.state == HERO_STATE.AirDodge)
			{
				if (this.dashTime > 0f)
				{
					this.dashTime -= Time.deltaTime;
					if (this.currentSpeed > this.originVM)
					{
						this.baseRigidBody.AddForce(-this.baseRigidBody.velocity * Time.deltaTime * 1.7f, ForceMode.VelocityChange);
					}
				}
				else
				{
					this.dashTime = 0f;
					this.idle();
				}
			}
			if (!GameMenu.InMenu())
			{
				if (SettingsManager.InputSettings.Human.HookLeft.GetKey() && ((!this.baseAnimation.IsPlaying("attack3_1") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && this.state != HERO_STATE.Grab) || this.state == HERO_STATE.Idle))
				{
					if (this.bulletLeft != null)
					{
						this.QHold = true;
					}
					else
					{
						Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
						LayerMask layerMask4 = 1 << LayerMask.NameToLayer("Ground");
						if (Physics.Raycast(ray4, out var hitInfo4, 10000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask4)).value))
						{
							this.launchLeftRope(hitInfo4, single: true);
							this.rope.Play();
						}
					}
				}
				else
				{
					this.QHold = false;
				}
				if (SettingsManager.InputSettings.Human.HookRight.GetKey() && ((!this.baseAnimation.IsPlaying("attack3_1") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && this.state != HERO_STATE.Grab) || this.state == HERO_STATE.Idle))
				{
					if (this.bulletRight != null)
					{
						this.EHold = true;
					}
					else
					{
						Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
						LayerMask layerMask5 = 1 << LayerMask.NameToLayer("Ground");
						if (Physics.Raycast(ray5, out var hitInfo5, 10000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask5)).value))
						{
							this.launchRightRope(hitInfo5, single: true);
							this.rope.Play();
						}
					}
				}
				else
				{
					this.EHold = false;
				}
				if (SettingsManager.InputSettings.Human.HookBoth.GetKey() && ((!this.baseAnimation.IsPlaying("attack3_1") && !this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra") && this.state != HERO_STATE.Grab) || this.state == HERO_STATE.Idle))
				{
					this.QHold = true;
					this.EHold = true;
					if (this.bulletLeft == null && this.bulletRight == null)
					{
						Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
						LayerMask layerMask6 = 1 << LayerMask.NameToLayer("Ground");
						if (Physics.Raycast(ray6, out var hitInfo6, 1000000f, ((LayerMask)((int)(LayerMask)(1 << LayerMask.NameToLayer("EnemyBox")) | (int)layerMask6)).value))
						{
							this.launchLeftRope(hitInfo6, single: false);
							this.launchRightRope(hitInfo6, single: false);
							this.rope.Play();
						}
					}
				}
			}
			if (IN_GAME_MAIN_CAMERA.gametype != 0 || (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !GameMenu.Paused))
			{
				this.calcSkillCD();
				this.calcFlareCD();
			}
			if (!this.useGun)
			{
				if (this.leftbladetrail.gameObject.GetActive())
				{
					this.leftbladetrail.update();
					this.rightbladetrail.update();
				}
				if (this.leftbladetrail2.gameObject.GetActive())
				{
					this.leftbladetrail2.update();
					this.rightbladetrail2.update();
				}
				if (this.leftbladetrail.gameObject.GetActive())
				{
					this.leftbladetrail.lateUpdate();
					this.rightbladetrail.lateUpdate();
				}
				if (this.leftbladetrail2.gameObject.GetActive())
				{
					this.leftbladetrail2.lateUpdate();
					this.rightbladetrail2.lateUpdate();
				}
			}
			if (!GameMenu.Paused)
			{
				this.showSkillCD();
				this.showFlareCD2();
				this.showGas2();
				this.showAimUI2();
			}
		}
		else if (this.isCannon && !GameMenu.Paused)
		{
			this.showAimUI2();
			this.calcSkillCD();
			this.showSkillCD();
		}
	}

	public void updateCannon()
	{
		this.baseTransform.position = this.myCannonPlayer.position;
		this.baseTransform.rotation = this.myCannonBase.rotation;
	}

	private void LaunchThunderSpear()
	{
		if (this.myBomb != null && !this.myBomb.Disabled)
		{
			this.myBomb.Explode(this.bombRadius);
		}
		this.detonate = false;
		this.bombTime = 0f;
		this.skillCDDuration = this.bombCD;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("EnemyBox"));
		Vector3 vector = this.baseTransform.position + Vector3.forward * 1000f;
		if (Physics.Raycast(ray, out var hitInfo, 1000000f, layerMask.value))
		{
			vector = hitInfo.point;
		}
		Vector3 vector2 = Vector3.Normalize(vector - this.baseTransform.position);
		Vector3 position;
		if (Vector3.Cross(this.baseTransform.forward, vector2).y < 0f && this.state != HERO_STATE.Land)
		{
			position = this.ThunderSpearL.transform.position;
			this.ThunderSpearL.audio.Play();
			this.SetThunderSpears(hasLeft: false, hasRight: true);
			this.attackAnimation = "AHSS_shoot_l";
		}
		else
		{
			position = this.ThunderSpearR.transform.position;
			this.ThunderSpearR.audio.Play();
			this.SetThunderSpears(hasLeft: true, hasRight: false);
			this.attackAnimation = "AHSS_shoot_r";
		}
		Vector3 vector3 = Vector3.Normalize(vector - position);
		if (this.grounded)
		{
			position += vector3 * 1f;
		}
		if (this.state != HERO_STATE.Slide)
		{
			if (this.state == HERO_STATE.Attack)
			{
				this.buttonAttackRelease = true;
			}
			this.playAnimationAt(this.attackAnimation, 0.1f);
			this.state = HERO_STATE.Attack;
			this.facingDirection = Quaternion.LookRotation(vector2).eulerAngles.y;
			this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
		}
		GameObject gameObject = PhotonNetwork.Instantiate("RCAsset/BombMain", position, Quaternion.LookRotation(vector3), 0);
		gameObject.rigidbody.velocity = vector3 * this.bombSpeed;
		this.myBomb = gameObject.GetComponent<Bomb>();
		this.myBomb.Setup(this, this.bombRadius);
	}

	public void UpdateThunderSpear()
	{
		if (!(this.skillId == "bomb"))
		{
			return;
		}
		this.leftArmAim = false;
		this.rightArmAim = false;
		bool keyDown = SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown();
		bool keyUp = SettingsManager.InputSettings.Human.AttackSpecial.GetKeyUp();
		if (this.skillCDDuration <= 0f && (!this.ThunderSpearLModel.activeSelf || !this.ThunderSpearRModel.activeSelf))
		{
			this.SetThunderSpears(hasLeft: true, hasRight: true);
		}
		if (keyDown && this.skillCDDuration <= 0f)
		{
			this.LaunchThunderSpear();
		}
		else if (this.myBomb != null && !this.myBomb.Disabled)
		{
			this.bombTime += Time.deltaTime;
			bool flag = false;
			if (keyUp)
			{
				this.detonate = true;
			}
			else if (keyDown && this.detonate)
			{
				this.detonate = false;
				flag = true;
			}
			if (this.bombTime >= this.bombTimeMax)
			{
				flag = true;
			}
			if (flag)
			{
				this.myBomb.Explode(this.bombRadius);
				this.detonate = false;
			}
		}
	}

	private bool IsFiringThunderSpear()
	{
		if (this.skillId == "bomb")
		{
			if (!this.baseAnimation.IsPlaying("AHSS_shoot_r"))
			{
				return this.baseAnimation.IsPlaying("AHSS_shoot_l");
			}
			return true;
		}
		return false;
	}

	private void updateLeftMagUI()
	{
		for (int i = 1; i <= this.bulletMAX; i++)
		{
			GameObject.Find("bulletL" + i).GetComponent<UISprite>().enabled = false;
		}
		for (int j = 1; j <= this.leftBulletLeft; j++)
		{
			GameObject.Find("bulletL" + j).GetComponent<UISprite>().enabled = true;
		}
	}

	private void updateRightMagUI()
	{
		for (int i = 1; i <= this.bulletMAX; i++)
		{
			GameObject.Find("bulletR" + i).GetComponent<UISprite>().enabled = false;
		}
		for (int j = 1; j <= this.rightBulletLeft; j++)
		{
			GameObject.Find("bulletR" + j).GetComponent<UISprite>().enabled = true;
		}
	}

	public void useBlade(int amount = 0)
	{
		if (amount == 0)
		{
			amount = 1;
		}
		amount *= 2;
		if (!(this.currentBladeSta > 0f))
		{
			return;
		}
		this.currentBladeSta -= amount;
		if (this.currentBladeSta <= 0f)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine)
			{
				this.leftbladetrail.Deactivate();
				this.rightbladetrail.Deactivate();
				this.leftbladetrail2.Deactivate();
				this.rightbladetrail2.Deactivate();
				this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
				this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
			}
			this.currentBladeSta = 0f;
			this.throwBlades();
		}
	}

	private void useGas(float amount = 0f)
	{
		if (SettingsManager.LegacyGameSettings.BombModeEnabled.Value && SettingsManager.LegacyGameSettings.BombModeInfiniteGas.Value)
		{
			return;
		}
		if (amount == 0f)
		{
			amount = this.useGasSpeed;
		}
		if (this.currentGas > 0f)
		{
			this.currentGas -= amount;
			if (this.currentGas < 0f)
			{
				this.currentGas = 0f;
			}
		}
	}

	[RPC]
	private void whoIsMyErenTitan(int id, PhotonMessageInfo info)
	{
		if (info != null && info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "hero eren titan exploit");
			return;
		}
		this.eren_titan = PhotonView.Find(id).gameObject;
		this.titanForm = true;
	}
}
