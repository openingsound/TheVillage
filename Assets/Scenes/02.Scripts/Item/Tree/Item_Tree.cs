using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Tree : Item
{
    #region Tree Basic Property

    /* 나무의 상태 관련 프로퍼티 */

    // 나무의 상태 열거형
    public enum TreeState { Bush, Fruit, Harvest };

    // 나무의 크기 열거형
    public enum SizeState { S, M, L, NULL };

    // 현재 나무의 상태
    public TreeState growth { get; private set; }
    
    // 현재 나무의 크기
    public SizeState size { get; private set; }

    // 현재 나무의 크기

    // 나무가 자라나는데 걸리는 시간
    [SerializeField]
    public float treeGrowTime { get; protected set; }

    // 열매가 자라나는데 걸리는 시간
    [SerializeField]
    public float fruitGrowTime { get; protected set; }

    // 상태 변환이 일어난 시각 (DateTime 구조체)
    //[SerializeField]
    //protected string stateStartTime;

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
        treeStateInit(TreeState.Bush, SizeState.S, treeGrowTime / 3);
    }


    /// <summary>
    /// 열매를 수확할 때 호출하는 함수
    /// </summary>
    public virtual void FruitHarvesting()
    {
        // 임시 - 이전에 떨구었던 열매 아이템 제거 애니메이션 함수
        anim.Anim_GetFruitBox();

        // 열매 아이템 떨구기
        anim.Anim_DropBox();

        // 다시 열매 성장 상태로 초기화
        treeStateInit(TreeState.Fruit, SizeState.S, fruitGrowTime / 3);
    }


    /// <summary>
    /// 나무의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newTreeState"></param>
    /// <param name="newSizeState"></param>
    /// <param name="endTime"></param>
    public virtual void treeStateInit(TreeState newTreeState, SizeState newSizeState, float endTime)
    {
        // 상태 변수의 값 변경
        growth = newTreeState;

        // 크기 변수의 값 변경
        size = newSizeState;

        // 상태 종료때의 시간
        stateEndTime = System.DateTime.Now.AddSeconds(endTime).ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("State Ending Time : " + stateEndTime);

        // 상태 애니메이션 초기화 함수 실행
        anim.AnimStateInit(newTreeState, newSizeState);
    }

    #endregion



    #region Tree Basic LifeCycle


    private void Awake()
    {
        anim = this.GetComponent<Anim_Tree>();
    }

    private void FixedUpdate()
    {
        if(growth == TreeState.Harvest)
        {
            if(isAuto)
            {
                // 수확하는 함수
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
            // 상태 변경

            // 만일 사이즈가 L이면 다음 상태로 넘어감
            if(size == SizeState.L)
            {
                switch(growth)
                {
                    // 묘목 -> 열매 성장
                    case TreeState.Bush:
                        treeStateInit(TreeState.Fruit, SizeState.S, fruitGrowTime / 3);
                        break;

                    // 열매 성장 -> 수확 대기
                    case TreeState.Fruit:
                        treeStateInit(TreeState.Harvest, SizeState.NULL, 0);
                        break;
                }

                return;
            }
            // 아직 묘목이나 열매가 성장중이라면
            else
            {
                // 다음 크기 상태로 넘어감
                
                switch(growth)
                {
                    case TreeState.Bush:
                        treeStateInit(growth, ++size, treeGrowTime / 3);
                        break;
                    
                    case TreeState.Fruit:
                        treeStateInit(growth, ++size, fruitGrowTime / 3);
                        break;
                }

                return;
            }

        }
    }

    #endregion
}
