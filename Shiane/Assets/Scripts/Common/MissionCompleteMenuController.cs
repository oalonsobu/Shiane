using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionCompleteMenuController : MonoBehaviour {

    [SerializeField]
    GameObject finalTitleGameObject;
    [SerializeField] 
    GameObject finalTextGameObject;

    public void NextLevel() {
        GameLoopManager.instance.PlayClickSound();
        if (SceneManager.sceneCountInBuildSettings > GameLoopManager.instance.CurrentScene.buildIndex + 1)
        {
            SceneManager.LoadScene(GameLoopManager.instance.CurrentScene.buildIndex + 1);
        } else {
            //TODO: Maybe a new scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Restart() {
        GameLoopManager.instance.PlayClickSound();
        SceneManager.LoadScene(GameLoopManager.instance.CurrentScene.buildIndex);
    }

    public void ExitToMainMenu() {
        GameLoopManager.instance.PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    //TODO
    public void setText(int killed, int total)
    {
        //TODO: Get name level
        if (GameLoopManager.instance.CurrentScene.name == "tutorial")
        {
            finalTextGameObject.GetComponent<Text>().text = "Tutorial passed";
        } else
        {
            finalTextGameObject.GetComponent<Text>().text = "Mission passed";
        }
        finalTextGameObject.GetComponent<Text>().text = killed + "/" + total + " Killed";
    }

}
