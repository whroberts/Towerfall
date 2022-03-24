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

    [SerializeField] public BoxCollider2D _jumpDetectionZone = null;
    [SerializeField] public CircleCollider2D _attackTrigger = null;
    [SerializeField] public AudioClip _boom = null;
    Animation _anim;

    private FwoompAnimation _fwoompAnimation;

    float _storedMoveSpeed = 0f;
    public bool _jumped = false;
    private bool _jumpCompleted = false;
    public bool JumpCompleted => _jumpCompleted;

    private void Start()
    {
        _anim = GetComponent<Animation>();
        _fwoompAnimation = GetComponent<FwoompAnimation>();
    }

    private IEnumerator JumpState(GameObject tower, int multiplier = 2)
    {
        var clip = _fwoompAnimation.JumpOnTower(tower, multiplier);
        _anim.AddClip(clip, clip.name);
        _anim.Play(clip.name);
        AudioHelper.PlayClip2D(_attackSound, 0.3f);
        yield return new WaitForSeconds(clip.length);
        _jumpCompleted = true;
        _jumped = false;
    }

    private IEnumerator LerpState(GameObject tower, int multiplier = 2)
    {
        _attackTrigger.enabled = false;
        _fwoompAnimation.SetLerp(tower, Time.time, multiplier);
        AudioHelper.PlayClip2D(_attackSound, 0.3f);
        yield return new WaitForSeconds(1.25f);
        _jumpCompleted = true;
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
            if (!_jumped)
            {
                IsMoving(false);
                StartCoroutine(LerpState(collision.gameObject));

                _jumped = true;
                _jumpCompleted = false;
                _jumpDetectionZone.gameObject.SetActive(false);
            }
        }
        else if (collision.GetComponent<WallToDefend>() != null)
        {
            if (!_jumped)
            {
                IsMoving(false);
                StartCoroutine(LerpState(collision.gameObject, 4));

                _jumped = true;
                _jumpCompleted = false;
                _jumpDetectionZone.gameObject.SetActive(false);
            }
        }
    }
}
