using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    [Range(0, 5f)] [SerializeField] float speed = 5f;

    SpriteRenderer spriteRenderer;
    
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        if (IsOutsideScreen(transform.position.x, spriteRenderer.size.x))
        {
            Destroy(gameObject);
        }
    }
    
    bool IsOutsideScreen(float pos, float size)
    {
        //Get the most left point in the screen
        float screenRatio = Camera.main.orthographicSize * Screen.width / Screen.height;
        float minX = Camera.main.transform.position.x - screenRatio;
        float maxX = Camera.main.transform.position.x + screenRatio;
        return minX > pos + size || maxX < pos - size;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.layer);
        if (col.gameObject.layer != 11 && col.gameObject.layer != 10) //TODO: Get layer by name
        {
            Destroy(gameObject);
        }
    }
    
}
