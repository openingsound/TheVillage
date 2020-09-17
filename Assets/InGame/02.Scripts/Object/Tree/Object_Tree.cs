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
    //[SerializeField]
    //public float treeGrowTime { get; protected set; }

    // 열매가 자라나는데 걸리는 시간
    //[SerializeField]
    //public float fruitGrowTime { get; protected set; }

    // 상태 변환이 일어난 시각 (DateTime 구조체)
    //[SerializeField]
    //protected string stateStartTime;

    // 상태 변환이 종료되는 시각 (DateTime 구조체)
    [SerializeField]
    public string stateEndTime { get; protected set; }



    /* 나무의 애니메이션 관련 프로퍼티 */

    // 애니메이션 스크립트
    public Anim_Tree anim;


    /* 나무의 수확 관련 프로퍼티 */

    // 자동수확을 할 것인가
    //public bool isAuto;

    #endregion



    #region Tree Basic Method


    /// <summary>
    /// 나무의 기본적인 수치들을 초기화하는 함수
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="plowTime"></param>
    /// <param name="cropTime"></param>
    public void InitTree(string _name, int _cycle, int _idx, int _level = 1)
    {
        // 오브젝트 이름 설정
        name = _name;

        // 오브젝트 종류는 밭
        type = "Tree";

        // 오브젝트의 수확주기 설정
        cycle = _cycle;

        // 레벨은 1
        level = _level;

        mapIdx = _idx;
    }


    /// <summary>
    /// 나무가 처음 심어질 때 호출하는 함수
    /// </summary>
    /// <param name="tree">완전 성장한 나무 프리팹</param>
    /// <param name="crop">자라날 작물 프리팹</param>
    /// <param name="box">수확 시 드랍할 박스 프리팹</param>
    public void Planting(GameObject tree, GameObject crop, GameObject box)
    {
        // 나무 애니메이션 초기화
        anim.Anim_Init(tree, crop, box);

        anim.Anim_SetLevel(level);

        // 나무의 상태 초기화
        treeStateInit(TreeState.Bush, SizeState.S);
    }


    /// <summary>
    /// 열매를 수확할 때 호출하는 함수
    /// </summary>
    public void FruitHarvesting()
    {
        // harvestCount 1 증가
        harvestCount++;

        // 임시 - 이전에 떨구었던 열매 아이템 제거 애니메이션 함수
        //anim.Anim_GetFruitBox();

        // 열매 아이템 떨구기
        anim.Anim_DropBox();

        // 다시 열매 성장 상태로 초기화
        treeStateInit(TreeState.Fruit, SizeState.NULL, 0, true);
    }

    /// <summary>
    /// 나무의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newTreeState">나무의 새 성장 상태</param>
    /// <param name="newSizeState">나무의 새 크기 상태</param>
    /// <param name="pastTime">현재 상태로부터 지난 시간</param>
    /// <param name="isHarvest">수확을 하려 하는가 (수확 가능 상태에 바로 안되고, 그 다음에 따고서 Fruit 상태로 돌아갈 때 isHarvest를 쓴다)</param>
    public void treeStateInit(TreeState newTreeState, SizeState newSizeState, float pastTime = 0, bool isHarvest = false)
    {
        // 상태 변수의 값 변경
        growth = newTreeState;

        GridMap.Map.tiles[mapIdx].CountHarvest = harvestCount;

        GridMap.Map.tiles[mapIdx].LastStateInt = (int)newTreeState + (int)newSizeState;

        // 상태변화에 필요한 전체 시간
        float totalTime;

        if (newTreeState != TreeState.Harvest)
        {
            totalTime = cycle;
        }
        else
        {
            totalTime = 0;
        }

        // 아직 상태 변화 중이라면
        if (pastTime != totalTime)
        {
            // 마지막 상태 변화는 지난 시간만큼 뺀 값
            GridMap.Map.tiles[mapIdx].LastStateTime = System.DateTime.Now.AddSeconds(
            (double)(pastTime * -1)).ToString("yyyy-MM-dd HH:mm:ss");
        }
        // 만일 상태 변화가 딱 끝났다면
        else
        {
            // 마지막 상태 변화는 현 상태의 변화 주기만큼 뺀값
            GridMap.Map.tiles[mapIdx].LastStateTime = System.DateTime.Now.AddSeconds(-1 * totalTime).ToString("yyyy-MM-dd HH:mm:ss");
        }

        // 크기 변수의 값 변경
        size = newSizeState;

        // 상태 종료때의 시간
        stateEndTime = System.DateTime.Now.AddSeconds(totalTime - pastTime).ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("State Ending Time : " + stateEndTime);

        // 상태 애니메이션 초기화 함수 실행
        anim.Anim_StateInit(newTreeState, newSizeState, isHarvest, (pastTime / totalTime));
    }

    /// <summary>
    /// 작물을 업그레이드하는 함수
    /// </summary>
    public void Upgrade(int newCycle)
    {
        if (level == MaxLevel)
        {
            Debug.Log("Error) 해당 오브젝트는 이미 최대 레벨입니다!");
            return;
        }

        // 최고 레벨이 아니면 레벨을 1 올린다
        level++;

        Debug.Log("현재 레벨 : " + level);

        // GridMap에 레벨 저장
        InGameManager.inGameManager.gridSystem.tiles[InGameManager.inGameManager.gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos)].Level++;

        float startRate = 1 - (((float)(System.DateTime.Parse(stateEndTime) - System.DateTime.Now).TotalSeconds) / cycle);

        System.DateTime newStateStartTime = System.DateTime.Now.AddSeconds(-1 * startRate * newCycle);

        // 완전히 성장하지 않았을 때
        System.DateTime newStateEndTime = System.DateTime.Now.AddSeconds((1 - startRate) * newCycle);

        stateEndTime = newStateEndTime.ToString("yyyy-MM-dd HH:mm:ss");

        InGameManager.inGameManager.gridSystem.tiles[InGameManager.inGameManager.gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos)].LastStateTime
            = newStateStartTime.ToString("yyyy-MM-dd HH:mm:ss");

        // 수확 시 필요한 성장 시간 새로 대입
        cycle = newCycle;

        // 레벨별 초기화 애니메이션 실행
        anim.Anim_SetLevel(level);

        anim.Anim_StateInit(growth, size, false, startRate);

        
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
            if(harvestCount <= (level - 2) * 2)
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
        //Debug.Log("Now Time : " + now);

        // 현재 시각이 상태 종료 시각과 일치하는가
        if(now == stateEndTime)
        {
            // 만일 묘목이 성장 중이라면
            if(growth == TreeState.Bush)
            {
                switch(size)
                {
                    case SizeState.S:
                        treeStateInit(TreeState.Bush, SizeState.M);
                        break;

                    case SizeState.M:
                        treeStateInit(TreeState.Bush, SizeState.L);
                        break;

                    case SizeState.L:
                        treeStateInit(TreeState.Fruit, SizeState.NULL);
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
