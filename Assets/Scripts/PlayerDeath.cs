using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
	// reference to AnimatorController
    public Animator playerAnimController;
    //sound
    public AudioClip deathSound;
     AudioSource sound;

    // string id for PlayerDeath trigger parameter
    public string playerDeathTrigger = "IsDead";

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    // play the death animation
    public void Die()
    {
        sound.clip = deathSound;
        sound.Play();
        if (playerAnimController != null)
        {
            playerAnimController.SetTrigger(playerDeathTrigger);
        }
    }

}
