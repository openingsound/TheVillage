using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Tree : MonoBehaviour
{
    /* 게임 오브젝트 */

    // 열매 아이템 오브젝트
    public GameObject fruitBox;


    /* 컴포넌트 */

    // <Item_Tree> 컴포넌트
    private Item_Tree thisTree;

    // <Animatior> 컴포넌트
    private Animator anim;



    /// <summary>
    /// 변수 초기화
    /// </summary>
    private void Awake()
    {
        /* 모든 오브젝트 비활성화 */

        // 열매 박스 오브젝트 비활성화
        fruitBox.SetActive(false);


        /* 컴포넌트 연결 */

        // <Item_Tree> 컴포넌트 연결
        thisTree = this.GetComponent<Item_Tree>();

        // <Animator> 컴포넌트 연결
        anim = this.GetComponent<Animator>();
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
            // 애니메이션의 재생 속도 조절
            anim.speed = 3 / thisTree.treeGrowTime;

            switch (size)
            {
                case Item_Tree.SizeState.S:
                    break;

                case Item_Tree.SizeState.M:
                    anim.SetBool("Bush_M", true);
                    break;

                case Item_Tree.SizeState.L:
                    anim.SetBool("Bush_L", true);
                    break;
            }
        }
        // 변경된 상태가 Fruit(과일)이라면
        else if (state == Item_Tree.TreeState.Fruit)
        {
            // 애니메이션의 재생 속도 조절
            anim.speed = 3 / thisTree.fruitGrowTime;

            switch (size)
            {
                case Item_Tree.SizeState.S:
                    anim.SetBool("Fruit_S", true);
                    anim.SetBool("Fruit_L", false);
                    break;

                case Item_Tree.SizeState.M:
                    anim.SetBool("Fruit_M", true);
                    anim.SetBool("Fruit_S", false);
                    break;

                case Item_Tree.SizeState.L:
                    anim.SetBool("Fruit_L", true);
                    anim.SetBool("Fruit_M", false);
                    break;
            }
        }
        // 변경된 상태가 Harvest(수확)이라면
        else
        {
            // 아무것도 하지 않는다
        }

        Debug.Log("Animation Start to " + state.ToString() + " " + size.ToString());
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
