using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Tree : MonoBehaviour
{
    [Header(" - 그리드 시스템")]
    public GridMap gridSystem;

    //[Header(" - 나무 오브젝트")]
    //public Plants_DB.Fruit selectedFruit;

    //[Header(" - 밭 오브젝트")]
    //public Plants_DB.Crop selectedCrop;



    /// <summary>
    /// 나무 건설하는 함수
    /// </summary>
    public void OnClickPlanting(int selectedFruit, float growtime)
    {
        GridTile newGridTile = new GridTile("Tree", selectedFruit.ToString(), selectedFruit, 1, false, Object_Tree.TreeState.Bush.ToString(), -1, System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        
        gridSystem.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);

        // 새 나무 생성
        GameObject newTree = Instantiate(Plants_DB.PlantDB.TreeBush, InputManager.InputSystem.TargetPos, Quaternion.identity);
        Object_Tree tree = newTree.GetComponent<Object_Tree>();

        tree.InitTree(selectedFruit.ToString(), growtime * 2, growtime, gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos));

        newTree.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize);

        // 새 나무 초기화
        tree.Planting(Plants_DB.PlantDB.OwnTrees[selectedFruit], Plants_DB.PlantDB.Fruits[selectedFruit], Plants_DB.PlantDB.FruitBoxes[selectedFruit]);


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

        if (tree.growth == Object_Tree.TreeState.Harvest && !tree.isAuto)
        {
            Debug.Log(tree.ToString()); 
            tree.FruitHarvesting();
        }
    }



    /// <summary>
    /// 밭을 제작하는 함수
    /// </summary>
    public void OnClickPlowing(int selectedCrop, float growtime)
    {
        GridTile newGridTile = new GridTile( "Field", selectedCrop.ToString(), selectedCrop, 1, false, Object_Field.FieldState.Plow.ToString(), -1, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

        gridSystem.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);

        // 새 밭 생성
        GameObject newField = Instantiate(Plants_DB.PlantDB.Field, InputManager.InputSystem.TargetPos, Quaternion.identity);
        Object_Field field = newField.GetComponent<Object_Field>();

        field.InitField(selectedCrop.ToString(), growtime * 2, growtime, gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos));

        newField.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f, GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f, GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f);

        // 새 밭 초기화
        field.Plowing(Plants_DB.PlantDB.OwnBushes[selectedCrop], Plants_DB.PlantDB.Crops[selectedCrop], Plants_DB.PlantDB.CropBoxes[selectedCrop]);


        //InGameUIManager.OnClickExit();
    }



    /// <summary>
    /// 밭 작물 수확하는 함수
    /// </summary>
    public void OnClickFieldHarvesting()
    {
        Object_Field field = InputManager.InputSystem.selectedObject.GetComponent<Object_Field>();

        if(field == null)
        {
            Debug.LogError("선택된 field에 <Object_Field>컴포넌트가 없습니다!");
            return;
        }

        if (field.growth == Object_Field.FieldState.Harvest && !field.isAuto)
        {
            field.FruitHarvesting();
        }
    }
}
