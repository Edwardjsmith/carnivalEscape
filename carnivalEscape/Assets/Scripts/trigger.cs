using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    
    public GameObject[] triggerObjects;
    public List<GameObject> colliders;
    public Light spotLight;
    
   
    private void OnCollisionEnter(Collision other)
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
                    Destroy(triggerObjects[i]);
                }
            }
            else
            {
                spotLight.color = Color.red;
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
                spotLight.color = Color.white;
            }
        }

    }

}
