using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

public class MagicCircle : MonoBehaviour
{
    public static MagicCircle instance;

    public GameObject MONSERT;
    public GameObject BONES;
    public TMP_Text circleText;

    [SerializeField] AudioSource monster;

    public bool canRitual = false;
    private void Start()
    {
        instance = this;
        circleText.text = "";
        BONES.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canRitual)
        {
            BONES.SetActive(true);
            monster.Play();

            MONSERT.SetActive(false);
            StartCoroutine(Ending());
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
                circleText.text = "Press E to do the expel ritual";
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

    IEnumerator Ending()
    {
        circleText.text = "I did it...";

        yield return new WaitForSeconds(3);

        circleText.text = "It's finally over...";

        yield return new WaitForSeconds (4);

        SceneManager.LoadScene("Win");
    }
}
