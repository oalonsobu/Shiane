using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    AudioClip shieldClip;
    AudioHelper audioHelper;
    
    void Start()
    {
        shieldClip   = Resources.Load<AudioClip>("Sounds/Drum");
        audioHelper  = GetComponent<AudioHelper>();
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 13) //TODO: Get layer by name
        {
            if (audioHelper != null)
            {
                audioHelper.PlaySound(shieldClip);
            }
            Destroy(col.gameObject);
        }
    }
}
