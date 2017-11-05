using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element {
	public GameObject fireball;
	[SerializeField]
	private float fireballCooldown = 0.2f;
	private float timeSinceFire;
	private bool fireReleased = true;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > fireballCooldown && fireReleased) {
			GameObject fb = Instantiate (fireball, pos, Quaternion.identity);
			fb.GetComponent<Fireball> ().Initialize (dir, 14);
			timeSinceFire = 0;
			fireReleased = false;
		}
	}

	void Update() {
		timeSinceFire += Time.deltaTime;

		// UGLY
		if (Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true ) {
			fireReleased = true;
		}
	}
}
