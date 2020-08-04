using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Field : BasicObject
{
    #region Field Basic Property

    /* 밭의 상태 관련 프로퍼티 */

    // 밭의 상태 열거형
    public enum FieldState { Plow, Grow, Harvest };

    // 밭의 크기 열거형
    public enum SizeState { S, M, L, NULL };

    // 현재 밭의 상태
    public FieldState growth { get; protected set; }
    
    // 현재 작물의 크기
    public SizeState size { get; protected set; }

    // 현재 나무의 크기

    // 밭을 가는데 걸리는 시간
    public float FieldPlowTime { get; protected set; }

    // 열매가 자라나는데 걸리는 시간
    public float CropGrowTime { get; protected set; }

    // 상태 변환이 종료되는 시각 (DateTime 구조체)
    [SerializeField]
    protected string stateEndTime;



    /* 밭의 애니메이션 관련 프로퍼티 */

    // 애니메이션 스크립트
    protected Anim_Field anim;


    /* 밭의 수확 관련 프로퍼티 */

    // 자동수확을 할 것인가
    [SerializeField]
    public bool isAuto { get; protected set; }

    #endregion



    #region Field Basic Method

    /// <summary>
    /// 밭을 처음 만들 때 호출하는 함수
    /// </summary>
    /// <param name="crop">심으려는 작물 프리팹</param>
    /// <param name="box">수확 시 드랍될 아이템 박스 프리팹</param>
    public virtual void Plowing(GameObject crop, GameObject box)
    {
        // 밭 애니메이션 초기화
        anim.Anim_Init(crop, box);

        // 아이템 종류는 밭
        type = "Field";

        // 레벨은 1
        level = 1;

        // 자동 수확은 off
        isAuto = false;

        // 밭 레벨에 따른 애니메이션 설정
        anim.Anim_SetLevel(level);

        // 나무의 상태 초기화
        fieldStateInit(FieldState.Plow, SizeState.NULL, FieldPlowTime);

        
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
        fieldStateInit(FieldState.Grow, SizeState.S, CropGrowTime / 3, true);

        anim.Anim_SetLevel(level);
    }


    /// <summary>
    /// 나무의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newTreeState"></param>
    /// <param name="newSizeState"></param>
    /// <param name="endTime"></param>
    public virtual void fieldStateInit(FieldState newFieldState, SizeState newSizeState, float endTime, bool isHarvest = false)
    {
        // 상태 변수의 값 변경
        growth = newFieldState;

        // 크기 변수의 값 변경
        size = newSizeState;

        // 상태 종료때의 시간
        stateEndTime = System.DateTime.Now.AddSeconds(endTime).ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log("State Ending Time : " + stateEndTime);

        // 상태 애니메이션 초기화 함수 실행
        anim.Anim_StateInit(newFieldState, newSizeState, isHarvest);
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
            // 만일 밭을 다 갈았다면
            if(growth == FieldState.Plow)
            {
                fieldStateInit(FieldState.Grow, SizeState.S, CropGrowTime / 3);
            }
            // 밭에서 작물이 성장 중이라면
            else
            {
                switch(size)
                {
                    case SizeState.S:
                        fieldStateInit(FieldState.Grow, SizeState.M, CropGrowTime / 3);
                        break;

                    case SizeState.M:
                        fieldStateInit(FieldState.Grow, SizeState.L, CropGrowTime / 3);
                        break;

                    case SizeState.L:
                        fieldStateInit(FieldState.Harvest, SizeState.NULL, 0);
                        break;
                }
            }
        }
    }

    #endregion
}
