using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Text riceDisplayer;
   
    void Update()
    {
        riceDisplayer.text = "rice: " + DataController.Instance.rice;
    }
}
