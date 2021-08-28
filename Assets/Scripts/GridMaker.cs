using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private int cellSize;
    

    int [,] groundCells;
    Dictionary<System.Tuple<int , int>, int> beltCells;
    
    private static long updateCount = 0;
    
    void Start()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start(); 
        groundCells = new int[gridWidth , gridHeight];
        for (int gridX = 0 ; gridX < gridWidth ; gridX++)
            for(int gridY = 0 ; gridY < gridHeight ; gridY++)
                {
                    groundCells[gridX , gridY] = 1;
                }
        sw.Stop();

        UnityEngine.Debug.Log(String.Format("Generated ground cells in {0} ms" , sw.ElapsedMilliseconds));

    }

    private void OnDrawGizmos()
    {
     /*   Stopwatch sw = new Stopwatch();
        sw.Start(); 
        Gizmos.color = Color.white;

        for (int gridX = 0 ; gridX < gridWidth ; gridX++)
            for(int gridY = 0 ; gridY < gridHeight ; gridY++)
                {
                    Gizmos.DrawLine(transform.position, new Vector2(gridX * cellSize,  gridY * cellSize , 0));
                }

        sw.Stop();

        UnityEngine.Debug.Log(String.Format("Drew gizmos in {0} ms" , sw.ElapsedMilliseconds));
        float gw = gridWidth * cellSize;
        float gh = gridHeight * cellSize;
        transform.position = new Vector2(-gw / 2 + cellSize /2 , gh / 2 - cellSize / 2);
    */
    }

    void Update()
    {
        updateCount++;
        UnityEngine.Debug.Log(String.Format("Update Count {0}" , updateCount));

    }
}
