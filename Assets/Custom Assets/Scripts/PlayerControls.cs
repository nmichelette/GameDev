using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Bullet projectile;
    public float moveSpeed = 20f;
    public float maxSpeed = 12f;
    public double fireRate = 0.5f;
    public double reloadTime = 0.5f;
    public double health = 10;
    public double damage = 5;
    public int maxAmmo = 6;
    public int currentAmmo = 6;
    private Rigidbody2D playerRB;
    private Transform aimingPivot;
    private Vector2 movement;
    private Transform firePoint;  
    private double fireCooldown = 0;
    private double reloading = 0;

    //jump variables
    [Range(1,20)]
    public float jumpVelocity;
    public float fallM = 2.5f;
    public float lowJumpM = 7.5f;

    private bool FacingRight = true;


    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        aimingPivot = this.transform.Find("PivotPoint");
        firePoint = aimingPivot.transform.Find("ShootPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float dir = Input.GetAxis("Horizontal");
        //checks if the x velocity is within certain parameters
        if (!Input.GetButton("Horizontal"))
        {
            //decelerating
            float vx = playerRB.velocity.x;

            movement = new Vector2(0f, 0f);

            if ((vx < 1 && vx > 0) || (vx > -1 && vx < 0))
                playerRB.velocity = new Vector2(0, playerRB.velocity.y);
            else
            {
                playerRB.velocity = new Vector2(vx * .9f, playerRB.velocity.y);
                
            }
                
        }
        else
        {
            //accelerating
            if (playerRB.velocity.x < maxSpeed && playerRB.velocity.x > -maxSpeed)
            {
                movement = new Vector2(dir, 0f); //gives direction     
            }
            else
            {
                playerRB.velocity = new Vector2(maxSpeed * dir, playerRB.velocity.y);
            }
           /* if (dir > 0 && !FacingRight) //going right
                flip();
            else if (dir < 0 && FacingRight)
                flip(); */

        }

        //hold jump to go further
        if (!IsGrounded())
        {
            if (playerRB.velocity.y < 0)
            {
                playerRB.velocity += Vector2.up * Physics2D.gravity.y * (fallM - 1) * Time.deltaTime;
            }
            else if (playerRB.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                playerRB.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpM - 1) * Time.deltaTime;
            }
        }

        //Control Shooting
        if (Input.GetButtonDown("Fire1") && currentAmmo!=0 && Time.time >= fireCooldown && Time.time >= reloading)
        {
            Bullet boolet = Instantiate(projectile, firePoint.position, firePoint.rotation);
            boolet.damage = damage;
            fireCooldown = Time.time + fireRate;
            currentAmmo--;
        }

        //Reload
        if(Input.GetKeyDown("r"))
        {
            aimingPivot.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            currentAmmo = maxAmmo;
            reloading = Time.time + reloadTime;
        }
    }

    void FixedUpdate()
    {
        //move
        moveCharacter(movement);
        //jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        //Control Aiming
        if (Time.time >= reloading)
        {
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - aimingPivot.transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 80f;
            //Debug.Log("Angle:" + angle);
            if ((angle > 182f || angle < -1) && FacingRight)
            {
                flip();
            }
            else if ((angle < 178f && angle > 1) && !FacingRight)
            {
                flip();
            }
            if (FacingRight)
                aimingPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            else
                aimingPivot.transform.rotation = Quaternion.Euler(0f, 180f, -angle - 20f);
        }
    }

    void flip()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void moveCharacter(Vector2 direction)
    {
         playerRB.AddForce(direction * moveSpeed);
    }

    void Jump()
    {
        playerRB.velocity += Vector2.up * jumpVelocity;
    }

    public void TakeDamage(double damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(playerRB.position, Vector3.down, 1.25f);
    }
}
