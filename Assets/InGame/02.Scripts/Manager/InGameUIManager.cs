using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    //public static InGameUIManager UImanager;

    /* 땅 클릭 시 UI표시 */

    // 링 이미지 오브젝트
    //public Image ring;

    public GameObject buildTargetUI;

    public GameObject buildUI;

    public GameObject treeUI;

    public GameObject fieldUI;

    /// <summary>
    /// UI 활성화 상태 비트 플래그
    /// 1 : 상점 / 2 : 건설 / 4 : 나무 / 8 : 밭
    /// </summary>
    public static int UICheck;

    //[System.Flags]
    public enum UI_BitFlag : byte { NONE = 0, SHOP = 1, BUILD = 2, TREE = 4, FIELD = 8 };

    // UI의 전체적인 활성화가 필요한가
    public static bool isUI { get; private set; }

    /*  Shop, Build UI관리 GameManager */
    public GameManager gameManager;



    private void Awake()
    {
        //UImanager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI 활성화 상태 비트 플래그 
        UICheck = (int) UI_BitFlag.NONE;

        isUI = false;

        VisualizedUI();

        buildTargetUI.SetActive(false);

        buildTargetUI.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize);

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
}
