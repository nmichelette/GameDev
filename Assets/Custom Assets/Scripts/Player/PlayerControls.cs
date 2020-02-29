using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float maxSpeed = 12f;
    public double health = 10;
    public float maxAirSpeed = 10f;
    public float decel = 50f;   //higher numbers make you decelerate slower
    public Gun[] Armory;
    public int currentWeapon;
    public Animator bodyAnimator;
    private Rigidbody2D playerRB;
    private Vector2 movement;
    private bool onGround = false;

    //jump variables
    [Range(1,20)]
    public float jumpVelocity;
    public float fallM = 2.5f;
    public float lowJumpM = 7.5f;
    private float jumpTimer = 0;

    private bool FacingRight = true;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        currentWeapon = 0;
        switchWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = IsGrounded();
        float dir = Input.GetAxis("Horizontal");
        bodyAnimator.SetFloat("Speed",Mathf.Abs(movement.x)); 
        //checks if the x velocity is within certain parameters
        if (!Input.GetButton("Horizontal"))
        {
            //decelerating
            float vx = playerRB.velocity.x;

            movement = new Vector2(0f, 0f);

            if ((vx < 1 && vx > 0) || (vx > -1 && vx < 0))
                playerRB.velocity = new Vector2(0, playerRB.velocity.y);
            else
                playerRB.velocity = new Vector2(vx * decel * Time.deltaTime, playerRB.velocity.y);
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
            {   //air acceleration
                if (playerRB.velocity.x < maxAirSpeed && playerRB.velocity.x > -maxAirSpeed)
                {
                    movement = new Vector2(dir, 0f); //gives direction     
                }
                else
                {
                    playerRB.velocity = new Vector2(maxAirSpeed * dir, playerRB.velocity.y);
                }
            }

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
        if (Input.GetButtonDown("Fire1") && Armory[currentWeapon].CanFire())
        {
            Armory[currentWeapon].Fire();
        }

        //Reload
        if(Input.GetButtonDown("Reload"))
        {
            if(FacingRight)
                Armory[currentWeapon].transform.rotation = Quaternion.Euler(0f, 0f, -40f);
            else
                Armory[currentWeapon].transform.rotation = Quaternion.Euler(180f, 0f, 140f);
            Armory[currentWeapon].Reload();
        }
        //Switch Weapons, brute force method
        if(Input.GetKeyDown("q"))
        {
            if (currentWeapon == 0)
                currentWeapon = 1;
            else
                currentWeapon = 0;
            switchWeapons();
        }
    }

    void FixedUpdate()
    {
        //move
        moveCharacter(movement);
        
        //Control Aiming
        if (!Armory[currentWeapon].isReloading())
        {
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Armory[currentWeapon].transform.position;
            direction.Normalize();
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
                Armory[currentWeapon].transform.rotation = Quaternion.Euler(0f, 0f, angle);
            else
                Armory[currentWeapon].transform.rotation = Quaternion.Euler(180f, 0f, -angle);
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

    private void switchWeapons()
    {
        if(currentWeapon == 0)
        {
            Armory[0].gameObject.SetActive(true);
            Armory[1].gameObject.SetActive(false);
        }
        else
        {
            Armory[0].gameObject.SetActive(false);
            Armory[1].gameObject.SetActive(true);
        }
    }

    private bool IsGrounded()
    {
        //Need to get a better way to check below player, probably just playerheight/2+1
        return Physics2D.Raycast(playerRB.position, Vector3.down, 3f, LayerMask.GetMask("Ground")) ||
              (Physics2D.Raycast(playerRB.position + new Vector2(.5f, 0), Vector3.down, 1.25f, LayerMask.GetMask("Ground")) ||
               Physics2D.Raycast(playerRB.position + new Vector2(-.5f, 0), Vector3.down, 1.25f, LayerMask.GetMask("Ground")));
    }
}
