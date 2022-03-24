using System;
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
    public GameObject _mainMenuPanel, _gamePanel, _pausePanel, _losePanel, _towerHealthText, _tutorialPanel, _infoPanel, _winPanel, _waveButton;
    private State _currentState = State.Intro;
    public State CurrentState => _currentState;

    private bool _isPaused = true;
    public bool IsPaused => _isPaused;

    private int _score;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private AudioSource _menuSoundSource;
    [SerializeField] private AudioClip _menuSound, _menuHighSound, _menuLowSound;
    
    private EnemyWaveManager _waveManager;

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
        _score = 20;
        _menuSoundSource = GetComponent<AudioSource>();
        _waveManager = GetComponent<EnemyWaveManager>();

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

    public void AddMoney(int added)
    {
        _score += added;
    }

    public void BeginPlay()
    {
        Time.timeScale = 1;
        _gamePanel.SetActive(true);
        _waveButton.SetActive(false);
        _mainMenuPanel.transform.DOScale(0f, 0.3f);
        _gamePanel.transform.DOMoveY(_gamePanel.transform.position.y + 800, 1f, false).From().OnComplete(Play);

        PlayRegularSound();
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
        _waveButton.SetActive(true);
    }

    public void BeginPause()
    {
        _gamePanel.transform.DOMoveY(_gamePanel.transform.position.y + 500, 0.3f, false);
        _pausePanel.SetActive(true);
        _pausePanel.transform.DOScale(0f, 0.3f).From().OnComplete(Pause);

        PlayLowSound();
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

        PlayLowSound();
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

        PlayLowSound();
    }

    public void InfoHide()
    {
        _infoPanel.SetActive(false);
        _pausePanel.SetActive(true);

        PlayLowSound();
    }

    public void WaveHide()
    {
        PlayLowSound();
        _waveButton.transform.DOMoveY(_waveButton.transform.position.y - 200, 1f, false);
    }

    public void WaveShow()
    {
        _waveButton.transform.DOMoveY(_waveButton.transform.position.y + 200, 1f, false);
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

    public void BeginWin()
    {
        _gamePanel.transform.DOMoveY(_gamePanel.transform.position.y + 500, 0.3f, false);
        _winPanel.transform.localScale = Vector3.zero;
        _winPanel.SetActive(true);
        _winPanel.transform.DOScale(1f, 0.3f).OnComplete(Win);
    }

    private void Win()
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

    public void PlayRegularSound()
    {
        _menuSoundSource.clip = _menuSound;
        _menuSoundSource.Play();
    }

    public void PlayHighSound()
    {
        _menuSoundSource.clip = _menuHighSound;
        _menuSoundSource.Play();
    }

    public void PlayLowSound()
    {
        _menuSoundSource.clip = _menuLowSound;
        _menuSoundSource.Play();
    }
}
