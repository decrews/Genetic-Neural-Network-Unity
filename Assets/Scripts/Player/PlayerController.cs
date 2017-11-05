using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour {

	private Rigidbody2D rb;
	private Collider2D col;
	private Player playerScript; // Reference to player script where elemental abilities live

	[SerializeField]
	private float baseSpeed = 250;
	[SerializeField]
	private float sprintMulti = 1.4f;
	private float speed;

	[SerializeField]
	private float gravity = 20;

	private bool grounded;

    [Range(0.01f, 5.0f)]
    public float jumpTravel = 1.8f;

    [Range(0.01f, 10.0f)]
    public float jumpSpeed = 3.0f;

    [Range(0.01f, 5.0f)]
    public float curveCutoff = 3.0f;

	public LayerMask groundMask;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<BoxCollider2D> ();
		playerScript = GetComponent<Player> ();
	}


	void Update () {
		// ------------- Visualize groundcheck rays -----------------------------
		Vector3[] castPos = new Vector3[] { transform.position, 
			new Vector3 (transform.position.x - col.bounds.extents.x + 0.1f, transform.position.y, transform.position.z),
			new Vector3 (transform.position.x + col.bounds.extents.x - 0.1f, transform.position.y, transform.position.z)
		};
		foreach (Vector3 pos in castPos) {
			Debug.DrawRay (pos, Vector3.down, Color.red);
		}
		// ------------- Visualize groundcheck rays -----------------------------

		// Jumping

		//Debug.Log(Input.GetButtonDown("Jump") && Grounded());
		if (Input.GetButtonDown("Jump") && Grounded()) {
            StartCoroutine("JumpCurve");
		}

		// Sprinting
		if (Input.GetKey (KeyCode.LeftShift) && Grounded()) {
			speed = baseSpeed * sprintMulti;
		} else if (Grounded()) {
			speed = baseSpeed;
		}
	}


	void FixedUpdate() {
		rb.AddForce(Vector3.down * gravity * rb.mass); // Add more weight to the player
		Move ();
	}
		

	private void Move() {
		rb.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed * Time.deltaTime, rb.velocity.y);
	}
		

    private IEnumerator JumpCurve()
    {
        float time = (10.0f - jumpSpeed) / Mathf.Pow(10.0f, jumpTravel);
        float curveVel = jumpTravel / time;

		while (Input.GetButton("Jump") && curveVel > curveCutoff)
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
}
