using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{

    public Transform groundDetection;
    public Transform hitbox;
    public bool moveRight = true;
    public double maxMoveDistance = 10f;
    public double fireRate = 0.5f;
    public double health = 10;
    public double damage = 1;
    public float speed = 2f;
    public float viewingDistance = 20f;
    private GameObject player;
    private Rigidbody2D rb;
    private double currentDistance;
    private bool canSeePlayer = false;
    private double fireCooldown = 0;
    private bool FacingRight = true;
    private SpriteRenderer sr;
    private float moveTimer = 0f;
    private bool help = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentDistance = 0;
        Physics2D.queriesStartInColliders = false;
        moveTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Check if player is within enemy's vieing distance
        if (Vector2.Distance(transform.position, player.transform.position) <= viewingDistance)
        {
            Vector3 direction = player.transform.position - transform.position;
            //Check if enemy can see the player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewingDistance);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player")
            {
                if (!FacingRight && direction.x > 0)
                    flip();
                else
                if (FacingRight && direction.x < 0)
                    flip();
                canSeePlayer = true;
                if (Time.time >= fireCooldown)
                {
                    Shooting();
                    fireCooldown = Time.time + fireRate;
                }
            }
            else
            {
                if (canSeePlayer)
                {
                    moveTimer = Time.time + 2f;
                    help = true;
                }
                canSeePlayer = false;
            }
        }
        else
        {
            if (canSeePlayer)
            {
                moveTimer = Time.time + 2f;
                help = true;
            }
            canSeePlayer = false;
        }
        if (!canSeePlayer && Time.time >= moveTimer)
        {
            if (help)
            {
                help = false;
            }
            currentDistance += Vector2.right.x * speed * Time.deltaTime;
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, sr.bounds.size.y + 1f);
            if (groundInfo.collider == null || groundInfo.collider.tag != "Ground" || currentDistance >= maxMoveDistance)
            {
                flip();
                currentDistance = 0;
                moveRight = !moveRight;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time >= fireCooldown)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Melee Hit Enemy");
                collision.gameObject.SendMessage("TakeDamage", damage);
            }
            fireCooldown = Time.time + fireRate;
        }
    }

    public void TakeDamage(double damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    void Shooting()
    {
        //hitbox.Translate(Vector2.right*3);
        //hitbox.Translate(Vector2.left / 3);
        //fireCooldown = Time.time + fireRate;
    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
