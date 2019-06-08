using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshDiamondController : MonoBehaviour
{
    float respawnTime = 5f;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11 && spriteRenderer.enabled) //TODO: Get layer by name
        {
            spriteRenderer.enabled = false;
            col.gameObject.GetComponent<PlayerMovementController>().RefreshCooldownDash();
            StartCoroutine(RefreshDiamond());
        }
    }

    IEnumerator RefreshDiamond()
    {
        yield return new WaitForSeconds(respawnTime);
        spriteRenderer.enabled = true;
    }
}
