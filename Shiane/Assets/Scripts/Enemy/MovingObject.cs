using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    
    [SerializeField] Transform[] positions;
    [Range(0, 5f)] [SerializeField] float speed = 1f;
    [Range(0, 5f)] [SerializeField] float reposetime = 1f;

    bool stopped = false;
    Vector3 currentVelocity = Vector3.zero;
    int current = 0;

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            Move();

            if (Vector3.Distance(transform.position, positions[current].position) < 0.1 && !stopped)
            {
                StartCoroutine(RestartMovement());
            }
        }
    }

    void Move()
    {
        transform.position = Vector3.SmoothDamp(transform.position, positions[current].position, ref currentVelocity, 0.1f, speed);
    }
    
    IEnumerator RestartMovement()
    {
        stopped = true;
        yield return new WaitForSeconds (reposetime);
        current++;  
        
        if (current >= positions.Length)
        {
            current = 0;
        }

        stopped = false;
    }
}
