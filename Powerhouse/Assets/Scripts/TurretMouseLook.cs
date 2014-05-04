using UnityEngine;
using System.Collections;

/**
 * Default mouselook, with the axis changed to work for the turret.
 */
[AddComponentMenu("Camera-Control/Mouse Look")]
public class TurretMouseLook: MonoBehaviour {
	
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -90F;
	public float maximumY = 0;
	
	float rotationY = -90F;
	
	void Update ()
	{
		if (axes == RotationAxes.MouseXAndY)
		{
			//float rotationX = transform.localEulerAngles.z + Input.GetAxis("Mouse X") * sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			//transform.localEulerAngles = new Vector3(rotationY, transform.localEulerAngles.y, rotationX);
			transform.localEulerAngles = new Vector3(rotationY, transform.localEulerAngles.y, transform.localEulerAngles.z);			
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0, Space.World);
		}
		else if (axes == RotationAxes.MouseX)
		{
			//transform.Rotate(0, 0, Input.GetAxis("Mouse X") * sensitivityX);
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0, Space.World);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
}


