using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JumpScare : MonoBehaviour
{
    [SerializeField] AudioSource raaahh;
    public GameObject monster;
    public GameObject jumpScareMonster;
    public Image pelkoTausta;
    private void Start()
    {
        jumpScareMonster.SetActive(false);
        pelkoTausta.enabled = false;
    }
    private void Update()
    {
        if (enemy.instance.jumpscareOMG)
        {

        }
    }

    IEnumerator HyppyPelko()
    {
        pelkoTausta.enabled = true;
        monster.SetActive(false);
        jumpScareMonster.SetActive(true);

        raaahh.Play();
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Lose");
    }
}
