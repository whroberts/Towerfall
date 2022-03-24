using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTower : TowerBase
{
    [SerializeField] private GameObject _projectileRef = null;
    [SerializeField] private GameObject _particleRef = null;
    private bool reloading = false;

    public override void PrintOnStart()
    {
        Debug.Log("Printing From Override function in Tower1 in Start");
    }

    public virtual void UpdateAmmoCount()
    {
        // might be completely unnecessary but we need to pull the UI updater out of the repeating shooting function

        transform.GetChild(2).localScale = new Vector3(transform.GetChild(2).localScale.x, (_currentAmmo / _totalAmmo) * 3.0f, transform.GetChild(2).localScale.z);
    }

    public override void Shoot()
    {
        if (_currentAmmo > 0)
        {
            //throw new System.NotImplementedException();

            Instantiate(_projectileRef, _shootPosition.position, _shootPosition.rotation);

            GameObject prt = Instantiate(_particleRef, _shootPosition.position, _shootPosition.rotation);
            prt.transform.rotation = Quaternion.Euler(prt.transform.rotation.x, prt.transform.rotation.y, prt.transform.rotation.z - 60f);

            //Rotational recoil from shooting
            _rb.AddTorque(_towerRotRecoil);
            //Linear recoil from shooting
            _rb.AddForce(_towerLinRecoil);

            _currentAmmo -= 1;
            UpdateAmmoCount();
        }

    }

    public override IEnumerator Reload()
    {
        //cancels the attack
        CancelInvoke();

        StartCoroutine(Timer(_reloadTime));

        //waits for the reload time
        yield return new WaitForSeconds(_reloadTime);

        //resets the ammo count
        _currentAmmo = _totalAmmo;

        reloading = false;

        transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color32(255, 200, 0, 125);

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

        //implemented the reload option
        if (_currentAmmo <= 0 && reloading == false)
        {
            reloading = true;
            StartCoroutine(Reload());
        }
    }

    

    private void Start()
    {
        //PrintOnStart();

        // TEMP ---> Maybe Coroutine?

        UpdateAmmoCount();
    }

    IEnumerator Timer(float initDur)
    {

        transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color32(255, 250, 200, 125);

        float duration = 0;

        while (duration < initDur)
        {
            duration += Time.deltaTime;

            transform.GetChild(2).localScale = new Vector3(transform.GetChild(2).localScale.x, ((duration / initDur) * 3.0f), transform.GetChild(2).localScale.z);

            yield return null;
        }
    }
}
