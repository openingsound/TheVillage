using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    
    public Text upgradeDisplayer;
    
    public string upgradeName;

    public int goldByUpgrade;

    public int startGoldByUpgrade =1;

    

    public int startCurrentCost =1;

    public int level = 1;

    public float upgradePow = 2.24f;

    public float costPow = 5.13f;

    public int currentCost = 1;

    void Start()
    {
        UpdateUpgrade();
        UpdateUI();
    }
    public void PurchaseUpgrade()
    {
      if(DataController.instance.gold >= currentCost)
        {
            DataController.instance.gold -= currentCost;
            level++;
            DataController.instance.ricePerClick += DataController.instance.ricePerClick*(int)Mathf.Pow(upgradePow,level);
       
            UpdateUpgrade();
            UpdateUI();
            }
    }

    public void UpdateUpgrade()
    {
        currentCost = startCurrentCost*(int)Mathf.Pow(costPow,level);

        }

    public void UpdateUI()
    {
        upgradeDisplayer.text = upgradeName + "\nCost: " + currentCost + "\nLevel: " + level;
    }

}
