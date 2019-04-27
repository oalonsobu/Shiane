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
    string currentText;
    int textPointer = 0;
    int charPointer = 0;
    PlayableDirector[] playableDirector;
    int[] playableIndex;
    
    public void Init(string[] t, PlayableDirector[] p, int[] pi)
    {
        text = t;
        playableDirector = p;
        playableIndex = pi;
        textPointer = 0;
        currentText = "";
        Next();
    }
    
    public void Next()
    {
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
        textGameObject.GetComponent<Text>().text = currentText;
        charPointer = currentText.Length;
    }
   
    IEnumerator UpdateText()
    {   
        currentText = text[textPointer];
        charPointer = 0;
        textPointer++;
        textGameObject.GetComponent<Text>().text = "";
        while (true)
        {
            yield return new WaitForSecondsRealtime (0.1f);
            if (textGameObject.GetComponent<Text>().text.Length >= currentText.Length)
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
}
