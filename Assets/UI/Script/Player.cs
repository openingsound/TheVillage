using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxExp = 100;
    public int currentExp = 0;
    public int intLevel = 1;
    
    public int maxMoney = 100;
    public int currentMoney = 0;


    public LevelBar levelBar;
    public MoneyBar moneyBar;
    
    void Start()
    {
        currentExp = 0;
        //intLevel = 1;
        levelBar.SetMaxExp(maxExp);

        currentMoney = 0;
        //moneyBar.SetMaxMoney(maxMoney);
        //levelBar.SetLevel(1);
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ExpUp(10);
            MoneyUp(20);
        }
    }

    void ExpUp(int exp)
    {
        currentExp += exp;

        /*경험치 차면 레벨업
        if(currentExp >= maxExp)
        {
            levelBar.SetMaxExp(maxExp + 50);
            levelBar.SetExp(0);
            levelBar.SetLevel(intLevel);
        }*/
        levelBar.SetExp(currentExp);
    }

    void MoneyUp(int money)
    {
        currentMoney += money;

        levelBar.SetExp(currentMoney);
    }
}
