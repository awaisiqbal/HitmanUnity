using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionScript : MonoBehaviour
{
    public GameObject levelSelectionMenu;

    public void PlayLevel1()
    {
        unloadScene();
        SceneManager.LoadScene(1);
    }

    public void PlayLevel2()
    {
        unloadScene();
        SceneManager.LoadScene(2);
    }

    public void PlayLevel3()
    {
        unloadScene();
        SceneManager.LoadScene(3);
    }

    public void unloadScene()
    {

    }
}
