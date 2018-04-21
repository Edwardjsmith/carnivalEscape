using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerLook : MonoBehaviour
{
    private static playerLook instance;

    public GameObject inv;

    bool inventoryActive = false;
    GameObject playerBody;
    public float mouseSensitivity;
    bool seenObject;

    public GameObject[] collectableItems;

    public Text seenObjectText;

    float xAxisClamp = 0.0f;
    public bool carry;

    bool equipItem;

    public bool[] hasItem;

    float timer;
    int hour = 3600;
   

    public static playerLook Instance
    {
        get
        {
            return instance;
        }
    }


    private void Awake()
    {
     

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;

        //Lock cursor to centre of screen
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = GameObject.FindGameObjectWithTag("playerBody");

        seenObject = false;
        seenObjectText.gameObject.SetActive(false);
        carry = false;

        hasItem = new bool[collectableItems.Length];

        for (int i = 0; i < collectableItems.Length; i++)
        {
            collectableItems[i].SetActive(false);
            equipItem = false;
        }

    }

    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(10, 10, 250, 100), niceTime);
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= hour)
        {
            Debug.Log("You failed");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActive = !inventoryActive;
            inv.SetActive(inventoryActive);
            Cursor.lockState = CursorLockMode.None;
        }

        if (!inventoryActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            rotateView();
            lookAtObject();
        }

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

        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f)); //Aligns ray to centre of cam
        RaycastHit objectHit;

        seenObject = Physics.Raycast(ray, out objectHit, 5.0f);

        if (seenObject)
        {
            seenObjectText.GetComponent<Text>().text = objectHit.collider.tag.ToString();
            seenObjectText.gameObject.SetActive(true);

            for (int i = 0; i < collectableItems.Length; i++)
            {
                if (objectHit.collider.tag == collectableItems[i].gameObject.tag)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        objectHit.transform.gameObject.SetActive(false);
                        hasItem[i] = true;
                    }
                }
            }

            if (objectHit.transform.GetComponent<dropOnCollision>())
            {

                if (Input.GetButton("Fire1") && objectHit.collider.tag != "Floor" && objectHit.transform.GetComponent<dropOnCollision>().carry == true)
                {
                    objectHit.transform.SetParent(gameObject.transform);

                    if (objectHit.transform.GetComponent<Rigidbody>())
                    {
                        objectHit.transform.GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
                else
                {
                    objectHit.transform.SetParent(null);

                    if (objectHit.transform.GetComponent<Rigidbody>())
                    {
                        objectHit.transform.GetComponent<Rigidbody>().isKinematic = false;
                    }
                }
            }
        }

        else
        {
            seenObjectText.gameObject.SetActive(false);
        }
    }
    public void updateEquip(GameObject obj)
    {
        equipItem = !equipItem;

        for (int i = 0; i < collectableItems.Length; i++)
        {
            if (obj.tag == collectableItems[i].tag)
            {
                if (equipItem)
                {
                    collectableItems[i].SetActive(true);
                }
                else
                {
                    collectableItems[i].SetActive(false);
                }
            }
            else
            {
                collectableItems[i].SetActive(false);
            }
        }
            
    }
}

    
    

