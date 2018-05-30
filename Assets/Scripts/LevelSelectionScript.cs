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
        StaticInformation.CurrentLevel = 1;
        setStarted();
        SceneManager.LoadSceneAsync(1);
    }

    public void PlayLevel2()
    {
        unloadScene();
        StaticInformation.CurrentLevel = 2;
        setStarted();
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayLevel3()
    {
        unloadScene();
        StaticInformation.CurrentLevel = 3;
        setStarted();
        SceneManager.LoadSceneAsync(3);
    }
    public void PlayLevel4()
    {
        unloadScene();
        StaticInformation.CurrentLevel = 4;
        setStarted();
        SceneManager.LoadSceneAsync(4);
    }
    public void PlayLevel5()
    {
        unloadScene();
        StaticInformation.CurrentLevel = 5;
        setStarted();
        SceneManager.LoadSceneAsync(5);
    }

    public void unloadScene()
    {

    }

    public void setStarted()
    {
        StaticInformation.gameStarted = true;
    }
}
