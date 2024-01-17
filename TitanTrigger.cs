using Constants;
using UnityEngine;

public class TitanTrigger : MonoBehaviour
{
	public bool isCollide;

	private void OnTriggerEnter(Collider other)
	{
		if (this.isCollide)
		{
			return;
		}
		GameObject gameObject = other.transform.root.gameObject;
		if (gameObject.layer != PhysicsLayer.Players)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (gameObject.GetPhotonView().isMine)
			{
				this.isCollide = true;
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			GameObject main_object = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
			if (main_object != null && main_object == gameObject)
			{
				this.isCollide = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!this.isCollide)
		{
			return;
		}
		GameObject gameObject = other.transform.root.gameObject;
		if (gameObject.layer != PhysicsLayer.Players)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
		{
			if (gameObject.GetPhotonView().isMine)
			{
				this.isCollide = false;
			}
		}
		else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			GameObject main_object = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object;
			if (main_object != null && main_object == gameObject)
			{
				this.isCollide = false;
			}
		}
	}
}
