using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerInfo
{
    [SerializeField]
    ///<summary>
    /// 플레이어의 닉네임
    ///</summary>
    private string nickname;

    [SerializeField]
    ///<summary>
    /// 플레이어의 현재 재화 보유량
    ///</summary>
    private int money;

    [SerializeField]
    ///<summary>
    /// 플레이어의 누적 경험치
    ///</summary>
    private int exp;

    [SerializeField]
    ///<summary>
    /// 픞레이어의 레벨
    ///</summary>
    private int level;

    /// <summary>
    /// 플레이어의 마지막 접속 시각
    /// </summary>
    public string lastConnectTime;



    /// <summary>
    /// 플레이어의 정보를 저장하는 객체 생성자
    /// </summary>
    /// <param name="_name">플레이어의 닉네임</param>
    public PlayerInfo(string _name)
    {
        nickname = _name;

        money = 0;
        exp = 0;
        level = 1;
        lastConnectTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public override string ToString()
    {
        return "nickname : " + nickname 
            + " / money : " + money 
            + " / exp : " + exp 
            + " / level : " + level 
            + " / lastConnectTime : " + lastConnectTime;
    }
}
