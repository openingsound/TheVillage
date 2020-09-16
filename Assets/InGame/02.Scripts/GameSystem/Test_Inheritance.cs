using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inheritance : MonoBehaviour
{
    public void OnEnable()
    {
        Debug.Log(this.gameObject.name + " Activate!");
    }

    public void OnDisable()
    {
        Debug.Log(this.gameObject.name + " Disactivate!");
    }
}