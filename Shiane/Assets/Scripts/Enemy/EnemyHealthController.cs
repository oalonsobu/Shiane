using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{

    [Range(0, 100)] [SerializeField] int health = 10;
    
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        Destroy(gameObject);
    }
}
