﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerDeath))]
public class PlayerManager : TurnManager
{
    // reference to PlayerMover and PlayerInput components
    public PlayerMover playerMover;
    public PlayerInput playerInput;

    public GameObject loseSound;

    // reference to Board component
    Board m_board;

    // messages to send when the Player dies
    public UnityEvent deathEvent;

    public Vector3 playerLastPos;

    bool m_PlayerDead = false;

    protected override void Awake()
    {
        base.Awake();

        // cache references to PlayerMover and PlayerInput
        playerMover = GetComponent<PlayerMover>();
        playerInput = GetComponent<PlayerInput>();

        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();

        // make sure that input is enabled when we begin
        playerInput.InputEnabled = true;
        playerLastPos = transform.position;
    }

    void Update()
    {
        // if the player is currently moving or if it's not the Player's turn, ignore user input
        if (playerMover.isMoving || m_gameManager.CurrentTurn != Turn.Player || m_gameManager.IsGameOver)
        {
            Debug.Log("GAME ENDED");
            return;
        }

        // get keyboard input
        playerInput.GetKeyInput();

        // connect user input with PlayerMover's Move methods
        if (playerInput.V == 0)
        {
            if (playerInput.H < 0)
            {
                playerMover.MoveLeft();
            }
            else if (playerInput.H > 0)
            {
                playerMover.MoveRight();
            }
        }
        else if (playerInput.H == 0)
        {
            if (playerInput.V < 0)
            {
                playerMover.MoveBackward();
            }
            else if (playerInput.V > 0)
            {
                playerMover.MoveForward();
            }
        }
    }

    // invoke any UnityActions on the deathEvent
    public void Die()
    {
        playerInput.InputEnabled = false;
        if (deathEvent != null)
        {
            if (!m_PlayerDead)
            {
                m_PlayerDead = true;
                loseSound.GetComponent<AudioSource>().Play();
            }
           
            deathEvent.Invoke();
        }
    }

    // capture any enemies on the PlayerNode
    void CaptureEnemies()
    {
        if (m_board != null)
        {
            // all enemies on the PlayerNode
            List<EnemyManager> enemies = m_board.FindEnemiesAt(m_board.PlayerNode);

            // if we find at least one enemy...
            if (enemies.Count != 0)
            {
                // ...invoke the Die method on each one
                foreach (EnemyManager enemy in enemies)
                {
                    if (enemy != null)
                    {
                        if ((playerLastPos == enemy.NextNodePos))
                        {
                            Debug.Log("Die trying to atack enemie");
                            m_gameManager.playerDiedByHostile();
                        }
                        else
                        {
                            enemy.Die();
                        }
                    }
                }
            }
        }
    }

    // override the TurnManager's FinishTurn
    public override void FinishTurn()
    {
        // capture any enemies standing on the PlayerNode
        CaptureEnemies();

        playerLastPos = transform.position;
        // tell the GameManager the PlayerTurn is complete
        base.FinishTurn();
    }

}
