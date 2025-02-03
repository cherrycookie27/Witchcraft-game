using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy : MonoBehaviour
{
    public static enemy instance;

    public GameObject player;
    [SerializeField] AudioSource raaah;
    bool trapped;
    public float speed;
    public bool jumpscareOMG;

    private void Start()
    {
        instance = this;
    }
    void Update()
    {
        transform.LookAt(player.transform.position);
        Vector3 sneakyPos = player.transform.position;
        sneakyPos.y = transform.position.y;

        if (trapped == false)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (trapped)
        {
            raaah.Play();
            StartCoroutine(OnTrap());
        }
    }
 
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jumpscareOMG = true;
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            trapped = true;
        }
    }

    IEnumerator OnTrap()
    {
        yield return new WaitForSeconds(2);

        trapped = false;
    }

    void Losing()
    {
        SceneManager.LoadScene("Lose");
    }
}
