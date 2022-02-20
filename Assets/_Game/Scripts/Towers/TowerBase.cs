using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public abstract class TowerBase : MonoBehaviour, IDamagable
{
    [Header("Tower Base Stats")]

    // Shoot every X seconds
    [SerializeField] protected float _towerFireRate = 0.75f;

    // Shooting Recoil
    [SerializeField] protected float _towerRotRecoil = 0; // Positive is to the left
    [SerializeField] protected Vector2 _towerLinRecoil = new Vector2 (0,0);

    // Automatically reloads
    [SerializeField] protected float _currentAmmo = 0;
    [SerializeField] protected float _totalAmmo = 10;

    // In seconds
    [SerializeField] protected float _reloadTime = 1;

    [SerializeField] protected int _currentHealth = 0;
    [SerializeField] protected int _totalHealth = 25;

    Collider2D _col;
    protected Rigidbody2D _rb;

    protected Transform _shootPosition;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _shootPosition = transform.GetChild(0);

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
