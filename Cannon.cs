using System;
using Photon;
using Settings;
using UnityEngine;

internal class Cannon : Photon.MonoBehaviour
{
	public Transform ballPoint;

	public Transform barrel;

	private Quaternion correctBarrelRot = Quaternion.identity;

	private Vector3 correctPlayerPos = Vector3.zero;

	private Quaternion correctPlayerRot = Quaternion.identity;

	public float currentRot;

	public Transform firingPoint;

	public bool isCannonGround;

	public GameObject myCannonBall;

	public LineRenderer myCannonLine;

	public HERO myHero;

	public string settings;

	public float SmoothingDelay = 5f;

	public void Awake()
	{
		if (!(base.photonView != null))
		{
			return;
		}
		base.photonView.observed = this;
		this.barrel = base.transform.Find("Barrel");
		this.correctPlayerPos = base.transform.position;
		this.correctPlayerRot = base.transform.rotation;
		this.correctBarrelRot = this.barrel.rotation;
		if (base.photonView.isMine)
		{
			this.firingPoint = this.barrel.Find("FiringPoint");
			this.ballPoint = this.barrel.Find("BallPoint");
			this.myCannonLine = this.ballPoint.GetComponent<LineRenderer>();
			if (base.gameObject.name.Contains("CannonGround"))
			{
				this.isCannonGround = true;
			}
		}
		if (!PhotonNetwork.isMasterClient)
		{
			return;
		}
		PhotonPlayer owner = base.photonView.owner;
		if (FengGameManagerMKII.instance.allowedToCannon.ContainsKey(owner.ID))
		{
			this.settings = FengGameManagerMKII.instance.allowedToCannon[owner.ID].settings;
			base.photonView.RPC("SetSize", PhotonTargets.All, this.settings);
			int viewID = FengGameManagerMKII.instance.allowedToCannon[owner.ID].viewID;
			FengGameManagerMKII.instance.allowedToCannon.Remove(owner.ID);
			CannonPropRegion component = PhotonView.Find(viewID).gameObject.GetComponent<CannonPropRegion>();
			if (component != null)
			{
				component.disabled = true;
				component.destroyed = true;
				PhotonNetwork.Destroy(component.gameObject);
			}
		}
		else if (!owner.isLocal && !FengGameManagerMKII.instance.restartingMC)
		{
			FengGameManagerMKII.instance.kickPlayerRC(owner, ban: false, "spawning cannon without request.");
		}
	}

