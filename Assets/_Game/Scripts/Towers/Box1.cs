using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box1 : TowerBase
{

    public override void PrintOnStart()
    {
        Debug.Log("Printing From Override function in Box1 in Start");
    }

    public override void Shoot()
    {

    }

    public override IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadTime);
    }

    private void Update()
    {

    } 

    private void Start()
    {

    }
}
