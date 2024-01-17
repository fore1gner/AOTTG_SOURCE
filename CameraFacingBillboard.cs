using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
	public enum Axis
	{
		up,
		down,
		left,
		right,
		forward,
		back
	}

	public Axis axis;

	private Camera referenceCamera;

	public bool reverseFace;

	private void Awake()
	{
		if (this.referenceCamera == null)
		{
			this.referenceCamera = Camera.main;
		}
	}

	public Vector3 GetAxis(Axis refAxis)
	{
		return refAxis switch
		{
			Axis.down => Vector3.down, 
			Axis.left => Vector3.left, 
			Axis.right => Vector3.right, 
			Axis.forward => Vector3.forward, 
			Axis.back => Vector3.back, 
			_ => Vector3.up, 
		};
	}

	private void Update()
	{
		Vector3 worldPosition = base.transform.position + this.referenceCamera.transform.rotation * ((!this.reverseFace) ? Vector3.back : Vector3.forward);
		Vector3 worldUp = this.referenceCamera.transform.rotation * this.GetAxis(this.axis);
		base.transform.LookAt(worldPosition, worldUp);
	}
}
