using System.Collections.Generic;
using UnityEngine;
using Xft;

public class HERO_SETUP : MonoBehaviour
{
	private string aniname;

	private float anitime;

	private List<BoneWeight> boneWeightsList = new List<BoneWeight>();

	private bool change;

	public GameObject chest_info;

	private byte[] config = new byte[4];

	private int currentOne;

	private SkinnedMeshRenderer[][] elements;

	public bool isDeadBody;

	private List<Material> materialList;

	private GameObject mount_3dmg;

	private GameObject mount_3dmg_gas_l;

	private GameObject mount_3dmg_gas_r;

	private GameObject mount_3dmg_gun_mag_l;

	private GameObject mount_3dmg_gun_mag_r;

	public GameObject mount_weapon_l;

	public GameObject mount_weapon_r;

	public HeroCostume myCostume;

	public GameObject part_3dmg;

	public GameObject part_3dmg_belt;

	public GameObject part_3dmg_gas_l;

	public GameObject part_3dmg_gas_r;

	public GameObject part_arm_l;

	public GameObject part_arm_r;

	public GameObject part_asset_1;

	public GameObject part_asset_2;

	public GameObject part_blade_l;

	public GameObject part_blade_r;

	public GameObject part_brand_1;

	public GameObject part_brand_2;

	public GameObject part_brand_3;

	public GameObject part_brand_4;

	public GameObject part_cape;

	public GameObject part_chest;

	public GameObject part_chest_1;

	public GameObject part_chest_2;

	public GameObject part_chest_3;

	public GameObject part_eye;

	public GameObject part_face;

	public GameObject part_glass;

	public GameObject part_hair;

	public GameObject part_hair_1;

	public GameObject part_hair_2;

	public GameObject part_hand_l;

	public GameObject part_hand_r;

	public GameObject part_head;

	public GameObject part_leg;

	public GameObject part_upper_body;

	public GameObject reference;

	private float timer;

	private void Awake()
	{
		this.part_head.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
		this.mount_3dmg = new GameObject();
		this.mount_3dmg_gas_l = new GameObject();
		this.mount_3dmg_gas_r = new GameObject();
		this.mount_3dmg_gun_mag_l = new GameObject();
		this.mount_3dmg_gun_mag_r = new GameObject();
		this.mount_weapon_l = new GameObject();
		this.mount_weapon_r = new GameObject();
		this.mount_3dmg.transform.position = base.transform.position;
		this.mount_3dmg.transform.rotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
		this.mount_3dmg.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
		this.mount_3dmg_gas_l.transform.position = base.transform.position;
		this.mount_3dmg_gas_l.transform.rotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
		this.mount_3dmg_gas_l.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine").transform;
		this.mount_3dmg_gas_r.transform.position = base.transform.position;
		this.mount_3dmg_gas_r.transform.rotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
		this.mount_3dmg_gas_r.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine").transform;
		this.mount_3dmg_gun_mag_l.transform.position = base.transform.position;
		this.mount_3dmg_gun_mag_l.transform.rotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
		this.mount_3dmg_gun_mag_l.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/thigh_L").transform;
		this.mount_3dmg_gun_mag_r.transform.position = base.transform.position;
		this.mount_3dmg_gun_mag_r.transform.rotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
		this.mount_3dmg_gun_mag_r.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/thigh_R").transform;
		this.mount_weapon_l.transform.position = base.transform.position;
		this.mount_weapon_l.transform.rotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
		this.mount_weapon_l.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
		this.mount_weapon_r.transform.position = base.transform.position;
		this.mount_weapon_r.transform.rotation = Quaternion.Euler(270f, base.transform.rotation.eulerAngles.y, 0f);
		this.mount_weapon_r.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
	}

