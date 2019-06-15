using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    [SerializeField] GameObject creditsMenu;

    AudioHelper audioHelper;
        
    void Start() {
        audioHelper    = GetComponent<AudioHelper>();
        gameObject.SetActive(true);
    }

    public void NewGame() {
        audioHelper.PlayClickSound();
        SceneManager.LoadScene("Tutorial");
    }

    public void ShowCredits() {
        audioHelper.PlayClickSound();
        creditsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ExitGame() {
        audioHelper.PlayClickSound();
        Application.Quit();
    }

}
