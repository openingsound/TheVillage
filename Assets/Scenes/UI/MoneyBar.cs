using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxMoney(int money)
    {
        slider.maxValue = money;
        //slider.value = money;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetMoney(int money)
    {
        slider.value = money;

        fill.color = gradient.Evaluate(slider.normalizedValue);

    }


}
