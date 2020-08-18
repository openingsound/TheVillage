using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Text percent;
    TimingManager _timingManager;
    void Update()
    {
        
        percent.text = "" + 
            _timingManager.drawNum;
    }
}
