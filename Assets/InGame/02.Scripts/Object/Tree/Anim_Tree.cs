using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Tree : MonoBehaviour
{
    /* 게임 오브젝트 */

    // 열매 나무 오브젝트
    private GameObject treeObject;

    // 열매 아이템 오브젝트
    private GameObject fruitBox;

    // 레벨별 작물 스폰포인트
    private Transform[] spawnpoints = new Transform[6];


    /* 컴포넌트 */

    // <Item_Tree> 컴포넌트
    private Object_Tree thisTree;

    // 나무 성장 <Animator> 컴포넌트
    private Animator treeAnim;

    // 열매 성장 <Animator> 컴포넌트
    private Animator[] fruitAnims = new Animator[6];



    /// <summary>
    /// 애니메이션 초기화 함수
    /// </summary>
    /// <param name="tree">완전 성장한 나무 프리팹</param>
    /// <param name="crop">자라날 작물 프리팹</param>
    /// <param name="box">수확 시 드랍할 박스 프리팹</param>
    public void Anim_Init(GameObject tree, GameObject crop, GameObject box)
    {
        /* 나무 오브젝트 생성 및 초기화 */

        // 나무 오브젝트 생성
        treeObject = Instantiate(tree, this.gameObject.transform.position, Quaternion.identity);

        // 나무 오브젝트를 이 나무 게임 오브젝트의 자식으로 설정
        treeObject.transform.parent = this.gameObject.transform;

        // 나무 오브젝트 비활성화
        treeObject.SetActive(false);



        /* 과일 상자 오브젝트 생성 및 초기화 */

        // 과일 상자 아이템 생성
        fruitBox = Instantiate(box, Vector3.zero, Quaternion.identity);

        // 상자 아이템을 이 나무 오브젝트의 자식으로 설정
        fruitBox.transform.parent = this.gameObject.transform;

        // 상자 아이템 비활성화
        fruitBox.SetActive(false);

        

        /* 레벨별 작물 스폰포인트 연결 및 작물 프리팹 생성 */

        for (int i = 0; i < 6; i++)
        {
            // Pos1 ~ Pos6 그룹 오브젝트 할당
            spawnpoints[i] = this.transform.Find("FruitSpawnPoint").transform.Find("Pos" + (i + 1).ToString());

            // 각각의 작물 애니메이터를 배열에 저장
            fruitAnims[i] = Instantiate(crop, spawnpoints[i].position, Quaternion.identity).GetComponent<Animator>();

            // 각 과일 오브젝트의 부모를 스폰포인트 오브젝트로 지정
            fruitAnims[i].transform.parent = spawnpoints[i];

            // 한 줄을 담당하는 그룹 오브젝트를 비활성화
            spawnpoints[i].gameObject.SetActive(false);
        }


        /* 컴포넌트 연결 */

        // <Item_Tree> 컴포넌트 연결
        thisTree = this.GetComponent<Object_Tree>();

        // 나무 성장 <Animator> 컴포넌트
        treeAnim = this.transform.Find("Bushes").GetChild(0).GetComponent<Animator>();
    }


    /// <summary>
    /// 애니메이션의 상태를 초기화하는 변수
    /// </summary>
    /// <param name="state">나무의 상태</param>
    /// <param name="size">나무의 성장 크기 정도</param>
    public void AnimStateInit(Object_Tree.TreeState state, Object_Tree.SizeState size = Object_Tree.SizeState.NULL, bool isHarvest = false, float startTimeRate = 0)
    {
        // 변경된 상태가 Bush(묘목)이라면
        if(state == Object_Tree.TreeState.Bush)
        {
            // 애니메이션의 재생 속도 조절
            treeAnim.speed = 3 / thisTree.treeGrowTime;

            // 다음 성장 애니메이션 실행
            //treeAnim.SetTrigger("Next");

            switch(size)
            {
                case Object_Tree.SizeState.S:
                    treeAnim.Play("Bush_S", 0, startTimeRate);
                    break;

                case Object_Tree.SizeState.M:
                    treeAnim.Play("Bush_M", 0, startTimeRate);
                    break;

                case Object_Tree.SizeState.L:
                    treeAnim.Play("Bush_L", 0, startTimeRate);
                    break;
            }
            
        }
        // 변경된 상태가 Fruit(과일)이라면
        else if (state == Object_Tree.TreeState.Fruit)
        {
            // 만일 나무 오브젝트가 비활성화 되어있다면
            if(treeObject.activeInHierarchy == false)
            {
                // 묘목 오브젝트 비활성화
                treeAnim.gameObject.SetActive(false);

                // 나무 오브젝트 활성화
                treeObject.SetActive(true);
            }

            // 수확 애니메이션 실행 시
            if(isHarvest)
            {
                for(int i = 0; i < 6; i++)
                {
                    // 해당 라인이 비활성화되어 있다면
                    if (spawnpoints[i].gameObject.activeInHierarchy == false)
                    {
                        // 처리를 건너뜀
                        continue;
                    }

                    // 해당 열매 수확
                    fruitAnims[i].SetTrigger("Harvest");
                }

                // 모든 열매 수확 후 오브젝트 레벨 애니메이션 초기화 설정 진행
                Anim_SetLevel(thisTree.level);
            }

            // 각 레벨별 작물 스폰포인트를 순회
            for (int i = 0; i < 6; i++)
            {
                // 해당 라인이 비활성화되어 있다면
                if (spawnpoints[i].gameObject.activeInHierarchy == false)
                {
                    // 처리를 건너뜀
                    continue;
                }

                // 과일 성장 시작
                fruitAnims[i].SetTrigger("Grow");
                fruitAnims[i].speed = 1 / thisTree.fruitGrowTime;
                fruitAnims[i].Play("Grow", 0, startTimeRate);
            }

        }
        else
        {
            // 만일 나무 오브젝트가 비활성화 되어있다면
            if (treeObject.activeInHierarchy == false)
            {
                // 묘목 오브젝트 비활성화
                treeAnim.gameObject.SetActive(false);

                // 나무 오브젝트 활성화
                treeObject.SetActive(true);
            }

            // 각 레벨별 작물 스폰포인트를 순회
            for (int i = 0; i < 6; i++)
            {
                // 해당 라인이 비활성화되어 있다면
                if (spawnpoints[i].gameObject.activeInHierarchy == false)
                {
                    // 처리를 건너뜀
                    continue;
                }

                // 과일 성장 시작
                fruitAnims[i].Play("Grow", 0, 1f);
            }
        }

        Debug.Log("Animation Start to " + state.ToString() + " " + ((int)size));
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



    /// <summary>
    /// 작물이 성장할 라인 개수를 설정하는 함수
    /// </summary>
    /// <param name="lvl">밭의 레벨</param>
    public void Anim_SetLevel(int lvl)
    {
        // 1 ~ 6 레벨 사이의 값이 아니라면 오류문 출력
        if(lvl < 1 || lvl > 6)
        {
            Debug.LogError("Parameter 'int lvl' of range is 1 ~ 5 - Anim_SetLevel(int lvl : " + lvl.ToString() + ")");
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            // 만일 해당 스폰포인트가 레벨 제한 안에 들어온다면
            if (lvl > i)
            {
                // 해당 스폰포인트에서 과일 자람
                spawnpoints[i].gameObject.SetActive(true);
            }
            // 해당 스폰포인트가 레벨 제한 밖이라면
            else
            {
                // 해당 스폰포인트를 비활성화
                spawnpoints[i].gameObject.SetActive(false);
            }
        }
    }

}
