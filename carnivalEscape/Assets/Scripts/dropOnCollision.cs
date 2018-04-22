﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropOnCollision : MonoBehaviour
{
    public bool carry;

    private void Update()
    {
        carry = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (transform.parent != null && collision)
        {
            carry = false;
        }
     
        
    }

}
