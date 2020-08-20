using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile
{
    // 현재 그리드 타일 위의 오브젝트 종류
    string type;

    // 오브젝트의 레벨
    int level;

    // 농작물 관련 오브젝트의 경우 자동수확인가
    bool isAuto;

    // 마지막 상태가 무엇인가
    string lastState;

    // 마지막으로 상태가 변한 것이 언제인가
    string lastStateTime;



    public GridTile(string _type = "", int _level = 0, bool _isAuto = false, string _lastState = "", string _lastStateTime = "")
    {
        type = _type;
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
