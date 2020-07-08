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
           return PlayerPrefs.GetInt("RicePerClick",3);
        }
        set
        {
            PlayerPrefs.SetInt("RicePerClick",value);
        }
    }


}