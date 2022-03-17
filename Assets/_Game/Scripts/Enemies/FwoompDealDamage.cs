using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwoompDealDamage : MonoBehaviour
{
    CircleCollider2D _attackTrigger;
    FwoompEnemy _base;

    private bool _onEnemy = false;

    private void Start()
    {
        _attackTrigger = GetComponent<CircleCollider2D>();
        _base = GetComponentInParent<FwoompEnemy>();
        _attackTrigger.radius = _base.AttackRange;
    }

    public void Attack()
    {

        //yield return new WaitForSeconds(_attackRate);

        // play attack animation

        // stores all colliders hit in front of enemy
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_attackTrigger.transform.position, _base.AttackRange);


        foreach (Collider2D col in hitObjects)
        {

            //scans all colliders for damagable objects
            IDamagable damagable = col.gameObject.GetComponent<IDamagable>();

            if (damagable != null && !col.GetComponent<EnemyBase>())
            {
                //if there is a tower, the enemy attacks dealing damage to the tower
                // does not apply a force with the attacks
                if (_base.AppliesForce)
                {
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, -100), ForceMode2D.Impulse);
                }

                damagable.TakeDamage(_base.OnHitDamage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Dealing Damage
        if (collision.GetComponent<TowerBase>() != null && _base.Jumped)
        {
            _onEnemy = true;
        }
        #endregion
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<TowerBase>() != null)
        {
            if (_base.JumpCompleted && _onEnemy)
            {
                TowerBase tower = collision.GetComponent<TowerBase>();

                Debug.Log(collision.gameObject.name + " is being attacked");

                tower.StopAllCoroutines();
                tower.CancelInvoke();

                InvokeRepeating("Attack", 1f, _base.AttackRate);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TowerBase>() != null)
        {
            if (_onEnemy)
            {
                Debug.Log("Killed Enemy");
                _onEnemy = false;
                _base.IsMoving(true);
            }
        }
    }
}
