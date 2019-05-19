using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{

    Rigidbody2D rigidbody;
    ShieldPowerController shieldPowerController;
    Transform initPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody             = gameObject.GetComponent<Rigidbody2D>();
        shieldPowerController = gameObject.GetComponent<ShieldPowerController>();
        initPoint = GameObject.FindWithTag("InitialPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFallDeath();
    }
    
    public void KillPlayer(bool ignoreShield)
    {
        if (!shieldPowerController.IsActive() || ignoreShield)
        {
            GameLoopManager.instance.GameOver();
        }
    }
    
    public void Respawn()
    {
        gameObject.transform.position = initPoint.position;
        gameObject.transform.rotation = initPoint.rotation;
        Camera.main.GetComponent<ParallaxController>().InitCamera();
    }

    void CheckFallDeath()
    {
        if (rigidbody.position.y < -2)
        {
            KillPlayer(true);
        }
    }
}
