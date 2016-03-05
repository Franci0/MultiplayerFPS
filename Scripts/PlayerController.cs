using UnityEngine;

[RequireComponent (typeof(PlayerMotor))]
[RequireComponent (typeof(ConfigurableJoint))]
[RequireComponent (typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	float speed = 5f;
	[SerializeField]
	float lookSensitivity = 3f;
	[SerializeField]
	float thrusterForce = 1000f;

	[Header ("Joint Settings")]
	[SerializeField]
	float jointSpring = 20;
	[SerializeField]
	float jointMaxForce = 40;

	//Component caching
	Animator animator;
	PlayerMotor motor;
	ConfigurableJoint joint;

	void Start ()
	{
		motor = GetComponent<PlayerMotor> ();
		joint = GetComponent<ConfigurableJoint> ();
		animator = GetComponent<Animator> ();
		SetJointSettings (jointSpring);
	}

	void Update ()
	{
		//Calculate movement velocity as a 3D Vector
		float _xMov = Input.GetAxis ("Horizontal");
		float _zMov = Input.GetAxis ("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		//Final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		//Animate movement
		animator.SetFloat ("ForwardVelocity", _zMov);

		//Apply movement
		motor.Move (_velocity);

		//Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw ("Mouse X");

		Vector3 _rotation = new Vector3 (0, _yRot, 0) * lookSensitivity;

		//Apply rotation
		motor.Rotate (_rotation);

		//Calculate camera rotation as a 3D vector (turning around)
		float _xRot = Input.GetAxisRaw ("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		//Apply camera rotation
		motor.RotateCamera (_cameraRotationX);

		//Calculate thruster force based on player input
		Vector3 _thrusterForce = Vector3.zero;

		if (Input.GetButton ("Jump")) {
			_thrusterForce = Vector3.up * thrusterForce;
			SetJointSettings (0f);
		} else {
			SetJointSettings (jointSpring);
		}

		//Apply thruster force
		motor.ApplyThruster (_thrusterForce);
	}

	void SetJointSettings (float _jointSpring)
	{
		joint.yDrive = new JointDrive {
			positionSpring = _jointSpring,
			maximumForce = jointMaxForce
		};
	}
}
