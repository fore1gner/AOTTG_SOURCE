using System.Collections;
using CustomSkins;
using Photon;
using UnityEngine;

internal class Bullet : Photon.MonoBehaviour
{
	private Vector3 heightOffSet = Vector3.up * 0.48f;

	private bool isdestroying;

	private float killTime;

	private float killTime2;

	private Vector3 launchOffSet = Vector3.zero;

	private bool left = true;

	public bool leviMode;

	public float leviShootTime;

	private LineRenderer lineRenderer;

	private GameObject master;

	private GameObject myRef;

	public TITAN myTitan;

	private ArrayList nodes = new ArrayList();

	private int phase;

	private GameObject rope;

	private int spiralcount;

	private ArrayList spiralNodes;

	private Vector3 velocity = Vector3.zero;

	private Vector3 velocity2 = Vector3.zero;

	private bool _hasSkin;

	private float _lastLength;

	private float _skinTiling;

	private bool _isTransparent;

	private void SetSkin()
	{
		HumanCustomSkinLoader customSkinLoader = this.master.GetComponent<HERO>()._customSkinLoader;
		HookCustomSkinPart hookCustomSkinPart = (this.left ? customSkinLoader.HookL : customSkinLoader.HookR);
		if (hookCustomSkinPart != null)
		{
			if (hookCustomSkinPart.HookMaterial != null)
			{
				this._hasSkin = true;
				this.lineRenderer.material = hookCustomSkinPart.HookMaterial;
				this._skinTiling = (this.left ? customSkinLoader.HookLTiling : customSkinLoader.HookRTiling);
			}
			if (hookCustomSkinPart.Transparent)
			{
				this._hasSkin = true;
				this._isTransparent = true;
				this.lineRenderer.enabled = false;
			}
		}
	}

	private void UpdateSkin()
	{
		if (this._hasSkin && !this._isTransparent)
		{
			float magnitude = (base.transform.position - this.myRef.transform.position).magnitude;
			if (magnitude != this._lastLength)
			{
				this._lastLength = magnitude;
				this.lineRenderer.material.mainTextureScale = new Vector2(this._skinTiling * magnitude, 1f);
			}
		}
	}

	public void checkTitan()
	{
		GameObject main_object = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
		if (!(main_object != null) || !(this.master != null) || !(this.master == main_object))
		{
			return;
		}
		LayerMask layerMask = 1 << LayerMask.NameToLayer("PlayerAttackBox");
		if (!Physics.Raycast(base.transform.position, this.velocity, out var hitInfo, 10f, layerMask.value))
		{
			return;
		}
		Collider collider = hitInfo.collider;
		if (!collider.name.Contains("PlayerDetectorRC"))
		{
			return;
		}
		TITAN component = collider.transform.root.gameObject.GetComponent<TITAN>();
		if (component != null)
		{
			if (this.myTitan == null)
			{
				this.myTitan = component;
				this.myTitan.isHooked = true;
			}
			else if (this.myTitan != component)
			{
				this.myTitan.isHooked = false;
				this.myTitan = component;
				this.myTitan.isHooked = true;
			}
		}
	}

