using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLook : MonoBehaviour {

    GameObject playerBody;
    public float mouseSensitivity;

    float xAxisClamp = 0.0f;

    private void Awake()
    {
        //Lock cursor to centre of screen
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = GameObject.FindGameObjectWithTag("playerBody");
    }

    // Update is called once per frame
    void Update ()
    {
        rotateView();

    }
    void rotateView()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 targetRotationCamera = transform.rotation.eulerAngles;
        Vector3 targetRotationBody = playerBody.transform.rotation.eulerAngles;

        targetRotationCamera.x -= rotAmountY; //Rotatates camera in direction of cursor up and down
        targetRotationCamera.z = 0; //Stops over rotation;
        targetRotationBody.y += rotAmountX; // Rotates whole body to make movement easier


            // Clamps the camera in the x between -90 and 90 (directly up and directly down)
           //To stop flicker
        if (xAxisClamp > 90)
        {
            xAxisClamp = targetRotationCamera.x = 90;
        }
        else if(xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotationCamera.x = 270;
        }

        transform.rotation = Quaternion.Euler(targetRotationCamera);
        playerBody.transform.rotation = Quaternion.Euler(targetRotationBody);
    }
}
