using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour {

    [SerializeField]
    GameObject textGameObject;
    [SerializeField]
    GameObject buttonGameObject;
    
    string[] text;
    string[] actor;
    string currentText;
    string currentActor;
    int textPointer = 0;
    int charPointer = 0;
    PlayableDirector[] playableDirector;
    int[] playableIndex;
    
    public void Init(string[] t, string[] a, PlayableDirector[] p, int[] pi)
    {
        text = t;
        actor = a;
        playableDirector = p;
        playableIndex = pi;
        textPointer = 0;
        currentText = "";
        Next();
    }
    
    public void Next()
    {
        GameLoopManager.instance.PlayClickSound();
        if (textGameObject.GetComponent<Text>().text.Length < currentText.Length)
        {
            SetCompleteText();
        }
        else if (playableIndex.Length > 0 && playableIndex.Contains(textPointer))
        {
            StartCoroutine(StartAnimation());
        }
        else if (textPointer < text.Length)
        {
            StartCoroutine(UpdateText());
        }
        else
        {
            GameLoopManager.instance.DisableDialogueText();
        }
    }
    
    void SetCompleteText()
    {
        textGameObject.GetComponent<Text>().text = currentActor + currentText;
        charPointer = currentText.Length;
    }
   
    IEnumerator UpdateText()
    {   
        currentText  = text[textPointer];
        currentActor = GetCurrentActor();
        charPointer  = 0;
        textPointer++;
        textGameObject.GetComponent<Text>().text = currentActor;
        while (true)
        {
            yield return new WaitForSecondsRealtime (0.1f);
            if (textGameObject.GetComponent<Text>().text.Length - currentActor.Length >= currentText.Length)
            {
                break;
            }
            textGameObject.GetComponent<Text>().text += currentText[charPointer++]; 
        }
    }
    
    
    IEnumerator StartAnimation()
    {
        int currentIndex = Array.IndexOf(playableIndex, textPointer);
        buttonGameObject.GetComponent<Button>().interactable = false;
        playableDirector[currentIndex].Play();
        while (true)
        {
            yield return new WaitForSecondsRealtime (0.1f);
            if (playableDirector[currentIndex].state != UnityEngine.Playables.PlayState.Playing)
            {
                break;
            }
        }
        playableIndex[currentIndex] = -1;
        buttonGameObject.GetComponent<Button>().interactable = true;
        Next();
    }

    string GetCurrentActor()
    {
        int aux = 0;
        if (actor.Length - 1 < textPointer)
        {
            aux = actor.Length - 1;
        }
        else
        {
            aux = textPointer;
        }
        return actor[aux] + ":\n";
    }
}
