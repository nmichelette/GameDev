using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    public EnemyBullet bullet;
    public Transform FirePoint;
    public Transform aimingPivot;
    public Transform groundDetection;
    public bool moveRight = true;
    public double maxMoveDistance = 10f;
    public double fireRate = 5;
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
        Vector3 direction = player.transform.position - aimingPivot.transform.position;
        direction.Normalize();
        //Check if player is within enemy's vieing distance
        if (Vector2.Distance(transform.position, player.transform.position) <= viewingDistance)
        {
            //Check if enemy can see the player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewingDistance);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player")
            {
<<<<<<< HEAD
                canSeePlayer = true;
=======
>>>>>>> 9c2e11d43beca7a4f827f372492a46f7ccde04d3
                //Control Aiming
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //Debug.Log("Angle:" + angle);
                if ((angle > -88 && angle < 88) && !FacingRight)
                {
                    flip();
                }
                else if ((angle > 92f || angle < -92) && FacingRight)
                {
                    flip();
                }
                if (FacingRight)
                    aimingPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                else
                    aimingPivot.transform.rotation = Quaternion.Euler(180f, 0f, -angle);

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
            if(canSeePlayer)
            {
                moveTimer = Time.time + 2f;
                help = true;
            }
            canSeePlayer = false;
        }
        if(!canSeePlayer && Time.time >= moveTimer)
        {
            if(help)
            { 
                aimingPivot.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                help = false;
            }
            currentDistance += Vector2.right.x * speed * Time.deltaTime;
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position,Vector2.down, sr.bounds.size.y+1f);
            if (groundInfo.collider == null || groundInfo.collider.tag != "Ground" || currentDistance>=maxMoveDistance)
            {
                flip();
                currentDistance = 0;
                moveRight = !moveRight;
            }
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
            EnemyBullet boolet = Instantiate(bullet, FirePoint.position, FirePoint.rotation);
            boolet.damage = damage;
    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
