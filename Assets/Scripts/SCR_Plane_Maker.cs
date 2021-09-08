using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Plane_Maker : MonoBehaviour
{
    public GameObject PFB_Plane;

    [Header("Grid Settings")]
    public int GridWidth = 5;
    public int GridHeight = 5;
    public int MeshWidth = 31;
    public int MeshHeight = 31;

    [Header("Noise Settings")]
    [Range(1, 2)]
    public int NoiseType = 1;

    [Header("Smooth Settings")]
    public int smoothIterations;
    [Range(1, 10)]
    public int edgeSize = 1;
    [Range(1,100)]
    public int waterFillPercent = 66;
    public int  waterFillPercentRows = 2;

    [Range(1,100)]
    public int inlandWater = 25;
    public int inlandSoil = 25;


    [Range(1, 10)]
    public int cornerRadius = 1;


    [Header("Perlin Settings")]
    public int magnification = 77;


    private GameObject[] planes;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        NoiseGenerator ng =
            new NoiseGenerator(GridWidth * MeshWidth,
                GridHeight * MeshHeight,
                magnification,
                edgeSize,
                cornerRadius,
                waterFillPercent,
                waterFillPercentRows,smoothIterations, inlandWater , inlandSoil);
        Color[,] fullNoiseMap = ng.GenerateNoise(NoiseType);
        planes = new GameObject[GridWidth * GridHeight];
        for (int y = 0; y < GridHeight; y++)
        for (int x = 0; x < GridWidth; x++)
        {
            GameObject go =
                Instantiate(PFB_Plane,
                new Vector3(x * MeshWidth, y * MeshHeight, 0),
                Quaternion.identity);
            SCR_MeshMaker meshMaker = go.GetComponent<SCR_MeshMaker>();
            meshMaker.sizeX = MeshWidth;
            meshMaker.sizeY = MeshHeight;

            if (x == 0) meshMaker.SetEdge(SCR_MeshMaker.Edges.LEFT);
            if (y == 0) meshMaker.SetEdge(SCR_MeshMaker.Edges.BOTTOM);
            if (x == GridWidth - 1)
                meshMaker.SetEdge(SCR_MeshMaker.Edges.RIGHT);
            if (y == GridHeight - 1) meshMaker.SetEdge(SCR_MeshMaker.Edges.TOP);

            meshMaker.SetColorMap (fullNoiseMap);
            go.name = "Plane_" + x + "_" + y;
            go.transform.SetParent(this.transform);
            planes[i++] = go;
        }
    }

    public void OnValidate()
    {
      
    }
}
