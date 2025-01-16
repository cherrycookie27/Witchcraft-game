using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicCircle : MonoBehaviour
{
    public static MagicCircle instance;

    public TMP_Text circleText;

    public bool canRitual = false;
    private void Start()
    {
        instance = this;
        circleText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canRitual)
        {
            //ritual!!!
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (inventorymanager.Instance.currentcollectibles <5)
            {
                circleText.text = "Find the bones first!";
            }
            else if (inventorymanager.Instance.currentcollectibles >= 5)
            {
                circleText.text = "Do the expel ritual";
                canRitual = true;
            }
            else
            {
                circleText.text = "";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            circleText.text = "";
        }
    }
}
