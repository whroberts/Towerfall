﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum State {intro, paused, playing};
    public GameObject mainMenuPanel, gamePanel, pausePanel;
    State currentState;
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
        }
    }

    public void BeginPlay()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}