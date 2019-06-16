using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialoguePointController : MonoBehaviour
{
    [SerializeField]
    string[] text;
    [SerializeField][Tooltip("Who says the line above")]
    string[] actorText;
    
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

        if (col.gameObject.layer == 11) //TODO: Get layer by name
        {
            pointReached = true;
            GameLoopManager.instance.InitializeDialogueText(text, actorText, cutSceneGO, playableIndex);
        }
    }
}