using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class TowerBase : MonoBehaviour, IDamagable
{
    [Header("Tower Base Stats")]

    // Shots per second
    [SerializeField] protected float _towerFireRate = 2;

    // Automatically reloads
    [SerializeField] protected float _currentAmmo = 0;
    [SerializeField] protected float _totalAmmo = 10;

    // In seconds
    [SerializeField] protected float _reloadTime = 1;

    // Multiple of normal gravity
    [SerializeField] protected float _towerGravity = 2;

    [SerializeField] protected int _currentHealth = 0;
    [SerializeField] protected int _totalHealth = 25;

    Collider2D _col;
    Rigidbody2D _rb;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _totalHealth;
        _currentAmmo = _totalAmmo;
    }

    public abstract void PrintOnStart();
    public abstract void Shoot();

    private void Start()
    {
        Debug.Log("Print From Base Class In Awake");
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
