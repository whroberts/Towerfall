using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower1 : TowerBase
{
    [SerializeField] private GameObject _projectileRef = null;

    public override void PrintOnStart()
    {
        Debug.Log("Printing From Override function in Tower1 in Start");
    }

    public override void Shoot()
    {
        if (_currentAmmo > 0)
        {
            //throw new System.NotImplementedException();

            Instantiate(_projectileRef, _shootPosition.position, _shootPosition.rotation);

            //Rotational recoil from shooting
            _rb.AddTorque(_towerRotRecoil);
            //Linear recoil from shooting
            _rb.AddForce(_towerLinRecoil);

            _currentAmmo -= 1;

            // TEMP
            transform.GetChild(1).GetComponent<TextMesh>().text = ("Ammo: " + _currentAmmo + " / " + _totalAmmo);
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Start()
    {
        //PrintOnStart();

        // TEMP ---> Maybe Coroutine?
        InvokeRepeating("Shoot", 2.5f, _towerFireRate);
        
    }
}
