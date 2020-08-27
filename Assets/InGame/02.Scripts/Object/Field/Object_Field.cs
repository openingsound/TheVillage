﻿using System.Collections;
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
    public float FieldPlowTime { get; protected set; }

    // 열매가 자라나는데 걸리는 시간
    public float CropGrowTime { get; protected set; }

    // 상태 변환이 종료되는 시각 (DateTime 구조체)
    [SerializeField]
    protected string stateEndTime;



    /* 밭의 애니메이션 관련 프로퍼티 */

    // 애니메이션 스크립트
    public Anim_Field anim;


    /* 밭의 수확 관련 프로퍼티 */

    // 자동수확을 할 것인가
    [SerializeField]
    public bool isAuto { get; protected set; }

    #endregion



    #region Field Basic Method


    /// <summary>
    /// 밭의 기본적인 수치들을 초기화하는 함수
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="plowTime"></param>
    /// <param name="cropTime"></param>
    public void InitField(string _name, float plowTime, float cropTime, int _idx, int _level = 1, bool auto = false)
    {
        // 오브젝트 이름 설정
        name = _name;

        // 오브젝트 종류는 밭
        type = "Field";
        
        // 밭을 가는데 걸리는 시간 설정
        FieldPlowTime = plowTime;

        // 작물이 자라는 시간 설정
        CropGrowTime = cropTime;

        // 레벨은 1
        level = _level;

        // 자동 수확은 off
        isAuto = auto;

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

        anim.Anim_SetLevel(level);

        // 다시 열매 성장 상태로 초기화
        fieldStateInit(FieldState.Grow, SizeState.S, CropGrowTime / 3, true);
    }


    /// <summary>
    /// 나무의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newTreeState"></param>
    /// <param name="newSizeState"></param>
    /// <param name="endTime"></param>
    public virtual void fieldStateInit(FieldState newFieldState, SizeState newSizeState, float endTime, bool isHarvest = false, float startTimeRate = 0)
    {
        // 상태 변수의 값 변경
        growth = newFieldState;

        GridMap.Map.tiles[mapIdx].LastStateInt = (int)newFieldState + (int)newSizeState;

        if (startTimeRate != 1)
        {
            GridMap.Map.tiles[mapIdx].LastStateTime = System.DateTime.Now.AddSeconds(
            (double)(endTime - (endTime / (1 - startTimeRate)))).ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            double addSeconds;

            if (newFieldState == FieldState.Plow)
            {
                addSeconds = FieldPlowTime;
            }
            else if (newFieldState == FieldState.Grow)
            {
                addSeconds = CropGrowTime / 3;
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
        anim.Anim_StateInit(newFieldState, newSizeState, isHarvest, startTimeRate);
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
