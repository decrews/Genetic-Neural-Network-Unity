using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject player;
	public bool controllerConnected = false;

	void Awake () {
		// Allow the game manage to survive scene transition
		DontDestroyOnLoad (transform.gameObject);

		// Disable cursor
		Cursor.visible = false;

		// Creates singleton game manager
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		CheckControllerStatus ();
	}


	void CheckControllerStatus() {
		string[] joystickNames = Input.GetJoystickNames ();

		if (joystickNames.Length > 0) {
			foreach (var str in joystickNames) {
				if (!string.IsNullOrEmpty (str) && controllerConnected == false) {
					controllerConnected = true;
				} else if (string.IsNullOrEmpty (str) && controllerConnected == true) {
					controllerConnected = false;
				}
			}
		}
	}
}
