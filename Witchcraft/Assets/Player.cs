using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public Rigidbody Rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Rigidbody.AddForce(transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Rigidbody.AddForce((transform.forward * -1) * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Rigidbody.AddForce(transform.right * speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Rigidbody.AddForce((transform.right * -1) * speed);
        }



    }
}
