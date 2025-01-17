using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy : MonoBehaviour
{
    public static enemy instance;
    public GameObject player;
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

       transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
    }
 
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            jumpscareOMG = true;
        }
    }

    void Losing()
    {
        SceneManager.LoadScene("Lose");
    }
}
