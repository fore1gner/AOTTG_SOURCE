using System;
using System.Collections.Generic;
using UnityEngine;

public class PunTeams : MonoBehaviour
{
	public enum Team : byte
	{
		blue = 2,
		none = 0,
		red = 1
	}

	public static Dictionary<Team, List<PhotonPlayer>> PlayersPerTeam;

	public const string TeamPlayerProp = "team";

	public void OnJoinedRoom()
	{
		this.UpdateTeams();
	}

	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		this.UpdateTeams();
	}

	public void Start()
	{
		PunTeams.PlayersPerTeam = new Dictionary<Team, List<PhotonPlayer>>();
		foreach (object value in Enum.GetValues(typeof(Team)))
		{
			PunTeams.PlayersPerTeam[(Team)(byte)value] = new List<PhotonPlayer>();
		}
	}

	public void UpdateTeams()
	{
		foreach (object value in Enum.GetValues(typeof(Team)))
		{
			PunTeams.PlayersPerTeam[(Team)(byte)value].Clear();
		}
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			Team team = photonPlayer.GetTeam();
			PunTeams.PlayersPerTeam[team].Add(photonPlayer);
		}
	}
}
