using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 10 && !col.gameObject.CompareTag("Boss")) //TODO: Get layer by name
        {
            col.gameObject.GetComponent<EnemyHealthController>().TakeDamage(10);
            Destroy(gameObject);
        } else if (col.gameObject.CompareTag("Boss"))
        {
            col.gameObject.GetComponent<BossController>().TakeFireballDamage(10);
            Destroy(gameObject);
        }
    }
    
}
