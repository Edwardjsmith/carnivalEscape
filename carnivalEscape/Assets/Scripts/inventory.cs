using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour {

    public Button axe;

    public static bool equipAxe = false;

    // Use this for initialization
    void Start ()
    {
        axe.gameObject.SetActive(false);
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(playerLook.hasAxe == true)
        {
            axe.gameObject.SetActive(true);
            if(!equipAxe)
            {
                axe.GetComponentInChildren<Text>().text = "Equip axe?";
            }
            else
            {
                axe.GetComponentInChildren<Text>().text = "Unequip axe?";
            }
        }
	}

    public void updateAxe()
    {
        equipAxe = !equipAxe;
    }
}
