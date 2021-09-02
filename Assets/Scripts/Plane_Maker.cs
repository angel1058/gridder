using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Maker : MonoBehaviour
{
    public int Width , Height;

    public GameObject PFB_Plane;

    private GameObject[] planes;
    // Start is called before the first frame update
    void Start()
    {
        planes = new GameObject[Width * Height];
        for (  int y = 0 ; y < Height ; y++)
        for (  int x = 0 ; x < Width ; x++)
        {
            GameObject go = Instantiate(PFB_Plane , new Vector3(x * 50, y * 50,0) , Quaternion.identity); 
            go.name = "Plane_" + x + "_" + y;
            go.transform.SetParent(this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
