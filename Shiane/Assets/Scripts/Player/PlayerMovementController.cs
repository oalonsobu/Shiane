using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.VR;

public class PlayerMovementController : MonoBehaviour {
    
    enum WallJump
    {
        NotAllowed = 0,
        RightSide  = 1,
        LeftSide   = 2
    };

    [Range(0, 5f)] [SerializeField] float playerSpeed = 5f;
    [Range(0, 20f)] [SerializeField] float dashVelocity = 20f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;
    [Range(400, 650)] [SerializeField] int jumpForce = 400;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;

    Vector3 currentVelocity;
    Vector3 dashDirection;
    float dashTime = 0.1f;
    float currentDashTime;
    int remainingDashes = 2;
    bool dashReleased = true;
    bool isDashing = false;
    bool grounded = true;
    WallJump wallJumpAllowed = WallJump.NotAllowed;
    float colliderSizeRaycast = 0;
    
    //Unity components
    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Start ()
    {
        currentVelocity = Vector2.zero;
        rigidbody       = gameObject.GetComponent<Rigidbody2D>();
        collider        = gameObject.GetComponent<CapsuleCollider2D>();
        spriteRenderer  = gameObject.GetComponent<SpriteRenderer>();
        animator        = gameObject.GetComponent<Animator>();
            
        colliderSizeRaycast = 3 * collider.size.y / 4;
        GameLoopManager.instance.UpdateDashesCounter(remainingDashes);
    }

    void FixedUpdate()
    {
        UpdateGrounded();
        Dash();
        Jump();
        Move();
        CheckForEnemies();
        FlipSprite();
    }

    void UpdateGrounded()
    {
        //By default he can't jump
        wallJumpAllowed = WallJump.NotAllowed;
        
        Vector3 pos = collider.transform.position;
        //TODO: No poner tan a la esquina
        pos.x += collider.size.x / 4;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, colliderSizeRaycast, groundLayer | enemyLayer);
        Debug.DrawRay(pos, Vector2.down * colliderSizeRaycast);
        
        if (hit.collider == null)
        {
            //If there is no collision in that place we calculate the other one (maybe we are missing some collisions, but I think is not that important
            pos.x -= collider.size.x / 2;
            hit = Physics2D.Raycast(pos, Vector2.down, colliderSizeRaycast, groundLayer | enemyLayer);
            Debug.DrawRay(pos, Vector2.down * colliderSizeRaycast);
        }
        
        if (hit.collider != null && (1 << hit.collider.gameObject.layer) == enemyLayer)
        {
            //jumping over it will kill the enemy ??
            //hit.transform.GetComponent<EnemyHealthController>().TakeDamage(10);
            if (hit.collider.gameObject.CompareTag("Boss"))
            {
                if (hit.transform.GetComponent<BossController>().IsShieldUp())
                {
                    transform.GetComponent<PlayerHealthController>().KillPlayer(true);
                }
            }
            else
            {
                rigidbody.AddForce(new Vector2(0f, jumpForce));
            }
        }
        
        if (hit.collider != null)
        {
            remainingDashes = 2;
            GameLoopManager.instance.UpdateDashesCounter(remainingDashes);
            grounded = true;
        }
        else
        {
            grounded = false;
            pos = collider.transform.position;
            pos.x -= colliderSizeRaycast/2;
            //TODO: bajar un poco, en plan a los pies
            hit = Physics2D.Raycast(pos,  Vector2.right, colliderSizeRaycast, groundLayer);
            Debug.DrawRay(pos, Vector2.right * colliderSizeRaycast);
            if (hit.collider != null)
            {
                wallJumpAllowed = hit.point.x < transform.position.x ? WallJump.LeftSide : WallJump.RightSide;
            }
        }

        if (grounded || wallJumpAllowed != WallJump.NotAllowed)
        {
            rigidbody.gravityScale = 1;
        }
        else
        {
            //Make the player fall faster
            rigidbody.gravityScale += 0.02f;
        }

        animator.SetBool("Grounded", grounded);
        animator.SetFloat("YVelocity", rigidbody.velocity.y);
    }

    void Jump()
    {
        
        bool jump = Input.GetButtonDown("Jump");
        if (grounded && jump && !isDashing)
        {
            rigidbody.AddForce(new Vector2(0f, jumpForce));
            grounded = false;
            rigidbody.velocity = Vector3.zero;
        } else if (wallJumpAllowed != WallJump.NotAllowed && jump && !isDashing)
        {
            float force = jumpForce / 2;
            rigidbody.AddForce(new Vector2((wallJumpAllowed == WallJump.RightSide ? -1 : 1) * force * 2, force));
            rigidbody.velocity = Vector3.zero;
        }
    }
    
    void Dash()
    {
        bool dash = dashReleased && Input.GetAxis("Dash") > 0;
        Vector3 vForce = Vector3.up * Input.GetAxis("Vertical");
        Vector3 hForce = Vector3.right * Input.GetAxis("Horizontal");
        dashDirection = hForce.normalized + vForce.normalized;
        if (remainingDashes > 0 && dash && Math.Abs(dashDirection.magnitude) > 0)
        {
            remainingDashes--;
            dashReleased = false;
            currentDashTime = dashTime;
            isDashing = true;
            Physics2D.IgnoreLayerCollision(11, 10, true);//TODO: Get layer by name
            GameLoopManager.instance.UpdateDashesCounter(remainingDashes);
        }
        
        if (currentDashTime > 0)
        {
            rigidbody.velocity = dashDirection * dashVelocity;
            currentDashTime -= Time.deltaTime;
        }
        else if (isDashing)
        {
            StopDash();
        }
        dashReleased = dashReleased || Input.GetAxis("Dash") <= 0; //To avoid continuously dashing;
    }

    void StopDash()
    {
        currentDashTime = 0;
        rigidbody.velocity = Vector3.zero;
        isDashing = false;
        Physics2D.IgnoreLayerCollision(11, 10, false);//TODO: Get layer by name
    }
    
    void Move()
    {
        if (!isDashing)
        {
            float movementForce = Input.GetAxis("Horizontal");
            Vector2 desiredVelocity = Vector2.right * movementForce * playerSpeed;
            rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, desiredVelocity, ref currentVelocity, movementSmoothing);
            animator.SetBool("IsWalking", Math.Abs(rigidbody.velocity.x) > 0.1f);
        }
    }

    void CheckForEnemies()
    {
        if (isDashing)
        {
            RaycastHit2D hit = Physics2D.Raycast(collider.transform.position, Vector2.right, 0.5f, enemyLayer);
            if (hit.collider != null)
            {
                if (!hit.collider.gameObject.CompareTag("Boss"))
                {
                    hit.transform.GetComponent<EnemyHealthController>().TakeDamage(10);
                }
                else if (hit.transform.GetComponent<BossController>().IsShieldUp())
                {
                    transform.GetComponent<PlayerHealthController>().KillPlayer(true);
                }
                else
                {
                    hit.transform.GetComponent<BossController>().TakeDashDamage(10);
                    var force = transform.position - hit.transform.position;
                    force.Normalize();
                    StopDash();
                    rigidbody.velocity = (new Vector2( force.x * 25, force.y * 25));
                }    
            }
        }
    }
    
    void FlipSprite()
    {
        bool isFacingRight  = Input.GetAxis("Horizontal") > 0;
        bool isFacingLeft   = Input.GetAxis("Horizontal") < 0;
        bool wasFacingRight = spriteRenderer.flipX;

        spriteRenderer.flipX = (!isFacingLeft && wasFacingRight) || isFacingRight;
    }
}
