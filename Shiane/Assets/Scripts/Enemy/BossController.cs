﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    [SerializeField] GameObject arrowPrefab;

    List<GameObject> arrowPositions = new List<GameObject>();

    float currentHealth;
    [SerializeField] float maxHealth = 100;
    [SerializeField] Slider healthUI;
    
    float currentShield;
    [SerializeField] float maxShield = 100;
    [SerializeField] Slider shieldUI;
    bool isShieldUp = true;
    
    bool shootArrowsInCD = false;
    float shootArrowsCdTime = .5f;
    float shootArrowsCurrentcdTime = .0f;
    
    
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
        Debug.Log(currentShield);
        UpdateCD();
        if (!shootArrowsInCD)
        {
            //ShootArrows();  
        }
    }

    void UpdateCD()
    {
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
    
    void ShootArrows()
    {
        shootArrowsInCD = true;
        shootArrowsCurrentcdTime = shootArrowsCdTime;
        foreach (GameObject arrowPosition in arrowPositions)
        {
            Instantiate(arrowPrefab, arrowPosition.transform.position, arrowPosition.transform.rotation);
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != 14) //TODO: Get layer by name
        {

        }
    }

    public void TakeFireballDamage(int damage)
    {
        if (currentShield > 0) //TODO: maybe shield is up or something like that
        {
            currentShield -= damage;
            shieldUI.value = currentShield;
        }

        if (currentShield <= 0)
        {
            isShieldUp = false;
        }
    }
    
    public void TakeDashDamage(int damage)
    {
        if (currentHealth > 0) //TODO: maybe shield is up or something like that
        {
            currentHealth -= damage;
            healthUI.value = currentHealth;
        }
    }
    
    public bool IsShieldUp()
    {
        return isShieldUp;
    }
}