using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridMap : MonoBehaviour
{
    /// <summary>
    /// 그리드 맵의 싱글턴
    /// </summary>
    public static GridMap Map { get; private set; } = null;

    /// <summary>
    /// 그리드 맵 타일의 기본 크기
    /// </summary>
    public const float BasicCellSize = 3f;

    /// <summary>
    /// 그리드 맵 타일의 한 변에서의 개수
    /// </summary>
    [Header(" - 그리드 설정")]
    public int GridSize = 1;

    /// <summary>
    /// 그리드 맵 타일이 놓일 높이
    /// </summary>
    [SerializeField]
    private float zHeight = -0.5f;

    /// <summary>
    /// 그리드 맵 타일 하나의 크기
    /// </summary>
    [SerializeField]
    private float cellSize = 3f;

    /// <summary>
    /// 그리드 맵 타일의 하나의 크기에 대한 프로퍼티
    /// </summary>
    public float CellSize { get { return cellSize; } private set { cellSize = value; } }

    /// <summary>
    /// 그리드 타일 프리팹
    /// </summary>
    [Header(" - 그리드 타일")]
    public GameObject gridTile;
    
    /// <summary>
    /// 그리드 타일들에 대한 배열
    /// </summary>
    [SerializeField]
    public GridTile[] tiles;
    
    /// <summary>
    /// 그리드 타일 오브젝트 배열
    /// </summary>
    private GameObject[] gridObjects;

    /// <summary>
    /// 플레이어의 마지막 접속 시각
    /// </summary>
    public string lastConnectTime;

    private void Awake()
    {
        Map = this;

        // 그리드 맵 초기 설정
        //GenerateGrid();
    }


    /// <summary>
    /// 그리드를 새로 만드는 함수
    /// </summary>
    public void GenerateGrid()
    {
        // 내부 칸의 내용물 배열
        tiles = new GridTile[GridSize * GridSize];
        gridObjects = new GameObject[GridSize * GridSize];
        
        // 그리드 생성
        for (int y = 0; y < GridSize; y++)
        {
            for(int x = 0; x < GridSize; x++)
            {
                // 빈 그리드 타일 생성
                gridObjects[y * GridSize + x] = CreateNullTile(x, y, GridSize);
                tiles[y * GridSize + x] = new GridTile();
            }
        }
    }

    /// <summary>
    /// JSON 파일에서 그리드 맵을 불러온 이후에 그리드 맵을 재생성하는 함수
    /// </summary>
    public void LoadGrid()
    {
        // 그리드 중앙 좌표
        int center = (GridSize - 1) / 2;

        // 기존 그리드 좌표 범위
        int range = ((int)Mathf.Sqrt((float)gridObjects.Length) - 1) / 2;

        // 내부 칸의 내용물 배열
        GameObject[] newgridObjects = new GameObject[GridSize * GridSize];

        // 그리드 생성
        for (int y = 0, idx = 0; y < GridSize; y++)
        {
            for (int x = 0; x < GridSize; x++)
            {
                // 만일 그리드의 범위가 이전에 생성했던 범위 이내라면
                if (y > center - range - 1 && y < center + range + 1 && x > center - range - 1 && x < center + range + 1)
                {
                    // 타일 오브젝트 연결
                    newgridObjects[y * GridSize + x] = gridObjects[idx];
                    newgridObjects[y * GridSize + x].name = "Grid(" + x + "," + y + ")";
                    newgridObjects[y * GridSize + x].transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                    idx++;
                }
                else
                {
                    // 만일 그리드의 범위가 이전에 생성했던 범위 밖이라면
                    // 그리드타일 오브젝트를 넣음
                    newgridObjects[y * GridSize + x] = CreateNullTile(x, y, GridSize);
                }
            }
        }

        gridObjects = newgridObjects;
    }


    /// <summary>
    /// 그리드의 크기를 재조절하는 함수
    /// </summary>
    /// <param name="newGridSize"></param>
    public void ResizeGrid(int newGridSize)
    {
        // 내부 칸의 내용물 배열
        GridTile[] newTiles = new GridTile[newGridSize * newGridSize];
        GameObject[] newgridObjects = new GameObject[newGridSize * newGridSize];

        // 그리드 중앙 좌표
        int center = (newGridSize - 1) / 2;

        // 기존 그리드 좌표 범위
        int range = (GridSize - 1) / 2;

        // 그리드 재생성
        for (int y = 0, idx = 0; y < newGridSize; y++)
        {
            for(int x = 0; x < newGridSize; x++)
            {
                // 만일 그리드의 범위가 이전에 생성했던 범위 이내라면
                if(y > center - range - 1 && y < center + range + 1 && x > center - range - 1 && x < center + range + 1)
                {
                    // 타일 오브젝트 연결
                    newTiles[y * newGridSize + x] = tiles[idx];
                    newgridObjects[y * newGridSize + x] = gridObjects[idx];
                    newgridObjects[y * newGridSize + x].name = "Grid(" + x + "," + y + ")";
                    newgridObjects[y * newGridSize + x].transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                    idx++;
                }
                else
                {
                    // 만일 그리드의 범위가 이전에 생성했던 범위 밖이라면
                    // 빈 그리드타일 오브젝트를 넣음
                    newgridObjects[y * newGridSize + x] = CreateNullTile(x, y, newGridSize);
                    newTiles[y * newGridSize + x] = new GridTile();
                }
            }
        }

        tiles = newTiles;
        gridObjects = newgridObjects;
        GridSize = newGridSize;

        // 맵 배경 오브젝트 새로 활성화
        for(int i = 1; i <= 10; i++)
        {
            if(i == 10)
            {
                InGameManager.inGameManager.BackGroud[i - 2].SetActive(true);
                continue;
            }

            if(i == InGameManager.inGameManager.ItemGameManager.UserLevel)
            {
                InGameManager.inGameManager.BackGroud[i-1].SetActive(true);
            }
            else
            {
                InGameManager.inGameManager.BackGroud[i-1].SetActive(false);
            }
        }
    }


    /// <summary>
    /// 새로운 빈 그리드 타일을 생성하는 함수
    /// </summary>
    /// <param name="x">그리드 내 x좌표</param>
    /// <param name="y">그리드 내 y좌표</param>
    /// <param name="size">새 그리드의 크기</param>
    private GameObject CreateNullTile(int x, int y, int size)
    {
        // 위치 설정
        // 그리드 좌표 : (((xSize - 1) / 2f - x) * cellSize, zHeight, ((ySize - 1) / 2f - y) * cellSize)
        Vector3 pos = GettingGridPos(y * size + x, size).Value;
        Quaternion rot = Quaternion.Euler(90, 0, 0);

        // 그리드 타일 생성
        GameObject tile = Instantiate(gridTile, pos, rot);

        tile.transform.localScale = new Vector3(cellSize, cellSize, cellSize);

        tile.name = "Grid(" + x + "," + y + ")";
        //gridObjects[y * size + x] = tile;

        // 부모를 이 스크립트를 가진 오브젝트로 함
        tile.transform.parent = this.transform;

        // 빈 그리드타일 오브젝트 생성
        //GridTile nullTile = new GridTile();

        // 그리드 값 초기화
        //tiles[y * size + x] = nullTile;

        Debug.Log("Create Tile[" + x + ", " + y + "] - null Tile");

        return tile;
    }

    #region GridMap Get_IDX/POS_Method

    /// <summary>
    /// 주어진 좌표로 grid의 어느 인덱스에 존재하는지 반환하는 함수
    /// </summary>
    /// <param name="pos">주어지는 좌표</param>
    /// <returns>grid의 인덱스</returns>
    public int GettingGridIdx(Vector3 pos)
    {
        int x = (int) ((GridSize - 1) / 2f - pos.x / cellSize);
        int y = (int) ((GridSize - 1) / 2f - pos.z / cellSize);

        // 만일 인덱스가 0보다 작다면
        if(x < 0 || y < 0)
        {
            // 그리드 경계 밖의 좌표임
            return -1;
        }

        if(x >= GridSize || y >= GridSize)
        {
            return -1;
        }

        return y * GridSize + x;
    }

    /// <summary>
    /// 인덱스 번호로 해당 그리드의 위치를 반환하는 함수
    /// </summary>
    /// <param name="idx">해당 그리드 타일의 인덱스 번호</param>
    /// <returns></returns>
    public Vector3? GettingGridPos(int idx, int size)
    {
        if(idx < 0 || idx > size * size)
        {
            return null;
        }

        int x = idx % size;
        int y = idx / size;

        return new Vector3(((size - 1) / 2f - x) * cellSize, zHeight, ((size - 1) / 2f - y) * cellSize);
    }

    /// <summary>
    /// 그리드 내용물 배열에 값을 변경하는 함수
    /// </summary>
    /// <param name="idx">배열의 인덱스 번호</param>
    /// <param name="newCh">새로 저장할 값</param>
    public void ChangeGridContent(int idx, GridTile newTiles)
    {
        if(idx < 0 || idx >= GridSize * GridSize)
        {
            return;
        }

        tiles[idx] = newTiles;

        Debug.Log(" - buidings[" + (idx % GridSize) + ", " + (idx / GridSize) + "] = " + newTiles.ToString());
    }

    /// <summary>
    /// 그리드 내용물을 변경하는 함수
    /// </summary>
    /// <param name="pos">찾고자 하는 위치</param>
    /// <param name="newTiles">변경하고자 하는 내용</param>
    public void ChangeGridContent(Vector3 pos, GridTile newTiles)
    {
        int idx = GettingGridIdx(pos);
        
        if(idx == -1)
        {
            return;
        }

        tiles[idx] = newTiles;

        Debug.Log(" - buidings[" + (idx % GridSize) + ", " + (idx / GridSize) + "] = " + newTiles.ToString());
    }

    #endregion
}
