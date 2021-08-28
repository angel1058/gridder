using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField] private float zoomOutMin = 0.1F;
    [SerializeField] private float zoomOutMax = 800;
    
    [Range(0,1)]
    [SerializeField] private float pinchMultiplier = 0.025f;

    float holdTime = 0f;
    public float minHoldTime = .4f;

    public Transform gridMakerTransform; 


    static GridMaker gridMaker = null;

    private Vector2  getPreviousPos(int index)
    {
            Touch touchZero = Input.GetTouch(index);
            return touchZero.position - touchZero.deltaPosition;
    }

    private void handlePinchZoom()
    {
        Vector2 touchZeroPrevPos = getPreviousPos(0);
        Vector2 touchOnePrevPos = getPreviousPos(1);

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
        float difference = currentMagnitude - prevMagnitude;
        zoom( difference * pinchMultiplier);
    }

    void OnClick()
    {
            //work out x / y
            Vector3 clickPos  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x= (int)(clickPos.x / gridMaker.CellSize);
            int y= (int)(clickPos.y / gridMaker.CellSize);
            gridMaker.SetGridBlue(x,y);

    }

    void MoveCamera()
    {
        Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Camera.main.transform.position += direction;
    }

    void Update()
    {
        if ( gridMaker == null)
            gridMaker  = gridMakerTransform.GetComponent<GridMaker>();

        if (Input.GetMouseButtonUp(0)) 
        {
            if ( holdTime < minHoldTime)
                OnClick();
          
            holdTime = 0;
        }

        if ( Input.GetMouseButtonDown(0))
        {
            holdTime += Time.deltaTime;
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if ( Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }

        if (Input.touchCount == 2)
            handlePinchZoom();

        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment , zoomOutMin, zoomOutMax);
    }

}
