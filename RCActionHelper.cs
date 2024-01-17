using System;

internal class RCActionHelper
{
	public enum helperClasses
	{
		primitive,
		variable,
		player,
		titan,
		region,
		convert
	}

	public enum mathTypes
	{
		add,
		subtract,
		multiply,
		divide,
		modulo,
		power
	}

	public enum other
	{
		regionX,
		regionY,
		regionZ
	}

	public enum playerTypes
	{
		playerType,
		playerTeam,
		playerAlive,
		playerTitan,
		playerKills,
		playerDeaths,
		playerMaxDamage,
		playerTotalDamage,
		playerCustomInt,
		playerCustomBool,
		playerCustomString,
		playerCustomFloat,
		playerName,
		playerGuildName,
		playerPosX,
		playerPosY,
		playerPosZ,
		playerSpeed
	}

	public enum titanTypes
	{
		titanType,
		titanSize,
		titanHealth,
		positionX,
		positionY,
		positionZ
	}

	public enum variableTypes
	{
		typeInt,
		typeBool,
		typeString,
		typeFloat,
		typePlayer,
		typeTitan
	}

	public int helperClass;

	public int helperType;

	private RCActionHelper nextHelper;

	private object parameters;

	public RCActionHelper(int sentClass, int sentType, object options)
	{
		this.helperClass = sentClass;
		this.helperType = sentType;
		this.parameters = options;
	}

	public void callException(string str)
	{
		FengGameManagerMKII.instance.chatRoom.addLINE(str);
	}

