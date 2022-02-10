using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower1 : TowerBase
{
    public override void PrintOnStart()
    {
        Debug.Log("Printing From Override function in Tower1 in Start");
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        PrintOnStart();
    }
}
