﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BossController : MonoBehaviour
{

    [SerializeField] LayerMask groundLayer;
    Animator animator;
    
    enum BossPhase
    {
        Shooting = 0,
        GoingDown = 1,
        GoingUp = 2,
        Down = 3
    };

    //Health vars
    float currentHealth;
    [SerializeField] float maxHealth = 100;
    [SerializeField] Slider healthUI;
    
    //Shield vars
    [SerializeField] GameObject shieldChild;
    CircleCollider2D shieldCollider2D;
    float currentShield;
    [SerializeField] float maxShield = 100;
    [SerializeField] Slider shieldUI;
    bool isShieldUp = true;
    
    //Shooting phase vars
    [SerializeField] GameObject arrowPrefab;
    List<GameObject> arrowPositions = new List<GameObject>();
    bool shootArrowsInCD = false;
    float shootArrowsCdTime = .75f;
    float shootArrowsCurrentcdTime = .0f;
    Vector3 positionToGo = Vector3.zero;
    float levitatingSpeed = 1f;

    //GoingDown phase vars
    Vector3 reposePosition = Vector3.zero;
    float fallingSpeed = 1.25f;
    
    //GoingUp phase vars
    Vector3 attackPosition;
    float goingUpSpeed = 2.5f;
    
    //Down phase vars
    [SerializeField] GameObject shockwavePrefab;
    
    
    BossPhase currentPhase;
    
    // Start is called before the first frame update
    void Start()
    {
        shieldCollider2D = gameObject.GetComponent<CircleCollider2D>();
        currentHealth = maxHealth;
        currentShield = maxShield;
        shieldUI.maxValue = maxShield;
        shieldUI.value = currentShield;
        healthUI.maxValue = maxHealth;
        healthUI.value = currentShield;
        Transform parentArrow = transform.Find("ArrowPositions");
        if (parentArrow != null)
        {
            foreach (Transform arrowPosition in parentArrow)
            {
                arrowPositions.Add(arrowPosition.gameObject);
            }
        }

        attackPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentPhase)
        {
            case BossPhase.Down:
                UpdateDownPhase();
                break;
            case BossPhase.Shooting:
                UpdateShootingPhase();
                break;
            case BossPhase.GoingDown:
                UpdateGoingDownPhase();
                break;
            case BossPhase.GoingUp:
                UpdateGoingUpPhase();
                break;
        }
    }
    
    void UpdateGoingUpPhase()
    {
        if (attackPosition != Vector3.zero)
        {
            Vector3 direction = attackPosition - transform.position;
            transform.Translate(direction.normalized * goingUpSpeed * Time.deltaTime);
            if (AlreadyInPosition(attackPosition) || attackPosition.y < transform.position.y)
            {
                ChangePhase(BossPhase.Shooting);
            }
        }
    }
    
    void UpdateGoingDownPhase()
    {
        if (reposePosition != Vector3.zero)
        {
            Vector3 direction = reposePosition - transform.position;
            transform.Translate(direction.normalized * fallingSpeed * Time.deltaTime);
            
            if (AlreadyInPosition(reposePosition) || transform.position.y < reposePosition.y)
            {
                ChangePhase(BossPhase.Down);
            }
        }
    }
    
    void CalculateReposePosition()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        
        if (hit.collider != null)
        {
            reposePosition = hit.point;
            reposePosition.y += gameObject.GetComponent<CircleCollider2D>().radius - 0.25f;
        }
    }

    void UpdateDownPhase()
    {
        RefillShield();
    }
    
    void UpdateShootingPhase()
    {
        if (!shootArrowsInCD)
        {
            ShootArrows();  
        }
        
        if (shootArrowsInCD)
        {
            shootArrowsCurrentcdTime -= Time.deltaTime;
            if (shootArrowsCurrentcdTime < 0)
            {
                shootArrowsCurrentcdTime = 0;
                shootArrowsInCD = false;
            }
        }

        if (positionToGo == Vector3.zero || AlreadyInPosition(positionToGo))
        {
            UpdateRandomPositionToGo();
        }

        if (positionToGo != Vector3.zero)
        {
            Vector3 direction = positionToGo - transform.position;
            transform.Translate(direction.normalized * levitatingSpeed * Time.deltaTime);
        }
    }

    void RefillShield()
    {
        if (!IsShieldUp())
        {
            currentShield += 5f * Time.deltaTime;
            shieldUI.value = currentShield;
            if (currentShield >= 100)
            {
                ChangePhase(BossPhase.GoingUp);
            }
        }
    }
    
    void ShootArrows()
    {
        if (IsShieldUp())//Maybe we want to add some more states, but for now this works
        {
            shootArrowsInCD = true;
            shootArrowsCurrentcdTime = shootArrowsCdTime;
            foreach (GameObject arrowPosition in arrowPositions)
            {
                Instantiate(arrowPrefab, arrowPosition.transform.position, arrowPosition.transform.rotation);
            }
        }
    }

    public void TakeFireballDamage(int damage)
    {
        if (IsShieldUp())
        {
            currentShield -= damage;
            shieldUI.value = currentShield;
            
            if (currentShield <= 0)
            {
                ChangePhase(BossPhase.GoingDown);
            }
        }
    }
    
    public void TakeDashDamage(int damage)
    {
        if (!IsShieldUp() && (currentPhase == BossPhase.Down || currentPhase == BossPhase.GoingDown))
        {
            currentHealth -= damage;
            healthUI.value = currentHealth;
            if (currentHealth <= 0)
            {
                GameLoopManager.instance.EndGame();
            }
            else
            {
                if (shockwavePrefab != null)
                {
                    Instantiate(shockwavePrefab, transform);   
                }
                ChangePhase(BossPhase.GoingUp);
            }
        }
    }

    void ChangePhase(BossPhase bossPhase)
    {
        switch (bossPhase)
        {
            case BossPhase.GoingUp:
                currentPhase = BossPhase.GoingUp;
                reposePosition = Vector3.zero;
                //Refill shield completely
                currentShield = 100;
                shieldUI.value = currentShield;
                shieldChild.SetActive(true);
                shieldCollider2D.enabled = true;
                break;
            case BossPhase.GoingDown:
                isShieldUp = false;
                currentPhase = BossPhase.GoingDown;
                shieldChild.SetActive(false);
                shieldCollider2D.enabled = false;
                CalculateReposePosition();
                break;
            case BossPhase.Down:
                currentPhase = BossPhase.Down;
                break;
            case BossPhase.Shooting:
                isShieldUp = true;
                currentPhase = BossPhase.Shooting;
                break;
        }
        animator.SetInteger("Phase", (int) currentPhase);
    }
    
    public bool IsShieldUp()
    {
        return isShieldUp;
    }

    bool AlreadyInPosition(Vector3 destination)
    {
        return Vector3.Distance(transform.position, destination) < 0.1f;
    }

    void UpdateRandomPositionToGo()
    {
        float x    = Random.Range(1f, 3f);
        int sign   = Random.Range(0, 2);

        positionToGo = transform.position;
        positionToGo.x = positionToGo.x + x * (sign == 0 ? -1 : 1);

        if (positionToGo.x < -5) positionToGo.x += 10;
        if (positionToGo.x > 5) positionToGo.x -= 10;
    }
}
