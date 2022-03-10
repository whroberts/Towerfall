using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State {Intro, Paused, Playing, End};
    public GameObject _mainMenuPanel, _gamePanel, _pausePanel, _losePanel;
    private State _currentState = State.Intro;
    public State CurrentState => _currentState;

    private bool _isPaused = true;
    public bool IsPaused => _isPaused;
    void Start()
    {
        _currentState = State.Intro;
        _mainMenuPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _losePanel.SetActive(false);

        //Begin frozen
        Time.timeScale = 0;

        //Lock to landscape
        Screen.orientation = ScreenOrientation.Landscape;
    }

    void Update()
    {
        
        //Handling the game states
        switch (_currentState)
        {
            //Press F to start the game, or tap on the intro menu
            case State.Intro:
                if (Input.GetKeyDown(KeyCode.F) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
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
        _mainMenuPanel.SetActive(false);
        _gamePanel.SetActive(true);

        _currentState = State.Playing;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _isPaused = true;
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(true);

        _currentState = State.Paused;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        _isPaused = false;
        _pausePanel.SetActive(false);
        _gamePanel.SetActive(true);

        _currentState = State.Playing;
    }

    public void EndPlay()
    {
        Time.timeScale = 0;
        _isPaused = true;
        _gamePanel.SetActive(false);
        _losePanel.SetActive(true);

        _currentState = State.End;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
