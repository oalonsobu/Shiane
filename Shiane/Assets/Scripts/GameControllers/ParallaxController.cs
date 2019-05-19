using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] GameObject backLayer;
    [SerializeField] GameObject frontLayer;
    [SerializeField] GameObject cloudLayer;
    Vector3 mainCameraLastPosition;
    float cloudLayerSpeed = 6.5f;
    float frontLayerSpeed = 3.5f;
    float backLayerSpeed  = 2f;

    private List<GameObject> alreadyClonedClouds;

    void Start()
    {
        InitCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLayer(backLayer, backLayerSpeed);
        UpdateLayer(frontLayer, frontLayerSpeed);
        UpdateCloudLayer(cloudLayerSpeed * 3);
        
        mainCameraLastPosition = Camera.main.transform.position;
    }
    
    public void InitCamera()
    {
        alreadyClonedClouds = new List<GameObject>();
        Vector3 position = GameObject.FindWithTag("Player").transform.position;
        position.z = Camera.main.transform.position.z;
        Camera.main.transform.position = position;
        mainCameraLastPosition = Camera.main.transform.position;
        InitLayer(backLayer);
        InitLayer(frontLayer);
        InitCloudLayer();
    }

    bool IsInLeftSideScreen(float pos, float size)
    {
        //Get the most left point in the screen
        float screenRatio = Camera.main.orthographicSize * Screen.width / Screen.height;
        float minX = Camera.main.transform.position.x - screenRatio;
        return minX > pos + size;
    }
    
    bool IsInRightSideScreen(float pos, float size)
    {
        //Get the most right point in the screen
        float screenRatio = Camera.main.orthographicSize * Screen.width / Screen.height;
        float maxX = Camera.main.transform.position.x + screenRatio;
        return maxX < pos - size;
    }

    void InitLayer(GameObject layer)
    {
        float yPosition = layer.transform.position.y - Camera.main.orthographicSize;
        int counter = 0;
        foreach (Transform child in layer.transform)
        {
            float extents = child.GetComponent<SpriteRenderer>().bounds.extents.x;
            float currentPos = Camera.main.transform.position.x + 2 * extents * counter;
            child.position = new Vector3(currentPos, yPosition, layer.transform.position.z);
            counter++;
        }
    }
    
    //This is a bit different
    void InitCloudLayer()
    {
        float maxYPosition    = cloudLayer.transform.position.y + Camera.main.orthographicSize;
        float cloudNumber     = cloudLayer.transform.childCount;
        float halfCameraXSize = (Camera.main.orthographicSize * Screen.width / Screen.height) / cloudNumber;
        int counter           = 0;
        foreach (Transform child in cloudLayer.transform)
        {
            float extents = child.GetComponent<SpriteRenderer>().bounds.extents.x;
            float xMin = halfCameraXSize * 2 * counter;
            float xMax = halfCameraXSize * 2 * (counter + 1) - extents/2;
            float xPos = Random.Range(xMin, xMax);
            float yPos = Random.Range(cloudLayer.transform.position.y, maxYPosition);
            child.position = new Vector3((cloudLayer.transform.position.x - halfCameraXSize) + xPos, yPos, cloudLayer.transform.position.z);
            counter++;
        }
    }

    void UpdateLayer(GameObject layer, float speed)
    {
        float cameraDistanceTravelled = mainCameraLastPosition.x - Camera.main.transform.position.x;
        foreach (Transform child in layer.transform)
        {
            float extents = child.GetComponent<SpriteRenderer>().bounds.extents.x;
            float currentPos = child.position.x;
            currentPos += Time.deltaTime * speed * cameraDistanceTravelled;
            if (IsInLeftSideScreen(currentPos, extents) && cameraDistanceTravelled < 0)
            {
                currentPos += 4 * extents;
            } else if (IsInRightSideScreen(currentPos, extents) && cameraDistanceTravelled > 0)
            {
                currentPos -= 4 * extents;
            }
            child.position = new Vector3(currentPos, child.position.y, child.position.z);
        }
    }
    
    void UpdateCloudLayer(float speed)
    {
        float cameraDistanceTravelled = mainCameraLastPosition.x - Camera.main.transform.position.x;
        foreach (Transform child in cloudLayer.transform)
        {
            float extents = child.GetComponent<SpriteRenderer>().bounds.extents.x;
            float currentPos = child.position.x;
            currentPos += Time.deltaTime * speed * cameraDistanceTravelled;
            if (IsInLeftSideScreen(currentPos, extents) && cameraDistanceTravelled < 0)
            {
                if (child.GetComponent<SpriteRenderer>().enabled)
                {
                    InstantiateCloud(child.gameObject);
                    child.GetComponent<SpriteRenderer>().enabled = false;
                }
            } else if (IsInRightSideScreen(currentPos, extents) && cameraDistanceTravelled > 0)
            {
                if (child.GetComponent<SpriteRenderer>().enabled)
                {
                    child.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if (!IsInLeftSideScreen(currentPos, extents) && !IsInRightSideScreen(currentPos, extents))
            {
                child.GetComponent<SpriteRenderer>().enabled = true;
            }
            child.position = new Vector3(currentPos, child.position.y, child.position.z);
        }
    }

    void InstantiateCloud(GameObject original)
    {
        if (!alreadyClonedClouds.Contains(original))
        {
            //Calculates all variables to get the new position to clone the cloud (randomly)
            GameObject cloud = Instantiate(original, cloudLayer.transform);
            float maxYPosition = cloudLayer.transform.position.y + Camera.main.orthographicSize;
            float screenRatio = Camera.main.orthographicSize * Screen.width / Screen.height;
            float extents = original.GetComponent<SpriteRenderer>().bounds.extents.x;
            float minX = Camera.main.transform.position.x + screenRatio + extents;
        
            float yPos = Random.Range(cloudLayer.transform.position.y, maxYPosition);
            float xPos = Random.Range(minX, minX + 0.25f);
            cloud.transform.position = new Vector3(xPos, yPos, cloudLayer.transform.position.z);
            alreadyClonedClouds.Add(original);
        }
    }
}
