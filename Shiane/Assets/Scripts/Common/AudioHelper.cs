using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour {

    AudioSource audioSource;

	void Start () {
	    audioSource = GetComponent<AudioSource>();
    }
	

    public void playSound(AudioClip audio) {
        if (audioSource == null || audio == null) return;

        audioSource.clip = audio;
        audioSource.Play();
    }
}
