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
    public bool keypad = false;
    string keypadPassword = "105";

    GUIStyle boxStyle;
    GameObject keypadObj;

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

        keypadObj = GameObject.FindGameObjectWithTag("Keypad");
    }



  
        


    void OnGUI()
    { 
        if (keypad)
        {
            Cursor.lockState = CursorLockMode.None;

            boxStyle = new GUIStyle(GUI.skin.box);

            if (keypadInput == keypadPassword)
            {
                boxStyle.normal.textColor = Color.green;

                if (keypadObj != null)
                {
                    keypadObj.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, Mathf.Lerp(keypadObj.GetComponent<MeshRenderer>().material.color.a, -0.1f, 1 * Time.deltaTime ));

                    if (keypadObj.GetComponent<MeshRenderer>().material.color.a <= 0)
                    {
                        Destroy(keypadObj);
                    }
                }
                if (!GameObject.FindGameObjectWithTag("Keypad"))
                {
                    keypad = false;
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

            if(keypadInput != null && keypadInput.Length > 3)
            {
                keypadInput = "";
            }
        }
       

        if(hasAxehead)
        {
            GUI.DrawTexture(new Rect(Screen.width - axeHead.width / 6, Screen.height - axeHead.height / 8, axeHead.width / 6, axeHead.height / 6), axeHead);
        }
        if(hasHandle)
        {
            GUI.DrawTexture(new Rect(Screen.width - axeHead.width / 6 - 30, Screen.height - axeHead.height / 6, axeHandle.width / 6, axeHandle.height / 6), axeHandle);
        }
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string clock = string.Format("{0:0}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(10, 10, 250, 100), clock);

        if (playerDead)
        {

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
                    Application.Quit();
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(crosshairActive == false)
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
                if(objectHit.collider.tag == "axeHead")
                {
                    if (Input.GetButton("Fire1"))
                    {
                        objectHit.transform.gameObject.SetActive(false);
                        hasAxehead = true;
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
                if (objectHit.collider.tag == "axeHandle")
                {
                    if (Input.GetButton("Fire1"))
                    {
                        objectHit.transform.gameObject.SetActive(false);
                        hasHandle = true;
                    }
                }
                if(hasAxehead == true && hasHandle == true && collectableItems[i].gameObject.tag == "Axe")
                {
                    hasItem[i] = true;
                }

                if (objectHit.collider.tag == "Keypad")
                {
                    if (Input.GetButton("Fire1"))
                    {
                        keypad = true;
                    }
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

    
    

