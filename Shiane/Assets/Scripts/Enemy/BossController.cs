using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BossController : MonoBehaviour
{

    [SerializeField] LayerMask groundLayer;
    
    enum BossPhase
    {
        Shooting = 0,
        GoingDown = 1,
        GoingUp = 2,
        Down = 3
    };
    
    [SerializeField] GameObject arrowPrefab;

    List<GameObject> arrowPositions = new List<GameObject>();

    float currentHealth;
    [SerializeField] float maxHealth = 100;
    [SerializeField] Slider healthUI;
    
    float currentShield;
    [SerializeField] float maxShield = 100;
    [SerializeField] Slider shieldUI;
    bool isShieldUp = true;
    
    //Shooting phase vars
    bool shootArrowsInCD = false;
    float shootArrowsCdTime = .5f;
    float shootArrowsCurrentcdTime = .0f;

    //GoingDown phase vars
    Vector3 reposePosition = Vector3.zero;
    Quaternion reposeRotation = Quaternion.identity;
    
    BossPhase currentPhase;
    
    // Start is called before the first frame update
    void Start()
    {
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
        }
    }
    
    void UpdateGoingDownPhase()
    {
        if (reposePosition != Vector3.zero)
        {
            transform.position = reposePosition;
            transform.rotation = reposeRotation;
            
            if (transform.position == reposePosition)
            {
                ChangePhase(BossPhase.Down);
                Debug.Log(currentPhase);
            }
        }
    }
    
    void CalculateReposePosition()
    {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
        
        if (hit.collider != null)
        {
            reposePosition = hit.point;
            reposeRotation = transform.rotation;
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
            //ShootArrows();  
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
    }

    void RefillShield()
    {
        if (!IsShieldUp())
        {
            currentShield += 1f * Time.deltaTime;
            shieldUI.value = currentShield;
            if (currentShield >= 100)
            {
                currentShield = 100;
                isShieldUp = true;
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
        }

        if (currentShield <= 0)
        {
            ChangePhase(BossPhase.GoingDown);
        }
    }
    
    public void TakeDashDamage(int damage)
    {
        if (!IsShieldUp())
        {
            currentHealth -= damage;
            healthUI.value = currentHealth;
            //Refill shield completely
            currentShield = 100;
            isShieldUp = true;
            shieldUI.value = currentShield;
            ChangePhase(BossPhase.GoingUp);
        }
    }

    void ChangePhase(BossPhase bossPhase)
    {
        switch (bossPhase)
        {
            case BossPhase.GoingUp:
                currentPhase = BossPhase.GoingUp;
                reposePosition = Vector3.zero;
                break;
            case BossPhase.GoingDown:
                isShieldUp = false;
                currentPhase = BossPhase.GoingDown;
                CalculateReposePosition();
                break;
            case BossPhase.Down:
                currentPhase = BossPhase.Down;
                break;
        }
    }
    
    public bool IsShieldUp()
    {
        return isShieldUp;
    }
}
