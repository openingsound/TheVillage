using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test_Json : MonoBehaviour
{
    // 플레이어 정보 저장
    private PlayerInfo player = null;

    // 닉네임을 입력받을 InputField
    public InputField nicknameField;

    // 그리드 맵 저장
    public GridMap map = null;

    // 그리드 맵 컴포넌트가 있는 게임 오브젝트
    public GameObject TestMap;

    // 앱의 활성화 상태
    private bool isPaused = false;

    /// <summary>
    /// 로드된 그리드 맵에서 오브젝트 생성하는 방식
    /// 0. Keep - 오브젝트가 이전 상태를 그대로 물려받음
    /// 1. Skip - 오브젝트가 현재 시간을 반영하여 상태를 계산함
    /// </summary>
    public enum LoadWay { Keep, Skip };
    public LoadWay load;

    // json 파일 저장 정보
    private string savePath;


    private string Json_Player = "";
    private string Json_Map = "";


    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.dataPath; ;
    }


    // 앱에 비활성화 / 재활성화 되었을 때 호출되는 함수
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            isPaused = true;

            /* 앱이 일시중지 되었을 때의 실행 */

            // 플레이어의 마지막 접속 시간을 현재로 변경
            if(player != null)
            {
                player.lastConnectTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Debug.Log("Last Conneting Time : " + player.lastConnectTime);
            }

            // Json 파일을 생성 및 쓰기
            OnCreateJson();
        }
        else
        {
            if(isPaused)
            {
                isPaused = false;

                /* 앱이 다시 활성화되었을 때의 실행 */

                // Json 파일을 불러오기
                OnLoadJson();
            }
        }
    }


    // 앱이 종료될 때 호출되는 함수
    private void OnApplicationQuit()
    {
        /* 앱이 종료되었을 때의 실행 */

        // 플레이어의 마지막 접속 시간을 현재로 변경
        if (player != null)
        {
            player.lastConnectTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Json 파일을 생성 및 쓰기
        OnCreateJson();
    }



    public void OnCreatePlayerInfo()
    {
        // 플레이어 객체 생성
        player = new PlayerInfo(nicknameField.text);
    }


    public void OnCreateJson()
    {
        // 만일 플레이어 객체가 생성되지 않았다면
        if(player == null)
        {
            // 저장하지 않음
            Debug.LogError("Error - Json ) 플레이어 정보 오브젝트가 생성되지 않았습니다!");
            return;
        }

        // 만일 그리드 맵 객체가 연결되지 않았다면
        if(map == null)
        {
            // 저장하지 않음
            Debug.LogError("Error - Json ) 그리드 맵 오브젝트가 연결되지 않았습니다!");
            return;
        }
        
        /* Json 문자열 생성 */

        // 플레이어 Json 문자열 생성
        Json_Player = JsonUtility.ToJson(player);

        // 그리드 맵 Json 문자열 생성
        Json_Map = JsonUtility.ToJson(TestMap.GetComponent<GridMap>());



        /* Json 파일 생성 */

        // 플레이어 Json 파일 생성
        CreateJsonFile(savePath, "PlayerInfoJson", Json_Player);

        // 그리드 맵 Json 파일 생성
        CreateJsonFile(savePath, "GridMapJson", Json_Map);



        /* (디버깅용) Json 문자열 출력 */

        // PlayerInfo 객체의 Json 문자열 출력
        Debug.Log("Player Json ) " + Json_Player);

        // GridMap 객체의 Json 문자열 출력
        Debug.Log("Map Json ) " + Json_Map);
    }



    /// <summary>
    /// Json 파일로부터 객체에 정보를 할당하는 함수
    /// </summary>
    public void OnLoadJson()
    {
        // 플레이어 객체에 Json 파일 정보 불러오기
        player = LoadJsonFile<PlayerInfo>(savePath, "PlayerInfoJson");

        Debug.Log("Player Info ) " + player.ToString());

        // GridMap은 MonoBehaviour을 상속하는 컴포넌트이므로 별도로 불러온다
        LoadMapJsonFile(savePath, "GridMapJson");
    }



    /// <summary>
    /// 해당 경로로 오브젝트의 정보를 Json파일로 저장하는 함수
    /// </summary>
    /// <param name="path">파일 경로</param>
    /// <param name="fileName">파일 이름</param>
    /// <param name="jsonData">json으로 변환된 데이터</param>
    public void CreateJsonFile(string path, string fileName, string jsonData)
    {
        // 파일 쓰기를 위한 파일 스트림 객체 생성
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, fileName), FileMode.OpenOrCreate);

        // 쓸 데이터를 byte형식으로 변환
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // 데이터 쓰기
        fileStream.Write(data, 0, data.Length);

        // 파일 닫기
        fileStream.Close();


        // (디버깅용) Json 파일의 경로 출력
        Debug.Log("Create File Success! ) Path : " + string.Format("{0}/{1}.json", path, fileName));
    }



    /// <summary>
    /// 해당 경로의 Json 파일로부터 MonoBehaviour을 상속받지 않는 오브젝트를 생성 및 반환하는 함수
    /// </summary>
    /// <typeparam name="T">반환받을 데이터 형</typeparam>
    /// <param name="path">파일 경로</param>
    /// <param name="fileName">파일 이름</param>
    /// <returns>JSON</returns>
    public T LoadJsonFile<T>(string path, string fileName)
    {
        // 만일 해당 경로에 Json 파일이 존재하지 않는다면
        if(!File.Exists(string.Format("{0}/{1}.json", path, fileName)))
        {
            Debug.LogError("Load Json Error ) 해당 경로에 Json 파일이 존재하지 않습니다!");

            // 기본값 출력
            return default;
        }

        // 파일 읽기를 위한 파일 스트림 객체 생성
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, fileName), FileMode.Open);

        // 파일 데이터를 받을 byte배열 선언
        byte[] data = new byte[fileStream.Length];

        // 파일 데이터 읽기
        fileStream.Read(data, 0, data.Length);

        // 파일 닫기
        fileStream.Close();

        // jsonData를 문자열로 변환
        string jsonData = System.Text.Encoding.UTF8.GetString(data);


        // (디버깅용) 로드된 Json 문자열을 출력
        Debug.Log("Load File Success! ) " + jsonData);

        // jsonData를 바탕으로 객체를 구성하여 반환
        return JsonUtility.FromJson<T>(jsonData);

    }


    /// <summary>
    /// GridMap 컴포넌트 정보를 Json파일로부터 불러와 로드하는 함수
    /// </summary>
    /// <param name="path">Json 파일의 경로</param>
    /// <param name="fileName">Json 파일의 이름</param>
    public void LoadMapJsonFile(string path, string fileName)
    {
        // 만일 해당 경로에 Json 파일이 존재하지 않는다면
        if (!File.Exists(string.Format("{0}/{1}.json", path, fileName)))
        {
            Debug.LogError("Load Json Error ) 해당 경로에 Json 파일이 존재하지 않습니다!");

            return;
        }


        /* Json 파일 읽기 */

        // 파일 읽기를 위한 파일 스트림 객체 생성
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, fileName), FileMode.Open);

        // 파일 데이터를 받을 byte배열 선언
        byte[] data = new byte[fileStream.Length];

        // 파일 데이터 읽기
        fileStream.Read(data, 0, data.Length);

        // 파일 닫기
        fileStream.Close();

        // jsonData를 문자열로 변환
        string jsonData = System.Text.Encoding.UTF8.GetString(data);

        Debug.Log("Load File Success! ) " + jsonData);


        /* <GridMap> 컴포넌트에 정보 덮어쓰기 */

        // 만일 TestMap 게임 오브젝트에 GridMap 컴포넌트가 있다면
        if(TestMap.TryGetComponent<GridMap>(out map))
        {
            // jsonData를 바탕으로 객체를 구성하여 덮어 씀
            JsonUtility.FromJsonOverwrite(jsonData, map);
        }
        else
        {
            // 없다면 새로 GridMap 컴포넌트 연결함
            map = TestMap.AddComponent<GridMap>();

            // jsonData를 바탕으로 객체를 구성하여 덮어 씀
            JsonUtility.FromJsonOverwrite(jsonData, map);
        }



        /* 인게임에 필요한 오브젝트들을 로드함 */

        // 인게임에서 그리드 타일 생성
        map.LoadGrid();

        // 그리드 타일 내 오브젝트 생성
        LoadObjectTile();
    }




    /// <summary>
    /// 나무 건설하는 함수
    /// </summary>
    public void OnLoadTree(GridTile tile, int idx)
    {
        // 새 나무 생성
        GameObject newTree = Instantiate(Plants_DB.PlantDB.TreeBush, map.GettingGridPos(idx).Value, Quaternion.identity);
        Object_Tree tree = newTree.GetComponent<Object_Tree>();

        Plants_DB.Fruit fruit = (Plants_DB.Fruit)tile.TypeInt;

        switch (fruit)
        {
            case Plants_DB.Fruit.Apple:
                tree.InitTree(tile.Name, 10f, 10f, idx, tile.Level, tile.IsAuto);
                break;

            default:
                tree.InitTree(tile.Name, 10f, 10f, idx, tile.Level, tile.IsAuto);
                break;
        }

        // 새 나무 초기화
        //tree.Planting(Plants_DB.PlantDB.OwnBushes[tile.TypeInt], Plants_DB.PlantDB.Crops[tile.TypeInt], Plants_DB.PlantDB.CropBoxes[tile.TypeInt]);

        
        // 나무 애니메이션 초기화
        tree.anim.Anim_Init(Plants_DB.PlantDB.OwnTrees[tile.TypeInt], Plants_DB.PlantDB.Fruits[tile.TypeInt], Plants_DB.PlantDB.FruitBoxes[tile.TypeInt]);

        // 나무 레벨 애니메이션 초기화
        tree.anim.Anim_SetLevel(tile.Level);


        if(load == LoadWay.Keep)
        {
            Object_Tree.TreeState loadTreeState = (Object_Tree.TreeState) (tile.LastStateInt / 10);
            Object_Tree.SizeState loadTreeSize = (Object_Tree.SizeState) (tile.LastStateInt % 10);

            // 마지막 상태변화 시간에서 마지막 접속 상태 사이의 지나간 시간의 간격을 구함
            System.TimeSpan pastTime = System.DateTime.Parse(player.lastConnectTime) - System.DateTime.Parse(tile.LastStateTime);

            // 현재 상태의 총 시간과 남은 시간
            float fullTime, leftTime;

            if(loadTreeState == Object_Tree.TreeState.Bush)
            {
                // 전체 시간은 나무 성장 시간
                fullTime = tree.treeGrowTime / 3;

                // 남은시간 계산
                leftTime = fullTime - (float)pastTime.TotalSeconds;
            }
            else if(loadTreeState == Object_Tree.TreeState.Fruit)
            {
                // 전체 시간은 열매 성장 시간
                fullTime = tree.fruitGrowTime;

                // 남은시간 계산
                leftTime = fullTime - (float)pastTime.TotalSeconds;
            }
            else
            {
                leftTime = 0;
                fullTime = 1;
            }
            
            // 나무 상태 초기화
            tree.treeStateInit(loadTreeState, loadTreeSize, leftTime, false, leftTime / fullTime);
        }
        else if(load == LoadWay.Skip)
        {
            Object_Tree.TreeState loadTreeState = (Object_Tree.TreeState)tile.LastStateInt;
            Object_Tree.SizeState loadTreeSize = Object_Tree.SizeState.NULL;

            // 마지막 상태변화 시간에서 현재 시간 사이의 지나간 시간의 간격을 구함
            System.TimeSpan pastTime = System.DateTime.Now - System.DateTime.Parse(tile.LastStateTime);

            // 다음 상태까지의 남은 시간 계산
            float fullTime, leftTime;
        }
        
    }




    /// <summary>
    /// 밭을 제작하는 함수
    /// </summary>
    public void OnLoadField(GridTile tile, int idx)
    {
        // 새 밭 생성
        GameObject newField = Instantiate(Plants_DB.PlantDB.Field, map.GettingGridPos(idx).Value, Quaternion.identity);
        Object_Field field = newField.GetComponent<Object_Field>();

        Plants_DB.Crop crop = (Plants_DB.Crop)tile.TypeInt;

        switch (crop)
        {
            case Plants_DB.Crop.Watermelon:
                field.InitField(tile.Name, 10f, 10f, idx, tile.Level, tile.IsAuto);
                break;

            default:
                field.InitField(tile.Name, 10f, 10f, idx, tile.Level, tile.IsAuto);
                break;
        }



        // 나무 애니메이션 초기화
        field.anim.Anim_Init(Plants_DB.PlantDB.OwnBushes[tile.TypeInt], Plants_DB.PlantDB.Crops[tile.TypeInt], Plants_DB.PlantDB.CropBoxes[tile.TypeInt]);

        // 나무 레벨 애니메이션 초기화
        field.anim.Anim_SetLevel(tile.Level);



        if (load == LoadWay.Keep)
        {
            Object_Field.FieldState loadFieldState = (Object_Field.FieldState)(tile.LastStateInt / 10);
            Object_Field.SizeState loadFieldSize = (Object_Field.SizeState)(tile.LastStateInt % 10);

            // 마지막 상태변화 시간에서 마지막 접속 상태 사이의 지나간 시간의 간격을 구함
            System.TimeSpan pastTime = System.DateTime.Parse(player.lastConnectTime) - System.DateTime.Parse(tile.LastStateTime);

            // 현재 상태의 총 시간과 남은 시간
            float fullTime, leftTime;

            if (loadFieldState == Object_Field.FieldState.Plow)
            {
                // 전체 시간은 나무 성장 시간
                fullTime = field.FieldPlowTime;

                // 남은시간 계산
                leftTime = fullTime - (float)pastTime.TotalSeconds;
            }
            else if (loadFieldState == Object_Field.FieldState.Grow)
            {
                // 전체 시간은 열매 성장 시간
                fullTime = field.CropGrowTime / 3;

                // 남은시간 계산
                leftTime = fullTime - (float)pastTime.TotalSeconds;
            }
            else
            {
                leftTime = 0;
                fullTime = 1;
            }

            // 나무 상태 초기화
            field.fieldStateInit(loadFieldState, loadFieldSize, leftTime, false, leftTime / fullTime);
        }
        else if (load == LoadWay.Skip)
        {
            Object_Field.FieldState loadFieldState = (Object_Field.FieldState)(tile.LastStateInt / 10);
            Object_Field.SizeState loadFieldSize = (Object_Field.SizeState)(tile.LastStateInt % 10);

            // 마지막 상태변화 시간에서 현재 시간 사이의 지나간 시간의 간격을 구함
            System.TimeSpan pastTime = System.DateTime.Now - System.DateTime.Parse(tile.LastStateTime);

            // 다음 상태까지의 남은 시간 계산
            float fullTime, leftTime;
        }


    }



    public void LoadObjectTile()
    {
        if(map == null)
        {
            Debug.LogError("Load Map Error ) 그리드 맵 오브젝트가 연결되지 않았습니다!");
        }

        for(int idx = 0; idx < map.GridSize * map.GridSize; idx++)
        {

            switch(map.tiles[idx].Type)
            {
                case "Tree":
                    OnLoadTree(map.tiles[idx], idx);
                    break;

                case "Field":
                    OnLoadField(map.tiles[idx], idx);
                    break;

                default:
                    continue;
            } 
        }
    }
} 
