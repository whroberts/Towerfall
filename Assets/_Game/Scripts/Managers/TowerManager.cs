using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject _basicTower = null;
    [SerializeField] private GameObject _sniperTower = null;
    [SerializeField] private GameObject _boxTower = null;

    [SerializeField] private int _basicTowerCost, _sniperTowerCost, _boxTowerCost;

    private bool _holdingTower = false;

    private GameManager _manager;
    private GameObject _newTower;
    private int _newTowerType;

    // Start is called before the first frame update
    void Start()
    {
        _newTowerType = 0;
        _manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameManager>()?.CurrentState == GameManager.State.Playing)
        {
            if (Input.touchSupported)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                pos.z = 0;

                if (touch.phase == TouchPhase.Began && _newTowerType != 0)
                {
                    if (_newTowerType == 1 && _manager.PayForTower(_basicTowerCost))
                        _newTower = Instantiate(_basicTower, pos, Quaternion.identity);

                    else if (_newTowerType == 2 && _manager.PayForTower(_sniperTowerCost))
                        _newTower = Instantiate(_sniperTower, pos, Quaternion.identity);

                    else if (_newTowerType == 3 && _manager.PayForTower(_boxTowerCost))
                        _newTower = Instantiate(_boxTower, pos, Quaternion.identity);

                    else _newTower = null;

                    _newTower.GetComponent<Rigidbody2D>().freezeRotation = true;
                    _newTower.GetComponent<Collider2D>().enabled = false;
                    _holdingTower = true;

                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _newTower.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    _newTower.transform.position = pos;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    _holdingTower = false;
                    _newTower.GetComponent<Rigidbody2D>().freezeRotation = false;
                    _newTower.GetComponent<TowerBase>()?.SetGravity(_newTower.GetComponent<TowerBase>().TowerGravity);
                    _newTower.GetComponent<Collider2D>().enabled = true;
                    _newTower = null;
                }
            }
            else
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;

                if (Input.GetMouseButtonDown(0) && !_holdingTower && _newTowerType != 0)
                {
                    if (_newTowerType == 1 && _manager.PayForTower(_basicTowerCost))
                        _newTower = Instantiate(_basicTower, pos, Quaternion.identity);

                    else if (_newTowerType == 2 && _manager.PayForTower(_sniperTowerCost))
                        _newTower = Instantiate(_sniperTower, pos, Quaternion.identity);

                    else if (_newTowerType == 3 && _manager.PayForTower(_boxTowerCost))
                        _newTower = Instantiate(_boxTower, pos, Quaternion.identity);

                    else _newTower = null;

                    if (_newTower != null)
                    {
                        _newTower.GetComponent<Rigidbody2D>().freezeRotation = true;
                        _holdingTower = true;
                    }

                }

                if (_newTower != null)
                {
                    if (_holdingTower)
                    {
                        _newTower.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                        _newTower.transform.position = pos;
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        _holdingTower = false;
                        _newTower.GetComponent<Rigidbody2D>().freezeRotation = false;
                        _newTower.GetComponent<TowerBase>()?.SetGravity(_newTower.GetComponent<TowerBase>().TowerGravity);
                    }
                }
            }
        }
        
    }

    public void TowerButtonPressEvent(int towerType)
    {
        _newTowerType = towerType;
    }

    public void TowerButtonReleaseEvent()
    {
        _newTowerType = 0;
    }


}
