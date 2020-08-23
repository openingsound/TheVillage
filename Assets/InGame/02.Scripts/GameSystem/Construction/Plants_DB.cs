using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plants_DB : MonoBehaviour
{
    public static Plants_DB PlantDB { get; private set; } = null;

    /* 밭 프리팹 */
    [Header(" - 밭 작물 관련 프리팹")]

    [Space(10)]
    // 기본 밭 프리팹
    public GameObject Field;

    [Space(10)]
    // 밭 공통 작물 성장 프리팹
    public GameObject[] Bushes = new GameObject[3];

    [Space(10)]
    // 밭 공통 덩굴 작물 성장 프리팹
    public GameObject[] GroundBushes = new GameObject[6];

    [Space(10)]
    // 밭 개별 작물 성장 프리팹
    public GameObject[] OwnBushes;

    [Space(10)]
    // 밭 작물 열매 프리팹
    public GameObject[] Crops;

    [Space(10)]
    // 밭 수확물 박스 프리팹
    public GameObject[] CropBoxes;



    /* 나무 프리팹 */
    [Header(" - 나무 관련 프리팹")]

    [Space(10)]
    // 나무 공통 묘목 성장 프리팹
    public GameObject TreeBush;

    [Space(10)]
    // 나무 개별 프리팹
    public GameObject[] OwnTrees;

    [Space(10)]
    // 나무 개별 열매 프리팹
    public GameObject[] Fruits;

    [Space(10)]
    // 나무 열매 수확물 박스 프리팹
    public GameObject[] FruitBoxes;

    /* 밭, 나무 작물들의 이름, 종류들을 열거한 열거형 */

    // 밭 열거형
    public enum Crop { NULL = -1, Asparagus, Watermelon };

    // 나무열매 열거형
    public enum Fruit { NULL = -1, Apple };




    private void Awake()
    {
        PlantDB = this;
    }
}