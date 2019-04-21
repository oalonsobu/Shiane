﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.VR;

public class PlayerMovementController : MonoBehaviour {

    [Range(0, 5f)] [SerializeField] float playerSpeed = 5f;
    [Range(0, 20f)] [SerializeField] float dashVelocity = 20f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .05f;
    [Range(200, 450)] [SerializeField] int jumpForce = 400;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;

    Vector3 currentVelocity;
    Vector3 dashDirection;
    float dashTime = 0.1f;
    float currentDashTime;
    bool canDash = false;
    bool dashReleased = true;
    bool isDashing = false;
    Rigidbody2D rigidbody;
    CapsuleCollider2D collider;
    bool isFacingRight = true;
    bool grounded = true;
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
        CheckForEnemies();
        dashReleased = dashReleased || Input.GetAxis("Dash") <= 0; //To avoid continuously dashing;
        if (rigidbody.position.y < -2)
        {
            KillPlayer();
        }
    }

    void UpdateGrounded()
    {
        //TODO: maybe two raycast, one in each side of the sprite...
        Vector3 pos = collider.transform.position;
        pos.x += collider.size.x / 2;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, colliderSizeRaycast, groundLayer | enemyLayer);
        
        if (hit.collider == null)
        {
            //If there is no collision in that place we calculate the other one (maybe we are missing some collisions, be I think is not that important
            pos.x -= collider.size.x;
            hit = Physics2D.Raycast(pos, Vector2.down, colliderSizeRaycast, groundLayer | enemyLayer);
        }
        
        if (hit.collider != null && (1 << hit.collider.gameObject.layer) == enemyLayer)
        {
            //TODO: jumping over it will kill the enemy ??
            hit.transform.GetComponent<EnemyController>().TakeDamage(10);
            rigidbody.AddForce(new Vector2(0f, jumpForce));
        }
        
        if (hit.collider != null)
        {
            canDash = true;
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
        bool dash = dashReleased && Input.GetAxis("Dash") > 0;
        Vector3 vForce = Vector3.up * Input.GetAxis("Vertical");
        Vector3 hForce = Vector3.right * Input.GetAxis("Horizontal");
        dashDirection = hForce.normalized + vForce.normalized;
        if (canDash && dash && Math.Abs(dashDirection.magnitude) > 0)
        {
            canDash  = false;
            dashReleased = false;
            currentDashTime = dashTime;
            isDashing = true;
            Physics2D.IgnoreLayerCollision(11, 10, true);//TODO: Get layer by name
        }
        
        if (currentDashTime >= 0)
        {
            rigidbody.velocity = dashDirection * dashVelocity;
            currentDashTime -= Time.deltaTime;
        }
        else if (isDashing)
        {
            rigidbody.velocity = Vector3.zero;
            isDashing = false;
            Physics2D.IgnoreLayerCollision(11, 10, false);//TODO: Get layer by name
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

    void CheckForEnemies()
    {
        if (isDashing)
        {
            RaycastHit2D hit = Physics2D.Raycast(collider.transform.position, Vector2.right, 0.5f, enemyLayer);
            if (hit.collider != null)
            {
                hit.transform.GetComponent<EnemyController>().TakeDamage(10);
            }
        }
    }

    void KillPlayer()
    {
        GameLoopManager.instance.GameOver();
    }

}
