using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : Item_Tree
{
    public override void Planting()
    {
        // 오브젝트 이름은 사과나무
        name = "AppleTree";

        // 사과 나무의 성장 시간은 10초
        base.treeGrowTime = 10; 

        // 사과 나무의 열매 성장 시간은 10초
        base.fruitGrowTime = 10;

        base.Planting();
    }
}
