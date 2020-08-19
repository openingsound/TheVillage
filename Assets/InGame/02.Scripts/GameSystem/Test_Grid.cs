using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Grid : MonoBehaviour
{
    public GridMap grid;

    public int GridSize;
    

    public void OnClickResizeGrid()
    {
        if (grid.GridSize >= GridSize)
        {
            Debug.LogError("Error) 새 그리드맵의 크기는 기존 값 " + grid.GridSize + "보다 작을 수 없습니다!");
            return;
        }

        if(GridSize % 2 != 0)
        {
            Debug.LogError("Error) 그리드맵의 크기는 반드시 홀수여야 합니다!");
        }

        Debug.Log("-------[Create Grid " + GridSize + " X " + GridSize + " Size]-------");
        grid.ResizeGrid(GridSize);
    }
}
