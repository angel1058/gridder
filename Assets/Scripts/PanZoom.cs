using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField] private float zoomOutMin = 1;
    [SerializeField] private float zoomOutMax = 8;
    [Range(0,1)]
    [SerializeField] private float pinchMultiplier = 0.025f;

    // Start is called before the first frame update
    void Start()
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
        UnityEngine.Debug.Log(difference * pinchMultiplier);
    }

    void Update()
    {
        if ( Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UnityEngine.Debug.Log(touchStart);
        }

        if (Input.touchCount == 2)
            handlePinchZoom();

        if ( Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }

        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment , zoomOutMin, zoomOutMax);
    }

}
