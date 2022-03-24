using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakerEnemy : EnemyBase
{
    private TweakerDealDamage _tDealDamage;
    private TweakerAnimation _tAnimation;

    private Animation _animation;

    private float _storedMoveSpeed = 0f;

    //private bool _isMoving = false;

    private void Start()
    {
        _tDealDamage = GetComponentInChildren<TweakerDealDamage>();
        _tAnimation = GetComponent<TweakerAnimation>();
        _animation = GetComponent<Animation>();

        //IsMoving(true);
    }

    /*
    private IEnumerator Hopping()
    {
        yield return new WaitForSeconds(1f);

        var clip = _tAnimation.HopAnimation();
        _animation.AddClip(clip, clip.name);
        _animation.Play(clip.name);

        yield return new WaitForSeconds(clip.length);

        if (_isMoving)
        {
            StartCoroutine(Hopping());
        }
    }
    */

    public void IsMoving(bool move)
    {
        if (move)
        {
            //_isMoving = true;
            //StartCoroutine(Hopping());
            _enemyMoveSpeed = _storedMoveSpeed;
        }
        else
        {
            //_isMoving = false;
            _enemyMoveSpeed = 0;
        }
    }
}
