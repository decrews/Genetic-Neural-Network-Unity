using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour {

	private bool initilized = false;

	private NeuralNetwork net;
	private Rigidbody2D rb;
	private Collider2D col;

	private GameObject[] platforms;
	private GameObject[] enemies;

	public LayerMask groundMask;
	private bool jumping;
	private float botSpeed = 250f;
	public float jumpTravel = 1.8f;
	public float jumpSpeed = 3.0f;
	public float curveCutoff = 3.0f;
	private bool jumpDown = false;

	private float minimumBotSpeed = 0.05f;

	public Vector3 prevPos;
	public int ticks = 0;

	void Start()
	{
		//prevPos = transform.position;
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<BoxCollider2D> (); 

		// Remove the collision with other bots
		GameObject[] otherBots = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < otherBots.Length; i++) {
			Physics2D.IgnoreCollision(otherBots[i].GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
	}

	void FixedUpdate ()
	{
		if (initilized == true)
		{
			// Every 10 updates, check to see if the bot has made progress
			if (ticks % 10 == 0) {
				if (Mathf.Abs(transform.position.x - prevPos.x) < minimumBotSpeed) {
					destroyBot ();
				} else {
					prevPos = transform.position;
				}
			}

			// Find all the gronud platforms and enemies (expensvie)
			GameObject[] platforms = GameObject.FindGameObjectsWithTag ("Ground");
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

			// Make a list for all platform positions relative to themselves
			List<float> positions = new List<float> ();

			// Add each x and y position to the input array
			for (int i = 0; i < platforms.Length; i++) {
				positions.Add(transform.position.x - platforms [i].transform.position.x);
				positions.Add(transform.position.y - platforms [i].transform.position.y);
			}

			// Check to see if there are enemies.  Add them.
			if (enemies != null) {
				for (int i = 0; i < enemies.Length; i++) {
					positions.Add(transform.position.x - enemies [i].transform.position.x);
					positions.Add(transform.position.y - enemies [i].transform.position.y);
				}
			}


			// Get the output from the network and set the move and jump values
			float[] output = net.FeedForward(positions.ToArray());
			float xMove = output [0];
			float jump = output [1];
			if (jump > 0.5) { 
				jumpDown = true;
			} else {
				jumpDown = false;	
			}


			// Move the player left or right based on the first output
			rb.velocity = new Vector2 (xMove * botSpeed * Time.deltaTime, rb.velocity.y);

			// Determine if the player should jump based on the second output
			if (jumpDown && Grounded()) {
				StartCoroutine("JumpCurve");
			}

			// Bot fitness is based on how far right it makes it in the level
			net.AddFitness (transform.position.x);
		}

		// Destroy the bot if it falls off the platform
		if (transform.position.y < 0) {
			destroyBot ();
		}

		ticks++;
	}

	public void Init(NeuralNetwork net)
	{
		this.net = net;
		initilized = true;

	}




	// From Tobor
	// --------------------------------------------------------------------
	private IEnumerator JumpCurve()
	{
		float time = (10.0f - jumpSpeed) / Mathf.Pow(10.0f, jumpTravel);
		float curveVel = jumpTravel / time;

		while (jumpDown && (curveVel > curveCutoff))
		{
			rb.velocity = new Vector2(rb.velocity.x, curveVel);
			time += Time.fixedDeltaTime;
			curveVel = jumpTravel / time;
			yield return new WaitForFixedUpdate ();
		}

		rb.velocity = new Vector2(rb.velocity.x, Vector2.down.y * 0.01f);
	}


	public bool Grounded() {
		if (JumpCasts()) {
			// On the ground
			return true;
		} else {
			// In the air
			return false;
		};
	}


	private bool JumpCasts() {
		Vector3[] castPos = new Vector3[] { transform.position, 
			new Vector3 (transform.position.x - col.bounds.extents.x + 0.005f, transform.position.y, transform.position.z),
			new Vector3 (transform.position.x + col.bounds.extents.x - 0.005f, transform.position.y, transform.position.z)
		};

		bool validJump = false;
		foreach (Vector3 pos in castPos) {
			if (Physics2D.Raycast (pos, Vector2.down, col.bounds.extents.y + 0.2f, groundMask).collider != null) {
				validJump = true;
			}
		}

		return validJump;
	}

	private void destroyBot() {
		GameObject gm = GameObject.Find ("GameManager");
		gm.GetComponent<AgentManager> ().botsList.Remove (this);

		Destroy (this.gameObject);
	}

	private void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Enemy") {
			destroyBot ();
		}
	}
}
