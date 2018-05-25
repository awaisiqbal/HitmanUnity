using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{
    // reference to visual arrows
    PlayerCompass m_playerCompass;
    //move sound
    public AudioClip moveSound;
    AudioSource sound;

    // invoke the base class Awake method and setup the PlayerMover
    protected override void Awake()
    {
        base.Awake();
        m_playerCompass = Object.FindObjectOfType<PlayerCompass>().GetComponent<PlayerCompass>();
        sound = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
		UpdateBoard();
    }

    // update the Board's PlayerNode
    void UpdateBoard()
    {
        if (m_board != null)
        {
            m_board.UpdatePlayerNode();
        }
    }

    protected override IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        // disable PlayerCompass arrows
		if (m_playerCompass != null)
		{
			m_playerCompass.ShowArrows(false);
		}

        //play sound
        sound.clip = moveSound;
        sound.Play();
        // run the parent class MoveRoutine
        
        yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));

        // update the Board's PlayerNode
		UpdateBoard();

        // enable PlayerCompass arrows
		if (m_playerCompass != null)
		{
			m_playerCompass.ShowArrows(true);
		}

        // broadcast message at the end of movement
        base.finishMovementEvent.Invoke();
    }
}
