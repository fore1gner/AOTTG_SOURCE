using System;
using ExitGames.Client.Photon;
using UnityEngine;

internal class RCAction
{
	public enum actionClasses
	{
		typeVoid,
		typeVariableInt,
		typeVariableBool,
		typeVariableString,
		typeVariableFloat,
		typeVariablePlayer,
		typeVariableTitan,
		typePlayer,
		typeTitan,
		typeGame
	}

	public enum gameTypes
	{
		printMessage,
		winGame,
		loseGame,
		restartGame
	}

	public enum playerTypes
	{
		killPlayer,
		spawnPlayer,
		spawnPlayerAt,
		movePlayer,
		setKills,
		setDeaths,
		setMaxDmg,
		setTotalDmg,
		setName,
		setGuildName,
		setTeam,
		setCustomInt,
		setCustomBool,
		setCustomString,
		setCustomFloat
	}

	public enum titanTypes
	{
		killTitan,
		spawnTitan,
		spawnTitanAt,
		setHealth,
		moveTitan
	}

	public enum varTypes
	{
		set,
		add,
		subtract,
		multiply,
		divide,
		modulo,
		power,
		concat,
		append,
		remove,
		replace,
		toOpposite,
		setRandom
	}

	private int actionClass;

	private int actionType;

	private RCEvent nextEvent;

	private RCActionHelper[] parameters;

	public RCAction(int category, int type, RCEvent next, RCActionHelper[] helpers)
	{
		this.actionClass = category;
		this.actionType = type;
		this.nextEvent = next;
		this.parameters = helpers;
	}

	public void callException(string str)
	{
		FengGameManagerMKII.instance.chatRoom.addLINE(str);
	}

