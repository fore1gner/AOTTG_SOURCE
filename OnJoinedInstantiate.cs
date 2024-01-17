using UnityEngine;

public class OnJoinedInstantiate : MonoBehaviour
{
	public float PositionOffset = 2f;

	public GameObject[] PrefabsToInstantiate;

	public Transform SpawnPosition;

	public void OnJoinedRoom()
	{
		if (this.PrefabsToInstantiate == null)
		{
			return;
		}
		GameObject[] prefabsToInstantiate = this.PrefabsToInstantiate;
		foreach (GameObject gameObject in prefabsToInstantiate)
		{
			Debug.Log("Instantiating: " + gameObject.name);
			Vector3 vector = Vector3.up;
			if (this.SpawnPosition != null)
			{
				vector = this.SpawnPosition.position;
			}
			Vector3 insideUnitSphere = Random.insideUnitSphere;
			insideUnitSphere.y = 0f;
			insideUnitSphere = insideUnitSphere.normalized;
			Vector3 position = vector + this.PositionOffset * insideUnitSphere;
			PhotonNetwork.Instantiate(gameObject.name, position, Quaternion.identity, 0);
		}
	}
}
