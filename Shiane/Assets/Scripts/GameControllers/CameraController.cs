using System;
using UnityEngine; 
using System.Collections; 

[RequireComponent (typeof(SpriteRenderer))] 
public class CameraController : MonoBehaviour { 

    [Range(0, 10f)] [SerializeField] float zDistance = 10f;
    
    public int offsetX = 2;
    private float spriteWidth = 0f; 
    private Camera cam; 
    Vector3 currentVelocity = Vector3.zero;

    void Awake()
    {
        cam = Camera.main; 
    }

    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>(); 
        spriteWidth = sRenderer.sprite.bounds.size.x;
    } 
    
    //Need to be in the same update as character movement ;)
    void FixedUpdate ()
    {
        Vector3 playerPosition = transform.position;
        Vector3 cameraPosition = cam.transform.position;
        
        //To avoid following the player falling
        if (cameraPosition.y >= -0.5f)
        {
            playerPosition.z -= zDistance;
            float smoothTime = 1 - Math.Min(Math.Abs(playerPosition.x - cameraPosition.x), 0.95f);
            Vector3 tempPosition = Vector3.SmoothDamp(cameraPosition, playerPosition, ref currentVelocity, 0.1f);
            cam.transform.position = tempPosition;
        }
    } 
}
﻿