	public void doAction()
	{
		switch (this.actionClass)
		{
		case 0:
			this.nextEvent.checkEvent();
			break;
		case 1:
		{
			string text4 = this.parameters[0].returnString(null);
			int num2 = this.parameters[1].returnInt(null);
			switch (this.actionType)
			{
			case 0:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					FengGameManagerMKII.intVariables.Add(text4, num2);
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = num2;
				}
				break;
			case 1:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					this.callException("Variable not found: " + text4);
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = (int)FengGameManagerMKII.intVariables[text4] + num2;
				}
				break;
			case 2:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					this.callException("Variable not found: " + text4);
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = (int)FengGameManagerMKII.intVariables[text4] - num2;
				}
				break;
			case 3:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					this.callException("Variable not found: " + text4);
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = (int)FengGameManagerMKII.intVariables[text4] * num2;
				}
				break;
			case 4:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					this.callException("Variable not found: " + text4);
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = (int)FengGameManagerMKII.intVariables[text4] / num2;
				}
				break;
			case 5:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					this.callException("Variable not found: " + text4);
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = (int)FengGameManagerMKII.intVariables[text4] % num2;
				}
				break;
			case 6:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					this.callException("Variable not found: " + text4);
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = (int)Math.Pow((int)FengGameManagerMKII.intVariables[text4], num2);
				}
				break;
			case 12:
				if (!FengGameManagerMKII.intVariables.ContainsKey(text4))
				{
					FengGameManagerMKII.intVariables.Add(text4, UnityEngine.Random.Range(num2, this.parameters[2].returnInt(null)));
				}
				else
				{
					FengGameManagerMKII.intVariables[text4] = UnityEngine.Random.Range(num2, this.parameters[2].returnInt(null));
				}
				break;
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				break;
			}
			break;
		}
		case 2:
		{
			string text3 = this.parameters[0].returnString(null);
			bool flag = this.parameters[1].returnBool(null);
			switch (this.actionType)
			{
			case 11:
				if (!FengGameManagerMKII.boolVariables.ContainsKey(text3))
				{
					this.callException("Variable not found: " + text3);
				}
				else
				{
					FengGameManagerMKII.boolVariables[text3] = !(bool)FengGameManagerMKII.boolVariables[text3];
				}
				break;
			case 12:
				if (!FengGameManagerMKII.boolVariables.ContainsKey(text3))
				{
					FengGameManagerMKII.boolVariables.Add(text3, Convert.ToBoolean(UnityEngine.Random.Range(0, 2)));
				}
				else
				{
					FengGameManagerMKII.boolVariables[text3] = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
				}
				break;
			case 0:
				if (!FengGameManagerMKII.boolVariables.ContainsKey(text3))
				{
					FengGameManagerMKII.boolVariables.Add(text3, flag);
				}
				else
				{
					FengGameManagerMKII.boolVariables[text3] = flag;
				}
				break;
			}
			break;
		}
		case 3:
		{
			string key3 = this.parameters[0].returnString(null);
			switch (this.actionType)
			{
			case 7:
			{
				string text2 = string.Empty;
				for (int i = 1; i < this.parameters.Length; i++)
				{
					text2 += this.parameters[i].returnString(null);
				}
				if (!FengGameManagerMKII.stringVariables.ContainsKey(key3))
				{
					FengGameManagerMKII.stringVariables.Add(key3, text2);
				}
				else
				{
					FengGameManagerMKII.stringVariables[key3] = text2;
				}
				break;
			}
			case 8:
			{
				string text = this.parameters[1].returnString(null);
				if (!FengGameManagerMKII.stringVariables.ContainsKey(key3))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.stringVariables[key3] = (string)FengGameManagerMKII.stringVariables[key3] + text;
				}
				break;
			}
			case 9:
				this.parameters[1].returnString(null);
				if (!FengGameManagerMKII.stringVariables.ContainsKey(key3))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.stringVariables[key3] = ((string)FengGameManagerMKII.stringVariables[key3]).Replace(this.parameters[1].returnString(null), this.parameters[2].returnString(null));
				}
				break;
			case 0:
			{
				string value2 = this.parameters[1].returnString(null);
				if (!FengGameManagerMKII.stringVariables.ContainsKey(key3))
				{
					FengGameManagerMKII.stringVariables.Add(key3, value2);
				}
				else
				{
					FengGameManagerMKII.stringVariables[key3] = value2;
				}
				break;
			}
			}
			break;
		}
		case 4:
		{
			string key2 = this.parameters[0].returnString(null);
			float num = this.parameters[1].returnFloat(null);
			switch (this.actionType)
			{
			case 0:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					FengGameManagerMKII.floatVariables.Add(key2, num);
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = num;
				}
				break;
			case 1:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = (float)FengGameManagerMKII.floatVariables[key2] + num;
				}
				break;
			case 2:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = (float)FengGameManagerMKII.floatVariables[key2] - num;
				}
				break;
			case 3:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = (float)FengGameManagerMKII.floatVariables[key2] * num;
				}
				break;
			case 4:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = (float)FengGameManagerMKII.floatVariables[key2] / num;
				}
				break;
			case 5:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = (float)FengGameManagerMKII.floatVariables[key2] % num;
				}
				break;
			case 6:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					this.callException("No Variable");
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = (float)Math.Pow((int)FengGameManagerMKII.floatVariables[key2], num);
				}
				break;
			case 12:
				if (!FengGameManagerMKII.floatVariables.ContainsKey(key2))
				{
					FengGameManagerMKII.floatVariables.Add(key2, UnityEngine.Random.Range(num, this.parameters[2].returnFloat(null)));
				}
				else
				{
					FengGameManagerMKII.floatVariables[key2] = UnityEngine.Random.Range(num, this.parameters[2].returnFloat(null));
				}
				break;
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				break;
			}
			break;
		}
		case 5:
		{
			string key4 = this.parameters[0].returnString(null);
			PhotonPlayer value3 = this.parameters[1].returnPlayer(null);
			if (this.actionType == 0)
			{
				if (!FengGameManagerMKII.playerVariables.ContainsKey(key4))
				{
					FengGameManagerMKII.playerVariables.Add(key4, value3);
				}
				else
				{
					FengGameManagerMKII.playerVariables[key4] = value3;
				}
			}
			break;
		}
		case 6:
		{
			string key = this.parameters[0].returnString(null);
			TITAN value = this.parameters[1].returnTitan(null);
			if (this.actionType == 0)
			{
				if (!FengGameManagerMKII.titanVariables.ContainsKey(key))
				{
					FengGameManagerMKII.titanVariables.Add(key, value);
				}
				else
				{
					FengGameManagerMKII.titanVariables[key] = value;
				}
			}
			break;
		}
		case 7:
		{
			PhotonPlayer photonPlayer = this.parameters[0].returnPlayer(null);
			switch (this.actionType)
			{
			case 0:
			{
				int ıD2 = photonPlayer.ID;
				if (FengGameManagerMKII.heroHash.ContainsKey(ıD2))
				{
					HERO obj = (HERO)FengGameManagerMKII.heroHash[ıD2];
					obj.markDie();
					obj.photonView.RPC("netDie2", PhotonTargets.All, -1, this.parameters[1].returnString(null) + " ");
				}
				else
				{
					this.callException("Player Not Alive");
				}
				break;
			}
			case 1:
				FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", photonPlayer);
				break;
			case 2:
				FengGameManagerMKII.instance.photonView.RPC("spawnPlayerAtRPC", photonPlayer, this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null));
				break;
			case 3:
			{
				int ıD = photonPlayer.ID;
				if (FengGameManagerMKII.heroHash.ContainsKey(ıD))
				{
					((HERO)FengGameManagerMKII.heroHash[ıD]).photonView.RPC("moveToRPC", photonPlayer, this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null));
				}
				else
				{
					this.callException("Player Not Alive");
				}
				break;
			}
			case 4:
			{
				Hashtable hashtable11 = new Hashtable();
				hashtable11.Add(PhotonPlayerProperty.kills, this.parameters[1].returnInt(null));
				photonPlayer.SetCustomProperties(hashtable11);
				break;
			}
			case 5:
			{
				Hashtable hashtable10 = new Hashtable();
				hashtable10.Add(PhotonPlayerProperty.deaths, this.parameters[1].returnInt(null));
				photonPlayer.SetCustomProperties(hashtable10);
				break;
			}
			case 6:
			{
				Hashtable hashtable9 = new Hashtable();
				hashtable9.Add(PhotonPlayerProperty.max_dmg, this.parameters[1].returnInt(null));
				photonPlayer.SetCustomProperties(hashtable9);
				break;
			}
			case 7:
			{
				Hashtable hashtable8 = new Hashtable();
				hashtable8.Add(PhotonPlayerProperty.total_dmg, this.parameters[1].returnInt(null));
				photonPlayer.SetCustomProperties(hashtable8);
				break;
			}
			case 8:
			{
				Hashtable hashtable7 = new Hashtable();
				hashtable7.Add(PhotonPlayerProperty.name, this.parameters[1].returnString(null));
				photonPlayer.SetCustomProperties(hashtable7);
				break;
			}
			case 9:
			{
				Hashtable hashtable6 = new Hashtable();
				hashtable6.Add(PhotonPlayerProperty.guildName, this.parameters[1].returnString(null));
				photonPlayer.SetCustomProperties(hashtable6);
				break;
			}
			case 10:
			{
				Hashtable hashtable5 = new Hashtable();
				hashtable5.Add(PhotonPlayerProperty.RCteam, this.parameters[1].returnInt(null));
				photonPlayer.SetCustomProperties(hashtable5);
				break;
			}
			case 11:
			{
				Hashtable hashtable4 = new Hashtable();
				hashtable4.Add(PhotonPlayerProperty.customInt, this.parameters[1].returnInt(null));
				photonPlayer.SetCustomProperties(hashtable4);
				break;
			}
			case 12:
			{
				Hashtable hashtable3 = new Hashtable();
				hashtable3.Add(PhotonPlayerProperty.customBool, this.parameters[1].returnBool(null));
				photonPlayer.SetCustomProperties(hashtable3);
				break;
			}
			case 13:
			{
				Hashtable hashtable2 = new Hashtable();
				hashtable2.Add(PhotonPlayerProperty.customString, this.parameters[1].returnString(null));
				photonPlayer.SetCustomProperties(hashtable2);
				break;
			}
			case 14:
			{
				Hashtable hashtable = new Hashtable();
				hashtable.Add(PhotonPlayerProperty.RCteam, this.parameters[1].returnFloat(null));
				photonPlayer.SetCustomProperties(hashtable);
				break;
			}
			}
			break;
		}
		case 8:
			switch (this.actionType)
			{
			case 0:
			{
				TITAN tITAN3 = this.parameters[0].returnTitan(null);
				object[] array = new object[2]
				{
					this.parameters[1].returnPlayer(null).ID,
					this.parameters[2].returnInt(null)
				};
				tITAN3.photonView.RPC("titanGetHit", tITAN3.photonView.owner, array);
				break;
			}
			case 1:
				FengGameManagerMKII.instance.spawnTitanAction(this.parameters[0].returnInt(null), this.parameters[1].returnFloat(null), this.parameters[2].returnInt(null), this.parameters[3].returnInt(null));
				break;
			case 2:
				FengGameManagerMKII.instance.spawnTitanAtAction(this.parameters[0].returnInt(null), this.parameters[1].returnFloat(null), this.parameters[2].returnInt(null), this.parameters[3].returnInt(null), this.parameters[4].returnFloat(null), this.parameters[5].returnFloat(null), this.parameters[6].returnFloat(null));
				break;
			case 3:
			{
				TITAN tITAN2 = this.parameters[0].returnTitan(null);
				int currentHealth = this.parameters[1].returnInt(null);
				tITAN2.currentHealth = currentHealth;
				if (tITAN2.maxHealth == 0)
				{
					tITAN2.maxHealth = tITAN2.currentHealth;
				}
				tITAN2.photonView.RPC("labelRPC", PhotonTargets.AllBuffered, tITAN2.currentHealth, tITAN2.maxHealth);
				break;
			}
			case 4:
			{
				TITAN tITAN = this.parameters[0].returnTitan(null);
				if (tITAN.photonView.isMine)
				{
					tITAN.moveTo(this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null));
					break;
				}
				tITAN.photonView.RPC("moveToRPC", tITAN.photonView.owner, this.parameters[1].returnFloat(null), this.parameters[2].returnFloat(null), this.parameters[3].returnFloat(null));
				break;
			}
			}
			break;
		case 9:
			switch (this.actionType)
			{
			case 0:
				FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, this.parameters[0].returnString(null), string.Empty);
				break;
			case 1:
				FengGameManagerMKII.instance.gameWin2();
				if (this.parameters[0].returnBool(null))
				{
					FengGameManagerMKII.intVariables.Clear();
					FengGameManagerMKII.boolVariables.Clear();
					FengGameManagerMKII.stringVariables.Clear();
					FengGameManagerMKII.floatVariables.Clear();
					FengGameManagerMKII.playerVariables.Clear();
					FengGameManagerMKII.titanVariables.Clear();
				}
				break;
			case 2:
				FengGameManagerMKII.instance.gameLose2();
				if (this.parameters[0].returnBool(null))
				{
					FengGameManagerMKII.intVariables.Clear();
					FengGameManagerMKII.boolVariables.Clear();
					FengGameManagerMKII.stringVariables.Clear();
					FengGameManagerMKII.floatVariables.Clear();
					FengGameManagerMKII.playerVariables.Clear();
					FengGameManagerMKII.titanVariables.Clear();
				}
				break;
			case 3:
				if (this.parameters[0].returnBool(null))
				{
					FengGameManagerMKII.intVariables.Clear();
					FengGameManagerMKII.boolVariables.Clear();
					FengGameManagerMKII.stringVariables.Clear();
					FengGameManagerMKII.floatVariables.Clear();
					FengGameManagerMKII.playerVariables.Clear();
					FengGameManagerMKII.titanVariables.Clear();
				}
				FengGameManagerMKII.instance.restartGame2();
				break;
			}
			break;
		}
	}
}
