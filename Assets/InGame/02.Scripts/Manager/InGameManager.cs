using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager inGameManager { get; private set; }

    public GameManager ItemGameManager;

    public Test_Tree ConstructorManager;


    private void Awake()
    {
        inGameManager = this;
    }


    /// <summary>
    /// 해당 아이템의 작물을 심는 함수
    /// </summary>
    /// <param name="item">심고 싶은 아이템</param>
    public void Build(Item item)
    {
        Debug.Log("Build() - Item : " + item.ToString());

        // 만일 해당 작물이 나무에서 자란다면
        if(item.Tree == "Tree")
        {
            foreach(var fruit in System.Enum.GetValues(typeof(Plants_DB.Fruit)))
            {
                if(System.Enum.GetName(typeof(Plants_DB.Fruit), fruit) == item.EngName)
                {
                    Debug.Log("Plant fruit tree - " + fruit.ToString());
                    ConstructorManager.OnClickPlanting((int)fruit, item.Cycle);   
                    return;
                }
            }
        }
        // 만일 해당 작물이 밭에서 자란다면
        else
        {
            foreach (var crop in System.Enum.GetValues(typeof(Plants_DB.Crop)))
            {
                if (System.Enum.GetName(typeof(Plants_DB.Crop), crop) == item.EngName)
                {
                    ConstructorManager.OnClickPlowing((int)crop, item.Cycle);
                    return;
                }
            }
        }
    }
}
