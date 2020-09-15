using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    //public static InGameUIManager UImanager;

    /* 현재 UI 상태 표시 변수 */

    /// <summary>
    /// UI 활성화 상태 비트 플래그
    /// 1 : 상점 / 2 : 건설 / 4 : 나무 / 8 : 밭
    /// </summary>
    public enum UI_BitFlag : int { NONE = 0, TREE = 1, FIELD = 2, BUILD = 4, SHOP = 8, AUCTION = 16 };

    public static UI_BitFlag UICheck;

    public static bool isUIon;


    /* 땅 클릭 시 UI표시 */

    // 타겟 표시 UI 오브젝트
    public GameObject TargetUI;
    


    /*  Shop, Build UI 관련 */

    // GameManager
    public GameManager gameManager;

    // Crop Canvas
    public GameObject cropUI;

    // Upgrade Canvas
    public GameObject upgradeUI;

    // Shop Canvas
    public GameObject shopUI;

    // Auction Canvas
    public GameObject auctionUI;



    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // UI 활성화 상태 비트 플래그 
        UICheck = (int) UI_BitFlag.NONE;

        // UI 비활성화 상태
        isUIon = false;

        // 타겟 표시 UI 비활성화 및 크기를 그리드 셀 크기에 맞게 조절
        //TargetUI.SetActive(false);
        TargetUI.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize);

    }



    private void LateUpdate()
    {
        // 만일 상점 UI 상태인데도 불구하고 UI가 꺼져있다면
        if (UICheck == UI_BitFlag.SHOP && shopUI.activeInHierarchy == false)
        {
            // 다시 UI를 킨다
            shopUI.SetActive(true);
        }

        // 이 아래는 빈땅, 나무, 밭 클릭 시 나타나는 UI 표시
        if (UICheck >= UI_BitFlag.SHOP)
            return;


        // 만일 클릭이 일어났다면 상태 반전(UI on <-> off)
        if(InputManager.InputSystem.State == InputManager.InputState.CLICK)
        {
            // UI 상태 반전
            isUIon = !isUIon;
            
            // 만일 UI가 꺼져 있었다면
            if(isUIon == true)
            {
                // 현재 클릭한 오브젝트의 태그로 UI 상태 변경
                switch(InputManager.InputSystem.selectedObject.tag)
                {
                    // 빈 땅인 경우
                    case "Boundary":
                        UICheck = UI_BitFlag.BUILD;
                        break;

                    // 밭인 경우
                    case "Field":
                        UICheck = UI_BitFlag.FIELD;
                        break;

                    // 나무인 경우
                    case "Tree":
                        UICheck = UI_BitFlag.TREE;
                        break;
                }
            }
        }
        // 만일 드래그가 일어났다면 UI를 끄기만 함(UI on -> off)
        else if(InputManager.InputSystem.State == InputManager.InputState.DRAG)
        {
            // 만일 UI가 켜져있다면
            if(isUIon == true)
            {
                // UI 비활성화
                isUIon = false;

            }
        }

        // UI 상태 변수들을 토대로 UI를 키고 끔
        VisualizeUI();
    }




    private void VisualizeUI()
    {
        // UI를 킬 때
        if(isUIon == true)
        {
            switch(UICheck)
            {
                case UI_BitFlag.BUILD:
                    gameManager.OpenBuildPop();
                    break;

                case UI_BitFlag.FIELD:
                    // 밭 UI 활성화
                    cropUI.SetActive(true);

                    upgradeUI.SetActive(true);
                    break;

                case UI_BitFlag.TREE:
                    // 나무 UI 활성화
                    cropUI.SetActive(true);

                    upgradeUI.SetActive(true);
                    break;
            }

            if(TargetUI.activeInHierarchy == false)
            {
                // 타겟 위치 표시 UI 활성화
                TargetUI.SetActive(true);
                TargetUI.transform.position = InputManager.InputSystem.TargetPos;

                // 캠의 위치 설점 (나중에 zoom 크기에 따라 변경되도록 조절)
                CamFollow.TargetPos = InputManager.InputSystem.TargetPos + new Vector3(10, 0, -3);
            }
        }
        // UI를 끌 때
        else
        {
            switch (UICheck)
            {
                case UI_BitFlag.BUILD:
                    gameManager.CloseBuildPop();
                    break;

                case UI_BitFlag.FIELD:
                    // 밭 UI 비활성화
                    cropUI.SetActive(false);

                    upgradeUI.SetActive(false);
                    break;

                case UI_BitFlag.TREE:
                    // 나무 UI 비활성화
                    cropUI.SetActive(false);

                    upgradeUI.SetActive(false);
                    break;

            }

            // 상태를 NONE으로 한다
            UICheck = UI_BitFlag.NONE;


            if (TargetUI.activeInHierarchy == true)
            {
                // 타겟 위치 표시 UI 비활성화
                TargetUI.gameObject.SetActive(false);
            }
        }
    }



    /// <summary>
    /// 상점 UI를 킬 때 호출되는 함수
    /// </summary>
    public void OnClickShopEnter()
    {
        // 상점과 경매 UI를 킨 것이 아니라면
        if(UICheck < UI_BitFlag.SHOP)
        {
            // 상태를 변경하고 상점을 킨다
            UICheck = UI_BitFlag.SHOP;

            // 상점 UI 활성화
            shopUI.SetActive(true);

            // UI 상태 활성화
            isUIon = true;
        }
    }



    /// <summary>
    /// 상점 UI를 끌 때 호출되는 함수
    /// </summary>
    public void OnClickShopExit()
    {
        // 만일 상점 UI가 켜져 있다면
        if (UICheck == UI_BitFlag.SHOP)
        {
            // 상태를 변경하고 상점을 끈다
            UICheck = UI_BitFlag.NONE;

            // 상점 UI 비활성화
            shopUI.SetActive(false);

            // UI 상태 비활성화
            isUIon = false;
        }
    }



    /// <summary>
    /// 경매 UI를 킬 때 호출되는 함수
    /// </summary>
    public void OnClickAuctionEnter()
    {
        // 상점과 경매 UI를 킨 것이 아니라면
        if (UICheck < UI_BitFlag.SHOP)
        {
            // 상태를 변경하고 경매를 킨다
            UICheck = UI_BitFlag.AUCTION;

            // 경매 UI 활성화
            gameManager.OpenSelldPop();

            // UI 상태 활성화
            isUIon = true;
        }
    }



    /// <summary>
    /// 경매 UI를 끌 때 호출되는 함수
    /// </summary>
    public void OnClickAuctionExit()
    {
        // 만일 경매 UI가 켜져 있다면
        if (UICheck == UI_BitFlag.AUCTION)
        {
            // 상태를 변경하고 경매를 끈다
            UICheck = UI_BitFlag.NONE;

            // 경매 UI 비활성화
            gameManager.CloseSellPop();

            // UI 상태 비활성화
            isUIon = false;
        }
    }


    /// <summary>
    /// 건설 UI를 끄는 함수
    /// </summary>
    public void OnClickBuildExit()
    {
        // 만일 건설 UI가 켜져 있다면
        if(UICheck == UI_BitFlag.BUILD)
        {
            // 상태를 변경하고 건설을 끈다
            UICheck = UI_BitFlag.NONE;

            // 건설 UI 비활성화
            gameManager.CloseBuildPop();

            // UI 상태 비활성화
            isUIon = false;
        }
    }



    /// <summary>
    /// 업그레이드 UI를 키는 함수
    /// </summary>
    public void OnClickUpgradeEnter()
    {
        if(UICheck == UI_BitFlag.FIELD || UICheck == UI_BitFlag.TREE)
        {
            upgradeUI.SetActive(true);
        }
    }



    /// <summary>
    /// 업그레이드 UI를 끄는 함수
    /// </summary>
    public void OnClickUpgradeExit()
    {
        if (UICheck == UI_BitFlag.FIELD || UICheck == UI_BitFlag.TREE)
        {
            upgradeUI.SetActive(false);
        }
    }
}
