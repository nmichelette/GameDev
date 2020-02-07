using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public AudioSource laser;

    public GameObject Shot;
    public Transform shotSpawn;

    MovingPlayer player;
    private float nextFire;

    public float fireRate;

    public float timeUntilSound;
    float timeUntilSoundCounter;

    // Use this for initialization
    void Start ()
    {
        timeUntilSoundCounter = timeUntilSound;
        player = FindObjectOfType<MovingPlayer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float mag = (float)Math.Sqrt(Math.Pow(transform.position.x - mousePosition.x, 2) + Math.Pow(transform.position.y - mousePosition.y, 2));
        Vector3 rota = new Vector3((transform.position.x - mousePosition.x) / mag, (transform.position.y - mousePosition.y) / mag, 10);
        Quaternion rot = Quaternion.LookRotation(rota, Vector3.forward);
        transform.rotation = rot;
        //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        timeUntilSoundCounter -= Time.deltaTime;

        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            laser.Play();
            Instantiate(Shot, shotSpawn.position, shotSpawn.rotation);
            
            if (player.MoreShots)
            {
                Vector3 forward1 = new Vector3(rota.x + .3f, rota.y + .3f, 10);
                float f1m = (float)Math.Sqrt(Math.Pow(forward1.x, 2) + Math.Pow(forward1.y, 2));
                forward1 = new Vector3(forward1.x / f1m, forward1.y / f1m, 10);
                Vector3 forward2 = new Vector3(rota.x - .3f, rota.y - .3f, 10);
                float f2m = (float)Math.Sqrt(Math.Pow(forward2.x, 2) + Math.Pow(forward2.y, 2));
                forward2 = new Vector3(forward2.x / f2m, forward2.y / f2m, 10);
                Instantiate(Shot, shotSpawn.position, Quaternion.LookRotation(forward1, Vector3.forward));
                Instantiate(Shot, shotSpawn.position, Quaternion.LookRotation(forward2, Vector3.forward));
            }
        }

    }

    private void Shoot(Vector2 mPos)
    {
        Debug.DrawLine(transform.position, (mPos), Color.cyan);
    }
}
