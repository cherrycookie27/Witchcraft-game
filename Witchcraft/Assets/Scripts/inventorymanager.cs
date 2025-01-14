using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class inventorymanager : MonoBehaviour
{
    public static inventorymanager Instance;

    public TMP_Text collectibletext;
    public TMP_Text taskText;

    public int currentcollectibles = 0;
    public int maxcollectibles = 5;


    public void increasecollectibles(int v)
    {
        currentcollectibles += v;

        collectibletext.text = "Spell Items: " + currentcollectibles.ToString() + "/5";
    }
    
    void Start()
    {
        collectibletext.text = "Spell Items: " + currentcollectibles.ToString() + "/5";

        Instance = this;
    }

    void Update()
    {
        if (currentcollectibles < maxcollectibles)
        {
            taskText.text = "Find all spell bones!";
        }
        if (currentcollectibles >= maxcollectibles)
        {
            taskText.text = "Cast the spell in the spell circle!";
            // can spell!!!!
        }
    }
}
