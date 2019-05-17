using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerController : MonoBehaviour
{
    [SerializeField] GameObject shieldGO;
    
    bool shieldInCD = false;
    float cdTime = 1.5f;
    float activeTime = .25f;
    float currentcdTime = 0;

    void Update()
    {
        UpdateCD();
        ActivateShield();
    }

    void UpdateCD()
    {
        if (currentcdTime < cdTime)
        {
            currentcdTime += Time.deltaTime;
        }
        else if (shieldInCD)
        {
            shieldInCD = false;
        }

        if (activeTime < currentcdTime)
        {
            shieldGO.SetActive(false);
        }
    }

    void ActivateShield()
    {
        bool shield = Input.GetAxis("Fire2") > 0;
        if (shield && !shieldInCD)
        {
            shieldInCD = true;
            currentcdTime = 0;
            shieldGO.SetActive(true);
        }
    }

    public bool IsActive()
    {
        return currentcdTime < activeTime;
    }
}
