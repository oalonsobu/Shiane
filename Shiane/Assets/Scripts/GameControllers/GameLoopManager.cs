using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameLoopManager : MonoBehaviour {

    public static GameLoopManager instance;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject missionCompleteMenu;
    [SerializeField] GameObject dialogueBox;

    private Scene currentScene;

    public Scene CurrentScene
    {
        get => currentScene;
        set => currentScene = value;
    }

    void Awake() {
        instance = this;
        currentScene = SceneManager.GetActiveScene();
    }

    void Start () {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        missionCompleteMenu.SetActive(false);
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameOverMenu.activeSelf || missionCompleteMenu.activeSelf)
        {
            //The game is over. Do not update
            return; 
        }
        
        if (Input.GetKeyDown(KeyCode.P)) {
            PauseGame();
        }
        
    }

    void ShowGameOverMenu() {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void ShowMissionCompleteMenu() {
        missionCompleteMenu.SetActive(true);
        missionCompleteMenu.GetComponent<MissionCompleteMenuController>().setText(0,1);
        Time.timeScale = 0;
    }

    void ShowPauseGame() {
        if (pauseMenu.activeSelf) {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        } else {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
        
    public void PauseGame() {
        ShowPauseGame();
    }

    public void GameOver()
    {
        ShowGameOverMenu();
    }
    
    public void EndGame()
    {
        ShowMissionCompleteMenu();
    }
    
    public void InitializeDialogueText(string[] t, PlayableDirector[] p, int[] pi)
    {
        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
            Time.timeScale = 0;
            dialogueBox.GetComponent<DialogueBoxController>().Init(t, p, pi);
        }
    }
    
    public void DisableDialogueText()
    {
        if (dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
            Time.timeScale = 1;;
        }
    }
}
