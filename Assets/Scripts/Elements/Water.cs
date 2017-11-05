using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Element {
	public GameObject waterJet;
	[SerializeField]
	private float waterJetCooldown = 0.1f;
	[SerializeField]
	private float jetStrength = 500;
	private float timeSinceFire;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > waterJetCooldown) {
			GameObject fb = Instantiate (waterJet, pos, Quaternion.identity);
			fb.GetComponent<WaterJet> ().Initialize (dir, jetStrength);
			timeSinceFire = 0;
		}
	}

	void Update() {
		timeSinceFire += Time.deltaTime;
	}
}