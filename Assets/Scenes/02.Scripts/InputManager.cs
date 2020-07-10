using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager InputSystem{ get{ return instance; } }
    private static InputManager instance = null;

    /* 현재 입력 상태 변수 */

    // [ 입력 상태 형식 ]
    // 1) CLICK         - 클릭
    // 2) DRAG          - 드래그 중
    // 3) NULL          - 아무런 입력도 없음
    public enum InputState { CLICK, DRAG, NULL };

    // 현재 입력 상태
    [SerializeField]
    private InputState nowState = InputState.NULL;

    // 외부에서 확인하는 입력 상태
    // 이 프로퍼티의 값 변경은 외부 클래스에서는 하지 않아야 함.
    /// <summary>현재 입력 상태</summary>
    public InputState State{ get{ return state; } }
    [SerializeField]
    private InputState state;
    



    /* 입력 조건을 위한 변수 */

    // 드래그가 완료되었다고 허용할 오차 길이 한계점
    private const float errorLimit = 0.01f;

    // 드래그로 인식할 이동 범위
    //private const float dragMinLimit = 0.1f;



    /* 입력 위치 */

    // 입력이 시작된 위치 - null값 허용
    public Vector3? StartPos { get { return startPos; } }
    private Vector3? startPos;

    // 입력이 종료된 위치 - null값 허용
    public Vector3? EndPos { get { return endPos; } }
    private Vector3? endPos;


    private void Awake()
    {
        // Scene에 이미 InputSystem 싱글톤이 존재하는지 검사함
        // 이미 존재하는 경우 이 인스턴스는 소멸시킴
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        // 이 인스턴스를 유효한 유일 오브젝트로 만든다
        instance = this;

        // InputSystem이 지속되도록 한다
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }



    private void CheckInput()
    {

#if UNITY_EDITOR || UNITY_STANDALON

        // 마우스가 누르기 시작하면
        if (Input.GetMouseButtonDown(0))
        {

            // 시작점은 현재 마우스 클릭을 시작한 위치로 함
            startPos = TouchScreen(Input.mousePosition);

            // 시작점이 null이면 인식을 끝내도 됨
            if (startPos == null)
            {
                return;
            }

            nowState = InputState.CLICK;

        }
        // 마우스 클릭을 뗀다면
        else if (Input.GetMouseButtonUp(0))
        {
            // 만일 현재 상태가 클릭상태였다면
            if(nowState == InputState.CLICK)
            {
                // 외부에서 보여지는 상태는 뗄때가 클릭 상태임
                state = InputState.CLICK;
            }
        }
        // 마우스가 눌리고 있다면
        else if (Input.GetMouseButton(0))
        {

            // 끝점은 현재 마우스가 있는 위치로 함
            endPos = TouchScreen(Input.mousePosition);

            // 시작점이 null이면 인식을 끝내도 됨
            if (startPos == null)
            {
                return;
            }

            // 끝점이 null이면 드래그 완료로 간주함
            // (시작점이 null일수는 없으므로)
            if (endPos == null)
            {
                // 현재 상태는 null
                nowState = InputState.NULL;

                // 외부에서 보여지는 것도 드래그 완료로 함.
                state = InputState.NULL;

                return;
            }


            // 시작점과 끝점의 차이가 없다면 일반 클릭으로 인식
            if(startPos.Value == endPos.Value)
            {
                return;
            }
            // 아니면 드래그로 인식
            else
            {
                nowState = InputState.DRAG;
                state = InputState.DRAG;
            }

        }
        else
        {
            nowState = InputState.NULL;
            state = InputState.NULL;
        }

#endif

#if UNITY_ANDROID

        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // 시작점은 현재 마우스 클릭을 시작한 위치로 함
                startPos = TouchScreen(Input.GetTouch(0).position);

                // 시작점이 null이면 인식을 끝내도 됨
                if (startPos == null)
                {
                    return;
                }

                nowState = InputState.CLICK;
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                // 만일 현재 상태가 클릭상태였다면
                if (nowState == InputState.CLICK)
                {
                    // 외부에서 보여지는 상태는 뗄때가 클릭 상태임
                    state = InputState.CLICK;
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                // 끝점은 현재 마우스가 있는 위치로 함
                endPos = TouchScreen(Input.GetTouch(0).position);

                // 시작점이 null이면 인식을 끝내도 됨
                if (startPos == null)
                {
                    return;
                }

                // 끝점이 null이면 드래그 완료로 간주함
                // (시작점이 null일수는 없으므로)
                if (endPos == null)
                {
                    // 현재 상태는 null
                    nowState = InputState.NULL;

                    // 외부에서 보여지는 것도 드래그 완료로 함.
                    state = InputState.NULL;

                    return;
                }


                // 시작점과 끝점의 차이가 없다면 일반 클릭으로 인식
                if (startPos.Value == endPos.Value)
                {
                    return;
                }
                // 아니면 드래그로 인식
                else
                {
                    nowState = InputState.DRAG;
                    state = InputState.DRAG;
                }
            }
        }
        else
        {
            nowState = InputState.NULL;
            state = InputState.NULL;
        }

#endif

    }

    /// <summary>
    /// 터치한 위치를 목표 이동좌표로 설정하는 함수
    /// </summary>
    /// <param name="Pos">터치한 좌표(Input.MousePosition 혹은 Input.touches[0].position 사용)</param>
    /// <returns>터치한 곳에 있는 오브젝트의 좌표(없을 시에 Vector3.zero 반환)</returns>
    private Vector3? TouchScreen(Vector3 Pos)
    {
        // 메인 카메라에서 터치한 위치로 캐스팅되는 Ray 생성함
        Ray ray = Camera.main.ScreenPointToRay(Pos);

        // DrawRay를 이용해 시각적으로 표시
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);

        // Ray가 맞은 위치 정보를 받아올 변수
        RaycastHit hit;

        // Ray가 100거리 내에 물체에 부딪힌다면
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            // 만일 부딪힌 물체의 태그가 "CUBE_TILE" 이라면
            if (hit.collider.CompareTag("CUBE_TILE"))
            {
                // 큐브에 클릭되었음을 전달
                hit.collider.GetComponent<CheckTouch>().OnClickBlock();

                // 큐브의 위치를 리턴값으로 전달
                return hit.collider.transform.position;
            }
            // 바닥에 부딫혔다면 충돌 지점을 Vector3로 반환
            else if (hit.collider.CompareTag("Boundary"))
            {
                return hit.point;
            }
        }

        // Vector3(0, 1, 0) 은 바닥에 부딪혀 만들 수 없음 -> 즉 null 값을 대체함.
        return null;
    }
}
