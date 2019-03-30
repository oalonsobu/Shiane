using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    [SerializeField] GameObject creditsMenu;

    void Start() {
        gameObject.SetActive(true);
    }

    public void NewGame() {
        SceneManager.LoadScene("Tutorial");
    }

    public void ShowCredits() {
        creditsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ExitGame() {
        Application.Quit();
    }

}
