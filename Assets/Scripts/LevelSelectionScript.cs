using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionScript : MonoBehaviour
{
    public GameObject levelSelectionMenu;
    public bool isLoadingLevel = false;

    public void PlayLevel1()
    {
        if (isLoadingLevel)
        {
            return;
        }
        unloadScene();
        StaticInformation.CurrentLevel = 1;
        setStarted();
        isLoadingLevel = true;
        SceneManager.LoadSceneAsync(1);
    }

    public void PlayLevel2()
    {
        if (isLoadingLevel)
        {
            return;
        }
        unloadScene();
        StaticInformation.CurrentLevel = 2;
        setStarted();
        isLoadingLevel = true;
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayLevel3()
    {
        if (isLoadingLevel)
        {
            return;
        }
        unloadScene();
        StaticInformation.CurrentLevel = 3;
        setStarted();
        isLoadingLevel = true;
        SceneManager.LoadSceneAsync(3);
    }
    public void PlayLevel4()
    {
        if (isLoadingLevel)
        {
            return;
        }
        unloadScene();
        StaticInformation.CurrentLevel = 4;
        setStarted();
        isLoadingLevel = true;
        SceneManager.LoadSceneAsync(4);
    }
    public void PlayLevel5()
    {
        if (isLoadingLevel)
        {
            return;
        }
        unloadScene();
        StaticInformation.CurrentLevel = 5;
        setStarted();
        isLoadingLevel = true;
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
