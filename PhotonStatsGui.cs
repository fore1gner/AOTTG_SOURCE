using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonStatsGui : MonoBehaviour
{
	public bool buttonsOn;

	public bool healthStatsVisible;

	public bool statsOn = true;

	public Rect statsRect = new Rect(0f, 100f, 200f, 50f);

	public bool statsWindowOn = true;

	public bool trafficStatsOn;

	public int WindowId = 100;

	public void OnGUI()
	{
		if (PhotonNetwork.networkingPeer.TrafficStatsEnabled != this.statsOn)
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = this.statsOn;
		}
		if (this.statsWindowOn)
		{
			this.statsRect = GUILayout.Window(this.WindowId, this.statsRect, TrafficStatsWindow, "Messages (shift+tab)");
		}
	}

	public void Start()
	{
		this.statsRect.x = (float)Screen.width - this.statsRect.width;
	}

	public void TrafficStatsWindow(int windowID)
	{
		bool flag = false;
		TrafficStatsGameLevel trafficStatsGameLevel = PhotonNetwork.networkingPeer.TrafficStatsGameLevel;
		long num = PhotonNetwork.networkingPeer.TrafficStatsElapsedMs / 1000;
		if (num == 0L)
		{
			num = 1L;
		}
		GUILayout.BeginHorizontal();
		this.buttonsOn = GUILayout.Toggle(this.buttonsOn, "buttons");
		this.healthStatsVisible = GUILayout.Toggle(this.healthStatsVisible, "health");
		this.trafficStatsOn = GUILayout.Toggle(this.trafficStatsOn, "traffic");
		GUILayout.EndHorizontal();
		string text = $"Out|In|Sum:\t{trafficStatsGameLevel.TotalOutgoingMessageCount,4} | {trafficStatsGameLevel.TotalIncomingMessageCount,4} | {trafficStatsGameLevel.TotalMessageCount,4}";
		string text2 = $"{num}sec average:";
		string text3 = $"Out|In|Sum:\t{trafficStatsGameLevel.TotalOutgoingMessageCount / num,4} | {trafficStatsGameLevel.TotalIncomingMessageCount / num,4} | {trafficStatsGameLevel.TotalMessageCount / num,4}";
		GUILayout.Label(text);
		GUILayout.Label(text2);
		GUILayout.Label(text3);
		if (this.buttonsOn)
		{
			GUILayout.BeginHorizontal();
			this.statsOn = GUILayout.Toggle(this.statsOn, "stats on");
			if (GUILayout.Button("Reset"))
			{
				PhotonNetwork.networkingPeer.TrafficStatsReset();
				PhotonNetwork.networkingPeer.TrafficStatsEnabled = true;
			}
			flag = GUILayout.Button("To Log");
			GUILayout.EndHorizontal();
		}
		string text4 = string.Empty;
		string text5 = string.Empty;
		if (this.trafficStatsOn)
		{
			text4 = "Incoming: " + PhotonNetwork.networkingPeer.TrafficStatsIncoming.ToString();
			text5 = "Outgoing: " + PhotonNetwork.networkingPeer.TrafficStatsOutgoing.ToString();
			GUILayout.Label(text4);
			GUILayout.Label(text5);
		}
		string text6 = string.Empty;
		if (this.healthStatsVisible)
		{
			object[] args = new object[8]
			{
				trafficStatsGameLevel.LongestDeltaBetweenSending,
				trafficStatsGameLevel.LongestDeltaBetweenDispatching,
				trafficStatsGameLevel.LongestEventCallback,
				trafficStatsGameLevel.LongestEventCallbackCode,
				trafficStatsGameLevel.LongestOpResponseCallback,
				trafficStatsGameLevel.LongestOpResponseCallbackOpCode,
				PhotonNetwork.networkingPeer.RoundTripTime,
				PhotonNetwork.networkingPeer.RoundTripTimeVariance
			};
			text6 = string.Format("ping: {6}[+/-{7}]ms\nlongest delta between\nsend: {0,4}ms disp: {1,4}ms\nlongest time for:\nev({3}):{2,3}ms op({5}):{4,3}ms", args);
			GUILayout.Label(text6);
		}
		if (flag)
		{
			object[] args2 = new object[6] { text, text2, text3, text4, text5, text6 };
			Debug.Log(string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", args2));
		}
		if (GUI.changed)
		{
			this.statsRect.height = 100f;
		}
		GUI.DragWindow();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
		{
			this.statsWindowOn = !this.statsWindowOn;
			this.statsOn = true;
		}
	}
}
