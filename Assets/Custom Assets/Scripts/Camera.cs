using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;

    //public float smoothSpeed = .125f;
    //public Vector2 offset;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
