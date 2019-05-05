using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    
    private List<SpriteRenderer> parallaxLayers;
    [SerializeField] GameObject backLayer;
    [SerializeField] GameObject frontLayer;
    Vector3 mainCameraLastPosition;
    float frontLayerSpeed = 3.5f;
    float backLayerSpeed  = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCameraLastPosition = Camera.main.transform.position;
        InitLayer(backLayer);
        InitLayer(frontLayer);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLayer(backLayer, backLayerSpeed);
        UpdateLayer(frontLayer, frontLayerSpeed);
        
        mainCameraLastPosition = Camera.main.transform.position;
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
            float currentPos = 2 * extents * counter;
            child.position = new Vector3(currentPos, yPosition, layer.transform.position.z);
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
}
