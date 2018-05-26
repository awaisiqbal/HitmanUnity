using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnSound : MonoBehaviour {

    public AudioSource sonido;
	// Use this for initialization
	void Awake () {
        sonido = GetComponent<AudioSource>();
	}
	
	public void playSound()
    {
        sonido.Play();
    }
}
