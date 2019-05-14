using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 10) //TODO: Get layer by name
        {
            col.gameObject.GetComponent<EnemyHealthController>().TakeDamage(10);
            Destroy(gameObject);
        }
    }
    
}
