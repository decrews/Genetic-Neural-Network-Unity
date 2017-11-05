using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	[SerializeField]
	private GameObject absorber;
	[SerializeField]
	private float shootCooldown = 0.25f;
	[SerializeField]
	private float absorberSpeed = 10f;
	[SerializeField]
	private float fireSpeed = 10f;
	[SerializeField]
	private float absorberCooldown = 0.25f;
	private float absorbTimer = 0f;

	private bool secondaryBtnRelease = true;
	private bool primaryBtnRelease = true;

	// Event broadcast
	public delegate void ShootAction();
	public static event ShootAction OnShoot;

	private Player plr;

	GameManager gm;
	void Start() {
		gm = GameManager.instance;
		plr = GetComponent<Player> ();
	}

	void Update () {
		absorbTimer += Time.deltaTime;

		if (gm.controllerConnected) {
			if (Input.GetAxisRaw ("SecondaryFireGamePad") <= 0.1) {
				secondaryBtnRelease = true;
			}
			if (Input.GetAxisRaw ("PrimaryFireGamePad") <= 0.1) {
				primaryBtnRelease = true;
			}

			if (Input.GetAxis ("PrimaryFireGamePad") > 0.5f && primaryBtnRelease) {
				Debug.Log ("Triggered primary fire on controller!");
				// Shoot goes here
			}

			if (Input.GetAxis ("SecondaryFireGamePad") > 0.5f && absorbTimer > absorberCooldown && secondaryBtnRelease) {
				secondaryBtnRelease = false;
				Absorb ("right");

				// Broadcast the shoot event to anyone who cares (PlayerAnimations)
				if (OnShoot != null) {
					OnShoot ();
				}
			}
		} else {
			
			//If PrimaryAbsorb
			if (Input.GetButton ("PrimaryFire") && (Input.GetButton ("LeftCtrl")) && absorbTimer > absorberCooldown) {
				Absorb ("left");
			}

			//If SecondaryAbsorb
			if (Input.GetButton ("SecondaryFire") && (Input.GetButton ("LeftCtrl")) && absorbTimer > absorberCooldown) {
				Absorb ("right");
			}

			if (Input.GetButton ("PrimaryFire") && !(Input.GetButton ("LeftCtrl"))) {
				if (plr.leftElement != null) {
					useElement ("left");
				}
			}
				
			if (Input.GetButton ("SecondaryFire") && !(Input.GetButton ("LeftCtrl"))) {
				if (plr.rightElement != null)
					useElement ("right");
			}
		}
	}

	void Absorb(string hand) {
		// No controller detected, use mouse and keyboard
		if (!gm.controllerConnected) {
			// Get the location of the mouse relative to the player
			Vector3 dirV3 = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;

			// Convert this into a Vector2
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			// Instantiate the Sorb Orb with a direction and speed
			GameObject blt = Instantiate (absorber, transform.position, transform.rotation);
			blt.GetComponent<Absorber> ().Initialize (dir, absorberSpeed, hand);

			// restart the clock on the shoot cooldown
			absorbTimer = 0f;
		} else {
			Debug.Log ("shooting absorb with controller");
			float x = Input.GetAxis ("RightHorizontal");
			float y = Input.GetAxis ("RightVertical");

			Vector2 dir = new Vector2 (x, -y).normalized;

			// Instantiate the Sorb Orb with a direction and speed
			GameObject blt = Instantiate (absorber, transform.position, transform.rotation);
			blt.GetComponent<Absorber> ().Initialize (dir, absorberSpeed, hand);

			// restart the clock on the shoot cooldown
			absorbTimer = 0f;	
		}
	}

	void useElement(string hand) {
		// No controller detected, use mouse and keyboard
		if (!gm.controllerConnected) {

			// Get the location of the mouse relative to the player
			Vector3 dirV3 = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			// Convert this into a Vector2
			Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

			if (hand == "left") {
				plr.leftElement.UseElement (transform.position, dir);
			} else {
				plr.rightElement.UseElement (transform.position, dir);
			}
		} else {
			Debug.Log ("shooting absorb with controller");
			float x = Input.GetAxis ("RightHorizontal");
			float y = Input.GetAxis ("RightVertical");

			Vector2 dir = new Vector2 (x, -y).normalized;

			if (hand == "left") {
				plr.leftElement.UseElement (transform.position, dir);
			} else {
				plr.rightElement.UseElement (transform.position, dir);
			}
		}
	}
}
