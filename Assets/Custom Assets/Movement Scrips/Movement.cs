using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody2D playerRB;
    public Vector2 movement;
    public float moveSpeed = 20f;
    public float maxSpeed = 12f;

    //jump variables
    [Range(1,10)]
    public float jumpVelocity;
    public float fallM = 2.5f;
    public float lowJumpM = 7.5f;


    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
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
    }




    void moveCharacter(Vector2 direction)
    {
         playerRB.AddForce(direction * moveSpeed);
    }

    void Jump()
    {
        playerRB.velocity += Vector2.up * jumpVelocity;
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(playerRB.position, Vector3.down, 1.25f);
    }
}
