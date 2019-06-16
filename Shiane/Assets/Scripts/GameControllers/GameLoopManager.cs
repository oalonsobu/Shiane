using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoopManager : MonoBehaviour {

    public static GameLoopManager instance;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject missionCompleteMenu;
    [SerializeField] GameObject dialogueBox;
    
    [SerializeField] Text timerText;
    float timer;

    [SerializeField] Text deathCounterText;
    [SerializeField] Text dashesCounterText;
    [SerializeField] Text shieldCounterText;
    [SerializeField] Text fireballCounterText;
    
    private Scene currentScene;

    bool endGameMenuEnabled = false;
    GameObject player;
    
    AudioHelper audioHelper;

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
        player = GameObject.FindWithTag("Player");

        timer = 0.0f;
        timerText.text = timer + " s";
        UpdateDeathCounter(0);
        
        audioHelper    = GetComponent<AudioHelper>();
    }

    public void PlayClickSound()
    {
        if (audioHelper != null)
        {
            audioHelper.PlayClickSound();
        }   
    }
	
	// Update is called once per frame
	void Update () {
        if (gameOverMenu.activeSelf || missionCompleteMenu.activeSelf)
        {
            //The game is over. Do not update
            return; 
        }
        
        if (Input.GetButtonDown("Pause")) {
            PauseGame();
        }

        UpdateCounter();
    }

    void ShowGameOverMenu() {
        if (endGameMenuEnabled)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            StartCoroutine(StartGameOverAnimation());
        }       
    }

    void ShowMissionCompleteMenu() {
        missionCompleteMenu.SetActive(true);
        missionCompleteMenu.GetComponent<MissionCompleteMenuController>().setText(deathCounterText.text);
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
        GameLoopManager.instance.PlayClickSound();
        ShowPauseGame();
    }

    public void GameOver()
    {
        GameLoopManager.instance.PlayClickSound();
        ShowGameOverMenu();
    }
    
    public void EndGame()
    {
        GameLoopManager.instance.PlayClickSound();
        ShowMissionCompleteMenu();
    }
    
    public void InitializeDialogueText(string[] t, string[] a, PlayableDirector[] p, int[] pi)
    {
        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
            Time.timeScale = 0;
            dialogueBox.GetComponent<DialogueBoxController>().Init(t, a, p, pi);
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

    void UpdateCounter()
    {
        timer += Time.unscaledDeltaTime;
        timerText.text = String.Format("{0:0.00}", timer) + " s";
    }


    public void UpdateDeathCounter(int deathCount)
    {
        deathCounterText.text = deathCount + " deaths";
    }
    
    public void UpdateDashesCounter(int count)
    {
        dashesCounterText.text = count + "";
    }
    
    public void UpdateShieldCounter(float time)
    {
        shieldCounterText.text = String.Format("{0:0.00}", time) + " s";
    }
    
    public void UpdateFireballCounter(float time)
    {
        fireballCounterText.text = String.Format("{0:0.00}", time) + " s";
    }
    
    IEnumerator StartGameOverAnimation()
    {
        Time.timeScale = 0f;
        GameObject g = GameObject.Find("Canvas/UID/BlackScreen");
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            if (g != null)
            {
                g.GetComponent<Image>().color = new Color(1,1,1,  g.GetComponent<Image>().color.a + 0.1f);
            }
            else
            {
                break;
            }

            if (g.GetComponent<Image>().color.a >= 1f)
            {
                break;    
            }
        }
        yield return new WaitForSecondsRealtime (1f);
        player.GetComponent<PlayerHealthController>().Respawn();
        g.GetComponent<Image>().color = new Color(1,1,1,  0);
        yield return new WaitForSecondsRealtime (.1f);
        Time.timeScale = 1f;
    }
}
