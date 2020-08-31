using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : MonoBehaviour
{
   PlayerController _playerController;
    int a =-1;
       
 
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        Random rRan = new Random();

        if(_playerController.n >= 2) { 

            if(transform.localPosition.y <= -400)
            {
                a = 1;
            }
        
            transform.localPosition += new Vector3(0,a*rRan.rSpeed,0)*Time.deltaTime;
        }
    }
    
}

