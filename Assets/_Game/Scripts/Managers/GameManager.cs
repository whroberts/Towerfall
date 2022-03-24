using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum State {Intro, Paused, Playing, End};
    public GameObject _mainMenuPanel, _gamePanel, _pausePanel, _losePanel, _towerHealthText, _tutorialPanel, _infoPanel;
    private State _currentState = State.Intro;
    public State CurrentState => _currentState;

    private bool _isPaused = true;
    public bool IsPaused => _isPaused;

    private int _score;
    [SerializeField] private TextMeshProUGUI _scoreText;

    void Start()
    {
        _currentState = State.Intro;
        _mainMenuPanel.SetActive(true);
        _gamePanel.SetActive(false);
        _pausePanel.SetActive(false);
        _losePanel.SetActive(false);
        _towerHealthText.SetActive(false);
        _tutorialPanel.SetActive(false);
        _infoPanel.SetActive(false);
        _score = 5;

        //Begin frozen
        Time.timeScale = 0;

        //Lock to landscape
        Screen.orientation = ScreenOrientation.Landscape;

        DOTween.Init();
    }

    void Update()
    {
        //Update inventory UI
        _scoreText.text = "$" + _score;
        
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
                    BeginPause();
                else if (Input.GetKeyDown(KeyCode.R))
                    ReloadScene();

                if (FindObjectOfType<WallToDefend>().IsDefeated)
                    BeginEnd();

                break;

            case State.Paused:
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                    BeginUnpause();
                break;

            case State.End:
                if (Input.GetKeyDown(KeyCode.R))
                    ReloadScene();
                break;
        }
    }

    public bool PayForTower(int price)
    {
        if (price > _score) return false;
        else
        {
            _score -= price;
            return true;
        }
    }

    public void BeginPlay()
    {
        Time.timeScale = 1;
        _gamePanel.SetActive(true);

        _mainMenuPanel.transform.DOScale(0f, 0.3f);
        _gamePanel.transform.DOMoveY(_gamePanel.transform.position.y + 500, 1f, false).From().OnComplete(Play);
        
    }

    private void Play()
    {
        _isPaused = false;
        _towerHealthText.SetActive(true);
        _tutorialPanel.SetActive(true);
        _currentState = State.Playing;
        _tutorialPanel.transform.DOScale(0f, 0.5f).From().OnComplete(TutorialPulse);
    }

    private void TutorialPulse()
    {
        _tutorialPanel.transform.DOScale(0.9f, 0.5f).SetLoops(10, LoopType.Yoyo).OnComplete(TutorialFade);
    }

    private void TutorialFade()
    {
        _tutorialPanel.transform.DOScale(0f, 0.5f);
    }

    public void BeginPause()
    {
        _gamePanel.transform.DOMoveY(_gamePanel.transform.position.y + 500, 0.3f, false);
        _pausePanel.SetActive(true);
        _pausePanel.transform.DOScale(0f, 0.3f).From().OnComplete(Pause);
    }

    private void Pause()
    {
        Time.timeScale = 0;
        _gamePanel.SetActive(false);
        _isPaused = true;
        _currentState = State.Paused;
    }

    public void BeginUnpause()
    {
        Time.timeScale = 1;
        _gamePanel.SetActive(true);
        _gamePanel.transform.DOMoveY(_gamePanel.transform.position.y - 500, 0.3f, false);
        _pausePanel.transform.DOScale(0f, 0.3f).OnComplete(UnPause);
    }

    private void UnPause()
    {
        _pausePanel.SetActive(false);
        _isPaused = false;
        _currentState = State.Playing;
        _pausePanel.transform.localScale = Vector3.one;
    }

    public void InfoShow()
    {
        _pausePanel.SetActive(false);
        _infoPanel.SetActive(true);
    }

    public void InfoHide()
    {
        _infoPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    public void BeginEnd()
    {
        _gamePanel.transform.DOMoveY(_gamePanel.transform.position.y + 500, 0.3f, false);
        _losePanel.transform.localScale = Vector3.zero;
        _losePanel.SetActive(true);
        _losePanel.transform.DOScale(1f, 0.3f).OnComplete(EndPlay);
    }

    private void EndPlay()
    {
        Time.timeScale = 0;
        _isPaused = true;
        _gamePanel.SetActive(false);
        _currentState = State.End;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
