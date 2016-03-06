using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 649
public class PlayerShoot : NetworkBehaviour
{
	const string PLAYER_TAG = "Player";

	[SerializeField]
	PlayerWeapon weapon;
	[SerializeField]
	GameObject weaponGfx;
	[SerializeField]
	string weaponLayerName = "Weapon";
	[SerializeField]
	Camera cam = null;
	[SerializeField]
	LayerMask mask;

	void Start ()
	{
		if (cam == null) {
			Debug.LogError ("PlayerShoot : No camera referenced");
			enabled = false;
		}

		weaponGfx.layer = LayerMask.NameToLayer (weaponLayerName);
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Fire1")) {
			Shoot ();
		}
	}

	[Client]
	void Shoot ()
	{
		RaycastHit _hit;

		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) {
			if (_hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot (_hit.collider.name, weapon.damage);
			}
		}
	}

	[Command]
	void CmdPlayerShot (string _playerID, int _damage)
	{
		Debug.Log (_playerID + " has been shot");
		Player _player = GameManager.GetPlayer (_playerID);
		_player.RpcTakeDamage (_damage);
	}
}
