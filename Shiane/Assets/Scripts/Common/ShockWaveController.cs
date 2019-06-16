using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveController : MonoBehaviour {

    float lifeTime;
    float currentLifeTime;
    float speed;

    // Use this for initialization
    void Start () {

        lifeTime = 0.5f;
        currentLifeTime = 0;
        speed = 2.5f;

        transform.localScale = new Vector3(0.1f,0.1f,0.1f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += (new Vector3(1, 1, 1)) * speed * Time.deltaTime;
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= lifeTime) {
            Destroy(gameObject);
        }
    }
}
