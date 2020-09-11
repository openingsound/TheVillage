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
    public static Vector3 TargetPos;

    // 카메라 이동의 시작 위치
    private Vector3 TargetStartPos;

    // 화면 - 인게임 간 화면 비율의 정도
    private const float ScreenToWorldConvert = 0.025f;

    // 카메라 이동에 걸리는 시간의 정도
    private float smoothTime = 0.2f;

    // 카메라의 이동 속도
    private Vector3 lastMovingVelocity;



    /* 드래그 관련 프로퍼티 */

    // 드래그로 움직이는 비율
    private float moveSpeedGradient;

    // 드래그 최소 속도
    public float moveSpeedMin = 1f;

    // 드래그 최대 속도
    public float moveSpeedMax = 5f;

    // 드래그가 완료되었다고 허용할 오차 길이 한계점
    private float errorLimit;



    // 카메라 크기에 대한 맵의 경계 좌표의 비율
    private float endOfPointGradient;

    // 맵의 경계 좌표
    private float endOfPoint;

    // 맵의 최대 경계
    public float endOfPointMax = 90f;

    // 맵의 최소 경계
    public float endOfPointMin = 40f;



    /* 줌 인, 아웃 관련 프로퍼티 */

    // 줌 인, 아웃 되는 정도
    [SerializeField]
    private float zoomRate = 1f;

    // 카메라 크기의 최저 사이즈
    [SerializeField]
    private float zoomMin = 8f;

    // 카메라 크기의 최대 사이즈
    [SerializeField]
    private float zoomMax = 40f;

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

        // 드래그 시 카메라 움직임의 기울기
        moveSpeedGradient = (moveSpeedMax - moveSpeedMin) / (zoomMax - zoomMin);

        // 카메라 크기에 대한 드래그의 한계점의 기울기
        endOfPointGradient = (endOfPointMax - endOfPointMin) / (zoomMax - zoomMin) * -1;

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
        //OnDrag();

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
    public void OnDrag()
    {
        // 마우스 클릭이 일어났을 시에
        if (Input.GetMouseButtonDown(0))
        {
            // 목표 위치는 현재 타겟위치로 설정
            TargetStartPos = rigTransform.position;
        }
        // 마우스 클릭이 일어나는 도중에
        else if (InputManager.InputSystem.State == InputManager.InputState.DRAG)
        {

            if (InputManager.InputSystem.EndPos == InputManager.InputSystem.StartPos)
            {
                return;
            }

            /*
            // 드래그 길이가 인식 최저 길이를 넘지 않는다면 인식을 끝냄
            if ((InputManager.InputSystem.EndPos - InputManager.InputSystem.StartPos).Value.sqrMagnitude < dragMinLimit)
            {
                return;
            }
            */

            // 드래그로 움직이는 정도 (드래그와 반대 방향으로 움직임)
            Vector3 dragVec = (InputManager.InputSystem.EndPos - InputManager.InputSystem.StartPos) * ScreenToWorldConvert;

            Vector3 moveVec = new Vector3(1, 0, -1).normalized * dragVec.x + new Vector3(1, 0, 1).normalized * dragVec.y; 

            // 목표 지점은 시작점에서 방향벡터를 뺀 만큼
            TargetPos = TargetStartPos - moveVec * (moveSpeedGradient * (cam.orthographicSize - zoomMin) + moveSpeedMin);

            // 목표 지점은 "x : -40 ~ 40, z : -40 ~ 40"(최대 축소 시) 으로 제한 됨
            // 확대 크기에 따라 값이 달라짐 - 최대 -90 ~ 90 (최대 확대 시)
            endOfPoint = endOfPointGradient * (cam.orthographicSize - zoomMin) + endOfPointMax;

            if(TargetPos.x < -1 * endOfPoint)
            {
                TargetPos.x = -1 * endOfPoint;
            }
            else if (TargetPos.x > endOfPoint)
            {
                TargetPos.x = endOfPoint;
            }

            if (TargetPos.z < -1 * endOfPoint)
            {
                TargetPos.z = -1 * endOfPoint;
            }
            else if (TargetPos.z > endOfPoint)
            {
                TargetPos.z = endOfPoint;
            }
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
        Vector3 smoothPosition = Vector3.SmoothDamp(rigTransform.position, pos, ref lastMovingVelocity, smoothTime);
        rigTransform.position = smoothPosition;
    }



    private void checkCamMove()
    {
        // 현재 카메라 위치와 목표 위치간 차이
        Vector3 posError = rigTransform.position - TargetPos;

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
