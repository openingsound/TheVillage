using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Tree : MonoBehaviour
{
    [Header(" - 그리드 시스템")]
    public GridMap gridSystem;

    [Header(" - 나무 오브젝트")]
    public Plants_DB.Fruit selectedFruit;

    [Header(" - 밭 오브젝트")]
    public Plants_DB.Crop selectedCrop;



    /// <summary>
    /// 나무 건설하는 함수
    /// </summary>
    public void OnClickPlanting()
    {
        GridTile newGridTile = new GridTile((int)selectedFruit,"Tree", selectedFruit.ToString(), 1, false, Object_Tree.TreeState.Bush.ToString(), System.DateTime.Now.ToString("yyyyMMddHHmmss"));
        
        gridSystem.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);

        // 새 나무 생성
        GameObject newTree = Instantiate(Plants_DB.PlantDB.TreeBush, InputManager.InputSystem.TargetPos, Quaternion.identity);
        Object_Tree tree = newTree.GetComponent<Object_Tree>();

        switch (selectedFruit)
        {
            case Plants_DB.Fruit.Apple:
                tree.InitTree(selectedCrop.ToString(), 10f, 10f);
                break;

            default:
                tree.InitTree(selectedCrop.ToString(), 10f, 10f);
                break;
        }

        // 새 나무 초기화
        tree.Planting(Plants_DB.PlantDB.OwnTrees[(int) selectedFruit], Plants_DB.PlantDB.Fruits[(int)selectedFruit], Plants_DB.PlantDB.FruitBoxes[(int)selectedFruit]);

        
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
    public void OnClickPlowing()
    {
        GridTile newGridTile = new GridTile((int)selectedCrop, "Field", selectedCrop.ToString(), 1, false, Object_Field.FieldState.Plow.ToString(), System.DateTime.Now.ToString("yyyyMMddHHmmss"));

        gridSystem.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);

        // 새 밭 생성
        GameObject newField = Instantiate(Plants_DB.PlantDB.Field, InputManager.InputSystem.TargetPos, Quaternion.identity);
        Object_Field field = newField.GetComponent<Object_Field>();

        switch(selectedCrop)
        {
            case Plants_DB.Crop.Watermelon:
                field.InitField(selectedCrop.ToString(), 10f, 10f);
                break;

            default:
                field.InitField(selectedCrop.ToString(), 10f, 10f);
                break;
        }

        // 새 밭 초기화
        field.Plowing(Plants_DB.PlantDB.OwnBushes[(int) selectedCrop], Plants_DB.PlantDB.Crops[(int)selectedCrop], Plants_DB.PlantDB.CropBoxes[(int)selectedCrop]);

        
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
