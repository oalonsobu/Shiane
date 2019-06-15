using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointController : MonoBehaviour
{
    
    AudioClip winClip;
    AudioHelper audioHelper;
    
    void Start()
    {
        winClip      = Resources.Load<AudioClip>("Sounds/Victory");
        audioHelper  = GetComponent<AudioHelper>();
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11) //TODO: Get layer by name
        {
            if (audioHelper != null)
            {
                audioHelper.PlaySound(winClip);
            } 
            GameLoopManager.instance.EndGame();
        }
    }
}
