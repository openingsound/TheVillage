using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridTile
{
    // 현재 그리드 타일 위의 오브젝트 종류
    [SerializeField]
    private string type;

    // 현재 그리드 타일 위의 오브젝트 이름
    [SerializeField]
    private string name;

    // 오브젝트의 레벨
    [SerializeField]
    private int level;

    // 농작물 관련 오브젝트의 경우 자동수확인가
    [SerializeField]
    private bool isAuto;

    // 마지막 상태가 무엇인가
    [SerializeField]
    private string lastState;

    // 마지막으로 상태가 변한 것이 언제인가
    [SerializeField]
    private string lastStateTime;



    public GridTile(string _type = "", string _name = "", int _level = 0, bool _isAuto = false, string _lastState = "", string _lastStateTime = "")
    {
        type = _type;
        name = _name;
        level = _level;
        isAuto = _isAuto;
        lastState = _lastState;
        lastStateTime = _lastStateTime;
    }


    public override string ToString()
    {
        string txt = "";

        txt += "type : " + type;
        txt += " / ";

        txt += "level : " + level;
        txt += " / ";

        txt += "isAuto : " + isAuto;
        txt += " / ";

        txt += "lastState : " + lastState;
        txt += " / ";

        txt += "lastStateTime : " + lastStateTime;

        return txt;
    }
}
