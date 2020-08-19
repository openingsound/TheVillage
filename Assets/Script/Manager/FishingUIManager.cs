using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FishingUIManager : MonoBehaviour
{
    public Text percent;
    public TimingManager _timingManager;
    void Update()
    {
        
        percent.text = "" + 
            _timingManager.drawNum;
    }
}
