using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public enum Turn
{
    Player,
    Enemy
}

public class GameManager : MonoBehaviour
{
    private GameController gameController;
    // reference to the GameBoard
    Board m_board;

    // reference to PlayerManager
    PlayerManager m_player;

    List<EnemyManager> m_enemies;

    Turn m_currentTurn = Turn.Player;
    public Turn CurrentTurn { get { return m_currentTurn; } }

    // has the user pressed start?
    bool m_hasLevelStarted = false;
    public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }

    // have we begun gamePlay?
    bool m_isGamePlaying = false;
    public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }

    // have we met the game over condition?
    bool m_isGameOver = false;
    public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }


    // is Player dead?
    bool m_PlayerDead = false;

    // have the end level graphics finished playing?
    bool m_hasLevelFinished = false;
    public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }

    // delay in between game stages
    public float delay = 1f;

    public GameObject block;
    public GameObject stone;
    public GameObject extinguer;
    public GameObject fire;
    public GameObject fireTrap;
    public GameObject winSound;
    public GameObject loseSound;
    public GameObject blockFallSound;
    public GameObject putOutFire;

    public Text extinguiserText;

    // events invoked for StartLevel/PlayLevel/EndLevel coroutines
    public UnityEvent setupEvent;
    public UnityEvent startLevelEvent;
    public UnityEvent playLevelEvent;
    public UnityEvent endLevelEvent;
    public UnityEvent loseLevelEvent;




    void Awake()
    {
        // populate Board and PlayerManager components
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        m_player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        m_enemies = (Object.FindObjectsOfType<EnemyManager>() as EnemyManager[]).ToList();
        gameController = GameController.Instance;
        if (gameController == null)
        {
            Debug.LogError("NULL");
        }

    }

    void Start()
    {
        // start the main game loop if the PlayerManager and Board are present
        if (m_player != null && m_board != null)
        {
            StartCoroutine("RunGameLoop");
        }
        else
        {
            Debug.LogWarning("GAMEMANAGER Error: no player or board found!");
        }
    }

    // run the main game loop, separated into different stages/coroutines
    IEnumerator RunGameLoop()
    {
        yield return StartCoroutine("StartLevelRoutine");
        yield return StartCoroutine("PlayLevelRoutine");
        yield return StartCoroutine("EndLevelRoutine");
    }

    // the initial stage after the level is loaded
    IEnumerator StartLevelRoutine()
    {
        Debug.Log("SETUP LEVEL");
        if (setupEvent != null)
        {
            setupEvent.Invoke();
        }

        Debug.Log("START LEVEL");
        m_player.playerInput.InputEnabled = false;
        yield return null;
        /*
        while (!m_hasLevelStarted)
        {
            //show start screen
            // user presses button to start
            // HasLevelStarted = true
            yield return null;
        }*/

        // trigger events when we press the StartButton
        if (startLevelEvent != null)
        {
            startLevelEvent.Invoke();
        }
    }

    // gameplay stage
    IEnumerator PlayLevelRoutine()
    {
        Debug.Log("PLAY LEVEL");
        m_isGamePlaying = true;
        yield return new WaitForSeconds(delay);
        m_player.playerInput.InputEnabled = true;

        // trigger any events as we start playing the level
        if (playLevelEvent != null)
        {
            playLevelEvent.Invoke();
        }

        while (!m_isGameOver)
        {
            // pause one frame
            yield return null;

            //block  fall
            if (m_board.PlayerNode == m_board.PressureNode)
            {
                if (!m_board.PressurePressed)
                {
                    AudioSource audio = blockFallSound.GetComponent<AudioSource>();
                    audio.Play();
                    //yield return new WaitForSeconds(audio.clip.length);
                }
                m_board.PressurePressed = true;
                block.transform.Translate(Vector3.down * Time.deltaTime * 5);

                yield return null;

            }
            if (m_board.PlayerNode == m_board.ExtinguerNode)
            {
                if (!m_board.ExtintorCollected)
                {
                    Debug.Log("denotr if extintor");
                    AudioSource audio = extinguer.GetComponent<AudioSource>();
                    audio.Play();
                    yield return new WaitForSeconds(audio.clip.length);


                }
                m_board.ExtintorCollected = true;
                extinguer.SetActive(false);
                extinguiserText.text = "x1";
                //yield return new WaitForSeconds(0.3f);
                //TODO activar canvas 

            }
            if (m_board.PlayerNode == m_board.KillerNode)
            {
                m_board.KillerPressed = true;
                iTween.MoveTo(stone, iTween.Hash(
                   "x", m_board.positionToKill.x,
                   "y", m_board.positionToKill.y,
                   "z", m_board.positionToKill.z,
                   "delay", 0,
                   "easetype", iTween.EaseType.easeInSine,
                   "speed", 30
               ));
                List<EnemyManager> enemies = m_board.FindEnemiesAt(m_board.FindNodeAt(m_board.positionToKill));
                if (enemies.Count != 0)
                {
                    // ...invoke the Die method on each one
                    foreach (EnemyManager enemy in enemies)
                    {
                        if (enemy != null)
                        {
                            enemy.Die();
                        }
                    }
                }

            }
            if (m_board.PlayerNode == m_board.FireNode)
            {
                if (m_board.ExtintorCollected)
                {
                    if (fire.activeSelf)
                    {
                        AudioSource audio = putOutFire.GetComponent<AudioSource>();
                        audio.Play();
                        //yield return new WaitForSeconds(audio.clip.length);
                    }
                    fire.SetActive(false);
                    extinguiserText.text = "x0";
                    yield return null;
                }
                else
                {
                    playerDiedByHostile();
                }
                
            }
            if (m_board.PlayerNode == m_board.TrapNode)
            {
                if (fireTrap == null)
                {
                    Debug.Log("FireTrap not set");
                }
                else
                {
                    fireTrap.SetActive(true);
                    m_PlayerDead = true;
                }

                playerDiedByHostile();

            }
            // check for level win condition
            m_isGameOver = IsWinner();
            //win sound
            if (m_isGameOver)
            {
                winSound.GetComponent<AudioSource>().Play();
                StaticInformation.win();
            }

            // check for the lose condition
        }
    }

    public void playerDiedByHostile()
    {
        m_player.Die();
        LoseLevel();
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelRoutine());
    }

    // trigger the "lose" condition
    IEnumerator LoseLevelRoutine()
    {
        // game is over
        m_isGameOver = true;
        // wait for a short delay then...
        yield return new WaitForSeconds(1.5f);
        if (m_PlayerDead)
        {
            //TODO
            AudioSource audio = loseSound.GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
        }

        // ...invoke loseLoveEvent
        if (loseLevelEvent != null)
        {

            loseLevelEvent.Invoke();
        }

        // pause for two seconds and then restart the level
        yield return new WaitForSeconds(1.5f);

        Debug.Log("LOSE! =============================");

        RestartLevel();
    }

    // end stage after gameplay is complete
    IEnumerator EndLevelRoutine()
    {
        Debug.Log("END LEVEL");
        m_player.playerInput.InputEnabled = false;

        // run events when we end the level
        if (endLevelEvent != null)
        {
            endLevelEvent.Invoke();
        }

        // show end screen
        while (!m_hasLevelFinished)
        {
            // user presses button to continue

            // HasLevelFinished = true
            yield return null;
        }

        // reload the current scene
        RestartLevel();
    }

    // restart the current level
    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // attach to StartButton, triggers PlayLevelRoutine
    public void PlayLevel()
    {
        m_hasLevelStarted = true;
    }

    // has the player reached the goal node?
    bool IsWinner()
    {
        if (m_board.PlayerNode != null)
        {
            return (m_board.PlayerNode == m_board.GoalNode);
        }
        return false;
    }

    // switch to Player turn
    void PlayPlayerTurn()
    {
        m_currentTurn = Turn.Player;
        m_player.IsTurnComplete = false;

        // allow Player to move
    }

    // switch to Enemy turn
    void PlayEnemyTurn()
    {
        m_currentTurn = Turn.Enemy;

        foreach (EnemyManager enemy in m_enemies)
        {
            if (enemy != null && !enemy.IsDead)
            {
                enemy.IsTurnComplete = false;

                enemy.PlayTurn();
            }
        }
    }

    // have all of the enemies completed their turns?
    bool IsEnemyTurnComplete()
    {
        foreach (EnemyManager enemy in m_enemies)
        {
            if (enemy.IsDead)
            {
                continue;
            }

            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }

        return true;
    }

    bool AreEnemiesAllDead()
    {
        foreach (EnemyManager enemy in m_enemies)
        {
            if (!enemy.IsDead)
            {
                return false;
            }
        }
        return true;
    }

    // switch between Player and Enemy Turns
    public void UpdateTurn()
    {
        if (m_currentTurn == Turn.Player && m_player != null)
        {
            if (m_player.IsTurnComplete && !AreEnemiesAllDead())
            {
                PlayEnemyTurn();
            }

        }
        else if (m_currentTurn == Turn.Enemy)
        {
            if (IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }
        }
    }

    public void moveToMenu()
    {
        if (gameController == null)
        {
            reloadAsyncLevel(0);
        }
        else
        {
            StaticInformation.CurrentLevel = 0;
            reloadAsyncLevel();
        }
    }

    public void goLevel1()
    {
        if (gameController == null)
        {
            reloadAsyncLevel(1);
        }
        else
        {
            StaticInformation.CurrentLevel = 1;
            reloadAsyncLevel();
        }
    }
    public void goLevel2()
    {
        if (gameController == null)
        {
            reloadAsyncLevel(2);
        }
        else
        {
            StaticInformation.CurrentLevel = 2;
            reloadAsyncLevel();
        }
    }
    public void goLevel3()
    {
        if (gameController == null)
        {
            reloadAsyncLevel(3);
        }
        else
        {
            StaticInformation.CurrentLevel = 3;
            reloadAsyncLevel();
        }
    }

    public void reloadAsyncLevel()
    {
        reloadAsyncLevel(StaticInformation.CurrentLevel);
    }
    public void reloadAsyncLevel(int scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

}
