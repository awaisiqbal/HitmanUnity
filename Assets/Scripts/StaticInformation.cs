using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInformation : MonoBehaviour {

    public static int CurrentLevel = 0;
    public static int CurrentGems = 0;
    public static int MaxArtefacts = 3;
    private static bool[] keysObtained = new bool[] { false, false, false };
    public static bool isSoundON = true;
    public static bool gameStarted = false;

    public static void win()
    {
        keysObtained[CurrentLevel] = true;
    }

    public static int getCurrentKeysFound()
    {
        int count = 0;
        for(int i = 0; i < keysObtained.Length; i++)
        {
            if (keysObtained[i])
            {
                count++;
            }
        }
        return count;
    }

    public static void setCurrentLevel(int level)
    {
        CurrentLevel = level;
    }
    
}
