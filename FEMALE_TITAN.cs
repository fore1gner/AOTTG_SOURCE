using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CustomSkins;
using Photon;
using Settings;
using UI;
using UnityEngine;

public class FEMALE_TITAN : Photon.MonoBehaviour
{
	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap1;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap2;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap3;

	private Vector3 abnorma_jump_bite_horizon_v;

	public int AnkleLHP = 200;

	private int AnkleLHPMAX = 200;

	public int AnkleRHP = 200;

	private int AnkleRHPMAX = 200;

	private string attackAnimation;

	private float attackCheckTime;

	private float attackCheckTimeA;

	private float attackCheckTimeB;

	private bool attackChkOnce;

	public float attackDistance = 13f;

	private bool attacked;

	public float attackWait = 1f;

	private float attention = 10f;

	public GameObject bottomObject;

	public float chaseDistance = 80f;

	private Transform checkHitCapsuleEnd;

	private Vector3 checkHitCapsuleEndOld;

	private float checkHitCapsuleR;

	private Transform checkHitCapsuleStart;

	public GameObject currentCamera;

	private Transform currentGrabHand;

	private float desDeg;

	private float dieTime;

	private GameObject eren;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchsmap1;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchsmap2;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchsmap3;

	private string fxName;

	private Vector3 fxPosition;

	private Quaternion fxRotation;

	private GameObject grabbedTarget;

	public GameObject grabTF;

	private float gravity = 120f;

	private bool grounded;

	public bool hasDie;

	private bool hasDieSteam;

	public bool hasspawn;

	public GameObject healthLabel;

	public float healthTime;

	private string hitAnimation;

	private bool isAttackMoveByCore;

	private bool isGrabHandLeft;

	public float lagMax;

	public int maxHealth;

	public float maxVelocityChange = 80f;

	public static float minusDistance = 99999f;

	public static GameObject minusDistanceEnemy;

	public float myDistance;

	public GameObject myHero;

	public int NapeArmor = 1000;

	private bool needFreshCorePosition;

	private string nextAttackAnimation;

	private Vector3 oldCorePosition;

	private float sbtime;

	public float size;

	public float speed = 80f;

	private bool startJump;

	private string state = "idle";

	private int stepSoundPhase = 2;

	private float tauntTime;

	private string turnAnimation;

	private float turnDeg;

	private GameObject whoHasTauntMe;

	private AnnieCustomSkinLoader _customSkinLoader;

	private void attack(string type)
	{
		this.state = "attack";
		this.attacked = false;
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
		this.startJump = false;
		this.attackChkOnce = false;
		this.nextAttackAnimation = null;
		this.fxName = null;
		this.isAttackMoveByCore = false;
		this.attackCheckTime = 0f;
		this.attackCheckTimeA = 0f;
		this.attackCheckTimeB = 0f;
		this.fxRotation = Quaternion.Euler(270f, 0f, 0f);
		switch (type)
		{
		case "combo_1":
			this.attackCheckTimeA = 0.63f;
			this.attackCheckTimeB = 0.8f;
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
			this.checkHitCapsuleR = 5f;
			this.isAttackMoveByCore = true;
			this.nextAttackAnimation = "combo_2";
			break;
		case "combo_2":
			this.attackCheckTimeA = 0.27f;
			this.attackCheckTimeB = 0.43f;
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
			this.checkHitCapsuleR = 5f;
			this.isAttackMoveByCore = true;
			this.nextAttackAnimation = "combo_3";
			break;
		case "combo_3":
			this.attackCheckTimeA = 0.15f;
			this.attackCheckTimeB = 0.3f;
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
			this.checkHitCapsuleR = 5f;
			this.isAttackMoveByCore = true;
			break;
		case "combo_blind_1":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.72f;
			this.attackCheckTimeB = 0.83f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			this.nextAttackAnimation = "combo_blind_2";
			break;
		case "combo_blind_2":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.5f;
			this.attackCheckTimeB = 0.6f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			this.nextAttackAnimation = "combo_blind_3";
			break;
		case "combo_blind_3":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.2f;
			this.attackCheckTimeB = 0.28f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			break;
		case "front":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.44f;
			this.attackCheckTimeB = 0.55f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			break;
		case "jumpCombo_1":
			this.isAttackMoveByCore = false;
			this.nextAttackAnimation = "jumpCombo_2";
			this.abnorma_jump_bite_horizon_v = Vector3.zero;
			break;
		case "jumpCombo_2":
			this.isAttackMoveByCore = false;
			this.attackCheckTimeA = 0.48f;
			this.attackCheckTimeB = 0.7f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			this.nextAttackAnimation = "jumpCombo_3";
			break;
		case "jumpCombo_3":
			this.isAttackMoveByCore = false;
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L/shin_L/foot_L");
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_L");
			this.checkHitCapsuleR = 5f;
			this.attackCheckTimeA = 0.22f;
			this.attackCheckTimeB = 0.42f;
			break;
		case "jumpCombo_4":
			this.isAttackMoveByCore = false;
			break;
		case "sweep":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.39f;
			this.attackCheckTimeB = 0.6f;
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R/shin_R/foot_R");
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/thigh_R");
			this.checkHitCapsuleR = 5f;
			break;
		case "sweep_back":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.41f;
			this.attackCheckTimeB = 0.48f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			break;
		case "sweep_front_left":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.53f;
			this.attackCheckTimeB = 0.63f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			break;
		case "sweep_front_right":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.5f;
			this.attackCheckTimeB = 0.62f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
			this.checkHitCapsuleR = 4f;
			break;
		case "sweep_head_b_l":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.4f;
			this.attackCheckTimeB = 0.51f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
			this.checkHitCapsuleR = 4f;
			break;
		case "sweep_head_b_r":
			this.isAttackMoveByCore = true;
			this.attackCheckTimeA = 0.4f;
			this.attackCheckTimeB = 0.51f;
			this.checkHitCapsuleStart = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
			this.checkHitCapsuleEnd = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
			this.checkHitCapsuleR = 4f;
			break;
		}
		this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.transform.position;
		this.needFreshCorePosition = true;
	}

