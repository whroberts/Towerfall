using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakerEnemy : EnemyBase
{
    private TweakerDealDamage _tDealDamage;
    private TweakerAnimation _tAnimation;

    private Animation _animation;

    private bool _isMoving = false;

    private void Start()
    {
        _tDealDamage = GetComponentInChildren<TweakerDealDamage>();
        _tAnimation = GetComponent<TweakerAnimation>();
        _animation = GetComponent<Animation>();

        IsMoving(true);
    }

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

    public void IsMoving(bool move)
    {
        if (move)
        {
            _isMoving = true;
            StartCoroutine(Hopping());
        }
        else
        {
            _isMoving = false;
        }
    }

    //begins when this enemy reaches a tower within its attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TowerBase>() != null && !_tDealDamage.Attacking)
        {
            Debug.Log("Detected: " + collision.gameObject.name);
        }
    }
}
