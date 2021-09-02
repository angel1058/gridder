using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MeshMaker : MonoBehaviour
{
    public int sizeX, sizeY;

    private MeshFilter mf;

    private void Awake()
    {
        TryGetComponent(out mf);
    }

    public void Validate()
    {
        Start();
    }

    public void Start()
    {
        if (mf)
        {
            Plane plane = new Plane(mf.mesh , sizeX , sizeY );
        }
    }
}


public abstract class GridGenerator
{
    protected Mesh mesh_;
    protected Vector3[] vertices;
    protected int[] triangles;
    protected Vector2[] uvs;
    protected Color[] colors;


    public GridGenerator(Mesh mesh)
    {
        mesh_ = mesh;
    }
} 

public class Plane : GridGenerator
{
    private int sizeX_;
    private int sizeY_;
    public Plane(Mesh mesh , int sizeX , int sizeY ) : base(mesh)
    {
        sizeX_ = sizeX;
        sizeY_ = sizeY;
        CreateMesh();
    }

    private void CreateMesh()
    {
        CreateVertices();
        CreateTriangles();
        CreateUVs();
        CreateColors();

        mesh_.Clear();
        mesh_.vertices = vertices;
        mesh_.triangles = triangles;
        SplitMesh();
        SetColors();
        // mesh_.uv = uvs;
//        mesh_.SetColors(colors);
        mesh_.RecalculateNormals();
    }

    private void CreateVertices()
    {

        vertices = new Vector3[(sizeX_ + 1 ) * (sizeY_ + 1 )];
        for (int i = 0, y  = 0; y  <= sizeY_; y ++)
        {
            for (int x = 0; x <= sizeX_; x++)
            {
                vertices[i] =  new Vector3(x , y , 0.0f);
                i++;
            }
        }
    }




void SplitMesh()
{
    
    int[] triangles = mesh_.triangles; 
    Vector3[] verts = mesh_.vertices;
    // Vector3[] normals = mesh_.normals;
    // Vector2[] uvs = mesh_.uv;

    Vector3[] newVerts;
    // Vector3[] newNormals;
    // Vector2[] newUvs;

    int n = triangles.Length;
    newVerts   = new Vector3[n];
    // newNormals = new Vector3[n];
    // newUvs     = new Vector2[n];

    for(int i = 0; i < n; i++)
    {
        newVerts[i] = verts[triangles[i]];
        // newNormals[i] = normals[triangles[i]];
        // if (uvs.Length > 0)
        // {
        //     newUvs[i] = uvs[triangles[i]];
        // }
        triangles[i] = i; 
    }        
    mesh_.vertices = newVerts;
    // mesh_.normals = newNormals;
    // mesh_.uv = newUvs;        
    mesh_.triangles = triangles;            
}   
void SetColors()
{
    Color[] colors = new Color[mesh_.vertexCount];

    Color[] chooseThese = new Color[2] {
        new Color(30f/255f,126f/255f,05f/255f), 
        new Color(35f/255f,131f/255f,10f/255f)
    };

    // Color[] chooseThese = new Color[2] { Color.red , Color.green};
    bool colorChooser = false;;
    int i = 0;
    for (int x = 0 ; x < sizeX_ ; x++)
    {
        for (int y = 0 ; y < sizeY_ ; y++)
        {
            Color c = chooseThese[colorChooser ? 0 : 1];
            for (int t = 0; t < 6; t++)
            {
             colors[i] = c;
             i++;    
            }
            colorChooser = !colorChooser;
        }
        //if we have odd number of columns bounce the color again
        if (sizeX_%2 == 0)
            colorChooser = !colorChooser;
    }
    mesh_.colors = colors;
}
//102966
private void CreateColors()
{
    int colorID = 0;
    bool isRed  =false;
    colors = new Color[(sizeX_ + 1 ) * (sizeY_ + 1 )];
        for ( int y = 0 ; y < sizeY_ ; y++)
        {
        for ( int x = 0 ; x < sizeX_ ; x++)
        {

            colors[colorID] = isRed ? Color.red : Color.blue;
            colorID++;
        }
            isRed = !isRed;
        
        }
}

    private void CreateTriangles()
    {
        int vert = 0;
        int tris = 0;

        triangles = new int[6 * sizeX_ * sizeY_];

        for ( int y = 0 ; y < sizeY_ ; y++)
        {
        for ( int x = 0 ; x < sizeX_ ; x++)
        {
            triangles[0+tris] = vert + 0;     
            triangles[1+tris] = vert + sizeX_ + 1;   
            triangles[2+tris] = vert + sizeX_ + 2;

            triangles[3+tris] = vert + sizeX_ + 2;
            triangles[4+tris] = vert + 1;
            triangles[5+tris] = vert + 0;
            vert++;
            tris += 6;
        }
        vert++;
        }
    }

    private Vector2 convertPixelsToVector(int x , int y , int textureWidth , int textureHeight)
    {
        return new Vector2((float)x / textureWidth , (float)y / textureHeight);
    }

    private Vector2[] GetUVRectangleFromPixels(int x , int y , int width , int height , int textureWidth , int textureHeight)
    {
        return new Vector2[] {
            convertPixelsToVector(x,y + height , textureWidth , textureHeight),
            convertPixelsToVector(x + width , y + height , textureWidth , textureHeight),
            convertPixelsToVector(x , y, textureWidth , textureHeight),
            convertPixelsToVector(x + width , y, textureWidth , textureHeight)
        };
    }

    private void CreateUVs()
    {


        // int textureWidth = 32;
        // int textureHeight = 32;
        // int spriteWidth = 32;
        // int spriteHeight = 32;

        // Vector2[] evenCell = GetUVRectangleFromPixels(0,0,spriteWidth,spriteHeight,textureWidth,textureHeight);
        // Vector2[] oddCell =  GetUVRectangleFromPixels(0,0,spriteWidth,spriteHeight,textureWidth,textureHeight);
        // uvs = new Vector2[(sizeX_+1) * (sizeY_+1)];
        // for ( int i = 0 ; i < uvs.Length ; i++)
        //     uvs[i] = new Vector2(0,0);


      
        //  uvs[0] = oddCell[0];
        //  uvs[1] = oddCell[1];
        //  uvs[4] = oddCell[2];
        //  uvs[5] = oddCell[3];

        //  uvs[2] = evenCell[0];
        //  uvs[3] = evenCell[1];
        // uvs[6] = evenCell[2];
        // uvs[7] = evenCell[3];


        // uvs[8+0] = evenCell[0];
        // uvs[8+1] = evenCell[1];
        // uvs[8+2] = 0venCell[2];
        // uvs[8+3] = 1venCell[3];4
        // uvs = new Vector2[(size5_+1) * (sizeY_+1)];
        // UnityEngine.Debug.Log(uvs.Length);
        // UnityEngine.Debug.Log(vertices.Length);
        // int uvIndex = 0;
        // bool odd = false;
        // while ( uvIndex < (sizeX_ * sizeY_) )
        // {
        //     Vector2[] cell = odd ? evenCell : oddCell;

        //     uvs[uvIndex] = cell[0];
        //     uvs[uvIndex+1] = cell[1];
        //     uvs[uvIndex+2] = cell[2];
        //     uvs[uvIndex+3] = cell[3];
        //     uvIndex += 4;
        //     odd = !odd;
        // }
    }



}