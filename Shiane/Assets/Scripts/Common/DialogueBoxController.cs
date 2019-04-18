using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour {

    [SerializeField]
    GameObject textGameObject;

    
    string[] text;
    string currentText;
    int textPointer = 0;
    int charPointer = 0;
    
    public void Init(string[] t)
    {
        textPointer = 0;
        charPointer = 0;
        text = t;
        currentText = text[textPointer];
        StartCoroutine(UpdateText());
    }
    
    public void Next()
    {
        if (textGameObject.GetComponent<Text>().text.Length < currentText.Length)
        {
            SetCompleteText();
        }
        else if (textPointer < text.Length - 1)
        {
            textPointer++;
            currentText = text[textPointer];
            charPointer = 0;
            StartCoroutine(UpdateText());
            textGameObject.GetComponent<Text>().text = "";
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

}
