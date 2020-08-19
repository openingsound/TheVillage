using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text level;
    public int intLevel;

    public void SetMaxExp(int exp)
    {
        slider.maxValue = exp;
        //slider.value = exp;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetExp(int exp)
    {
        slider.value = exp;

        fill.color = gradient.Evaluate(slider.normalizedValue);

    }

    

    /*
    public void SetLevel(int currentLevel)
    {
        currentLevel++;
        level.text = currentLevel.ToString();
    }
    
    */
}
