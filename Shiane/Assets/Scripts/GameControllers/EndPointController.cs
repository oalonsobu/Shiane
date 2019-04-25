using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        //TODO: check integrity between cutSceneGO and playableIndex
        //TODO: Make the player to be in the floor before playing the dialogue
        if (col.gameObject.layer == 11) //TODO: Get layer by name
        {
            GameLoopManager.instance.EndGame();
        }
    }
}
