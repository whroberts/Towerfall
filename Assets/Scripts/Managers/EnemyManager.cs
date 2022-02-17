using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Round Information")]
    [SerializeField] int _currentRound = 1;
    [SerializeField] int _maxRounds = 5;

    [Header("Enemy Prefabs")]
    [SerializeField] GameObject _enemyBasic = null;

    List<GameObject> _enemiesThisRound = new List<GameObject>();

    private void Start()
    {
        SetEnemiesToSpawn();
        StartCoroutine(SpawnEnemies());
    }
    void SetEnemiesToSpawn()
    {
        for (int i = 0; i < _currentRound; i++)
        {
            _enemiesThisRound.Add(_enemyBasic);
        }

        Debug.Log(_enemiesThisRound.Count);
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < _enemiesThisRound.Count; i++)
        {
            GameObject enemy = Instantiate(_enemyBasic, gameObject.transform, true);
            enemy.transform.position = new Vector3(30, 0, 0);
            enemy.transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(2f);
        }
    }

    void CheckForCurrentEnemies()
    {
        Collider[] currentEnemies = GetComponentsInChildren<Collider>();
    }
}
