using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    [SerializeField] Transform Center;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;



    public GameObject _timing;

    ppo _ppo;

    public int drawNum = 0;
    public int num = 0;

    bool reStart = false;

    PlayerController _playerController;

    Note _note;

    Character _character;
    bool boly = true;

    UIManager uIManager;
    void Start()
    {
        _timing = GameObject.FindWithTag("Timing");

        Center = _timing.transform;

        _note = FindObjectOfType<Note>();

        _playerController = FindObjectOfType<PlayerController>();

        _ppo = FindObjectOfType<ppo>();

        uIManager = FindObjectOfType<UIManager>();

        _character = FindObjectOfType<Character>();

        timingBoxs = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.y - timingRect[i].rect.height / 2
                , Center.localPosition.y + timingRect[i].rect.height / 2);
        }
    }





    public void CheckTiming()
    {


        for (int i = 0; i < timingBoxs.Length; i++)
        {
            float _posY = transform.localPosition.y;

            if (_posY <= timingBoxs[i].y && _posY >= timingBoxs[i].x)
            {
                num++;
                Debug.Log("HIt");

                _ppo.ppo0();

                Random rD = new Random();



                if (num == 1)
                {

                    drawNum += rD.rDraw;
                    OnDraw();
                }
                if (num > 1)
                {
                    drawNum += rD.rDraw;

                    OnDraw();

                    reStart = true;

                    boly = false;

                    _character.boly(boly);
                }
                return;
            }

        }

        _ppo.dro0();

        num++;
        Debug.Log("MISS");
        if (num > 1)
        {
            boly = false;
            _character.boly(boly);

            OnDraw();
            reStart = true;

        }

    }



    public void OnDraw()
    {

        if (num > 1)
        {

            Random rD = new Random();

            int dD = rD.rNum;

            if (drawNum != 0)
            {

                Debug.Log(dD);

                if (drawNum >= dD)
                {
                    uIManager.Success();


                }
                else
                {

                    uIManager.Fail();


                }


            }
            else
            {
                uIManager.Fail();
                Debug.Log("FAil");

            }
        }
    }

    private void Update()
    {


        if (reStart)
        {
            Debug.Log(drawNum);



            drawNum = 0;

            num = 0;

            reStart = false;

        }




    }
}