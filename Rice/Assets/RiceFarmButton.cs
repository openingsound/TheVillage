using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class RiceFarmButton : MonoBehaviour
{
 
    public GameObject tex;
     private bool press = false;
    
   
    public void OnMouseDown()
    {
        tex = GameObject.FindWithTag("Button1");
        DataController.instance.rice += DataController.instance.ricePerClick;
        tex.SetActive(false);
        Invoke("On",3);
      
    }

    public void On()
    {
        tex.SetActive(true);
    }


 
  }
