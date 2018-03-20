﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    private CharacterController playerController;
    private Rigidbody rigid;
    public float speed;


    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();
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
        Vector3 moveHorizontal = transform.right * horizontal * speed;

        //Move forward and back
        Vector3 moveForward = transform.forward * vertical * speed;

        //Implement said moves
        playerController.SimpleMove(moveHorizontal);
        playerController.SimpleMove(moveForward);
    }

  
}
