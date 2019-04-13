using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.VR;

public class PlayerMovementController : MonoBehaviour {

    [Range(0, 5f)] [SerializeField] float playerSpeed = 5f;
    [Range(0, 10f)] [SerializeField] float dashVelocity = 10f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;
    [Range(200, 450)] [SerializeField] int jumpForce = 400;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;

    Vector3 currentVelocity;
    float dasTime = 0.1f;
    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;
    bool isFacingRight = true;
    bool grounded = true;
    bool dashUsed = false;
    bool isDashing = false;
    Vector3 dashDirection;
    float colliderSizeRaycast = 0;

    void Start ()
    {
        currentVelocity = Vector2.zero;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        collider  = gameObject.GetComponent<CapsuleCollider2D>();
        colliderSizeRaycast = 3 * collider.size.y / 4;
    }

    void FixedUpdate()
    {
        UpdateGrounded();
        Dash();
        Jump();
        Move();
        if (rigidbody.position.y < -2)
        {
            KillPlayer();
        }
    }

    void UpdateGrounded()
    {
        //TODO: maybe two raycast, one in each side of the sprite...
        RaycastHit2D hit = Physics2D.Raycast(collider.transform.position, Vector2.down, colliderSizeRaycast, groundLayer | enemyLayer);
        if (hit.collider != null && (1 << hit.collider.gameObject.layer) == enemyLayer)
        {
            //TODO: jumping over it will kill the enemy ??
            hit.transform.GetComponent<EnemyController>().TakeDamage(10);
            rigidbody.AddForce(new Vector2(0f, jumpForce));
        }
        
        if (hit.collider != null)
        {
            dashUsed = isDashing;
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void Jump()
    {
        bool jump = Input.GetAxis("Jump") > 0 ;
        if (grounded && jump && !isDashing)
        {
            rigidbody.AddForce(new Vector2(0f, jumpForce));
            grounded = false;
        }
    }
    
    void Dash()
    {
        bool dash = Input.GetAxis("Dash") > 0;
        if (dash && !isDashing && !dashUsed)
        {
            Vector3 hForce = Vector3.up * Input.GetAxis("Horizontal");
            Vector3 vForce = Vector3.right * Input.GetAxis("Vertical");
            dashDirection = hForce.normalized + vForce.normalized;
            isDashing = true;
            dashUsed  = true;
        }
        
        if (isDashing)
        {
            rigidbody.AddForce(new Vector2(0f, jumpForce));
            isDashing = false;
        }
    }
    
    void Move()
    {
        if (!isDashing)
        {
            float movementForce = Input.GetAxis("Horizontal");
            Vector2 desiredVelocity = Vector2.right * movementForce * playerSpeed;
            rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, desiredVelocity, ref currentVelocity, movementSmoothing);
        }
    }

    void KillPlayer()
    {
        GameLoopManager.instance.GameOver();
    }

}
