using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test_Json : MonoBehaviour
{
    // 플레이어 정보 저장
    private PlayerInfo player = null;

    public InputField nicknameField;

    // 그리드 타일 저장
    private GridTile tile = null;

    // 그리드 맵 저장
    public GridMap map = null;

    public GameObject TestMap;

    // json 파일 저장 정보
    private string savePath;


    private string Json_Player = "";
    private string Json_Tile = "";
    private string Json_Map = "";


    // Start is called before the first frame update
    void Start()
    {
        savePath = Application.dataPath; ;

        tile = new GridTile("Test", "None", 1, false, "NotState", "yyyyMMddHHmmss");
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
        Json_Tile = JsonUtility.ToJson(tile);

        //Json_Map = JsonUtility.ToJson(map);
        Json_Map = JsonUtility.ToJson(TestMap.GetComponent<GridMap>());

        CreateJsonFile(savePath, "PlayerInfoJson", Json_Player);
        CreateJsonFile(savePath, "GridTileJson", Json_Tile);
        CreateJsonFile(savePath, "GridMapJson", Json_Map);


        Debug.Log("Player Json ) " + Json_Player);
        Debug.Log("Tile Json ) " + Json_Tile);
        Debug.Log("Map Json ) " + Json_Map);
    }



    public void OnLoadJson()
    {

        player = LoadJsonFile<PlayerInfo>(savePath, "PlayerInfoJson");
        tile = LoadJsonFile<GridTile>(savePath, "GridTileJson");
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

        map.GenerateGrid();
    }
} 