	private bool attackTarget(GameObject target)
	{
		float num = 0f;
		Vector3 vector = target.transform.position - base.transform.position;
		num = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, base.gameObject.transform.rotation.eulerAngles.y - 90f);
		if (this.eren != null && this.myDistance < 35f)
		{
			this.attack("combo_1");
			return true;
		}
		int num2 = 0;
		string text = string.Empty;
		ArrayList arrayList = new ArrayList();
		if (this.myDistance < 40f)
		{
			num2 = ((Mathf.Abs(num) < 90f) ? ((num > 0f) ? 1 : 2) : ((!(num > 0f)) ? 3 : 4));
			float num3 = target.transform.position.y - base.transform.position.y;
			if (Mathf.Abs(num) < 90f)
			{
				if (num3 > 0f && num3 < 12f && this.myDistance < 22f)
				{
					arrayList.Add("attack_sweep");
				}
				if (num3 >= 55f && num3 < 90f)
				{
					arrayList.Add("attack_jumpCombo_1");
				}
			}
			if (Mathf.Abs(num) < 90f && num3 > 12f && num3 < 40f)
			{
				arrayList.Add("attack_combo_1");
			}
			if (Mathf.Abs(num) < 30f)
			{
				if (num3 > 0f && num3 < 12f && this.myDistance > 20f && this.myDistance < 30f)
				{
					arrayList.Add("attack_front");
				}
				if (this.myDistance < 12f && num3 > 33f && num3 < 51f)
				{
					arrayList.Add("grab_up");
				}
			}
			if (Mathf.Abs(num) > 100f && this.myDistance < 11f && num3 >= 15f && num3 < 32f)
			{
				arrayList.Add("attack_sweep_back");
			}
			switch (num2)
			{
			case 1:
				if (this.myDistance >= 11f)
				{
					if (this.myDistance < 20f)
					{
						if (num3 >= 12f && num3 < 21f)
						{
							arrayList.Add("grab_bottom_right");
						}
						else if (num3 >= 21f && num3 < 32f)
						{
							arrayList.Add("grab_mid_right");
						}
						else if (num3 >= 32f && num3 < 47f)
						{
							arrayList.Add("grab_up_right");
						}
					}
				}
				else if (num3 >= 21f && num3 < 32f)
				{
					arrayList.Add("attack_sweep_front_right");
				}
				break;
			case 2:
				if (this.myDistance >= 11f)
				{
					if (this.myDistance < 20f)
					{
						if (num3 >= 12f && num3 < 21f)
						{
							arrayList.Add("grab_bottom_left");
						}
						else if (num3 >= 21f && num3 < 32f)
						{
							arrayList.Add("grab_mid_left");
						}
						else if (num3 >= 32f && num3 < 47f)
						{
							arrayList.Add("grab_up_left");
						}
					}
				}
				else if (num3 >= 21f && num3 < 32f)
				{
					arrayList.Add("attack_sweep_front_left");
				}
				break;
			case 3:
				if (this.myDistance >= 11f)
				{
					arrayList.Add("turn180");
				}
				else if (num3 >= 33f && num3 < 51f)
				{
					arrayList.Add("attack_sweep_head_b_l");
				}
				break;
			case 4:
				if (this.myDistance >= 11f)
				{
					arrayList.Add("turn180");
				}
				else if (num3 >= 33f && num3 < 51f)
				{
					arrayList.Add("attack_sweep_head_b_r");
				}
				break;
			}
		}
		if (arrayList.Count > 0)
		{
			text = (string)arrayList[UnityEngine.Random.Range(0, arrayList.Count)];
		}
		else if (UnityEngine.Random.Range(0, 100) < 10)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
			this.myHero = array[UnityEngine.Random.Range(0, array.Length)];
			this.attention = UnityEngine.Random.Range(5f, 10f);
			return true;
		}
		switch (text)
		{
		case "grab_bottom_left":
			this.grab("bottom_left");
			return true;
		case "grab_bottom_right":
			this.grab("bottom_right");
			return true;
		case "grab_mid_left":
			this.grab("mid_left");
			return true;
		case "grab_mid_right":
			this.grab("mid_right");
			return true;
		case "grab_up":
			this.grab("up");
			return true;
		case "grab_up_left":
			this.grab("up_left");
			return true;
		case "grab_up_right":
			this.grab("up_right");
			return true;
		case "attack_combo_1":
			this.attack("combo_1");
			return true;
		case "attack_front":
			this.attack("front");
			return true;
		case "attack_jumpCombo_1":
			this.attack("jumpCombo_1");
			return true;
		case "attack_sweep":
			this.attack("sweep");
			return true;
		case "attack_sweep_back":
			this.attack("sweep_back");
			return true;
		case "attack_sweep_front_left":
			this.attack("sweep_front_left");
			return true;
		case "attack_sweep_front_right":
			this.attack("sweep_front_right");
			return true;
		case "attack_sweep_head_b_l":
			this.attack("sweep_head_b_l");
			return true;
		case "attack_sweep_head_b_r":
			this.attack("sweep_head_b_r");
			return true;
		case "turn180":
			this.turn180();
			return true;
		default:
			return false;
		}
	}

	private void Awake()
	{
		base.rigidbody.freezeRotation = true;
		base.rigidbody.useGravity = false;
		this._customSkinLoader = base.gameObject.AddComponent<AnnieCustomSkinLoader>();
	}

	public void beTauntedBy(GameObject target, float tauntTime)
	{
		this.whoHasTauntMe = target;
		this.tauntTime = tauntTime;
	}

	private void chase()
	{
		this.state = "chase";
		this.crossFade("run", 0.5f);
	}

	private RaycastHit[] checkHitCapsule(Vector3 start, Vector3 end, float r)
	{
		return Physics.SphereCastAll(start, r, end - start, Vector3.Distance(start, end));
	}

	private GameObject checkIfHitHand(Transform hand)
	{
		float num = 9.6f;
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

	private GameObject checkIfHitHead(Transform head, float rad)
	{
		float num = rad * 4f;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<TITAN_EREN>() == null && !gameObject.GetComponent<HERO>().isInvincible())
			{
				float num2 = gameObject.GetComponent<CapsuleCollider>().height * 0.5f;
				if (Vector3.Distance(gameObject.transform.position + Vector3.up * num2, head.transform.position + Vector3.up * 1.5f * 4f) < num + num2)
				{
					return gameObject;
				}
			}
		}
		return null;
	}

	private void crossFade(string aniName, float time)
	{
		base.animation.CrossFade(aniName, time);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[2] { aniName, time };
			base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
		}
	}

	private void eatSet(GameObject grabTarget)
	{
		if (!grabTarget.GetComponent<HERO>().isGrabbed)
		{
			this.grabToRight();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
			{
				object[] parameters = new object[2]
				{
					base.photonView.viewID,
					false
				};
				grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, parameters);
				object[] parameters2 = new object[1] { "grabbed" };
				grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters2);
				base.photonView.RPC("grabToRight", PhotonTargets.Others);
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
		if (!grabTarget.GetComponent<HERO>().isGrabbed)
		{
			this.grabToLeft();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
			{
				object[] parameters = new object[2]
				{
					base.photonView.viewID,
					true
				};
				grabTarget.GetPhotonView().RPC("netGrabbed", PhotonTargets.All, parameters);
				object[] parameters2 = new object[1] { "grabbed" };
				grabTarget.GetPhotonView().RPC("netPlayAnimation", PhotonTargets.All, parameters2);
				base.photonView.RPC("grabToLeft", PhotonTargets.Others);
			}
			else
			{
				grabTarget.GetComponent<HERO>().grabbed(base.gameObject, leftHand: true);
				grabTarget.GetComponent<HERO>().animation.Play("grabbed");
			}
		}
	}

	public void erenIsHere(GameObject target)
	{
		this.myHero = (this.eren = target);
	}

	private void findNearestFacingHero()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		Vector3 position = base.transform.position;
		float num2 = 180f;
		GameObject[] array2 = array;
		foreach (GameObject gameObject2 in array2)
		{
			float sqrMagnitude = (gameObject2.transform.position - position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				Vector3 vector = gameObject2.transform.position - base.transform.position;
				if (Mathf.Abs(0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, base.gameObject.transform.rotation.eulerAngles.y - 90f)) < num2)
				{
					gameObject = gameObject2;
					num = sqrMagnitude;
				}
			}
		}
		if (gameObject != null)
		{
			this.myHero = gameObject;
			this.tauntTime = 5f;
		}
	}

	private void findNearestHero()
	{
		this.myHero = this.getNearestHero();
		this.attention = UnityEngine.Random.Range(5f, 10f);
	}

	private void FixedUpdate()
	{
		if ((GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine))
		{
			return;
		}
		if (this.bottomObject.GetComponent<CheckHitGround>().isGrounded)
		{
			this.grounded = true;
			this.bottomObject.GetComponent<CheckHitGround>().isGrounded = false;
		}
		else
		{
			this.grounded = false;
		}
		if (this.needFreshCorePosition)
		{
			this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
			this.needFreshCorePosition = false;
		}
		if ((this.state == "attack" && this.isAttackMoveByCore) || this.state == "hit" || this.state == "turn180" || this.state == "anklehurt")
		{
			Vector3 vector = base.transform.position - base.transform.Find("Amarture/Core").position - this.oldCorePosition;
			base.rigidbody.velocity = vector / Time.deltaTime + Vector3.up * base.rigidbody.velocity.y;
			this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
		}
		else if (this.state == "chase")
		{
			if (this.myHero == null)
			{
				return;
			}
			Vector3 vector2 = base.transform.forward * this.speed;
			Vector3 velocity = base.rigidbody.velocity;
			Vector3 force = vector2 - velocity;
			force.y = 0f;
			base.rigidbody.AddForce(force, ForceMode.VelocityChange);
			Vector3 vector3 = this.myHero.transform.position - base.transform.position;
			float num = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector3.z, vector3.x)) * 57.29578f, base.gameObject.transform.rotation.eulerAngles.y - 90f);
			base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, base.gameObject.transform.rotation.eulerAngles.y + num, 0f), this.speed * Time.deltaTime);
		}
		else if (this.grounded && !base.animation.IsPlaying("attack_jumpCombo_1"))
		{
			base.rigidbody.AddForce(new Vector3(0f - base.rigidbody.velocity.x, 0f, 0f - base.rigidbody.velocity.z), ForceMode.VelocityChange);
		}
		base.rigidbody.AddForce(new Vector3(0f, (0f - this.gravity) * base.rigidbody.mass, 0f));
	}

	private void getDown()
	{
		this.state = "anklehurt";
		this.playAnimation("legHurt");
		this.AnkleRHP = this.AnkleRHPMAX;
		this.AnkleLHP = this.AnkleLHPMAX;
		this.needFreshCorePosition = true;
	}

	private GameObject getNearestHero()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		GameObject result = null;
		float num = float.PositiveInfinity;
		Vector3 position = base.transform.position;
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			if ((gameObject.GetComponent<HERO>() == null || !gameObject.GetComponent<HERO>().HasDied()) && (gameObject.GetComponent<TITAN_EREN>() == null || !gameObject.GetComponent<TITAN_EREN>().hasDied))
			{
				float sqrMagnitude = (gameObject.transform.position - position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = gameObject;
					num = sqrMagnitude;
				}
			}
		}
		return result;
	}

	private float getNearestHeroDistance()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		float num = float.PositiveInfinity;
		Vector3 position = base.transform.position;
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			float magnitude = (array2[i].transform.position - position).magnitude;
			if (magnitude < num)
			{
				num = magnitude;
			}
		}
		return num;
	}

	private void grab(string type)
	{
		this.state = "grab";
		this.attacked = false;
		this.attackAnimation = type;
		if (base.animation.IsPlaying("attack_grab_" + type))
		{
			base.animation["attack_grab_" + type].normalizedTime = 0f;
			this.playAnimation("attack_grab_" + type);
		}
		else
		{
			this.crossFade("attack_grab_" + type, 0.1f);
		}
		this.isGrabHandLeft = true;
		this.grabbedTarget = null;
		this.attackCheckTime = 0f;
		switch (type)
		{
		case "bottom_left":
			this.attackCheckTimeA = 0.28f;
			this.attackCheckTimeB = 0.38f;
			this.attackCheckTime = 0.65f;
			this.isGrabHandLeft = false;
			break;
		case "bottom_right":
			this.attackCheckTimeA = 0.27f;
			this.attackCheckTimeB = 0.37f;
			this.attackCheckTime = 0.65f;
			break;
		case "mid_left":
			this.attackCheckTimeA = 0.27f;
			this.attackCheckTimeB = 0.37f;
			this.attackCheckTime = 0.65f;
			this.isGrabHandLeft = false;
			break;
		case "mid_right":
			this.attackCheckTimeA = 0.27f;
			this.attackCheckTimeB = 0.36f;
			this.attackCheckTime = 0.66f;
			break;
		case "up":
			this.attackCheckTimeA = 0.25f;
			this.attackCheckTimeB = 0.32f;
			this.attackCheckTime = 0.67f;
			break;
		case "up_left":
			this.attackCheckTimeA = 0.26f;
			this.attackCheckTimeB = 0.4f;
			this.attackCheckTime = 0.66f;
			break;
		case "up_right":
			this.attackCheckTimeA = 0.26f;
			this.attackCheckTimeB = 0.4f;
			this.attackCheckTime = 0.66f;
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
	public void grabbedTargetEscape()
	{
		this.grabbedTarget = null;
	}

	[RPC]
	public void grabToLeft()
	{
		Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L/hand_L_001");
		this.grabTF.transform.parent = transform;
		this.grabTF.transform.parent = transform;
		this.grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
		this.grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
		this.grabTF.transform.localPosition -= Vector3.right * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localPosition -= Vector3.up * transform.GetComponent<SphereCollider>().radius * 0.51f;
		this.grabTF.transform.localPosition -= Vector3.forward * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localRotation = Quaternion.Euler(this.grabTF.transform.localRotation.eulerAngles.x, this.grabTF.transform.localRotation.eulerAngles.y + 180f, this.grabTF.transform.localRotation.eulerAngles.z + 180f);
	}

	[RPC]
	public void grabToRight()
	{
		Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
		this.grabTF.transform.parent = transform;
		this.grabTF.transform.position = transform.GetComponent<SphereCollider>().transform.position;
		this.grabTF.transform.rotation = transform.GetComponent<SphereCollider>().transform.rotation;
		this.grabTF.transform.localPosition -= Vector3.right * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localPosition += Vector3.up * transform.GetComponent<SphereCollider>().radius * 0.51f;
		this.grabTF.transform.localPosition -= Vector3.forward * transform.GetComponent<SphereCollider>().radius * 0.3f;
		this.grabTF.transform.localRotation = Quaternion.Euler(this.grabTF.transform.localRotation.eulerAngles.x, this.grabTF.transform.localRotation.eulerAngles.y + 180f, this.grabTF.transform.localRotation.eulerAngles.z);
	}

	public void hit(int dmg)
	{
		this.NapeArmor -= dmg;
		if (this.NapeArmor <= 0)
		{
			this.NapeArmor = 0;
		}
	}

	public void hitAnkleL(int dmg)
	{
		if (!this.hasDie && this.state != "anklehurt")
		{
			this.AnkleLHP -= dmg;
			if (this.AnkleLHP <= 0)
			{
				this.getDown();
			}
		}
	}

	[RPC]
	public void hitAnkleLRPC(int viewID, int dmg)
	{
		if (this.hasDie || !(this.state != "anklehurt"))
		{
			return;
		}
		PhotonView photonView = PhotonView.Find(viewID);
		if (!(photonView != null))
		{
			return;
		}
		if (this.grabbedTarget != null)
		{
			this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
		}
		if ((photonView.gameObject.transform.position - base.transform.Find("Amarture/Core/Controller_Body").transform.position).magnitude < 20f)
		{
			this.AnkleLHP -= dmg;
			if (this.AnkleLHP <= 0)
			{
				this.getDown();
			}
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(t1: false, (string)photonView.owner.customProperties[PhotonPlayerProperty.name], t2: true, "Female Titan's ankle", dmg);
			object[] parameters = new object[1] { dmg };
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, parameters);
		}
	}

	public void hitAnkleR(int dmg)
	{
		if (!this.hasDie && this.state != "anklehurt")
		{
			this.AnkleRHP -= dmg;
			if (this.AnkleRHP <= 0)
			{
				this.getDown();
			}
		}
	}

	[RPC]
	public void hitAnkleRRPC(int viewID, int dmg)
	{
		if (this.hasDie || !(this.state != "anklehurt"))
		{
			return;
		}
		PhotonView photonView = PhotonView.Find(viewID);
		if (!(photonView != null))
		{
			return;
		}
		if (this.grabbedTarget != null)
		{
			this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
		}
		if ((photonView.gameObject.transform.position - base.transform.Find("Amarture/Core/Controller_Body").transform.position).magnitude < 20f)
		{
			this.AnkleRHP -= dmg;
			if (this.AnkleRHP <= 0)
			{
				this.getDown();
			}
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(t1: false, (string)photonView.owner.customProperties[PhotonPlayerProperty.name], t2: true, "Female Titan's ankle", dmg);
			object[] parameters = new object[1] { dmg };
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, parameters);
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
		if (!this.hasDie)
		{
			if (this.grabbedTarget != null)
			{
				this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
			}
			Transform transform = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
			PhotonView photonView = PhotonView.Find(viewID);
			if (photonView != null && (photonView.gameObject.transform.position - transform.transform.position).magnitude < 20f)
			{
				this.justHitEye();
			}
		}
	}

	private void idle(float sbtime = 0f)
	{
		this.sbtime = sbtime;
		this.sbtime = Mathf.Max(0.5f, this.sbtime);
		this.state = "idle";
		this.crossFade("idle", 0.2f);
	}

	public bool IsGrounded()
	{
		return this.bottomObject.GetComponent<CheckHitGround>().isGrounded;
	}

	private void justEatHero(GameObject target, Transform hand)
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			if (!target.GetComponent<HERO>().HasDied())
			{
				target.GetComponent<HERO>().markDie();
				object[] parameters = new object[2] { -1, "Female Titan" };
				target.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, parameters);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			target.GetComponent<HERO>().die2(hand);
		}
	}

	private void justHitEye()
	{
		this.attack("combo_blind_1");
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
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient && !hitHero.GetComponent<HERO>().HasDied())
		{
			hitHero.GetComponent<HERO>().markDie();
			object[] parameters = new object[5]
			{
				(hitHero.transform.position - position) * 15f * 4f,
				false,
				-1,
				"Female Titan",
				true
			};
			hitHero.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters);
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
			this.healthLabel.transform.localPosition = new Vector3(0f, 52f, 0f);
			float num = 4f;
			if (this.size > 0f && this.size < 1f)
			{
				num = 4f / this.size;
				num = Mathf.Min(num, 15f);
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

	public void lateUpdate()
	{
		if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return;
		}
		if (base.animation.IsPlaying("run"))
		{
			if (base.animation["run"].normalizedTime % 1f > 0.1f && base.animation["run"].normalizedTime % 1f < 0.6f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				Transform obj = base.transform.Find("snd_titan_foot");
				obj.GetComponent<AudioSource>().Stop();
				obj.GetComponent<AudioSource>().Play();
			}
			if (base.animation["run"].normalizedTime % 1f > 0.6f && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				Transform obj2 = base.transform.Find("snd_titan_foot");
				obj2.GetComponent<AudioSource>().Stop();
				obj2.GetComponent<AudioSource>().Play();
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0)
		{
			_ = base.photonView.isMine;
		}
	}

	public void lateUpdate2()
	{
		if (GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			return;
		}
		if (base.animation.IsPlaying("run"))
		{
			if (base.animation["run"].normalizedTime % 1f > 0.1f && base.animation["run"].normalizedTime % 1f < 0.6f && this.stepSoundPhase == 2)
			{
				this.stepSoundPhase = 1;
				Transform obj = base.transform.Find("snd_titan_foot");
				obj.GetComponent<AudioSource>().Stop();
				obj.GetComponent<AudioSource>().Play();
			}
			if (base.animation["run"].normalizedTime % 1f > 0.6f && this.stepSoundPhase == 1)
			{
				this.stepSoundPhase = 2;
				Transform obj2 = base.transform.Find("snd_titan_foot");
				obj2.GetComponent<AudioSource>().Stop();
				obj2.GetComponent<AudioSource>().Play();
			}
		}
		this.updateLabel();
		this.healthTime -= Time.deltaTime;
	}

	public void loadskin()
	{
		BaseCustomSkinSettings<ShifterCustomSkinSet> shifter = SettingsManager.CustomSkinSettings.Shifter;
		string value = ((ShifterCustomSkinSet)shifter.GetSelectedSet()).Annie.Value;
		if (shifter.SkinsEnabled.Value && TextureDownloader.ValidTextureURL(value))
		{
			base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, value);
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
			this.crossFade("die", 0.05f);
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
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeFT(this);
		}
	}

	private void playAnimation(string aniName)
	{
		base.animation.Play(aniName);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[1] { aniName };
			base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
		}
	}

	private void playAnimationAt(string aniName, float normalizedTime)
	{
		base.animation.Play(aniName);
		base.animation[aniName].normalizedTime = normalizedTime;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[2] { aniName, normalizedTime };
			base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
		}
	}

	private void playSound(string sndname)
	{
		this.playsoundRPC(sndname);
		if (Network.peerType == NetworkPeerType.Server)
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

	[RPC]
	public void setSize(float size, PhotonMessageInfo info)
	{
		size = Mathf.Clamp(size, 0.2f, 30f);
		if (info.sender.isMasterClient)
		{
			base.transform.localScale *= size * 0.25f;
			this.size = size;
		}
	}

	private void Start()
	{
		this.startMain();
		this.size = 4f;
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
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addFT(this);
		base.name = "Female Titan";
		this.grabTF = new GameObject();
		this.grabTF.name = "titansTmpGrabTF";
		this.currentCamera = GameObject.Find("MainCamera");
		this.oldCorePosition = base.transform.position - base.transform.Find("Amarture/Core").position;
		if (this.myHero == null)
		{
			this.findNearestHero();
		}
		foreach (AnimationState item in base.animation)
		{
			item.speed = 0.7f;
		}
		base.animation["turn180"].speed = 0.5f;
		this.NapeArmor = 1000;
		this.AnkleLHP = 50;
		this.AnkleRHP = 50;
		this.AnkleLHPMAX = 50;
		this.AnkleRHPMAX = 50;
		bool flag = false;
		if (LevelInfo.getInfo(FengGameManagerMKII.level).respawnMode == RespawnMode.NEVER)
		{
			flag = true;
		}
		if (IN_GAME_MAIN_CAMERA.difficulty == 0)
		{
			this.NapeArmor = ((!flag) ? 1000 : 1000);
			this.AnkleLHP = (this.AnkleLHPMAX = ((!flag) ? 50 : 50));
			this.AnkleRHP = (this.AnkleRHPMAX = ((!flag) ? 50 : 50));
		}
		else if (IN_GAME_MAIN_CAMERA.difficulty == 1)
		{
			this.NapeArmor = ((!flag) ? 3000 : 2500);
			this.AnkleLHP = (this.AnkleLHPMAX = ((!flag) ? 200 : 100));
			this.AnkleRHP = (this.AnkleRHPMAX = ((!flag) ? 200 : 100));
			foreach (AnimationState item2 in base.animation)
			{
				item2.speed = 0.7f;
			}
			base.animation["turn180"].speed = 0.7f;
		}
		else if (IN_GAME_MAIN_CAMERA.difficulty == 2)
		{
			this.NapeArmor = ((!flag) ? 6000 : 4000);
			this.AnkleLHP = (this.AnkleLHPMAX = ((!flag) ? 1000 : 200));
			this.AnkleRHP = (this.AnkleRHPMAX = ((!flag) ? 1000 : 200));
			foreach (AnimationState item3 in base.animation)
			{
				item3.speed = 1f;
			}
			base.animation["turn180"].speed = 0.9f;
		}
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			this.NapeArmor = (int)((float)this.NapeArmor * 0.8f);
		}
		base.animation["legHurt"].speed = 1f;
		base.animation["legHurt_loop"].speed = 1f;
		base.animation["legHurt_getup"].speed = 1f;
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
		if (this.NapeArmor <= 0)
		{
			this.NapeArmor = 0;
			if (!this.hasDie)
			{
				base.photonView.RPC("netDie", PhotonTargets.OthersBuffered);
				if (this.grabbedTarget != null)
				{
					this.grabbedTarget.GetPhotonView().RPC("netUngrabbed", PhotonTargets.All);
				}
				this.netDie();
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().titanGetKill(photonView.owner, speed, base.name);
			}
		}
		else
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().sendKillInfo(t1: false, (string)photonView.owner.customProperties[PhotonPlayerProperty.name], t2: true, "Female Titan's neck", speed);
			object[] parameters = new object[1] { speed };
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("netShowDamage", photonView.owner, parameters);
		}
		this.healthTime = 0.2f;
	}

	private void turn(float d)
	{
		if (d > 0f)
		{
			this.turnAnimation = "turnaround1";
		}
		else
		{
			this.turnAnimation = "turnaround2";
		}
		this.playAnimation(this.turnAnimation);
		base.animation[this.turnAnimation].time = 0f;
		d = Mathf.Clamp(d, -120f, 120f);
		this.turnDeg = d;
		this.desDeg = base.gameObject.transform.rotation.eulerAngles.y + this.turnDeg;
		this.state = "turn";
	}

	private void turn180()
	{
		this.turnAnimation = "turn180";
		this.playAnimation(this.turnAnimation);
		base.animation[this.turnAnimation].time = 0f;
		this.state = "turn180";
		this.needFreshCorePosition = true;
	}

	public void update()
	{
		if ((GameMenu.Paused && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine))
		{
			return;
		}
		if (this.hasDie)
		{
			this.dieTime += Time.deltaTime;
			if (base.animation["die"].normalizedTime >= 1f)
			{
				this.playAnimation("die_cry");
				if (IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE)
				{
					for (int i = 0; i < 15; i++)
					{
						GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().randomSpawnOneTitan("titanRespawn", 50)
							.GetComponent<TITAN>()
							.beTauntedBy(base.gameObject, 20f);
					}
				}
			}
			if (this.dieTime > 2f && !this.hasDieSteam)
			{
				this.hasDieSteam = true;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					GameObject obj = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie1"));
					obj.transform.position = base.transform.Find("Amarture/Core/Controller_Body/hip").position;
					obj.transform.localScale = base.transform.localScale;
				}
				else if (base.photonView.isMine)
				{
					PhotonNetwork.Instantiate("FX/FXtitanDie1", base.transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = base.transform.localScale;
				}
			}
			if (this.dieTime > ((IN_GAME_MAIN_CAMERA.gamemode != GAMEMODE.PVP_CAPTURE) ? 20f : 5f))
			{
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/FXtitanDie"));
					obj2.transform.position = base.transform.Find("Amarture/Core/Controller_Body/hip").position;
					obj2.transform.localScale = base.transform.localScale;
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else if (base.photonView.isMine)
				{
					PhotonNetwork.Instantiate("FX/FXtitanDie", base.transform.Find("Amarture/Core/Controller_Body/hip").position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = base.transform.localScale;
					PhotonNetwork.Destroy(base.gameObject);
				}
			}
			return;
		}
		if (this.attention > 0f)
		{
			this.attention -= Time.deltaTime;
			if (this.attention < 0f)
			{
				this.attention = 0f;
				GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
				this.myHero = array[UnityEngine.Random.Range(0, array.Length)];
				this.attention = UnityEngine.Random.Range(5f, 10f);
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
		}
		if (this.eren != null)
		{
			if (!this.eren.GetComponent<TITAN_EREN>().hasDied)
			{
				this.myHero = this.eren;
			}
			else
			{
				this.eren = null;
				this.myHero = null;
			}
		}
		if (this.myHero == null)
		{
			this.findNearestHero();
			if (this.myHero != null)
			{
				return;
			}
		}
		if (this.myHero == null)
		{
			this.myDistance = float.MaxValue;
		}
		else
		{
			this.myDistance = Mathf.Sqrt((this.myHero.transform.position.x - base.transform.position.x) * (this.myHero.transform.position.x - base.transform.position.x) + (this.myHero.transform.position.z - base.transform.position.z) * (this.myHero.transform.position.z - base.transform.position.z));
		}
		if (this.state == "idle")
		{
			if (!(this.myHero != null))
			{
				return;
			}
			float num = 0f;
			Vector3 vector = this.myHero.transform.position - base.transform.position;
			num = 0f - Mathf.DeltaAngle((0f - Mathf.Atan2(vector.z, vector.x)) * 57.29578f, base.gameObject.transform.rotation.eulerAngles.y - 90f);
			if (this.attackTarget(this.myHero))
			{
				return;
			}
			if (Mathf.Abs(num) < 90f)
			{
				this.chase();
			}
			else if (UnityEngine.Random.Range(0, 100) < 1)
			{
				this.turn180();
			}
			else if (Mathf.Abs(num) > 100f)
			{
				if (UnityEngine.Random.Range(0, 100) < 10)
				{
					this.turn180();
				}
			}
			else if (Mathf.Abs(num) > 45f && UnityEngine.Random.Range(0, 100) < 30)
			{
				this.turn(num);
			}
		}
		else if (this.state == "attack")
		{
			if (!this.attacked && this.attackCheckTime != 0f && base.animation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTime)
			{
				this.attacked = true;
				this.fxPosition = base.transform.Find("ap_" + this.attackAnimation).position;
				GameObject gameObject = ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !PhotonNetwork.isMasterClient) ? ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/" + this.fxName), this.fxPosition, this.fxRotation)) : PhotonNetwork.Instantiate("FX/" + this.fxName, this.fxPosition, this.fxRotation, 0));
				gameObject.transform.localScale = base.transform.localScale;
				float b = 1f - Vector3.Distance(this.currentCamera.transform.position, gameObject.transform.position) * 0.05f;
				b = Mathf.Min(1f, b);
				this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b, b);
			}
			if (this.attackCheckTimeA != 0f && ((base.animation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA && base.animation["attack_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB) || (!this.attackChkOnce && base.animation["attack_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA)))
			{
				if (!this.attackChkOnce)
				{
					this.attackChkOnce = true;
					this.playSound("snd_eren_swing" + UnityEngine.Random.Range(1, 3));
				}
				RaycastHit[] array2 = this.checkHitCapsule(this.checkHitCapsuleStart.position, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR);
				foreach (RaycastHit raycastHit in array2)
				{
					GameObject gameObject2 = raycastHit.collider.gameObject;
					if (gameObject2.tag == "Player")
					{
						this.killPlayer(gameObject2);
					}
					if (!(gameObject2.tag == "erenHitbox"))
					{
						continue;
					}
					if (this.attackAnimation == "combo_1")
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
						{
							gameObject2.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(1);
						}
					}
					else if (this.attackAnimation == "combo_2")
					{
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
						{
							gameObject2.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(2);
						}
					}
					else if (this.attackAnimation == "combo_3" && IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
					{
						gameObject2.transform.root.gameObject.GetComponent<TITAN_EREN>().hitByFTByServer(3);
					}
				}
				array2 = this.checkHitCapsule(this.checkHitCapsuleEndOld, this.checkHitCapsuleEnd.position, this.checkHitCapsuleR);
				foreach (RaycastHit raycastHit2 in array2)
				{
					GameObject gameObject3 = raycastHit2.collider.gameObject;
					if (gameObject3.tag == "Player")
					{
						this.killPlayer(gameObject3);
					}
				}
				this.checkHitCapsuleEndOld = this.checkHitCapsuleEnd.position;
			}
			if (this.attackAnimation == "jumpCombo_1" && base.animation["attack_" + this.attackAnimation].normalizedTime >= 0.65f && !this.startJump && this.myHero != null)
			{
				this.startJump = true;
				float y = this.myHero.rigidbody.velocity.y;
				float num2 = -20f;
				float num3 = this.gravity;
				float y2 = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position.y;
				float num4 = (num2 - num3) * 0.5f;
				float num5 = y;
				float num6 = this.myHero.transform.position.y - y2;
				float num7 = Mathf.Abs((Mathf.Sqrt(num5 * num5 - 4f * num4 * num6) - num5) / (2f * num4));
				Vector3 vector2 = this.myHero.transform.position + this.myHero.rigidbody.velocity * num7 + Vector3.up * 0.5f * num2 * num7 * num7;
				float y3 = vector2.y;
				if (num6 < 0f || y3 - y2 < 0f)
				{
					this.idle();
					num7 = 0.5f;
					vector2 = base.transform.position + (y2 + 5f) * Vector3.up;
					y3 = vector2.y;
				}
				float num8 = y3 - y2;
				float num9 = Mathf.Sqrt(2f * num8 / this.gravity);
				float value = this.gravity * num9 + 20f;
				value = Mathf.Clamp(value, 20f, 90f);
				Vector3 vector3 = (vector2 - base.transform.position) / num7;
				this.abnorma_jump_bite_horizon_v = new Vector3(vector3.x, 0f, vector3.z);
				Vector3 velocity = base.rigidbody.velocity;
				Vector3 vector4 = new Vector3(this.abnorma_jump_bite_horizon_v.x, value, this.abnorma_jump_bite_horizon_v.z);
				if (vector4.magnitude > 90f)
				{
					vector4 = vector4.normalized * 90f;
				}
				Vector3 force = vector4 - velocity;
				base.rigidbody.AddForce(force, ForceMode.VelocityChange);
				float num10 = Vector2.Angle(new Vector2(base.transform.position.x, base.transform.position.z), new Vector2(this.myHero.transform.position.x, this.myHero.transform.position.z));
				num10 = Mathf.Atan2(this.myHero.transform.position.x - base.transform.position.x, this.myHero.transform.position.z - base.transform.position.z) * 57.29578f;
				base.gameObject.transform.rotation = Quaternion.Euler(0f, num10, 0f);
			}
			if (this.attackAnimation == "jumpCombo_3")
			{
				if (base.animation["attack_" + this.attackAnimation].normalizedTime >= 1f && this.IsGrounded())
				{
					this.attack("jumpCombo_4");
				}
			}
			else
			{
				if (!(base.animation["attack_" + this.attackAnimation].normalizedTime >= 1f))
				{
					return;
				}
				if (this.nextAttackAnimation != null)
				{
					this.attack(this.nextAttackAnimation);
					if (this.eren != null)
					{
						base.gameObject.transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(this.eren.transform.position - base.transform.position).eulerAngles.y, 0f);
					}
				}
				else
				{
					this.findNearestHero();
					this.idle();
				}
			}
		}
		else if (this.state == "grab")
		{
			if (base.animation["attack_grab_" + this.attackAnimation].normalizedTime >= this.attackCheckTimeA && base.animation["attack_grab_" + this.attackAnimation].normalizedTime <= this.attackCheckTimeB && this.grabbedTarget == null)
			{
				GameObject gameObject4 = this.checkIfHitHand(this.currentGrabHand);
				if (gameObject4 != null)
				{
					if (this.isGrabHandLeft)
					{
						this.eatSetL(gameObject4);
						this.grabbedTarget = gameObject4;
					}
					else
					{
						this.eatSet(gameObject4);
						this.grabbedTarget = gameObject4;
					}
				}
			}
			if (base.animation["attack_grab_" + this.attackAnimation].normalizedTime > this.attackCheckTime && this.grabbedTarget != null)
			{
				this.justEatHero(this.grabbedTarget, this.currentGrabHand);
				this.grabbedTarget = null;
			}
			if (base.animation["attack_grab_" + this.attackAnimation].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
		else if (this.state == "turn")
		{
			base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.desDeg, 0f), Time.deltaTime * Mathf.Abs(this.turnDeg) * 0.1f);
			if (base.animation[this.turnAnimation].normalizedTime >= 1f)
			{
				this.idle();
			}
		}
		else if (this.state == "chase")
		{
			if ((this.eren == null || this.myDistance >= 35f || !this.attackTarget(this.myHero)) && (this.getNearestHeroDistance() >= 50f || UnityEngine.Random.Range(0, 100) >= 20 || !this.attackTarget(this.getNearestHero())) && this.myDistance < this.attackDistance - 15f)
			{
				this.idle(UnityEngine.Random.Range(0.05f, 0.2f));
			}
		}
		else if (this.state == "turn180")
		{
			if (base.animation[this.turnAnimation].normalizedTime >= 1f)
			{
				base.gameObject.transform.rotation = Quaternion.Euler(base.gameObject.transform.rotation.eulerAngles.x, base.gameObject.transform.rotation.eulerAngles.y + 180f, base.gameObject.transform.rotation.eulerAngles.z);
				this.idle();
				this.playAnimation("idle");
			}
		}
		else if (this.state == "anklehurt")
		{
			if (base.animation["legHurt"].normalizedTime >= 1f)
			{
				this.crossFade("legHurt_loop", 0.2f);
			}
			if (base.animation["legHurt_loop"].normalizedTime >= 3f)
			{
				this.crossFade("legHurt_getup", 0.2f);
			}
			if (base.animation["legHurt_getup"].normalizedTime >= 1f)
			{
				this.idle();
				this.playAnimation("idle");
			}
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
