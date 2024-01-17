using UnityEngine;

public class HeroCostume
{
	public string _3dmg_texture = string.Empty;

	public string arm_l_mesh = string.Empty;

	public string arm_r_mesh = string.Empty;

	public string beard_mesh = string.Empty;

	public int beard_texture_id = -1;

	public static string[] body_casual_fa_texture;

	public static string[] body_casual_fb_texture;

	public static string[] body_casual_ma_texture;

	public static string[] body_casual_mb_texture;

	public string body_mesh = string.Empty;

	public string body_texture = string.Empty;

	public static string[] body_uniform_fa_texture;

	public static string[] body_uniform_fb_texture;

	public static string[] body_uniform_ma_texture;

	public static string[] body_uniform_mb_texture;

	public string brand_texture = string.Empty;

	public string brand1_mesh = string.Empty;

	public string brand2_mesh = string.Empty;

	public string brand3_mesh = string.Empty;

	public string brand4_mesh = string.Empty;

	public bool cape;

	public string cape_mesh = string.Empty;

	public string cape_texture = string.Empty;

	public static HeroCostume[] costume;

	public int costumeId;

	public static HeroCostume[] costumeOption;

	public DIVISION division;

	public string eye_mesh = string.Empty;

	public int eye_texture_id = -1;

	public string face_texture = string.Empty;

	public string glass_mesh = string.Empty;

	public int glass_texture_id = -1;

	public string hair_1_mesh = string.Empty;

	public Color hair_color = new Color(0.5f, 0.1f, 0f);

	public string hair_mesh = string.Empty;

	public CostumeHair hairInfo;

	public string hand_l_mesh = string.Empty;

	public string hand_r_mesh = string.Empty;

	public int id;

	private static bool inited;

	public string mesh_3dmg = string.Empty;

	public string mesh_3dmg_belt = string.Empty;

	public string mesh_3dmg_gas_l = string.Empty;

	public string mesh_3dmg_gas_r = string.Empty;

	public string name = string.Empty;

	public string part_chest_1_object_mesh = string.Empty;

	public string part_chest_1_object_texture = string.Empty;

	public string part_chest_object_mesh = string.Empty;

	public string part_chest_object_texture = string.Empty;

	public string part_chest_skinned_cloth_mesh = string.Empty;

	public string part_chest_skinned_cloth_texture = string.Empty;

	public SEX sex;

	public int skin_color = 1;

	public string skin_texture = string.Empty;

	public HeroStat stat;

	public UNIFORM_TYPE uniform_type = UNIFORM_TYPE.CasualA;

	public string weapon_l_mesh = string.Empty;

	public string weapon_r_mesh = string.Empty;

	public void checkstat()
	{
		if (0 + this.stat.SPD + this.stat.GAS + this.stat.BLA + this.stat.ACL > 400)
		{
			this.stat.SPD = (this.stat.GAS = (this.stat.BLA = (this.stat.ACL = 100)));
		}
	}