	private void combineSMR(GameObject go, GameObject go2)
	{
		if (go.GetComponent<SkinnedMeshRenderer>() == null)
		{
			go.AddComponent<SkinnedMeshRenderer>();
		}
		SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
		List<CombineInstance> list = new List<CombineInstance>();
		this.materialList = new List<Material>();
		this.materialList.Add(component.material);
		this.boneWeightsList = new List<BoneWeight>();
		Transform[] bones = component.bones;
		SkinnedMeshRenderer component2 = go2.GetComponent<SkinnedMeshRenderer>();
		for (int i = 0; i < component2.sharedMesh.subMeshCount; i++)
		{
			CombineInstance combineInstance = default(CombineInstance);
			combineInstance.mesh = component2.sharedMesh;
			combineInstance.transform = component2.transform.localToWorldMatrix;
			combineInstance.subMeshIndex = i;
			CombineInstance item = combineInstance;
			list.Add(item);
			for (int j = 0; j < this.materialList.Count; j++)
			{
				if (this.materialList[j].name != component2.material.name)
				{
					this.materialList.Add(component2.material);
					break;
				}
			}
		}
		Object.Destroy(component2.gameObject);
		component.sharedMesh = new Mesh();
		component.sharedMesh.CombineMeshes(list.ToArray(), mergeSubMeshes: true, useMatrices: false);
		component.bones = bones;
		component.materials = this.materialList.ToArray();
		List<Matrix4x4> list2 = new List<Matrix4x4>();
		for (int k = 0; k < bones.Length; k++)
		{
			if (bones[k] != null)
			{
				list2.Add(bones[k].worldToLocalMatrix * base.transform.localToWorldMatrix);
			}
		}
		component.sharedMesh.bindposes = list2.ToArray();
	}

