using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Round Information")]
    [SerializeField] int _currentRound = 1;
    [SerializeField] int _maxRounds = 5;

    [Header("Enemy Prefabs")]
    [SerializeField] GameObject _enemy1 = null;

    List<GameObject> _enemiesThisRound = new List<GameObject>();


    //Does not spawn enemies randomly, only spawns when the player presses S
    private void Start()
    {
        //StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        //CheckForCurrentEnemies();

        if (Input.GetKeyDown(KeyCode.S) && FindObjectOfType<GameManager>()?.CurrentState == GameManager.State.Playing)
        {
            GameObject enemy = Instantiate(_enemy1, gameObject.transform.position, Quaternion.identity);
            enemy.transform.position = new Vector3(30, 0, 0);
            enemy.transform.rotation = Quaternion.identity;
        }
    }

    void SetEnemiesToSpawn()
    {
        for (int i = 0; i < _currentRound; i++)
        {
            _enemiesThisRound.Add(_enemy1);
        }
    }

    IEnumerator SpawnEnemies()
    {
        SetEnemiesToSpawn();

        for (int i = 0; i < _enemiesThisRound.Count; i++)
        {
            GameObject enemy = Instantiate(_enemy1, gameObject.transform, true);
            enemy.transform.position = new Vector3(30, 0, 0);
            enemy.transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(2);
        }
    }

    void CheckForCurrentEnemies()
    {
        EnemyBase[] currentEnemies = GetComponentsInChildren<EnemyBase>();

        if (currentEnemies.Length == 0)
        {
            if (_currentRound < _maxRounds)
            {
                _currentRound++;
                StartCoroutine(SpawnEnemies());
            }
            else if (_currentRound >= _maxRounds)
            {
                Debug.Log("YOU WIN");
            }
        }
       
    }
}
