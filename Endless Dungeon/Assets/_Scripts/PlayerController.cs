using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Rigidbody2D rb;
    public Animator animator;

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnemies;
    public int damage;

    Vector2 movement;

    float dirFacingX;
    float dirFacingY;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("Attack", Input.GetKey(KeyCode.Space));

        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }

                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

        if (health > numOfHearts) 
        {
            health = numOfHearts;
        }
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else 
            {
                hearts[i].sprite = emptyHeart;
            }
            
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else 
            { 
                hearts[i].enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //TODO: Need to update attackPos so that it matches with dirFacing

        if (movement.x > 0.01) {
            //Moving Right
            dirFacingX = 1;
            dirFacingY = 0;

            
        }
        else if (movement.x < -0.01) 
        {
            //Moving Left
            dirFacingX = -1;
            dirFacingY = 0;

            
        }
        else if (movement.y > 0.01)
        {
            //Moving Up
            dirFacingX = 0;
            dirFacingY = 1;

            
        }
        else if (movement.y < -0.01)
        {
            //Moving Down
            dirFacingX = 0;
            dirFacingY = -1;

            
        }
        
        animator.SetFloat("DirFacingX", dirFacingX);
        animator.SetFloat("DirFacingY", dirFacingY);

        Debug.Log("Direction: " + dirFacingX + ", " + dirFacingY);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
