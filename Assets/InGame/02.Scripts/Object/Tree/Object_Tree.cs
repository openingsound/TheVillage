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
    /// <param name="_name">나무의 이름</param>
    /// <param name="_cycle">나무의 기본 수확 주기</param>
    /// <param name="_idx">그리드 상의 인덱스</param>
    /// <param name="_level">나무 레벨</param>
    /// <param name="_harvestCount">나무의 자동 수확 횟수</param>
    public void InitTree(string _name, int _cycle, int _idx, int _level = 1, int _harvestCount = 0)
    {
        // 오브젝트 이름 설정
        name = _name;

        // 오브젝트 종류는 나무
        type = "Tree";

        // 오브젝트의 수확주기 설정
        cycle = _cycle;

        // 레벨 초기화
        level = _level;

        // 그리드 상의 인덱스 초기화
        mapIdx = _idx;

        // 수확 횟수 초기화
        harvestCount = _harvestCount;

        // 오브젝트의 실제 수확 주기 초기화
        realCycle = cycle - (cycle / 10f) * (level - 1);
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

        //anim.Anim_SetLevel(level);

        // 나무의 상태 초기화
        //treeStateInit(TreeState.Bush, SizeState.S);
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
        //anim.Anim_DropBox();

        // 다시 열매 성장 상태로 초기화
        treeStateInit(TreeState.Fruit, SizeState.NULL, 0);
    }

    /// <summary>
    /// 나무의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newTreeState">나무의 새 성장 상태</param>
    /// <param name="newSizeState">나무의 새 크기 상태</param>
    /// <param name="pastTime">현재 상태로부터 지난 시간</param>
    public void treeStateInit(TreeState newTreeState, SizeState newSizeState, float pastTime = 0)
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
        anim.Anim_StateInit(newTreeState, newSizeState, ((newTreeState == TreeState.Harvest)? 0: (pastTime / totalTime)));
    }

    /// <summary>
    /// 작물을 업그레이드하는 함수
    /// </summary>
    public override void Upgrade()
    {
        if(level == MaxLevel)
        {
            Debug.Log("Error) 해당 오브젝트는 이미 최대 레벨입니다!");
            return;
        }

        // 최고 레벨이 아니면 레벨을 1 올린다
        level++;

        // 그리고 이를 그리드 타일에도 저장한다
        GridMap.Map.tiles[mapIdx].Level++;

        // 새 수확 주기
        float newLevelCycle = cycle - (cycle / 10f) * (level - 1);

        // 진척도를 0.0 ~ 1.0 사이로 표현
        float progress = ((float)(System.DateTime.Now - System.DateTime.Parse(GridMap.Map.tiles[mapIdx].LastStateTime)).Seconds) / realCycle;
        
        // 새 수확 주기 대입
        realCycle = newLevelCycle;

        // 상태 변경
        treeStateInit(growth, size, progress * newLevelCycle);
        
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
            if(harvestCount < Mathf.Max(0, (level - 2) * 2))
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
