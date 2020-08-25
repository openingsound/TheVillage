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

    public GameObject TestMap;


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

    
    public void OnCreatePlayerInfo()
    {
        player = new PlayerInfo(nicknameField.text);
    }


    public void OnCreateJson()
    {
        if(player == null)
        {
            Debug.LogError("Error - Json ) 플레이어 정보 오브젝트가 생성되지 않았습니다!");
            return;
        }

        if(map == null)
        {
            Debug.LogError("Error - Json ) 그리드 맵 오브젝트가 연결되지 않았습니다!");
            return;
        }

        Json_Player = JsonUtility.ToJson(player);
        Json_Map = JsonUtility.ToJson(TestMap.GetComponent<GridMap>());

        CreateJsonFile(savePath, "PlayerInfoJson", Json_Player);
        CreateJsonFile(savePath, "GridMapJson", Json_Map);


        Debug.Log("Player Json ) " + Json_Player);
        Debug.Log("Map Json ) " + Json_Map);
    }



    public void OnLoadJson()
    {

        player = LoadJsonFile<PlayerInfo>(savePath, "PlayerInfoJson");
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

        Debug.Log("Load File Success! ) " + jsonData);

        // jsonData를 바탕으로 객체를 구성하여 반환
        return JsonUtility.FromJson<T>(jsonData);

    }


    public void LoadMapJsonFile(string path, string fileName)
    {
        // 만일 해당 경로에 Json 파일이 존재하지 않는다면
        if (!File.Exists(string.Format("{0}/{1}.json", path, fileName)))
        {
            Debug.LogError("Load Json Error ) 해당 경로에 Json 파일이 존재하지 않습니다!");

            return;
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

        Debug.Log("Load File Success! ) " + jsonData);


        if(TestMap.TryGetComponent<GridMap>(out map))
        {
            // jsonData를 바탕으로 객체를 구성하여 덮어 씀
            JsonUtility.FromJsonOverwrite(jsonData, map);
        }
        else
        {
            map = TestMap.AddComponent<GridMap>();

            JsonUtility.FromJsonOverwrite(jsonData, map);
        }

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
        tree.Planting(Plants_DB.PlantDB.OwnBushes[tile.TypeInt], Plants_DB.PlantDB.Crops[tile.TypeInt], Plants_DB.PlantDB.CropBoxes[tile.TypeInt]);

        /*
        // 나무 애니메이션 초기화
        tree.anim.Anim_Init(Plants_DB.PlantDB.OwnTrees[tile.TypeInt], Plants_DB.PlantDB.Fruits[tile.TypeInt], Plants_DB.PlantDB.FruitBoxes[tile.TypeInt]);

        // 나무 레벨 애니메이션 초기화
        tree.anim.Anim_SetLevel(tile.Level);


        if(load == LoadWay.Keep)
        {
            //tree.treeStateInit((Object_Tree.TreeState)tile.LastStateInt, );
            //tree.treeStateInit((Object_Tree.TreeState)tile.LastStateInt, )
        }
        */
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

        // 새 밭 초기화
        field.Plowing(Plants_DB.PlantDB.OwnBushes[tile.TypeInt], Plants_DB.PlantDB.Crops[tile.TypeInt], Plants_DB.PlantDB.CropBoxes[tile.TypeInt]);


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
