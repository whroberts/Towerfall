using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : TowerBase
{
    [SerializeField] private GameObject _projectileRef = null;

    public override void PrintOnStart()
    {
        Debug.Log("Printing From Override function in Tower1 in Start");
    }

    public virtual void UpdateAmmoCount()
    {
        // might be completely unnecessary but we need to pull the UI updater out of the repeating shooting function

        transform.GetChild(1).GetComponent<TextMesh>().text = ("Ammo: " + _currentAmmo + " / " + _totalAmmo);
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
            UpdateAmmoCount();
        }

        //implemented the reload option
        else if (_currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }

    }

    public override IEnumerator Reload()
    {
        //cancels the attack
        CancelInvoke();

        //waits for the reload time
        yield return new WaitForSeconds(_reloadTime);

        //resets the ammo count
        _currentAmmo = _totalAmmo;

        //sets UI
        UpdateAmmoCount();
        
        //reinvokes the shooting
        InvokeRepeating("Shoot", _timeToStartFiring, _towerFireRate);

        // reload sound effect
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if(Input.GetButtonDown("Fire1"))
        {
            _canShoot = true;
        }
        
        if(Input.GetButtonDown("Fire2"))
        {
            _canShoot = false;
        }
    }

    

    private void Start()
    {
        //PrintOnStart();

        // TEMP ---> Maybe Coroutine?

        UpdateAmmoCount();
    }
}
