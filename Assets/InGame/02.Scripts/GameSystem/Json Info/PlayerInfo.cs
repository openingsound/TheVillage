using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerInfo
{
    [SerializeField]
    private string nickname;

    [SerializeField]
    private int exp;

    [SerializeField]
    private int level;




    public PlayerInfo(string _name)
    {
        nickname = _name;

        exp = 0;
        level = 1;
    }
}
