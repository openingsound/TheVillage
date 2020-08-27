using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Object_Tree : BasicObject
{
    #region Tree Basic Property

    /* 나무의 상태 관련 프로퍼티 */

    // 나무의 상태 열거형
    public enum TreeState { Bush = 0, Fruit = 10, Harvest = 20 };

    // 나무의 크기 열거형
    public enum SizeState { NULL, S, M, L };

    // 현재 나무의 상태
    public TreeState growth { get; private set; }
    
    // 현재 나무의 크기
    public SizeState size { get; private set; }

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
    public Anim_Tree anim;


    /* 나무의 수확 관련 프로퍼티 */

    // 자동수확을 할 것인가
    public bool isAuto;

    #endregion



    #region Tree Basic Method


    /// <summary>
    /// 나무의 기본적인 수치들을 초기화하는 함수
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="plowTime"></param>
    /// <param name="cropTime"></param>
    public void InitTree(string _name, float treeTime, float fruitTime, int _idx, int _level = 1, bool auto = false)
    {
        // 오브젝트 이름 설정
        name = _name;

        // 오브젝트 종류는 밭
        type = "Tree";

        // 밭을 가는데 걸리는 시간 설정
        treeGrowTime = treeTime;

        // 작물이 자라는 시간 설정
        fruitGrowTime = fruitTime;

        // 레벨은 1
        level = _level;

        // 자동 수확은 off
        isAuto = auto;

        mapIdx = _idx;
    }


    /// <summary>
    /// 나무가 처음 심어질 때 호출하는 함수
    /// </summary>
    /// <param name="tree">완전 성장한 나무 프리팹</param>
    /// <param name="crop">자라날 작물 프리팹</param>
    /// <param name="box">수확 시 드랍할 박스 프리팹</param>
    public virtual void Planting(GameObject tree, GameObject crop, GameObject box)
    {
        // 나무 애니메이션 초기화
        anim.Anim_Init(tree, crop, box);

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
    public virtual void treeStateInit(TreeState newTreeState, SizeState newSizeState, float endTime, bool isHarvest = false, float startTimeRate = 0)
    {
        // 상태 변수의 값 변경
        growth = newTreeState;

        GridMap.Map.tiles[mapIdx].LastStateInt = (int)newTreeState + (int)newSizeState;

        if(startTimeRate != 1)
        {
            GridMap.Map.tiles[mapIdx].LastStateTime = System.DateTime.Now.AddSeconds(
            (double)(endTime - (endTime / (1 - startTimeRate)))).ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            double addSeconds;

            if(newTreeState == TreeState.Bush)
            {
                addSeconds = treeGrowTime / 3;
            }
            else if(newTreeState == TreeState.Fruit)
            {
                addSeconds = fruitGrowTime;
            }
            else
            {
                addSeconds = 0;
            }

            GridMap.Map.tiles[mapIdx].LastStateTime = System.DateTime.Now.AddSeconds(-1 * addSeconds).ToString("yyyy-MM-dd HH:mm:ss");
        }
        

        // 크기 변수의 값 변경
        size = newSizeState;

        // 상태 종료때의 시간
        stateEndTime = System.DateTime.Now.AddSeconds(endTime).ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("State Ending Time : " + stateEndTime);

        // 상태 애니메이션 초기화 함수 실행
        anim.AnimStateInit(newTreeState, newSizeState, isHarvest, startTimeRate);
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
                        treeStateInit(TreeState.Fruit, SizeState.NULL, fruitGrowTime);
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
