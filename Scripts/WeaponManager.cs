using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 649
public class WeaponManager : NetworkBehaviour
{
	[SerializeField]
	PlayerWeapon primaryWeapon;
	[SerializeField]
	string weaponLayerName = "Weapon";
	[SerializeField]
	Transform weaponHolder;
	PlayerWeapon currentWeapon;

	public PlayerWeapon GetCurrentWeapon ()
	{
		return currentWeapon;
	}

	void Start ()
	{
		EquipWeapon (primaryWeapon);
	}

	void EquipWeapon (PlayerWeapon _weapon)
	{
		currentWeapon = _weapon;
		GameObject _weaponIns = (GameObject)Instantiate (_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
		_weaponIns.transform.SetParent (weaponHolder);

		if (isLocalPlayer) {
			_weaponIns.layer = LayerMask.NameToLayer (weaponLayerName);
		}
	}
}
