using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CamFollow : MonoBehaviour
{

    public static CamFollow TouchManager = null;

    //------------------[캠 이동 관련 프로퍼티]-------------------

    /* 레이캐스트 관련 프로퍼티 */


    /* 카메라 기본 관련 프로퍼티 */

    // 카메라 오브젝트
    private Camera cam;



    /* 카메라 이동 관련 프로퍼티 */

    // 카메라의 부모 게임 오브젝트 -> 실제로 움직일 오브젝트
    private Transform rigTransform;

    // 카메라가 이동할 목표 위치
    [SerializeField]
    private Vector3 TargetPos;

    // 카메라 이동에 걸리는 시간의 정도
    private float smoothTime = 0.2f;

    // 카메라의 이동 속도
    private Vector3 lastMovingVelocity;



    /* 드래그 관련 프로퍼티 */

    // 드래그로 움직이는 비율
    public float MoveRate;

    // 드래그가 완료되었다고 허용할 오차 길이 한계점
    private float errorLimit;



    /* 줌 인, 아웃 관련 프로퍼티 */

    // 줌 인, 아웃 되는 정도
    [SerializeField]
    private float zoomRate = 0.3f;

    // 카메라 크기의 최저 사이즈
    [SerializeField]
    private float zoomMin = 5f;

    // 카메라 크기의 최대 사이즈
    [SerializeField]
    private float zoomMax = 15f;

    //------------------[캠 이동 관련 기본  루틴]-------------------



    private void Awake()
    {
        // 자신의 싱글톤에 자기 자신을 할당함
        TouchManager = this;

        // 카메라 컴포넌트 할당
        cam = GetComponentInChildren<Camera>();

        // 자신의 Transform를 할당
        rigTransform = this.gameObject.GetComponent<Transform>();

        // 목표 위치를 (0, 0, 0)으로 설정
        TargetPos = Vector3.zero;

        // 드래그 완료 허용 오차 길이 한계점 설정
        errorLimit = 0.01f;

        // 드래그 인식 최저길이 설정
        //dragMinLimit = 1.0f;
    }

    private void LateUpdate()
    {
        // LateUpdate
    }

    private void FixedUpdate()
    {
        // 실제로 좌표를 계산하는 함수 실행
        OnDrag();

        // 타겟 위치가 표적 위치와 일치하지 않으면
        if (TargetPos != rigTransform.transform.position)
        {
            // 표적 위치를 움직임
            MoveCamera(TargetPos);
        }
        
        // 목표 지점까지 움직임이 완수 되었는지 보정하는 함수
        checkCamMove();
    }


    //------------------[캠 이동 관련 핵심 기능]-------------------

    
    /// <summary>
    /// 시작점과 끝점의 좌표를 계산하는 함수
    /// </summary>
    private void OnDrag()
    {
        // 마우스 클릭이 일어났을 시에
        if (InputManager.InputSystem.State == InputManager.InputState.CLICK)
        {
            // 목표 위치는 현재 타겟위치로 설정
            TargetPos = rigTransform.position;
        }
        // 마우스 클릭이 일어나는 도중에
        else if (InputManager.InputSystem.State == InputManager.InputState.DRAG)
        {
            /*
            // 드래그 길이가 인식 최저 길이를 넘지 않는다면 인식을 끝냄
            if ((InputManager.InputSystem.EndPos - InputManager.InputSystem.StartPos).Value.sqrMagnitude < dragMinLimit)
            {
                return;
            }
            */

            // 드래그로 움직이는 정도 (드래그와 반대 방향으로 움직임)
            Vector3 moveVec = (InputManager.InputSystem.EndPos.Value - InputManager.InputSystem.StartPos.Value) * MoveRate;

            // 목표 지점은 시작점에서 방향벡터를 뺀 만큼
            TargetPos = InputManager.InputSystem.StartPos.Value - moveVec;
        }
        // 만일 줌인이 일어났다면
        else if(InputManager.InputSystem.State == InputManager.InputState.ZOOM_IN)
        {
            if(cam.orthographicSize < zoomMax)
            {
                cam.orthographicSize += zoomRate;
            }
            
        }
        // 만일 줌아웃이 일어났다면
        else if (InputManager.InputSystem.State == InputManager.InputState.ZOOM_OUT)
        {
            if (cam.orthographicSize > zoomMin)
            {
                cam.orthographicSize -= zoomRate;
            }
        }
    }


    /// <summary>
    /// 카메라를 목표 위치까지 움직이는 함수
    /// </summary>
    private void MoveCamera(Vector3 pos)
    {
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, pos, ref lastMovingVelocity, smoothTime);
        transform.position = smoothPosition;
    }



    private void checkCamMove()
    {
        // 현재 카메라 위치와 목표 위치간 차이
        Vector3 posError = transform.position - TargetPos;

        // 오차 한계 범위 내까지 이동이 되었다면
        // 이동을 완료 및 드래그 이동 종료
        if(posError.magnitude < errorLimit)
        {
            // 카메라를 목표 위치로 딱 고정함
            transform.position = TargetPos;
        }
    }



    /// <summary>
    /// 카메라의 현재 목표위치를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTargetPos()
    {
        return TargetPos;
    }
}
