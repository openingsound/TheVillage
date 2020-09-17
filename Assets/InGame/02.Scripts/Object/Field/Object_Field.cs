using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Object_Field : BasicObject
{
    #region Field Basic Property

    /* 밭의 상태 관련 프로퍼티 */

    // 밭의 상태 열거형
    public enum FieldState { NULL = -1, Plow = 0, Grow = 10, Harvest = 20 };

    // 밭의 크기 열거형
    public enum SizeState { NULL, S, M, L };

    // 현재 밭의 상태
    public FieldState growth { get; protected set; }
    
    // 현재 작물의 크기
    public SizeState size { get; protected set; }

    // 현재 나무의 크기

    // 밭을 가는데 걸리는 시간
    //public float FieldPlowTime { get; protected set; }

    // 열매가 자라나는데 걸리는 시간
    //public float CropGrowTime { get; protected set; }

    // 상태 변환이 종료되는 시각 (DateTime 구조체)
    [SerializeField]
    protected string stateEndTime;



    /* 밭의 애니메이션 관련 프로퍼티 */

    // 애니메이션 스크립트
    public Anim_Field anim;


    /* 밭의 수확 관련 프로퍼티 */

    // 자동수확을 할 것인가
    //public bool isAuto;

    #endregion



    #region Field Basic Method


    /// <summary>
    /// 밭의 기본적인 수치들을 초기화하는 함수
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="plowTime"></param>
    /// <param name="cropTime"></param>
    public void InitField(string _name, int _cycle, int _idx, int _level = 1)
    {
        // 오브젝트 이름 설정
        name = _name;

        // 오브젝트 종류는 밭
        type = "Field";

        // 오브젝트의 수확주기
        cycle = _cycle;

        // 레벨은 1
        level = _level;

        // 오브젝트가 자동 수확을 진행한 횟수
        harvestCount = 0;

        mapIdx = _idx;
    }



    /// <summary>
    /// 밭을 처음 만들 때 호출하는 함수
    /// </summary>
    /// <param name="bush">성장하는 작물 프리팹</param>
    /// <param name="crop">다 성장한 작물 프리팹</param>
    /// <param name="box">수확 시 드랍될 아이템 박스 프리팹</param>
    public virtual void Plowing(GameObject bush, GameObject crop, GameObject box)
    {
        // 밭 애니메이션 초기화
        anim.Anim_Init(bush, crop, box);

        // 밭 레벨에 따른 애니메이션 설정
        anim.Anim_SetLevel(level);

        // 나무의 상태 초기화
        fieldStateInit(FieldState.Plow, SizeState.NULL);
    }


    /// <summary>
    /// 열매를 수확할 때 호출하는 함수
    /// </summary>
    public virtual void FruitHarvesting()
    {
        harvestCount++;

        // 임시 - 이전에 떨구었던 열매 아이템 제거 애니메이션 함수
        //anim.Anim_GetFruitBox();

        // 열매 아이템 떨구기
        anim.Anim_DropBox();

        //anim.Anim_SetLevel(level);

        // 다시 열매 성장 상태로 초기화
        fieldStateInit(FieldState.Grow, SizeState.S, 0, true);
    }

    /// <summary>
    /// 밭의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newFieldState">밭의 새 성장 상태</param>
    /// <param name="newSizeState">밭의 새 크기 상태</param>
    /// <param name="pastTime">현재 상태에서 지난 시간</param>
    /// <param name="isHarvest">수확을 하려 하는가 (수확 가능 상태에 바로 안되고, 그 다음에 따고서 Fruit 상태로 돌아갈 때 isHarvest를 쓴다)</param>
    public virtual void fieldStateInit(FieldState newFieldState, SizeState newSizeState, float pastTime = 0, bool isHarvest = false)
    {
        // 상태 변수의 값 변경
        growth = newFieldState;

        GridMap.Map.tiles[mapIdx].CountHarvest = harvestCount;

        GridMap.Map.tiles[mapIdx].LastStateInt = (int)newFieldState + (int)newSizeState;

        // 상태변화에 필요한 전체 시간
        float totalTime;

        if (newFieldState == FieldState.Plow)
        {
            totalTime = cycle * 3;
        }
        else if (newFieldState == FieldState.Grow)
        {
            totalTime = cycle / 3f;
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
        anim.Anim_StateInit(newFieldState, newSizeState, isHarvest, (pastTime / totalTime));
    }
    
    /// <summary>
    /// 작물을 업그레이드하는 함수
    /// </summary>
    public override void Upgrade(int newCycle)
    {
        if (level == MaxLevel)
        {
            Debug.LogError("Error) 해당 오브젝트는 이미 최대 레벨입니다!");
            return;
        }

        // 최고 레벨이 아니면 레벨을 1 올린다
        level++;

        // 완전히 성장하지 않았을 때
        System.DateTime newStateEndTime = System.DateTime.Now.AddSeconds(((newCycle / cycle) * (System.DateTime.Now - System.DateTime.Parse(stateEndTime)).TotalSeconds));

        stateEndTime = newStateEndTime.ToString("yyyy-MM-dd HH:mm:ss");

        // 수확 시 필요한 성장 시간 새로 대입
        cycle = newCycle;

        // 레벨별 초기화 애니메이션 실행
        anim.Anim_SetLevel(level);

        // 상태변화에 필요한 전체 시간
        float totalTime;

        if (growth == FieldState.Plow)
        {
            totalTime = cycle * 3;
        }
        else if (growth == FieldState.Grow)
        {
            totalTime = cycle / 3f;
        }
        else
        {
            totalTime = 0;
        }

        anim.Anim_StateInit(growth, size, false, (cycle - ((float)(newStateEndTime - System.DateTime.Now).TotalSeconds)) / totalTime);
    }

    #endregion



    #region Tree Basic LifeCycle


    private void Awake()
    {
        anim = this.GetComponent<Anim_Field>();
    }

    private void FixedUpdate()
    {
        if(growth == FieldState.Harvest)
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
            // 만일 밭을 다 갈았다면
            if(growth == FieldState.Plow)
            {
                fieldStateInit(FieldState.Grow, SizeState.S);
            }
            // 밭에서 작물이 성장 중이라면
            else
            {
                switch(size)
                {
                    case SizeState.S:
                        fieldStateInit(FieldState.Grow, SizeState.M);
                        break;

                    case SizeState.M:
                        fieldStateInit(FieldState.Grow, SizeState.L);
                        break;

                    case SizeState.L:
                        fieldStateInit(FieldState.Harvest, SizeState.NULL);
                        break;
                }
            }
        }
    }

    #endregion
}
