using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePowerController : MonoBehaviour
{
    [SerializeField] GameObject fireballPrefab;

    bool shootInCD = false;
    
    void Update()
    {
        bool shoot = Input.GetAxis("Fire1") > 0;
        if (shoot && !shootInCD)
        {
            shootInCD = true;
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Debug.Log(position);
            Instantiate(fireballPrefab, transform.position, rotation);
        }
    }
}
