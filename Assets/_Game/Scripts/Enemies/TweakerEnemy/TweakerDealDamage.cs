using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakerDealDamage : MonoBehaviour
{
    CircleCollider2D _attackTrigger;
    TweakerEnemy _base;

    private bool _attacking = false;
    public bool Attacking => _attacking;

    private void Start()
    {
        _attackTrigger = GetComponent<CircleCollider2D>();
        _base = GetComponentInParent<TweakerEnemy>();
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
                if (_base.AppliesForce && col.GetComponent<Rigidbody2D>() != null)
                {
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 400), ForceMode2D.Impulse);
                }

                AudioHelper.PlayClip2D(_base._attackSound, 1);
                damagable.TakeDamage(_base.OnHitDamage);
            }
        }
    }

    private void DealDamage(TowerBase tower = null)
    {
        InvokeRepeating("Attack", 3f, _base.AttackRate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TowerBase>() != null)
        {
            _attacking = true;
            _base.IsMoving(false);
            Debug.Log("Here");
            DealDamage(collision?.GetComponent<TowerBase>());
        }
        else if (collision.GetComponent<WallToDefend>() != null)
        {
            _attacking = true;
            DealDamage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<TowerBase>() != null && _attacking)
        {
            _attacking = false;
            CancelInvoke();
            _base.IsMoving(true);
        }
    }
}
