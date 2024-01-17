using Photon;
using UnityEngine;

public class RockThrow : Photon.MonoBehaviour
{
	private bool launched;

	private Vector3 oldP;

	private Vector3 r;

	private Vector3 v;

	private void explore()
	{
		GameObject gameObject;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			gameObject = PhotonNetwork.Instantiate("FX/boom6", base.transform.position, base.transform.rotation, 0);
			if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
			{
				gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
				gameObject.GetComponent<EnemyfxIDcontainer>().titanName = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
			}
		}
		else
		{
			gameObject = (GameObject)Object.Instantiate(Resources.Load("FX/boom6"), base.transform.position, base.transform.rotation);
		}
		gameObject.transform.localScale = base.transform.localScale;
		float b = 1f - Vector3.Distance(GameObject.Find("MainCamera").transform.position, gameObject.transform.position) * 0.05f;
		b = Mathf.Min(1f, b);
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startShake(b, b);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			PhotonNetwork.Destroy(base.photonView);
		}
	}

	private void hitPlayer(GameObject hero)
	{
		if (!(hero != null) || hero.GetComponent<HERO>().HasDied() || hero.GetComponent<HERO>().isInvincible())
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			if (!hero.GetComponent<HERO>().isGrabbed)
			{
				hero.GetComponent<HERO>().die(this.v.normalized * 1000f + Vector3.up * 50f, isBite: false);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !hero.GetComponent<HERO>().HasDied() && !hero.GetComponent<HERO>().isGrabbed)
		{
			hero.GetComponent<HERO>().markDie();
			int num = -1;
			string text = string.Empty;
			if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null)
			{
				num = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID;
				text = base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().titanName;
			}
			object[] parameters = new object[5]
			{
				this.v.normalized * 1000f + Vector3.up * 50f,
				false,
				num,
				text,
				true
			};
			hero.GetComponent<HERO>().photonView.RPC("netDie", PhotonTargets.All, parameters);
		}
	}

	[RPC]
	private void initRPC(int viewID, Vector3 scale, Vector3 pos, float level)
	{
		GameObject gameObject = PhotonView.Find(viewID).gameObject;
		Transform parent = gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R/hand_R_001");
		base.transform.localScale = gameObject.transform.localScale;
		base.transform.parent = parent;
		base.transform.localPosition = pos;
	}

	public void launch(Vector3 v1)
	{
		this.launched = true;
		this.oldP = base.transform.position;
		this.v = v1;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
		{
			object[] parameters = new object[2] { this.v, this.oldP };
			base.photonView.RPC("launchRPC", PhotonTargets.Others, parameters);
		}
	}

	[RPC]
	private void launchRPC(Vector3 v, Vector3 p)
	{
		this.launched = true;
		base.transform.position = p;
		this.oldP = p;
		base.transform.parent = null;
		this.launch(v);
	}

	private void Start()
	{
		this.r = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
	}

	private void Update()
	{
		if (!this.launched)
		{
			return;
		}
		base.transform.Rotate(this.r);
		this.v -= 20f * Vector3.up * Time.deltaTime;
		base.transform.position += this.v * Time.deltaTime;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && !PhotonNetwork.isMasterClient)
		{
			return;
		}
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = 1 << LayerMask.NameToLayer("Players");
		LayerMask layerMask3 = 1 << LayerMask.NameToLayer("EnemyAABB");
		LayerMask layerMask4 = (int)layerMask2 | (int)layerMask | (int)layerMask3;
		RaycastHit[] array = Physics.SphereCastAll(base.transform.position, 2.5f * base.transform.lossyScale.x, base.transform.position - this.oldP, Vector3.Distance(base.transform.position, this.oldP), layerMask4);
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit raycastHit = array[i];
			if (LayerMask.LayerToName(raycastHit.collider.gameObject.layer) == "EnemyAABB")
			{
				GameObject gameObject = raycastHit.collider.gameObject.transform.root.gameObject;
				if (gameObject.GetComponent<TITAN>() != null && !gameObject.GetComponent<TITAN>().hasDie)
				{
					gameObject.GetComponent<TITAN>().hitAnkle();
					_ = base.transform.position;
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
					{
						gameObject.GetComponent<TITAN>().hitAnkle();
					}
					else
					{
						if (base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>() != null && PhotonView.Find(base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID) != null)
						{
							_ = PhotonView.Find(base.transform.root.gameObject.GetComponent<EnemyfxIDcontainer>().myOwnerViewID).transform.position;
						}
						gameObject.GetComponent<HERO>().photonView.RPC("hitAnkleRPC", PhotonTargets.All);
					}
				}
				this.explore();
			}
			else if (LayerMask.LayerToName(raycastHit.collider.gameObject.layer) == "Players")
			{
				GameObject gameObject2 = raycastHit.collider.gameObject.transform.root.gameObject;
				if (gameObject2.GetComponent<TITAN_EREN>() != null)
				{
					if (!gameObject2.GetComponent<TITAN_EREN>().isHit)
					{
						gameObject2.GetComponent<TITAN_EREN>().hitByTitan();
					}
				}
				else if (gameObject2.GetComponent<HERO>() != null && !gameObject2.GetComponent<HERO>().isInvincible())
				{
					this.hitPlayer(gameObject2);
				}
			}
			else if (LayerMask.LayerToName(raycastHit.collider.gameObject.layer) == "Ground")
			{
				this.explore();
			}
		}
		this.oldP = base.transform.position;
	}
}
