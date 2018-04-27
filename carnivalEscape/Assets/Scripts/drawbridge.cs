using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawbridge : MonoBehaviour {


    //Rotates draw bridge when triggered

    public static bool lowerBridge = false;
    // Use this for initialization


    private void Start()
    {
        GetComponentInChildren<Collider>().enabled = false;
    }
    // Update is called once per frame
    void Update ()
    {
		if(lowerBridge)
        {

            transform.GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z));

            GetComponentInChildren<Collider>().enabled = true;
            GetComponentInChildren<Rigidbody>().useGravity = true;
        }
	}

   
}
