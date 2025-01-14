using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class pauseMenu : MonoBehaviour
{

    public GameObject pausemenu;
    public bool isPaused;
    public StarterAssetsInputs sai;


    public void resumeGame()
    {

        pausemenu.SetActive(false);

        Time.timeScale = 1f;

        isPaused = false;
        Cursor.visible = false;

        sai.SetCursorState(true);
        sai.cursorInputForLook = true;
    }


    public void pauseGame()
    {

        pausemenu.SetActive(true);

        Time.timeScale = 0f;

        isPaused = true;

        itemPickUp.Instance.CanInteract = false;
        Cursor.visible = true;
        sai.SetCursorState(false);
        sai.cursorInputForLook = false;

        sai.LookInput(new Vector2());
        //camera constrains!!!!
    }


    // Start is called before the first frame update
    void Start()
    {
        pausemenu.SetActive(false);

        isPaused = false;
        Cursor.visible = false;
        sai.SetCursorState(true);
        sai.cursorInputForLook = true;


    }








    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!isPaused)
            {
                if(sai.GetComponent<FirstPersonController>().minigameActive)
                {
                    sai.GetComponent<FirstPersonController>().ToggleMinigame();
                }
                else
                {
                    pauseGame();
                }

               

            }
            else
            {

                resumeGame();


            }

        }




    }




}
