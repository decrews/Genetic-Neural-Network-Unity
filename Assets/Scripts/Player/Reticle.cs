using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour {
	public float reticleDistance = 1.5f;
	GameManager gm;

	void Start () {
		// Get the game manager so we can check to see if a controller is connected
		gm = GameManager.instance;	
	}

	void Update () {

	}

	void LateUpdate () {

		if (!gm.controllerConnected) {
			// Get the location of the mouse relative to the player
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			transform.position = new Vector3 (mousePos.x, mousePos.y, 1);
		} else {
			float x = Input.GetAxis ("RightHorizontal");
			float y = Input.GetAxis ("RightVertical");

			Vector3 dir = new Vector3 (x, -y, 0).normalized;
			transform.position = transform.parent.position + (dir * reticleDistance);
		}

		/* Old stuff, could be useful
		// Get the location of the mouse relative to the player
		Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.root.position;

		// Update the position of the reticle
		Vector3 offset = new Vector3(dir.x, dir.y, 1).normalized * reticleDistance;
		transform.position = transform.parent.position + offset;
		*/

	}
}