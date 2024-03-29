using Photon;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : Photon.MonoBehaviour
{
	public bool PickupOnCollide;

	public float SecondsBeforeRespawn = 2f;

	public bool SentPickup;

	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnCollide && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	public void Pickup()
	{
		if (!this.SentPickup)
		{
			this.SentPickup = true;
			base.photonView.RPC("PunPickupSimple", PhotonTargets.AllViaServer);
		}
	}

	[RPC]
	public void PunPickupSimple(PhotonMessageInfo msgInfo)
	{
		if (this.SentPickup && msgInfo.sender.isLocal)
		{
			base.gameObject.GetActive();
		}
		this.SentPickup = false;
		if (!base.gameObject.GetActive())
		{
			Debug.Log("Ignored PU RPC, cause item is inactive. " + base.gameObject);
			return;
		}
		double num = PhotonNetwork.time - msgInfo.timestamp;
		float num2 = this.SecondsBeforeRespawn - (float)num;
		if (num2 > 0f)
		{
			base.gameObject.SetActive(value: false);
			base.Invoke("RespawnAfter", num2);
		}
	}

	public void RespawnAfter()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(value: true);
		}
	}
}
