using Photon;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MoveByKeys : Photon.MonoBehaviour
{
	public float speed = 10f;

	private void Start()
	{
		base.enabled = base.photonView.isMine;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			base.transform.position += Vector3.left * (this.speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			base.transform.position += Vector3.right * (this.speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.W))
		{
			base.transform.position += Vector3.forward * (this.speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			base.transform.position += Vector3.back * (this.speed * Time.deltaTime);
		}
	}
}
