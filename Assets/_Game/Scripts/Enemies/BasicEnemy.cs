using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : EnemyBase
{
    float _storedMoveSpeed = 0f;

    public void Attack()
    {

        //yield return new WaitForSeconds(_attackRate);

        // play attack animation

        // stores all colliders hit in front of enemy
        /*
         * Collider2D[] hitObjects = Physics2D.OverlapCircleAll(_attackTrigger.transform.position, _attackRange);
         * 
        foreach (Collider2D col in hitObjects)
        {

            //scans all colliders for damagable objects
            IDamagable damagable = col.gameObject.GetComponent<IDamagable>();

            if (damagable != null && !col.GetComponent<EnemyBase>())
            {
                //if there is a tower, the enemy attacks dealing damage to the tower
                //can damage multiple towers at once 
                if (_attackAppliesForce)
                {
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, -100), ForceMode2D.Impulse);
                }

                damagable.TakeDamage(_onHitDamage);
            }
        }
        */
    }

    IEnumerator ResetMoveSpeed()
    {
        yield return new WaitForSeconds(_attackRate);
        _enemyMoveSpeed = _storedMoveSpeed;
    }

    /*
    //draws the attack zone in the scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackTrigger.transform.position, _attackRange);
    }
    */

    //begins when this enemy reaches a tower within its attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            if (collision.GetComponent<EnemyBase>() != null)
            {
                _storedMoveSpeed = _enemyMoveSpeed;
                _enemyMoveSpeed = 0;
            }

            if (collision.GetComponent<TowerBase>() != null)
            {
                TowerBase tower = collision.GetComponent<TowerBase>();

                Debug.Log(collision.gameObject.name + " is being attacked");
                _storedMoveSpeed = _enemyMoveSpeed;
                _enemyMoveSpeed = 0;
                // starts attacking

                //NOT A PERMANENT SOLUTION
                tower.StopAllCoroutines();
                tower.CancelInvoke();

                InvokeRepeating("Attack", 1f, _attackRate);
            }

            else if (collision.GetComponent<WallToDefend>() != null)
            {
                Debug.Log(collision.gameObject.name + " is being attacked");
                _storedMoveSpeed = _enemyMoveSpeed;
                _enemyMoveSpeed = 0;

                InvokeRepeating("Attack", 1f, _attackRate);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        //a rudimentary function to check if the enemy died
        //definitely can be changed

        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            CancelInvoke("Attack");
            Debug.Log(collision.gameObject.name + " is out of range of " + gameObject.name);
            //resets this enemies movement speed
            StartCoroutine(ResetMoveSpeed());

        }
    }
}
