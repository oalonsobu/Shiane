using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour {

    AudioSource audioSource;
    AudioClip clickClip;

	void Start () {
	    audioSource = GetComponent<AudioSource>();
        clickClip   = Resources.Load<AudioClip>("Sounds/Click");
    }
	

    public void PlaySound(AudioClip audio) {
        if (audioSource == null || audio == null) return;

        audioSource.PlayOneShot(audio);
    }
    
    //It's very common so we use this a util class
    public void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clickClip);
        }   
    }
}
