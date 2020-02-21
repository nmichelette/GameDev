using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 2f;
    public double damage = 1;
    public float DestroyTimer = 10;
    private double deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        deathTimer = Time.time + DestroyTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject); //Remove Boolet
        }

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Enemy hit Player");
            collision.gameObject.SendMessage("TakeDamage", damage);
            Destroy(this.gameObject); //Remove Boolet
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= deathTimer) //Destroy bullet after awhile
        {
            Destroy(this.gameObject); //Remove Boolet
        }
    }
}