	public static void init()
	{
		if (!HeroCostume.inited)
		{
			HeroCostume.inited = true;
			CostumeHair.init();
			HeroCostume.body_uniform_ma_texture = new string[3] { "aottg_hero_uniform_ma_1", "aottg_hero_uniform_ma_2", "aottg_hero_uniform_ma_3" };
			HeroCostume.body_uniform_fa_texture = new string[3] { "aottg_hero_uniform_fa_1", "aottg_hero_uniform_fa_2", "aottg_hero_uniform_fa_3" };
			HeroCostume.body_uniform_mb_texture = new string[4] { "aottg_hero_uniform_mb_1", "aottg_hero_uniform_mb_2", "aottg_hero_uniform_mb_3", "aottg_hero_uniform_mb_4" };
			HeroCostume.body_uniform_fb_texture = new string[2] { "aottg_hero_uniform_fb_1", "aottg_hero_uniform_fb_2" };
			HeroCostume.body_casual_ma_texture = new string[3] { "aottg_hero_casual_ma_1", "aottg_hero_casual_ma_2", "aottg_hero_casual_ma_3" };
			HeroCostume.body_casual_fa_texture = new string[3] { "aottg_hero_casual_fa_1", "aottg_hero_casual_fa_2", "aottg_hero_casual_fa_3" };
			HeroCostume.body_casual_mb_texture = new string[4] { "aottg_hero_casual_mb_1", "aottg_hero_casual_mb_2", "aottg_hero_casual_mb_3", "aottg_hero_casual_mb_4" };
			HeroCostume.body_casual_fb_texture = new string[2] { "aottg_hero_casual_fb_1", "aottg_hero_casual_fb_2" };
			HeroCostume.costume = new HeroCostume[38];
			HeroCostume.costume[0] = new HeroCostume();
			HeroCostume.costume[0].name = "annie";
			HeroCostume.costume[0].sex = SEX.FEMALE;
			HeroCostume.costume[0].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[0].part_chest_object_mesh = "character_cap_uniform";
			HeroCostume.costume[0].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
			HeroCostume.costume[0].cape = true;
			HeroCostume.costume[0].body_texture = HeroCostume.body_uniform_fb_texture[0];
			HeroCostume.costume[0].hairInfo = CostumeHair.hairsF[5];
			HeroCostume.costume[0].eye_texture_id = 0;
			HeroCostume.costume[0].beard_texture_id = 33;
			HeroCostume.costume[0].glass_texture_id = -1;
			HeroCostume.costume[0].skin_color = 1;
			HeroCostume.costume[0].hair_color = new Color(1f, 0.9f, 0.5f);
			HeroCostume.costume[0].division = DIVISION.TheMilitaryPolice;
			HeroCostume.costume[0].costumeId = 0;
			HeroCostume.costume[1] = new HeroCostume();
			HeroCostume.costume[1].name = "annie";
			HeroCostume.costume[1].sex = SEX.FEMALE;
			HeroCostume.costume[1].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[1].part_chest_object_mesh = "character_cap_uniform";
			HeroCostume.costume[1].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
			HeroCostume.costume[1].body_texture = HeroCostume.body_uniform_fb_texture[0];
			HeroCostume.costume[1].cape = false;
			HeroCostume.costume[1].hairInfo = CostumeHair.hairsF[5];
			HeroCostume.costume[1].eye_texture_id = 0;
			HeroCostume.costume[1].beard_texture_id = 33;
			HeroCostume.costume[1].glass_texture_id = -1;
			HeroCostume.costume[1].skin_color = 1;
			HeroCostume.costume[1].hair_color = new Color(1f, 0.9f, 0.5f);
			HeroCostume.costume[1].division = DIVISION.TraineesSquad;
			HeroCostume.costume[1].costumeId = 0;
			HeroCostume.costume[2] = new HeroCostume();
			HeroCostume.costume[2].name = "annie";
			HeroCostume.costume[2].sex = SEX.FEMALE;
			HeroCostume.costume[2].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[2].part_chest_object_mesh = "character_cap_casual";
			HeroCostume.costume[2].part_chest_object_texture = "aottg_hero_annie_cap_causal";
			HeroCostume.costume[2].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[2].part_chest_1_object_texture = HeroCostume.body_casual_fb_texture[0];
			HeroCostume.costume[2].body_texture = HeroCostume.body_casual_fb_texture[0];
			HeroCostume.costume[2].cape = false;
			HeroCostume.costume[2].hairInfo = CostumeHair.hairsF[5];
			HeroCostume.costume[2].eye_texture_id = 0;
			HeroCostume.costume[2].beard_texture_id = 33;
			HeroCostume.costume[2].glass_texture_id = -1;
			HeroCostume.costume[2].skin_color = 1;
			HeroCostume.costume[2].hair_color = new Color(1f, 0.9f, 0.5f);
			HeroCostume.costume[2].costumeId = 1;
			HeroCostume.costume[3] = new HeroCostume();
			HeroCostume.costume[3].name = "mikasa";
			HeroCostume.costume[3].sex = SEX.FEMALE;
			HeroCostume.costume[3].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[3].body_texture = HeroCostume.body_uniform_fb_texture[1];
			HeroCostume.costume[3].cape = true;
			HeroCostume.costume[3].hairInfo = CostumeHair.hairsF[7];
			HeroCostume.costume[3].eye_texture_id = 2;
			HeroCostume.costume[3].beard_texture_id = 33;
			HeroCostume.costume[3].glass_texture_id = -1;
			HeroCostume.costume[3].skin_color = 1;
			HeroCostume.costume[3].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[3].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[3].costumeId = 2;
			HeroCostume.costume[4] = new HeroCostume();
			HeroCostume.costume[4].name = "mikasa";
			HeroCostume.costume[4].sex = SEX.FEMALE;
			HeroCostume.costume[4].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[4].part_chest_skinned_cloth_mesh = "mikasa_asset_uni";
			HeroCostume.costume[4].part_chest_skinned_cloth_texture = HeroCostume.body_uniform_fb_texture[1];
			HeroCostume.costume[4].body_texture = HeroCostume.body_uniform_fb_texture[1];
			HeroCostume.costume[4].cape = false;
			HeroCostume.costume[4].hairInfo = CostumeHair.hairsF[7];
			HeroCostume.costume[4].eye_texture_id = 2;
			HeroCostume.costume[4].beard_texture_id = 33;
			HeroCostume.costume[4].glass_texture_id = -1;
			HeroCostume.costume[4].skin_color = 1;
			HeroCostume.costume[4].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[4].division = DIVISION.TraineesSquad;
			HeroCostume.costume[4].costumeId = 3;
			HeroCostume.costume[5] = new HeroCostume();
			HeroCostume.costume[5].name = "mikasa";
			HeroCostume.costume[5].sex = SEX.FEMALE;
			HeroCostume.costume[5].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[5].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
			HeroCostume.costume[5].part_chest_skinned_cloth_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[5].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[5].part_chest_1_object_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[5].body_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[5].cape = false;
			HeroCostume.costume[5].hairInfo = CostumeHair.hairsF[7];
			HeroCostume.costume[5].eye_texture_id = 2;
			HeroCostume.costume[5].beard_texture_id = 33;
			HeroCostume.costume[5].glass_texture_id = -1;
			HeroCostume.costume[5].skin_color = 1;
			HeroCostume.costume[5].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[5].costumeId = 4;
			HeroCostume.costume[6] = new HeroCostume();
			HeroCostume.costume[6].name = "levi";
			HeroCostume.costume[6].sex = SEX.MALE;
			HeroCostume.costume[6].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[6].body_texture = HeroCostume.body_uniform_mb_texture[1];
			HeroCostume.costume[6].cape = true;
			HeroCostume.costume[6].hairInfo = CostumeHair.hairsM[7];
			HeroCostume.costume[6].eye_texture_id = 1;
			HeroCostume.costume[6].beard_texture_id = -1;
			HeroCostume.costume[6].glass_texture_id = -1;
			HeroCostume.costume[6].skin_color = 1;
			HeroCostume.costume[6].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[6].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[6].costumeId = 11;
			HeroCostume.costume[7] = new HeroCostume();
			HeroCostume.costume[7].name = "levi";
			HeroCostume.costume[7].sex = SEX.MALE;
			HeroCostume.costume[7].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[7].body_texture = HeroCostume.body_casual_mb_texture[1];
			HeroCostume.costume[7].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[7].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[1];
			HeroCostume.costume[7].cape = false;
			HeroCostume.costume[7].hairInfo = CostumeHair.hairsM[7];
			HeroCostume.costume[7].eye_texture_id = 1;
			HeroCostume.costume[7].beard_texture_id = -1;
			HeroCostume.costume[7].glass_texture_id = -1;
			HeroCostume.costume[7].skin_color = 1;
			HeroCostume.costume[7].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[7].costumeId = 12;
			HeroCostume.costume[8] = new HeroCostume();
			HeroCostume.costume[8].name = "eren";
			HeroCostume.costume[8].sex = SEX.MALE;
			HeroCostume.costume[8].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[8].body_texture = HeroCostume.body_uniform_mb_texture[0];
			HeroCostume.costume[8].cape = true;
			HeroCostume.costume[8].hairInfo = CostumeHair.hairsM[4];
			HeroCostume.costume[8].eye_texture_id = 3;
			HeroCostume.costume[8].beard_texture_id = -1;
			HeroCostume.costume[8].glass_texture_id = -1;
			HeroCostume.costume[8].skin_color = 1;
			HeroCostume.costume[8].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[8].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[8].costumeId = 13;
			HeroCostume.costume[9] = new HeroCostume();
			HeroCostume.costume[9].name = "eren";
			HeroCostume.costume[9].sex = SEX.MALE;
			HeroCostume.costume[9].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[9].body_texture = HeroCostume.body_uniform_mb_texture[0];
			HeroCostume.costume[9].cape = false;
			HeroCostume.costume[9].hairInfo = CostumeHair.hairsM[4];
			HeroCostume.costume[9].eye_texture_id = 3;
			HeroCostume.costume[9].beard_texture_id = -1;
			HeroCostume.costume[9].glass_texture_id = -1;
			HeroCostume.costume[9].skin_color = 1;
			HeroCostume.costume[9].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[9].division = DIVISION.TraineesSquad;
			HeroCostume.costume[9].costumeId = 13;
			HeroCostume.costume[10] = new HeroCostume();
			HeroCostume.costume[10].name = "eren";
			HeroCostume.costume[10].sex = SEX.MALE;
			HeroCostume.costume[10].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[10].body_texture = HeroCostume.body_casual_mb_texture[0];
			HeroCostume.costume[10].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[10].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[0];
			HeroCostume.costume[10].cape = false;
			HeroCostume.costume[10].hairInfo = CostumeHair.hairsM[4];
			HeroCostume.costume[10].eye_texture_id = 3;
			HeroCostume.costume[10].beard_texture_id = -1;
			HeroCostume.costume[10].glass_texture_id = -1;
			HeroCostume.costume[10].skin_color = 1;
			HeroCostume.costume[10].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[10].costumeId = 14;
			HeroCostume.costume[11] = new HeroCostume();
			HeroCostume.costume[11].name = "sasha";
			HeroCostume.costume[11].sex = SEX.FEMALE;
			HeroCostume.costume[11].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[11].body_texture = HeroCostume.body_uniform_fa_texture[1];
			HeroCostume.costume[11].cape = true;
			HeroCostume.costume[11].hairInfo = CostumeHair.hairsF[10];
			HeroCostume.costume[11].eye_texture_id = 4;
			HeroCostume.costume[11].beard_texture_id = 33;
			HeroCostume.costume[11].glass_texture_id = -1;
			HeroCostume.costume[11].skin_color = 1;
			HeroCostume.costume[11].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[11].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[11].costumeId = 5;
			HeroCostume.costume[12] = new HeroCostume();
			HeroCostume.costume[12].name = "sasha";
			HeroCostume.costume[12].sex = SEX.FEMALE;
			HeroCostume.costume[12].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[12].body_texture = HeroCostume.body_uniform_fa_texture[1];
			HeroCostume.costume[12].cape = false;
			HeroCostume.costume[12].hairInfo = CostumeHair.hairsF[10];
			HeroCostume.costume[12].eye_texture_id = 4;
			HeroCostume.costume[12].beard_texture_id = 33;
			HeroCostume.costume[12].glass_texture_id = -1;
			HeroCostume.costume[12].skin_color = 1;
			HeroCostume.costume[12].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[12].division = DIVISION.TraineesSquad;
			HeroCostume.costume[12].costumeId = 5;
			HeroCostume.costume[13] = new HeroCostume();
			HeroCostume.costume[13].name = "sasha";
			HeroCostume.costume[13].sex = SEX.FEMALE;
			HeroCostume.costume[13].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[13].body_texture = HeroCostume.body_casual_fa_texture[1];
			HeroCostume.costume[13].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[13].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[1];
			HeroCostume.costume[13].cape = false;
			HeroCostume.costume[13].hairInfo = CostumeHair.hairsF[10];
			HeroCostume.costume[13].eye_texture_id = 4;
			HeroCostume.costume[13].beard_texture_id = 33;
			HeroCostume.costume[13].glass_texture_id = -1;
			HeroCostume.costume[13].skin_color = 1;
			HeroCostume.costume[13].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[13].costumeId = 6;
			HeroCostume.costume[14] = new HeroCostume();
			HeroCostume.costume[14].name = "hanji";
			HeroCostume.costume[14].sex = SEX.FEMALE;
			HeroCostume.costume[14].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[14].body_texture = HeroCostume.body_uniform_fa_texture[2];
			HeroCostume.costume[14].cape = true;
			HeroCostume.costume[14].hairInfo = CostumeHair.hairsF[6];
			HeroCostume.costume[14].eye_texture_id = 5;
			HeroCostume.costume[14].beard_texture_id = 33;
			HeroCostume.costume[14].glass_texture_id = 49;
			HeroCostume.costume[14].skin_color = 1;
			HeroCostume.costume[14].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[14].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[14].costumeId = 7;
			HeroCostume.costume[15] = new HeroCostume();
			HeroCostume.costume[15].name = "hanji";
			HeroCostume.costume[15].sex = SEX.FEMALE;
			HeroCostume.costume[15].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[15].body_texture = HeroCostume.body_casual_fa_texture[2];
			HeroCostume.costume[15].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[15].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[2];
			HeroCostume.costume[15].cape = false;
			HeroCostume.costume[15].hairInfo = CostumeHair.hairsF[6];
			HeroCostume.costume[15].eye_texture_id = 5;
			HeroCostume.costume[15].beard_texture_id = 33;
			HeroCostume.costume[15].glass_texture_id = 49;
			HeroCostume.costume[15].skin_color = 1;
			HeroCostume.costume[15].hair_color = new Color(0.295f, 0.23f, 0.17f);
			HeroCostume.costume[15].costumeId = 8;
			HeroCostume.costume[16] = new HeroCostume();
			HeroCostume.costume[16].name = "rico";
			HeroCostume.costume[16].sex = SEX.FEMALE;
			HeroCostume.costume[16].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[16].body_texture = HeroCostume.body_uniform_fa_texture[0];
			HeroCostume.costume[16].cape = true;
			HeroCostume.costume[16].hairInfo = CostumeHair.hairsF[9];
			HeroCostume.costume[16].eye_texture_id = 6;
			HeroCostume.costume[16].beard_texture_id = 33;
			HeroCostume.costume[16].glass_texture_id = 48;
			HeroCostume.costume[16].skin_color = 1;
			HeroCostume.costume[16].hair_color = new Color(1f, 1f, 1f);
			HeroCostume.costume[16].division = DIVISION.TheGarrison;
			HeroCostume.costume[16].costumeId = 9;
			HeroCostume.costume[17] = new HeroCostume();
			HeroCostume.costume[17].name = "rico";
			HeroCostume.costume[17].sex = SEX.FEMALE;
			HeroCostume.costume[17].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[17].body_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[17].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[17].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[17].cape = false;
			HeroCostume.costume[17].hairInfo = CostumeHair.hairsF[9];
			HeroCostume.costume[17].eye_texture_id = 6;
			HeroCostume.costume[17].beard_texture_id = 33;
			HeroCostume.costume[17].glass_texture_id = 48;
			HeroCostume.costume[17].skin_color = 1;
			HeroCostume.costume[17].hair_color = new Color(1f, 1f, 1f);
			HeroCostume.costume[17].costumeId = 10;
			HeroCostume.costume[18] = new HeroCostume();
			HeroCostume.costume[18].name = "jean";
			HeroCostume.costume[18].sex = SEX.MALE;
			HeroCostume.costume[18].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[18].body_texture = HeroCostume.body_uniform_ma_texture[1];
			HeroCostume.costume[18].cape = true;
			HeroCostume.costume[18].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[18].eye_texture_id = 7;
			HeroCostume.costume[18].beard_texture_id = -1;
			HeroCostume.costume[18].glass_texture_id = -1;
			HeroCostume.costume[18].skin_color = 1;
			HeroCostume.costume[18].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[18].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[18].costumeId = 15;
			HeroCostume.costume[19] = new HeroCostume();
			HeroCostume.costume[19].name = "jean";
			HeroCostume.costume[19].sex = SEX.MALE;
			HeroCostume.costume[19].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[19].body_texture = HeroCostume.body_uniform_ma_texture[1];
			HeroCostume.costume[19].cape = false;
			HeroCostume.costume[19].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[19].eye_texture_id = 7;
			HeroCostume.costume[19].beard_texture_id = -1;
			HeroCostume.costume[19].glass_texture_id = -1;
			HeroCostume.costume[19].skin_color = 1;
			HeroCostume.costume[19].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[19].division = DIVISION.TraineesSquad;
			HeroCostume.costume[19].costumeId = 15;
			HeroCostume.costume[20] = new HeroCostume();
			HeroCostume.costume[20].name = "jean";
			HeroCostume.costume[20].sex = SEX.MALE;
			HeroCostume.costume[20].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[20].body_texture = HeroCostume.body_casual_ma_texture[1];
			HeroCostume.costume[20].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[20].part_chest_1_object_texture = HeroCostume.body_casual_ma_texture[1];
			HeroCostume.costume[20].cape = false;
			HeroCostume.costume[20].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[20].eye_texture_id = 7;
			HeroCostume.costume[20].beard_texture_id = -1;
			HeroCostume.costume[20].glass_texture_id = -1;
			HeroCostume.costume[20].skin_color = 1;
			HeroCostume.costume[20].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[20].costumeId = 16;
			HeroCostume.costume[21] = new HeroCostume();
			HeroCostume.costume[21].name = "marco";
			HeroCostume.costume[21].sex = SEX.MALE;
			HeroCostume.costume[21].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[21].body_texture = HeroCostume.body_uniform_ma_texture[2];
			HeroCostume.costume[21].cape = false;
			HeroCostume.costume[21].hairInfo = CostumeHair.hairsM[8];
			HeroCostume.costume[21].eye_texture_id = 8;
			HeroCostume.costume[21].beard_texture_id = -1;
			HeroCostume.costume[21].glass_texture_id = -1;
			HeroCostume.costume[21].skin_color = 1;
			HeroCostume.costume[21].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[21].division = DIVISION.TraineesSquad;
			HeroCostume.costume[21].costumeId = 17;
			HeroCostume.costume[22] = new HeroCostume();
			HeroCostume.costume[22].name = "marco";
			HeroCostume.costume[22].sex = SEX.MALE;
			HeroCostume.costume[22].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[22].body_texture = HeroCostume.body_casual_ma_texture[2];
			HeroCostume.costume[22].cape = false;
			HeroCostume.costume[22].hairInfo = CostumeHair.hairsM[8];
			HeroCostume.costume[22].eye_texture_id = 8;
			HeroCostume.costume[22].beard_texture_id = -1;
			HeroCostume.costume[22].glass_texture_id = -1;
			HeroCostume.costume[22].skin_color = 1;
			HeroCostume.costume[22].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[22].costumeId = 18;
			HeroCostume.costume[23] = new HeroCostume();
			HeroCostume.costume[23].name = "mike";
			HeroCostume.costume[23].sex = SEX.MALE;
			HeroCostume.costume[23].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[23].body_texture = HeroCostume.body_uniform_mb_texture[3];
			HeroCostume.costume[23].cape = true;
			HeroCostume.costume[23].hairInfo = CostumeHair.hairsM[9];
			HeroCostume.costume[23].eye_texture_id = 9;
			HeroCostume.costume[23].beard_texture_id = 32;
			HeroCostume.costume[23].glass_texture_id = -1;
			HeroCostume.costume[23].skin_color = 1;
			HeroCostume.costume[23].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[23].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[23].costumeId = 19;
			HeroCostume.costume[24] = new HeroCostume();
			HeroCostume.costume[24].name = "mike";
			HeroCostume.costume[24].sex = SEX.MALE;
			HeroCostume.costume[24].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[24].body_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[24].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[24].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[24].cape = false;
			HeroCostume.costume[24].hairInfo = CostumeHair.hairsM[9];
			HeroCostume.costume[24].eye_texture_id = 9;
			HeroCostume.costume[24].beard_texture_id = 32;
			HeroCostume.costume[24].glass_texture_id = -1;
			HeroCostume.costume[24].skin_color = 1;
			HeroCostume.costume[24].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[24].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[24].costumeId = 20;
			HeroCostume.costume[25] = new HeroCostume();
			HeroCostume.costume[25].name = "connie";
			HeroCostume.costume[25].sex = SEX.MALE;
			HeroCostume.costume[25].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[25].body_texture = HeroCostume.body_uniform_mb_texture[2];
			HeroCostume.costume[25].cape = true;
			HeroCostume.costume[25].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[25].eye_texture_id = 10;
			HeroCostume.costume[25].beard_texture_id = -1;
			HeroCostume.costume[25].glass_texture_id = -1;
			HeroCostume.costume[25].skin_color = 1;
			HeroCostume.costume[25].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[25].costumeId = 21;
			HeroCostume.costume[26] = new HeroCostume();
			HeroCostume.costume[26].name = "connie";
			HeroCostume.costume[26].sex = SEX.MALE;
			HeroCostume.costume[26].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[26].body_texture = HeroCostume.body_uniform_mb_texture[2];
			HeroCostume.costume[26].cape = false;
			HeroCostume.costume[26].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[26].eye_texture_id = 10;
			HeroCostume.costume[26].beard_texture_id = -1;
			HeroCostume.costume[26].glass_texture_id = -1;
			HeroCostume.costume[26].skin_color = 1;
			HeroCostume.costume[26].division = DIVISION.TraineesSquad;
			HeroCostume.costume[26].costumeId = 21;
			HeroCostume.costume[27] = new HeroCostume();
			HeroCostume.costume[27].name = "connie";
			HeroCostume.costume[27].sex = SEX.MALE;
			HeroCostume.costume[27].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[27].body_texture = HeroCostume.body_casual_mb_texture[2];
			HeroCostume.costume[27].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[27].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[2];
			HeroCostume.costume[27].cape = false;
			HeroCostume.costume[27].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[27].eye_texture_id = 10;
			HeroCostume.costume[27].beard_texture_id = -1;
			HeroCostume.costume[27].glass_texture_id = -1;
			HeroCostume.costume[27].skin_color = 1;
			HeroCostume.costume[27].costumeId = 22;
			HeroCostume.costume[28] = new HeroCostume();
			HeroCostume.costume[28].name = "armin";
			HeroCostume.costume[28].sex = SEX.MALE;
			HeroCostume.costume[28].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[28].body_texture = HeroCostume.body_uniform_ma_texture[0];
			HeroCostume.costume[28].cape = true;
			HeroCostume.costume[28].hairInfo = CostumeHair.hairsM[5];
			HeroCostume.costume[28].eye_texture_id = 11;
			HeroCostume.costume[28].beard_texture_id = -1;
			HeroCostume.costume[28].glass_texture_id = -1;
			HeroCostume.costume[28].skin_color = 1;
			HeroCostume.costume[28].hair_color = new Color(0.95f, 0.8f, 0.5f);
			HeroCostume.costume[28].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[28].costumeId = 23;
			HeroCostume.costume[29] = new HeroCostume();
			HeroCostume.costume[29].name = "armin";
			HeroCostume.costume[29].sex = SEX.MALE;
			HeroCostume.costume[29].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[29].body_texture = HeroCostume.body_uniform_ma_texture[0];
			HeroCostume.costume[29].cape = false;
			HeroCostume.costume[29].hairInfo = CostumeHair.hairsM[5];
			HeroCostume.costume[29].eye_texture_id = 11;
			HeroCostume.costume[29].beard_texture_id = -1;
			HeroCostume.costume[29].glass_texture_id = -1;
			HeroCostume.costume[29].skin_color = 1;
			HeroCostume.costume[29].hair_color = new Color(0.95f, 0.8f, 0.5f);
			HeroCostume.costume[29].division = DIVISION.TraineesSquad;
			HeroCostume.costume[29].costumeId = 23;
			HeroCostume.costume[30] = new HeroCostume();
			HeroCostume.costume[30].name = "armin";
			HeroCostume.costume[30].sex = SEX.MALE;
			HeroCostume.costume[30].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[30].body_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[30].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[30].part_chest_1_object_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[30].cape = false;
			HeroCostume.costume[30].hairInfo = CostumeHair.hairsM[5];
			HeroCostume.costume[30].eye_texture_id = 11;
			HeroCostume.costume[30].beard_texture_id = -1;
			HeroCostume.costume[30].glass_texture_id = -1;
			HeroCostume.costume[30].skin_color = 1;
			HeroCostume.costume[30].hair_color = new Color(0.95f, 0.8f, 0.5f);
			HeroCostume.costume[30].costumeId = 24;
			HeroCostume.costume[31] = new HeroCostume();
			HeroCostume.costume[31].name = "petra";
			HeroCostume.costume[31].sex = SEX.FEMALE;
			HeroCostume.costume[31].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[31].body_texture = HeroCostume.body_uniform_fa_texture[0];
			HeroCostume.costume[31].cape = true;
			HeroCostume.costume[31].hairInfo = CostumeHair.hairsF[8];
			HeroCostume.costume[31].eye_texture_id = 27;
			HeroCostume.costume[31].beard_texture_id = -1;
			HeroCostume.costume[31].glass_texture_id = -1;
			HeroCostume.costume[31].skin_color = 1;
			HeroCostume.costume[31].hair_color = new Color(1f, 0.725f, 0.376f);
			HeroCostume.costume[31].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[31].costumeId = 9;
			HeroCostume.costume[32] = new HeroCostume();
			HeroCostume.costume[32].name = "petra";
			HeroCostume.costume[32].sex = SEX.FEMALE;
			HeroCostume.costume[32].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[32].body_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[32].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[32].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[32].cape = false;
			HeroCostume.costume[32].hairInfo = CostumeHair.hairsF[8];
			HeroCostume.costume[32].eye_texture_id = 27;
			HeroCostume.costume[32].beard_texture_id = -1;
			HeroCostume.costume[32].glass_texture_id = -1;
			HeroCostume.costume[32].skin_color = 1;
			HeroCostume.costume[32].hair_color = new Color(1f, 0.725f, 0.376f);
			HeroCostume.costume[32].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[32].costumeId = 10;
			HeroCostume.costume[33] = new HeroCostume();
			HeroCostume.costume[33].name = "custom";
			HeroCostume.costume[33].sex = SEX.FEMALE;
			HeroCostume.costume[33].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[33].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
			HeroCostume.costume[33].part_chest_skinned_cloth_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[33].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[33].part_chest_1_object_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[33].body_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[33].cape = false;
			HeroCostume.costume[33].hairInfo = CostumeHair.hairsF[2];
			HeroCostume.costume[33].eye_texture_id = 12;
			HeroCostume.costume[33].beard_texture_id = 33;
			HeroCostume.costume[33].glass_texture_id = -1;
			HeroCostume.costume[33].skin_color = 1;
			HeroCostume.costume[33].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[33].costumeId = 4;
			HeroCostume.costume[34] = new HeroCostume();
			HeroCostume.costume[34].name = "custom";
			HeroCostume.costume[34].sex = SEX.MALE;
			HeroCostume.costume[34].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[34].body_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[34].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[34].part_chest_1_object_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[34].cape = false;
			HeroCostume.costume[34].hairInfo = CostumeHair.hairsM[3];
			HeroCostume.costume[34].eye_texture_id = 26;
			HeroCostume.costume[34].beard_texture_id = 44;
			HeroCostume.costume[34].glass_texture_id = -1;
			HeroCostume.costume[34].skin_color = 1;
			HeroCostume.costume[34].hair_color = new Color(0.41f, 1f, 0f);
			HeroCostume.costume[34].costumeId = 24;
			HeroCostume.costume[35] = new HeroCostume();
			HeroCostume.costume[35].name = "custom";
			HeroCostume.costume[35].sex = SEX.FEMALE;
			HeroCostume.costume[35].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[35].body_texture = HeroCostume.body_uniform_fa_texture[1];
			HeroCostume.costume[35].cape = false;
			HeroCostume.costume[35].hairInfo = CostumeHair.hairsF[4];
			HeroCostume.costume[35].eye_texture_id = 22;
			HeroCostume.costume[35].beard_texture_id = 33;
			HeroCostume.costume[35].glass_texture_id = 56;
			HeroCostume.costume[35].skin_color = 1;
			HeroCostume.costume[35].hair_color = new Color(0f, 1f, 0.874f);
			HeroCostume.costume[35].costumeId = 5;
			HeroCostume.costume[36] = new HeroCostume();
			HeroCostume.costume[36].name = "feng";
			HeroCostume.costume[36].sex = SEX.MALE;
			HeroCostume.costume[36].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[36].body_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[36].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[36].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[36].cape = true;
			HeroCostume.costume[36].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[36].eye_texture_id = 25;
			HeroCostume.costume[36].beard_texture_id = 39;
			HeroCostume.costume[36].glass_texture_id = 53;
			HeroCostume.costume[36].skin_color = 1;
			HeroCostume.costume[36].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[36].costumeId = 20;
			HeroCostume.costume[37] = new HeroCostume();
			HeroCostume.costume[37].name = "AHSS";
			HeroCostume.costume[37].sex = SEX.MALE;
			HeroCostume.costume[37].uniform_type = UNIFORM_TYPE.CasualAHSS;
			HeroCostume.costume[37].body_texture = HeroCostume.body_casual_ma_texture[0] + "_ahss";
			HeroCostume.costume[37].cape = false;
			HeroCostume.costume[37].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[37].eye_texture_id = 25;
			HeroCostume.costume[37].beard_texture_id = 39;
			HeroCostume.costume[37].glass_texture_id = 53;
			HeroCostume.costume[37].skin_color = 3;
			HeroCostume.costume[37].division = DIVISION.TheMilitaryPolice;
			HeroCostume.costume[37].costumeId = 25;
			for (int i = 0; i < HeroCostume.costume.Length; i++)
			{
				HeroCostume.costume[i].stat = HeroStat.getInfo("CUSTOM_DEFAULT");
				HeroCostume.costume[i].id = i;
				HeroCostume.costume[i].setMesh2();
				HeroCostume.costume[i].setTexture();
			}
			HeroCostume.costumeOption = new HeroCostume[26]
			{
				HeroCostume.costume[0],
				HeroCostume.costume[2],
				HeroCostume.costume[3],
				HeroCostume.costume[4],
				HeroCostume.costume[5],
				HeroCostume.costume[11],
				HeroCostume.costume[13],
				HeroCostume.costume[14],
				HeroCostume.costume[15],
				HeroCostume.costume[16],
				HeroCostume.costume[17],
				HeroCostume.costume[6],
				HeroCostume.costume[7],
				HeroCostume.costume[8],
				HeroCostume.costume[10],
				HeroCostume.costume[18],
				HeroCostume.costume[19],
				HeroCostume.costume[21],
				HeroCostume.costume[22],
				HeroCostume.costume[23],
				HeroCostume.costume[24],
				HeroCostume.costume[25],
				HeroCostume.costume[27],
				HeroCostume.costume[28],
				HeroCostume.costume[30],
				HeroCostume.costume[37]
			};
		}
	}

