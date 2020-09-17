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
    private int _money;
    public int money { get { return _money; } set { _money = value; } }

    [SerializeField]
    ///<summary>
    /// 플레이어의 누적 경험치
    ///</summary>
    private int _exp;
    public int exp { get { return _exp; } set { _exp = value; } }

    [SerializeField]
    ///<summary>
    /// 픞레이어의 레벨
    ///</summary>
    private int _level;
    public int level { get { return _level; }
        set {
            InGameManager.inGameManager.BackGround[Mathf.Max((level - 1), 0)].SetActive(false);
            _level = value;
            ChangeLevel();
        }
    }

    /// <summary>
    /// 플레이어의 마지막 접속 시각
    /// </summary>
    public string lastConnectTime;



    /// <summary>
    /// 플레이어의 정보를 저장하는 객체 생성자
    /// </summary>
    /// <param name="_name">플레이어의 닉네임</param>
    public PlayerInfo(string _name, int money = 0, int exp = 0, int level = 1)
    {
        nickname = _name;

        _money = money;
        _exp = exp;
        _level = level ;
        lastConnectTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public override string ToString()
    {
        return "nickname : " + nickname 
            + " / money : " + _money 
            + " / exp : " + _exp 
            + " / level : " + _level 
            + " / lastConnectTime : " + lastConnectTime;
    }

    private void ChangeLevel()
    {
        InGameManager.inGameManager.BackGround[(level - 1)].SetActive(true);
        InGameManager.inGameManager.gridSystem.ResizeGrid(2 * level + 1);
    }
}
