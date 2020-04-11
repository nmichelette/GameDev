using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //movement
    public float moveSpeed = 20f;
    public float airMoveSpeed = 15f;
    public float maxSpeed = 12f;
    public float maxAirSpeed = 10f;
    [Range(0, .5f)]
    public float decel = .25f;
    [Range(0, .5f)]
    public float airDecel = .25f;

    private Vector2 movement;
    private Rigidbody2D playerRB;
    private bool onGround = false;

    //jump variables
    public float jumpVelocity;
    public float fallM = 2.5f;
    public float lowJumpM = 7.5f;
    private float jumpTimer = 0;

    //combat
    public Gun[] Armory;
    public int currentWeapon;

    //animation
    public Animator bodyAnimator;
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
        float dir = Input.GetAxisRaw("Horizontal");
        bodyAnimator.SetFloat("Speed", Mathf.Abs(movement.x));

        //checks if the x velocity is within certain parameters
        if (Input.GetButton("Horizontal"))
        {
            //accelerate
            movement = new Vector2(dir, 0f); //gives direction
            if (onGround)
                playerRB.velocity = new Vector2(Mathf.Clamp(playerRB.velocity.x, -maxSpeed, maxSpeed), playerRB.velocity.y);
            else
                playerRB.velocity = new Vector2(Mathf.Clamp(playerRB.velocity.x, -maxAirSpeed, maxAirSpeed), playerRB.velocity.y);
        }
        else //stops all forces
            movement = new Vector2(0f, 0f);


        //jump
        if ((Input.GetButtonDown("Jump") && onGround) && Time.time > jumpTimer)
        {
            Jump();
            jumpTimer = Time.time + .25f;
        }

        //hold jump to go further
        if (!onGround)
        {
            if (playerRB.velocity.y < 1)
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
        if (Mathf.Abs(direction.x) > 0)
            playerRB.AddForce(direction * moveSpeed);
        else
        {
            if (Mathf.Abs(playerRB.velocity.x) > .1)
            {
                if (onGround)
                    playerRB.velocity = new Vector2(Mathf.Lerp(playerRB.velocity.x, 0, decel), playerRB.velocity.y);
                else
                    playerRB.velocity = new Vector2(Mathf.Lerp(playerRB.velocity.x, 0, airDecel), playerRB.velocity.y);
            }
            else
                playerRB.velocity = new Vector2(0, playerRB.velocity.y);

        }
    }

    void Jump()
    {
        playerRB.velocity += Vector2.up * jumpVelocity;
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
