using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    public GameObject _Btn;

    private float done = 35.0f; // 시간 제한 35초

    public Text _time; //시간 텍스트

    public GameObject _gameOver;

    private void Start()
    {
        _Btn = GameObject.FindWithTag("Button1");
        _gameOver.SetActive(false);
    }
    void Update()
    {

        if (done > 0F)
        {

            done -= Time.deltaTime; //시간 -1

            _time.text = "" + (int)done;

        }

        else
        {
            _Btn.SetActive(false);
            _gameOver.SetActive(true);

        }



    }
}
