using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    [Range(0, 5f)] [SerializeField] float playerSpeed = 5f;
    [Range(0, .3f)] [SerializeField] float movementSmoothing = .3f;

    Vector3 currentVelocity;
    Rigidbody2D rigidbody;
    bool isFacingRight = true;

    void Start ()
    {
        currentVelocity = Vector2.zero;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        Vector2 desiredVelocity = Vector2.right * movement * playerSpeed;
        rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, desiredVelocity, ref currentVelocity, movementSmoothing);
    }

}
