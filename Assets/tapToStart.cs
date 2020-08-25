using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tapToStart : MonoBehaviour
{
    public GameObject gameStartCanvas;
    
    void Start()
    {
        
    }

    
    void Update()
    {   
        Pause();
        if (Input.GetMouseButtonDown(0))
            Resume();
    }



    void Pause()
    {
        //gameStartCanvas.SetActive(true);
        Time.timeScale = 0f;

    }

    void Resume()
    {
        gameStartCanvas.SetActive(false);
        Time.timeScale = 1f;

    }
}
