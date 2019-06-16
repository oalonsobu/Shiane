using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePowerController : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;

    [SerializeField]
    bool fireballActive = true;
    
    AudioClip fireballClip;
    AudioHelper audioHelper;

    bool shootInCD = false;
    float cdTime = 1.5f;
    float currentcdTime = 0;

    void Start()
    {
        fireballClip = Resources.Load<AudioClip>("Sounds/Foom");
        audioHelper  = GetComponent<AudioHelper>();
    }

    void Update()
    {
        if (!fireballActive) return;
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
            float hDir = Input.GetAxis("HorizontalJoystick");
            float vDir = Input.GetAxis("VerticalJoystick");
            Vector3 dir = Vector3.zero;
            if (hDir == 0) //Means that there is no controller
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                dir = position - transform.position;
            }
            else
            {
                dir.x = hDir;
                dir.y = -vDir;
                dir.Normalize();
            }
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Instantiate(fireballPrefab, transform.position, rotation);
            audioHelper.PlaySound(fireballClip);
        }
    }
}