using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     int a =-1;
   TimingManager theTimingManager;
   public GameObject tex;
    public int n = 0;
   public GameObject _note;
    void Start()
    {
         _note = GameObject.FindWithTag("Note");
         tex = GameObject.FindWithTag("Button1");
      
    }

   public void OnMouseDown()
    {
         Random rR = new Random();
        n++;
        if (n <= 1) { 
        tex.SetActive(false);
        }
        Invoke("on",rR.rTime);
    }
    void on()
    {
        
        tex.SetActive(true);
    }
    void Update()
    {
        if (n >=2)
        {
    
            if (Input.GetMouseButtonDown(0)) { 
                theTimingManager = FindObjectOfType<TimingManager>();
            theTimingManager.CheckTiming();
                }
        }
    
      if(_note != null) { 
        GoNote();
        }
       
       
            }
  
   public void GoNote()
    {
         Random rRan = new Random();
       if(n >= 2) { 
       if(_note.transform.localPosition.y <= -400)
        {
            a = 1;
        }
       _note.transform.localPosition += new Vector3(0,a*rRan.rSpeed,0)*Time.deltaTime;
        }
    }
   
   

}
