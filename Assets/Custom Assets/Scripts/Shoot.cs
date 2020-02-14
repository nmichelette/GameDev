using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform FirePoint;
    public Bullet projectile;
    public double damage = 5;

    // Update is called once per frame
    void Update()
    {
        Shooting();
    }

    public void Shooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Bullet boolet =  Instantiate(projectile,FirePoint.position,FirePoint.rotation);
            boolet.damage = damage;
        }
    }
}
