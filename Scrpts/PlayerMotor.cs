using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
#pragma warning disable 649
public class PlayerMotor : MonoBehaviour
{
	[SerializeField]
	Camera cam;

	Vector3 velocity = Vector3.zero;
	Vector3 rotation = Vector3.zero;
	float cameraRotationX = 0f;
	float currentCameraRotationX = 0f;
	Vector3 thrusterForce = Vector3.zero;

	[SerializeField]
	float cameraRotationLimit = 85f;

	Rigidbody rb;

	//Gets a movement vector
	public void Move (Vector3 _velocity)
	{
		velocity = _velocity;
	}

	//Gets a rotational vector
	public void Rotate (Vector3 _rotation)
	{
		rotation = _rotation;
	}

	//Gets a rotational vector for the camera
	public void RotateCamera (float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}

	//Gets a thruster vector
	public void ApplyThruster (Vector3 _thrusterForce)
	{
		thrusterForce = _thrusterForce;
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	//Run every fixed iteration
	void FixedUpdate ()
	{
		PerformMovement ();
		PerformRotation ();
	}

	//Perform movement based on velocity variable
	void PerformMovement ()
	{
		if (velocity != Vector3.zero) {
			rb.MovePosition (transform.position	+ velocity * Time.fixedDeltaTime);
		}

		if (thrusterForce != Vector3.zero) {
			rb.AddForce (thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
	}

	//Perform rotation based on a rotation variable
	void PerformRotation ()
	{
		rb.MoveRotation (transform.rotation * Quaternion.Euler (rotation));

		if (cam != null) {

			//Set rotation and clamp it
			currentCameraRotationX -= cameraRotationX;
			currentCameraRotationX = Mathf.Clamp (currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

			//Apply rotation to the camera transform
			cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0, 0);
		}
	}
}
