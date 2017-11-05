using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AbsorbLayer : MonoBehaviour
{

    private Rigidbody2D rb;
    private Tilemap tilemap;
    public GameObject lavaBlockPrefab;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tilemap = GetComponent<Tilemap>();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hello?!");
        if (col.gameObject.tag == "Absorber")
        {
            Debug.Log("Hit lava");
            DestroyBlock(col);
        }
    }


    void DestroyBlock(Collider2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        Vector3 blockPosition = Vector3.zero;

        List<Vector3> tilesHit = new List<Vector3>();
        //Tile tileHit;

        bool tileDestroyed = false;

        hitPosition.x = collision.transform.position.x;
        hitPosition.y = collision.transform.position.y - 0.2f;
        var cellPos = tilemap.WorldToCell(hitPosition);

        if (tilemap.GetTile(cellPos) != null)
        {
            blockPosition = tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;

            // Get The tile sprite for replacement
            Sprite replacementSprite = tilemap.GetSprite(cellPos);
            // Delete tile
            tilemap.SetTile(cellPos, null);
            tileDestroyed = true;
            tilesHit.Add(blockPosition);
        }

        if (tileDestroyed)
        {
            foreach (var location in tilesHit)
            {
                Instantiate(lavaBlockPrefab, location, Quaternion.identity);
            }
        }
    }
}
