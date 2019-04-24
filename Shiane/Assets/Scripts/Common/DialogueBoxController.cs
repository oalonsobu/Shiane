using System.Collections;
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
    PlayableDirector playableDirector;
    int playableIndex;
    
    public void Init(string[] t, PlayableDirector p, int pi)
    {
        textPointer = -1;
        currentText = "";
        text = t;
        playableDirector = p;
        playableIndex = p ? pi : -1;
        Next();
    }
    
    public void Next()
    {
        if (textGameObject.GetComponent<Text>().text.Length < currentText.Length)
        {
            SetCompleteText();
        }
        else if (textPointer < text.Length - 1)
        {
            textGameObject.GetComponent<Text>().text = "";
            textPointer++;
            currentText = text[textPointer];
            charPointer = 0;
            if (textPointer == playableIndex)
            {
                StartCoroutine(StartAnimation());
            }
            else
            {
                StartCoroutine(UpdateText());
            } 
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
        buttonGameObject.GetComponent<Button>().interactable = false;
        playableDirector.Play();
        while (true)
        {
            yield return new WaitForSecondsRealtime (0.1f);
            if (playableDirector.state != UnityEngine.Playables.PlayState.Playing)
            {
                break;
            }
        }
        buttonGameObject.GetComponent<Button>().interactable = true;
        StartCoroutine(UpdateText());
    }
}
