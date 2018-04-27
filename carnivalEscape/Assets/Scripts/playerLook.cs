using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class playerLook : MonoBehaviour
{
    public Texture2D axeHandle;
    public Texture2D axeHead;

    public Text tagText;
    public Text crosshair;

    public Texture2D restartTex;
    public Texture2D quitTex;

    private static playerLook instance;

    public GameObject inventory;

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
    int fiftyMins = 3000;

    public bool tenMinsRemaining = false;

    public Texture2D fadeTexture;
    float fadeSpeed = 0.2f;
    float drawDepth = -1000;

    private float alpha = 0.0f;
    private float fadeDir = -1;

    public bool playerDead = false;

    string keypadInput;

    public bool crosshairActive = true;

    bool hasHandle = false;
    bool hasAxehead = false;

    public string[] keypadPassword;

    public bool balanceWeights = false;

    GUIStyle boxStyle;

    public GameObject[] keypadObj;

    int objRef = 0;
    public bool keypad;
    int currentPassword = 0;

    bool hasBalancePole = false;

    GameObject rope1;
   

    public bool[] useLadder;
    public GameObject[] ladders;

    int poleCount;

    

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

        
        rope1 = GameObject.FindGameObjectWithTag("rope1");
        if (rope1 != null)
        {
            rope1.GetComponent<Collider>().enabled = false;
        }
        

        useLadder = new bool[ladders.Length];
    }

    





    void OnGUI()
    {

        if (keypad)
        {
            //Creates a keypad on GUI
            Cursor.lockState = CursorLockMode.None;

            boxStyle = new GUIStyle(GUI.skin.box);

            if (keypadInput == keypadPassword[currentPassword])
            {
                
                
                boxStyle.normal.textColor = Color.green;

                if (keypadObj[objRef] != null)
                {
                    keypadObj[objRef].GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, Mathf.Lerp(keypadObj[objRef].GetComponent<MeshRenderer>().material.color.a, -0.1f, 1 * Time.deltaTime));

                    if (keypadObj[objRef].GetComponent<MeshRenderer>().material.color.a <= 0)
                    {
                        Destroy(keypadObj[objRef]);
                        keypad = false;
                        currentPassword++;
                        keypadInput = "";

                        //If current password is right, do above
                    }
                }


            }
            else
            {
                boxStyle.normal.textColor = Color.white;
            }


            GUI.Box(new Rect(0, 0, 320, 455), "");
            GUI.Box(new Rect(5, 5, 310, 25), keypadInput, boxStyle);

            if (GUI.Button(new Rect(5, 35, 100, 100), "1"))
            {
                keypadInput += "1";
            }
            if (GUI.Button(new Rect(110, 35, 100, 100), "2"))
            {
                keypadInput += "2";
            }
            if (GUI.Button(new Rect(215, 35, 100, 100), "3"))
            {
                keypadInput += "3";
            }
            if (GUI.Button(new Rect(5, 140, 100, 100), "4"))
            {
                keypadInput += "4";
            }
            if (GUI.Button(new Rect(110, 140, 100, 100), "5"))
            {
                keypadInput += "5";
            }
            if (GUI.Button(new Rect(215, 140, 100, 100), "6"))
            {
                keypadInput += "6";
            }
            if (GUI.Button(new Rect(5, 245, 100, 100), "7"))
            {
                keypadInput += "7";
            }
            if (GUI.Button(new Rect(110, 245, 100, 100), "8"))
            {
                keypadInput += "8";
            }
            if (GUI.Button(new Rect(215, 245, 100, 100), "9"))
            {
                keypadInput += "9";
            }
            if (GUI.Button(new Rect(110, 350, 100, 100), "0"))
            {
                keypadInput += "0";
            }
            if (GUI.Button(new Rect(5, 350, 100, 100), "Clear"))
            {
                keypadInput = "";
            }
            if (GUI.Button(new Rect(215, 350, 100, 100), "Exit"))
            {
                keypad = false;
            }

            if (keypadInput != null && keypadInput.Length > keypadPassword[currentPassword].Length)
            {
                keypadInput = "";
            }
        }


        //Draws axe pieces to screen
        if (hasAxehead)
        {
            GUI.DrawTexture(new Rect(Screen.width - axeHead.width / 6, Screen.height - axeHead.height / 8, axeHead.width / 6, axeHead.height / 6), axeHead);
        }
        if (hasHandle)
        {
            GUI.DrawTexture(new Rect(Screen.width - axeHead.width / 6 - 30, Screen.height - axeHead.height / 6, axeHandle.width / 6, axeHandle.height / 6), axeHandle);
        }

        //Creates the timer
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string clock = string.Format("{0:0}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(10, 10, 250, 100), clock);

        if (playerDead)
        {
            //Fades in the death screen then gives the option to restart or quit
            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

            GUI.depth = (int)drawDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

            if (alpha >= 1)
            {
                Cursor.lockState = CursorLockMode.None;

                if (GUI.Button(new Rect(Screen.width / 2 - restartTex.width / 2 / 3, Screen.height / 1.5f, restartTex.width / 3.5f, restartTex.height / 3.5f), restartTex))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().path);
                }
                if (GUI.Button(new Rect(Screen.width / 2 - quitTex.width / 2 / 3, Screen.height / 3.4f, quitTex.width / 3.5f, quitTex.height / 3.5f), quitTex))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

        objRef = currentPassword;
        if (crosshairActive == false)
        {
            tagText.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(false);
        }
        else
        {
            tagText.gameObject.SetActive(true);
            crosshair.gameObject.SetActive(true);
        }
        crosshairActive = true;

        if (!playerDead)
        {
            timer += Time.deltaTime;

            if (timer >= hour)
            {
                playerDead = true;

            }
            if (timer >= fiftyMins)
            {
                tenMinsRemaining = true;
            }
            //for(int i = 0; i < keypad.Length; i++)
            {
                if (!keypad)
                {
                    if (Input.GetKeyDown(KeyCode.I))
                    {
                        inventoryActive = !inventoryActive;
                        inventory.SetActive(inventoryActive);
                        Cursor.lockState = CursorLockMode.None;
                    }

                    if (!inventoryActive)
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        rotateView();
                        lookAtObject();
                    }
                }
            }
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


        //Allows for the manipulation of objects below


        if (seenObject)
        {
            var otherWheel = GameObject.FindGameObjectWithTag("codePuzzle2");
            
            //Puts text on screen with the color yellow
            seenObjectText.GetComponent<Text>().text = objectHit.collider.tag.ToString();
            seenObjectText.GetComponent<Text>().color = Color.yellow;
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
                if (objectHit.collider.tag == "axeHead")
                {
                    if (Input.GetButton("Fire1"))
                    {
                        objectHit.transform.gameObject.SetActive(false);
                        hasAxehead = true;
                    }
                }
                for (int j = 0; j < ladders.Length; j++)
                {
                    if (objectHit.collider == ladders[j].GetComponent<Collider>())
                    {
                        if (Input.GetButton("Fire1"))
                        {
                            useLadder[j] = true;
                        }
                    }
                }

                if (objectHit.collider.tag == "axeHandle")
                {
                    if (Input.GetButton("Fire1"))
                    {
                        objectHit.transform.gameObject.SetActive(false);
                        hasHandle = true;
                    }
                }
                if (hasAxehead == true && hasHandle == true && collectableItems[i].gameObject.tag == "Axe")
                {
                    hasItem[i] = true;
                }
                if (poleCount == 5 && collectableItems[i].gameObject.tag == "Rope ladder")
                {
                    hasItem[i] = true;
                }

                if (balanceWeights == true && hasBalancePole == true && collectableItems[i].gameObject.tag == "Balancing pole")
                {
                    hasItem[i] = true;
                }
                if(objectHit.collider.tag == "Pole" && collectableItems[i].gameObject.tag == "Axe" && collectableItems[i].activeSelf)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        hasBalancePole = true;
                        objectHit.transform.gameObject.SetActive(false);
                    }
                }

                if (objectHit.collider.gameObject == keypadObj[objRef])
                {
                    if (Input.GetButton("Fire1"))
                    {
                        keypad = true;
                    }
                }
            }
            if (objectHit.collider.tag == "codePuzzle")
            {
                if (Input.GetButton("Fire1"))
                {
                    objectHit.transform.RotateAround(objectHit.collider.bounds.center, Vector3.right, 45  * Time.deltaTime);
                  
                    otherWheel.transform.RotateAround(otherWheel.GetComponent<Collider>().bounds.center, Vector3.right, 45 * Time.deltaTime);
                }
            }

            if (objectHit.collider.tag == "selector")
            {
                if (Input.GetButton("Fire1"))
                {
                    objectHit.transform.RotateAround(otherWheel.GetComponent<Collider>().bounds.center, Vector3.right, 45 * Time.deltaTime);
                }
            }

            if (objectHit.collider.tag == "Cannon")
            {
                if (Input.GetButton("Fire1"))
                {
                    SceneManager.LoadScene(0);
                }
            }

            if (objectHit.collider.tag == "Small pole")
            {
                if (Input.GetButton("Fire1"))
                {
                    objectHit.transform.gameObject.SetActive(false);
                    poleCount++;
                }
            }

            if (objectHit.transform.GetComponent<dropOnCollision>())
        {
            var objectTransform = objectHit.transform.rotation;

            if (Input.GetButton("Fire1") && objectHit.collider.tag != "Floor" && objectHit.transform.GetComponent<dropOnCollision>().carryObject == true)
            {

                objectHit.transform.SetParent(gameObject.transform);
                objectHit.transform.rotation = objectTransform;

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
        else
        {
            return;
        }
    }
        

        else
        {
            seenObjectText.gameObject.SetActive(false);
        }
    }

    public void updateEquip(GameObject obj)
    {
        //Updates the equip state of each collectable objects

        equipItem = !equipItem;

        for (int i = 0; i < collectableItems.Length; i++)
        {
            if (obj.tag == collectableItems[i].gameObject.tag)
            {
                if (equipItem)
                {
                    collectableItems[i].SetActive(true);
                    if(collectableItems[i].tag == "Balancing pole")
                    {
                        rope1.GetComponent<Collider>().enabled = true;
                    }
                }
                else
                {
                    collectableItems[i].SetActive(false);
                    if (collectableItems[i].tag == "Balancing pole")
                    {
                       rope1.GetComponent<Collider>().enabled = false;
                    }
                }
            }
            else
            {
                collectableItems[i].SetActive(false);
            }
        }
            
    }
}

    
    

