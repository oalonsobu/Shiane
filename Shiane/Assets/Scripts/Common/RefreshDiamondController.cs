using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshDiamondController : MonoBehaviour
{
    float respawnTime = 5f;

    SpriteRenderer spriteRenderer;
    
    AudioClip diamondClip;
    AudioHelper audioHelper;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        diamondClip    = Resources.Load<AudioClip>("Sounds/Diamond");
        audioHelper    = GetComponent<AudioHelper>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11 && spriteRenderer.enabled) //TODO: Get layer by name
        {
            spriteRenderer.enabled = false;
            col.gameObject.GetComponent<PlayerMovementController>().RefreshCooldownDash();
            StartCoroutine(RefreshDiamond());
            if (audioHelper != null)
            {
                audioHelper.PlaySound(diamondClip);
            }
        }
    }

    IEnumerator RefreshDiamond()
    {
        yield return new WaitForSeconds(respawnTime);
        spriteRenderer.enabled = true;
    }
}
