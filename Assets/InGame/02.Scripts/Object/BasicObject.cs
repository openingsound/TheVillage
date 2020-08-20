using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObject : MonoBehaviour
{
    // 오브젝트의 타입
    public string type { get; protected set; }

    // 오브젝트의 이름
    public string name { get; protected set; }

    // 오브젝트의 레벨 정도
    public int level { get { return _level; } protected set { _level = value; } }
    [SerializeField, Range(1, 6)]
    private int _level;
}
