using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private int cellSize = 5;
    
    public int CellSize {get {return cellSize;}}

    int [,] groundCells;
    Dictionary<System.Tuple<int , int>, int> beltCells;
    
    private static long updateCount = 0;
    
    

    void Start()
    {
        loadGrid();
    }

    public void OnValidate()
    {
        loadGrid();
    }

    public void SetGridBlue(int x , int y)
    {
        groundCells[x,y] = 2;        
    }

    public void OnDrawGizmos()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start(); 
        Gizmos.color = Color.white;

        for (int gridX = 0 ; gridX < gridWidth ; gridX++)
            for(int gridY = 0 ; gridY < gridHeight ; gridY++)
                {
                    int left = gridX * cellSize;
                    int right = left + cellSize;
                    int top = gridY * cellSize;
                    int bottom = top + cellSize;
                    if (groundCells[gridX,gridY] > 0)
                    {
                        switch ( groundCells[gridX,gridY])
                        {
                            case 1 : Gizmos.color = Color.blue;break;
                            case 2 : Gizmos.color = Color.red;break;
                        }
                        Gizmos.DrawLine(new Vector3(left , top, 0) , new Vector3(right , top, 0) );
                        Gizmos.DrawLine(new Vector3(right , top , 0) , new Vector3(right , bottom , 0) );
                        Gizmos.DrawLine(new Vector3(right , bottom , 0) , new Vector3(left , bottom , 0) );
                        Gizmos.DrawLine(new Vector3(left , bottom , 0) , new Vector3(left , top , 0) );
                        //

                    }
                }

        sw.Stop();

        float gw = gridWidth * cellSize;
        float gh = gridHeight * cellSize;
     //  transform.position = new Vector2(-gw / 2 + cellSize /2 , gh / 2 - cellSize / 2);
    }

    private void loadGrid()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start(); 
        groundCells = new int[gridWidth , gridHeight];
        for (int gridX = 0 ; gridX < gridWidth ; gridX++)
            for(int gridY = 0 ; gridY < gridHeight ; gridY++)
                {
                    if ( gridX == 0 || gridY == 0 || gridX == gridWidth-1 || gridY == gridHeight-1)
                        groundCells[gridX , gridY] = 1;
                    else
                        groundCells[gridX , gridY] = 0;
                }
        sw.Stop();
    }

 

    void Update()
    {
   
    }
}
