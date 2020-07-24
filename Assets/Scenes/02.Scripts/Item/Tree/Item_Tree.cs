using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Tree : Item
{
    #region Tree Basic Property

    // 나무의 상태 열거형
    public enum TreeState {TreeGrow, FruitGrow, Harvest };

    // 현재 나무의 상태
    public TreeState state { get; private set; }

    // 나무가 자라나는데 걸리는 시간
    [SerializeField]
    protected double treeGrowTime;

    // 열매가 자라나는데 걸리는 시간
    [SerializeField]
    protected double fruitGrowTime;

    // 상태 변환이 일어난 시각 (DateTime 구조체)
    [SerializeField]
    protected string stateStartTime;

    // 상태 변환이 종료되는 시각 (DateTime 구조체)
    [SerializeField]
    protected string stateEndTime;

    // 열매 오브젝트
    [SerializeField]
    protected GameObject fruit;

    #endregion



    #region Tree Basic Method

    public virtual void Planting()
    {
        // 아이템 종류는 나무
        type = "Tree";

        // 나무의 상태 초기화
        state = TreeState.TreeGrow;

        // 나무가 자라기 시작하는 시각정보
        stateStartTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 나무가 다 자라나는 시각정보
        stateEndTime = System.DateTime.Now.AddSeconds(treeGrowTime).ToString("yyyy-MM-dd HH:mm:ss");


        Debug.Log("Plant a tree : " + stateStartTime);
        Debug.Log("State Ending Time : " + stateEndTime);
    }


    public virtual void TreeGrowingComplete()
    {
        // 나무의 상태 초기화
        state = TreeState.FruitGrow;

        // 열매가 자라기 시작하는 시각정보
        stateEndTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 열매가 다 자라나는 시각정보
        stateEndTime = System.DateTime.Now.AddSeconds(treeGrowTime).ToString("yyyy-MM-dd HH:mm:ss");


        Debug.Log("Tree Growing Complete : " + stateStartTime);
        Debug.Log("State Ending Time : " + stateEndTime);
    }


    public virtual void FruitGrowingComplete()
    {
        // 나무의 상태 초기화
        state = TreeState.Harvest;

        Debug.Log("Fruit Growing Complete : " + stateStartTime);
        Debug.Log("State Ending Time : " + stateEndTime);
    }


    public virtual void FruitHarvestingComplete()
    {
        // 열매 아이템 떨구기


        // 나무의 상태 초기화
        state = TreeState.FruitGrow;

        // 열매가 자라기 시작하는 시각정보
        stateStartTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 열매가 다 자라나는 시각정보
        stateEndTime = System.DateTime.Now.AddSeconds(treeGrowTime).ToString("yyyy-MM-dd HH:mm:ss");


        Debug.Log("Fruit Harvesting Complete : " + stateStartTime);
        Debug.Log("State Ending Time : " + stateEndTime);
    }

    #endregion



    #region Tree Basic LifeCycle

    private void Update()
    {
        if(state == TreeState.Harvest)
        {
            return;
        }

        // 현재 시각
        string now = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("Now Time : " + now);

        // 현재 시각이 상태 종료 시각과 일치하는가
        if(now == stateEndTime)
        {
            switch(state)
            {
                case TreeState.TreeGrow:
                    TreeGrowingComplete();
                    break;

                case TreeState.FruitGrow:
                    FruitGrowingComplete();
                    break;
            } 
        }
    }

    #endregion
}
