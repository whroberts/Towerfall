using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum State {intro, paused, playing, end};
    public GameObject mainMenuPanel, gamePanel, pausePanel;
    State currentState;

    private bool _isPaused = true;
    public bool IsPaused => _isPaused;
    void Start()
    {
        currentState = State.intro;
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        //Handling the game states
        switch (currentState)
        {
            //Press F to start the game
            case State.intro:
                if (Input.GetKeyDown(KeyCode.F))
                    BeginPlay();
                break;

            case State.playing:

                break;

            case State.paused:

                break;

            case State.end:
                if (FindObjectOfType<WallToDefend>().IsDefeated) 
                    EndPlay();
                break;
        }
    }

    public void BeginPlay()
    {
        _isPaused = false;
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _isPaused = true;
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        _isPaused = false;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void EndPlay()
    {
        Time.timeScale = 0;
        _isPaused = true;
        gamePanel.SetActive(false);

        //placeholder for end game panel
        mainMenuPanel.SetActive(true);
    }
}
