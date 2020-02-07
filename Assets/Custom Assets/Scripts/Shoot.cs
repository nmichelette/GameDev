using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject projectile;


    // Update is called once per frame
    void Update()
    {
        Shooting();
    }

    public void Shooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(projectile,FirePoint.position,FirePoint.rotation);
        }
    }
}
