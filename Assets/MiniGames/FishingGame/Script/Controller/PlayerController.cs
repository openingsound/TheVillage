using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
     Note _note;

    Random rR = new Random();

   TimingManager theTimingManager;

   public GameObject tex;

    public GameObject button;

    Character _character;
    
    bool boly = false;

    Button1 _button;
    
    void Start()
    {
        _button = FindObjectOfType<Button1>();

        button = GameObject.FindWithTag("Button"); //낚기 버튼

        _note = FindObjectOfType<Note>(); 

        _character = FindObjectOfType<Character>();

         tex = GameObject.FindWithTag("Button1"); //미끼버튼
      
    }
    
   public void OnClick()
    {      
       

        tex.SetActive(false);

        _button.ButtonInteractable(boly);  //버튼1 비활성화

            boly = true;

            _character.boly(boly); //캐릭터 움직임
        
        

        Invoke("on",rR.rTime);

    }

    
    void on()
    {     
        tex.SetActive(true);   

         

         _button.ButtonInteractable(boly);  //버튼1 활성화

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
