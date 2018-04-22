using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class inventory : MonoBehaviour {

   
   
    public Button[] buttons;


    
    
    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf)
        {
            playerLook.Instance.UIactive = false;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
                if (playerLook.Instance.hasItem[i] && buttons[i].gameObject.tag == playerLook.Instance.collectableItems[i].gameObject.tag)
                {
                    buttons[i].gameObject.SetActive(true);

                    if (!playerLook.Instance.collectableItems[i].activeSelf)
                    {
                        buttons[i].GetComponentInChildren<Text>().text = "Equip " + playerLook.Instance.collectableItems[i].gameObject.tag + "?";
                    }
                    else
                    {
                        buttons[i].GetComponentInChildren<Text>().text = "Unequip " + playerLook.Instance.collectableItems[i].gameObject.tag + "?";
                    }
                }
            }
        }

  
}

   

