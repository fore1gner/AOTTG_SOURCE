using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonLagSimulationGui : MonoBehaviour
{
	public bool Visible = true;

	public int WindowId = 101;

	public Rect WindowRect = new Rect(0f, 100f, 120f, 100f);

	public PhotonPeer Peer { get; set; }

	private void NetSimHasNoPeerWindow(int windowId)
	{
		GUILayout.Label("No peer to communicate with. ");
	}

	private void NetSimWindow(int windowId)
	{
		GUILayout.Label($"Rtt:{this.Peer.RoundTripTime,4} +/-{this.Peer.RoundTripTimeVariance,3}");
		bool ısSimulationEnabled = this.Peer.IsSimulationEnabled;
		bool flag = GUILayout.Toggle(ısSimulationEnabled, "Simulate");
		if (flag != ısSimulationEnabled)
		{
			this.Peer.IsSimulationEnabled = flag;
		}
		float value = this.Peer.NetworkSimulationSettings.IncomingLag;
		GUILayout.Label("Lag " + value);
		value = GUILayout.HorizontalSlider(value, 0f, 500f);
		this.Peer.NetworkSimulationSettings.IncomingLag = (int)value;
		this.Peer.NetworkSimulationSettings.OutgoingLag = (int)value;
		float value2 = this.Peer.NetworkSimulationSettings.IncomingJitter;
		GUILayout.Label("Jit " + value2);
		value2 = GUILayout.HorizontalSlider(value2, 0f, 100f);
		this.Peer.NetworkSimulationSettings.IncomingJitter = (int)value2;
		this.Peer.NetworkSimulationSettings.OutgoingJitter = (int)value2;
		float value3 = this.Peer.NetworkSimulationSettings.IncomingLossPercentage;
		GUILayout.Label("Loss " + value3);
		value3 = GUILayout.HorizontalSlider(value3, 0f, 10f);
		this.Peer.NetworkSimulationSettings.IncomingLossPercentage = (int)value3;
		this.Peer.NetworkSimulationSettings.OutgoingLossPercentage = (int)value3;
		if (GUI.changed)
		{
			this.WindowRect.height = 100f;
		}
		GUI.DragWindow();
	}

	public void OnGUI()
	{
		if (this.Visible)
		{
			if (this.Peer == null)
			{
				this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, NetSimHasNoPeerWindow, "Netw. Sim.");
			}
			else
			{
				this.WindowRect = GUILayout.Window(this.WindowId, this.WindowRect, NetSimWindow, "Netw. Sim.");
			}
		}
	}

	public void Start()
	{
		this.Peer = PhotonNetwork.networkingPeer;
	}
}
