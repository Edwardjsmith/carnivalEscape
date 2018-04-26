using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerMove : MonoBehaviour
{
    private CharacterController playerController;
    public GameObject playerSpotlight;

    public float speed;
    Vector3 moveForward;

    float horizontal;
    float vertical;

    bool onLadder;

    public GameObject[] deadZones;
   

    Vector3 lightPos;

    private void Start()
    {
        lightPos = new Vector3(transform.position.x, playerSpotlight.transform.position.y, transform.position.z);
        playerSpotlight.transform.position = lightPos;
    }
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

        for (int i = 0; i < playerLook.Instance.useLadder.Length; i++)
        {
            if (playerLook.Instance.useLadder[i])
            {
                transform.position = playerLook.Instance.ladders[i].GetComponentInChildren<Transform>().position;
                playerLook.Instance.useLadder[i] = false;
            }
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


        lightPos = new Vector3(transform.position.x, playerSpotlight.transform.position.y, transform.position.z);
        playerSpotlight.transform.position = lightPos;

    }


    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < deadZones.Length; i++)
        {
            if (other.name == deadZones[i].GetComponent<Collider>().name)
            {
                //Resets pos if the player either falls on the tight rope or the player simply tries to skip parts of the level

                gameObject.transform.position = other.transform.position;
            }

        }

    }
}


