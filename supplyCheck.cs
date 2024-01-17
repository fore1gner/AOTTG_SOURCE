using UnityEngine;

public class supplyCheck : MonoBehaviour
{
	private float elapsedTime;

	private float stepTime = 1f;

	private void Start()
	{
		if (Minimap.instance != null)
		{
			Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.white, trackOrientation: false, depthAboveAll: true, Minimap.IconStyle.SUPPLY);
		}
	}

	private void Update()
	{
		this.elapsedTime += Time.deltaTime;
		if (!(this.elapsedTime > this.stepTime))
		{
			return;
		}
		this.elapsedTime -= this.stepTime;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject gameObject in array)
		{
			if (!(gameObject.GetComponent<HERO>() != null))
			{
				continue;
			}
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				if (Vector3.Distance(gameObject.transform.position, base.transform.position) < 1.5f)
				{
					gameObject.GetComponent<HERO>().getSupply();
				}
			}
			else if (gameObject.GetPhotonView().isMine && Vector3.Distance(gameObject.transform.position, base.transform.position) < 1.5f)
			{
				gameObject.GetComponent<HERO>().getSupply();
			}
		}
	}
}
