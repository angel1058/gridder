using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Control : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField] private float zoomOutMin = 0.1F;
    [SerializeField] private float zoomOutMax = 800;
    [SerializeField] private float scrollMultiplier = 2f;
    [SerializeField] private Camera mainCamera;
    private int gridSize = 8;

    [Range(0,1)]
    [SerializeField] private float pinchMultiplier = 0.025f;
    [SerializeField] private GameObject gridManager;

    SCR_GridManager script_GridManager = null;
    float holdTime = 0f;
    public float minHoldTime = .4f;
    Vector2 _startPosition = Vector2.zero;

    public void Start() 
    {
        UnityEngine.Debug.Log("Startup");
        script_GridManager = gridManager.GetComponent<SCR_GridManager>();
        gridSize = script_GridManager.cellSize;
    }

    public void Awake()
    {
        UnityEngine.Debug.Log("Awake");
        script_GridManager = gridManager.GetComponent<SCR_GridManager>();

    }


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


        var touchOne = Input.GetTouch(0);
        var touchTwo = Input.GetTouch(1);
 
        if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
        _startPosition = touchTwo.position - touchOne.position;
    
        if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
        {
            var currVector = touchTwo.position - touchOne.position;
            var angle = Vector2.SignedAngle(_startPosition, currVector);
            mainCamera.transform.rotation = Quaternion.Euler(0.0f, -180f , mainCamera.transform.rotation.eulerAngles.z - angle);
            _startPosition = currVector;
        }
    }

    void OnClick()
    {
        Vector3 clickPos  = touchStart;
        int x= (int)((clickPos.x ) / gridSize) ;
        int y= (int)(clickPos.y / gridSize);
        UnityEngine.Debug.Log(string.Format("Clicked : {0}, {1}" , x , y));

        script_GridManager.SetGridValue(x,y,2);
    }

    void MoveCamera()
    {
        Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mainCamera.transform.position += direction;
    }

    void Update()
    {
        // if ( gridMaker == null)
        //     gridMaker  = gridMakerTransform.GetComponent<GridMaker>();

        if (Input.GetMouseButtonUp(0)) 
        {
            if ( holdTime < minHoldTime)
                OnClick();
          
            holdTime = 0;
        }

        if ( Input.GetMouseButtonDown(0))
        {
            holdTime += Time.deltaTime;
            Vector3 mousePos = new Vector3(Input.mousePosition.x ,Input.mousePosition.y  ,  mainCamera.nearClipPlane);
            var oldTouchStart = touchStart.x;
            touchStart = mainCamera.ScreenToWorldPoint(mousePos);
            UnityEngine.Debug.Log("Touch start = " + touchStart + " Delta : " + (touchStart.x - oldTouchStart));
            
            UnityEngine.Debug.Log("Input Pos = " + mousePos);
        }

        if ( Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            MoveCamera();
        }

        if (Input.touchCount == 2)
            handlePinchZoom();

        zoom(Input.GetAxis("Mouse ScrollWheel") * scrollMultiplier);

    //  Camera cam = GetComponent<Camera>();
        Vector3 cameraPos = mainCamera.transform.position;
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        float top = cameraPos.y;
        float left = cameraPos.x;
        float bottom = cameraPos.y + height;
        // UnityEngine.Debug.Log("top : " + top + " , Left: " + left + " , Width : " + width +" , Height : " + height);
    }

    void zoom(float increment)
    {
        // if ( Camera == null) UnityEngine.Debug.Log("CAMERA IS NULL");
        if ( mainCamera == null) UnityEngine.Debug.Log("CAMERA.MAIN IS NULL");
        


        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment , zoomOutMin, zoomOutMax);
    }

}
