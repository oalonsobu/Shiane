using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePointController : MonoBehaviour
{
    [SerializeField]
    string[] text;

    bool pointReached = false;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (pointReached && text.Length != 0)
        {
            return;
        }

        pointReached = true;
        if (col.gameObject.layer == 11) //TODO: Get layer by name
        {
            GameLoopManager.instance.UpdateDialogueText(text);
        }
    }
}
