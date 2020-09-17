using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager inGameManager { get; private set; }

    public GameManager ItemGameManager;

    public GridMap gridSystem;

    public Test_Json Json;


    private void Awake()
    {
        inGameManager = this;
    }



    /// <summary>
    /// 나무 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="fruitName">과일 이름</param>
    /// <param name="growtime">과일의 수확 주기(단위 : 초)</param>
    /// <param name="index"> 그리드 상의 인덱스</param>
    /// <param name="isNew">(그리드 맵 상에서) 새로 만드는 오브젝트인가 (아닌 경우 : 그리드 맵 타일 정보로부터 생성하는 경우)</param>
    /// <returns>생성된 나무 게임 오브젝트</returns>
    public GameObject OnClickPlanting(string fruitName, int growtime, int index, bool isNew)
    {
        int selectedFruit = -1;

        // Fruit 열거형에서 작물을 찾는다
        foreach (var fruit in System.Enum.GetValues(typeof(Plants_DB.Fruit)))
        {
            if (System.Enum.GetName(typeof(Plants_DB.Fruit), fruit) == fruitName)
            {
                selectedFruit = (int)fruit;
                break;
            }
        }

        // 만일 열거형에 없다면 오류문 출력 후 
        if(selectedFruit == -1)
        {
            Debug.LogError("Tree Building Error) 없는 과일 작물을 심으려고 시도했습니다!");
            return null;
        }

        // 만일 새로 만드는 객체라면 그리드 타일에 정보 기록함
        if(isNew == true)
        {
            GridTile newGridTile = new GridTile("Tree", System.Enum.GetName(typeof(Plants_DB.Fruit), selectedFruit), selectedFruit, 1, 0, Object_Tree.TreeState.Bush.ToString(), -1, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

            gridSystem.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);
        }

        // 새 나무 생성
        GameObject newTree = Instantiate(Plants_DB.PlantDB.TreeBush, InGameManager.inGameManager.gridSystem.GettingGridPos(index).Value, Quaternion.identity);
        Object_Tree tree = newTree.GetComponent<Object_Tree>();

        tree.InitTree(System.Enum.GetName(typeof(Plants_DB.Fruit), selectedFruit), growtime, index);

        newTree.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize);

        tree.Planting(Plants_DB.PlantDB.OwnTrees[selectedFruit], Plants_DB.PlantDB.Fruits[selectedFruit], Plants_DB.PlantDB.FruitBoxes[selectedFruit]);

        return tree.gameObject;

        //InGameUIManager.OnClickExit();
    }

    /// <summary>
    /// 밭 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="cropName">작물 이름</param>
    /// <param name="growtime">작물의 수확 주기(단위 : 초)</param>
    /// <param name="isNew">(그리드 맵 상에서) 새로 만드는 오브젝트인가 (아닌 경우 : 그리드 맵 타일 정보로부터 생성하는 경우)</param>
    /// <returns>생성된 밭 게임 오브젝트</returns>
    public GameObject OnClickPlowing(string cropName, int growtime, int index, bool isNew)
    {
        int selectedCrop = -1;

        // Crop 열거형에서 작물을 찾는다
        foreach (var crop in System.Enum.GetValues(typeof(Plants_DB.Crop)))
        {
            if (System.Enum.GetName(typeof(Plants_DB.Crop), crop) == cropName)
            {
                selectedCrop = (int)crop;
                break;
            }
        }

        // 만일 원하는 작물이 없다면 오류문 출력 후 종료
        if (selectedCrop == -1)
        {
            Debug.LogError("Tree Building Error) 없는 과일 작물을 심으려고 시도했습니다!");
            return null;
        }

        // 만일 새로 만드는 객체라면 그리드 타일에 정보 기록함
        if (isNew == true)
        {
            GridTile newGridTile = new GridTile("Field", System.Enum.GetName(typeof(Plants_DB.Crop), selectedCrop), selectedCrop, 1, 0, Object_Field.FieldState.Plow.ToString(), -1, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

            gridSystem.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);
        }

        // 새 밭 생성
        GameObject newField = Instantiate(Plants_DB.PlantDB.Field, InGameManager.inGameManager.gridSystem.GettingGridPos(index).Value, Quaternion.identity);
        Object_Field field = newField.GetComponent<Object_Field>();

        field.InitField(System.Enum.GetName(typeof(Plants_DB.Crop), selectedCrop), growtime, index);

        newField.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f, GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f, GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f);

        field.Plowing(Plants_DB.PlantDB.OwnBushes[selectedCrop], Plants_DB.PlantDB.Crops[selectedCrop], Plants_DB.PlantDB.CropBoxes[selectedCrop]);

        return field.gameObject;

        //InGameUIManager.OnClickExit();
    }

    /// <summary>
    /// 나무에서 수확하는 함수
    /// </summary>
    public void OnClickTreeHarvesting()
    {
        Object_Tree tree = InputManager.InputSystem.selectedObject.GetComponent<Object_Tree>();

        if (tree == null)
        {
            Debug.LogError("선택된 Tree에 <Object_Tree>컴포넌트가 없습니다!");
            return;
        }

        // 만일 나무에서 자동 수확이 진행되었거나, 현재 수확이 가능하다면
        // 수확 실행
    }

    /// <summary>
    /// 밭 작물 수확하는 함수
    /// </summary>
    public void OnClickFieldHarvesting()
    {
        Object_Field field = InputManager.InputSystem.selectedObject.GetComponent<Object_Field>();

        if (field == null)
        {
            Debug.LogError("선택된 field에 <Object_Field>컴포넌트가 없습니다!");
            return;
        }

        // 만일 밭에서 자동 수확이 진행되었거나, 현재 수확이 가능하다면
        // 수확 실행
    }


    /// <summary>
    /// 해당 아이템의 작물을 심는 함수
    /// </summary>
    /// <param name="item">심고 싶은 아이템</param>
    public void Build(Item item)
    {
        Debug.Log("Build() - Item : " + item.ToString());
        

        // 만일 해당 작물이 나무에서 자란다면
        if(item.Tree == "Tree")
        {
            OnClickPlanting(item.EName, item.Cycle * 60, InGameManager.inGameManager.gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos), true);
        }
        // 만일 해당 작물이 밭에서 자란다면
        else
        {
            OnClickPlowing(item.EName, item.Cycle * 60, InGameManager.inGameManager.gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos), true);
        }
    }

    /// <summary>
    /// 오브젝트의 업그레이드를 하는 함수
    /// </summary>
    /// <param name="newCycle"></param>
    public void Upgrade(int newCycle)
    {
        BasicObject selectedObject = InputManager.InputSystem.selectedObject.GetComponent<BasicObject>();

        selectedObject.Upgrade(newCycle);
    }

    /// <summary>
    /// 오브젝트의 철거를 하는 함수
    /// </summary>
    /// <param name="newCycle"></param>
    public void Destroy()
    {
        // 그리드 맵의 내용물을 비움
        gridSystem.ChangeGridContent(gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos), new GridTile());

        // 해당 오브젝트 삭제
        Destroy(InputManager.InputSystem.selectedObject);
    }
}
