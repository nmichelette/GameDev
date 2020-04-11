using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public double spread = 0.5;
    public int numPellets = 5;

    /*private void Start()
    {
        aimingPivot = this.transform;
        firePoint = this.transform.GetChild(0);
    } */

    public override void Fire()
    {
        Debug.Log(firePoint.position);
        Debug.Log(aimingPivot.position);
        var dir = firePoint.rotation * Vector3.forward;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        float spread = Random.Range(-10, 10);
        Bullet boolet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        boolet.damage = damage;
       /* for (int i = 1; i < numPellets; i++)
        { 
            //Bullet boolet = Instantiate(projectile, firePoint.position, Quaternion.Euler(new Vector3(0, 0, angle + spread+i)));
            //boolet.damage = damage;
            Bullet boolet = Instantiate(projectile, firePoint.position, firePoint.rotation);
            boolet.damage = damage;
            fireCooldown = Time.time + fireRate;
            currentAmmo--;
        } */
    }
}
