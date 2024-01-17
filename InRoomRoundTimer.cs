using ExitGames.Client.Photon;
using UnityEngine;

public class InRoomRoundTimer : MonoBehaviour
{
	public int SecondsPerTurn = 5;

	private bool startRoundWhenTimeIsSynced;

	public double StartTime;

	private const string StartTimeKey = "st";

	public Rect TextPos = new Rect(0f, 80f, 150f, 300f);

	public void OnGUI()
	{
		double num = PhotonNetwork.time - this.StartTime;
		double num2 = (double)this.SecondsPerTurn - num % (double)this.SecondsPerTurn;
		int num3 = (int)(num / (double)this.SecondsPerTurn);
		GUILayout.BeginArea(this.TextPos);
		GUILayout.Label($"elapsed: {num:0.000}");
		GUILayout.Label($"remaining: {num2:0.000}");
		GUILayout.Label($"turn: {num3:0}");
		if (GUILayout.Button("new round"))
		{
			this.StartRoundNow();
		}
		GUILayout.EndArea();
	}

	public void OnJoinedRoom()
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.StartRoundNow();
		}
		else
		{
			Debug.Log("StartTime already set: " + PhotonNetwork.room.customProperties.ContainsKey("st"));
		}
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!PhotonNetwork.room.customProperties.ContainsKey("st"))
		{
			Debug.Log("The new master starts a new round, cause we didn't start yet.");
			this.StartRoundNow();
		}
	}

	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("st"))
		{
			this.StartTime = (double)propertiesThatChanged["st"];
		}
	}

	private void StartRoundNow()
	{
		if (PhotonNetwork.time < 9.999999747378752E-05)
		{
			this.startRoundWhenTimeIsSynced = true;
			return;
		}
		this.startRoundWhenTimeIsSynced = false;
		Hashtable hashtable = new Hashtable();
		hashtable["st"] = PhotonNetwork.time;
		PhotonNetwork.room.SetCustomProperties(hashtable);
	}

	private void Update()
	{
		if (this.startRoundWhenTimeIsSynced)
		{
			this.StartRoundNow();
		}
	}
}
