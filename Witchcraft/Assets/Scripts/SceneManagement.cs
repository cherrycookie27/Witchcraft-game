using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{



 

    void WhatScene()
    {
        Scene s = SceneManager.GetActiveScene();
        Debug.Log(s.name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void quitgame()
    {
        Application.Quit();

    }

    public void skenevaihto(string levelname)
    {

        SceneManager.LoadScene(levelname);
    }

}
