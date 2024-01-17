using System.Collections;
using System.Collections.Generic;
using Constants;
using GameProgress;
using Photon;
using Settings;
using UnityEngine;

internal class Bomb : Photon.MonoBehaviour
{
	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	private Vector3 correctPlayerVelocity = Vector3.zero;

	public bool Disabled;

	public float SmoothingDelay = 10f;

	public float BombRadius;

	private TITAN _collidedTitan;

	private SphereCollider _sphereCollider;

	private List<GameObject> _hideUponDestroy;

	private ParticleSystem _trail;

	private ParticleSystem _flame;

	private float _DisabledTrailFadeMultiplier = 0.6f;

	private HERO _owner;

	private bool _receivedNonZeroVelocity;

	private bool _ownerIsUpdated;

	public void Setup(HERO owner, float bombRadius)
	{
		this._owner = owner;
		this.BombRadius = bombRadius;
	}

	public void Awake()
	{
		if (base.photonView != null)
		{
			base.photonView.observed = this;
			this.correctPlayerPos = base.transform.position;
			this.correctPlayerRot = base.transform.rotation;
			PhotonPlayer owner = base.photonView.owner;
			this._trail = base.transform.Find("Trail").GetComponent<ParticleSystem>();
			this._flame = base.transform.Find("Flame").GetComponent<ParticleSystem>();
			this._sphereCollider = base.GetComponent<SphereCollider>();
			this._hideUponDestroy = new List<GameObject>();
			this._hideUponDestroy.Add(base.transform.Find("Flame").gameObject);
			this._hideUponDestroy.Add(base.transform.Find("ThunderSpearModel").gameObject);
			if (SettingsManager.AbilitySettings.ShowBombColors.Value)
			{
				Color bombColor = BombUtil.GetBombColor(owner, 1f);
				this._trail.startColor = bombColor;
				this._flame.startColor = bombColor;
			}
			if (base.photonView.isMine)
			{
				base.photonView.RPC("IsUpdatedRPC", PhotonTargets.All);
			}
		}
	}

	[RPC]
	private void IsUpdatedRPC(PhotonMessageInfo info)
	{
		if (info.sender == base.photonView.owner)
		{
			this._ownerIsUpdated = true;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (base.photonView.isMine && !this.Disabled)
		{
			this.Explode(this.BombRadius);
		}
	}

	public void DestroySelf()
	{
		if (base.photonView.isMine && !this.Disabled)
		{
			base.photonView.RPC("DisableRPC", PhotonTargets.All);
			base.StartCoroutine(this.WaitAndFinishDestroyCoroutine(1.5f));
		}
	}

	private IEnumerator WaitAndFinishDestroyCoroutine(float time)
	{
		yield return new WaitForSeconds(time);
		if (this._collidedTitan != null)
		{
			this._collidedTitan.isThunderSpear = false;
		}
		PhotonNetwork.Destroy(base.gameObject);
	}

	[RPC]
	public void DisableRPC(PhotonMessageInfo info = null)
	{
		if (this.Disabled || (info != null && info.sender != base.photonView.owner))
		{
			return;
		}
		foreach (GameObject item in this._hideUponDestroy)
		{
			item.SetActive(value: false);
		}
		this._sphereCollider.enabled = false;
		this.SetDisabledTrailFade();
		base.rigidbody.velocity = Vector3.zero;
		this.Disabled = true;
	}

	private void SetDisabledTrailFade()
	{
		int particleCount = this._trail.particleCount;
		float startLifetime = this._trail.startLifetime * this._DisabledTrailFadeMultiplier;
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[particleCount];
		this._trail.GetParticles(array);
		for (int i = 0; i < particleCount; i++)
		{
			array[i].lifetime *= this._DisabledTrailFadeMultiplier;
			array[i].startLifetime = startLifetime;
		}
		this._trail.SetParticles(array, particleCount);
	}

	public void Explode(float radius)
	{
		if (!this.Disabled)
		{
			PhotonNetwork.Instantiate("RCAsset/BombExplodeMain", base.transform.position, Quaternion.Euler(0f, 0f, 0f), 0);
			this.KillPlayersInRadius(radius);
			this.DestroySelf();
		}
	}

	private void KillPlayersInRadius(float radius)
	{
		foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
		{
			GameObject gameObject = player.gameObject;
			if (!(Vector3.Distance(gameObject.transform.position, base.transform.position) < radius) || gameObject.GetPhotonView().isMine || player.bombImmune)
			{
				continue;
			}
			PhotonPlayer owner = gameObject.GetPhotonView().owner;
			if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null && owner.customProperties[PhotonPlayerProperty.RCteam] != null)
			{
				int num = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]);
				int num2 = RCextensions.returnIntFromObject(owner.customProperties[PhotonPlayerProperty.RCteam]);
				if (num == 0 || num != num2)
				{
					this.KillPlayer(player);
				}
			}
			else
			{
				this.KillPlayer(player);
			}
		}
	}

	private void KillPlayer(HERO hero)
	{
		hero.markDie();
		hero.photonView.RPC("netDie2", PhotonTargets.All, -1, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]) + " ");
		FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
		GameProgressManager.RegisterHumanKill(this._owner.gameObject, hero, KillWeapon.ThunderSpear);
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			stream.SendNext(base.rigidbody.velocity);
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			this.correctPlayerVelocity = (Vector3)stream.ReceiveNext();
		}
	}

	private void Update()
	{
		if (base.photonView.isMine)
		{
			return;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
		base.rigidbody.velocity = this.correctPlayerVelocity;
		if (base.rigidbody.velocity != Vector3.zero)
		{
			this._receivedNonZeroVelocity = true;
		}
		else
		{
			if (this._ownerIsUpdated || !this._receivedNonZeroVelocity || this.Disabled)
			{
				return;
			}
			this.Disabled = true;
			foreach (GameObject item in this._hideUponDestroy)
			{
				item.SetActive(value: false);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!this.Disabled && base.photonView.isMine)
		{
			this.CheckCollide();
		}
	}

	private void CheckCollide()
	{
		LayerMask layerMask = (1 << PhysicsLayer.PlayerAttackBox) | (1 << PhysicsLayer.PlayerHitBox);
		Collider[] array = Physics.OverlapSphere(base.transform.position + this._sphereCollider.center, this._sphereCollider.radius, layerMask);
		foreach (Collider collider in array)
		{
			if (collider.name.Contains("PlayerDetectorRC"))
			{
				TITAN component = collider.transform.root.gameObject.GetComponent<TITAN>();
				if (component != null)
				{
					if (this._collidedTitan == null)
					{
						this._collidedTitan = component;
						this._collidedTitan.isThunderSpear = true;
					}
					else if (this._collidedTitan != component)
					{
						this._collidedTitan.isThunderSpear = false;
						this._collidedTitan = component;
						this._collidedTitan.isThunderSpear = true;
					}
				}
			}
			else if (collider.gameObject.layer == PhysicsLayer.PlayerHitBox && !collider.transform.root.gameObject.GetComponent<HERO>().photonView.isMine)
			{
				this.Explode(this.BombRadius);
			}
		}
	}
}