	public static void init2()
	{
		if (!HeroCostume.inited)
		{
			HeroCostume.inited = true;
			CostumeHair.init();
			HeroCostume.body_uniform_ma_texture = new string[3] { "aottg_hero_uniform_ma_1", "aottg_hero_uniform_ma_2", "aottg_hero_uniform_ma_3" };
			HeroCostume.body_uniform_fa_texture = new string[3] { "aottg_hero_uniform_fa_1", "aottg_hero_uniform_fa_2", "aottg_hero_uniform_fa_3" };
			HeroCostume.body_uniform_mb_texture = new string[4] { "aottg_hero_uniform_mb_1", "aottg_hero_uniform_mb_2", "aottg_hero_uniform_mb_3", "aottg_hero_uniform_mb_4" };
			HeroCostume.body_uniform_fb_texture = new string[2] { "aottg_hero_uniform_fb_1", "aottg_hero_uniform_fb_2" };
			HeroCostume.body_casual_ma_texture = new string[3] { "aottg_hero_casual_ma_1", "aottg_hero_casual_ma_2", "aottg_hero_casual_ma_3" };
			HeroCostume.body_casual_fa_texture = new string[3] { "aottg_hero_casual_fa_1", "aottg_hero_casual_fa_2", "aottg_hero_casual_fa_3" };
			HeroCostume.body_casual_mb_texture = new string[4] { "aottg_hero_casual_mb_1", "aottg_hero_casual_mb_2", "aottg_hero_casual_mb_3", "aottg_hero_casual_mb_4" };
			HeroCostume.body_casual_fb_texture = new string[2] { "aottg_hero_casual_fb_1", "aottg_hero_casual_fb_2" };
			HeroCostume.costume = new HeroCostume[39];
			HeroCostume.costume[0] = new HeroCostume();
			HeroCostume.costume[0].name = "annie";
			HeroCostume.costume[0].sex = SEX.FEMALE;
			HeroCostume.costume[0].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[0].part_chest_object_mesh = "character_cap_uniform";
			HeroCostume.costume[0].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
			HeroCostume.costume[0].cape = true;
			HeroCostume.costume[0].body_texture = HeroCostume.body_uniform_fb_texture[0];
			HeroCostume.costume[0].hairInfo = CostumeHair.hairsF[5];
			HeroCostume.costume[0].eye_texture_id = 0;
			HeroCostume.costume[0].beard_texture_id = 33;
			HeroCostume.costume[0].glass_texture_id = -1;
			HeroCostume.costume[0].skin_color = 1;
			HeroCostume.costume[0].hair_color = new Color(1f, 0.9f, 0.5f);
			HeroCostume.costume[0].division = DIVISION.TheMilitaryPolice;
			HeroCostume.costume[0].costumeId = 0;
			HeroCostume.costume[1] = new HeroCostume();
			HeroCostume.costume[1].name = "annie";
			HeroCostume.costume[1].sex = SEX.FEMALE;
			HeroCostume.costume[1].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[1].part_chest_object_mesh = "character_cap_uniform";
			HeroCostume.costume[1].part_chest_object_texture = "aottg_hero_annie_cap_uniform";
			HeroCostume.costume[1].body_texture = HeroCostume.body_uniform_fb_texture[0];
			HeroCostume.costume[1].cape = false;
			HeroCostume.costume[1].hairInfo = CostumeHair.hairsF[5];
			HeroCostume.costume[1].eye_texture_id = 0;
			HeroCostume.costume[1].beard_texture_id = 33;
			HeroCostume.costume[1].glass_texture_id = -1;
			HeroCostume.costume[1].skin_color = 1;
			HeroCostume.costume[1].hair_color = new Color(1f, 0.9f, 0.5f);
			HeroCostume.costume[1].division = DIVISION.TraineesSquad;
			HeroCostume.costume[1].costumeId = 0;
			HeroCostume.costume[2] = new HeroCostume();
			HeroCostume.costume[2].name = "annie";
			HeroCostume.costume[2].sex = SEX.FEMALE;
			HeroCostume.costume[2].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[2].part_chest_object_mesh = "character_cap_casual";
			HeroCostume.costume[2].part_chest_object_texture = "aottg_hero_annie_cap_causal";
			HeroCostume.costume[2].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[2].part_chest_1_object_texture = HeroCostume.body_casual_fb_texture[0];
			HeroCostume.costume[2].body_texture = HeroCostume.body_casual_fb_texture[0];
			HeroCostume.costume[2].cape = false;
			HeroCostume.costume[2].hairInfo = CostumeHair.hairsF[5];
			HeroCostume.costume[2].eye_texture_id = 0;
			HeroCostume.costume[2].beard_texture_id = 33;
			HeroCostume.costume[2].glass_texture_id = -1;
			HeroCostume.costume[2].skin_color = 1;
			HeroCostume.costume[2].hair_color = new Color(1f, 0.9f, 0.5f);
			HeroCostume.costume[2].costumeId = 1;
			HeroCostume.costume[3] = new HeroCostume();
			HeroCostume.costume[3].name = "mikasa";
			HeroCostume.costume[3].sex = SEX.FEMALE;
			HeroCostume.costume[3].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[3].body_texture = HeroCostume.body_uniform_fb_texture[1];
			HeroCostume.costume[3].cape = true;
			HeroCostume.costume[3].hairInfo = CostumeHair.hairsF[7];
			HeroCostume.costume[3].eye_texture_id = 2;
			HeroCostume.costume[3].beard_texture_id = 33;
			HeroCostume.costume[3].glass_texture_id = -1;
			HeroCostume.costume[3].skin_color = 1;
			HeroCostume.costume[3].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[3].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[3].costumeId = 2;
			HeroCostume.costume[4] = new HeroCostume();
			HeroCostume.costume[4].name = "mikasa";
			HeroCostume.costume[4].sex = SEX.FEMALE;
			HeroCostume.costume[4].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[4].part_chest_skinned_cloth_mesh = "mikasa_asset_uni";
			HeroCostume.costume[4].part_chest_skinned_cloth_texture = HeroCostume.body_uniform_fb_texture[1];
			HeroCostume.costume[4].body_texture = HeroCostume.body_uniform_fb_texture[1];
			HeroCostume.costume[4].cape = false;
			HeroCostume.costume[4].hairInfo = CostumeHair.hairsF[7];
			HeroCostume.costume[4].eye_texture_id = 2;
			HeroCostume.costume[4].beard_texture_id = 33;
			HeroCostume.costume[4].glass_texture_id = -1;
			HeroCostume.costume[4].skin_color = 1;
			HeroCostume.costume[4].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[4].division = DIVISION.TraineesSquad;
			HeroCostume.costume[4].costumeId = 3;
			HeroCostume.costume[5] = new HeroCostume();
			HeroCostume.costume[5].name = "mikasa";
			HeroCostume.costume[5].sex = SEX.FEMALE;
			HeroCostume.costume[5].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[5].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
			HeroCostume.costume[5].part_chest_skinned_cloth_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[5].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[5].part_chest_1_object_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[5].body_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[5].cape = false;
			HeroCostume.costume[5].hairInfo = CostumeHair.hairsF[7];
			HeroCostume.costume[5].eye_texture_id = 2;
			HeroCostume.costume[5].beard_texture_id = 33;
			HeroCostume.costume[5].glass_texture_id = -1;
			HeroCostume.costume[5].skin_color = 1;
			HeroCostume.costume[5].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[5].costumeId = 4;
			HeroCostume.costume[6] = new HeroCostume();
			HeroCostume.costume[6].name = "levi";
			HeroCostume.costume[6].sex = SEX.MALE;
			HeroCostume.costume[6].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[6].body_texture = HeroCostume.body_uniform_mb_texture[1];
			HeroCostume.costume[6].cape = true;
			HeroCostume.costume[6].hairInfo = CostumeHair.hairsM[7];
			HeroCostume.costume[6].eye_texture_id = 1;
			HeroCostume.costume[6].beard_texture_id = -1;
			HeroCostume.costume[6].glass_texture_id = -1;
			HeroCostume.costume[6].skin_color = 1;
			HeroCostume.costume[6].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[6].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[6].costumeId = 11;
			HeroCostume.costume[7] = new HeroCostume();
			HeroCostume.costume[7].name = "levi";
			HeroCostume.costume[7].sex = SEX.MALE;
			HeroCostume.costume[7].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[7].body_texture = HeroCostume.body_casual_mb_texture[1];
			HeroCostume.costume[7].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[7].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[1];
			HeroCostume.costume[7].cape = false;
			HeroCostume.costume[7].hairInfo = CostumeHair.hairsM[7];
			HeroCostume.costume[7].eye_texture_id = 1;
			HeroCostume.costume[7].beard_texture_id = -1;
			HeroCostume.costume[7].glass_texture_id = -1;
			HeroCostume.costume[7].skin_color = 1;
			HeroCostume.costume[7].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[7].costumeId = 12;
			HeroCostume.costume[8] = new HeroCostume();
			HeroCostume.costume[8].name = "eren";
			HeroCostume.costume[8].sex = SEX.MALE;
			HeroCostume.costume[8].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[8].body_texture = HeroCostume.body_uniform_mb_texture[0];
			HeroCostume.costume[8].cape = true;
			HeroCostume.costume[8].hairInfo = CostumeHair.hairsM[4];
			HeroCostume.costume[8].eye_texture_id = 3;
			HeroCostume.costume[8].beard_texture_id = -1;
			HeroCostume.costume[8].glass_texture_id = -1;
			HeroCostume.costume[8].skin_color = 1;
			HeroCostume.costume[8].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[8].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[8].costumeId = 13;
			HeroCostume.costume[9] = new HeroCostume();
			HeroCostume.costume[9].name = "eren";
			HeroCostume.costume[9].sex = SEX.MALE;
			HeroCostume.costume[9].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[9].body_texture = HeroCostume.body_uniform_mb_texture[0];
			HeroCostume.costume[9].cape = false;
			HeroCostume.costume[9].hairInfo = CostumeHair.hairsM[4];
			HeroCostume.costume[9].eye_texture_id = 3;
			HeroCostume.costume[9].beard_texture_id = -1;
			HeroCostume.costume[9].glass_texture_id = -1;
			HeroCostume.costume[9].skin_color = 1;
			HeroCostume.costume[9].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[9].division = DIVISION.TraineesSquad;
			HeroCostume.costume[9].costumeId = 13;
			HeroCostume.costume[10] = new HeroCostume();
			HeroCostume.costume[10].name = "eren";
			HeroCostume.costume[10].sex = SEX.MALE;
			HeroCostume.costume[10].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[10].body_texture = HeroCostume.body_casual_mb_texture[0];
			HeroCostume.costume[10].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[10].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[0];
			HeroCostume.costume[10].cape = false;
			HeroCostume.costume[10].hairInfo = CostumeHair.hairsM[4];
			HeroCostume.costume[10].eye_texture_id = 3;
			HeroCostume.costume[10].beard_texture_id = -1;
			HeroCostume.costume[10].glass_texture_id = -1;
			HeroCostume.costume[10].skin_color = 1;
			HeroCostume.costume[10].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[10].costumeId = 14;
			HeroCostume.costume[11] = new HeroCostume();
			HeroCostume.costume[11].name = "sasha";
			HeroCostume.costume[11].sex = SEX.FEMALE;
			HeroCostume.costume[11].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[11].body_texture = HeroCostume.body_uniform_fa_texture[1];
			HeroCostume.costume[11].cape = true;
			HeroCostume.costume[11].hairInfo = CostumeHair.hairsF[10];
			HeroCostume.costume[11].eye_texture_id = 4;
			HeroCostume.costume[11].beard_texture_id = 33;
			HeroCostume.costume[11].glass_texture_id = -1;
			HeroCostume.costume[11].skin_color = 1;
			HeroCostume.costume[11].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[11].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[11].costumeId = 5;
			HeroCostume.costume[12] = new HeroCostume();
			HeroCostume.costume[12].name = "sasha";
			HeroCostume.costume[12].sex = SEX.FEMALE;
			HeroCostume.costume[12].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[12].body_texture = HeroCostume.body_uniform_fa_texture[1];
			HeroCostume.costume[12].cape = false;
			HeroCostume.costume[12].hairInfo = CostumeHair.hairsF[10];
			HeroCostume.costume[12].eye_texture_id = 4;
			HeroCostume.costume[12].beard_texture_id = 33;
			HeroCostume.costume[12].glass_texture_id = -1;
			HeroCostume.costume[12].skin_color = 1;
			HeroCostume.costume[12].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[12].division = DIVISION.TraineesSquad;
			HeroCostume.costume[12].costumeId = 5;
			HeroCostume.costume[13] = new HeroCostume();
			HeroCostume.costume[13].name = "sasha";
			HeroCostume.costume[13].sex = SEX.FEMALE;
			HeroCostume.costume[13].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[13].body_texture = HeroCostume.body_casual_fa_texture[1];
			HeroCostume.costume[13].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[13].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[1];
			HeroCostume.costume[13].cape = false;
			HeroCostume.costume[13].hairInfo = CostumeHair.hairsF[10];
			HeroCostume.costume[13].eye_texture_id = 4;
			HeroCostume.costume[13].beard_texture_id = 33;
			HeroCostume.costume[13].glass_texture_id = -1;
			HeroCostume.costume[13].skin_color = 1;
			HeroCostume.costume[13].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[13].costumeId = 6;
			HeroCostume.costume[14] = new HeroCostume();
			HeroCostume.costume[14].name = "hanji";
			HeroCostume.costume[14].sex = SEX.FEMALE;
			HeroCostume.costume[14].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[14].body_texture = HeroCostume.body_uniform_fa_texture[2];
			HeroCostume.costume[14].cape = true;
			HeroCostume.costume[14].hairInfo = CostumeHair.hairsF[6];
			HeroCostume.costume[14].eye_texture_id = 5;
			HeroCostume.costume[14].beard_texture_id = 33;
			HeroCostume.costume[14].glass_texture_id = 49;
			HeroCostume.costume[14].skin_color = 1;
			HeroCostume.costume[14].hair_color = new Color(0.45f, 0.33f, 0.255f);
			HeroCostume.costume[14].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[14].costumeId = 7;
			HeroCostume.costume[15] = new HeroCostume();
			HeroCostume.costume[15].name = "hanji";
			HeroCostume.costume[15].sex = SEX.FEMALE;
			HeroCostume.costume[15].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[15].body_texture = HeroCostume.body_casual_fa_texture[2];
			HeroCostume.costume[15].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[15].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[2];
			HeroCostume.costume[15].cape = false;
			HeroCostume.costume[15].hairInfo = CostumeHair.hairsF[6];
			HeroCostume.costume[15].eye_texture_id = 5;
			HeroCostume.costume[15].beard_texture_id = 33;
			HeroCostume.costume[15].glass_texture_id = 49;
			HeroCostume.costume[15].skin_color = 1;
			HeroCostume.costume[15].hair_color = new Color(0.295f, 0.23f, 0.17f);
			HeroCostume.costume[15].costumeId = 8;
			HeroCostume.costume[16] = new HeroCostume();
			HeroCostume.costume[16].name = "rico";
			HeroCostume.costume[16].sex = SEX.FEMALE;
			HeroCostume.costume[16].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[16].body_texture = HeroCostume.body_uniform_fa_texture[0];
			HeroCostume.costume[16].cape = true;
			HeroCostume.costume[16].hairInfo = CostumeHair.hairsF[9];
			HeroCostume.costume[16].eye_texture_id = 6;
			HeroCostume.costume[16].beard_texture_id = 33;
			HeroCostume.costume[16].glass_texture_id = 48;
			HeroCostume.costume[16].skin_color = 1;
			HeroCostume.costume[16].hair_color = new Color(1f, 1f, 1f);
			HeroCostume.costume[16].division = DIVISION.TheGarrison;
			HeroCostume.costume[16].costumeId = 9;
			HeroCostume.costume[17] = new HeroCostume();
			HeroCostume.costume[17].name = "rico";
			HeroCostume.costume[17].sex = SEX.FEMALE;
			HeroCostume.costume[17].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[17].body_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[17].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[17].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[17].cape = false;
			HeroCostume.costume[17].hairInfo = CostumeHair.hairsF[9];
			HeroCostume.costume[17].eye_texture_id = 6;
			HeroCostume.costume[17].beard_texture_id = 33;
			HeroCostume.costume[17].glass_texture_id = 48;
			HeroCostume.costume[17].skin_color = 1;
			HeroCostume.costume[17].hair_color = new Color(1f, 1f, 1f);
			HeroCostume.costume[17].costumeId = 10;
			HeroCostume.costume[18] = new HeroCostume();
			HeroCostume.costume[18].name = "jean";
			HeroCostume.costume[18].sex = SEX.MALE;
			HeroCostume.costume[18].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[18].body_texture = HeroCostume.body_uniform_ma_texture[1];
			HeroCostume.costume[18].cape = true;
			HeroCostume.costume[18].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[18].eye_texture_id = 7;
			HeroCostume.costume[18].beard_texture_id = -1;
			HeroCostume.costume[18].glass_texture_id = -1;
			HeroCostume.costume[18].skin_color = 1;
			HeroCostume.costume[18].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[18].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[18].costumeId = 15;
			HeroCostume.costume[19] = new HeroCostume();
			HeroCostume.costume[19].name = "jean";
			HeroCostume.costume[19].sex = SEX.MALE;
			HeroCostume.costume[19].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[19].body_texture = HeroCostume.body_uniform_ma_texture[1];
			HeroCostume.costume[19].cape = false;
			HeroCostume.costume[19].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[19].eye_texture_id = 7;
			HeroCostume.costume[19].beard_texture_id = -1;
			HeroCostume.costume[19].glass_texture_id = -1;
			HeroCostume.costume[19].skin_color = 1;
			HeroCostume.costume[19].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[19].division = DIVISION.TraineesSquad;
			HeroCostume.costume[19].costumeId = 15;
			HeroCostume.costume[20] = new HeroCostume();
			HeroCostume.costume[20].name = "jean";
			HeroCostume.costume[20].sex = SEX.MALE;
			HeroCostume.costume[20].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[20].body_texture = HeroCostume.body_casual_ma_texture[1];
			HeroCostume.costume[20].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[20].part_chest_1_object_texture = HeroCostume.body_casual_ma_texture[1];
			HeroCostume.costume[20].cape = false;
			HeroCostume.costume[20].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[20].eye_texture_id = 7;
			HeroCostume.costume[20].beard_texture_id = -1;
			HeroCostume.costume[20].glass_texture_id = -1;
			HeroCostume.costume[20].skin_color = 1;
			HeroCostume.costume[20].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[20].costumeId = 16;
			HeroCostume.costume[21] = new HeroCostume();
			HeroCostume.costume[21].name = "marco";
			HeroCostume.costume[21].sex = SEX.MALE;
			HeroCostume.costume[21].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[21].body_texture = HeroCostume.body_uniform_ma_texture[2];
			HeroCostume.costume[21].cape = false;
			HeroCostume.costume[21].hairInfo = CostumeHair.hairsM[8];
			HeroCostume.costume[21].eye_texture_id = 8;
			HeroCostume.costume[21].beard_texture_id = -1;
			HeroCostume.costume[21].glass_texture_id = -1;
			HeroCostume.costume[21].skin_color = 1;
			HeroCostume.costume[21].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[21].division = DIVISION.TraineesSquad;
			HeroCostume.costume[21].costumeId = 17;
			HeroCostume.costume[22] = new HeroCostume();
			HeroCostume.costume[22].name = "marco";
			HeroCostume.costume[22].sex = SEX.MALE;
			HeroCostume.costume[22].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[22].body_texture = HeroCostume.body_casual_ma_texture[2];
			HeroCostume.costume[22].cape = false;
			HeroCostume.costume[22].hairInfo = CostumeHair.hairsM[8];
			HeroCostume.costume[22].eye_texture_id = 8;
			HeroCostume.costume[22].beard_texture_id = -1;
			HeroCostume.costume[22].glass_texture_id = -1;
			HeroCostume.costume[22].skin_color = 1;
			HeroCostume.costume[22].hair_color = new Color(0.295f, 0.295f, 0.275f);
			HeroCostume.costume[22].costumeId = 18;
			HeroCostume.costume[23] = new HeroCostume();
			HeroCostume.costume[23].name = "mike";
			HeroCostume.costume[23].sex = SEX.MALE;
			HeroCostume.costume[23].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[23].body_texture = HeroCostume.body_uniform_mb_texture[3];
			HeroCostume.costume[23].cape = true;
			HeroCostume.costume[23].hairInfo = CostumeHair.hairsM[9];
			HeroCostume.costume[23].eye_texture_id = 9;
			HeroCostume.costume[23].beard_texture_id = 32;
			HeroCostume.costume[23].glass_texture_id = -1;
			HeroCostume.costume[23].skin_color = 1;
			HeroCostume.costume[23].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[23].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[23].costumeId = 19;
			HeroCostume.costume[24] = new HeroCostume();
			HeroCostume.costume[24].name = "mike";
			HeroCostume.costume[24].sex = SEX.MALE;
			HeroCostume.costume[24].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[24].body_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[24].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[24].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[24].cape = false;
			HeroCostume.costume[24].hairInfo = CostumeHair.hairsM[9];
			HeroCostume.costume[24].eye_texture_id = 9;
			HeroCostume.costume[24].beard_texture_id = 32;
			HeroCostume.costume[24].glass_texture_id = -1;
			HeroCostume.costume[24].skin_color = 1;
			HeroCostume.costume[24].hair_color = new Color(0.94f, 0.84f, 0.6f);
			HeroCostume.costume[24].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[24].costumeId = 20;
			HeroCostume.costume[25] = new HeroCostume();
			HeroCostume.costume[25].name = "connie";
			HeroCostume.costume[25].sex = SEX.MALE;
			HeroCostume.costume[25].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[25].body_texture = HeroCostume.body_uniform_mb_texture[2];
			HeroCostume.costume[25].cape = true;
			HeroCostume.costume[25].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[25].eye_texture_id = 10;
			HeroCostume.costume[25].beard_texture_id = -1;
			HeroCostume.costume[25].glass_texture_id = -1;
			HeroCostume.costume[25].skin_color = 1;
			HeroCostume.costume[25].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[25].costumeId = 21;
			HeroCostume.costume[26] = new HeroCostume();
			HeroCostume.costume[26].name = "connie";
			HeroCostume.costume[26].sex = SEX.MALE;
			HeroCostume.costume[26].uniform_type = UNIFORM_TYPE.UniformB;
			HeroCostume.costume[26].body_texture = HeroCostume.body_uniform_mb_texture[2];
			HeroCostume.costume[26].cape = false;
			HeroCostume.costume[26].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[26].eye_texture_id = 10;
			HeroCostume.costume[26].beard_texture_id = -1;
			HeroCostume.costume[26].glass_texture_id = -1;
			HeroCostume.costume[26].skin_color = 1;
			HeroCostume.costume[26].division = DIVISION.TraineesSquad;
			HeroCostume.costume[26].costumeId = 21;
			HeroCostume.costume[27] = new HeroCostume();
			HeroCostume.costume[27].name = "connie";
			HeroCostume.costume[27].sex = SEX.MALE;
			HeroCostume.costume[27].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[27].body_texture = HeroCostume.body_casual_mb_texture[2];
			HeroCostume.costume[27].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[27].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[2];
			HeroCostume.costume[27].cape = false;
			HeroCostume.costume[27].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[27].eye_texture_id = 10;
			HeroCostume.costume[27].beard_texture_id = -1;
			HeroCostume.costume[27].glass_texture_id = -1;
			HeroCostume.costume[27].skin_color = 1;
			HeroCostume.costume[27].costumeId = 22;
			HeroCostume.costume[28] = new HeroCostume();
			HeroCostume.costume[28].name = "armin";
			HeroCostume.costume[28].sex = SEX.MALE;
			HeroCostume.costume[28].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[28].body_texture = HeroCostume.body_uniform_ma_texture[0];
			HeroCostume.costume[28].cape = true;
			HeroCostume.costume[28].hairInfo = CostumeHair.hairsM[5];
			HeroCostume.costume[28].eye_texture_id = 11;
			HeroCostume.costume[28].beard_texture_id = -1;
			HeroCostume.costume[28].glass_texture_id = -1;
			HeroCostume.costume[28].skin_color = 1;
			HeroCostume.costume[28].hair_color = new Color(0.95f, 0.8f, 0.5f);
			HeroCostume.costume[28].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[28].costumeId = 23;
			HeroCostume.costume[29] = new HeroCostume();
			HeroCostume.costume[29].name = "armin";
			HeroCostume.costume[29].sex = SEX.MALE;
			HeroCostume.costume[29].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[29].body_texture = HeroCostume.body_uniform_ma_texture[0];
			HeroCostume.costume[29].cape = false;
			HeroCostume.costume[29].hairInfo = CostumeHair.hairsM[5];
			HeroCostume.costume[29].eye_texture_id = 11;
			HeroCostume.costume[29].beard_texture_id = -1;
			HeroCostume.costume[29].glass_texture_id = -1;
			HeroCostume.costume[29].skin_color = 1;
			HeroCostume.costume[29].hair_color = new Color(0.95f, 0.8f, 0.5f);
			HeroCostume.costume[29].division = DIVISION.TraineesSquad;
			HeroCostume.costume[29].costumeId = 23;
			HeroCostume.costume[30] = new HeroCostume();
			HeroCostume.costume[30].name = "armin";
			HeroCostume.costume[30].sex = SEX.MALE;
			HeroCostume.costume[30].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[30].body_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[30].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[30].part_chest_1_object_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[30].cape = false;
			HeroCostume.costume[30].hairInfo = CostumeHair.hairsM[5];
			HeroCostume.costume[30].eye_texture_id = 11;
			HeroCostume.costume[30].beard_texture_id = -1;
			HeroCostume.costume[30].glass_texture_id = -1;
			HeroCostume.costume[30].skin_color = 1;
			HeroCostume.costume[30].hair_color = new Color(0.95f, 0.8f, 0.5f);
			HeroCostume.costume[30].costumeId = 24;
			HeroCostume.costume[31] = new HeroCostume();
			HeroCostume.costume[31].name = "petra";
			HeroCostume.costume[31].sex = SEX.FEMALE;
			HeroCostume.costume[31].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[31].body_texture = HeroCostume.body_uniform_fa_texture[0];
			HeroCostume.costume[31].cape = true;
			HeroCostume.costume[31].hairInfo = CostumeHair.hairsF[8];
			HeroCostume.costume[31].eye_texture_id = 27;
			HeroCostume.costume[31].beard_texture_id = -1;
			HeroCostume.costume[31].glass_texture_id = -1;
			HeroCostume.costume[31].skin_color = 1;
			HeroCostume.costume[31].hair_color = new Color(1f, 0.725f, 0.376f);
			HeroCostume.costume[31].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[31].costumeId = 9;
			HeroCostume.costume[32] = new HeroCostume();
			HeroCostume.costume[32].name = "petra";
			HeroCostume.costume[32].sex = SEX.FEMALE;
			HeroCostume.costume[32].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[32].body_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[32].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[32].part_chest_1_object_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[32].cape = false;
			HeroCostume.costume[32].hairInfo = CostumeHair.hairsF[8];
			HeroCostume.costume[32].eye_texture_id = 27;
			HeroCostume.costume[32].beard_texture_id = -1;
			HeroCostume.costume[32].glass_texture_id = -1;
			HeroCostume.costume[32].skin_color = 1;
			HeroCostume.costume[32].hair_color = new Color(1f, 0.725f, 0.376f);
			HeroCostume.costume[32].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[32].costumeId = 10;
			HeroCostume.costume[33] = new HeroCostume();
			HeroCostume.costume[33].name = "custom";
			HeroCostume.costume[33].sex = SEX.FEMALE;
			HeroCostume.costume[33].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[33].part_chest_skinned_cloth_mesh = "mikasa_asset_cas";
			HeroCostume.costume[33].part_chest_skinned_cloth_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[33].part_chest_1_object_mesh = "character_body_blade_keeper_f";
			HeroCostume.costume[33].part_chest_1_object_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[33].body_texture = HeroCostume.body_casual_fb_texture[1];
			HeroCostume.costume[33].cape = false;
			HeroCostume.costume[33].hairInfo = CostumeHair.hairsF[2];
			HeroCostume.costume[33].eye_texture_id = 12;
			HeroCostume.costume[33].beard_texture_id = 33;
			HeroCostume.costume[33].glass_texture_id = -1;
			HeroCostume.costume[33].skin_color = 1;
			HeroCostume.costume[33].hair_color = new Color(0.15f, 0.15f, 0.145f);
			HeroCostume.costume[33].costumeId = 4;
			HeroCostume.costume[34] = new HeroCostume();
			HeroCostume.costume[34].name = "custom";
			HeroCostume.costume[34].sex = SEX.MALE;
			HeroCostume.costume[34].uniform_type = UNIFORM_TYPE.CasualA;
			HeroCostume.costume[34].body_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[34].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[34].part_chest_1_object_texture = HeroCostume.body_casual_ma_texture[0];
			HeroCostume.costume[34].cape = false;
			HeroCostume.costume[34].hairInfo = CostumeHair.hairsM[3];
			HeroCostume.costume[34].eye_texture_id = 26;
			HeroCostume.costume[34].beard_texture_id = 44;
			HeroCostume.costume[34].glass_texture_id = -1;
			HeroCostume.costume[34].skin_color = 1;
			HeroCostume.costume[34].hair_color = new Color(0.41f, 1f, 0f);
			HeroCostume.costume[34].costumeId = 24;
			HeroCostume.costume[35] = new HeroCostume();
			HeroCostume.costume[35].name = "custom";
			HeroCostume.costume[35].sex = SEX.FEMALE;
			HeroCostume.costume[35].uniform_type = UNIFORM_TYPE.UniformA;
			HeroCostume.costume[35].body_texture = HeroCostume.body_uniform_fa_texture[1];
			HeroCostume.costume[35].cape = false;
			HeroCostume.costume[35].hairInfo = CostumeHair.hairsF[4];
			HeroCostume.costume[35].eye_texture_id = 22;
			HeroCostume.costume[35].beard_texture_id = 33;
			HeroCostume.costume[35].glass_texture_id = 56;
			HeroCostume.costume[35].skin_color = 1;
			HeroCostume.costume[35].hair_color = new Color(0f, 1f, 0.874f);
			HeroCostume.costume[35].costumeId = 5;
			HeroCostume.costume[36] = new HeroCostume();
			HeroCostume.costume[36].name = "feng";
			HeroCostume.costume[36].sex = SEX.MALE;
			HeroCostume.costume[36].uniform_type = UNIFORM_TYPE.CasualB;
			HeroCostume.costume[36].body_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[36].part_chest_1_object_mesh = "character_body_blade_keeper_m";
			HeroCostume.costume[36].part_chest_1_object_texture = HeroCostume.body_casual_mb_texture[3];
			HeroCostume.costume[36].cape = true;
			HeroCostume.costume[36].hairInfo = CostumeHair.hairsM[10];
			HeroCostume.costume[36].eye_texture_id = 25;
			HeroCostume.costume[36].beard_texture_id = 39;
			HeroCostume.costume[36].glass_texture_id = 53;
			HeroCostume.costume[36].skin_color = 1;
			HeroCostume.costume[36].division = DIVISION.TheSurveryCorps;
			HeroCostume.costume[36].costumeId = 20;
			HeroCostume.costume[37] = new HeroCostume();
			HeroCostume.costume[37].name = "AHSS";
			HeroCostume.costume[37].sex = SEX.MALE;
			HeroCostume.costume[37].uniform_type = UNIFORM_TYPE.CasualAHSS;
			HeroCostume.costume[37].body_texture = HeroCostume.body_casual_ma_texture[0] + "_ahss";
			HeroCostume.costume[37].cape = false;
			HeroCostume.costume[37].hairInfo = CostumeHair.hairsM[6];
			HeroCostume.costume[37].eye_texture_id = 25;
			HeroCostume.costume[37].beard_texture_id = 39;
			HeroCostume.costume[37].glass_texture_id = 53;
			HeroCostume.costume[37].skin_color = 3;
			HeroCostume.costume[37].division = DIVISION.TheMilitaryPolice;
			HeroCostume.costume[37].costumeId = 25;
			HeroCostume.costume[38] = new HeroCostume();
			HeroCostume.costume[38].name = "AHSS (F)";
			HeroCostume.costume[38].sex = SEX.FEMALE;
			HeroCostume.costume[38].uniform_type = UNIFORM_TYPE.CasualAHSS;
			HeroCostume.costume[38].body_texture = HeroCostume.body_casual_fa_texture[0];
			HeroCostume.costume[38].cape = false;
			HeroCostume.costume[38].hairInfo = CostumeHair.hairsF[6];
			HeroCostume.costume[38].eye_texture_id = 2;
			HeroCostume.costume[38].beard_texture_id = 33;
			HeroCostume.costume[38].glass_texture_id = -1;
			HeroCostume.costume[38].skin_color = 3;
			HeroCostume.costume[38].division = DIVISION.TheMilitaryPolice;
			HeroCostume.costume[38].costumeId = 26;
			for (int i = 0; i < HeroCostume.costume.Length; i++)
			{
				HeroCostume.costume[i].stat = HeroStat.getInfo("CUSTOM_DEFAULT");
				HeroCostume.costume[i].id = i;
				HeroCostume.costume[i].setMesh2();
				HeroCostume.costume[i].setTexture();
			}
			HeroCostume.costumeOption = new HeroCostume[27]
			{
				HeroCostume.costume[0],
				HeroCostume.costume[2],
				HeroCostume.costume[3],
				HeroCostume.costume[4],
				HeroCostume.costume[5],
				HeroCostume.costume[11],
				HeroCostume.costume[13],
				HeroCostume.costume[14],
				HeroCostume.costume[15],
				HeroCostume.costume[16],
				HeroCostume.costume[17],
				HeroCostume.costume[6],
				HeroCostume.costume[7],
				HeroCostume.costume[8],
				HeroCostume.costume[10],
				HeroCostume.costume[18],
				HeroCostume.costume[19],
				HeroCostume.costume[21],
				HeroCostume.costume[22],
				HeroCostume.costume[23],
				HeroCostume.costume[24],
				HeroCostume.costume[25],
				HeroCostume.costume[27],
				HeroCostume.costume[28],
				HeroCostume.costume[30],
				HeroCostume.costume[37],
				HeroCostume.costume[38]
			};
		}
	}