	public bool returnBool(object sentObject)
	{
		object obj = sentObject;
		if (this.parameters != null)
		{
			obj = this.parameters;
		}
		switch (this.helperClass)
		{
		case 0:
			return (bool)obj;
		case 1:
		{
			RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
			return this.helperType switch
			{
				0 => this.nextHelper.returnBool(FengGameManagerMKII.intVariables[rCActionHelper2.returnString(null)]), 
				1 => (bool)FengGameManagerMKII.boolVariables[rCActionHelper2.returnString(null)], 
				2 => this.nextHelper.returnBool(FengGameManagerMKII.stringVariables[rCActionHelper2.returnString(null)]), 
				3 => this.nextHelper.returnBool(FengGameManagerMKII.floatVariables[rCActionHelper2.returnString(null)]), 
				4 => this.nextHelper.returnBool(FengGameManagerMKII.playerVariables[rCActionHelper2.returnString(null)]), 
				5 => this.nextHelper.returnBool(FengGameManagerMKII.titanVariables[rCActionHelper2.returnString(null)]), 
				_ => false, 
			};
		}
		case 2:
		{
			PhotonPlayer photonPlayer = (PhotonPlayer)obj;
			if (photonPlayer != null)
			{
				switch (this.helperType)
				{
				case 0:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.team]);
				case 1:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.RCteam]);
				case 2:
					return !(bool)photonPlayer.customProperties[PhotonPlayerProperty.dead];
				case 3:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]);
				case 4:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
				case 5:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
				case 6:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
				case 7:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
				case 8:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.customInt]);
				case 9:
					return (bool)photonPlayer.customProperties[PhotonPlayerProperty.customBool];
				case 10:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.customString]);
				case 11:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.customFloat]);
				case 12:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.name]);
				case 13:
					return this.nextHelper.returnBool(photonPlayer.customProperties[PhotonPlayerProperty.guildName]);
				case 14:
				{
					int ıD4 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD4))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD4];
						return this.nextHelper.returnBool(hERO.transform.position.x);
					}
					return false;
				}
				case 15:
				{
					int ıD3 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD3))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD3];
						return this.nextHelper.returnBool(hERO.transform.position.y);
					}
					return false;
				}
				case 16:
				{
					int ıD2 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD2))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD2];
						return this.nextHelper.returnBool(hERO.transform.position.z);
					}
					return false;
				}
				case 17:
				{
					int ıD = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD];
						return this.nextHelper.returnBool(hERO.rigidbody.velocity.magnitude);
					}
					return false;
				}
				}
			}
			return false;
		}
		case 3:
		{
			TITAN tITAN = (TITAN)obj;
			if (tITAN != null)
			{
				switch (this.helperType)
				{
				case 0:
					return this.nextHelper.returnBool(tITAN.abnormalType);
				case 1:
					return this.nextHelper.returnBool(tITAN.myLevel);
				case 2:
					return this.nextHelper.returnBool(tITAN.currentHealth);
				case 3:
					return this.nextHelper.returnBool(tITAN.transform.position.x);
				case 4:
					return this.nextHelper.returnBool(tITAN.transform.position.y);
				case 5:
					return this.nextHelper.returnBool(tITAN.transform.position.z);
				}
			}
			return false;
		}
		case 4:
		{
			RCActionHelper rCActionHelper = (RCActionHelper)obj;
			RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
			return this.helperType switch
			{
				0 => this.nextHelper.returnBool(rCRegion.GetRandomX()), 
				1 => this.nextHelper.returnBool(rCRegion.GetRandomY()), 
				2 => this.nextHelper.returnBool(rCRegion.GetRandomZ()), 
				_ => false, 
			};
		}
		case 5:
			return this.helperType switch
			{
				0 => Convert.ToBoolean((int)obj), 
				1 => (bool)obj, 
				2 => Convert.ToBoolean((string)obj), 
				3 => Convert.ToBoolean((float)obj), 
				_ => false, 
			};
		default:
			return false;
		}
	}

	public float returnFloat(object sentObject)
	{
		object obj = sentObject;
		if (this.parameters != null)
		{
			obj = this.parameters;
		}
		switch (this.helperClass)
		{
		case 0:
			return (float)obj;
		case 1:
		{
			RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
			return this.helperType switch
			{
				0 => this.nextHelper.returnFloat(FengGameManagerMKII.intVariables[rCActionHelper2.returnString(null)]), 
				1 => this.nextHelper.returnFloat(FengGameManagerMKII.boolVariables[rCActionHelper2.returnString(null)]), 
				2 => this.nextHelper.returnFloat(FengGameManagerMKII.stringVariables[rCActionHelper2.returnString(null)]), 
				3 => (float)FengGameManagerMKII.floatVariables[rCActionHelper2.returnString(null)], 
				4 => this.nextHelper.returnFloat(FengGameManagerMKII.playerVariables[rCActionHelper2.returnString(null)]), 
				5 => this.nextHelper.returnFloat(FengGameManagerMKII.titanVariables[rCActionHelper2.returnString(null)]), 
				_ => 0f, 
			};
		}
		case 2:
		{
			PhotonPlayer photonPlayer = (PhotonPlayer)obj;
			if (photonPlayer != null)
			{
				switch (this.helperType)
				{
				case 0:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.team]);
				case 1:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.RCteam]);
				case 2:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.dead]);
				case 3:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]);
				case 4:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
				case 5:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
				case 6:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
				case 7:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
				case 8:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.customInt]);
				case 9:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.customBool]);
				case 10:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.customString]);
				case 11:
					return (float)photonPlayer.customProperties[PhotonPlayerProperty.customFloat];
				case 12:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.name]);
				case 13:
					return this.nextHelper.returnFloat(photonPlayer.customProperties[PhotonPlayerProperty.guildName]);
				case 14:
				{
					int ıD4 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD4))
					{
						return ((HERO)FengGameManagerMKII.heroHash[ıD4]).transform.position.x;
					}
					return 0f;
				}
				case 15:
				{
					int ıD3 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD3))
					{
						return ((HERO)FengGameManagerMKII.heroHash[ıD3]).transform.position.y;
					}
					return 0f;
				}
				case 16:
				{
					int ıD2 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD2))
					{
						return ((HERO)FengGameManagerMKII.heroHash[ıD2]).transform.position.z;
					}
					return 0f;
				}
				case 17:
				{
					int ıD = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
					{
						return ((HERO)FengGameManagerMKII.heroHash[ıD]).rigidbody.velocity.magnitude;
					}
					return 0f;
				}
				}
			}
			return 0f;
		}
		case 3:
		{
			TITAN tITAN = (TITAN)obj;
			if (tITAN != null)
			{
				switch (this.helperType)
				{
				case 0:
					return this.nextHelper.returnFloat(tITAN.abnormalType);
				case 1:
					return tITAN.myLevel;
				case 2:
					return this.nextHelper.returnFloat(tITAN.currentHealth);
				case 3:
					return tITAN.transform.position.x;
				case 4:
					return tITAN.transform.position.y;
				case 5:
					return tITAN.transform.position.z;
				}
			}
			return 0f;
		}
		case 4:
		{
			RCActionHelper rCActionHelper = (RCActionHelper)obj;
			RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
			return this.helperType switch
			{
				0 => rCRegion.GetRandomX(), 
				1 => rCRegion.GetRandomY(), 
				2 => rCRegion.GetRandomZ(), 
				_ => 0f, 
			};
		}
		case 5:
			switch (this.helperType)
			{
			case 0:
				return Convert.ToSingle((int)obj);
			case 1:
				return Convert.ToSingle((bool)obj);
			case 2:
			{
				_ = (string)obj;
				if (float.TryParse((string)obj, out var result))
				{
					return result;
				}
				return 0f;
			}
			case 3:
				return (float)obj;
			default:
				return (float)obj;
			}
		default:
			return 0f;
		}
	}

	public int returnInt(object sentObject)
	{
		object obj = sentObject;
		if (this.parameters != null)
		{
			obj = this.parameters;
		}
		switch (this.helperClass)
		{
		case 0:
			return (int)obj;
		case 1:
		{
			RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
			return this.helperType switch
			{
				0 => (int)FengGameManagerMKII.intVariables[rCActionHelper2.returnString(null)], 
				1 => this.nextHelper.returnInt(FengGameManagerMKII.boolVariables[rCActionHelper2.returnString(null)]), 
				2 => this.nextHelper.returnInt(FengGameManagerMKII.stringVariables[rCActionHelper2.returnString(null)]), 
				3 => this.nextHelper.returnInt(FengGameManagerMKII.floatVariables[rCActionHelper2.returnString(null)]), 
				4 => this.nextHelper.returnInt(FengGameManagerMKII.playerVariables[rCActionHelper2.returnString(null)]), 
				5 => this.nextHelper.returnInt(FengGameManagerMKII.titanVariables[rCActionHelper2.returnString(null)]), 
				_ => 0, 
			};
		}
		case 2:
		{
			PhotonPlayer photonPlayer = (PhotonPlayer)obj;
			if (photonPlayer != null)
			{
				switch (this.helperType)
				{
				case 0:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.team];
				case 1:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.RCteam];
				case 2:
					return this.nextHelper.returnInt(photonPlayer.customProperties[PhotonPlayerProperty.dead]);
				case 3:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.isTitan];
				case 4:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.kills];
				case 5:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.deaths];
				case 6:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.max_dmg];
				case 7:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.total_dmg];
				case 8:
					return (int)photonPlayer.customProperties[PhotonPlayerProperty.customInt];
				case 9:
					return this.nextHelper.returnInt(photonPlayer.customProperties[PhotonPlayerProperty.customBool]);
				case 10:
					return this.nextHelper.returnInt(photonPlayer.customProperties[PhotonPlayerProperty.customString]);
				case 11:
					return this.nextHelper.returnInt(photonPlayer.customProperties[PhotonPlayerProperty.customFloat]);
				case 12:
					return this.nextHelper.returnInt(photonPlayer.customProperties[PhotonPlayerProperty.name]);
				case 13:
					return this.nextHelper.returnInt(photonPlayer.customProperties[PhotonPlayerProperty.guildName]);
				case 14:
				{
					int ıD4 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD4))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD4];
						return this.nextHelper.returnInt(hERO.transform.position.x);
					}
					return 0;
				}
				case 15:
				{
					int ıD3 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD3))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD3];
						return this.nextHelper.returnInt(hERO.transform.position.y);
					}
					return 0;
				}
				case 16:
				{
					int ıD2 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD2))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD2];
						return this.nextHelper.returnInt(hERO.transform.position.z);
					}
					return 0;
				}
				case 17:
				{
					int ıD = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD];
						return this.nextHelper.returnInt(hERO.rigidbody.velocity.magnitude);
					}
					return 0;
				}
				}
			}
			return 0;
		}
		case 3:
		{
			TITAN tITAN = (TITAN)obj;
			if (tITAN != null)
			{
				switch (this.helperType)
				{
				case 0:
					return (int)tITAN.abnormalType;
				case 1:
					return this.nextHelper.returnInt(tITAN.myLevel);
				case 2:
					return tITAN.currentHealth;
				case 3:
					return this.nextHelper.returnInt(tITAN.transform.position.x);
				case 4:
					return this.nextHelper.returnInt(tITAN.transform.position.y);
				case 5:
					return this.nextHelper.returnInt(tITAN.transform.position.z);
				}
			}
			return 0;
		}
		case 4:
		{
			RCActionHelper rCActionHelper = (RCActionHelper)obj;
			RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
			return this.helperType switch
			{
				0 => this.nextHelper.returnInt(rCRegion.GetRandomX()), 
				1 => this.nextHelper.returnInt(rCRegion.GetRandomY()), 
				2 => this.nextHelper.returnInt(rCRegion.GetRandomZ()), 
				_ => 0, 
			};
		}
		case 5:
			switch (this.helperType)
			{
			case 0:
				return (int)obj;
			case 1:
				return Convert.ToInt32((bool)obj);
			case 2:
			{
				_ = (string)obj;
				if (int.TryParse((string)obj, out var result))
				{
					return result;
				}
				return 0;
			}
			case 3:
				return Convert.ToInt32((float)obj);
			default:
				return (int)obj;
			}
		default:
			return 0;
		}
	}

	public PhotonPlayer returnPlayer(object objParameter)
	{
		object obj = objParameter;
		if (this.parameters != null)
		{
			obj = this.parameters;
		}
		switch (this.helperClass)
		{
		case 1:
		{
			RCActionHelper rCActionHelper = (RCActionHelper)obj;
			return (PhotonPlayer)FengGameManagerMKII.playerVariables[rCActionHelper.returnString(null)];
		}
		case 2:
			return (PhotonPlayer)obj;
		default:
			return (PhotonPlayer)obj;
		}
	}

	public string returnString(object sentObject)
	{
		object obj = sentObject;
		if (this.parameters != null)
		{
			obj = this.parameters;
		}
		switch (this.helperClass)
		{
		case 0:
			return (string)obj;
		case 1:
		{
			RCActionHelper rCActionHelper2 = (RCActionHelper)obj;
			return this.helperType switch
			{
				0 => this.nextHelper.returnString(FengGameManagerMKII.intVariables[rCActionHelper2.returnString(null)]), 
				1 => this.nextHelper.returnString(FengGameManagerMKII.boolVariables[rCActionHelper2.returnString(null)]), 
				2 => (string)FengGameManagerMKII.stringVariables[rCActionHelper2.returnString(null)], 
				3 => this.nextHelper.returnString(FengGameManagerMKII.floatVariables[rCActionHelper2.returnString(null)]), 
				4 => this.nextHelper.returnString(FengGameManagerMKII.playerVariables[rCActionHelper2.returnString(null)]), 
				5 => this.nextHelper.returnString(FengGameManagerMKII.titanVariables[rCActionHelper2.returnString(null)]), 
				_ => string.Empty, 
			};
		}
		case 2:
		{
			PhotonPlayer photonPlayer = (PhotonPlayer)obj;
			if (photonPlayer != null)
			{
				switch (this.helperType)
				{
				case 0:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.team]);
				case 1:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.RCteam]);
				case 2:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.dead]);
				case 3:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]);
				case 4:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.kills]);
				case 5:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.deaths]);
				case 6:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.max_dmg]);
				case 7:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.total_dmg]);
				case 8:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.customInt]);
				case 9:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.customBool]);
				case 10:
					return (string)photonPlayer.customProperties[PhotonPlayerProperty.customString];
				case 11:
					return this.nextHelper.returnString(photonPlayer.customProperties[PhotonPlayerProperty.customFloat]);
				case 12:
					return (string)photonPlayer.customProperties[PhotonPlayerProperty.name];
				case 13:
					return (string)photonPlayer.customProperties[PhotonPlayerProperty.guildName];
				case 14:
				{
					int ıD4 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD4))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD4];
						return this.nextHelper.returnString(hERO.transform.position.x);
					}
					return string.Empty;
				}
				case 15:
				{
					int ıD3 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD3))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD3];
						return this.nextHelper.returnString(hERO.transform.position.y);
					}
					return string.Empty;
				}
				case 16:
				{
					int ıD2 = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD2))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD2];
						return this.nextHelper.returnString(hERO.transform.position.z);
					}
					return string.Empty;
				}
				case 17:
				{
					int ıD = photonPlayer.ID;
					if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
					{
						HERO hERO = (HERO)FengGameManagerMKII.heroHash[ıD];
						return this.nextHelper.returnString(hERO.rigidbody.velocity.magnitude);
					}
					return string.Empty;
				}
				}
			}
			return string.Empty;
		}
		case 3:
		{
			TITAN tITAN = (TITAN)obj;
			if (tITAN != null)
			{
				switch (this.helperType)
				{
				case 0:
					return this.nextHelper.returnString(tITAN.abnormalType);
				case 1:
					return this.nextHelper.returnString(tITAN.myLevel);
				case 2:
					return this.nextHelper.returnString(tITAN.currentHealth);
				case 3:
					return this.nextHelper.returnString(tITAN.transform.position.x);
				case 4:
					return this.nextHelper.returnString(tITAN.transform.position.y);
				case 5:
					return this.nextHelper.returnString(tITAN.transform.position.z);
				}
			}
			return string.Empty;
		}
		case 4:
		{
			RCActionHelper rCActionHelper = (RCActionHelper)obj;
			RCRegion rCRegion = (RCRegion)FengGameManagerMKII.RCRegions[rCActionHelper.returnString(null)];
			return this.helperType switch
			{
				0 => this.nextHelper.returnString(rCRegion.GetRandomX()), 
				1 => this.nextHelper.returnString(rCRegion.GetRandomY()), 
				2 => this.nextHelper.returnString(rCRegion.GetRandomZ()), 
				_ => string.Empty, 
			};
		}
		case 5:
			return this.helperType switch
			{
				0 => ((int)obj).ToString(), 
				1 => ((bool)obj).ToString(), 
				2 => (string)obj, 
				3 => ((float)obj).ToString(), 
				_ => string.Empty, 
			};
		default:
			return string.Empty;
		}
	}

	public TITAN returnTitan(object objParameter)
	{
		object obj = objParameter;
		if (this.parameters != null)
		{
			obj = this.parameters;
		}
		switch (this.helperClass)
		{
		case 1:
		{
			RCActionHelper rCActionHelper = (RCActionHelper)obj;
			return (TITAN)FengGameManagerMKII.titanVariables[rCActionHelper.returnString(null)];
		}
		case 3:
			return (TITAN)obj;
		default:
			return (TITAN)obj;
		}
	}

	public void setNextHelper(RCActionHelper sentHelper)
	{
		this.nextHelper = sentHelper;
	}
}
