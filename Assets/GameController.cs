using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameController : MonoBehaviour
{

    public static GameController instance = null;

    public static GameController Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
       
    }

    void Update()
    {
        if (artefactCounter == null || gemsCounter == null || maxArtefactsCounter == null)
        {
            artefactCounter = GameObject.Find("ArtefactsValueText").GetComponent<TextMeshProUGUI>();
            gemsCounter = GameObject.Find("GemsValueText").GetComponent<TextMeshProUGUI>();
            maxArtefactsCounter = GameObject.Find("ArtefactsMaxValueText").GetComponent<TextMeshProUGUI>();
        }
        currentLevel = StaticInformation.CurrentLevel;
        keyFound = StaticInformation.getCurrentKeysFound();
        gems = StaticInformation.CurrentGems;
        artefactCounter.SetText("" + StaticInformation.getCurrentKeysFound());
        maxArtefactsCounter.SetText("" + StaticInformation.MaxArtefacts);
        gemsCounter.SetText("" + StaticInformation.CurrentGems);
    }

    public void startPlay()
    {
        StaticInformation.gameStarted = true;
    }

    public bool hasGameStarted()
    {
        return StaticInformation.gameStarted;
    }

    public TextMeshProUGUI artefactCounter;
    public TextMeshProUGUI maxArtefactsCounter;
    public TextMeshProUGUI gemsCounter;
    public int currentLevel;
    public int keyFound;
    public int gems;


}
