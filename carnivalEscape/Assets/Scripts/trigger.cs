﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    
    public GameObject[] triggerObjects;
    public List<GameObject> colliders;
    public Light spotLight;
    
   
    private void OnCollisionStay(Collision other)
    {
        for (int i = 0; i < triggerObjects.Length; i++)
        {
            if (other.gameObject == triggerObjects[i].gameObject)
            {
                colliders.Add(other.gameObject);

                if (triggerObjects.Length == colliders.Count)
                {
                    playerLook.Instance.balanceWeights = true;
                    spotLight.color = Color.green;
                    Destroy(triggerObjects[1]);
                    Destroy(triggerObjects[0]);
                }
            }
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        for (int i = 0; i < triggerObjects.Length; i++)
        {
            if (other.gameObject == triggerObjects[i].gameObject)
            {
                colliders.Remove(other.gameObject);

            }
        }

    }

}
