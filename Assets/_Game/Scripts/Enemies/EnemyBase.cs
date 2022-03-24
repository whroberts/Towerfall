using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    //public abstract void Attack();

    [Header("Could Be Fun")]
    [SerializeField] protected bool _attackAppliesForce = true;

    [Header("For Later")]
    [SerializeField] protected int _moneyOnDeath = 100;

    [Header("Enemy Base Stats")]
    [SerializeField] protected float _enemyMoveSpeed = 1;
    [SerializeField] protected int _currentHealth = 0;
    [SerializeField] protected int _totalHealth = 100;

    [Header("Enemy Attack Stats")]
    [SerializeField] protected int _onHitDamage = 12;
    [SerializeField] protected float _attackRange = 1;
    [SerializeField] protected float _attackRate = 1;

    Collider2D _col;
    protected Rigidbody2D _rb;

    public int CurrentHealth => _currentHealth;
    public int TotalHealth => _totalHealth;
    public int OnHitDamage => _onHitDamage;
    public float AttackRange => _attackRange;
    public float AttackRate => _attackRate;
    public bool AppliesForce => _attackAppliesForce;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _totalHealth;
    }

    void FixedUpdate()
    {
        MoveForward();
    }

    void MoveForward()
    {
        //moves the enemy forward
        if (_rb != null)
        {
            _rb.position = new Vector2(_rb.position.x - _enemyMoveSpeed * Time.deltaTime, _rb.position.y);
        }

        //HORRIBLE SOLUTION
        FallenOver();
    }

    //take damage function from interface IDamagable
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Debug.Log(this.name + "has taken fatal damage");
            Destroy(gameObject);
        }
    }

    //TERRIBLE SHOULD NOT BE PERMANENT
    public void FallenOver()
    {
        if (this.transform.rotation.z <= -0.5f || this.transform.rotation.z >= 0.5f)
        {
            Destroy(gameObject);
        }
    }
}
