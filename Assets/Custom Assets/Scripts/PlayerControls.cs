using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Bullet projectile;
    public float moveSpeed = 20f;
    public float maxSpeed = 12f;
<<<<<<< HEAD:Assets/Custom Assets/Scripts/PlayerControls.cs
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
=======
    public float maxAirSpeed = 10f;
    public float decel = 50f;   //higher numbers make you decelerate slower
>>>>>>> 9c2e11d43beca7a4f827f372492a46f7ccde04d3:Assets/Custom Assets/Scripts/Movement.cs

    //jump variables
    [Range(1,20)]
    public float jumpVelocity;
    public float fallM = 2.5f;
    public float lowJumpM = 7.5f;

    private bool FacingRight = true;
    private float jumpTimer = 0; 


    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
<<<<<<< HEAD:Assets/Custom Assets/Scripts/PlayerControls.cs
        aimingPivot = this.transform.Find("PivotPoint");
        firePoint = aimingPivot.transform.Find("ShootPoint");
=======
        jumpTimer = Time.time;
>>>>>>> 9c2e11d43beca7a4f827f372492a46f7ccde04d3:Assets/Custom Assets/Scripts/Movement.cs
    }

    // Update is called once per frame
    void Update()
    {
        bool onGround = IsGrounded();
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
                playerRB.velocity = new Vector2(vx * decel * Time.deltaTime, playerRB.velocity.y);
            }
                
        }
        else
        {
            //accelerating
            if (onGround)
            {
                if (playerRB.velocity.x < maxSpeed && playerRB.velocity.x > -maxSpeed)
                {
                    movement = new Vector2(dir, 0f); //gives direction     
                }
                else
                {
                    playerRB.velocity = new Vector2(maxSpeed * dir, playerRB.velocity.y);
                }
            }
            else
            {
                if (playerRB.velocity.x < maxAirSpeed && playerRB.velocity.x > -maxAirSpeed)
                {
                    movement = new Vector2(dir, 0f); //gives direction     
                }
                else
                {
                    playerRB.velocity = new Vector2(maxAirSpeed * dir, playerRB.velocity.y);
                }
            }
           /* if (dir > 0 && !FacingRight) //going right
                flip();
            else if (dir < 0 && FacingRight)
                flip(); */

        }

        //jump
        if (Time.time > jumpTimer && (Input.GetButtonDown("Jump") && onGround))
        {
            Jump();
            jumpTimer = Time.time + .25f;
        }

        //hold jump to go further
        if (!onGround)
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
        

        //Control Aiming
<<<<<<< HEAD:Assets/Custom Assets/Scripts/PlayerControls.cs
        if (Time.time >= reloading)
=======
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - aimingPivot.transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log("Angle:" + angle);
        if ((angle > -88 && angle < 88) && !FacingRight)
        {
            flip();
        }
        else if ((angle > 92f || angle < -92) && FacingRight)
>>>>>>> 9c2e11d43beca7a4f827f372492a46f7ccde04d3:Assets/Custom Assets/Scripts/Movement.cs
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
<<<<<<< HEAD:Assets/Custom Assets/Scripts/PlayerControls.cs
=======
        if (FacingRight)
            aimingPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        else
            aimingPivot.transform.rotation = Quaternion.Euler(180f, 0f, -angle);

>>>>>>> 9c2e11d43beca7a4f827f372492a46f7ccde04d3:Assets/Custom Assets/Scripts/Movement.cs
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
        return Physics2D.Raycast(playerRB.position, Vector3.down, 1.25f, LayerMask.GetMask("Ground")) || 
              (Physics2D.Raycast(playerRB.position + new Vector2(.5f, 0) , Vector3.down, 1.25f, LayerMask.GetMask("Ground")) || 
               Physics2D.Raycast(playerRB.position + new Vector2(-.5f, 0), Vector3.down, 1.25f, LayerMask.GetMask("Ground")));
    }
}