	public void create3DMG()
	{
		Object.Destroy(this.part_3dmg);
		Object.Destroy(this.part_3dmg_belt);
		Object.Destroy(this.part_3dmg_gas_l);
		Object.Destroy(this.part_3dmg_gas_r);
		Object.Destroy(this.part_blade_l);
		Object.Destroy(this.part_blade_r);
		if (this.myCostume.mesh_3dmg.Length > 0)
		{
			this.part_3dmg = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.mesh_3dmg));
			this.part_3dmg.transform.position = this.mount_3dmg.transform.position;
			this.part_3dmg.transform.rotation = this.mount_3dmg.transform.rotation;
			this.part_3dmg.transform.parent = this.mount_3dmg.transform.parent;
			this.part_3dmg.renderer.material = CharacterMaterials.materials[this.myCostume._3dmg_texture];
		}
		if (this.myCostume.mesh_3dmg_belt.Length > 0)
		{
			this.part_3dmg_belt = this.GenerateCloth(this.reference, "Character/" + this.myCostume.mesh_3dmg_belt);
			this.part_3dmg_belt.renderer.material = CharacterMaterials.materials[this.myCostume._3dmg_texture];
		}
		if (this.myCostume.mesh_3dmg_gas_l.Length > 0)
		{
			this.part_3dmg_gas_l = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.mesh_3dmg_gas_l));
			if (this.myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
			{
				this.part_3dmg_gas_l.transform.position = this.mount_3dmg_gas_l.transform.position;
				this.part_3dmg_gas_l.transform.rotation = this.mount_3dmg_gas_l.transform.rotation;
				this.part_3dmg_gas_l.transform.parent = this.mount_3dmg_gas_l.transform.parent;
			}
			else
			{
				this.part_3dmg_gas_l.transform.position = this.mount_3dmg_gun_mag_l.transform.position;
				this.part_3dmg_gas_l.transform.rotation = this.mount_3dmg_gun_mag_l.transform.rotation;
				this.part_3dmg_gas_l.transform.parent = this.mount_3dmg_gun_mag_l.transform.parent;
			}
			this.part_3dmg_gas_l.renderer.material = CharacterMaterials.materials[this.myCostume._3dmg_texture];
		}
		if (this.myCostume.mesh_3dmg_gas_r.Length > 0)
		{
			this.part_3dmg_gas_r = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.mesh_3dmg_gas_r));
			if (this.myCostume.uniform_type != UNIFORM_TYPE.CasualAHSS)
			{
				this.part_3dmg_gas_r.transform.position = this.mount_3dmg_gas_r.transform.position;
				this.part_3dmg_gas_r.transform.rotation = this.mount_3dmg_gas_r.transform.rotation;
				this.part_3dmg_gas_r.transform.parent = this.mount_3dmg_gas_r.transform.parent;
			}
			else
			{
				this.part_3dmg_gas_r.transform.position = this.mount_3dmg_gun_mag_r.transform.position;
				this.part_3dmg_gas_r.transform.rotation = this.mount_3dmg_gun_mag_r.transform.rotation;
				this.part_3dmg_gas_r.transform.parent = this.mount_3dmg_gun_mag_r.transform.parent;
			}
			this.part_3dmg_gas_r.renderer.material = CharacterMaterials.materials[this.myCostume._3dmg_texture];
		}
		if (this.myCostume.weapon_l_mesh.Length > 0)
		{
			this.part_blade_l = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.weapon_l_mesh));
			this.part_blade_l.transform.position = this.mount_weapon_l.transform.position;
			this.part_blade_l.transform.rotation = this.mount_weapon_l.transform.rotation;
			this.part_blade_l.transform.parent = this.mount_weapon_l.transform.parent;
			this.part_blade_l.renderer.material = CharacterMaterials.materials[this.myCostume._3dmg_texture];
			if (this.part_blade_l.transform.Find("X-WeaponTrailA") != null)
			{
				this.part_blade_l.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>().Deactivate();
				this.part_blade_l.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>().Deactivate();
				if (base.gameObject.GetComponent<HERO>() != null)
				{
					base.gameObject.GetComponent<HERO>().leftbladetrail = this.part_blade_l.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>();
					base.gameObject.GetComponent<HERO>().leftbladetrail2 = this.part_blade_l.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>();
				}
			}
		}
		if (this.myCostume.weapon_r_mesh.Length <= 0)
		{
			return;
		}
		this.part_blade_r = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.weapon_r_mesh));
		this.part_blade_r.transform.position = this.mount_weapon_r.transform.position;
		this.part_blade_r.transform.rotation = this.mount_weapon_r.transform.rotation;
		this.part_blade_r.transform.parent = this.mount_weapon_r.transform.parent;
		this.part_blade_r.renderer.material = CharacterMaterials.materials[this.myCostume._3dmg_texture];
		if (this.part_blade_r.transform.Find("X-WeaponTrailA") != null)
		{
			this.part_blade_r.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>().Deactivate();
			this.part_blade_r.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>().Deactivate();
			if (base.gameObject.GetComponent<HERO>() != null)
			{
				base.gameObject.GetComponent<HERO>().rightbladetrail = this.part_blade_r.transform.Find("X-WeaponTrailA").GetComponent<XWeaponTrail>();
				base.gameObject.GetComponent<HERO>().rightbladetrail2 = this.part_blade_r.transform.Find("X-WeaponTrailB").GetComponent<XWeaponTrail>();
			}
		}
	}

	public void createCape2()
	{
		if (!this.isDeadBody)
		{
			ClothFactory.DisposeObject(this.part_cape);
			if (this.myCostume.cape_mesh.Length > 0)
			{
				this.part_cape = ClothFactory.GetCape(this.reference, "Character/" + this.myCostume.cape_mesh, CharacterMaterials.materials[this.myCostume.brand_texture]);
			}
		}
	}

	public void createFace()
	{
		this.part_face = (GameObject)Object.Instantiate(Resources.Load("Character/character_face"));
		this.part_face.transform.position = this.part_head.transform.position;
		this.part_face.transform.rotation = this.part_head.transform.rotation;
		this.part_face.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
	}

	public void createGlass()
	{
		this.part_glass = (GameObject)Object.Instantiate(Resources.Load("Character/glass"));
		this.part_glass.transform.position = this.part_head.transform.position;
		this.part_glass.transform.rotation = this.part_head.transform.rotation;
		this.part_glass.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
	}

	public void createHair2()
	{
		Object.Destroy(this.part_hair);
		if (!this.isDeadBody)
		{
			ClothFactory.DisposeObject(this.part_hair_1);
		}
		if (this.myCostume.hair_mesh != string.Empty)
		{
			this.part_hair = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.hair_mesh));
			this.part_hair.transform.position = this.part_head.transform.position;
			this.part_hair.transform.rotation = this.part_head.transform.rotation;
			this.part_hair.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
			this.part_hair.renderer.material = CharacterMaterials.materials[this.myCostume.hairInfo.texture];
			this.part_hair.renderer.material.color = this.myCostume.hair_color;
		}
		if (this.myCostume.hair_1_mesh.Length > 0 && !this.isDeadBody)
		{
			string text = "Character/" + this.myCostume.hair_1_mesh;
			Material material = CharacterMaterials.materials[this.myCostume.hairInfo.texture];
			this.part_hair_1 = ClothFactory.GetHair(this.reference, text, material, this.myCostume.hair_color);
		}
	}

	public void createHead2()
	{
		Object.Destroy(this.part_eye);
		Object.Destroy(this.part_face);
		Object.Destroy(this.part_glass);
		Object.Destroy(this.part_hair);
		if (!this.isDeadBody)
		{
			ClothFactory.DisposeObject(this.part_hair_1);
		}
		this.createHair2();
		if (this.myCostume.eye_mesh.Length > 0)
		{
			this.part_eye = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.eye_mesh));
			this.part_eye.transform.position = this.part_head.transform.position;
			this.part_eye.transform.rotation = this.part_head.transform.rotation;
			this.part_eye.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head").transform;
			this.setFacialTexture(this.part_eye, this.myCostume.eye_texture_id);
		}
		if (this.myCostume.beard_texture_id >= 0)
		{
			this.createFace();
			this.setFacialTexture(this.part_face, this.myCostume.beard_texture_id);
		}
		if (this.myCostume.glass_texture_id >= 0)
		{
			this.createGlass();
			this.setFacialTexture(this.part_glass, this.myCostume.glass_texture_id);
		}
		this.part_head.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
		this.part_chest.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
	}

	public void createLeftArm()
	{
		Object.Destroy(this.part_arm_l);
		if (this.myCostume.arm_l_mesh.Length > 0)
		{
			this.part_arm_l = this.GenerateCloth(this.reference, "Character/" + this.myCostume.arm_l_mesh);
			this.part_arm_l.renderer.material = CharacterMaterials.materials[this.myCostume.body_texture];
		}
		Object.Destroy(this.part_hand_l);
		if (this.myCostume.hand_l_mesh.Length > 0)
		{
			this.part_hand_l = this.GenerateCloth(this.reference, "Character/" + this.myCostume.hand_l_mesh);
			this.part_hand_l.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
		}
	}

	public void createLowerBody()
	{
		this.part_leg.renderer.material = CharacterMaterials.materials[this.myCostume.body_texture];
	}

	public void createRightArm()
	{
		Object.Destroy(this.part_arm_r);
		if (this.myCostume.arm_r_mesh.Length > 0)
		{
			this.part_arm_r = this.GenerateCloth(this.reference, "Character/" + this.myCostume.arm_r_mesh);
			this.part_arm_r.renderer.material = CharacterMaterials.materials[this.myCostume.body_texture];
		}
		Object.Destroy(this.part_hand_r);
		if (this.myCostume.hand_r_mesh.Length > 0)
		{
			this.part_hand_r = this.GenerateCloth(this.reference, "Character/" + this.myCostume.hand_r_mesh);
			this.part_hand_r.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
		}
	}

	public void createUpperBody2()
	{
		Object.Destroy(this.part_upper_body);
		Object.Destroy(this.part_brand_1);
		Object.Destroy(this.part_brand_2);
		Object.Destroy(this.part_brand_3);
		Object.Destroy(this.part_brand_4);
		Object.Destroy(this.part_chest_1);
		Object.Destroy(this.part_chest_2);
		if (!this.isDeadBody)
		{
			ClothFactory.DisposeObject(this.part_chest_3);
		}
		this.createCape2();
		if (this.myCostume.part_chest_object_mesh.Length > 0)
		{
			this.part_chest_1 = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.part_chest_object_mesh));
			this.part_chest_1.transform.position = this.chest_info.transform.position;
			this.part_chest_1.transform.rotation = this.chest_info.transform.rotation;
			this.part_chest_1.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
			this.part_chest_1.renderer.material = CharacterMaterials.materials[this.myCostume.part_chest_object_texture];
		}
		if (this.myCostume.part_chest_1_object_mesh.Length > 0)
		{
			this.part_chest_2 = (GameObject)Object.Instantiate(Resources.Load("Character/" + this.myCostume.part_chest_1_object_mesh));
			this.part_chest_2.transform.position = this.chest_info.transform.position;
			this.part_chest_2.transform.rotation = this.chest_info.transform.rotation;
			this.part_chest_2.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
			this.part_chest_2.transform.parent = base.transform.Find("Amarture/Controller_Body/hip/spine/chest").transform;
			this.part_chest_2.renderer.material = CharacterMaterials.materials[this.myCostume.part_chest_1_object_texture];
		}
		if (this.myCostume.part_chest_skinned_cloth_mesh.Length > 0 && !this.isDeadBody)
		{
			this.part_chest_3 = ClothFactory.GetCape(this.reference, "Character/" + this.myCostume.part_chest_skinned_cloth_mesh, CharacterMaterials.materials[this.myCostume.part_chest_skinned_cloth_texture]);
		}
		if (this.myCostume.body_mesh.Length > 0)
		{
			this.part_upper_body = this.GenerateCloth(this.reference, "Character/" + this.myCostume.body_mesh);
			this.part_upper_body.renderer.material = CharacterMaterials.materials[this.myCostume.body_texture];
		}
		if (this.myCostume.brand1_mesh.Length > 0)
		{
			this.part_brand_1 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand1_mesh);
			this.part_brand_1.renderer.material = CharacterMaterials.materials[this.myCostume.brand_texture];
		}
		if (this.myCostume.brand2_mesh.Length > 0)
		{
			this.part_brand_2 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand2_mesh);
			this.part_brand_2.renderer.material = CharacterMaterials.materials[this.myCostume.brand_texture];
		}
		if (this.myCostume.brand3_mesh.Length > 0)
		{
			this.part_brand_3 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand3_mesh);
			this.part_brand_3.renderer.material = CharacterMaterials.materials[this.myCostume.brand_texture];
		}
		if (this.myCostume.brand4_mesh.Length > 0)
		{
			this.part_brand_4 = this.GenerateCloth(this.reference, "Character/" + this.myCostume.brand4_mesh);
			this.part_brand_4.renderer.material = CharacterMaterials.materials[this.myCostume.brand_texture];
		}
		this.part_head.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
		this.part_chest.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
	}

	public void deleteCharacterComponent2()
	{
		Object.Destroy(this.part_eye);
		Object.Destroy(this.part_face);
		Object.Destroy(this.part_glass);
		Object.Destroy(this.part_hair);
		if (!this.isDeadBody)
		{
			ClothFactory.DisposeObject(this.part_hair_1);
		}
		Object.Destroy(this.part_upper_body);
		Object.Destroy(this.part_arm_l);
		Object.Destroy(this.part_arm_r);
		if (!this.isDeadBody)
		{
			ClothFactory.DisposeObject(this.part_hair_2);
			ClothFactory.DisposeObject(this.part_cape);
		}
		Object.Destroy(this.part_brand_1);
		Object.Destroy(this.part_brand_2);
		Object.Destroy(this.part_brand_3);
		Object.Destroy(this.part_brand_4);
		Object.Destroy(this.part_chest_1);
		Object.Destroy(this.part_chest_2);
		Object.Destroy(this.part_chest_3);
		Object.Destroy(this.part_3dmg);
		Object.Destroy(this.part_3dmg_belt);
		Object.Destroy(this.part_3dmg_gas_l);
		Object.Destroy(this.part_3dmg_gas_r);
		Object.Destroy(this.part_blade_l);
		Object.Destroy(this.part_blade_r);
	}

	private GameObject GenerateCloth(GameObject go, string res)
	{
		if (go.GetComponent<SkinnedMeshRenderer>() == null)
		{
			go.AddComponent<SkinnedMeshRenderer>();
		}
		SkinnedMeshRenderer component = go.GetComponent<SkinnedMeshRenderer>();
		Transform[] bones = component.bones;
		SkinnedMeshRenderer component2 = ((GameObject)Object.Instantiate(Resources.Load(res))).GetComponent<SkinnedMeshRenderer>();
		component2.gameObject.transform.parent = component.gameObject.transform.parent;
		component2.transform.localPosition = Vector3.zero;
		component2.transform.localScale = Vector3.one;
		component2.bones = bones;
		component2.quality = SkinQuality.Bone4;
		return component2.gameObject;
	}

	private byte[] GetCurrentConfig()
	{
		return this.config;
	}

	public void init()
	{
		CharacterMaterials.init();
	}

	public void setCharacterComponent()
	{
		this.createHead2();
		this.createUpperBody2();
		this.createLeftArm();
		this.createRightArm();
		this.createLowerBody();
		this.create3DMG();
	}

	public void setFacialTexture(GameObject go, int id)
	{
		if (id >= 0)
		{
			go.renderer.material = CharacterMaterials.materials[this.myCostume.face_texture];
			float x = 0.125f * (float)(int)((float)id / 8f);
			float y = (0f - 0.125f) * (float)(id % 8);
			go.renderer.material.mainTextureOffset = new Vector2(x, y);
		}
	}

	public void setSkin()
	{
		this.part_head.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
		this.part_chest.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
		this.part_hand_l.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
		this.part_hand_r.renderer.material = CharacterMaterials.materials[this.myCostume.skin_texture];
	}
}
