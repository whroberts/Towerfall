using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    // Use this for initialization
    public float speed = 60f;
    public float rotationAngle = 30f;
    public float rotationOffset = 0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time*speed, rotationAngle) - rotationOffset);
    }
}
