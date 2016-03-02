using UnityEngine;

[RequireComponent (typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private float speed = 5f;

	private PlayerMotor motor;

	void Start ()
	{
		motor = GetComponent<PlayerMotor> ();
	}

	void Update ()
	{
		//Calculate movement velocity as a 3D Vector
		float _xMov = Input.GetAxisRaw ("Horizontal");
		float _zMov = Input.GetAxisRaw ("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		//Final movememnt vector
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

		//Apply movement
		motor.Move (_velocity);
	}
}
