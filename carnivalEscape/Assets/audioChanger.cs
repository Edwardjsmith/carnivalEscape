﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioChanger : MonoBehaviour {


   AudioSource thisClip;

	// Use this for initialization
	void Start ()
    {
        thisClip = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(playerLook.Instance.tenMinsRemaining)
        {
            thisClip.pitch = -1;
        }
	}
}
