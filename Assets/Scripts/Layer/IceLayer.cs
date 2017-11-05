using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IceLayer : MonoBehaviour {

	private Rigidbody2D rb;
	private Tilemap tilemap;
	public GameObject iceReplacement;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		tilemap = GetComponent<Tilemap>();
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "FireElement")
		{
			DestroyBlock(col);
		}
	}


	void DestroyBlock(Collider2D col)
	{
		Vector3 hitPosition = Vector3.zero;
		Vector3 blockPosition = Vector3.zero;

		bool tileDestroyed = false;

		Vector3Int cellPos = tilemap.WorldToCell (col.transform.position);

		List<Vector3> positionChecks = getPositionChecks (col.transform.position);

		foreach (Vector3 pos in positionChecks) {
			if (tilemap.GetTile(tilemap.WorldToCell (pos)) != null) {
				cellPos = tilemap.WorldToCell (pos);
			}
		}

		if (tilemap.GetTile(cellPos) != null)
		{
			blockPosition = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;
			Debug.Log("Hit Ice");
			// Get The tile sprite for replacement
			Sprite replacementSprite = tilemap.GetSprite(cellPos);
			// Delete tile
			tilemap.SetTile(cellPos, null);
			tileDestroyed = true;
		}
	}

	// Collects vectors of all positions slightly above, below, and to the left and right of the given point
	List<Vector3> getPositionChecks(Vector3 pos) {
		List<Vector3> positionChecks = new List<Vector3> ();
		Vector3 upLeftPos, downLeftPos, leftPos, upRightPos, downRightPos, rightPos;
		upLeftPos = downLeftPos = leftPos = upRightPos = downRightPos = rightPos = pos;

		// Check from the left
		leftPos.x += 0.4f;
		positionChecks.Add (leftPos);

		// Check from the upper left
		upLeftPos.x += 0.4f;
		upLeftPos.y += 0.4f;
		positionChecks.Add (upLeftPos);

		// Check from the lower left
		downLeftPos.x += 0.4f;
		downLeftPos.y -= 0.4f;
		positionChecks.Add (downLeftPos);

		// Check from the right
		rightPos.x -= 0.4f;
		positionChecks.Add (rightPos);

		// Check from the upper right
		upRightPos.x -= 0.4f;
		upRightPos.y += 0.4f;
		positionChecks.Add (upRightPos);

		// Check from the lower right
		downRightPos.x -= 0.4f;
		downRightPos.y -= 0.4f;
		positionChecks.Add (downRightPos);

		return positionChecks;
	}
}
