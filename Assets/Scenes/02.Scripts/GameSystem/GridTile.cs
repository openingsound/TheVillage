using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridTile : MonoBehaviour
{
    [Header(" - 그리드 좌표 설정")]
    public int xSize;
    public int ySize;
    public float zHeight;

    [Header(" - 그리드 셀 크기")]
    public float cellSize;

    [Header(" - 그리드 타일")]
    public GameObject gridTile;
    
    private char[] buildings;


    private void Awake()
    {
        // 그리드 맵 초기 설정
        GenerateGrid();
    }



    private void GenerateGrid()
    {
        // 내부 칸의 내용물 배열
        buildings = new char[xSize * ySize];

        // 그리드 좌표 : (((xSize - 1) / 2f - x) * cellSize, zHeight, ((ySize - 1) / 2f - y) * cellSize)

        // 그리드 생성
        for (int y = 0; y < ySize; y++)
        {
            for(int x = 0; x <xSize; x++)
            {
                // 위치 설정
                Vector3 pos = new Vector3(((xSize - 1) / 2f - x) * cellSize, zHeight, ((ySize - 1) / 2f - y) * cellSize);
                Quaternion rot = Quaternion.Euler(90, 0, 0);

                // 그리드 타일 생성
                GameObject tile = Instantiate(gridTile, pos, rot);

                // 부모를 이 스크립트를 가진 오브젝트로 함
                tile.transform.parent = this.transform;

                // 그리드 값 초기화
                buildings[y * xSize + x] = '0';
            }
        }
    }


    /// <summary>
    /// 주어진 좌표로 grid의 어느 인덱스에 존재하는지 반환하는 함수
    /// </summary>
    /// <param name="pos">주어지는 좌표</param>
    /// <returns>grid의 인덱스</returns>
    public int GettingGridPos(Vector3 pos)
    {
        int x = (int) ((xSize - 1) / 2f - pos.x / cellSize);
        int y = (int) ((ySize - 1) / 2f - pos.z / cellSize);

        // 만일 인덱스가 0보다 작다면
        if(x < 0 || y < 0)
        {
            // 그리드 경계 밖의 좌표임
            return -1;
        }

        if(x >= xSize || y >= ySize)
        {
            return -1;
        }

        return y * xSize + x;
    }



    public void ChangeGridContent(int idx, char newCh)
    {
        if(idx < 0 || idx >= ySize * xSize)
        {
            return;
        }

        buildings[idx] = newCh;

        Debug.Log(" - buidings[" + (idx % xSize) + ", " + (idx / xSize) + "] = " + newCh);
    }

    
    public void ChangeGridContent(int x, int y, char newCh)
    {
        if(x < 0 || y < 0 || x >= xSize || y >= ySize)
        {
            return;
        }

        buildings[y * xSize + x] = newCh;

        Debug.Log(" - buidings[" + x + ", " + y + "] = " + newCh);
    }

    public void ChangeGridContent(Vector3 pos, char newCh)
    {
        int idx = GettingGridPos(pos);
        
        if(idx == -1)
        {
            return;
        }

        buildings[idx] = newCh;

        Debug.Log(" - buidings[" + (idx % xSize) + ", " + (idx / xSize) + "] = " + newCh);
    }
}
