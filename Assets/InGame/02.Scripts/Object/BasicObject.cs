using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicObject : MonoBehaviour
{
    /// <summary>
    /// 오브젝트의 최대 레벨
    /// </summary>
    public const int MaxLevel = 5;

    /// <summary>
    /// 오브젝트의 타입
    /// </summary>
    public string type { get; protected set; }

    /// <summary>
    /// 오브젝트의 이름
    /// </summary>
    public string name { get; protected set; }

    /// <summary>
    /// 오브젝트의 레벨 
    /// </summary>
    public int level { get { return _level; } protected set { _level = value; } }
    [SerializeField, Range(1, 5)]
    private int _level;

    /// <summary>
    ///  오브젝트가 자동수확을 한 횟수
    /// </summary>
    public int harvestCount { get; set; }

    /// <summary>
    /// 오브젝트의 기본 수확주기 (건설주기는 수확주기의 2배) (단위 : 초)
    /// </summary>
    public int cycle { get; protected set; }

    /// <summary>
    /// 실제 작물의 사이클 (단위 : 초)
    /// </summary>
    public float realCycle { get; protected set; }

    /// <summary>
    /// 현재 오브젝트의 인덱스
    /// </summary>
    public int mapIdx { get; protected set; }

    /// <summary>
    /// 오브젝트의 업그레이드를 하는 함수
    /// </summary>
    public abstract void Upgrade();
}
