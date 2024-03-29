using ExitGames.Client.Photon;
using UnityEngine;

public class CostumeConeveter
{
	private static int DivisionToInt(DIVISION id)
	{
		return id switch
		{
			DIVISION.TheGarrison => 0, 
			DIVISION.TheMilitaryPolice => 1, 
			DIVISION.TraineesSquad => 3, 
			_ => 2, 
		};
	}

	public static void HeroCostumeToLocalData(HeroCostume costume, string slot)
	{
		slot = slot.ToUpper();
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.sex, CostumeConeveter.SexToInt(costume.sex));
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.costumeId, costume.costumeId);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.heroCostumeId, costume.id);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.cape, costume.cape ? 1 : 0);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.skin_color, costume.skin_color);
		PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color1, costume.hair_color.r);
		PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color2, costume.hair_color.g);
		PlayerPrefs.SetFloat(slot + PhotonPlayerProperty.hair_color3, costume.hair_color.b);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.division, CostumeConeveter.DivisionToInt(costume.division));
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statSPD, costume.stat.SPD);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statGAS, costume.stat.GAS);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statBLA, costume.stat.BLA);
		PlayerPrefs.SetInt(slot + PhotonPlayerProperty.statACL, costume.stat.ACL);
		PlayerPrefs.SetString(slot + PhotonPlayerProperty.statSKILL, costume.stat.skillId);
	}

	public static void HeroCostumeToPhotonData2(HeroCostume costume, PhotonPlayer player)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add(PhotonPlayerProperty.sex, CostumeConeveter.SexToInt(costume.sex));
		int num = costume.costumeId;
		if (num == 26)
		{
			num = 25;
		}
		hashtable.Add(PhotonPlayerProperty.costumeId, num);
		hashtable.Add(PhotonPlayerProperty.heroCostumeId, costume.id);
		hashtable.Add(PhotonPlayerProperty.cape, costume.cape);
		hashtable.Add(PhotonPlayerProperty.hairInfo, costume.hairInfo.id);
		hashtable.Add(PhotonPlayerProperty.eye_texture_id, costume.eye_texture_id);
		hashtable.Add(PhotonPlayerProperty.beard_texture_id, costume.beard_texture_id);
		hashtable.Add(PhotonPlayerProperty.glass_texture_id, costume.glass_texture_id);
		hashtable.Add(PhotonPlayerProperty.skin_color, costume.skin_color);
		hashtable.Add(PhotonPlayerProperty.hair_color1, costume.hair_color.r);
		hashtable.Add(PhotonPlayerProperty.hair_color2, costume.hair_color.g);
		hashtable.Add(PhotonPlayerProperty.hair_color3, costume.hair_color.b);
		hashtable.Add(PhotonPlayerProperty.division, CostumeConeveter.DivisionToInt(costume.division));
		hashtable.Add(PhotonPlayerProperty.statSPD, costume.stat.SPD);
		hashtable.Add(PhotonPlayerProperty.statGAS, costume.stat.GAS);
		hashtable.Add(PhotonPlayerProperty.statBLA, costume.stat.BLA);
		hashtable.Add(PhotonPlayerProperty.statACL, costume.stat.ACL);
		hashtable.Add(PhotonPlayerProperty.statSKILL, costume.stat.skillId);
		player.SetCustomProperties(hashtable);
	}

	private static DIVISION IntToDivision(int id)
	{
		return id switch
		{
			0 => DIVISION.TheGarrison, 
			1 => DIVISION.TheMilitaryPolice, 
			3 => DIVISION.TraineesSquad, 
			_ => DIVISION.TheSurveryCorps, 
		};
	}

	private static SEX IntToSex(int id)
	{
		if (id == 0)
		{
			return SEX.FEMALE;
		}
		_ = 1;
		return SEX.MALE;
	}

	private static UNIFORM_TYPE IntToUniformType(int id)
	{
		return id switch
		{
			0 => UNIFORM_TYPE.CasualA, 
			1 => UNIFORM_TYPE.CasualB, 
			3 => UNIFORM_TYPE.UniformB, 
			4 => UNIFORM_TYPE.CasualAHSS, 
			_ => UNIFORM_TYPE.UniformA, 
		};
	}

	public static HeroCostume LocalDataToHeroCostume(string slot)
	{
		slot = slot.ToUpper();
		if (!PlayerPrefs.HasKey(slot + PhotonPlayerProperty.sex))
		{
			return HeroCostume.costume[0];
		}
		new HeroCostume();
		HeroCostume heroCostume = new HeroCostume();
		heroCostume.sex = CostumeConeveter.IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex));
		heroCostume.id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.heroCostumeId);
		heroCostume.costumeId = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.costumeId);
		heroCostume.cape = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.cape) == 1;
		heroCostume.hairInfo = ((CostumeConeveter.IntToSex(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.sex)) == SEX.FEMALE) ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + PhotonPlayerProperty.hairInfo)]);
		heroCostume.eye_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.eye_texture_id);
		heroCostume.beard_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.beard_texture_id);
		heroCostume.glass_texture_id = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.glass_texture_id);
		heroCostume.skin_color = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.skin_color);
		heroCostume.hair_color = new Color(PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color1), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color2), PlayerPrefs.GetFloat(slot + PhotonPlayerProperty.hair_color3));
		heroCostume.division = CostumeConeveter.IntToDivision(PlayerPrefs.GetInt(slot + PhotonPlayerProperty.division));
		heroCostume.stat = new HeroStat();
		heroCostume.stat.SPD = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statSPD);
		heroCostume.stat.GAS = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statGAS);
		heroCostume.stat.BLA = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statBLA);
		heroCostume.stat.ACL = PlayerPrefs.GetInt(slot + PhotonPlayerProperty.statACL);
		heroCostume.stat.skillId = PlayerPrefs.GetString(slot + PhotonPlayerProperty.statSKILL);
		heroCostume.setBodyByCostumeId();
		heroCostume.setMesh2();
		heroCostume.setTexture();
		return heroCostume;
	}

	public static HeroCostume PhotonDataToHeroCostume2(PhotonPlayer player)
	{
		HeroCostume heroCostume = new HeroCostume();
		SEX sEX = CostumeConeveter.IntToSex((int)player.customProperties[PhotonPlayerProperty.sex]);
		heroCostume = new HeroCostume
		{
			sex = sEX,
			costumeId = (int)player.customProperties[PhotonPlayerProperty.costumeId],
			id = (int)player.customProperties[PhotonPlayerProperty.heroCostumeId],
			cape = (bool)player.customProperties[PhotonPlayerProperty.cape],
			hairInfo = ((sEX != 0) ? CostumeHair.hairsF[(int)player.customProperties[PhotonPlayerProperty.hairInfo]] : CostumeHair.hairsM[(int)player.customProperties[PhotonPlayerProperty.hairInfo]]),
			eye_texture_id = (int)player.customProperties[PhotonPlayerProperty.eye_texture_id],
			beard_texture_id = (int)player.customProperties[PhotonPlayerProperty.beard_texture_id],
			glass_texture_id = (int)player.customProperties[PhotonPlayerProperty.glass_texture_id],
			skin_color = (int)player.customProperties[PhotonPlayerProperty.skin_color],
			hair_color = new Color((float)player.customProperties[PhotonPlayerProperty.hair_color1], (float)player.customProperties[PhotonPlayerProperty.hair_color2], (float)player.customProperties[PhotonPlayerProperty.hair_color3]),
			division = CostumeConeveter.IntToDivision((int)player.customProperties[PhotonPlayerProperty.division]),
			stat = new HeroStat()
		};
		heroCostume.stat.SPD = (int)player.customProperties[PhotonPlayerProperty.statSPD];
		heroCostume.stat.GAS = (int)player.customProperties[PhotonPlayerProperty.statGAS];
		heroCostume.stat.BLA = (int)player.customProperties[PhotonPlayerProperty.statBLA];
		heroCostume.stat.ACL = (int)player.customProperties[PhotonPlayerProperty.statACL];
		heroCostume.stat.skillId = (string)player.customProperties[PhotonPlayerProperty.statSKILL];
		if (heroCostume.costumeId == 25 && heroCostume.sex == SEX.FEMALE)
		{
			heroCostume.costumeId = 26;
		}
		heroCostume.setBodyByCostumeId();
		heroCostume.setMesh2();
		heroCostume.setTexture();
		return heroCostume;
	}

	private static int SexToInt(SEX id)
	{
		if (id == SEX.FEMALE)
		{
			return 0;
		}
		return 1;
	}

	private static int UniformTypeToInt(UNIFORM_TYPE id)
	{
		return id switch
		{
			UNIFORM_TYPE.CasualA => 0, 
			UNIFORM_TYPE.CasualB => 1, 
			UNIFORM_TYPE.UniformB => 3, 
			UNIFORM_TYPE.CasualAHSS => 4, 
			_ => 2, 
		};
	}
}
