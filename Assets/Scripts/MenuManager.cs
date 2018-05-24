using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public UnityEvent setupEvent;


    // Use this for initialization
    void Start()
    {
        Debug.Log("START");
        if (setupEvent != null)
        {
            setupEvent.Invoke();
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
