using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapperEnemy : EnemyBase
{
    private ZapperDealDamage _zDealDamage;

    private float _storedMoveSpeed = 0f;
    private float _speedBoost = 2f;

    private void Start()
    {
        _zDealDamage = GetComponentInChildren<ZapperDealDamage>();
        _storedMoveSpeed = _enemyMoveSpeed;
    }

    public void IsMoving(bool move)
    {
        if (move)
        {
            _enemyMoveSpeed = _storedMoveSpeed;
        }
        else
        {
            _enemyMoveSpeed = 0;
        }
    }

    //begins when this enemy reaches a tower within its attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.GetComponent<WallToDefend>() != null || collision.GetComponent<TowerBase>() != null) && !_zDealDamage.Attacking)
        {
            Debug.Log("Detected: " + collision.gameObject.name);

            _enemyMoveSpeed *= _speedBoost;
        }
    }
}
