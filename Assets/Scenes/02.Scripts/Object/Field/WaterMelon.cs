using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMelon : Object_Field
{
    public override void Plowing(GameObject crop, GameObject box)
    {
        Debug.Log("수박을 심었습니다...");

        // 오브젝트 이름은 사과나무
        name = "WaterMelon";

        // 밭을 가는데 걸리는 시간은 10초
        FieldPlowTime = 10; 

        // 수박의 열매 성장 시간은 10초
        CropGrowTime = 10;

        base.Plowing(crop, box);
    }
}
