using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager InputSystem { get; private set; } = null;

    /* 현재 입력 상태 변수 */

    // [ 입력 상태 형식 ]
    // 1) CLICK         - 클릭
    // 2) DRAG          - 드래그 중
    // 3) ZOOM_IN       - 마우스 휠을 위로 올릴 때
    // 4) ZOOM_OUT      - 마우스 휠을 아래로 내릴 때
    // 5) NULL          - 아무런 입력도 없음
    public enum InputState { CLICK, DRAG, ZOOM_IN, ZOOM_OUT, NULL };

    // 현재 입력 상태
    [SerializeField]
    private InputState nowState = InputState.NULL;


    // 외부에서 확인하는 입력 상태
    // 이 프로퍼티의 값 변경은 외부 클래스에서는 하지 않아야 함.
    /// <summary>현재 입력 상태</summary>
    public InputState State{ get; private set; }
    



    /* 입력 조건을 위한 변수 */

    // 드래그가 완료되었다고 허용할 오차 길이 한계점
    private const float errorLimit = 0.01f;

    // 드래그로 인식할 이동 범위
    //private const float dragMinLimit = 0.1f;



    /* 입력 위치 */

    // 입력이 시작된 위치 - null값 허용
    public Vector3? StartPos { get; private set; }

    // 입력이 종료된 위치 - null값 허용
    public Vector3? EndPos { get; private set; }

    // 입력에 들어온 오브젝트
    public GameObject selectedObject { get; private set; }

    // null RaycastHit
    private RaycastHit nullHit = new RaycastHit();


    // 멀티 터치 시 이전 입력 간 거리
    private float m_lastTouchLenth = 0;


    private void Awake()
    {
        // Scene에 이미 InputSystem 싱글톤이 존재하는지 검사함
        // 이미 존재하는 경우 이 인스턴스는 소멸시킴
        if (InputSystem != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        // 이 인스턴스를 유효한 유일 오브젝트로 만든다
        InputSystem = this;

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
            StartPos = TouchScreen(Input.mousePosition).point;

            // 시작점이 null이면 인식을 끝내도 됨
            if (StartPos == null)
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
                State = InputState.CLICK;

                selectedObject = TouchScreen(Input.mousePosition).collider.gameObject;
            }
        }
        // 마우스가 눌리고 있다면
        else if (Input.GetMouseButton(0))
        {

            // 끝점은 현재 마우스가 있는 위치로 함
            EndPos = TouchScreen(Input.mousePosition).point;

            // 시작점이 null이면 인식을 끝내도 됨
            if (StartPos == null)
            {
                return;
            }

            // 끝점이 null이면 드래그 완료로 간주함
            // (시작점이 null일수는 없으므로)
            if (EndPos == null)
            {
                // 현재 상태는 null
                nowState = InputState.NULL;

                // 외부에서 보여지는 것도 드래그 완료로 함.
                State = InputState.NULL;

                return;
            }


            // 시작점과 끝점의 차이가 없다면 일반 클릭으로 인식
            if(StartPos.Value == EndPos.Value)
            {
                return;
            }
            // 아니면 드래그로 인식
            else
            {
                nowState = InputState.DRAG;
                State = InputState.DRAG;
            }

        }
        else
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");

            // 휠을 올렸을 때 처리 ↑
            if(wheelInput > 0)
            {
                nowState = InputState.ZOOM_IN;
                State = InputState.ZOOM_IN;
            }
            // 휠을 내렸을 때 처리 ↓
            else if (wheelInput < 0)
            {
                nowState = InputState.ZOOM_OUT;
                State = InputState.ZOOM_OUT;
            }
            else
            {
                nowState = InputState.NULL;
                State = InputState.NULL;
            }
            
        }



#endif

#if UNITY_ANDROID

        // 멀티 터치가 들어왔다면
        if(Input.touchCount > 2)
        {
            Vector2 pos1 = Input.GetTouch(0).position;
            Vector2 pos2 = Input.GetTouch(1).position;

            float m_touchLenth = (pos1 - pos2).sqrMagnitude;

            // 이전에 멀티 터치가 없었다면
            if(m_lastTouchLenth == 0)
            {
                m_lastTouchLenth = m_touchLenth;
                return;
            }
            // 거리가 더 멀어졌다면 줌인
            else if(m_touchLenth > m_lastTouchLenth)
            {
                nowState = InputState.ZOOM_IN;
                State = InputState.ZOOM_IN;
            }
            // 거리가 더 가까어졌다면 줌아웃
            else
            {
                nowState = InputState.ZOOM_OUT;
                State = InputState.ZOOM_OUT;
            }

            // 현재 터치간 거리를 이전 터치간 거리로 이동
            m_lastTouchLenth = m_touchLenth;
            return;

        }
        else
        {
            m_lastTouchLenth = 0;
        }

        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                // 시작점은 현재 마우스 클릭을 시작한 위치로 함
                StartPos = TouchScreen(Input.GetTouch(0).position).point;

                // 시작점이 null이면 인식을 끝내도 됨
                if (StartPos == null)
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
                    State = InputState.CLICK;

                    selectedObject = TouchScreen(Input.mousePosition).collider.gameObject;
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                // 끝점은 현재 마우스가 있는 위치로 함
                EndPos = TouchScreen(Input.GetTouch(0).position).point;

                // 시작점이 null이면 인식을 끝내도 됨
                if (StartPos == null)
                {
                    return;
                }

                // 끝점이 null이면 드래그 완료로 간주함
                // (시작점이 null일수는 없으므로)
                if (EndPos == null)
                {
                    // 현재 상태는 null
                    nowState = InputState.NULL;

                    // 외부에서 보여지는 것도 드래그 완료로 함.
                    State = InputState.NULL;

                    return;
                }


                // 시작점과 끝점의 차이가 없다면 일반 클릭으로 인식
                if (StartPos.Value == endPos.Value)
                {
                    return;
                }
                // 아니면 드래그로 인식
                else
                {
                    nowState = InputState.DRAG;
                    State = InputState.DRAG;
                }
            }
        }
        else
        {
            nowState = InputState.NULL;
            State = InputState.NULL;
        }

#endif

    }

    /// <summary>
    /// 터치한 위치를 목표 이동좌표로 설정하는 함수
    /// </summary>
    /// <param name="Pos">터치한 좌표(Input.MousePosition 혹은 Input.touches[0].position 사용)</param>
    /// <returns>터치한 곳에 있는 오브젝트의 좌표(없을 시에 Vector3.zero 반환)</returns>
    private RaycastHit TouchScreen(Vector3 Pos)
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
            return hit;
        }

        // Vector3(0, 1, 0) 은 바닥에 부딪혀 만들 수 없음 -> 즉 null 값을 대체함.
        return nullHit;
    }
}
