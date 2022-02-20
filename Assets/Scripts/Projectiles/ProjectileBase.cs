using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public abstract class ProjectileBase : MonoBehaviour
{
    [Header("Projectile Base Stats")]

    [SerializeField] protected float _projectileSpeed = 1250;
    [SerializeField] protected int _projectileDamage = 50;

    Collider2D _col;
    protected Rigidbody2D _rb;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Random rotaion after hitting object
        _rb.AddTorque(Random.Range(-100,100));
    }

}
