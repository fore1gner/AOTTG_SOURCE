using ExitGames.Client.Photon;
using UnityEngine;

public class BTN_choose_human : MonoBehaviour
{
	public bool isPlayerAllDead()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if ((int)photonPlayer.customProperties[PhotonPlayerProperty.isTitan] == 1)
			{
				num++;
				if ((bool)photonPlayer.customProperties[PhotonPlayerProperty.dead])
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	public bool isPlayerAllDead2()
	{
		int num = 0;
		int num2 = 0;
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (RCextensions.returnIntFromObject(photonPlayer.customProperties[PhotonPlayerProperty.isTitan]) == 1)
			{
				num++;
				if (RCextensions.returnBoolFromObject(photonPlayer.customProperties[PhotonPlayerProperty.dead]))
				{
					num2++;
				}
			}
		}
		return num == num2;
	}

	private void OnClick()
	{
		string selection = GameObject.Find("PopupListCharacterHUMAN").GetComponent<UIPopupList>().selection;
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], state: true);
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().needChooseSide = false;
		if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().checkpoint = GameObject.Find("PVPchkPtH");
		}
		if (!PhotonNetwork.isMasterClient && GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().roundTime > 60f)
		{
			if (!this.isPlayerAllDead2())
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
			}
			else
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.BOSS_FIGHT_CT || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.TROST || IN_GAME_MAIN_CAMERA.gamemode == GAMEMODE.PVP_CAPTURE)
		{
			if (this.isPlayerAllDead2())
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().NOTSpawnPlayer(selection);
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
			}
			else
			{
				GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection);
			}
		}
		else
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().SpawnPlayer(selection);
		}
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
		NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
		IN_GAME_MAIN_CAMERA.usingTitan = false;
		GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
		Hashtable customProperties = new Hashtable { 
		{
			PhotonPlayerProperty.character,
			selection
		} };
		PhotonNetwork.player.SetCustomProperties(customProperties);
	}
}
