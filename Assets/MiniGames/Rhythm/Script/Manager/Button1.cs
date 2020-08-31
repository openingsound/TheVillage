using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button1 : MonoBehaviour
{
    public Button _btn;

    private void Start()
    {
        _btn.interactable = false;
    }

    public void ButtonInteractable(bool i)  //버튼1 비활성화
    {
        bool k = i;
        
        _btn.interactable = k;
    }
}
