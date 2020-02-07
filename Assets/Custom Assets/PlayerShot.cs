using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour {

    Rigidbody2D rigid;
    public float speed;
    public float damage;
    MovingPlayer player;
    private float baseDamage;

    float counter;
    public float TimeUntilDeleted;

    // Use this for initialization
    void Start ()
    {
        counter = TimeUntilDeleted;
        rigid = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<MovingPlayer>();
        baseDamage = damage;
	}
	
	// Update is called once per frame
	void Update ()
    {
        TimeUntilDeleted -= Time.deltaTime;

        if (TimeUntilDeleted <= 0)
            Destroy(gameObject);
        rigid.velocity = transform.up * speed;

        if (player.MoreDmg)
        {
            damage = baseDamage * 1.4f;
        }
        else
            damage = baseDamage;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Enemy script = collision.GetComponent<Enemy>();
            script.health -= damage;

            Destroy(gameObject);
        }
        if(collision.tag == "Tile")
        {
            Debug.Log(collision.GetComponent<Tile>().getColor().Equals(Color.gray));
            if (collision.GetComponent<Tile>().getColor().Equals(Color.gray))
                Destroy(gameObject);
        }
    }


}
