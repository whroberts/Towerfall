using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwoompDealDamage : MonoBehaviour
{
    CircleCollider2D _attackTrigger;
    FwoompEnemy _base;

    private bool _attacking = false;
    public bool Attacking => _attacking;

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
                if (_base.AppliesForce && col.GetComponent<Rigidbody2D>() != null)
                {
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, -100), ForceMode2D.Impulse);
                }

                damagable.TakeDamage(_base.OnHitDamage);
            }
        }
    }

    private void DealDamage(TowerBase tower = null)
    {
        tower?.StopAllCoroutines();
        tower?.CancelInvoke();

        InvokeRepeating("Attack", _base.AttackRate, _base.AttackRate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TowerBase>() != null && _base.JumpCompleted)
        {
            _attacking = true;
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
        if (collision.gameObject.GetComponent<TowerBase>() != null)
        {
            if (_attacking)
            {
                AudioHelper.PlayClip2D(_base._boom, 0.2f);
                Destroy(this.gameObject,0.5f);
                //_attacking = false;
                //CancelInvoke();
                //_base.IsMoving(true);
                //StartCoroutine(WaitForTrigger());
            }
        }
    }

    private IEnumerator WaitForTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        _base._jumpDetectionZone.gameObject.SetActive(true);
        _base._jumped = false;
    }
}
