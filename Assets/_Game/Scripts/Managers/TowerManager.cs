using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject _basicTower = null;
    [SerializeField] private GameObject _sniperTower = null;
    [SerializeField] private Text _text = null;


    private bool _holdingTower = false;

    private GameObject _newTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("Raw: " + mousePos);
        mousePos.z = 35;
        Debug.Log("Filtered: " + mousePos);


        if (FindObjectOfType<GameManager>()?.CurrentState == GameManager.State.Playing)
        {
            if (Input.touchSupported)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _text.text = "Began Movement";
                    _newTower = Instantiate(_basicTower, mousePos, Quaternion.identity);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _text.text = "Moving";
                    _newTower.transform.position = mousePos;
                    _text.text = mousePos.ToString();
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    _text.text = "Placed Tower";
                    _newTower?.GetComponent<TowerBase>()?.SetGravity(_newTower.GetComponent<TowerBase>().TowerGravity);
                }
            }
            else
            {

                if (Input.GetMouseButtonDown(0) && !_holdingTower)
                {
                    _text.text = "Began Movement";
                    _newTower = Instantiate(_basicTower, mousePos, Quaternion.identity);
                    _holdingTower = true;
                }

                if (_holdingTower)
                {
                    _text.text = "Moving: " + mousePos.ToString();
                    _newTower.transform.position = mousePos;
                }

                /*
                if (Input.GetMouseButtonUp(0))
                {
                    _text.text = "Placed Tower";
                    _holdingTower = false;
                    _newTower.GetComponent<TowerBase>()?.SetGravity(_newTower.GetComponent<TowerBase>().TowerGravity);
                }
                */
            }
        }
        
    }
}
