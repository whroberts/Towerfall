using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor.Animations;

public class FwoompEnemyDepricated : EnemyBase
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
    private bool _jumped = false;
    public bool Jumped => _jumped;
    private bool _jumpCompleted = false;
    public bool JumpCompleted => _jumpCompleted;

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

        AnimatorState jump = new AnimatorState();
        AnimatorState idle = new AnimatorState();
        idle.name = "Idle";
        jump.motion = clip;
        jump.name = "Jump";


        idle.AddTransition(jump);
        idle.transitions[0].hasExitTime = true;
        idle.transitions[0].exitTime = 0f;
        idle.transitions[0].duration = 0f;

        jump.AddTransition(idle);
        jump.transitions[0].hasExitTime = true;
        jump.transitions[0].exitTime = 0f;
        jump.transitions[0].duration = 0f;

        controller.layers[0].stateMachine.AddState(idle, new Vector3(100, 100));
        controller.layers[0].stateMachine.defaultState = idle;
        controller.layers[0].stateMachine.AddState(jump,new Vector3(200,100));
        //controller.layers[0].stateMachine.AddState(idle, new Vector3(100, 100));
        //controller.AddMotion(clip,0);

        _animator.runtimeAnimatorController = controller;

        StartCoroutine(JumpState(clip.length));
    }

    IEnumerator JumpState(float length)
    {
        Debug.Log("JumpState called");
        _animator.Play("Jump");
        yield return new WaitForSeconds(length);
        _rb.position = transform.position;
        _animator.StopPlayback();
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
            Debug.Log(_storedMoveSpeed);
        }
    }

    //begins when this enemy reaches a tower within its attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TowerBase>() != null && !_jumpCompleted)
        {
            if (!_jumped)
            {
                IsMoving(false);
                JumpOnTower(collision.gameObject);

                _jumped = true;
                _jumpCompleted = false;
            }
        }
    }
}
