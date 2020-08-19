using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /* 땅 클릭 시 UI표시 */

    // 링 이미지 오브젝트
    public Image ring;

    public List<Image> ringUIList = new List<Image>();

    // 링 오브젝트 표시 여부
    private bool isRing;


    /* 터치 시스템 */

    // 현재 UI를 켜야 하는가
    private bool isUI;



    // Start is called before the first frame update
    void Start()
    {
        /* 땅 클릭 시 UI표시 */

        // 링 UI를 표시할지 확인하는 변수 초기화
        isRing = false;

        /* 터치 시스템 */

        // 현재 UI를 켜야 하는가
        isUI = false;
        
    }
    
    private void LateUpdate()
    {
        // 터치 및 클릭 인식
        CheckInput();

        // 어떠한 UI를 키고 끌지 정하는 함수
        CheckUI();

        // 실제로 UI를 배치하는 함수
        VisualizedUI();
    }


    /// <summary>
    /// 입력을 감지하여 ui의 활성화 여부 결정
    /// </summary>
    private void CheckInput()
    {
        // 만일 클릭이라면
        if (InputManager.InputSystem.State == InputManager.InputState.CLICK)
        {
            // UI ON -> OFF, UI OFF -> ON 으로 변경
            if (isUI == true)
            {
                isUI = false;
            }
            else
            {
                isUI = true;
            }

        }
        // 만일 드래그 중 이었다면
        else if (InputManager.InputSystem.State == InputManager.InputState.DRAG)
        {
            // UI ON -> OFF 로만 변경 가능
            if (isUI == true)
            {
                isUI = false;
            }
        }
    }


    /// <summary>
    /// ui를 킬지, 어떤 Ui를 킬지 정하는 함수
    /// </summary>
    private void CheckUI()
    {
        // isUI가 OFF이면 모든 ui를 끈다
        if(isUI == false)
        {
            isRing = false;
            return;
        }

        // 현재는 UI가 링 하나밖에 없으므로 isUI가 true이면 isRing을 킨다
        isRing = true;
    }

    /// <summary>
    /// 실제 UI 구성품을 활성화하는 함수
    /// </summary>
    private void VisualizedUI()
    {
        if(isRing == true)
        {
            string type = InputManager.InputSystem.selectedObject.tag;

            switch(type)
            {
                case "Boundary":
                    ring = ringUIList[0];
                    break;

                case "Tree":
                    ring = ringUIList[1];
                    break;

                case "Field":
                    ring = ringUIList[2];
                    break;
            }

            ring.gameObject.SetActive(true);

            ring.gameObject.transform.position = InputManager.InputSystem.TargetPos;

            //Debug.Log("ring Pos - " + ring.gameObject.transform.position.ToString());

        }
        else
        {
            ring.gameObject.SetActive(false);
        }
    }
}
