using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNail : ProjectileBase
{
    private void Start()
    {
        Attack();
    }

    public override void Attack()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(_projectileSpeed, 1f));

        // TEMP
        Destroy(gameObject, 5f);
    }
}
