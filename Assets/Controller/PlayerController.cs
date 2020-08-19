using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 타이밍 매니저 컴포넌트
    TimingManager theTimingManager;

    // 버튼 오브젝트
    public GameObject tex;

    // 노트 오브젝트
    public GameObject _note;

    // 마우스 버튼이 감지된 횟수
    public int n = 0;

    // ???
    int a = -1;




    void Start()
    {
        // 노트 오브젝트 할당
        _note = GameObject.FindWithTag("Note");

        // 버튼 오브젝트 할당
        tex = GameObject.FindWithTag("Button1");

    }



    public void OnMouseDown()
    {
        // 랜덤 객체 -> 랜덤한 시간 대기
        Random rR = new Random();

        // 마우스 버튼이 내려가는 횟수를 1 증가
        n++;

        // 만일 마우스 버튼 클릭이 처음 일어난다면 버튼 오브젝트 비활성화
        if (n <= 1)
        {
            tex.SetActive(false);
        }

        // 랜덤한 시간을 대기 후 버튼 오브젝트 활성화
        Invoke("on", rR.rTime);
    }

    

    void Update()
    {
        // 만일 마우스 버튼이 2회이상 일어났다면
        if (n > 1)
        {
            // 마우스 버튼이 내려갔다면
            if (Input.GetMouseButtonDown(0))
            {
                // 타이밍 매니저 객체를 할당
                theTimingManager = FindObjectOfType<TimingManager>();

                // 
                theTimingManager.CheckTiming();
            }
        }

        // 만일 노트가 빈 객체가 아니라면
        if (_note != null)
        {
            // 노트를 움직인다
            GoNote();
        }


    }

    public void GoNote()
    {
        Random rRan = new Random();
        if (n >= 2)
        {
            if (_note.transform.localPosition.y <= -400)
            {
                a = 1;
            }
            _note.transform.localPosition += new Vector3(0, a * rRan.rSpeed, 0) * Time.deltaTime;
        }
    }



    /// <summary>
    /// 버튼 오브젝트를 다시 활성화시키는  함수
    /// </summary>
    void on()
    {
        tex.SetActive(true);
    }
}
