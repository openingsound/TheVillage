using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class Auto : MonoBehaviour
{
    public GameObject tex;
   public string autoName;
    public int num =0;
    Coroutine myAuto;
   public bool isPress;
    void Start()
    {
        tex = GameObject.FindWithTag("Button1");
    }
    public void ClickButtonAuto()
    {       
        switch (num%2)
        {
            case 0:
               myAuto = StartCoroutine(AutoLoop()); 
            
                tex.SetActive(false);
                break;
                
                case 1:
                StartCoroutine(stoop());
                
                tex.SetActive(true);
                break;

                default:
                break;

        }
        num++;
        
    }
  
    IEnumerator AutoLoop()
    {
        while (true)
        {
            
            DataController.instance.rice += DataController.instance.ricePerClick;
            

            yield return new WaitForSeconds(3.0f);
        }
    }
    IEnumerator stoop()
    {
        yield return null;
        StopCoroutine(AutoLoop());
    }
}
