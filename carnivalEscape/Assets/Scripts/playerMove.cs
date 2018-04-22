using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    private CharacterController playerController;
    GameObject playerSpotlight;
    
    public float speed;


    private void Awake()
    {
        playerSpotlight = GameObject.FindGameObjectWithTag("PlayerSpotlight");
        playerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        movement();
    }

    void movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Movement

        //Move side to side
        Vector3 moveHorizontal = transform.right * horizontal * speed * Time.deltaTime;

        //Move forward and back
        Vector3 moveForward = transform.forward * vertical * speed * Time.deltaTime;

        //Implement said moves
        playerController.SimpleMove(moveHorizontal);
        playerController.SimpleMove(moveForward);

        var lightPos = new Vector3(transform.position.x, playerSpotlight.transform.position.y, transform.position.z);

        playerSpotlight.transform.position = lightPos;
    }
  
}
