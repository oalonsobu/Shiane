using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseController : MonoBehaviour {

    public void Resume() {
        GameLoopManager.instance.PlayClickSound();
        GameLoopManager.instance.PauseGame();
    }

    public void Restart() {
        GameLoopManager.instance.PlayClickSound();
        SceneManager.LoadScene(GameLoopManager.instance.CurrentScene.name);
    }

    public void ExitToMainMenu() {
        GameLoopManager.instance.PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }

}
