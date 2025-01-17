using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    public void quitgame()
    {
        Application.Quit();

    }

    public void skenevaihto(string levelname)
    {

        SceneManager.LoadScene(levelname);
    }

    public void lose()
    {
        SceneManager.LoadScene("Lose");
    }

}
