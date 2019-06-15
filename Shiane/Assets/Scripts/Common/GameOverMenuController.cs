using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour {

    public void Restart() {
        GameLoopManager.instance.PlayClickSound();
        SceneManager.LoadScene(GameLoopManager.instance.CurrentScene.buildIndex);
    }

    public void ExitToMainMenu() {
        GameLoopManager.instance.PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }

}
