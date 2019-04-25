using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialoguePointController : MonoBehaviour
{
    [SerializeField]
    string[] text;

    [SerializeField]
    PlayableDirector[] cutSceneGO;
    [SerializeField][Tooltip("When the cutScene will play")]
    int[] playableIndex; 

    bool pointReached = false;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        //TODO: check integrity between cutSceneGO and playableIndex
        //TODO: Make the player to be in the floor before playing the dialogue
        if (pointReached || text.Length == 0)
        {
            return;
        }

        pointReached = true;
        if (col.gameObject.layer == 11) //TODO: Get layer by name
        {
            GameLoopManager.instance.InitializeDialogueText(text, cutSceneGO, playableIndex);
        }
    }
}