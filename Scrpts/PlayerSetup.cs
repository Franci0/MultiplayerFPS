using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof(Player))]
#pragma warning disable 649
public class PlayerSetup : NetworkBehaviour
{
	[SerializeField]
	Behaviour[] componentsToDisable;
	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	Camera sceneCamera;

	void Start ()
	{
		if (!isLocalPlayer) {
			DisableComponents ();
			AssingRemoteLayer ();
		} else {
			sceneCamera = Camera.main;

			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive (false);
			}
		}

		GetComponent<Player> ().Setup ();
	}

	public override void OnStartClient ()
	{
		base.OnStartClient ();
		string _netID = GetComponent<NetworkIdentity> ().netId.ToString ();
		Player _player = GetComponent<Player> ();
		GameManager.RegisterPlayer (_netID, _player);
	}

	void OnDisable ()
	{
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}

		GameManager.UnregisterPlayer (transform.name);
	}

	void DisableComponents ()
	{
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable [i].enabled = false;
		}
	}

	void AssingRemoteLayer ()
	{
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}
}
