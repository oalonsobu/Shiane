using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 13) //TODO: Get layer by name
        {
            Destroy(col.gameObject);
        }
    }
}
