using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    float _storedMoveSpeed = 0f;

    public void Attack()
    {

        //yield return new WaitForSeconds(_attackRate);

        // play attack animation

        // stores all colliders hit in front of enemy
        Collider2D[] hitObjects  = Physics2D.OverlapCircleAll(_enemyCheck.transform.position, _attackRange);


        foreach (Collider2D col in hitObjects)
        {

            //scans all colliders for damagable objects
            IDamagable damagable = col.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                //if there is a tower, the enemy attacks dealing damage to the tower
                //can damage multiple towers at once 
                TowerBase tower = col.gameObject.GetComponent<TowerBase>();
                if (tower != null)
                {
                    if (_attackAppliesForce)
                    {
                        tower.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, -100), ForceMode2D.Impulse);
                    }

                    tower.TakeDamage(_onHitDamage);
                }

                //leaves opprotunity for other damagable objects
                else
                {
                    // do nothing yet
                }
            }
            else
            {
                // do nothing yet 
            }
        }

        //yield return new WaitForEndOfFrame();
    }

    IEnumerator ResetMoveSpeed()
    {
        yield return new WaitForSeconds(_attackRate);
        _enemyMoveSpeed = _storedMoveSpeed;
    }

    //draws the attack zone in the scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_enemyCheck.transform.position, _attackRange);
    }


    //begins when this enemy reaches a tower within its attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            TowerBase tower = collision.GetComponent<TowerBase>();

            if (tower != null)
            {
                Debug.Log(tower.name + " is in of range of " + gameObject.name);
                // starts attacking

                _storedMoveSpeed = _enemyMoveSpeed;
                _enemyMoveSpeed = 0;

                //NOT A PERMANENT SOLUTION
                tower.StopAllCoroutines();
                tower.CancelInvoke();

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
            TowerBase tower = collision.GetComponent<TowerBase>();

            if (tower != null)
            {
                CancelInvoke("Attack");
                Debug.Log(tower.name + " is out of range of " + gameObject.name);
                //resets this enemies movement speed
                StartCoroutine(ResetMoveSpeed());

            }
        }
    }
}
