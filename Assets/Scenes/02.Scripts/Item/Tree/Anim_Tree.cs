using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Tree : MonoBehaviour
{
    /* 게임 오브젝트 */

    // 나무가 자랄 때의 오브젝트
    public GameObject emptyTree;

    // 열매가 자랄 때의 오브젝트
    public GameObject fruitTree;

    // 열매 프리팹
    public GameObject fruitObject;



    /* 컴포넌트 */

    // <Item_Tree> 컴포넌트
    private Item_Tree thisTree;
    


    /* 성장 애니메이션 관련 변수 */

    // 성장 크기 변수
    private Vector3 startTreeGrowth = new Vector3(0.3f, 0.3f, 0.3f);
    private Vector3 endTreeGrowth = new Vector3(1f, 1f, 1f);



    /// <summary>
    /// 변수 초기화
    /// </summary>
    private void Awake()
    {
        // 모든 오브젝트 비활성화
        emptyTree.SetActive(false);
        fruitTree.SetActive(false);

        // <Item_Tree> 컴포넌트 연결
        thisTree = this.GetComponent<Item_Tree>();
    }


    private void Update()
    {
        switch(thisTree.state)
        {
            case Item_Tree.TreeState.TreeGrow:
                emptyTree.transform.localScale += new Vector3(
                    (endTreeGrowth.x - startTreeGrowth.x) / thisTree.treeGrowTime * Time.deltaTime,
                    (endTreeGrowth.y - startTreeGrowth.y) / thisTree.treeGrowTime * Time.deltaTime,
                    (endTreeGrowth.z - startTreeGrowth.z) / thisTree.treeGrowTime * Time.deltaTime);
                break;

            case Item_Tree.TreeState.FruitGrow:
                // 열매 성장 애니메이션
                break;

            case Item_Tree.TreeState.Harvest:
                break;

            default:
                break;
        }
    }





    /// <summary>
    /// 나무를 심을 때 오브젝트 설정
    /// </summary>
    public void Anim_PlantingTree()
    {
        // 기본 나무 오브젝트 활성화
        emptyTree.SetActive(true);

        // 나무 오브젝트 사이즈 설정
        emptyTree.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }


    public void Anim_FruitGrowthStart()
    {
        // 나무 열매 성장 애니메이션 초기화 및 시작
    }



    public void Anim_FruitGrowthEnd()
    {
        // 열매 나무 오브젝트 활성화
        fruitTree.SetActive(true);
    }



    public void Anim_HarvestStart()
    {
        // 열매 상자 비활성화 및 초기화
        fruitObject.transform.localPosition = new Vector3(0.5f, 2, -1);

        // 열매 상자 드롭
        fruitObject.SetActive(true);

        // 열매 나무는 비활성화
        fruitTree.SetActive(false);
    }


    public void Anim_GetFruitItem()
    {
        // 열매 상자 비활성화
        fruitObject.SetActive(false);
    }
}
