using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

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
        if (FindObjectOfType<GameManager>()?.CurrentState == GameManager.State.Playing)
        {
            if (Input.touchSupported)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                pos.z = 0;

                if (touch.phase == TouchPhase.Began)
                {
                    _newTower = Instantiate(_basicTower, pos, Quaternion.identity);
                    _newTower.GetComponent<Rigidbody2D>().freezeRotation = true;
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
                }
            }
            else
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;

                if (Input.GetMouseButtonDown(0) && !_holdingTower)
                {
                    _newTower = Instantiate(_basicTower, pos, Quaternion.identity);
                    _newTower.GetComponent<Rigidbody2D>().freezeRotation = true;
                    _holdingTower = true;
                }

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
