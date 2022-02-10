using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBase : MonoBehaviour
{
    public abstract void PrintOnStart();

    private void Awake()
    {
        Debug.Log("Print From Base Class In Awake");
    }
}
