using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Field : MonoBehaviour
{
    /* 게임 오브젝트 */

    // 열매 박스 오브젝트
    private GameObject fruitBox;

    // 다 성장한 작물 오브젝트
    private GameObject[,] fruits = new GameObject[5, 5];

    // 레벨별 작물 그룹 오브젝트
    private GameObject[] levelPos = new GameObject[5];

    // 레벨별 작물 스폰포인트
    private Transform[,] spawnpoints = new Transform[5, 5];



    /* 컴포넌트 */

    // <Item_Field> 컴포넌트
    private Object_Field thisField;

    // 밭 갈기 <Animator> 컴포넌트
    private Animator fieldAnim;

    // 작물 성장 <Animator> 컴포넌트
    private Animator[,] bushAnims = new Animator[5, 5];



    /// <summary>
    /// 애니메이션 초기화 함수
    /// </summary>
    /// <param name="bush">성장하는 작물 프리팹</param>
    /// <param name="crop">다 자란 작물 프리팹</param>
    /// <param name="box">수확 시 드랍할 박스 프리팹</param>
    public void Anim_Init(GameObject bush, GameObject crop, GameObject box)
    {
        /* 모든 오브젝트 비활성화 */

        // 과일 상자 아이템 생성
        fruitBox = Instantiate(box, Vector3.zero, Quaternion.identity);

        // 상자 아이템을 이 밭 오브젝트의 자식으로 설정
        fruitBox.transform.parent = this.gameObject.transform;

        // 상자 아이템 비활성화
        fruitBox.SetActive(false);
        


        /* 레벨별 작물 스폰포인트 연결 및 작물 프리팹 생성 */

        for (int i = 0; i < 5; i++)
        {
            // row1 ~ row5 그룹 오브젝트 할당
            levelPos[i] = this.transform.Find("CropSpawnPoint").transform.Find("row" + (i + 1).ToString()).gameObject;

            for (int j = 0; j < 5; j++)
            {
                // 각각의 스폰포인트를 배열에 저장
                spawnpoints[i, j] = levelPos[i].transform.Find("Pos" + (j + 1).ToString());

                // 각각의 작물 애니메이터를 배열에 저장
                bushAnims[i, j] = Instantiate(bush, spawnpoints[i, j].position, Quaternion.identity).GetComponent<Animator>();

                bushAnims[i, j].transform.parent = spawnpoints[i, j];

                // 각각의 다 자란 작물 오브젝트를 배열에 저장
                fruits[i, j] = Instantiate(crop, spawnpoints[i, j].position + new Vector3(0, 0.25f, 0), Quaternion.identity);

                fruits[i, j].transform.parent = spawnpoints[i, j];

                fruits[i, j].SetActive(false);

            }

            // 한 줄을 담당하는 그룹 오브젝트를 비활성화
            levelPos[i].SetActive(false);
        }


        /* 컴포넌트 연결 */

        // <Item_Field> 컴포넌트 연결
        thisField = this.GetComponent<Object_Field>();

        // 밭 갈기 <Animator> 컴포넌트
        fieldAnim = this.transform.Find("Field").GetChild(0).GetComponent<Animator>();
    }


    /// <summary>
    /// 애니메이션의 상태를 초기화하는 변수
    /// </summary>
    /// <param name="state">나무의 상태</param>
    /// <param name="size">나무의 성장 크기 정도</param>
    public void Anim_StateInit(Object_Field.FieldState state, Object_Field.SizeState size, bool isHarvest = false)
    {
        // 밭 가는 애니메이션 실행
        if(state == Object_Field.FieldState.Plow)
        {
            fieldAnim.SetBool("Plow", true);
            fieldAnim.speed = 1 / thisField.FieldPlowTime;
        }
        else if(state == Object_Field.FieldState.Grow)
        {
            // 각 레벨별 작물 스폰포인트를 순회
            for (int i = 0; i < 5; i++)
            {
                // 해당 라인이 비활성화되어 있다면
                if (levelPos[i].activeInHierarchy == false)
                {
                    // 처리를 건너뜀
                    continue;
                }

                // 다음 성장 상태로 변경
                for (int j = 0; j < 5; j++)
                {
                    fruits[i, j].SetActive(false);

                    bushAnims[i, j].SetTrigger("Next");
                    bushAnims[i, j].speed = 3 / thisField.CropGrowTime;
                }
            }
         
        }
        else if(state == Object_Field.FieldState.Harvest)
        {
            // 각 레벨별 작물 스폰포인트를 순회
            for (int i = 0; i < 5; i++)
            {
                // 해당 라인이 비활성화되어 있다면
                if (levelPos[i].activeInHierarchy == false)
                {
                    // 처리를 건너뜀
                    continue;
                }

                // 다음 성장 상태로 변경
                for (int j = 0; j < 5; j++)
                {
                    bushAnims[i, j].SetTrigger("Next");

                    fruits[i, j].SetActive(true);
                }
            }
        }

        Debug.Log("Animation Start to " + state.ToString() + " " + size.ToString());
    }



    /// <summary>
    /// 작물이 성장할 라인 개수를 설정하는 함수
    /// </summary>
    /// <param name="lvl">밭의 레벨</param>
    public void Anim_SetLevel(int lvl)
    {
        int levelMask;

        switch(lvl)
        {
            case 1:
                levelMask = 1 << 2;
                break;

            case 2:
                levelMask = (1 << 1) + (1 << 3);
                break;

            case 3:
                levelMask = 1 + (1 << 2) + (1 << 4);
                break;

            case 4:
                levelMask = 1 + (1 << 1) + (1 << 3) + (1 << 4);
                break;

            case 5:
                levelMask = 1 + (1 << 1) + (1 << 2) + (1 << 3) + (1 << 4);
                break;

            default:
                Debug.LogError("Parameter 'int lvl' of range is 1 ~ 5 - Anim_SetLevel(int lvl : " +  lvl.ToString() + ")");
                return;
        }

        for(int i = 0; i < 5; i++)
        {
            int lineCheck = 1 << i;

            // 만일 해당 라인의 bitFlag가 0이라면
            if((lineCheck & levelMask) == 0)
            {
                levelPos[i].SetActive(false);
            }
            else
            {
                levelPos[i].SetActive(true);
            }
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
