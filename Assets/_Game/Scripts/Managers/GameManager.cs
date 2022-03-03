using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State {Intro, Paused, Playing, End};
    public GameObject mainMenuPanel, gamePanel, pausePanel, losePanel;
    private State currentState = State.Intro;
    public State CurrentState => currentState;


    private bool _isPaused = true;
    public bool IsPaused => _isPaused;
    void Start()
    {
        currentState = State.Intro;
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
        losePanel.SetActive(false);

        //Begin frozen
        Time.timeScale = 0;
    }

    void Update()
    {
        //Handling the game states
        switch (currentState)
        {
            //Press F to start the game
            case State.Intro:
                if (Input.GetKeyDown(KeyCode.F))
                    BeginPlay();
                break;

            case State.Playing:
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                    Pause();
                else if (Input.GetKeyDown(KeyCode.R))
                    ReloadScene();

                    if (FindObjectOfType<WallToDefend>().IsDefeated)
                    EndPlay();
                break;

            case State.Paused:
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                    UnPause();
                break;

            case State.End:
                if (Input.GetKeyDown(KeyCode.R))
                    ReloadScene();
                break;
        }
    }

    public void BeginPlay()
    {
        Time.timeScale = 1;
        _isPaused = false;
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);

        currentState = State.Playing;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _isPaused = true;
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);

        currentState = State.Paused;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        _isPaused = false;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);

        currentState = State.Playing;
    }

    public void EndPlay()
    {
        Time.timeScale = 0;
        _isPaused = true;
        gamePanel.SetActive(false);
        losePanel.SetActive(true);

        currentState = State.End;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
