using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {

    [SerializeField] GameObject mainMenu;

    AudioHelper audioHelper;
    void Start() {
        audioHelper = GetComponent<AudioHelper>();
        gameObject.SetActive(false);
    }

    public void GoBack() {
        audioHelper.PlayClickSound();
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

}
