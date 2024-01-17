using System.Collections;
using System.Collections.Generic;
using Photon;
using Settings;
using UnityEngine;

internal class CannonBall : Photon.MonoBehaviour
{
	private Vector3 correctPos;

	private Vector3 correctVelocity;

	public bool disabled;

	public Transform firingPoint;

	public bool isCollider;

	public HERO myHero;

	public List<TitanTrigger> myTitanTriggers;

	public float SmoothingDelay = 10f;

	private void Awake()
	{
		if (base.photonView != null)
		{
			base.photonView.observed = this;
			this.correctPos = base.transform.position;
			this.correctVelocity = Vector3.zero;
			base.GetComponent<SphereCollider>().enabled = false;
			if (base.photonView.isMine)
			{
				base.StartCoroutine(this.WaitAndDestroy(10f));
				this.myTitanTriggers = new List<TitanTrigger>();
			}
		}
	}

	public void destroyMe()
	{
		if (this.disabled)
		{
			return;
		}
		this.disabled = true;
		EnemyCheckCollider[] componentsInChildren = PhotonNetwork.Instantiate("FX/boom4", base.transform.position, base.transform.rotation, 0).GetComponentsInChildren<EnemyCheckCollider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].dmg = 0;
		}
		if (SettingsManager.LegacyGameSettings.CannonsFriendlyFire.Value)
		{
			foreach (HERO player in FengGameManagerMKII.instance.getPlayers())
			{
				if (!(player != null) || !(Vector3.Distance(player.transform.position, base.transform.position) <= 20f) || player.photonView.isMine)
				{
					continue;
				}
				GameObject gameObject = player.gameObject;
				PhotonPlayer owner = gameObject.GetPhotonView().owner;
				if (SettingsManager.LegacyGameSettings.TeamMode.Value > 0 && PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null && owner.customProperties[PhotonPlayerProperty.RCteam] != null)
				{
					int num = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]);
					int num2 = RCextensions.returnIntFromObject(owner.customProperties[PhotonPlayerProperty.RCteam]);
					if (num == 0 || num != num2)
					{
						gameObject.GetComponent<HERO>().markDie();
						gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]) + " ");
						FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
					}
				}
				else
				{
					gameObject.GetComponent<HERO>().markDie();
					gameObject.GetComponent<HERO>().photonView.RPC("netDie2", PhotonTargets.All, -1, RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]) + " ");
					FengGameManagerMKII.instance.playerKillInfoUpdate(PhotonNetwork.player, 0);
				}
			}
		}
		if (this.myTitanTriggers != null)
		{
			for (int j = 0; j < this.myTitanTriggers.Count; j++)
			{
				if (this.myTitanTriggers[j] != null)
				{
					this.myTitanTriggers[j].isCollide = false;
				}
			}
		}
		PhotonNetwork.Destroy(base.gameObject);
	}

	public void FixedUpdate()
	{
		if (!base.photonView.isMine || this.disabled)
		{
			return;
		}
		LayerMask layerMask = 1 << LayerMask.NameToLayer("PlayerAttackBox");
		LayerMask layerMask2 = 1 << LayerMask.NameToLayer("EnemyBox");
		LayerMask layerMask3 = (int)layerMask | (int)layerMask2;
		if (!this.isCollider)
		{
			LayerMask layerMask4 = 1 << LayerMask.NameToLayer("Ground");
			layerMask3 = (int)layerMask3 | (int)layerMask4;
		}
		Collider[] array = Physics.OverlapSphere(base.transform.position, 0.6f, layerMask3.value);
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i].gameObject;
			if (gameObject.layer == 16)
			{
				TitanTrigger component = gameObject.GetComponent<TitanTrigger>();
				if (!(component == null) && !this.myTitanTriggers.Contains(component))
				{
					component.isCollide = true;
					this.myTitanTriggers.Add(component);
				}
			}
			else if (gameObject.layer == 10)
			{
				TITAN component2 = gameObject.transform.root.gameObject.GetComponent<TITAN>();
				if (!(component2 != null))
				{
					continue;
				}
				if (component2.abnormalType == AbnormalType.TYPE_CRAWLER)
				{
					if (gameObject.name == "head")
					{
						component2.photonView.RPC("DieByCannon", component2.photonView.owner, this.myHero.photonView.viewID);
						component2.dieBlow(base.transform.position, 0.2f);
						i = array.Length;
					}
				}
				else if (gameObject.name == "head")
				{
					component2.photonView.RPC("DieByCannon", component2.photonView.owner, this.myHero.photonView.viewID);
					component2.dieHeadBlow(base.transform.position, 0.2f);
					i = array.Length;
				}
				else if (Random.Range(0f, 1f) < 0.5f)
				{
					component2.hitL(base.transform.position, 0.05f);
				}
				else
				{
					component2.hitR(base.transform.position, 0.05f);
				}
				this.destroyMe();
			}
			else if (gameObject.layer == 9 && (gameObject.transform.root.name.Contains("CannonWall") || gameObject.transform.root.name.Contains("CannonGround")))
			{
				flag = true;
			}
		}
		if (!(this.isCollider || flag))
		{
			this.isCollider = true;
			base.GetComponent<SphereCollider>().enabled = true;
		}
	}

	public void OnCollisionEnter(Collision myCollision)
	{
		if (base.photonView.isMine)
		{
			this.destroyMe();
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.rigidbody.velocity);
		}
		else
		{
			this.correctPos = (Vector3)stream.ReceiveNext();
			this.correctVelocity = (Vector3)stream.ReceiveNext();
		}
	}

	public void Update()
	{
		if (!base.photonView.isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPos, Time.deltaTime * this.SmoothingDelay);
			base.rigidbody.velocity = this.correctVelocity;
		}
	}

	public IEnumerator WaitAndDestroy(float time)
	{
		yield return new WaitForSeconds(time);
		this.destroyMe();
	}
}
