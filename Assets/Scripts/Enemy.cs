using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Vector3 vel;
	private float previousTime;
	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D> ();
		previousTime = Time.time;
		vel = new Vector3 (-1, 0, 0);
	}

	void Update () {
		rb.velocity = vel;
		if (Time.time - previousTime > 2) {
			vel = new Vector3 (-vel.x, vel.y, vel.z);
			previousTime = Time.time;
		}
	}
}
