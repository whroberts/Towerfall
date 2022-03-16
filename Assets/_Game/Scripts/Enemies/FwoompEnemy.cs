using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor.Animations;

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
    Animator _animator;

    //Animation _animation;

    float _storedMoveSpeed = 0f;
    bool _jumped = false;
    bool _jumpCompleted = false;

    private void Start()
    {
        _jumpDetectionZone = GetComponentInChildren<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void JumpOnTower(GameObject tower)
    {
        Debug.Log("Called JumpOnTower");

        AnimationCurve curveX;
        AnimationCurve curveY;
        AnimationClip clip = new AnimationClip();
        AnimatorController controller = new AnimatorController();

        Sprite sprite = tower.GetComponentInChildren<SpriteRenderer>()?.sprite;
        float spriteHeight = sprite.bounds.extents.y * 2;
        float distance = Vector2.Distance(gameObject.transform.position, tower.transform.position);

        float length =
            Mathf.Sqrt
            (
            Mathf.Pow(gameObject.transform.position.x - tower.transform.position.x, 2)
            +
            Mathf.Pow(gameObject.transform.position.y - spriteHeight, 2)
            );

        Keyframe[] keysX = new Keyframe[2];
        keysX[0] = new Keyframe(0f, gameObject.transform.position.x);
        keysX[1] = new Keyframe(1f, tower.transform.position.x);
        curveX = new AnimationCurve(keysX);

        Keyframe[] keysY = new Keyframe[3];
        keysY[0] = new Keyframe(0f, gameObject.transform.position.y);
        keysY[1] = new Keyframe(0.5f, spriteHeight * 3);
        keysY[2] = new Keyframe(1f, spriteHeight);
        curveY = new AnimationCurve(keysY);

        clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
        clip.name = "Jump";
        clip.legacy = false;

        controller.AddLayer("1");
        controller.AddMotion(clip,0);

        _animator.runtimeAnimatorController = controller;

        StartCoroutine(JumpState(clip.length));

        //controller.AddClip(clip, "Temp");
        //_animation.Play("Temp");
    }

    IEnumerator JumpState(float length)
    {
        _animator.Play("Jump");
        yield return new WaitForSeconds(length);
        _jumpCompleted = true;
    }

    //draws the attack zone in the scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_attackTrigger.transform.position, _attackRange);
    }


    //begins when this enemy reaches a tower within its attack radius
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

                InvokeRepeating("Attack", 1f, _attackRate);
            }
            else if (collision.GetComponent<TowerBase>() != null && !_jumpCompleted)
            {
                _storedMoveSpeed = _enemyMoveSpeed;
                _enemyMoveSpeed = 0;

                if (!_jumped)
                {
                    JumpOnTower(collision.gameObject);
                    _jumped = true;
                }
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
