using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public abstract class TowerBase : MonoBehaviour, IDamagable
{
    [Header("For Testing Purposes")]
    [SerializeField] protected bool _canShoot = true;

    [Header("Tower Physics")] 
    [SerializeField] protected int _towerGravity = 1;
    public int TowerGravity => _towerGravity;

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

    // we can change this to have detection when enemies come
    [SerializeField] protected float _timeToStartFiring = 0.1f;

    Collider2D _col;
    protected Rigidbody2D _rb;

    protected Transform _shootPosition;

    int _enemiesInRange;

    void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _shootPosition = transform.GetChild(0);

        _currentHealth = _totalHealth;
        _currentAmmo = _totalAmmo;

        //must be zero for touch placement
        _rb.gravityScale = 0;
    }

    public abstract void PrintOnStart();
    public abstract void Shoot();
    public abstract IEnumerator Reload();

    public void SetGravity(int gravity)
    {
        _rb.gravityScale = _towerGravity;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Debug.Log(this.name + "has taken fatal damage");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if an enemy walks in the detection zone
        if (collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            // adds an enemy to the list of enemies inside the zone
            _enemiesInRange++;

            // testing purposes only
            if (_canShoot)
            {

                // insures that the tower will not invoke multiple times
                if (_enemiesInRange <= 1)
                {
                    InvokeRepeating("Shoot", _timeToStartFiring, _towerFireRate);
                    
                }
            }

            // testing
            Debug.Log("Enemy entered zone: " + _enemiesInRange);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // checks if an enemy was the collision that left
        if (collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            // takes an enemy out of the list

            _enemiesInRange--;
            
            // if there are no enemies in range, attacking stops
            if (_enemiesInRange <= 0)
            {
                CancelInvoke();
                
            }

            //testing 
            Debug.Log("Enemy left zone: " + _enemiesInRange);
        }
    }

    protected void TowerFXSystem()
    {
        // play effects on hit
    }

    protected void TowerAudioEffects()
    {
        // play sound effects
    }
}
