using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
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
    [SerializeField] protected float _attackRange = 5;
    [SerializeField] protected int _attackRate = 1;
    [SerializeField] protected CircleCollider2D _enemyCheck = null;

    Collider2D _col;
    Rigidbody2D _rb;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _totalHealth;
        _enemyCheck.radius = _attackRange;
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
