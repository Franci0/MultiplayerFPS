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
	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGraphics;
	[SerializeField]
	GameObject playerUIPrefab;

	GameObject playerUIInstance;
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

			//Disable player graphics for local player
			SetLayerRecursively (playerGraphics, LayerMask.NameToLayer (dontDrawLayerName));

			//Create playerUI
			playerUIInstance = Instantiate (playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;
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
		Destroy (playerUIInstance);

		//Re-enable the scene camera
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

	void SetLayerRecursively (GameObject _object, int _newLayer)
	{
		_object.layer = _newLayer;

		foreach (Transform child in _object.transform) {
			SetLayerRecursively (child.gameObject, _newLayer);
		}
	}
}
