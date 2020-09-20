﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test_Json : MonoBehaviour
{
    /// <summary>
    /// 앱의 활성화 상태
    /// </summary>
    private bool isPaused = false;

    /// <summary>
    /// 로드된 그리드 맵에서 오브젝트 생성하는 방식
    /// 0. Keep - 오브젝트가 이전 상태를 그대로 물려받음
    /// 1. Skip - 오브젝트가 현재 시간을 반영하여 상태를 계산함
    /// </summary>
    public enum LoadWay { Keep, Skip };
    public LoadWay load;

    /// <summary>
    /// json 파일 저장 정보
    /// </summary>
    public string savePath;
    
    /// <summary>
    /// 불러온 Map의 JSON 문자열
    /// </summary>
    private string Json_Map = "";


    // Start is called before the first frame update
    void Awake()
    {
        savePath = Application.persistentDataPath;
    }

    /// <summary>
    /// 앱에 비활성화 / 재활성화 되었을 때 호출되는 함수
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            isPaused = true;

            /* 앱이 일시중지 되었을 때의 실행 */

            // 그리드 맵에 현재의 시간을 저장함
            GridMap.Map.lastConnectTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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

        // 그리드 맵에 현재의 시간을 저장함
        GridMap.Map.lastConnectTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Json 파일을 생성 및 쓰기
        OnCreateJson();
    }

    /// <summary>
    /// Json 파일을 생성하는 함수
    /// </summary>
    public void OnCreateJson()
    {   
        /* Json 문자열 생성 */

        // 그리드 맵 Json 문자열 생성
        Json_Map = JsonUtility.ToJson(GridMap.Map);

        /* Json 파일 생성 */

        // 그리드 맵 Json 파일 생성
        CreateJsonFile(savePath, "GridMapJson", Json_Map);

        /* (디버깅용) Json 문자열 출력 */

        // GridMap 객체의 Json 문자열 출력
        Debug.Log("Map Json ) " + Json_Map);
    }

    /// <summary>
    /// Json 파일로부터 객체에 정보를 할당하는 함수
    /// </summary>
    public void OnLoadJson()
    {
        print("파일 경로 : " + string.Format("{0}/{1}.txt", savePath, "GridMapJson"));

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
        // 텍스트 파일 생성
        File.WriteAllText(string.Format("{0}/{1}.txt", path, fileName), jsonData);

        // (디버깅용) Json 파일의 경로 출력
        Debug.Log("Create File Success! ) Path : " + string.Format("{0}/{1}.txt", path, fileName));

        Debug.Log("Create File Success! ) 생성여부 : " + new FileInfo(string.Format("{0}/{1}.txt", path, fileName)).Exists);
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
        //// 만일 해당 경로에 Json 파일이 존재하지 않는다면
        //if(File.Exists(string.Format("{0}/{1}.txt", path, fileName)) == false)
        //{
        //    Debug.LogError("Load Json Error ) 해당 경로에 Json 파일이 존재하지 않습니다!");

        //    // 기본값 출력
        //    return default;
        //}

        if (!File.Exists(string.Format("{0}/{1}.txt", path, fileName))) 
        {
            CreateJsonFile(path, fileName, "");
        }

        string jsonData = File.ReadAllText(string.Format("{0}/{1}.txt", path, fileName));

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
        if (!File.Exists(string.Format("{0}/{1}.txt", path, fileName)))
        {
            Debug.Log("Load Json Error ) 해당 경로에 Json 파일이 존재하지 않습니다!");

            return;
        }

        /* Json 파일 읽기 */

        string jsonData = File.ReadAllText(string.Format("{0}/{1}.txt", path, fileName));

        Debug.Log("Load File Success! ) " + jsonData);

        /* <GridMap> 컴포넌트에 정보 덮어쓰기 */

        // jsonData를 바탕으로 객체를 구성하여 덮어 씀
        JsonUtility.FromJsonOverwrite(jsonData, GridMap.Map);

        /* 인게임에 필요한 오브젝트들을 로드함 */

        // 인게임에서 그리드 타일 생성
        GridMap.Map.LoadGrid();

        // 그리드 타일 내 오브젝트 생성
        LoadObjectTile();
    }

    /// <summary>
    /// 그리드 맵에서 오브젝트들을 생성하는 함수
    /// </summary>
    public void LoadObjectTile()
    {
        // 그리드 맵의 크기만큼 반복문 수행
        for(int idx = 0; idx < GridMap.Map.GridSize * GridMap.Map.GridSize; idx++)
        {
            // 그리드 맵 타일의 Type으로 불러올 오브젝트를 결정함
            switch(GridMap.Map.tiles[idx].Type)
            {
                // 나무 오브젝트 로드
                case "Tree":
                    OnLoadTree(idx);
                    break;

                // 밭 오브젝트 로드
                case "Field":
                    OnLoadField(idx);
                    break;

                default:
                    continue;
            } 
        }
    }

    /// <summary>
    /// 그리드 타일 정보로 나무를 생성하는 함수
    /// </summary>
    /// <param name="index"></param>
    private void OnLoadTree(int index)
    {
        // 1. 나무를 그리드 타일 내 정보대로 인스턴스를 생성한다

        // 작물의 기본 수확 주기 (데이터 상에는 분단위로 주기가 기록되어 있으므로 초로 환산한다)
        int cycle = InGameManager.inGameManager.ItemGameManager.AllItemList.Find(x => x.EName == GridMap.Map.tiles[index].Name).Cycle * 60;

        // 작물 인스턴스 생성
        Object_Tree tree = InGameManager.inGameManager.OnClickPlanting(GridMap.Map.tiles[index].Name, cycle, index, false).GetComponent<Object_Tree>();

        // 2. 불러오는 방식에 따라 지난 시간을 계산한다

        // 지난 시간을 저장하는 변수
        float pastTime = 0;        

        // 불러오는 방식에 따라 지난 시간을 다르게 계산한다
        if (load == LoadWay.Keep)
        {
            // Keep 방식 : 마지막 접속 시간 - 마지막 상태 변화 시작 시간으로 계산함
            pastTime = (float) (System.DateTime.Parse(GridMap.Map.lastConnectTime) - System.DateTime.Parse(GridMap.Map.tiles[index].LastStateTime)).TotalSeconds;
        }
        else if(load == LoadWay.Skip)
        {
            // Skip 방식 : 현재 시간 - 마지막 상태 변화 시작 시간으로 계산함
            pastTime = (float)(System.DateTime.Now - System.DateTime.Parse(GridMap.Map.tiles[index].LastStateTime)).TotalSeconds;
        }

        // 3. 지난 시간을 가지고 상태 변화의 정도를 계산한다.

        // 현재의 오브젝트 성장 상태 (Bush = 0, Fruit = 10, Harvest = 20)
        Object_Tree.TreeState nowTreeState;

        // 현재의 오브젝트 크기 상태 (S = 1, M = 2, L = 3)
        Object_Tree.SizeState nowSizeState;

        // 만일 마지막 상태가 부쉬 상태였다면
        if (((Object_Tree.TreeState)(GridMap.Map.tiles[index].LastStateInt / 10)) == Object_Tree.TreeState.Bush)
        {
            // 만일 마지막 상태가 부쉬라면
            // 1. 아직 Bush 시간 내라면 크기 상태 구하기
            if (pastTime < tree.realCycle * 3)
            {
                // 현재 상태는 Bush
                nowTreeState = Object_Tree.TreeState.Bush;

                // 현재 크기 상태 (S = 1, M = 2, L = 3) : (지난 시간) / (수확 주기) + 1
                nowSizeState = (Object_Tree.SizeState)(((int)(pastTime / tree.realCycle)) + 1);

                // 나무 상태 초기화
                tree.treeStateInit(nowTreeState, nowSizeState, pastTime - (((int)nowSizeState) - 1) * tree.realCycle);

                // 다 설정했으므로 함수 종료
                return;
            }
            else
            {
                // 이미 나무로 다 성장하였으므로 성장 시간을 뺀다
                pastTime -= cycle * 3;
            }
        }

        // 2. 만일 나무로 다 자랐다면 열매가 몇 번 맺히는지 검사 (단, 최댓값은 ((level - 2) * 2)이다)

        // 수확한 횟수는 맵 타일에 저장됨 (수확한 횟수, 최대 저장 가능한 자동 수확 횟수, 0(초기 값))
        GridMap.Map.tiles[index].CountHarvest += Mathf.Max(((int)(pastTime / cycle)), ((InGameManager.inGameManager.ItemGameManager.UserLevel - 2) * 2), 0);

        // 3. 이제 현재 상태를 계산한다

        // 크기 상태는 null
        nowSizeState = Object_Tree.SizeState.NULL;

        // 성장 상태는 Fruit 아니면 Harvest (단, Harvest는 수확 횟수가 다 찼을 때나 딱 걸쳤을 때 변경한다)
        nowTreeState = ( (pastTime % cycle == 0) || 
            (GridMap.Map.tiles[index].CountHarvest == Mathf.Max(((InGameManager.inGameManager.ItemGameManager.UserLevel - 2) * 2), 0)) ) ? Object_Tree.TreeState.Harvest : Object_Tree.TreeState.Fruit;

        // 나무 상태 초기화
        tree.treeStateInit(nowTreeState, nowSizeState,
            (nowTreeState == Object_Tree.TreeState.Harvest) ? 0 : pastTime % cycle);


    }

    /// <summary>
    /// 그리드 타일 정보로 밭을 생성하는 함수
    /// </summary>
    /// <param name="index"></param>
    private void OnLoadField(int index)
    {
        // 1. 밭을 그리드 타일 내 정보대로 인스턴스를 생성한다

        // 작물의 수확 주기 (데이터 상에는 분단위로 주기가 기록되어 있으므로 초로 환산한다)
        int cycle = InGameManager.inGameManager.ItemGameManager.AllItemList.Find(x => x.EName == GridMap.Map.tiles[index].Name).Cycle * 60;

        // 작물 인스턴스 생성
        Object_Field field = InGameManager.inGameManager.OnClickPlowing(GridMap.Map.tiles[index].Name, cycle, index, false).GetComponent<Object_Field>();

        // 2. 불러오는 방식에 따라 지난 시간을 계산한다

        // 지난 시간을 저장하는 변수
        float pastTime = 0;

        if (load == LoadWay.Keep)
        {
            // Keep 방식 : 마지막 접속 시간 - 마지막 상태 변화 시작 시간으로 계산함
            pastTime = (float)(System.DateTime.Parse(GridMap.Map.lastConnectTime) - System.DateTime.Parse(GridMap.Map.tiles[index].LastStateTime)).TotalSeconds;
        }
        else if (load == LoadWay.Skip)
        {
            // Skip 방식 : 현재 시간 - 마지막 상태 변화 시작 시간으로 계산함
            pastTime = (float)(System.DateTime.Now - System.DateTime.Parse(GridMap.Map.tiles[index].LastStateTime)).TotalSeconds;
        }

        // 3. 지난 시간을 가지고 상태 변화의 정도를 계산한다.

        // 현재의 오브젝트 성장 상태 (Plow = 0, Crop = 10, Harvest = 20)
        Object_Field.FieldState nowFieldState;

        // 현재의 오브젝트 크기 상태 (S = 1, M = 2, L = 3)
        Object_Field.SizeState nowSizeState;

        if (((Object_Field.FieldState)(GridMap.Map.tiles[index].LastStateInt / 10)) == Object_Field.FieldState.Plow)
        {
            // 만일 마지막 상태가 부쉬라면
            // 1. 아직 Plow 시간 내라면
            if (pastTime < field.realCycle * 3)
            {
                // 현재 상태는 Plow
                nowFieldState = Object_Field.FieldState.Plow;

                // 현재 크기 상태는 Null
                nowSizeState = Object_Field.SizeState.NULL;

                // 밭 상태 초기화
                field.fieldStateInit(nowFieldState, nowSizeState, pastTime);

                // 다 설정했으므로 함수 종료
                return;
            }
            else
            {
                // 이미 밭으로 다 성장하였으므로 성장 시간을 뺀다
                pastTime -= field.realCycle * 3;
            }
        }

        // 2. 만일 밭을 다 갈았다면 열매가 몇 번 맺히는지 검사 (단, 최댓값은 ((level - 2) * 2)이다)

        // 수확한 횟수는 맵 타일에 저장됨 (수확한 횟수, 최대 저장 가능한 자동 수확 횟수, 0(초기 값))
        GridMap.Map.tiles[index].CountHarvest += Mathf.Max((int)(pastTime / field.realCycle), ((InGameManager.inGameManager.ItemGameManager.UserLevel - 2) * 2), 0);
        
        // 3. 이제 현재 상태를 계산한다

        // 크기 상태 구하기 (S = 1, M = 2, L = 3)
        nowSizeState = (Object_Field.SizeState)(((int)(pastTime % field.realCycle) / (field.realCycle / 3f) + 1));

        // 성장 상태는 Grow 아니면 Harvest (단, Harvest는 수확 횟수가 다 찼을 때나 딱 걸쳤을 때 변경한다)
        nowFieldState = ((pastTime % field.realCycle == 0) 
            || (GridMap.Map.tiles[index].CountHarvest == Mathf.Max((InGameManager.inGameManager.ItemGameManager.UserLevel - 2) * 2, 0))) ? Object_Field.FieldState.Harvest : Object_Field.FieldState.Grow;

        // 밭 상태 초기화
        field.fieldStateInit(nowFieldState, nowSizeState,
            (nowFieldState == Object_Field.FieldState.Harvest) ? 0 : pastTime % ((int) (field.realCycle / 3f)));
    }
} 
