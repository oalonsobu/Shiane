using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {

    [SerializeField] GameObject mainMenu;

    void Start() {
        gameObject.SetActive(false);
    }

    public void GoBack() {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

}
