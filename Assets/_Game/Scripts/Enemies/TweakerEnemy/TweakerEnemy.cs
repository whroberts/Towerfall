using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakerEnemy : EnemyBase
{
    private TweakerDealDamage _tDealDamage;

    private float _storedMoveSpeed = 0f;

    //private bool _isMoving = false;

    private void Start()
    {
        _tDealDamage = GetComponentInChildren<TweakerDealDamage>();
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TowerBase>() != null)
        {
            IsMoving(false);
        }
    }
}
