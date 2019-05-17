﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePowerController : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;

    bool shootInCD = false;
    float cdTime = 1.5f;
    float currentcdTime = 0;

    void Update()
    {
        UpdateCD();
        Shoot();
    }

    void UpdateCD()
    {
        if (currentcdTime < cdTime)
        {
            currentcdTime += Time.deltaTime;
        }
        else if (shootInCD)
        {
            shootInCD = false;
        }
    }

    void Shoot()
    {
        bool shoot = Input.GetAxis("Fire1") > 0;
        if (shoot && !shootInCD)
        {
            shootInCD = true;
            currentcdTime = 0;
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(fireballPrefab, transform.position, rotation);
        }
    }
}