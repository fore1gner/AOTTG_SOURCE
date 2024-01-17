using UnityEngine;

public class BloodSplatterScript : MonoBehaviour
{
	private GameObject[] bloodInstances;

	public int bloodLocalRotationYOffset;

	public Transform bloodPosition;

	public Transform bloodPrefab;

	public Transform bloodRotation;

	public int maxAmountBloodPrefabs = 20;

	public void Main()
	{
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.bloodRotation.Rotate(0f, this.bloodLocalRotationYOffset, 0f);
			Object.Instantiate(this.bloodPrefab, this.bloodPosition.position, this.bloodRotation.rotation);
			this.bloodInstances = GameObject.FindGameObjectsWithTag("blood");
			if (this.bloodInstances.Length >= this.maxAmountBloodPrefabs)
			{
				Object.Destroy(this.bloodInstances[0]);
			}
		}
	}
}
