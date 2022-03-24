using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Enemies")] 
    [SerializeField] private GameObject _fwoompEnemy = null;
    [SerializeField] private GameObject _tweakerEnemy = null;
    [SerializeField] private GameObject _zapperEnemy = null;

    [Header("Data")]
    [SerializeField] private int _currentRound = 0;
    [SerializeField] private int _winRound = 3;
    [SerializeField] private Vector3 _spawnPosition = new Vector3();
    [SerializeField] private bool _roundActive = false;
    public bool RoundActive => _roundActive;

    [Space]
    public List<GameObject> _enemiesActive = new List<GameObject>();
    [Space]

    [Header("Round One")]
    public List<GameObject> _enemiesToSpawnR1 = new List<GameObject>();
    public float[] _timeBetweenEnemySpawns1;

    [Space]

    [Header("Round Two")]
    public List<GameObject> _enemiesToSpawnR2 = new List<GameObject>();
    public float[] _timeBetweenEnemySpawns2;

    [Space]

    [Header("Round Three")]
    public List<GameObject> _enemiesToSpawnR3 = new List<GameObject>();
    public float[] _timeBetweenEnemySpawns3;

    [Space]

    [Header("Round Four")]
    public List<GameObject> _enemiesToSpawnR4 = new List<GameObject>();
    public float[] _timeBetweenEnemySpawns4;

    [Space]

    [Header("Round Five")]
    public List<GameObject> _enemiesToSpawnR5 = new List<GameObject>();
    public float[] _timeBetweenEnemySpawns5;

    private bool _winTrigger = false;
    public bool WinTrigger => _winTrigger;

    private List<GameObject> _currentEnemiesToSpawn = new List<GameObject>();
    private float[] _timeBetweenSpawns;

    private int _enemiesDestroyed = 0;
    private int _enemySpawnCount = 0;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        EnemiesAliveCheck();

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextRound();
        }
    }

    private IEnumerator StartWave()
    {
        for (int i = 0; i < _enemySpawnCount; i++)
        {
            yield return new WaitForSeconds(_timeBetweenSpawns[i]);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemyToSpawn = Instantiate(_currentEnemiesToSpawn[0]);
        enemyToSpawn.transform.position = _spawnPosition;
        enemyToSpawn.transform.rotation = Quaternion.identity;

        _currentEnemiesToSpawn.RemoveAt(0);
        _enemiesActive.Add(enemyToSpawn);
    }

    private void EnemiesAliveCheck()
    {
        if (_currentEnemiesToSpawn.Count == 0 && _enemiesActive.Count > 0)
        {
            foreach (var enemy in _enemiesActive)
            {
                if (enemy != null)
                {
                    _enemiesDestroyed = 0;
                    break;
                }
                else _enemiesDestroyed++;
            }

            if (_enemiesDestroyed == _enemySpawnCount)
            {
                _enemiesActive.Clear();
                _roundActive = false;
                if (_currentRound == _winRound)
                {
                    _winTrigger = true;
                    _gameManager.BeginWin();
                }
                else _gameManager.WaveShow();
            }
        }
    }

    public void NextRound()
    {
        if (!_roundActive)
        {
            _currentRound++;

            switch (_currentRound)
            {
                case 1:
                    _currentEnemiesToSpawn = _enemiesToSpawnR1;
                    _timeBetweenSpawns = _timeBetweenEnemySpawns1;
                    break;
                case 2:
                    _currentEnemiesToSpawn = _enemiesToSpawnR2;
                    _timeBetweenSpawns = _timeBetweenEnemySpawns2;
                    break;
                case 3:
                    _currentEnemiesToSpawn = _enemiesToSpawnR3;
                    _timeBetweenSpawns = _timeBetweenEnemySpawns3;
                    break;
                case 4:
                    _currentEnemiesToSpawn = _enemiesToSpawnR4;
                    _timeBetweenSpawns = _timeBetweenEnemySpawns4;
                    break;
                case 5:
                    _currentEnemiesToSpawn = _enemiesToSpawnR5;
                    _timeBetweenSpawns = _timeBetweenEnemySpawns5;
                    break;
            }

            _enemySpawnCount = _currentEnemiesToSpawn.Count;
            StartCoroutine(StartWave());
            _roundActive = true;
            _gameManager.WaveHide();
        }
        else Debug.LogError("There are still enemies left alive or that have not spawned yet");
    }
}
