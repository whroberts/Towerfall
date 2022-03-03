using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject _basicTower = null;
    [SerializeField] private GameObject _sniperTower = null;


    private bool _holdingTower = false;

    private GameObject _newTower;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<GameManager>().IsPaused)
        {
            if (Input.touchSupported)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _newTower = Instantiate(_basicTower, touch.position, Quaternion.identity);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _newTower.transform.position = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (_basicTower.GetComponent<TowerBase>() != null)
                    {
                        _newTower.GetComponent<TowerBase>()
                            .SetGravity(_newTower.GetComponent<TowerBase>().TowerGravity);
                    }
                }
            }
            else
            {
                

                /*
                if (Input.GetMouseButtonDown(0) && !_holdingTower)
                {
                    Debug.Log("Spawned Tower");
                    _newTower = Instantiate(_basicTower, heldPos, Quaternion.identity);
                    _holdingTower = true;
                }
                else if (Input.GetMouseButtonUp(0) && _holdingTower)
                {
                    Debug.Log("Released Tower");
                    _newTower.GetComponent<TowerBase>().SetGravity(_newTower.GetComponent<TowerBase>().TowerGravity);
                    _holdingTower = false;
                }
                */

                if (Input.GetMouseButton(0))
                {
                    var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0f;
                    Debug.Log(mousePos);

                    if (Input.GetMouseButtonDown(0) && !_holdingTower)
                    {
                        Debug.Log("Spawned Tower");
                        _newTower = Instantiate(_basicTower, mousePos, Quaternion.identity);
                        _holdingTower = true;
                    }
                    if (_holdingTower)
                    {
                        Debug.Log("Holding Tower: " + _newTower.transform.position);
                        _newTower.transform.position = mousePos;
                    }

                    
                }
                else if (Input.GetMouseButtonUp(0) && _holdingTower)
                {
                    Debug.Log("Released Tower");
                    _newTower.GetComponent<TowerBase>().SetGravity(_newTower.GetComponent<TowerBase>().TowerGravity);
                    _holdingTower = false;
                }
            }
        }
    }
}
