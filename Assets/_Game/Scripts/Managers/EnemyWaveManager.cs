using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Enemies")] 
    [SerializeField] private GameObject _fwoompEnemy = null;
    [SerializeField] private GameObject _tweakerEnemy = null;
    [SerializeField] private GameObject _zapperEnemy = null;

    [Header("Data")] 
    [SerializeField] private float _timeBetweenEnemySpawns = 3f;
    [SerializeField] private int _currentRound = 0;
    [SerializeField] private Vector3 _spawnPosition = new Vector3();
    [SerializeField] public bool _roundActive = false;
    [SerializeField] public bool _winTrigger = false;

    [Space]
    public List<GameObject> _enemiesActive = new List<GameObject>();
    [Space]

    [Header("Round One")]
    public List<GameObject> _enemiesToSpawnR1 = new List<GameObject>();

    [Space]

    [Header("Round Two")]
    public List<GameObject> _enemiesToSpawnR2 = new List<GameObject>();

    [Space]

    [Header("Round Three")]
    public List<GameObject> _enemiesToSpawnR3 = new List<GameObject>();

    [Space]

    [Header("Round Four")]
    public List<GameObject> _enemiesToSpawnR4 = new List<GameObject>();

    [Space]

    [Header("Round Five")]
    public List<GameObject> _enemiesToSpawnR5 = new List<GameObject>();

    private GameObject[] _enemies;
    private List<GameObject> _currentEnemiesToSpawn = new List<GameObject>();

    private void Awake()
    {
        _enemies = new GameObject[3] {_fwoompEnemy, _tweakerEnemy, _zapperEnemy};
    }

    private void Start()
    {
        NextRound();
    }

    private IEnumerator StartWave()
    {
        _roundActive = true;
        for (int i = 0; i <= _currentEnemiesToSpawn.Count; i++)
        {
            yield return new WaitForSeconds(_timeBetweenEnemySpawns);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var enemyToSpawn = Instantiate(_currentEnemiesToSpawn[0]);
        enemyToSpawn.transform.position = _spawnPosition;
        enemyToSpawn.transform.rotation = Quaternion.identity;

        _enemiesToSpawnR1.RemoveAt(0);
        _enemiesActive.Add(enemyToSpawn);
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
                    break;
                case 2:
                    _currentEnemiesToSpawn = _enemiesToSpawnR2;
                    break;
                case 3:
                    _currentEnemiesToSpawn = _enemiesToSpawnR3;
                    break;
                case 4:
                    _currentEnemiesToSpawn = _enemiesToSpawnR4;
                    break;
                case 5:
                    _currentEnemiesToSpawn = _enemiesToSpawnR5;
                    break;
            }
        }
    }
}
