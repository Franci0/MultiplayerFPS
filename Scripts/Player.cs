using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

#pragma warning disable 649
public class Player : NetworkBehaviour
{
	public bool IsDead { 
		get { return _isDead; } 
		protected set { _isDead = value; } 
	}

	[SerializeField]
	int maxHealth = 100;

	[SerializeField]
	Behaviour[] disableOnDeath;

	bool[] wasEnabled;

	[SyncVar]
	int currentHealth;

	[SyncVar]
	bool _isDead = false;

	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.K)) {
			RpcTakeDamage (9999);
		}
	}

	[ClientRpc]
	public void RpcTakeDamage (int _damage)
	{
		if (IsDead) {
			return;
		}

		currentHealth -= _damage;
		Debug.Log (transform.name + " has now " + currentHealth + " health");

		if (currentHealth <= 0) {
			Die ();
		}
	}

	public void SetDefaults ()
	{
		IsDead = false;
		currentHealth = maxHealth;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = wasEnabled [i];
		}

		Collider _col = GetComponent<Collider> ();

		if (_col != null) {
			_col.enabled = true;
		}
	}

	public void Setup ()
	{
		wasEnabled = new bool[disableOnDeath.Length];

		for (int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled [i] = disableOnDeath [i].enabled;
		}

		SetDefaults ();
	}

	void Die ()
	{
		IsDead = true;
		DisableComponents ();

		Debug.Log (transform.name + " is dead");

		StartCoroutine (Respawn ());
	}

	void DisableComponents ()
	{
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath [i].enabled = false;
		}

		Collider _col = GetComponent<Collider> ();

		if (_col != null) {
			_col.enabled = false;
		}
	}

	IEnumerator Respawn ()
	{
		yield return new WaitForSeconds (GameManager.instance.matchSettings.respawnTime);
		SetDefaults ();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition ();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
		Debug.Log (transform.name + " respawned");
	}
}
