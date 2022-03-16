using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwoompDealDamage : FwoompEnemy
{
    CircleCollider2D _attackTrigger;

    private void Start()
    {
        _attackTrigger = GetComponent<CircleCollider2D>();
    }

    public void Attack()
    {

        //yield return new WaitForSeconds(_attackRate);

        // play attack animation

        // stores all colliders hit in front of enemy
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_attackTrigger.transform.position, _attackRange);


        foreach (Collider2D col in hitObjects)
        {

            //scans all colliders for damagable objects
            IDamagable damagable = col.gameObject.GetComponent<IDamagable>();

            if (damagable != null && !col.GetComponent<EnemyBase>())
            {
                //if there is a tower, the enemy attacks dealing damage to the tower
                // does not apply a force with the attacks
                if (_attackAppliesForce)
                {
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, -100), ForceMode2D.Impulse);
                }

                damagable.TakeDamage(_onHitDamage);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Dealing Damage
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            if (collision.GetComponent<TowerBase>() != null && _jumpCompleted)
            {
                TowerBase tower = collision.GetComponent<TowerBase>();

                Debug.Log(collision.gameObject.name + " is being attacked");

                //NOT A PERMANENT SOLUTION
                tower.StopAllCoroutines();
                tower.CancelInvoke();

                InvokeRepeating("Attack", 1f, _base.AttackRate);
            }
        }
        #endregion
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IDamagable>() != null)
        {
            if (collision.gameObject.GetComponent<TowerBase>() != null && _jumped && _jumpCompleted)
            {
                Debug.Log("Killed Enemy");
            }
        }
    }
}
