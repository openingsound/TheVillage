using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridTile
{
    // 현재 그리드 타일 위의 오브젝트 종류
    [SerializeField]
    private string type;
    public string Type { get { return type; } private set { type = value; } }

    // 현재 그리드 타일 위의 오브젝트 이름
    [SerializeField]
    private string name;
    public string Name { get { return name; } private set { name = value; } }

    // 현재 그리드 타일 위의 오브젝트 타입 열거형
    [SerializeField]
    private int typeInt;
    public int TypeInt { get { return typeInt; } private set { typeInt = value; } }

    // 오브젝트의 레벨
    [SerializeField]
    private int level;
    public int Level { get { return level; } private set { level = value; } }

    // 농작물 관련 오브젝트의 경우 자동수확인가
    [SerializeField]
    private bool isAuto;
    public bool IsAuto { get { return isAuto; } private set { isAuto = value; } }

    // 마지막 상태가 무엇인가
    [SerializeField]
    private string lastState;
    public string LastState { get { return lastState; } private set { lastState = value; } }

    // 마지막 식물의 상태 열거형
    [SerializeField]
    private int lastStateInt;
    public int LastStateInt { get { return lastStateInt; } set { lastStateInt = value; } }

    // 마지막으로 상태가 변한 것이 언제인가
    [SerializeField]
    private string lastStateTime;
    public string LastStateTime { get { return lastStateTime; } set { lastStateTime = value; } }



    public GridTile(string _type = "", string _name = "", int _typeInt = -1, int _level = 0, bool _isAuto = false, string _lastState = "", int _lastStateInt = -1, string _lastStateTime = "")
    {
        typeInt = _typeInt;
        type = _type;
        name = _name;
        level = _level;
        isAuto = _isAuto;
        lastState = _lastState;
        lastStateInt = _lastStateInt;
        lastStateTime = _lastStateTime;
    }


    public override string ToString()
    {
        string txt = "";

        txt += "type : " + type;
        txt += " / ";

        txt += "name : " + name;
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
