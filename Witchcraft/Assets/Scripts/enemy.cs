using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemy : MonoBehaviour
{
    public static enemy instance;

    public GameObject player;
    [SerializeField] AudioSource raaah;
    [SerializeField] AudioSource jumpScareRoar;
    bool trapped;
    public float speed;
    public bool jumpscareOMG;
    [SerializeField] Animator anim;
    public Transform jumpScarePos;
    public AudioClip roar;

    private void Start()
    {
        instance = this;
    }
    void Update()
    {
        Vector3 lookPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(lookPos);
        Vector3 sneakyPos = player.transform.position;
        sneakyPos.y = transform.position.y;

        if (trapped == false && jumpscareOMG == false)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
 
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Jumpscare", true);
            jumpscareOMG = true;      
            jumpScareRoar.Play();

            StartCoroutine(Losing());
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            trapped = true;
            StartCoroutine(OnTrap(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            trapped = false;
        }
    }
    IEnumerator OnTrap(GameObject g)
    {
        raaah.Play();
        yield return new WaitForSeconds(2);
        Destroy(g);
        trapped = false;
    }

    IEnumerator Losing()
    {
        yield return new WaitForSeconds(2);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Lose");
    }
}
