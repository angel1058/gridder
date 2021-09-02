using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GridManager : MonoBehaviour
{
    [SerializeField] public int cellSize = 8;
    public SCR_GridManager()
    {
        beltCells = new Dictionary<Tuple<int , int > ,int>();
    }

    Dictionary<Tuple<int , int>, int> beltCells;
    
    void Start()
    {
    }

    public void SetGridValue(int x , int y , int value)
    {
        Tuple<int, int> co_ords = Tuple.Create(x , y);
        if ( beltCells == null) 
            return;
        
        if (!beltCells.ContainsKey(co_ords))
            beltCells[co_ords] = value;
    }


    private void DrawBox(int x , int y , Color c)
    {
        int left = (x * cellSize) + cellSize;
        int right = left + cellSize;
        int top = y * cellSize;
        int bottom = top + cellSize;

        Gizmos.color = c;

        Gizmos.DrawLine(new Vector3(left , top, 0) , new Vector3(right , top, 0) );
        Gizmos.DrawLine(new Vector3(right , top , 0) , new Vector3(right , bottom , 0) );
        Gizmos.DrawLine(new Vector3(right , bottom , 0) , new Vector3(left , bottom , 0) );
        Gizmos.DrawLine(new Vector3(left , bottom , 0) , new Vector3(left , top , 0) );
    }

    public void OnDrawGizmos()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start(); 
        Gizmos.color = Color.white;

        foreach( Tuple<int, int> t in beltCells.Keys)
            DrawBox(t.Item1 , t.Item2 , Color.red);
    }


    void Update()
    {
   
    }
}
