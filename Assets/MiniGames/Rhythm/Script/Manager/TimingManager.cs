using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    // 라인의 중앙 위치 컴포넌트
    [SerializeField] Transform Center = null;

    //
    [SerializeField] RectTransform[] timingRect = null;

    //
    Vector2[] timingBoxs = null;

    Random rD = new Random();

    public int[] drawArray = new int[100];
    public int drawNum = 0;
    public int num = 0;

    void Start()
    {
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
                Debug.Log("HIt" + i);

                drawNum += rD.rDraw;

                if (num > 1)
                {
                    Destroy(GameObject.FindWithTag("Note"));
                    OnDraw();
                }
                return;
            }

        }
        num++;
        Debug.Log("MISS");
        if (num > 1)
        {
            Destroy(GameObject.FindWithTag("Note"));
            OnDraw();
        }
    }

    public void OnDraw()
    {
        int dD = rD.rDraw;
        if (drawNum != 0)
        {
            for (int i = 0; i < drawNum; i++)
            {
                drawArray[i] = 1;
                drawArray[i + drawNum] = 2;



                if (dD == i && drawArray[i] == 1)
                {
                    Debug.Log("Success");
                    break;
                }
                else if (dD == i && drawArray[i] == 2)
                {
                    Debug.Log("Fail");
                    break;
                }
            }
        }
        else
        {
            Debug.Log("FAil");
        }
    }

}
