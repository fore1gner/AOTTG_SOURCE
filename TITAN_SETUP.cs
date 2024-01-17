using System.Collections;
using CustomSkins;
using Photon;
using Settings;
using UnityEngine;

public class TITAN_SETUP : Photon.MonoBehaviour
{
	public GameObject eye;

	private CostumeHair hair;

	private GameObject hair_go_ref;

	private int hairType;

	public bool haseye;

	public GameObject part_hair;

	public int skin;

	private TitanCustomSkinLoader _customSkinLoader;

	private void Awake()
	{
		CostumeHair.init();
		CharacterMaterials.init();
		HeroCostume.init2();
		this.hair_go_ref = new GameObject();
		this.eye.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
		this.hair_go_ref.transform.position = this.eye.transform.position + Vector3.up * 3.5f + base.transform.forward * 5.2f;
		this.hair_go_ref.transform.rotation = this.eye.transform.rotation;
		this.hair_go_ref.transform.RotateAround(this.eye.transform.position, base.transform.right, -20f);
		this.hair_go_ref.transform.localScale = new Vector3(210f, 210f, 210f);
		this.hair_go_ref.transform.parent = base.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head").transform;
		this._customSkinLoader = base.gameObject.AddComponent<TitanCustomSkinLoader>();
	}

	public IEnumerator loadskinE(int hair, int eye, string hairlink)
	{
		Object.Destroy(this.part_hair);
		this.hair = CostumeHair.hairsM[hair];
		this.hairType = hair;
		if (this.hair.hair != string.Empty)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
			gameObject.transform.parent = this.hair_go_ref.transform.parent;
			gameObject.transform.position = this.hair_go_ref.transform.position;
			gameObject.transform.rotation = this.hair_go_ref.transform.rotation;
			gameObject.transform.localScale = this.hair_go_ref.transform.localScale;
			gameObject.renderer.material = CharacterMaterials.materials[this.hair.texture];
			this.part_hair = gameObject;
			yield return base.StartCoroutine(this._customSkinLoader.LoadSkinsFromRPC(new object[2] { true, hairlink }));
		}
		if (eye >= 0)
		{
			this.setFacialTexture(this.eye, eye);
		}
		yield return null;
	}

	public void setFacialTexture(GameObject go, int id)
	{
		if (id >= 0)
		{
			float x = 0.125f * (float)(int)((float)id / 8f);
			float y = (0f - 0.25f) * (float)(id % 4);
			go.renderer.material.mainTextureOffset = new Vector2(x, y);
		}
	}

	public void setHair2()
	{
		BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
		if (titan.SkinsEnabled.Value && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || base.photonView.isMine))
		{
			TitanCustomSkinSet obj = (TitanCustomSkinSet)titan.GetSelectedSet();
			int num = Random.Range(0, 9);
			if (num == 3)
			{
				num = 9;
			}
			int index = this.skin;
			if (obj.RandomizedPairs.Value)
			{
				index = Random.Range(0, 5);
			}
			int num2 = ((IntSetting)obj.HairModels.GetItemAt(index)).Value - 1;
			if (num2 >= 0)
			{
				num = num2;
			}
			string value = ((StringSetting)obj.Hairs.GetItemAt(index)).Value;
			int num3 = Random.Range(1, 8);
			if (this.haseye)
			{
				num3 = 0;
			}
			bool flag = false;
			if (value.EndsWith(".jpg") || value.EndsWith(".png") || value.EndsWith(".jpeg"))
			{
				flag = true;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
			{
				if (flag)
				{
					object[] parameters = new object[3] { num, num3, value };
					base.photonView.RPC("setHairRPC2", PhotonTargets.AllBuffered, parameters);
				}
				else
				{
					Color hair_color = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
					object[] parameters = new object[5] { num, num3, hair_color.r, hair_color.g, hair_color.b };
					base.photonView.RPC("setHairPRC", PhotonTargets.AllBuffered, parameters);
				}
			}
			else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (flag)
				{
					base.StartCoroutine(this.loadskinE(num, num3, value));
					return;
				}
				Color hair_color = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
				this.setHairPRC(num, num3, hair_color.r, hair_color.g, hair_color.b);
			}
		}
		else
		{
			int num = Random.Range(0, CostumeHair.hairsM.Length);
			if (num == 3)
			{
				num = 9;
			}
			Object.Destroy(this.part_hair);
			this.hairType = num;
			this.hair = CostumeHair.hairsM[num];
			if (this.hair.hair == string.Empty)
			{
				this.hair = CostumeHair.hairsM[9];
				this.hairType = 9;
			}
			this.part_hair = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
			this.part_hair.transform.parent = this.hair_go_ref.transform.parent;
			this.part_hair.transform.position = this.hair_go_ref.transform.position;
			this.part_hair.transform.rotation = this.hair_go_ref.transform.rotation;
			this.part_hair.transform.localScale = this.hair_go_ref.transform.localScale;
			this.part_hair.renderer.material = CharacterMaterials.materials[this.hair.texture];
			this.part_hair.renderer.material.color = HeroCostume.costume[Random.Range(0, HeroCostume.costume.Length - 5)].hair_color;
			int num4 = Random.Range(1, 8);
			this.setFacialTexture(this.eye, num4);
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
			{
				object[] parameters = new object[5]
				{
					this.hairType,
					num4,
					this.part_hair.renderer.material.color.r,
					this.part_hair.renderer.material.color.g,
					this.part_hair.renderer.material.color.b
				};
				base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, parameters);
			}
		}
	}

	[RPC]
	private void setHairPRC(int type, int eye_type, float c1, float c2, float c3)
	{
		Object.Destroy(this.part_hair);
		this.hair = CostumeHair.hairsM[type];
		this.hairType = type;
		if (this.hair.hair != string.Empty)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
			gameObject.transform.parent = this.hair_go_ref.transform.parent;
			gameObject.transform.position = this.hair_go_ref.transform.position;
			gameObject.transform.rotation = this.hair_go_ref.transform.rotation;
			gameObject.transform.localScale = this.hair_go_ref.transform.localScale;
			gameObject.renderer.material = CharacterMaterials.materials[this.hair.texture];
			gameObject.renderer.material.color = new Color(c1, c2, c3);
			this.part_hair = gameObject;
		}
		this.setFacialTexture(this.eye, eye_type);
	}

	[RPC]
	public void setHairRPC2(int hair, int eye, string hairlink, PhotonMessageInfo info)
	{
		BaseCustomSkinSettings<TitanCustomSkinSet> titan = SettingsManager.CustomSkinSettings.Titan;
		if (info.sender == base.photonView.owner && titan.SkinsEnabled.Value && (!titan.SkinsLocal.Value || base.photonView.isMine))
		{
			base.StartCoroutine(this.loadskinE(hair, eye, hairlink));
		}
	}

	public void setPunkHair()
	{
		Object.Destroy(this.part_hair);
		this.hair = CostumeHair.hairsM[3];
		this.hairType = 3;
		GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.hair.hair));
		gameObject.transform.parent = this.hair_go_ref.transform.parent;
		gameObject.transform.position = this.hair_go_ref.transform.position;
		gameObject.transform.rotation = this.hair_go_ref.transform.rotation;
		gameObject.transform.localScale = this.hair_go_ref.transform.localScale;
		gameObject.renderer.material = CharacterMaterials.materials[this.hair.texture];
		switch (Random.Range(1, 4))
		{
		case 1:
			gameObject.renderer.material.color = FengColor.hairPunk1;
			break;
		case 2:
			gameObject.renderer.material.color = FengColor.hairPunk2;
			break;
		case 3:
			gameObject.renderer.material.color = FengColor.hairPunk3;
			break;
		}
		this.part_hair = gameObject;
		this.setFacialTexture(this.eye, 0);
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && base.photonView.isMine)
		{
			object[] parameters = new object[5]
			{
				this.hairType,
				0,
				this.part_hair.renderer.material.color.r,
				this.part_hair.renderer.material.color.g,
				this.part_hair.renderer.material.color.b
			};
			base.photonView.RPC("setHairPRC", PhotonTargets.OthersBuffered, parameters);
		}
	}

	public void setVar(int skin, bool haseye)
	{
		this.skin = skin;
		this.haseye = haseye;
	}
}
