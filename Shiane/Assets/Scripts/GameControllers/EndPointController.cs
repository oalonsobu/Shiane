using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11) //TODO: Get layer by name
        {
            GameLoopManager.instance.EndGame();
        }
    }
}
