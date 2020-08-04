using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Tree : MonoBehaviour
{
    [Header(" - 나무 오브젝트")]
    public GameObject Tree;
    public enum FruitType { apple };
    public FruitType fruitType;
    public GameObject TreePrefab;
    public GameObject Fruit;
    public GameObject tree_FruitBox;

    [Space(20)]

    [Header(" - 밭 오브젝트")]
    public GameObject Field;
    public enum CropType { watermelon };
    public CropType cropType;
    public GameObject Bush;
    public GameObject Crop;
    public GameObject field_FruitBox;


    public void OnClickPlanting()
    {
        // 새 나무 생성
        GameObject newTree = Instantiate(Tree, InputManager.InputSystem.TargetPos, Quaternion.identity);
        Object_Tree tree;

        switch (fruitType)
        {
            case FruitType.apple:
                tree = newTree.gameObject.AddComponent<AppleTree>();
                break;

            default:
                tree = newTree.gameObject.AddComponent<Object_Tree>();
                break;
        }

        // 새 나무 초기화
        tree.Planting(TreePrefab, Fruit, tree_FruitBox);
    }


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

    public void OnClickPlowing()
    {
        // 새 밭 생성
        GameObject newField = Instantiate(Field, InputManager.InputSystem.TargetPos, Quaternion.identity);
        Object_Field field;

        switch(cropType)
        {
            case CropType.watermelon:
                field = newField.gameObject.AddComponent<WaterMelon>();
                break;

            default:
                field = newField.gameObject.AddComponent<Object_Field>();
                break;
        }

        // 새 밭 초기화
        field.Plowing(Bush, Crop, field_FruitBox);
    }

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
