using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Tree : MonoBehaviour
{
    public GameObject Tree;

    public void OnClickPlanting()
    {
        // 새 나무 생성
        Item_Tree newTree = Instantiate(Tree, InputManager.InputSystem.TargetPos - new Vector3(0, 0.5f, 0), Quaternion.identity).GetComponent<Item_Tree>();

        // 새 나무 초기화
        newTree.Planting();
    }


    public void OnClickHarvesting()
    {
        Item_Tree tree = InputManager.InputSystem.selectedObject.GetComponent<Item_Tree>();

        if(tree.growth == Item_Tree.TreeState.Harvest && !tree.isAuto)
        {
            tree.FruitHarvesting();
        }
    }
}
