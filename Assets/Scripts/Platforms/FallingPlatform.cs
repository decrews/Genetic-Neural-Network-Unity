using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatform : MonoBehaviour {

	private Rigidbody2D rb;
	private Tilemap tilemap;
	public GameObject fallingPrefab;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		tilemap = GetComponent<Tilemap>();
	}


	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Player") {
			DestroyBlock (collision);
		}
	}


	void DestroyBlock(Collision2D collision) {
		Vector3 hitPosition = Vector3.zero;
		Vector3 blockPosition = Vector3.zero;

		List<Vector3> tilesHit = new List<Vector3>();
		//Tile tileHit;

		bool tileDestroyed = false;

		foreach (ContactPoint2D hit in collision.contacts) {
			hitPosition.x = hit.point.x;
			hitPosition.y = hit.point.y - 0.2f;
			var cellPos = tilemap.WorldToCell (hitPosition);

			if (tilemap.GetTile (cellPos) != null) {
				blockPosition = tilemap.CellToWorld (cellPos) + tilemap.tileAnchor;

				// Get The tile sprite for replacement
				Sprite replacementSprite = tilemap.GetSprite (cellPos);
				fallingPrefab.GetComponent<SpriteRenderer> ().sprite = replacementSprite;

				// Delete tile
				tilemap.SetTile (cellPos, null);
				tileDestroyed = true;
				tilesHit.Add (blockPosition);
			}
		}

		if (tileDestroyed) {
			foreach (var location in tilesHit) {
				Instantiate (fallingPrefab, location, Quaternion.identity);
			}
		}
	}
}
