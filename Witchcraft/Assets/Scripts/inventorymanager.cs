using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class inventorymanager : MonoBehaviour
{



    public static inventorymanager Instance;

    public TMP_Text collectibletext;

    public int currentcollectibles = 0;

    public int maxcollectibles = 5;


    public void increasecollectibles(int v)
    {


        currentcollectibles += v;

        collectibletext.text = "Spell Items: " + currentcollectibles.ToString();


    }












    // Start is called before the first frame update
    void Start()
    {

        collectibletext.text = "Spell Items: " + currentcollectibles.ToString();

        Instance = this;


    }

    // Update is called once per frame
    void Update()
    {
        

        if (currentcollectibles >= maxcollectibles)
        {

            // can spell!!!!

        }





    }
}
