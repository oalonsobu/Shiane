using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    float playerSpeed;

    void Start ()
    {
        playerSpeed = 5;
    }

    void Update()
    {
        move();
    }

    void move()
    {
        Vector2 vectorDirection = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.D))
        {
            vectorDirection = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            vectorDirection = Vector2.left;
        }

        transform.Translate(vectorDirection * playerSpeed * Time.deltaTime, Space.World);
    }

}
