using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Tree : BasicObject
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
    /// <param name="tree">완전 성장한 나무 프리팹</param>
    /// <param name="crop">자라날 작물 프리팹</param>
    /// <param name="box">수확 시 드랍할 박스 프리팹</param>
    public virtual void Planting(GameObject tree, GameObject crop, GameObject box)
    {
        // 밭 애니메이션 초기화
        anim.Anim_Init(tree, crop, box);

        // 아이템 종류는 나무
        type = "Tree";

        // 레벨은 1
        level = 1;

        // 자동 수확은 off
        isAuto = false;

        anim.Anim_SetLevel(level);

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
        treeStateInit(TreeState.Fruit, SizeState.NULL, fruitGrowTime, true);
    }


    /// <summary>
    /// 나무의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newTreeState"></param>
    /// <param name="newSizeState"></param>
    /// <param name="endTime"></param>
    public virtual void treeStateInit(TreeState newTreeState, SizeState newSizeState, float endTime, bool isHarvest = false)
    {
        // 상태 변수의 값 변경
        growth = newTreeState;

        // 크기 변수의 값 변경
        size = newSizeState;

        // 상태 종료때의 시간
        stateEndTime = System.DateTime.Now.AddSeconds(endTime).ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("State Ending Time : " + stateEndTime);

        // 상태 애니메이션 초기화 함수 실행
        anim.AnimStateInit(newTreeState, newSizeState, isHarvest);
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
            // 만일 묘목이 성장 중이라면
            if(growth == TreeState.Bush)
            {
                switch(size)
                {
                    case SizeState.S:
                        treeStateInit(TreeState.Bush, SizeState.M, treeGrowTime / 3);
                        break;

                    case SizeState.M:
                        treeStateInit(TreeState.Bush, SizeState.L, treeGrowTime / 3);
                        break;

                    case SizeState.L:
                        treeStateInit(TreeState.Fruit, SizeState.NULL, treeGrowTime / 3);
                        break;
                }
            }
            // 만일 과일 성장이 다 끝나면
            else
            {
                treeStateInit(TreeState.Harvest, SizeState.NULL, 0);
            }
        }
    }

    #endregion
}
