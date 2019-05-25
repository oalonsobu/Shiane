using System.Collections;
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
        if (currentcdTime > 0)
        {
            currentcdTime -= Time.deltaTime;
            GameLoopManager.instance.UpdateFireballCounter(currentcdTime);
        }
        else if (shootInCD)
        {
            currentcdTime = 0;
            GameLoopManager.instance.UpdateFireballCounter(currentcdTime);
            shootInCD = false;
        }
    }

    void Shoot()
    {
        bool shoot = Input.GetAxis("Fire") > 0;
        if (shoot && !shootInCD)
        {
            shootInCD = true;
            currentcdTime = cdTime;
            GameLoopManager.instance.UpdateFireballCounter(currentcdTime);
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(fireballPrefab, transform.position, rotation);
        }
    }
}