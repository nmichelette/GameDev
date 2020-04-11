using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 10;
    private int hp = 10;

    public Animator[] anims;
    public Rigidbody2D rb;

    void Start()
    {
        anims = GetComponentsInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hp = maxHealth;
    }

    public void addHP(int health)
    {
        hp += health;

        if (hp <= 0)
        { //die just not yet
            /*
            foreach (Animator a in anims)
            {
                a.SetTrigger("death");
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            */
        }
    }
}
