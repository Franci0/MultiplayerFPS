using UnityEngine;
using UnityEngine.Networking;

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

		RegisterPlayer ();
	}

	void OnDisable ()
	{
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}
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

	void RegisterPlayer ()
	{
		gameObject.name = "Player " + GetComponent<NetworkIdentity> ().netId;
	}
}
