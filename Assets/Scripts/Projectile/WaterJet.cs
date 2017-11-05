using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJet : MonoBehaviour {
	public Vector3 direction;
	[SerializeField]

	public void Initialize(Vector2 direction, float speed) {
		this.direction = direction.normalized;
		gameObject.GetComponent<Rigidbody2D> ().AddForce (this.direction*speed);
	}

	/*
	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}
	*/

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player" && col.gameObject.tag != "WaterElement" ) {
			DestroyObject (this.gameObject);
		}
	}
}
