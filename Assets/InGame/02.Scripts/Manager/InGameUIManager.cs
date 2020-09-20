using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    #region InGameUI Property
    
    /* 현재 UI 상태 표시 변수 */

    /// <summary>
    /// UI 활성화 상태 비트 플래그
    /// 1 : 상점 / 2 : 건설 / 4 : 나무 / 8 : 밭
    /// </summary>
    [System.Flags]
    public enum UI_BitFlag : int { NONE = 0, TREE = 1, FIELD = 2, BUILD = 4, SHOP = 8, AUCTION = 16, OPTION = 32 };

    public static UI_BitFlag UICheck;

    //public static bool isUIon;


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

    // Option Canvas
    public GameObject OptionUI;

    #endregion

    #region InGameUI Basic Method

    // Start is called before the first frame update
    void Start()
    {
        // UI 활성화 상태 비트 플래그 
        UICheck = (int) UI_BitFlag.NONE;

        // 타겟 표시 UI 비활성화 및 크기를 그리드 셀 크기에 맞게 조절
        TargetUI.SetActive(false);
        TargetUI.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize);
    }

    private void LateUpdate()
    {
        Debug.Log("UICheck : " + UICheck.ToString());

        // 만일 상점 UI 상태인데도 불구하고 UI가 꺼져있다면
        if (UICheck == UI_BitFlag.SHOP && shopUI.activeInHierarchy == false)
        {
            // 다시 UI를 킨다
            shopUI.SetActive(true);
        }

        // UI가 켜져있지 않을 때 클릭이 일어났다면
        if(UICheck == UI_BitFlag.NONE && InputManager.InputSystem.State == InputManager.InputState.CLICK)
        {
            // 만일 범위 밖이라면 UI표시를 하지 않는다
            if (GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos) == -1)
                return;

            // 현재 클릭한 오브젝트의 태그로 UI 상태 변경
            switch (InputManager.InputSystem.selectedObject.tag)
            {
                // 빈 땅인 경우 - 건설 UI를 킨다
                case "Boundary":
                    // 단, 그리드의 내용물에 아무것도 없어야 한다
                    if(GridMap.Map.tiles[GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos)].Type == "")
                        OnClickBuildEnter();
                    break;

                // 밭인 경우 - 작물 UI를 킨다
                case "Field":
                    // 단, 그리드의 내용물이 밭이어야 한다
                    if (GridMap.Map.tiles[GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos)].Type == "Field")
                        OnClickCropEnter();
                    break;

                // 나무인 경우 - 작물 UI를 킨다
                case "Tree":
                    // 단, 그리드의 내용물이 나무이어야 한다
                    if (GridMap.Map.tiles[GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos)].Type == "Tree")
                        OnClickCropEnter();
                    break;
            }

            // 만일 TargetUI가 꺼져 있으면 끈다
            if (TargetUI.activeInHierarchy == false)
            {
                // 타겟 위치 표시 UI 활성화
                TargetUI.SetActive(true);
            }

            // 땅 위치 표시 UI를 위치에 맞게 조절
            TargetUI.transform.position = InputManager.InputSystem.TargetPos;

            // 캠의 위치 설점 (나중에 zoom 크기에 따라 변경되도록 조절)
            CamFollow.TargetPos = InputManager.InputSystem.TargetPos + new Vector3(10, 0, -3);
        }
    }

    #endregion

    #region InGameUI OnClick Method

    /// <summary>
    /// 상점 UI를 킬 때 호출되는 함수
    /// </summary>
    public void OnClickShopEnter()
    {
        // 상점과 경매 UI를 킨 것이 아니라면
        if ((UICheck != UI_BitFlag.SHOP) && (UICheck != UI_BitFlag.AUCTION))
        {
            // 다른 UI를 일단 끈다
            switch (UICheck)
            {
                case UI_BitFlag.BUILD:
                    gameManager.CloseBuildPop();

                    TargetUI.SetActive(false);
                    break;

                case UI_BitFlag.FIELD:
                    // 밭 UI 비활성화
                    cropUI.SetActive(false);

                    upgradeUI.SetActive(false);

                    TargetUI.SetActive(false);
                    break;

                case UI_BitFlag.TREE:
                    // 나무 UI 비활성화
                    cropUI.SetActive(false);

                    upgradeUI.SetActive(false);

                    TargetUI.SetActive(false);
                    break;
            }
            
            // 상태를 변경하고 상점을 킨다
            UICheck = UI_BitFlag.SHOP;

            // 상점 UI 활성화
            shopUI.SetActive(true);
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
        }
    }

    /// <summary>
    /// 경매 UI를 킬 때 호출되는 함수
    /// </summary>
    public void OnClickAuctionEnter()
    {
        // 상점과 경매 UI를 킨 것이 아니라면
        if ((UICheck != UI_BitFlag.SHOP) && (UICheck != UI_BitFlag.AUCTION))
        {
            // 다른 UI를 일단 끈다
            switch (UICheck)
            {
                case UI_BitFlag.BUILD:
                    gameManager.CloseBuildPop();

                    TargetUI.SetActive(false);
                    break;

                case UI_BitFlag.FIELD:
                    // 밭 UI 비활성화
                    cropUI.SetActive(false);

                    upgradeUI.SetActive(false);

                    TargetUI.SetActive(false);
                    break;

                case UI_BitFlag.TREE:
                    // 나무 UI 비활성화
                    cropUI.SetActive(false);

                    upgradeUI.SetActive(false);

                    TargetUI.SetActive(false);
                    break;
            }

            // 상태를 변경하고 경매를 킨다
            UICheck = UI_BitFlag.AUCTION;

            // 경매 UI 활성화
            gameManager.OpenSelldPop();
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
        }
    }

    /// <summary>
    /// 건설 UI를 키는 함수
    /// </summary>
    public void OnClickBuildEnter()
    {
        // 만일 아무 UI도 켜져 있지 않다면
        if(UICheck == UI_BitFlag.NONE)
        {
            // 상태를 건설 모드로 변경
            UICheck = UI_BitFlag.BUILD;

            // 건설 UI 활성화
            gameManager.OpenBuildPop();
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

            // 타겟 UI 비활성화
            TargetUI.SetActive(false);
        }
    }

    /// <summary>
    /// 업그레이드 UI를 키는 함수
    /// </summary>
    public void OnClickUpgradeEnter()
    {
        if(UICheck == UI_BitFlag.FIELD || UICheck == UI_BitFlag.TREE)
        {
            Debug.Log("OnClickUpgradeEnter() 실행!");

            InGameManager.inGameManager.ItemGameManager.UpgradeUpdate(
                GridMap.Map.tiles[GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos)].Name,
                GridMap.Map.tiles[GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos)].Level);
        }
    }

    /// <summary>
    /// 업그레이드 UI를 끄는 함수
    /// </summary>
    public void OnClickUpgradeExit()
    {
        if (UICheck == UI_BitFlag.FIELD || UICheck == UI_BitFlag.TREE)
        {
            Debug.Log("OnClickUpgradeExit() 실행!");

            upgradeUI.SetActive(false);
        }
    }
    
    /// <summary>
    /// 작물 UI를 키는 함수
    /// </summary>
    public void OnClickCropEnter()
    {
        if (UICheck == UI_BitFlag.NONE)
        {
            UICheck = (InputManager.InputSystem.selectedObject.CompareTag("Tree")) ? UI_BitFlag.TREE :
                ((InputManager.InputSystem.selectedObject.CompareTag("Field")) ? UI_BitFlag.FIELD : UI_BitFlag.NONE);

            if (UICheck == UI_BitFlag.NONE)
                return;

            InGameManager.inGameManager.ItemGameManager.CropUpdate(
                GridMap.Map.tiles[GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos)].Name,
                GridMap.Map.tiles[GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos)].Level);
        }
    }

    /// <summary>
    /// 작물 UI를 끄는 함수
    /// </summary>
    public void OnClickCropExit()
    {
        if (UICheck == UI_BitFlag.FIELD || UICheck == UI_BitFlag.TREE)
        {
            cropUI.SetActive(false);

            TargetUI.SetActive(false);

            UICheck = UI_BitFlag.NONE;
        }
    }

    /// <summary>
    /// Option UI를 키는 함수
    /// </summary>
    public void OnClickOptionEnter()
    {
        // 만일 옵션이 켜져있지 않다면
        if((UICheck & UI_BitFlag.OPTION) == 0)
        {
            // 옵션 창을 킨다
            OptionUI.SetActive(true);

            // 상태를 옵션창을 킨 상태로 변경
            UICheck |= UI_BitFlag.OPTION;
        }
    }

    /// <summary>
    /// Option UI를 끄는 함수
    /// </summary>
    public void OnClickOptionExit()
    {
        // 만일 옵션창이 켜져 있다면
        if((UICheck & UI_BitFlag.OPTION) != 0)
        {
            // 옵션창을 끈다
            OptionUI.SetActive(false);

            // 상태를 옵션창을 끈 상태로 변경
            UICheck &= ~(UI_BitFlag.OPTION);
        }
    }

    #endregion
}
