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

    public GameObject HunterDroid;
    public Transform Plane_Maker;

    int CenterX;
    int CenterY;

    [Range(0,1)]
    [SerializeField] private float pinchMultiplier = 0.025f;

    SCR_Plane_Maker script_Plane_Maker = null;
    SCR_Droid script_droid = null;

    float holdTime = 0f;
    public float minHoldTime = .2f;
    Vector2 _startPosition = Vector2.zero;

    Vector3 _centerVector;

    GameObject hunterDroid; 
    public void Start() 
    {
        script_Plane_Maker = Plane_Maker.GetComponent<SCR_Plane_Maker>();
        
        CenterX = (int)(script_Plane_Maker.GridWidth * script_Plane_Maker.MeshWidth / 2);
        CenterY = (int)(script_Plane_Maker.GridHeight * script_Plane_Maker.MeshHeight / 2);

        _centerVector = new Vector3(CenterX + 0.5f , CenterY  + 0.5f, 0);
        Debug.Log("Center is " + _centerVector);
        SetupHunterDroid();
        
        _centerVector.z = 0;
        Vector3 cameraVector = mainCamera.transform.position;
        cameraVector.x = _centerVector.x;
        cameraVector.y = _centerVector.y;

        mainCamera.transform.position = cameraVector;
    }

    private void SetupHunterDroid( )
    {
        //droid needs an initial offset as its center point will 
        hunterDroid = Instantiate(HunterDroid,_centerVector,Quaternion.identity);
        hunterDroid.transform.SetParent(this.transform);
        hunterDroid.transform.position = _centerVector;
        script_droid = hunterDroid.transform.GetComponent<SCR_Droid>();
    }


    public void Awake()
    {

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
            mainCamera.transform.rotation = Quaternion.Euler(0.0f, 0.0f , mainCamera.transform.rotation.eulerAngles.z - angle);
            _startPosition = currVector;
        }
    }

    void OnClick()
    {
        Vector3 clickPos  = touchStart;
        int x= (int)((clickPos.x ) ) ;
        int y= (int)(clickPos.y );
        UnityEngine.Debug.Log(string.Format("Clicked : {0}, {1}" , x , y));
        Vector2 droidMove = new Vector2(x , y );
        script_droid.SetTargetLocation(droidMove);
    }

    void MoveCamera()
    {
        Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction.z = 0;
        Debug.Log("Move Camera - direction : " + direction);
        
        mainCamera.transform.position += direction;
        
        Debug.Log("Move Camera - now at" + mainCamera.transform.position);
    }

    void Update()
    {
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
        }

        if ( Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            if ( holdTime > minHoldTime)
                MoveCamera();
        }

        if (Input.touchCount == 2)
            handlePinchZoom();

        zoom(Input.GetAxis("Mouse ScrollWheel") * scrollMultiplier);

        Vector3 cameraPos = mainCamera.transform.position;
        float height = 2f * mainCamera.orthographicSize;

        float top = cameraPos.y;
        float left = cameraPos.x;
        float bottom = cameraPos.y + height;
    }

    void zoom(float increment)
    {
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment , zoomOutMin, zoomOutMax);
    }

}
