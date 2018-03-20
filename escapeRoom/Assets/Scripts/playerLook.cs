using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerLook : MonoBehaviour
{

    GameObject playerBody;
    public float mouseSensitivity;

    public Transform sightRayRange;
    bool seenObject;

    public Text seenObjectText;

    float xAxisClamp = 0.0f;

    private void Awake()
    {
        //Lock cursor to centre of screen
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = GameObject.FindGameObjectWithTag("playerBody");

        seenObject = false;
        seenObjectText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        rotateView();
        lookAtObject();
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
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotationCamera.x = 270;
        }

        transform.rotation = Quaternion.Euler(targetRotationCamera);
        playerBody.transform.rotation = Quaternion.Euler(targetRotationBody);
    }


    void lookAtObject()
    {
        RaycastHit objectHit;
        seenObject = Physics.Raycast(transform.position, sightRayRange.position, out objectHit);
        
        
        if(seenObject)
        {
            seenObjectText.GetComponent<Text>().text = objectHit.collider.tag.ToString();
            seenObjectText.gameObject.SetActive(true);

            if (Input.GetButton("Fire1")) 
            {
                objectHit.transform.parent = gameObject.transform;
            }
            else
            {
                objectHit.transform.parent = null;
            }
        }
        else
        {
            seenObjectText.gameObject.SetActive(false);
        }
    }
}
