using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	public int health;
	public Element leftElement;
	public Element rightElement;
	public Dictionary<string, Element> elements;

	void Start () {
		// Initialize a dictionary of elements that can be found in the world
		elements = new Dictionary<string, Element> ();
		Element fire = GetComponentInChildren<Fire> ();
		elements.Add("fire", fire);

		Element water = GetComponentInChildren<Water> ();
		elements.Add("water", water);
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	// Test for the Playground, if you hit Lava reload
	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log (col.gameObject.name);
		if (col.gameObject.name == "Lava") {
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
	}
}
