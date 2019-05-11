using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyBasicController : MonoBehaviour
{

    [SerializeField] bool canMove = true;
    [Range(0, 5f)] [SerializeField] float enemySpeed = 2.5f;
    
    GameObject player;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collider;
    Animator animator;
    
    
    bool canGoForward  = false;
    bool playerInRange = false;
    bool isAttacking   = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
        UpdateStatus();
        Move();
    }

    void FlipSprite()
    {
        if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    
    void Move()
    {
        if (canGoForward)
        {
            transform.position += Vector3.right * enemySpeed * (spriteRenderer.flipX ? 1 : -1) * Time.deltaTime;
        }

        if (playerInRange && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("Attacking", isAttacking);
        }
        
        animator.SetBool("Walking", canGoForward);
    }

    void UpdateStatus()
    {
        if (isAttacking)
        {
            //If attacking, the enemy should stay quiet
            canGoForward  = false;
            playerInRange = false;
            return;
        }
        
        
        if (canMove)
        {
            canGoForward = true; //By default, he can move
        }
        
        var playerMask = (1 << 11);
        Vector3 pos = collider.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, spriteRenderer.flipX ? Vector2.right: Vector2.left, collider.size.x, playerMask);
        Debug.DrawRay(pos, (spriteRenderer.flipX ? Vector2.right: Vector2.left) * collider.size.x * 2);
        if (hit.collider != null)
        {
            playerInRange = true;
            canGoForward = false;
        }
        //If he can attack the play he will not move
        else
        {
            var worldMask = (1 << 9) | (1 << 12);
            hit = Physics2D.Raycast(pos, spriteRenderer.flipX ? Vector2.right: Vector2.left, collider.size.x, worldMask);
            if (hit.collider != null)
            {
                canGoForward = false;
            }
            else
            {
                pos += new Vector3(collider.size.x / 4 * (spriteRenderer.flipX ? 1 : -1), 0, 0);
                hit = Physics2D.Raycast(pos, Vector3.down, collider.size.y, worldMask);
                if (hit.collider == null)
                {
                    canGoForward = false;
                }
            }
        }
    }

    public void Attack()
    {
        //isAttacking = false;
    }
    
    public void StopAttacking()
    {
        Debug.Log("asdasdasd");
        isAttacking = false;
        animator.SetBool("Attacking", isAttacking);
    }
}
