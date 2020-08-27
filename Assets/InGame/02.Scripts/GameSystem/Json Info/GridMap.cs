using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridMap : MonoBehaviour
{
    public static GridMap Map { get; private set; } = null;


    [Header(" - 그리드 설정")]
    public int GridSize;

    [SerializeField]
    private float zHeight = -0.5f;

    [SerializeField]
    private float cellSize = 3f;

    [Header(" - 그리드 타일")]

    public GameObject gridTile;
    
    [SerializeField]
    public GridTile[] tiles;
    
    private GameObject[] gridObjects;

    private void Awake()
    {
        Map = this;

        // 그리드 맵 초기 설정
        GenerateGrid();
    }



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
    }


    /// <summary>
    /// 새로운 빈 그리드 타일을 생성하는 함수
    /// </summary>
    /// <param name="x">그리드 내 x좌표</param>
    /// <param name="y">그리드 내 y좌표</param>
    /// <param name="size">현재 그리드의 크기</param>
    private GameObject CreateNullTile(int x, int y, int size)
    {
        // 위치 설정
        // 그리드 좌표 : (((xSize - 1) / 2f - x) * cellSize, zHeight, ((ySize - 1) / 2f - y) * cellSize)
        Vector3 pos = new Vector3(((size - 1) / 2f - x) * cellSize, zHeight, ((size - 1) / 2f - y) * cellSize);
        Quaternion rot = Quaternion.Euler(90, 0, 0);

        // 그리드 타일 생성
        GameObject tile = Instantiate(gridTile, pos, rot);

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


    public Vector3? GettingGridPos(int idx)
    {
        if(idx < 0 || idx > GridSize * GridSize)
        {
            return null;
        }

        int x = idx % GridSize;
        int y = idx / GridSize;

        return new Vector3(((GridSize - 1) / 2f - x) * cellSize, zHeight, ((GridSize - 1) / 2f - y) * cellSize);
    }


    public Vector3? GettingGridPos(int x, int y)
    {
        if(x < 0 || x >= GridSize)
        {
            return null;
        }

        if(y < 0 || y >= GridSize)
        {
            return null;
        }

        return new Vector3(((GridSize - 1) / 2f - x) * cellSize, zHeight, ((GridSize - 1) / 2f - y) * cellSize);
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

    
    public void ChangeGridContent(int x, int y, GridTile newTiles)
    {
        if(x < 0 || y < 0 || x >= GridSize || y >= GridSize)
        {
            return;
        }

        tiles[y * GridSize + x] = newTiles;

        Debug.Log(" - buidings[" + x + ", " + y + "] = " + newTiles.ToString());
    }

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
}
