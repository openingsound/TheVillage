using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class MyVector3
{
    public List<Vector3> scale = new List<Vector3>();
}

public class Anim_Tree : MonoBehaviour
{
    /* 게임 오브젝트 */

    // 나무 묘목 오브젝트 - S, M, L 3개 존재
    public GameObject[] bushes = new GameObject[3];

    // 열매 오브젝트
    public GameObject[] fruits = new GameObject[6];

    // 다 성장한 나무 오브젝트
    public GameObject emptyTree;

    // 열매 아이템 오브젝트
    public GameObject fruitBox;

    // 현재 활성화된 묘목 혹은 열매 오브젝트
    private GameObject ActivatedObject = null;



    /* 컴포넌트 */

    // <Item_Tree> 컴포넌트
    private Item_Tree thisTree;



    /* 성장 애니메이션 관련 변수 */

    // 나무의 성장 크기 정도
    public Vector3[] bushGrowthSizes = new Vector3[6];

    // 열매의 성장 크기 정도
    public Vector3[] fruitGrowthSizes = new Vector3[6];



    /// <summary>
    /// 변수 초기화
    /// </summary>
    private void Awake()
    {
        /* 모든 오브젝트 비활성화 */

        // 나무 묘목 오브젝트 비활성화
        foreach(GameObject obj in bushes)
        {
            obj.SetActive(false);
        }

        // 나무 성체 오브젝트 비활성화
        emptyTree.SetActive(false);

        // 나무 열매 오브젝트 비활성화
        foreach(GameObject obj in fruits)
        {
            obj.SetActive(false);
        }

        // 열매 박스 오브젝트 비활성화
        fruitBox.SetActive(false);


        /* 컴포넌트 연결 */

        // <Item_Tree> 컴포넌트 연결
        thisTree = this.GetComponent<Item_Tree>();
    }


    private void FixedUpdate()
    {
        switch(thisTree.growth)
        {
            case Item_Tree.TreeState.Bush:
                ActivatedObject.transform.localScale += 
                    (bushGrowthSizes[(int)thisTree.growth * 2 + 1] - bushGrowthSizes[(int)thisTree.growth * 2]) / (thisTree.treeGrowTime / 3) * Time.deltaTime;
                break;

            case Item_Tree.TreeState.Fruit:
                foreach (GameObject fruit in fruits)
                {
                    fruit.transform.localScale +=
                    (fruitGrowthSizes[(int)thisTree.growth * 2 + 1] - fruitGrowthSizes[(int)thisTree.growth * 2]) / (thisTree.fruitGrowTime / 3) * Time.deltaTime;
                }

                break;
        }
    }


    /// <summary>
    /// 애니메이션의 상태를 초기화하는 변수
    /// </summary>
    /// <param name="state">나무의 상태</param>
    /// <param name="size">나무의 성장 크기 정도</param>
    public void AnimStateInit(Item_Tree.TreeState state, Item_Tree.SizeState size)
    {
        // 변경된 상태가 Bush(묘목)이라면
        if(state == Item_Tree.TreeState.Bush)
        {
            // 현재 활성화된 오브젝트가 있다면 비활성화로 변경
            if (ActivatedObject != null)
            {
                ActivatedObject.SetActive(false);
            }

            // 현재 활성화할 묘목 오브젝트를 활성화된 오브젝트 변수로 넣음
            ActivatedObject = bushes[(int)size];

            // 현재 활성화할 묘목 오브젝트의 크기 초기화
            ActivatedObject.transform.localScale = bushGrowthSizes[(int)size * 2];

            // 현재 변경된 상태에 맞는 묘목 오브젝트 활성화
            ActivatedObject.SetActive(true);
        }
        // 변경된 상태가 Fruit(과일)이라면
        else if (state == Item_Tree.TreeState.Fruit)
        {
            // 완전히 자란 나무 오브젝트가 비활성화되어 있다면
            if (emptyTree.activeSelf == false)
            {
                // 빈 나무 오브젝트 활성화
                emptyTree.SetActive(true);
            }

            // 현재 활성화된 오브젝트가 있다면 비활성화로 변경
            if (ActivatedObject != null)
            {
                ActivatedObject.SetActive(false);
            }

            foreach (GameObject fruit in fruits)
            {
                // 열매의 크기 설정
                fruit.transform.localScale = fruitGrowthSizes[(int)size * 2];

                // 만일 열매가 비활성화 되어있다면
                if (fruit.activeSelf == false)
                {
                    // 열매 오브젝트 활성화
                    fruit.SetActive(true);
                }
            }

        }
        // 변경된 상태가 Harvest(수확)이라면
        else
        {
            // 아무것도 하지 않는다
        }
    }


    /// <summary>
    /// 열매 상자를 드롭하는 함수
    /// </summary>
    public void Anim_DropBox()
    {
        // 열매 상자 비활성화 및 초기화
        fruitBox.transform.localPosition = new Vector3(0.5f, 2, -1);

        // 열매 상자 드롭
        fruitBox.SetActive(true);
    }


    /// <summary>
    /// 열매 상자를 회수하는 함수
    /// </summary>
    public void Anim_GetFruitBox()
    {
        // 열매 상자 비활성화
        fruitBox.SetActive(false);
    }


}