	public void Fire()
	{
		if (this.myHero.skillCDDuration <= 0f)
		{
			EnemyCheckCollider[] componentsInChildren = PhotonNetwork.Instantiate("FX/boom2", this.firingPoint.position, this.firingPoint.rotation, 0).GetComponentsInChildren<EnemyCheckCollider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].dmg = 0;
			}
			this.myCannonBall = PhotonNetwork.Instantiate("RCAsset/CannonBallObject", this.ballPoint.position, this.firingPoint.rotation, 0);
			this.myCannonBall.rigidbody.velocity = this.firingPoint.forward * 300f;
			this.myCannonBall.GetComponent<CannonBall>().myHero = this.myHero;
			this.myHero.skillCDDuration = 3.5f;
		}
	}

	public void OnDestroy()
	{
		if (!PhotonNetwork.isMasterClient || FengGameManagerMKII.instance.isRestarting)
		{
			return;
		}
		string[] array = this.settings.Split(',');
		if (array[0] == "photon")
		{
			if (array.Length > 15)
			{
				GameObject obj = PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[12]), Convert.ToSingle(array[13]), Convert.ToSingle(array[14])), new Quaternion(Convert.ToSingle(array[15]), Convert.ToSingle(array[16]), Convert.ToSingle(array[17]), Convert.ToSingle(array[18])), 0);
				obj.GetComponent<CannonPropRegion>().settings = this.settings;
				obj.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, this.settings);
			}
			else
			{
				PhotonNetwork.Instantiate("RCAsset/" + array[1] + "Prop", new Vector3(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]), Convert.ToSingle(array[4])), new Quaternion(Convert.ToSingle(array[5]), Convert.ToSingle(array[6]), Convert.ToSingle(array[7]), Convert.ToSingle(array[8])), 0).GetComponent<CannonPropRegion>().settings = this.settings;
			}
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
			stream.SendNext(this.barrel.rotation);
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			this.correctBarrelRot = (Quaternion)stream.ReceiveNext();
		}
	}

	[RPC]
	public void SetSize(string settings, PhotonMessageInfo info)
	{
		if (!info.sender.isMasterClient)
		{
			return;
		}
		string[] array = settings.Split(',');
		if (array.Length <= 15)
		{
			return;
		}
		float a = 1f;
		GameObject gameObject = null;
		gameObject = base.gameObject;
		if (array[2] != "default")
		{
			if (array[2].StartsWith("transparent"))
			{
				if (float.TryParse(array[2].Substring(11), out var result))
				{
					a = result;
				}
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.material = (Material)FengGameManagerMKII.RCassets.Load("transparent");
					if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
					{
						renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(array[11]));
					}
				}
			}
			else
			{
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer2 in componentsInChildren)
				{
					if (!renderer2.name.Contains("Line Renderer"))
					{
						renderer2.material = (Material)FengGameManagerMKII.RCassets.Load(array[2]);
						if (Convert.ToSingle(array[10]) != 1f || Convert.ToSingle(array[11]) != 1f)
						{
							renderer2.material.mainTextureScale = new Vector2(renderer2.material.mainTextureScale.x * Convert.ToSingle(array[10]), renderer2.material.mainTextureScale.y * Convert.ToSingle(array[11]));
						}
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
			return;
		}
		Color color = new Color(Convert.ToSingle(array[7]), Convert.ToSingle(array[8]), Convert.ToSingle(array[9]), a);
		MeshFilter[] componentsInChildren2 = gameObject.GetComponentsInChildren<MeshFilter>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			Mesh mesh = componentsInChildren2[i].mesh;
			Color[] array2 = new Color[mesh.vertexCount];
			for (int j = 0; j < mesh.vertexCount; j++)
			{
				array2[j] = color;
			}
			mesh.colors = array2;
		}
	}

	public void Update()
	{
		if (!base.photonView.isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
			this.barrel.rotation = Quaternion.Lerp(this.barrel.rotation, this.correctBarrelRot, Time.deltaTime * this.SmoothingDelay);
			return;
		}
		Vector3 vector = new Vector3(0f, -30f, 0f);
		Vector3 position = this.ballPoint.position;
		Vector3 vector2 = this.ballPoint.forward * 300f;
		float num = 40f / vector2.magnitude;
		this.myCannonLine.SetWidth(0.5f, 40f);
		this.myCannonLine.SetVertexCount(100);
		for (int i = 0; i < 100; i++)
		{
			this.myCannonLine.SetPosition(i, position);
			position += vector2 * num + 0.5f * vector * num * num;
			vector2 += vector * num;
		}
		float num2 = 30f;
		if (SettingsManager.InputSettings.Interaction.CannonSlow.GetKey())
		{
			num2 = 5f;
		}
		if (this.isCannonGround)
		{
			if (SettingsManager.InputSettings.General.Forward.GetKey())
			{
				if (this.currentRot <= 32f)
				{
					this.currentRot += Time.deltaTime * num2;
					this.barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * num2));
				}
			}
			else if (SettingsManager.InputSettings.General.Back.GetKey() && this.currentRot >= -18f)
			{
				this.currentRot += Time.deltaTime * (0f - num2);
				this.barrel.Rotate(new Vector3(0f, 0f, Time.deltaTime * (0f - num2)));
			}
			if (SettingsManager.InputSettings.General.Left.GetKey())
			{
				base.transform.Rotate(new Vector3(0f, Time.deltaTime * (0f - num2), 0f));
			}
			else if (SettingsManager.InputSettings.General.Right.GetKey())
			{
				base.transform.Rotate(new Vector3(0f, Time.deltaTime * num2, 0f));
			}
		}
		else
		{
			if (SettingsManager.InputSettings.General.Forward.GetKey())
			{
				if (this.currentRot >= -50f)
				{
					this.currentRot += Time.deltaTime * (0f - num2);
					this.barrel.Rotate(new Vector3(Time.deltaTime * (0f - num2), 0f, 0f));
				}
			}
			else if (SettingsManager.InputSettings.General.Back.GetKey() && this.currentRot <= 40f)
			{
				this.currentRot += Time.deltaTime * num2;
				this.barrel.Rotate(new Vector3(Time.deltaTime * num2, 0f, 0f));
			}
			if (SettingsManager.InputSettings.General.Left.GetKey())
			{
				base.transform.Rotate(new Vector3(0f, Time.deltaTime * (0f - num2), 0f));
			}
			else if (SettingsManager.InputSettings.General.Right.GetKey())
			{
				base.transform.Rotate(new Vector3(0f, Time.deltaTime * num2, 0f));
			}
		}
		if (SettingsManager.InputSettings.Interaction.CannonFire.GetKey())
		{
			this.Fire();
		}
		else if (SettingsManager.InputSettings.Interaction.Interact.GetKeyDown())
		{
			if (this.myHero != null)
			{
				this.myHero.isCannon = false;
				this.myHero.myCannonRegion = null;
				Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(this.myHero.gameObject);
				this.myHero.baseRigidBody.velocity = Vector3.zero;
				this.myHero.photonView.RPC("ReturnFromCannon", PhotonTargets.Others);
				this.myHero.skillCDLast = this.myHero.skillCDLastCannon;
				this.myHero.skillCDDuration = this.myHero.skillCDLast;
			}
			PhotonNetwork.Destroy(base.gameObject);
		}
	}
}
