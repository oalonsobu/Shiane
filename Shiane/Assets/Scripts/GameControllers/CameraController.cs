using System;
using UnityEngine; 
using System.Collections; 

[RequireComponent (typeof(SpriteRenderer))] 
public class CameraController : MonoBehaviour { 

    [Range(0, 10f)] [SerializeField] float zDistance = 10f;
    

    private Camera cam; 
    Vector3 currentVelocity = Vector3.zero;

    void Awake()
    {
        cam = Camera.main; 
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
            Vector3 tempPosition = Vector3.SmoothDamp(cameraPosition, playerPosition, ref currentVelocity, 0.1f);
            cam.transform.position = tempPosition;
        }
    } 
}
﻿