using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    
    public GameObject[] triggerObjects;

    public List<GameObject> colliders;
    
    
   
    private void OnCollisionEnter(Collision other)
    {
        for (int i = 0; i < triggerObjects.Length; i++)
        {
            if (other.gameObject == triggerObjects[i].gameObject)
            {
                colliders.Add(other.gameObject);

                if (triggerObjects.Length == colliders.Count)
                {
                    Debug.Log("I am triggered!!!!!!");
                }
                else
                {
                    Debug.Log("I am not triggered");
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
