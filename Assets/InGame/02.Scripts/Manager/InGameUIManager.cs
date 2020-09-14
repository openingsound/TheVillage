using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    //public static InGameUIManager UImanager;

    /* 땅 클릭 시 UI표시 */

    // 타겟 표시 UI 오브젝트
    public GameObject TargetUI;
    

    /// <summary>
    /// UI 활성화 상태 비트 플래그
    /// 1 : 상점 / 2 : 건설 / 4 : 나무 / 8 : 밭
    /// </summary>
    public enum UI_BitFlag : int { NONE = 0, TREE = 1, FIELD = 2, BUILD = 4, SHOP = 8, AUCTION = 16 };

    public static UI_BitFlag UICheck;

    /*  Shop, Build UI관리 GameManager */
    public GameManager gameManager;



    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI 활성화 상태 비트 플래그 
        UICheck = (int) UI_BitFlag.NONE;

        // 타겟 표시 UI 비활성화 및 크기를 그리드 셀 크기에 맞게 조절
        TargetUI.SetActive(false);
        TargetUI.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize);

    }

    /*

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
        // 상점이 켜져 있다면
        if(UICheck == (int)UI_BitFlag.SHOP)
        {
            // 종료
            return;
        }

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
            UICheck = (int)UI_BitFlag.NONE;
        }
        // 만일 상점이 켜져 있다면 GameManager에서 끌 때까지 다른 UI를 킬 수 없음
        else if(UICheck == (int)UI_BitFlag.SHOP)
        {
            return;
        }
        else
        {
            string type = InputManager.InputSystem.selectedObject.tag;

            switch (type)
            {
                case "Boundary":
                    if(GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos) > -1)
                    {
                        // 건설 UI 실행
                        UICheck = (int)UI_BitFlag.BUILD;
                    }
                    else
                    {
                        isUI = false;
                    }

                    break;  

                case "Tree":
                    // 나무 수확 UI 실행
                    UICheck = (int)UI_BitFlag.TREE;
                    break;

                case "Field":
                    // 밭 수확 UI 실행
                    UICheck = (int)UI_BitFlag.FIELD;
                    break;
            }
        }
    }

    /// <summary>
    /// 실제 UI 구성품을 활성화하는 함수
    /// </summary>
    private void VisualizedUI()
    {        
        // UI가 활성화 되어 있다면
        if (isUI == true)
        {
            // 해당 UI 활성화
            switch ((UI_BitFlag)UICheck)
            {

                case UI_BitFlag.SHOP:
                    if (!gameManager.Shop.activeInHierarchy)
                    {
                        gameManager.Shop.SetActive(true);
                    }
                    break;

                case UI_BitFlag.BUILD:
                    if (!gameManager.BuildPop.activeInHierarchy)
                    {
                        gameManager.BuildPop.SetActive(true);
                    }

                    if(!buildUI.activeInHierarchy)
                    {
                        buildUI.SetActive(true);
                    }

                    

                    break;

                case UI_BitFlag.TREE:
                    if (!treeUI.activeInHierarchy)
                    {
                        treeUI.SetActive(true);
                    }

                    break;

                case UI_BitFlag.FIELD:
                    if (!fieldUI.activeInHierarchy)
                    {
                        fieldUI.SetActive(true);
                    }
                    break;
            }

            if (UICheck != (int)UI_BitFlag.SHOP)
            {
                if(!buildTargetUI.activeInHierarchy)
                {
                    buildTargetUI.gameObject.SetActive(true);

                    Debug.Log("Build Target UI Activate : " + (buildTargetUI.activeInHierarchy).ToString());
                }
                

                buildTargetUI.transform.position = InputManager.InputSystem.TargetPos;

                
                //Debug.Log("Build Target UI position : " + buildTargetUI.transform.position.ToString());

                CamFollow.TargetPos = InputManager.InputSystem.TargetPos + new Vector3(10, 0, -2.5f);
            }
        }

        // 해당 UI를 제외한 켜져 있는 나머지 UI를 끔
        for (int i = 1; i <= (int)UI_BitFlag.FIELD; i = i << 1)
        {
            if((i & UICheck) == 0)
            {
                switch ((UI_BitFlag) i)
                {
                    case UI_BitFlag.SHOP:
                        if(gameManager.Shop.activeInHierarchy)
                        {
                            gameManager.Shop.SetActive(false);
                        }
                        break;

                    case UI_BitFlag.BUILD:
                        if(gameManager.BuildPop.activeInHierarchy)
                        {
                            gameManager.BuildPop.SetActive(false);
                        }

                        if (buildUI.activeInHierarchy)
                        {
                            buildUI.SetActive(false);
                        }
                        break;

                    case UI_BitFlag.TREE:
                        if(treeUI.activeInHierarchy)
                        {
                            treeUI.SetActive(false);
                        }
                        break;

                    case UI_BitFlag.FIELD:
                        if (fieldUI.activeInHierarchy)
                        {
                            fieldUI.SetActive(false);
                        }
                        break;
                }
            }
        }

        if (UICheck < (int)UI_BitFlag.BUILD)
        {
            if (buildTargetUI.activeInHierarchy)
            {
                buildTargetUI.SetActive(false);
            }
        }
    }



    public void OnClickShop()
    {
        UICheck = (int)UI_BitFlag.SHOP;

        isUI = true;
    }

    public static void OnClickExit()
    {
        UICheck = (int)UI_BitFlag.NONE;

        isUI = false;
    }

    public void Btn_OnClickExit()
    {
        OnClickExit();
    }
    */
}
