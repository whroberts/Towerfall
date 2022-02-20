using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    [Header("Enemy Base Stats")]
    [SerializeField] protected float _enemyMoveSpeed = 1;
    [SerializeField] protected int _currentHealth = 0;
    [SerializeField] protected int _totalHealth = 100;

    Collider _col;
    Rigidbody _rb;

    void Awake()
    {
        _col = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();

        _currentHealth = _totalHealth;
    }

    void FixedUpdate()
    {
        MoveForward();
    }

    void MoveForward()
    {
        if (_rb != null)
        {
            _rb.position = new Vector3(_rb.position.x - _enemyMoveSpeed * Time.deltaTime, _rb.position.y, _rb.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        KillTrigger killTrigger = other.GetComponent<KillTrigger>();

        if (killTrigger != null)
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = -damage;

        if (_currentHealth <= 0)
        {
            Debug.Log(this.name + "has taken fatal damage");
            Destroy(gameObject);
        }
    }
}
