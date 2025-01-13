using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemPickUp : MonoBehaviour
{
    public static itemPickUp Instance;
    public GameObject InteractionE;
    public bool CanInteract;



    // Start is called before the first frame update
    void Start()
    {

        InteractionE.active = false;

        Instance = this;



    }


    


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            // murder

            CanInteract = true;

            InteractionE.active = true;

        }



    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            // no murder

            CanInteract = false;

            InteractionE.active = false;

        }

    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && CanInteract)
        {

            inventorymanager.Instance.increasecollectibles(1);
            Destroy(gameObject);

        }





    }
}
