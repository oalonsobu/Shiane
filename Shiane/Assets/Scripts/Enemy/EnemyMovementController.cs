﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{

    [SerializeField] bool canMove = true;
    [Range(0, 5f)] [SerializeField] float enemySpeed = 2.5f;
    
    GameObject player;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collider;
    bool canGoForward = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
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
    }

    void UpdateStatus()
    {
        if (canMove)
        {
            canGoForward = true; //By default, he can move
        }
        
        //TODO: maybe two raycast, one in each side of the sprite...
        Vector3 pos = collider.transform.position;
        var layerMask = (1 << 9) | (1 << 12) | (1 << 11);
        RaycastHit2D hit = Physics2D.Raycast(pos, spriteRenderer.flipX ? Vector2.right: Vector2.left, collider.size.x, layerMask);
        if (hit.collider != null)
        {
            canGoForward = false;
        }
        else
        {
            pos += new Vector3(collider.size.x / 4 * (spriteRenderer.flipX ? 1 : -1), 0, 0);
            hit = Physics2D.Raycast(pos, Vector3.down, collider.size.y, layerMask);
            if (hit.collider == null)
            {
                canGoForward = false;
            }
        }
    }
}