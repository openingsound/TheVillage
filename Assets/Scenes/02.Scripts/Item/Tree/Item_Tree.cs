using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Tree : Item
{
    #region Tree Basic Property

    /* 나무의 상태 관련 프로퍼티 */

    // 나무의 상태 열거형
    public enum TreeState {TreeGrow, FruitGrow, Harvest };

    // 현재 나무의 상태
    public TreeState state { get; private set; }

    // 나무가 자라나는데 걸리는 시간
    [SerializeField]
    public float treeGrowTime { get; protected set; }

    // 열매가 자라나는데 걸리는 시간
    [SerializeField]
    public float fruitGrowTime { get; protected set; }

    // 상태 변환이 일어난 시각 (DateTime 구조체)
    [SerializeField]
    protected string stateStartTime;

    // 상태 변환이 종료되는 시각 (DateTime 구조체)
    [SerializeField]
    protected string stateEndTime;



    /* 나무의 애니메이션 관련 프로퍼티 */

    // 애니메이션 스크립트
    protected Anim_Tree anim;



    /* 나무의 수확 관련 프로퍼티 */

    // 자동수확을 할 것인가
    public bool isAuto;

    #endregion



    #region Tree Basic Method


    /// <summary>
    /// 나무가 처음 심어질 때 호출하는 함수
    /// </summary>
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


        // 애니메이션 실행
        anim.Anim_PlantingTree();


        Debug.Log("Plant a tree : " + stateStartTime);
        Debug.Log("State Ending Time : " + stateEndTime);
    }


    /// <summary>
    /// 나무의 성장이 완료되었을 때 호출하는 함수
    /// </summary>
    public virtual void TreeGrowingComplete()
    {
        // 나무의 상태 초기화
        state = TreeState.FruitGrow;

        // 열매가 자라기 시작하는 시각정보
        stateEndTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 열매가 다 자라나는 시각정보
        stateEndTime = System.DateTime.Now.AddSeconds(treeGrowTime).ToString("yyyy-MM-dd HH:mm:ss");

        // 애니메이션 실행
        anim.Anim_FruitGrowthStart();


        Debug.Log("Tree Growing Complete : " + stateStartTime);
        Debug.Log("State Ending Time : " + stateEndTime);
    }



    /// <summary>
    /// 열매의 성장이 완료되었을 때 호출하는 함수
    /// </summary>
    public virtual void FruitGrowingComplete()
    {
        // 나무의 상태 초기화
        state = TreeState.Harvest;

        // 열매 성장 완료 애니메이션 실행
        anim.Anim_FruitGrowthEnd();

        Debug.Log("Fruit Growing Complete : " + stateStartTime);
        Debug.Log("State Ending Time : " + stateEndTime);
    }



    /// <summary>
    /// 열매를 수확할 때 호출하는 함수
    /// </summary>
    public virtual void FruitHarvesting()
    {
        // 임시 - 이전에 떨구었던 열매 아이템 제거 애니메이션 함수
        anim.Anim_GetFruitItem();

        // 열매 아이템 떨구기
        anim.Anim_HarvestStart();

        // 열매가 다시 자라나는 함수 실행
        this.TreeGrowingComplete();
    }

    #endregion



    #region Tree Basic LifeCycle


    private void Awake()
    {
        anim = this.GetComponent<Anim_Tree>();
    }

    private void Update()
    {
        if(state == TreeState.Harvest)
        {
            if(isAuto)
            {
                FruitHarvesting();
            }
            else
            {
                return;
            }
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
