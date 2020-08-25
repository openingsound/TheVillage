using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inheritance : MonoBehaviour
{
    public GameObject something;

    private void Update()
    {
        if(something == null)
        {
            Debug.LogError("오브젝트가 사라졌습니다!!!");
        }
        else
        {
            Debug.Log("오브젝트 위치 - " + something.transform.position.ToString());
        }
    }
}
