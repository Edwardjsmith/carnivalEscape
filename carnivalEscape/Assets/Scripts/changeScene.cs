using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{

    // Use this for initialization


    // Update is called once per frame

    public void change(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}


	
