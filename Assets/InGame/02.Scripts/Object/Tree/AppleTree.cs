using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AppleTree : Object_Tree
{
    /// <summary>
    /// 사과 나무가 처음 심어질 때 호출하는 함수
    /// </summary>
    /// <param name="tree">완전 성장한 나무 프리팹</param>
    /// <param name="crop">자라날 작물 프리팹</param>
    /// <param name="box">수확 시 드랍할 박스 프리팹</param>
    public override void Planting(GameObject tree, GameObject crop, GameObject box)
    {
        // 오브젝트 이름은 사과나무
        name = "AppleTree";

        // 사과 나무의 성장 시간은 10초
        treeGrowTime = 10; 

        // 사과 나무의 열매 성장 시간은 10초
        fruitGrowTime = 10;

        base.Planting(tree, crop, box);
    }
}
