using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwoompEnemy : EnemyBase
{
    /* Fwoomp Enemy Design
     * 
     * Enemy is a jump type enemy
     * With in a certain range, this enemy jumps on an enemy tower dealing damage to it
     * It will do increased damage over time until the tower is destroyed or the enemy is destroyed
     * Jumping on an enemy will cause the tower to be unable to fire
     * Should test with force on attacks or not
     * 
     * Stats:
     * 
     * Low Health
     * Medium Damage
     */

    BoxCollider2D _jumpDetectionZone;
    Animation _anim;

    private FwoompAnimation _fwoompAnimation;

    float _storedMoveSpeed = 0f;
    private bool _jumped = false;
    public bool Jumped => _jumped;
    private bool _jumpCompleted = false;
    public bool JumpCompleted => _jumpCompleted;

    private void Start()
    {
        _jumpDetectionZone = GetComponentInChildren<BoxCollider2D>();
        _anim = GetComponent<Animation>();
        _fwoompAnimation = GetComponent<FwoompAnimation>();
    }

    private IEnumerator JumpState(GameObject tower)
    {
        var clip = _fwoompAnimation.JumpOnTower(tower);
        _anim.AddClip(clip, clip.name);
        _anim.Play(clip.name);
        yield return new WaitForSeconds(clip.length);
        _jumpCompleted = true;
        _jumped = false;
    }

    public void IsMoving(bool move)
    {
        if (move)
        {
            _enemyMoveSpeed = _storedMoveSpeed;
            _storedMoveSpeed = 0;
        }
        else
        {
            _storedMoveSpeed = _enemyMoveSpeed;
            _enemyMoveSpeed = 0;
        }
    }

    //begins when this enemy reaches a tower within its attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TowerBase>() != null)
        {
            Debug.Log("Detected: " + collision.gameObject.name);
            if (!_jumped)
            {
                IsMoving(false);
                StartCoroutine(JumpState(collision.gameObject));

                _jumped = true;
                _jumpCompleted = false;
            }
        }
    }
}
