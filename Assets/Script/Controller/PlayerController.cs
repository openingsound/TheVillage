using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     Note _note;

    Random rR = new Random();

   TimingManager theTimingManager;

   public GameObject tex;

    public GameObject button;

    Character _character;
    
    bool boly = false;

    
    void Start()
    {
        

        button = GameObject.FindWithTag("Button"); //낚기 버튼

        _note = FindObjectOfType<Note>(); 

        _character = FindObjectOfType<Character>();

         tex = GameObject.FindWithTag("Button1"); //미끼버튼
      
    }
    
   public void OnClick()
    {      
       button.SetActive(false);

        tex.SetActive(false);

            boly = true;

            _character.boly(boly); //캐릭터 움직임
        
        Invoke("on",rR.rTime);

    }

    
    void on()
    {     
        tex.SetActive(true);   

        button.SetActive(true);  

        _note.aManager(boly); // 노트의 a값 변경을 위함

        boly = false;

    }

    public void OnClick1()
    {
                   
            theTimingManager = FindObjectOfType<TimingManager>();

            theTimingManager.CheckTiming();
                
    
    }

    void Update()
    {
       
              button = GameObject.FindWithTag("Button");
    
       
       
            }
  
    

   
    

}
