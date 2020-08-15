using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController instance;

    public static DataController Instance {

        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<DataController>();
                if(instance == null)
                {
                    GameObject container = new GameObject("DataController");
                    instance = container.AddComponent<DataController>();
                }
            }

            return instance;

        }

        }

   
    public long rice
    {
        get
        {
            if (!PlayerPrefs.HasKey("Rice"))
            {
                return 0;
            }

           string tmpRice = PlayerPrefs.GetString("Rice");
            
           return long.Parse(tmpRice);
        }
        set
        {
            PlayerPrefs.SetString("Rice",value.ToString());
        }
    }

    public int ricePerClick
    {
        get
        {
           return PlayerPrefs.GetInt("RicePerClick",1);
        }
        set
        {
            PlayerPrefs.SetInt("RicePerClick",value);
        }
    }

    public int gold
    {
        get
        {
            return PlayerPrefs.GetInt("Gold",1);
        }
        set
        {
            PlayerPrefs.SetInt("Gold",value);
        }
     }
    
    

    public int UpgradeButton
    {
        get
       {
            return PlayerPrefs.GetInt("level",1);

        }
        set
        {
            PlayerPrefs.SetInt("level",value);
        }
    }

  
}