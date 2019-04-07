﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    [Range(0, 5f)] [SerializeField] float playerSpeed = 5f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .3f;
    [Range(200, 450)] [SerializeField] int jumpForce = 400;
    [SerializeField] LayerMask groundLayer = 400;

    Vector3 currentVelocity;
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
        grounded = UpdateGrounded();
        Debug.Log(grounded);
        Move();
    }

    bool UpdateGrounded()
    {
        //TODO: maybe two raycast, one in each side of the sprite...
        RaycastHit2D hit = Physics2D.Raycast(collider.transform.position, Vector2.down, colliderSizeRaycast, groundLayer);
        return hit.collider != null;
    }

    void Move()
    {
        bool jump = Input.GetAxis("Jump") > 0 ;
        if (grounded && jump)
        {
            rigidbody.AddForce(new Vector2(0f, jumpForce));
            grounded = false;
        }
        
        float movementForce = Input.GetAxis("Horizontal");
        Vector2 desiredVelocity = Vector2.right * movementForce * playerSpeed;
        rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, desiredVelocity, ref currentVelocity, movementSmoothing);
    }

}
