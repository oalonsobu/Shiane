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
        if (currentcdTime > 0)
        {
            currentcdTime -= Time.deltaTime;
            GameLoopManager.instance.UpdateShieldCounter(currentcdTime);
        }
        else if (shieldInCD)
        {
            currentcdTime = 0;
            GameLoopManager.instance.UpdateShieldCounter(currentcdTime);
            shieldInCD = false;
        }

        if (cdTime - activeTime > currentcdTime)
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
            currentcdTime = cdTime;
            GameLoopManager.instance.UpdateShieldCounter(currentcdTime);
            shieldGO.SetActive(true);
        }
    }

    public bool IsActive()
    {
        return shieldGO.activeSelf;
    }
}
