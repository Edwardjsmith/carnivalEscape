using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    private CharacterController playerController;
    GameObject playerSpotlight;

    public float speed;
    Vector3 moveForward;

    float horizontal;
    float vertical;

    bool onLadder;

    private void Awake()
    {
        playerSpotlight = GameObject.FindGameObjectWithTag("PlayerSpotlight");
        playerController = GetComponent<CharacterController>();

    }


    private void Update()
    {
        if (!playerLook.Instance.playerDead)
        {
            movement();
        }
    }

    void movement()
    {
        //Movement
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        //Move side to side
        Vector3 moveHorizontal = transform.right * horizontal * speed * Time.deltaTime;

        //Move forward and back
        if (!onLadder)
        {
            moveForward = transform.forward * vertical * speed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }

        //Implement said moves
        playerController.SimpleMove(moveHorizontal);
        playerController.SimpleMove(moveForward);

        var lightPos = new Vector3(transform.position.x, playerSpotlight.transform.position.y, transform.position.z);

        playerSpotlight.transform.position = lightPos;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ladder")
        {
            onLadder = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
        { 
            onLadder = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}


