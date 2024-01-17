using System.Collections.Generic;
using UnityEngine;

public class CharacterMaterials
{
	private static bool inited;

	public static Dictionary<string, Material> materials;

	public static void init()
	{
		if (!CharacterMaterials.inited)
		{
			CharacterMaterials.inited = true;
			CharacterMaterials.materials = new Dictionary<string, Material>();
			CharacterMaterials.newMaterial("AOTTG_HERO_3DMG");
			CharacterMaterials.newMaterial("aottg_hero_AHSS_3dmg");
			CharacterMaterials.newMaterial("aottg_hero_annie_cap_causal");
			CharacterMaterials.newMaterial("aottg_hero_annie_cap_uniform");
			CharacterMaterials.newMaterial("aottg_hero_brand_sc");
			CharacterMaterials.newMaterial("aottg_hero_brand_mp");
			CharacterMaterials.newMaterial("aottg_hero_brand_g");
			CharacterMaterials.newMaterial("aottg_hero_brand_ts");
			CharacterMaterials.newMaterial("aottg_hero_skin_1");
			CharacterMaterials.newMaterial("aottg_hero_skin_2");
			CharacterMaterials.newMaterial("aottg_hero_skin_3");
			CharacterMaterials.newMaterial("aottg_hero_casual_fa_1");
			CharacterMaterials.newMaterial("aottg_hero_casual_fa_2");
			CharacterMaterials.newMaterial("aottg_hero_casual_fa_3");
			CharacterMaterials.newMaterial("aottg_hero_casual_fb_1");
			CharacterMaterials.newMaterial("aottg_hero_casual_fb_2");
			CharacterMaterials.newMaterial("aottg_hero_casual_ma_1");
			CharacterMaterials.newMaterial("aottg_hero_casual_ma_1_ahss");
			CharacterMaterials.newMaterial("aottg_hero_casual_ma_2");
			CharacterMaterials.newMaterial("aottg_hero_casual_ma_3");
			CharacterMaterials.newMaterial("aottg_hero_casual_mb_1");
			CharacterMaterials.newMaterial("aottg_hero_casual_mb_2");
			CharacterMaterials.newMaterial("aottg_hero_casual_mb_3");
			CharacterMaterials.newMaterial("aottg_hero_casual_mb_4");
			CharacterMaterials.newMaterial("aottg_hero_uniform_fa_1");
			CharacterMaterials.newMaterial("aottg_hero_uniform_fa_2");
			CharacterMaterials.newMaterial("aottg_hero_uniform_fa_3");
			CharacterMaterials.newMaterial("aottg_hero_uniform_fb_1");
			CharacterMaterials.newMaterial("aottg_hero_uniform_fb_2");
			CharacterMaterials.newMaterial("aottg_hero_uniform_ma_1");
			CharacterMaterials.newMaterial("aottg_hero_uniform_ma_2");
			CharacterMaterials.newMaterial("aottg_hero_uniform_ma_3");
			CharacterMaterials.newMaterial("aottg_hero_uniform_mb_1");
			CharacterMaterials.newMaterial("aottg_hero_uniform_mb_2");
			CharacterMaterials.newMaterial("aottg_hero_uniform_mb_3");
			CharacterMaterials.newMaterial("aottg_hero_uniform_mb_4");
			CharacterMaterials.newMaterial("hair_annie");
			CharacterMaterials.newMaterial("hair_armin");
			CharacterMaterials.newMaterial("hair_boy1");
			CharacterMaterials.newMaterial("hair_boy2");
			CharacterMaterials.newMaterial("hair_boy3");
			CharacterMaterials.newMaterial("hair_boy4");
			CharacterMaterials.newMaterial("hair_eren");
			CharacterMaterials.newMaterial("hair_girl1");
			CharacterMaterials.newMaterial("hair_girl2");
			CharacterMaterials.newMaterial("hair_girl3");
			CharacterMaterials.newMaterial("hair_girl4");
			CharacterMaterials.newMaterial("hair_girl5");
			CharacterMaterials.newMaterial("hair_hanji");
			CharacterMaterials.newMaterial("hair_jean");
			CharacterMaterials.newMaterial("hair_levi");
			CharacterMaterials.newMaterial("hair_marco");
			CharacterMaterials.newMaterial("hair_mike");
			CharacterMaterials.newMaterial("hair_petra");
			CharacterMaterials.newMaterial("hair_rico");
			CharacterMaterials.newMaterial("hair_sasha");
			CharacterMaterials.newMaterial("hair_mikasa");
			Texture mainTexture = (Texture)Object.Instantiate(Resources.Load("NewTexture/aottg_hero_eyes"));
			Material material = (Material)Object.Instantiate(Resources.Load("NewTexture/MaterialGLASS"));
			material.mainTexture = mainTexture;
			CharacterMaterials.materials.Add("aottg_hero_eyes", material);
		}
	}

	private static void newMaterial(string pref)
	{
		Texture mainTexture = (Texture)Object.Instantiate(Resources.Load("NewTexture/" + pref));
		Material material = (Material)Object.Instantiate(Resources.Load("NewTexture/MaterialCharacter"));
		material.mainTexture = mainTexture;
		CharacterMaterials.materials.Add(pref, material);
	}
}
