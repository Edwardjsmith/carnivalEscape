using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropOnCollision : MonoBehaviour
{
    public bool carryObject;

    private void Update()
    {
        carryObject = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Floor" || collision.tag == "Wall")
        {
            carryObject = false;
        }
        
            
        
     
        
    }

}
