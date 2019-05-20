using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatformController : MonoBehaviour
{
    Tilemap tilemap;

    void Start()
    {
        tilemap         = gameObject.GetComponent<Tilemap>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 11) //TODO: Get layer by name
        {
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in col.contacts)
            {
                hitPosition.x = hit.point.x + 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y + 0.01f * hit.normal.y;
                StartCoroutine(DestroyTileMap(tilemap.WorldToCell(hitPosition)));
            }
        }
    }

    IEnumerator DestroyTileMap(Vector3Int position)
    {
        tilemap.SetTileFlags(position, TileFlags.None);
        while (true)
        {
            Color color = tilemap.GetColor(position);
            color.a -= 0.1f;
            if (color.a < 0)
            {
                tilemap.SetTile(position, null);
                break;
            }
            tilemap.SetColor(position, color);
            yield return new WaitForSeconds(0.05f);
        }
    }
}