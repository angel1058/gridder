using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MeshMaker : MonoBehaviour
{
    public enum Edges
    {
        TOP = 1,
        LEFT = 2,
        RIGHT = 4,
        BOTTOM = 8
    }

    public int

            sizeX,
            sizeY;

    private MeshFilter mf;

    private List<Edges> _edges = new List<Edges>();

    private Color[,] _colorMap;

    public void SetEdge(Edges edge)
    {
        _edges.Add (edge);
    }

    public void SetColorMap(Color[,] colorMap)
    {
        _colorMap = colorMap;
    }

    private void Awake()
    {
        TryGetComponent(out mf);
    }

    public void OnValidate()
    {
        Start();
    }

    public void Start()
    {
        if (mf)
        {
            Plane plane =
                new Plane(mf.mesh,
                    sizeX,
                    sizeY,
                    _edges,
                    _colorMap,
                    this.gameObject.transform);
        }
    }
}

public abstract class GridGenerator
{
    protected Mesh mesh_;

    protected Vector3[] vertices;

    protected int[] triangles;

    protected Vector2[] uvs;

    public GridGenerator(Mesh mesh)
    {
        mesh_ = mesh;
    }
}

public class Plane : GridGenerator
{
    private int sizeX_;

    private int sizeY_;

    private List<SCR_MeshMaker.Edges> _edges;

    private Color[,] _colorMap;

    Transform _tf;

    public Plane(
        Mesh mesh,
        int sizeX,
        int sizeY,
        List<SCR_MeshMaker.Edges> edges,
        Color[,] colorMap,
        Transform tf
    ) :
        base(mesh)
    {
        _tf = tf;
        sizeX_ = sizeX;
        sizeY_ = sizeY;
        _edges = edges;
        _colorMap = colorMap;
        CreateMesh();
    }

    private void CreateMesh()
    {
        CreateVertices();
        CreateTriangles();

        mesh_.Clear();
        mesh_.vertices = vertices;
        mesh_.triangles = triangles;
        SplitMesh();
        SetColors();

        mesh_.RecalculateNormals();
    }

    private void CreateVertices()
    {
        vertices = new Vector3[(sizeX_ + 1) * (sizeY_ + 1)];
        for (
            int
                i = 0,
                y = 0;
            y <= sizeY_;
            y++
        )
        {
            for (int x = 0; x <= sizeX_; x++)
            {
                vertices[i] = new Vector3(x, y, 0.0f);
                i++;
            }
        }
    }

    void SplitMesh()
    {
        int[] triangles = mesh_.triangles;
        Vector3[] verts = mesh_.vertices;

        Vector3[] newVerts;

        int n = triangles.Length;
        newVerts = new Vector3[n];

        for (int i = 0; i < n; i++)
        {
            newVerts[i] = verts[triangles[i]];
            triangles[i] = i;
        }
        mesh_.vertices = newVerts;
        mesh_.triangles = triangles;
    }

    void SetColors()
    {
        Color[] colors = new Color[mesh_.vertexCount];

        int i = 0;
        for (int y = 0; y < sizeY_; y++)
        {
            for (int x = 0; x < sizeX_; x++)
            {
                for (int t = 0; t < 6; t++)
                {
                    if ( _tf == null) return;
                    if ( colors == null) return;
                    if ( _tf.position == null) return;
                    if ( _colorMap == null) return;
                    colors[i] = _colorMap[x + (int)((_tf.position.x)),(int)(y + (_tf.position.y))];
                    i++;
                }
            }
        }
        mesh_.colors = colors;
    }

  

    private void CreateTriangles()
    {
        int vert = 0;
        int tris = 0;

        triangles = new int[6 * sizeX_ * sizeY_];

        for (int y = 0; y < sizeY_; y++)
        {
            for (int x = 0; x < sizeX_; x++)
            {
                triangles[0 + tris] = vert + 0;
                triangles[1 + tris] = vert + sizeX_ + 1;
                triangles[2 + tris] = vert + sizeX_ + 2;

                triangles[3 + tris] = vert + sizeX_ + 2;
                triangles[4 + tris] = vert + 1;
                triangles[5 + tris] = vert + 0;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }
}
