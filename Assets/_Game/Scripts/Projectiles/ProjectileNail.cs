using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNail : ProjectileBase
{
    private void Start()
    {
        // Force in direction of projectile
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(_projectileSpeed, 1f ));

        // TEMP
        Destroy(gameObject, 5f);
    }
}
