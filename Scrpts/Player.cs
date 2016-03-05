using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	[SerializeField]
	int maxHealth = 100;

	[SyncVar]
	int currentHealth;

	public void TakeDamage (int _damage)
	{
		currentHealth -= _damage;
		Debug.Log (transform.name + " has now " + currentHealth + " health");
	}

	public void SetDefaults ()
	{
		currentHealth = maxHealth;
	}

	void Awake ()
	{
		SetDefaults ();
	}
		
}
