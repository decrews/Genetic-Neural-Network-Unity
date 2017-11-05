using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Absorbs different materials for player to use
public class Absorber : MonoBehaviour {

	public float speed;
	public Vector3 direction;
	private string hand;

	Player plr;

	public void Initialize(Vector2 direction, float speed, string hand) {
		this.speed = speed;
		this.direction = direction.normalized;
		this.hand = hand;
	}

	void Start() {
		plr = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
	}

	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player") {
			DestroyObject (this.gameObject);
		}

		if (col.gameObject.tag == "FireElement") {
			if (hand == "left") {
				Debug.Log ("Added fire to player's left hand");
				plr.leftElement = plr.elements["fire"];
			} else {
				Debug.Log ("Added fire to player's right hand");
				plr.rightElement = plr.elements["fire"];
			}
		}

		if (col.gameObject.tag == "WaterElement") {
			if (hand == "left") {
				Debug.Log ("Added water to player's left hand");
				plr.leftElement = plr.elements["water"];
			} else {
				Debug.Log ("Added water to player's right hand");
				plr.rightElement = plr.elements["water"];
			}
		}
	}
}