	public void setBodyByCostumeId(int id = -1)
	{
		if (id == -1)
		{
			id = this.costumeId;
		}
		this.costumeId = id;
		this.arm_l_mesh = HeroCostume.costumeOption[id].arm_l_mesh;
		this.arm_r_mesh = HeroCostume.costumeOption[id].arm_r_mesh;
		this.body_mesh = HeroCostume.costumeOption[id].body_mesh;
		this.body_texture = HeroCostume.costumeOption[id].body_texture;
		this.uniform_type = HeroCostume.costumeOption[id].uniform_type;
		this.part_chest_1_object_mesh = HeroCostume.costumeOption[id].part_chest_1_object_mesh;
		this.part_chest_1_object_texture = HeroCostume.costumeOption[id].part_chest_1_object_texture;
		this.part_chest_object_mesh = HeroCostume.costumeOption[id].part_chest_object_mesh;
		this.part_chest_object_texture = HeroCostume.costumeOption[id].part_chest_object_texture;
		this.part_chest_skinned_cloth_mesh = HeroCostume.costumeOption[id].part_chest_skinned_cloth_mesh;
		this.part_chest_skinned_cloth_texture = HeroCostume.costumeOption[id].part_chest_skinned_cloth_texture;
	}

	public void setCape()
	{
		if (this.cape)
		{
			this.cape_mesh = "character_cape";
		}
		else
		{
			this.cape_mesh = string.Empty;
		}
	}

