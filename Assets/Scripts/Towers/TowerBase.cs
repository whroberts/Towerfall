using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour, IDamagable
{
    public abstract void PrintOnStart();
    public abstract void Shoot();

    private void Start()
    {
        Debug.Log("Print From Base Class In Awake");
    }

    public void TakeDamage(int damage)
    {

    }
}
