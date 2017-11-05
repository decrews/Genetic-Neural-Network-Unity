using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimation : MonoBehaviour {
	[SerializeField]
	private Animator playerAnim;
	[SerializeField]
	private float sprintMulti;

	private PlayerController playerControl;
	private bool landed;
	private bool shooting;

	void Start() {
		playerAnim = GetComponent<Animator> ();
		playerControl = GetComponent<PlayerController> ();

		PlayerShooting.OnShoot += PlayShoot;
	}

	void Update() {
		// Jumping
		if (Input.GetButtonDown ("Jump") && landed) {
			landed = false;
		}

		// Sprinting
		if (Input.GetKey (KeyCode.LeftShift) && landed) {
			if (playerAnim != null)
				playerAnim.speed = sprintMulti;
		} else if (landed) {
			if (playerAnim != null)
				playerAnim.speed = 1;
		}


		if (landed == false) {
			playerAnim.SetInteger ("State", 2);
		}

			
		/*
		// Change the player's facing direction based on mouse
		if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x) {
			transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
		} else if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x) {
			transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
		}
		*/

		// Change the player's facing direction based on directional movement
		if (Input.GetAxis ("Horizontal") > 0) {
			transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
		} else if (Input.GetAxis ("Horizontal") < 0) {
			transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
		}


		// Determine if the player is standing or running
		if (landed && Input.GetAxis("Horizontal") > 0.001 || landed && Input.GetAxis("Horizontal") < -0.001) {
			if (shooting != true)
				playerAnim.SetInteger ("State", 1);
		}
		else if (landed && Input.GetAxis("Horizontal") <= 0.001 && Input.GetAxis("Horizontal") >= -0.001) {
			if (shooting != true)
				playerAnim.SetInteger ("State", 0);
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		landed = true;
	}


	IEnumerator StandUp() {
		yield return new WaitForSeconds (0.25f);
		shooting = false;
		playerAnim.SetInteger ("State", 0);
	}

	void PlayShoot() {
		if (landed) {
			shooting = true;
			playerAnim.SetInteger ("State", 3);
			StartCoroutine ("StandUp");
		}
	}

}
