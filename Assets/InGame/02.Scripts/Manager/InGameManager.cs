using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    #region InGameManager Property
    public static InGameManager inGameManager { get; private set; }

    public GameManager ItemGameManager;

    public Test_Json Json;

    /// <summary>
    /// 그리드 타일 프리팹
    /// </summary>
    public GameObject gridTile;

    public List<GameObject> BackGroud = new List<GameObject>();

    private void Start()
    {
        inGameManager = this;

        // 기본 그리드맵 로드
        GridMap.Map.GenerateGrid();

        // GameManager 초기화
        ItemGameManager.Init_GameManager();

        // 맵 파일 로드
        Json.OnLoadJson();

        // 가격 갱신 코루틴 실행
        StartCoroutine(ResetPrice());
    }

    /// <summary>
    /// 갱신일에 맞춰 가격 갱신을 자동적으로 처리하는 함수
    /// </summary>
    IEnumerator ResetPrice()
    {
        // 다음 갱신 날짜 변수
        System.DateTime nextResetTime;

        // 만일 현재 AM 12:00:00 이라면 가격 갱신을 함
        if(System.DateTime.Now.Hour == 0 && System.DateTime.Now.Minute == 0 && System.DateTime.Now.Second == 0)
        {
            // 가격 갱신
            InGameManager.inGameManager.ItemGameManager.ChangePrice();

            // 다음 갱신 날짜 저장
            nextResetTime = System.DateTime.Now.AddDays(1);
        }
        // 만일 처음 들어와서 마지막 접속 기록이 없다면 다음 갱신 날짜 계산
        else if(GridMap.Map.lastConnectTime == "")
        {
            // 다음 갱신 날짜 저장
            nextResetTime = System.DateTime.Now.AddDays(1).AddHours(-1 * System.DateTime.Now.Hour).AddMinutes(-1 * System.DateTime.Now.Minute).AddSeconds(-1 * System.DateTime.Now.Second);
        }
        // 만일 마지막 접속일의 날짜와 현재의 날짜가 일치하지 않는다면 가격 갱신을 함
        // 작성 필요! (추후 JSON 변경 이후)
        else if (System.DateTime.Parse(GridMap.Map.lastConnectTime).Day != System.DateTime.Now.Day)
        {
            // 가격갱신
            InGameManager.inGameManager.ItemGameManager.ChangePrice();

            // 다음 갱신 날짜 저장
            nextResetTime = System.DateTime.Now.AddDays(1).AddHours(-1 * System.DateTime.Now.Hour).AddMinutes(-1 * System.DateTime.Now.Minute).AddSeconds(-1 * System.DateTime.Now.Second);
        }
        // 모두다 아니라면 다음 갱신 날짜 계산
        else
        {
            // 다음 갱신 날짜 저장
            nextResetTime = System.DateTime.Now.AddDays(1).AddHours(-1 * System.DateTime.Now.Hour).AddMinutes(-1 * System.DateTime.Now.Minute).AddSeconds(-1 * System.DateTime.Now.Second);
        }

        // 다음 갱신 날짜까지 대기함
        yield return new WaitForSeconds((float)(nextResetTime - System.DateTime.Now).TotalSeconds);
    }

    #endregion

    #region InGameManager BasicObject Method

    /// <summary>
    /// 나무 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="fruitName">과일 이름</param>
    /// <param name="growtime">과일의 수확 주기(단위 : 초)</param>
    /// <param name="index"> 그리드 상의 인덱스</param>
    /// <param name="isNew">(그리드 맵 상에서) 새로 만드는 오브젝트인가 (아닌 경우 : 그리드 맵 타일 정보로부터 생성하는 경우)</param>
    /// <returns>생성된 나무 게임 오브젝트</returns>
    public GameObject OnClickPlanting(string fruitName, int growtime, int index, bool isNew)
    {
        int selectedFruit = -1;

        // Fruit 열거형에서 작물을 찾는다
        foreach (var fruit in System.Enum.GetValues(typeof(Plants_DB.Fruit)))
        {
            if (System.Enum.GetName(typeof(Plants_DB.Fruit), fruit) == fruitName)
            {
                selectedFruit = (int)fruit;
                break;
            }
        }

        // 만일 열거형에 없다면 오류문 출력 후 
        if(selectedFruit == -1)
        {
            Debug.LogError("Tree Building Error) 없는 과일 작물을 심으려고 시도했습니다!");
            return null;
        }

        // 만일 새로 만드는 객체라면 그리드 타일에 정보 기록함
        if(isNew == true)
        {
            GridTile newGridTile = new GridTile("Tree", System.Enum.GetName(typeof(Plants_DB.Fruit), selectedFruit), selectedFruit, 1, 0, Object_Tree.TreeState.Bush.ToString(), -1, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

            GridMap.Map.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);
        }

        // 새 나무 생성
        GameObject newTree = Instantiate(Plants_DB.PlantDB.TreeBush, GridMap.Map.GettingGridPos(index, GridMap.Map.GridSize).Value, Quaternion.identity);
        Object_Tree tree = newTree.GetComponent<Object_Tree>();

        // 나무 오브젝트의 프로퍼티 초기화
        tree.InitTree(System.Enum.GetName(typeof(Plants_DB.Fruit), selectedFruit), growtime, index);

        // 나무 오브젝트 사이즈 조절
        newTree.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize, GridMap.Map.CellSize / GridMap.BasicCellSize);

        // 나무를 처음 심을 때 오브젝트들 연결
        tree.Planting(Plants_DB.PlantDB.OwnTrees[selectedFruit], Plants_DB.PlantDB.Fruits[selectedFruit], Plants_DB.PlantDB.FruitBoxes[selectedFruit]);

        // 나무의 상태 설정
        tree.treeStateInit(Object_Tree.TreeState.Bush, Object_Tree.SizeState.S);

        // 심은 나무 오브젝트 반환
        return tree.gameObject;

        //InGameUIManager.OnClickExit();
    }

    /// <summary>
    /// 밭 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="cropName">작물 이름</param>
    /// <param name="growtime">작물의 수확 주기(단위 : 초)</param>
    /// <param name="isNew">(그리드 맵 상에서) 새로 만드는 오브젝트인가 (아닌 경우 : 그리드 맵 타일 정보로부터 생성하는 경우)</param>
    /// <returns>생성된 밭 게임 오브젝트</returns>
    public GameObject OnClickPlowing(string cropName, int growtime, int index, bool isNew)
    {
        int selectedCrop = -1;

        // Crop 열거형에서 작물을 찾는다
        foreach (var crop in System.Enum.GetValues(typeof(Plants_DB.Crop)))
        {
            if (System.Enum.GetName(typeof(Plants_DB.Crop), crop) == cropName)
            {
                selectedCrop = (int)crop;
                break;
            }
        }

        // 만일 원하는 작물이 없다면 오류문 출력 후 종료
        if (selectedCrop == -1)
        {
            Debug.LogError("Tree Building Error) 없는 과일 작물을 심으려고 시도했습니다!");
            return null;
        }

        // 만일 새로 만드는 객체라면 그리드 타일에 정보 기록함
        if (isNew == true)
        {
            GridTile newGridTile = new GridTile("Field", System.Enum.GetName(typeof(Plants_DB.Crop), selectedCrop), selectedCrop, 1, 0, Object_Field.FieldState.Plow.ToString(), -1, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

            GridMap.Map.ChangeGridContent(InputManager.InputSystem.TargetPos, newGridTile);
        }

        // 새 밭 생성
        GameObject newField = Instantiate(Plants_DB.PlantDB.Field, GridMap.Map.GettingGridPos(index, GridMap.Map.GridSize).Value, Quaternion.identity);
        Object_Field field = newField.GetComponent<Object_Field>();

        // 밭 오브젝트의 프로퍼티 초기화
        field.InitField(System.Enum.GetName(typeof(Plants_DB.Crop), selectedCrop), growtime, index);

        // 밭 오브젝트 크기 조절
        newField.transform.localScale = new Vector3(GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f, GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f, GridMap.Map.CellSize / GridMap.BasicCellSize * 0.5f);

        // 밭 오브젝트의 내용물 생성
        field.Plowing(Plants_DB.PlantDB.OwnBushes[selectedCrop], Plants_DB.PlantDB.Crops[selectedCrop], Plants_DB.PlantDB.CropBoxes[selectedCrop]);

        // 밭 성장 시작
        field.fieldStateInit(Object_Field.FieldState.Plow, Object_Field.SizeState.NULL);

        // 생성한 밭 오브젝트 반환
        return field.gameObject;

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
            Debug.Log("선택된 Tree에 <Object_Tree>컴포넌트가 없습니다!");
            return;
        }

        // 만일 나무에서 자동 수확이 진행되었거나, 현재 수확이 가능하다면
        // 수확 실행

        // 현재 수확이 가능하다면 (자동 수확으로 쌓인 것이 있거나, 수확 가능 상태라면)
        if(tree.harvestCount > 0 || tree.growth == Object_Tree.TreeState.Harvest)
        {
            // 실제로 수확한 아이템을 인벤토리에 넣음
            InGameManager.inGameManager.ItemGameManager.harvest(tree.name, tree.harvestCount + ((tree.growth == Object_Tree.TreeState.Harvest) ? 1 : 0));

            // 수확 횟수를 0으로 초기화함
            tree.harvestCount = 0;

            // 수확한 아이템 상자를 드랍하는 애니메이션 실행
            tree.GetComponent<Anim_Tree>().Anim_DropBox();

            // 만일 수확 가능 상태였다면 상태를 변경함
            if(tree.growth == Object_Tree.TreeState.Harvest)
            {
                // 다시 과일 성장 상태로 되돌림
                tree.treeStateInit(Object_Tree.TreeState.Fruit, Object_Tree.SizeState.NULL);
            }
        }
    }

    /// <summary>
    /// 밭 작물 수확하는 함수
    /// </summary>
    public void OnClickFieldHarvesting()
    {
        Object_Field field = InputManager.InputSystem.selectedObject.GetComponent<Object_Field>();

        if (field == null)
        {
            Debug.LogError("선택된 field에 <Object_Field>컴포넌트가 없습니다!");
            return;
        }

        // 만일 밭에서 자동 수확이 진행되었거나, 현재 수확이 가능하다면
        // 수확 실행

        // 현재 수확이 가능하다면 (자동 수확으로 쌓인 것이 있거나, 수확 가능 상태라면)
        if (field.harvestCount > 0 || field.growth == Object_Field.FieldState.Harvest)
        {
            // 실제로 수확한 아이템을 인벤토리에 넣음
            InGameManager.inGameManager.ItemGameManager.harvest(field.name, field.harvestCount + ((field.growth == Object_Field.FieldState.Harvest) ? 1 : 0));

            // 수확 횟수를 0으로 초기화함
            field.harvestCount = 0;

            // 수확한 아이템 상자를 드랍하는 애니메이션 실행
            field.GetComponent<Anim_Field>().Anim_DropBox();

            // 만일 수확 가능 상태였다면 상태를 변경함
            if (field.growth == Object_Field.FieldState.Harvest)
            {
                // 다시 작물 성장 상태로 되돌림
                field.fieldStateInit(Object_Field.FieldState.Grow, Object_Field.SizeState.NULL);
            }
        }
    }

    #endregion

    #region InGameManager Function

    /// <summary>
    /// 해당 아이템의 작물을 심는 함수
    /// </summary>
    /// <param name="item">심고 싶은 아이템</param>
    public void Build(Item item)
    {
        Debug.Log("Build() - Item : " + item.ToString());
        
        // 만일 선택한 땅이 빈 땅이라면
        if(InputManager.InputSystem.selectedObject.CompareTag("Boundary"))
        {
            // 만일 선택한 아이템이 나무라면 나무를 설치한다
            if (item.Tree == "Tree")
            {
                //OnClickPlanting(item.EName, item.Cycle * 60, InGameManager.inGameManager.gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos), true);

                OnClickPlanting(item.EName, item.Cycle, GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos), true);
            }
            // 만일 선택한 아이템이 밭이라면 밭을 설치한다
            else if (item.Tree != "Tree")
            {
                //OnClickPlowing(item.EName, item.Cycle * 60, InGameManager.inGameManager.gridSystem.GettingGridIdx(InputManager.InputSystem.TargetPos), true);

                OnClickPlowing(item.EName, item.Cycle, GridMap.Map.GettingGridIdx(InputManager.InputSystem.TargetPos), true);
            }
        }
        
    }

    /// <summary>
    /// 오브젝트의 업그레이드를 하는 함수
    /// </summary>
    /// <param name="newCycle"></param>
    public void Upgrade()
    {
        if (InputManager.InputSystem.selectedObject.CompareTag("Tree") || InputManager.InputSystem.selectedObject.CompareTag("Field"))
        {
            BasicObject selectedObject = InputManager.InputSystem.selectedObject.GetComponent<BasicObject>();

            selectedObject.Upgrade();
        }
    }

    /// <summary>
    /// 오브젝트의 수확을 하는 함수
    /// </summary>
    public void Harvest()
    {
        // 만일 선택한 대상이 나무라면
        if(InputManager.InputSystem.selectedObject.CompareTag("Tree"))
        {
            // 나무의 수확을 함
            OnClickTreeHarvesting();
        }
        // 만일 선택한 대상이 밭이라면
        else if(InputManager.InputSystem.selectedObject.CompareTag("Field"))
        {
            // 밭의 수확을 함
            OnClickFieldHarvesting();
        }
    }

    /// <summary>
    /// 오브젝트의 철거를 하는 함수
    /// </summary>
    /// <param name="newCycle"></param>
    public void Remove()
    {
        // 그리드 맵의 내용물을 비움

        if(InputManager.InputSystem.selectedObject.CompareTag("Tree") || InputManager.InputSystem.selectedObject.CompareTag("Field"))
        {
            // 해당 위치의 그리드 타일 정보 초기화
            GridMap.Map.ChangeGridContent(InputManager.InputSystem.TargetPos, new GridTile());

            // 해당 오브젝트 삭제
            Destroy(InputManager.InputSystem.selectedObject);
        }
    }

    #endregion
}
