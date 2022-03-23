using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public abstract class ProjectileBase : MonoBehaviour
{
    //added the attack function just so I remembered
    public abstract void Attack();

    [Header("Projectile Base Stats")]

    [SerializeField] protected float _projectileSpeed = 1250;
    [SerializeField] protected int _projectileDamage = 100;

    Collider2D _col;
    protected Rigidbody2D _rb;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg), Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //checks for damagable objects
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        
        if (damagable != null)
        {

            //continues if there is an enemy
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                //damages the enemy
                enemy.TakeDamage(_projectileDamage);
            }

            // leaves room for more damagable objects
            else
            {
                // do nothing
            }
            ProjectileFXSystem();
            ProjectileAudioEffects();

            //destroys the projectile if it hits a damagable object
            Destroy(gameObject);


        }
        else
        {
            // Random rotaion after hitting object
            _rb.AddTorque(Random.Range(-100, 100));

            // destroy when hit something else maybe
            Destroy(gameObject);
        }
    }

    protected void ProjectileFXSystem()
    {
        // play effects on hit
    }

    protected void ProjectileAudioEffects()
    {
        // play sound effects
    }

}
