using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public EnemyBullet bullet;
    public Transform FirePoint;
    public Transform aimingPivot;
    public double fireRate = 5;
    public double damage = 1;
    public float viewingDistance = 20f;
    private bool canSeePlayer = false;
    private double fireCooldown = 0;
    private bool FacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = player.transform.position - aimingPivot.transform.position;
        direction.Normalize();
        if (Vector2.Distance(transform.position, player.transform.position) <= viewingDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewingDistance);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player")
            {
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

        }
    }

    public void Shooting()
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
