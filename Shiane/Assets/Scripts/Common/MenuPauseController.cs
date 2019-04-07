using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseController : MonoBehaviour {

    public void Resume() {
        GameLoopManager.instance.PauseGame();
    }

    public void Restart() {
        SceneManager.LoadScene(GameLoopManager.instance.CurrentScene.name);
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

}
