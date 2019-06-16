using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    
    AudioClip damageClip;
    AudioHelper audioHelper;

    Rigidbody2D rigidbody;
    ShieldPowerController shieldPowerController;
    Transform initPoint;

    int deathCounter;
    bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody             = gameObject.GetComponent<Rigidbody2D>();
        shieldPowerController = gameObject.GetComponent<ShieldPowerController>();
        initPoint             = GameObject.FindWithTag("InitialPoint").transform;
        deathCounter = 0;
        
        damageClip  = Resources.Load<AudioClip>("Sounds/Ouch");
        audioHelper = GetComponent<AudioHelper>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckFallDeath();
    }
    
    public void KillPlayer(bool ignoreShield)
    {
        if (isDead) return;
        if (!shieldPowerController.IsActive() || ignoreShield)
        {
            DestroyAllArrows();
            isDead = true;
            audioHelper.PlaySound(damageClip);
            GameLoopManager.instance.GameOver();
            deathCounter++;
            GameLoopManager.instance.UpdateDeathCounter(deathCounter);
        }
    }
    
    public void Respawn()
    {
        gameObject.transform.position = initPoint.position;
        gameObject.transform.rotation = initPoint.rotation;
        rigidbody.position = transform.position; //Avoid dying twice (falling) because the rigidbody does not update instantly
        Camera.main.GetComponent<ParallaxController>().InitCamera();
        isDead = false;
    }

    void CheckFallDeath()
    {
        if (rigidbody.position.y < -2 && !isDead)
        {
            Debug.Log(rigidbody.position.y);
            KillPlayer(true);
        }
    }
    
    void DestroyAllArrows()
    {
        var gameObjects = GameObject.FindGameObjectsWithTag("Arrow");
         
        foreach(var gameObject in gameObjects)
        {
            Destroy(gameObject);
        }
    }
}