	public void disable()
	{
		this.phase = 2;
		this.killTime = 0f;
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			object[] parameters = new object[1] { 2 };
			base.photonView.RPC("setPhase", PhotonTargets.Others, parameters);
		}
	}

	private void FixedUpdate()
	{
		if ((this.phase == 2 || this.phase == 1) && this.leviMode)
		{
			this.spiralcount++;
			if (this.spiralcount >= 60)
			{
				this.isdestroying = true;
				this.removeMe();
				return;
			}
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && !base.photonView.isMine)
		{
			if (this.phase == 0)
			{
				base.gameObject.transform.position += this.velocity * Time.deltaTime * 50f + this.velocity2 * Time.deltaTime;
				this.nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
			}
		}
		else
		{
			if (this.phase != 0)
			{
				return;
			}
			this.checkTitan();
			base.gameObject.transform.position += this.velocity * Time.deltaTime * 50f + this.velocity2 * Time.deltaTime;
			LayerMask layerMask = 1 << LayerMask.NameToLayer("EnemyBox");
			LayerMask layerMask2 = 1 << LayerMask.NameToLayer("Ground");
			LayerMask layerMask3 = 1 << LayerMask.NameToLayer("NetworkObject");
			LayerMask layerMask4 = (int)layerMask | (int)layerMask2 | (int)layerMask3;
			bool flag = false;
			bool flag2 = false;
			if ((this.nodes.Count <= 1) ? Physics.Linecast((Vector3)this.nodes[this.nodes.Count - 1], base.gameObject.transform.position, out var hitInfo, layerMask4.value) : Physics.Linecast((Vector3)this.nodes[this.nodes.Count - 2], base.gameObject.transform.position, out hitInfo, layerMask4.value))
			{
				bool flag3 = true;
				if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("EnemyBox"))
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						object[] parameters = new object[1] { hitInfo.collider.transform.root.gameObject.GetPhotonView().viewID };
						base.photonView.RPC("tieMeToOBJ", PhotonTargets.Others, parameters);
					}
					this.master.GetComponent<HERO>().lastHook = hitInfo.collider.transform.root;
					base.transform.parent = hitInfo.collider.transform;
				}
				else if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
				{
					this.master.GetComponent<HERO>().lastHook = null;
				}
				else if (hitInfo.collider.transform.gameObject.layer == LayerMask.NameToLayer("NetworkObject") && hitInfo.collider.transform.gameObject.tag == "Player" && !this.leviMode)
				{
					if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
					{
						object[] parameters2 = new object[1] { hitInfo.collider.transform.root.gameObject.GetPhotonView().viewID };
						base.photonView.RPC("tieMeToOBJ", PhotonTargets.Others, parameters2);
					}
					this.master.GetComponent<HERO>().hookToHuman(hitInfo.collider.transform.root.gameObject, base.transform.position);
					base.transform.parent = hitInfo.collider.transform;
					this.master.GetComponent<HERO>().lastHook = null;
				}
				else
				{
					flag3 = false;
				}
				if (this.phase == 2)
				{
					flag3 = false;
				}
				if (flag3)
				{
					this.master.GetComponent<HERO>().launch(hitInfo.point, this.left, this.leviMode);
					base.transform.position = hitInfo.point;
					if (this.phase != 2)
					{
						this.phase = 1;
						if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
						{
							object[] parameters3 = new object[1] { 1 };
							base.photonView.RPC("setPhase", PhotonTargets.Others, parameters3);
							object[] parameters4 = new object[1] { base.transform.position };
							base.photonView.RPC("tieMeTo", PhotonTargets.Others, parameters4);
						}
						if (this.leviMode)
						{
							this.getSpiral(this.master.transform.position, this.master.transform.rotation.eulerAngles);
						}
						flag = true;
					}
				}
			}
			this.nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
			if (flag)
			{
				return;
			}
			this.killTime2 += Time.deltaTime;
			if (this.killTime2 > 0.8f)
			{
				this.phase = 4;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
				{
					object[] parameters5 = new object[1] { 4 };
					base.photonView.RPC("setPhase", PhotonTargets.Others, parameters5);
				}
			}
		}
	}

	private void getSpiral(Vector3 masterposition, Vector3 masterrotation)
	{
		float num = 1.2f;
		float num2 = 30f;
		float num3 = 0.5f;
		num = 30f;
		float num4 = 0.05f + (float)this.spiralcount * 0.03f;
		num = ((this.spiralcount >= 5) ? (1.2f + (float)(60 - this.spiralcount) * 0.1f) : Vector2.Distance(new Vector2(masterposition.x, masterposition.z), new Vector2(base.gameObject.transform.position.x, base.gameObject.transform.position.z)));
		num3 -= (float)this.spiralcount * 0.06f;
		float num5 = num / num2;
		float num6 = num4 / num2 * 2f * 3.141593f;
		num3 *= 6.283185f;
		this.spiralNodes = new ArrayList();
		for (int i = 1; (float)i <= num2; i++)
		{
			float num7 = (float)i * num5 * (1f + 0.05f * (float)i);
			float f = (float)i * num6 + num3 + 1.256637f + masterrotation.y * 0.0173f;
			float x = Mathf.Cos(f) * num7;
			float z = (0f - Mathf.Sin(f)) * num7;
			this.spiralNodes.Add(new Vector3(x, 0f, z));
		}
	}

	public bool isHooked()
	{
		return this.phase == 1;
	}

	private void killObject()
	{
		Object.Destroy(this.rope);
		Object.Destroy(base.gameObject);
	}

	public void launch(Vector3 v, Vector3 v2, string launcher_ref, bool isLeft, GameObject hero, bool leviMode = false)
	{
		if (this.phase != 2)
		{
			this.master = hero;
			this.velocity = v;
			if (Mathf.Abs(Mathf.Acos(Vector3.Dot(v.normalized, v2.normalized)) * 57.29578f) > 90f)
			{
				this.velocity2 = Vector3.zero;
			}
			else
			{
				this.velocity2 = Vector3.Project(v2, v);
			}
			if (launcher_ref == "hookRefL1")
			{
				this.myRef = hero.GetComponent<HERO>().hookRefL1;
			}
			if (launcher_ref == "hookRefL2")
			{
				this.myRef = hero.GetComponent<HERO>().hookRefL2;
			}
			if (launcher_ref == "hookRefR1")
			{
				this.myRef = hero.GetComponent<HERO>().hookRefR1;
			}
			if (launcher_ref == "hookRefR2")
			{
				this.myRef = hero.GetComponent<HERO>().hookRefR2;
			}
			this.nodes = new ArrayList();
			this.nodes.Add(this.myRef.transform.position);
			this.phase = 0;
			this.leviMode = leviMode;
			this.left = isLeft;
			if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
			{
				object[] parameters = new object[2]
				{
					hero.GetComponent<HERO>().photonView.viewID,
					launcher_ref
				};
				base.photonView.RPC("myMasterIs", PhotonTargets.Others, parameters);
				object[] parameters2 = new object[3] { v, this.velocity2, this.left };
				base.photonView.RPC("setVelocityAndLeft", PhotonTargets.Others, parameters2);
			}
			base.transform.position = this.myRef.transform.position;
			base.transform.rotation = Quaternion.LookRotation(v.normalized);
			this.SetSkin();
		}
	}

	[RPC]
	private void myMasterIs(int id, string launcherRef, PhotonMessageInfo info)
	{
		if (info.sender != base.photonView.owner || id < 0)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "bullet myMasterIs");
			return;
		}
		PhotonView photonView = PhotonView.Find(id);
		if (photonView.owner != info.sender)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "bullet myMasterIs");
		}
		else if (!(photonView == null) && !(photonView.gameObject == null))
		{
			this.master = PhotonView.Find(id).gameObject;
			if (launcherRef == "hookRefL1")
			{
				this.myRef = this.master.GetComponent<HERO>().hookRefL1;
			}
			if (launcherRef == "hookRefL2")
			{
				this.myRef = this.master.GetComponent<HERO>().hookRefL2;
			}
			if (launcherRef == "hookRefR1")
			{
				this.myRef = this.master.GetComponent<HERO>().hookRefR1;
			}
			if (launcherRef == "hookRefR2")
			{
				this.myRef = this.master.GetComponent<HERO>().hookRefR2;
			}
		}
	}

	private void netLaunch(Vector3 newPosition)
	{
		this.nodes = new ArrayList();
		this.nodes.Add(newPosition);
	}

	private void netUpdateLeviSpiral(Vector3 newPosition, Vector3 masterPosition, Vector3 masterrotation)
	{
		this.phase = 2;
		this.leviMode = true;
		this.getSpiral(masterPosition, masterrotation);
		Vector3 vector = masterPosition - (Vector3)this.spiralNodes[0];
		this.lineRenderer.SetVertexCount(this.spiralNodes.Count - (int)((float)this.spiralcount * 0.5f));
		for (int i = 0; (float)i <= (float)(this.spiralNodes.Count - 1) - (float)this.spiralcount * 0.5f; i++)
		{
			if (this.spiralcount < 5)
			{
				Vector3 vector2 = (Vector3)this.spiralNodes[i] + vector;
				float num = (float)(this.spiralNodes.Count - 1) - (float)this.spiralcount * 0.5f;
				vector2 = new Vector3(vector2.x, vector2.y * ((num - (float)i) / num) + newPosition.y * ((float)i / num), vector2.z);
				this.lineRenderer.SetPosition(i, vector2);
			}
			else
			{
				this.lineRenderer.SetPosition(i, (Vector3)this.spiralNodes[i] + vector);
			}
		}
	}

	private void netUpdatePhase1(Vector3 newPosition, Vector3 masterPosition)
	{
		this.lineRenderer.SetVertexCount(2);
		this.lineRenderer.SetPosition(0, newPosition);
		this.lineRenderer.SetPosition(1, masterPosition);
		base.transform.position = newPosition;
	}

	private void OnDestroy()
	{
		if (FengGameManagerMKII.instance != null)
		{
			FengGameManagerMKII.instance.removeHook(this);
		}
		if (this.myTitan != null)
		{
			this.myTitan.isHooked = false;
		}
		Object.Destroy(this.rope);
	}

	public void removeMe()
	{
		this.isdestroying = true;
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && base.photonView.isMine)
		{
			PhotonNetwork.Destroy(base.photonView);
			PhotonNetwork.RemoveRPCs(base.photonView);
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			Object.Destroy(this.rope);
			Object.Destroy(base.gameObject);
		}
	}

	private void setLinePhase0()
	{
		if (this.master == null)
		{
			Object.Destroy(this.rope);
			Object.Destroy(base.gameObject);
		}
		else if (this.nodes.Count > 0)
		{
			Vector3 vector = this.myRef.transform.position - (Vector3)this.nodes[0];
			this.lineRenderer.SetVertexCount(this.nodes.Count);
			for (int i = 0; i <= this.nodes.Count - 1; i++)
			{
				this.lineRenderer.SetPosition(i, (Vector3)this.nodes[i] + vector * Mathf.Pow(0.75f, i));
			}
			if (this.nodes.Count > 1)
			{
				this.lineRenderer.SetPosition(1, this.myRef.transform.position);
			}
		}
	}

	[RPC]
	private void setPhase(int value, PhotonMessageInfo info)
	{
		if (info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "bullet setphase");
		}
		else
		{
			this.phase = value;
		}
	}

	[RPC]
	private void setVelocityAndLeft(Vector3 value, Vector3 v2, bool l, PhotonMessageInfo info)
	{
		if (info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "bullet setvelocity");
			return;
		}
		this.velocity = value;
		this.velocity2 = v2;
		this.left = l;
		base.transform.rotation = Quaternion.LookRotation(value.normalized);
		this.SetSkin();
	}

	private void Awake()
	{
		this.rope = (GameObject)Object.Instantiate(Resources.Load("rope"));
		this.lineRenderer = this.rope.GetComponent<LineRenderer>();
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addHook(this);
	}

	[RPC]
	private void tieMeTo(Vector3 p, PhotonMessageInfo info)
	{
		if (info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "bullet tieMeTo");
		}
		else
		{
			base.transform.position = p;
		}
	}

	[RPC]
	private void tieMeToOBJ(int id, PhotonMessageInfo info)
	{
		if (info.sender != base.photonView.owner)
		{
			FengGameManagerMKII.instance.kickPlayerRCIfMC(info.sender, ban: true, "bullet TieMeToObj");
		}
		else
		{
			base.transform.parent = PhotonView.Find(id).gameObject.transform;
		}
	}

	public void update()
	{
		if (this.master == null)
		{
			this.removeMe();
		}
		else
		{
			if (this.isdestroying)
			{
				return;
			}
			if (this.leviMode)
			{
				this.leviShootTime += Time.deltaTime;
				if (this.leviShootTime > 0.4f)
				{
					this.phase = 2;
					base.gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
			}
			if (this.phase == 0)
			{
				this.setLinePhase0();
			}
			else if (this.phase == 1)
			{
				Vector3 vector = base.transform.position - this.myRef.transform.position;
				_ = base.transform.position + this.myRef.transform.position;
				Vector3 vector2 = this.master.rigidbody.velocity;
				float magnitude = vector2.magnitude;
				float magnitude2 = vector.magnitude;
				int value = (int)((magnitude2 + magnitude) / 5f);
				value = Mathf.Clamp(value, 2, 6);
				this.lineRenderer.SetVertexCount(value);
				this.lineRenderer.SetPosition(0, this.myRef.transform.position);
				int i = 1;
				float num = Mathf.Pow(magnitude2, 0.3f);
				for (; i < value; i++)
				{
					int num2 = value / 2;
					float num3 = Mathf.Abs(i - num2);
					float f = ((float)num2 - num3) / (float)num2;
					f = Mathf.Pow(f, 0.5f);
					float num4 = (num + magnitude) * 0.0015f * f;
					this.lineRenderer.SetPosition(i, new Vector3(Random.Range(0f - num4, num4), Random.Range(0f - num4, num4), Random.Range(0f - num4, num4)) + this.myRef.transform.position + vector * ((float)i / (float)value) - Vector3.up * num * 0.05f * f - vector2 * 0.001f * f * num);
				}
				this.lineRenderer.SetPosition(value - 1, base.transform.position);
			}
			else if (this.phase == 2)
			{
				if (!this.leviMode)
				{
					this.lineRenderer.SetVertexCount(2);
					this.lineRenderer.SetPosition(0, base.transform.position);
					this.lineRenderer.SetPosition(1, this.myRef.transform.position);
					this.killTime += Time.deltaTime * 0.2f;
					this.lineRenderer.SetWidth(0.1f - this.killTime, 0.1f - this.killTime);
					if (this.killTime > 0.1f)
					{
						this.removeMe();
					}
				}
				else
				{
					this.getSpiral(this.master.transform.position, this.master.transform.rotation.eulerAngles);
					Vector3 vector3 = this.myRef.transform.position - (Vector3)this.spiralNodes[0];
					this.lineRenderer.SetVertexCount(this.spiralNodes.Count - (int)((float)this.spiralcount * 0.5f));
					for (int j = 0; (float)j <= (float)(this.spiralNodes.Count - 1) - (float)this.spiralcount * 0.5f; j++)
					{
						if (this.spiralcount < 5)
						{
							Vector3 vector4 = (Vector3)this.spiralNodes[j] + vector3;
							float num5 = (float)(this.spiralNodes.Count - 1) - (float)this.spiralcount * 0.5f;
							vector4 = new Vector3(vector4.x, vector4.y * ((num5 - (float)j) / num5) + base.gameObject.transform.position.y * ((float)j / num5), vector4.z);
							this.lineRenderer.SetPosition(j, vector4);
						}
						else
						{
							this.lineRenderer.SetPosition(j, (Vector3)this.spiralNodes[j] + vector3);
						}
					}
				}
			}
			else if (this.phase == 4)
			{
				base.gameObject.transform.position += this.velocity + this.velocity2 * Time.deltaTime;
				this.nodes.Add(new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z));
				Vector3 vector5 = this.myRef.transform.position - (Vector3)this.nodes[0];
				for (int k = 0; k <= this.nodes.Count - 1; k++)
				{
					this.lineRenderer.SetVertexCount(this.nodes.Count);
					this.lineRenderer.SetPosition(k, (Vector3)this.nodes[k] + vector5 * Mathf.Pow(0.5f, k));
				}
				this.killTime2 += Time.deltaTime;
				if (this.killTime2 > 0.8f)
				{
					this.killTime += Time.deltaTime * 0.2f;
					this.lineRenderer.SetWidth(0.1f - this.killTime, 0.1f - this.killTime);
					if (this.killTime > 0.1f)
					{
						this.removeMe();
					}
				}
			}
			this.UpdateSkin();
		}
	}
}
