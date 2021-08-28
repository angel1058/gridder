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

    public void OnDrawGizmos()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start(); 
        Gizmos.color = Color.white;

        for (int gridX = 0 ; gridX < gridWidth ; gridX++)
            for(int gridY = 0 ; gridY < gridHeight ; gridY++)
                {
                    if (groundCells[gridX,gridY] == 1)
                    {
                        Gizmos.DrawLine(new Vector3(gridX * cellSize , gridY * cellSize , 0) , new Vector3((gridX*cellSize)+cellSize , gridY * cellSize, 0) );
                        Gizmos.DrawLine(new Vector3((gridX*cellSize)+cellSize , gridY*cellSize , 0) , new Vector3((gridX*cellSize)+cellSize , (gridY*cellSize)+cellSize , 0) );
                        
                    }
                }

        sw.Stop();

        float gw = gridWidth * cellSize;
        float gh = gridHeight * cellSize;
        transform.position = new Vector2(-gw / 2 + cellSize /2 , gh / 2 - cellSize / 2);
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
