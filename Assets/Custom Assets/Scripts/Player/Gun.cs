using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public double fireRate = 0.5f;
    public double reloadTime = 0.5f;
    public double damage = 5;
    public int maxAmmo = 6;
    public int currentAmmo = 6;
    public Transform aimingPivot;
    public Transform firePoint;
    public Bullet projectile;
    private double reloading;
    private double fireCooldown;

    public double getFireRate() { return fireRate; }
    public double getReloadTime() { return reloadTime; }
    public double getDamage() { return damage; }
    public double getMaxAmmo() { return maxAmmo; }
    public double getCurrentAmmo() { return currentAmmo; }
    public void Reload()
    {
        reloading = Time.time + reloadTime;
        currentAmmo = maxAmmo;
    }
    public void Reload(int num) //If you want to reload a specific num
    {
        reloading = Time.time + reloadTime;
        currentAmmo += num;
        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
    }
    public void Fire()
    {
        Bullet boolet = Instantiate(projectile, firePoint.position, firePoint.rotation);
        boolet.damage = damage;
        fireCooldown = Time.time + fireRate;
        currentAmmo--;
    }
    public void Fire(int num) //Fire multiple shots
    {

    }
    public bool isReloading()
    {
        if (Time.time >= reloading)
            return false;
        else
            return true;
    }
    public bool CanFire()
    {
        if (currentAmmo != 0 && Time.time >= fireCooldown && Time.time >= reloading)
            return true;
        else
            return false;
    }
}
