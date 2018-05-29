
using UnityEngine;
using UnityEngine.UI;

public class SkipPlayButton : MonoBehaviour
{
    public Button button;

    // Use this for initialization
    void Start()
    {
        if (StaticInformation.gameStarted)
        {
            button = GameObject.Find("StartButton").GetComponent<Button>();
            button.onClick.Invoke();
        }

    }


}