	public void setMesh()
	{
		this.brand1_mesh = string.Empty;
		this.brand2_mesh = string.Empty;
		this.brand3_mesh = string.Empty;
		this.brand4_mesh = string.Empty;
		this.hand_l_mesh = "character_hand_l";
		this.hand_r_mesh = "character_hand_r";
		this.mesh_3dmg = "character_3dmg";
		this.mesh_3dmg_belt = "character_3dmg_belt";
		this.mesh_3dmg_gas_l = "character_3dmg_gas_l";
		this.mesh_3dmg_gas_r = "character_3dmg_gas_r";
		this.weapon_l_mesh = "character_blade_l";
		this.weapon_r_mesh = "character_blade_r";
		if (this.uniform_type == UNIFORM_TYPE.CasualAHSS)
		{
			this.hand_l_mesh = "character_hand_l_ah";
			this.hand_r_mesh = "character_hand_r_ah";
			this.arm_l_mesh = "character_arm_casual_l_ah";
			this.arm_r_mesh = "character_arm_casual_r_ah";
			this.body_mesh = "character_body_casual_MA";
			this.mesh_3dmg = "character_3dmg_2";
			this.mesh_3dmg_belt = string.Empty;
			this.mesh_3dmg_gas_l = "character_gun_mag_l";
			this.mesh_3dmg_gas_r = "character_gun_mag_r";
			this.weapon_l_mesh = "character_gun_l";
			this.weapon_r_mesh = "character_gun_r";
		}
		else if (this.uniform_type == UNIFORM_TYPE.UniformA)
		{
			this.arm_l_mesh = "character_arm_uniform_l";
			this.arm_r_mesh = "character_arm_uniform_r";
			this.brand1_mesh = "character_brand_arm_l";
			this.brand2_mesh = "character_brand_arm_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_uniform_FA";
				this.brand3_mesh = "character_brand_chest_f";
				this.brand4_mesh = "character_brand_back_f";
			}
			else
			{
				this.body_mesh = "character_body_uniform_MA";
				this.brand3_mesh = "character_brand_chest_m";
				this.brand4_mesh = "character_brand_back_m";
			}
		}
		else if (this.uniform_type == UNIFORM_TYPE.UniformB)
		{
			this.arm_l_mesh = "character_arm_uniform_l";
			this.arm_r_mesh = "character_arm_uniform_r";
			this.brand1_mesh = "character_brand_arm_l";
			this.brand2_mesh = "character_brand_arm_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_uniform_FB";
				this.brand3_mesh = "character_brand_chest_f";
				this.brand4_mesh = "character_brand_back_f";
			}
			else
			{
				this.body_mesh = "character_body_uniform_MB";
				this.brand3_mesh = "character_brand_chest_m";
				this.brand4_mesh = "character_brand_back_m";
			}
		}
		else if (this.uniform_type == UNIFORM_TYPE.CasualA)
		{
			this.arm_l_mesh = "character_arm_casual_l";
			this.arm_r_mesh = "character_arm_casual_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_casual_FA";
			}
			else
			{
				this.body_mesh = "character_body_casual_MA";
			}
		}
		else if (this.uniform_type == UNIFORM_TYPE.CasualB)
		{
			this.arm_l_mesh = "character_arm_casual_l";
			this.arm_r_mesh = "character_arm_casual_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_casual_FB";
			}
			else
			{
				this.body_mesh = "character_body_casual_MB";
			}
		}
		if (this.hairInfo.hair.Length > 0)
		{
			this.hair_mesh = this.hairInfo.hair;
		}
		if (this.hairInfo.hasCloth)
		{
			this.hair_1_mesh = this.hairInfo.hair_1;
		}
		if (this.eye_texture_id >= 0)
		{
			this.eye_mesh = "character_eye";
		}
		if (this.beard_texture_id >= 0)
		{
			this.beard_mesh = "character_face";
		}
		else
		{
			this.beard_mesh = string.Empty;
		}
		if (this.glass_texture_id >= 0)
		{
			this.glass_mesh = "glass";
		}
		else
		{
			this.glass_mesh = string.Empty;
		}
		this.setCape();
	}

	public void setMesh2()
	{
		this.brand1_mesh = string.Empty;
		this.brand2_mesh = string.Empty;
		this.brand3_mesh = string.Empty;
		this.brand4_mesh = string.Empty;
		this.hand_l_mesh = "character_hand_l";
		this.hand_r_mesh = "character_hand_r";
		this.mesh_3dmg = "character_3dmg";
		this.mesh_3dmg_belt = "character_3dmg_belt";
		this.mesh_3dmg_gas_l = "character_3dmg_gas_l";
		this.mesh_3dmg_gas_r = "character_3dmg_gas_r";
		this.weapon_l_mesh = "character_blade_l";
		this.weapon_r_mesh = "character_blade_r";
		if (this.uniform_type == UNIFORM_TYPE.CasualAHSS)
		{
			this.hand_l_mesh = "character_hand_l_ah";
			this.hand_r_mesh = "character_hand_r_ah";
			this.arm_l_mesh = "character_arm_casual_l_ah";
			this.arm_r_mesh = "character_arm_casual_r_ah";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_casual_FA";
			}
			else
			{
				this.body_mesh = "character_body_casual_MA";
			}
			this.mesh_3dmg = "character_3dmg_2";
			this.mesh_3dmg_belt = string.Empty;
			this.mesh_3dmg_gas_l = "character_gun_mag_l";
			this.mesh_3dmg_gas_r = "character_gun_mag_r";
			this.weapon_l_mesh = "character_gun_l";
			this.weapon_r_mesh = "character_gun_r";
		}
		else if (this.uniform_type == UNIFORM_TYPE.UniformA)
		{
			this.arm_l_mesh = "character_arm_uniform_l";
			this.arm_r_mesh = "character_arm_uniform_r";
			this.brand1_mesh = "character_brand_arm_l";
			this.brand2_mesh = "character_brand_arm_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_uniform_FA";
				this.brand3_mesh = "character_brand_chest_f";
				this.brand4_mesh = "character_brand_back_f";
			}
			else
			{
				this.body_mesh = "character_body_uniform_MA";
				this.brand3_mesh = "character_brand_chest_m";
				this.brand4_mesh = "character_brand_back_m";
			}
		}
		else if (this.uniform_type == UNIFORM_TYPE.UniformB)
		{
			this.arm_l_mesh = "character_arm_uniform_l";
			this.arm_r_mesh = "character_arm_uniform_r";
			this.brand1_mesh = "character_brand_arm_l";
			this.brand2_mesh = "character_brand_arm_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_uniform_FB";
				this.brand3_mesh = "character_brand_chest_f";
				this.brand4_mesh = "character_brand_back_f";
			}
			else
			{
				this.body_mesh = "character_body_uniform_MB";
				this.brand3_mesh = "character_brand_chest_m";
				this.brand4_mesh = "character_brand_back_m";
			}
		}
		else if (this.uniform_type == UNIFORM_TYPE.CasualA)
		{
			this.arm_l_mesh = "character_arm_casual_l";
			this.arm_r_mesh = "character_arm_casual_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_casual_FA";
			}
			else
			{
				this.body_mesh = "character_body_casual_MA";
			}
		}
		else if (this.uniform_type == UNIFORM_TYPE.CasualB)
		{
			this.arm_l_mesh = "character_arm_casual_l";
			this.arm_r_mesh = "character_arm_casual_r";
			if (this.sex == SEX.FEMALE)
			{
				this.body_mesh = "character_body_casual_FB";
			}
			else
			{
				this.body_mesh = "character_body_casual_MB";
			}
		}
		if (this.hairInfo.hair.Length > 0)
		{
			this.hair_mesh = this.hairInfo.hair;
		}
		if (this.hairInfo.hasCloth)
		{
			this.hair_1_mesh = this.hairInfo.hair_1;
		}
		if (this.eye_texture_id >= 0)
		{
			this.eye_mesh = "character_eye";
		}
		if (this.beard_texture_id >= 0)
		{
			this.beard_mesh = "character_face";
		}
		else
		{
			this.beard_mesh = string.Empty;
		}
		if (this.glass_texture_id >= 0)
		{
			this.glass_mesh = "glass";
		}
		else
		{
			this.glass_mesh = string.Empty;
		}
		this.setCape();
	}

	public void setTexture()
	{
		if (this.uniform_type == UNIFORM_TYPE.CasualAHSS)
		{
			this._3dmg_texture = "aottg_hero_AHSS_3dmg";
		}
		else
		{
			this._3dmg_texture = "AOTTG_HERO_3DMG";
		}
		this.face_texture = "aottg_hero_eyes";
		if (this.division == DIVISION.TheMilitaryPolice)
		{
			this.brand_texture = "aottg_hero_brand_mp";
		}
		if (this.division == DIVISION.TheGarrison)
		{
			this.brand_texture = "aottg_hero_brand_g";
		}
		if (this.division == DIVISION.TheSurveryCorps)
		{
			this.brand_texture = "aottg_hero_brand_sc";
		}
		if (this.division == DIVISION.TraineesSquad)
		{
			this.brand_texture = "aottg_hero_brand_ts";
		}
		if (this.skin_color == 1)
		{
			this.skin_texture = "aottg_hero_skin_1";
		}
		else if (this.skin_color == 2)
		{
			this.skin_texture = "aottg_hero_skin_2";
		}
		else if (this.skin_color == 3)
		{
			this.skin_texture = "aottg_hero_skin_3";
		}
	}
}
