using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Droid : MonoBehaviour
{
    public int droidSpeed;
    public int droidRotateSpeed;
    private Vector2 _target;
    private bool _moving = false;

    //how we determine if we're there yet?
    private bool isTargetXGreater;
    private bool isTargetYGreater;

    public void SetTargetLocation(Vector2 target)
    {
        if (_moving) return;
        _target = target;
        //offset due to rotation point
        _target.x += 0.5f;
        _target.y += 0.5f;

        Debug.Log("Setting droid target" + _target);
        Debug.Log("Droid currently at " + transform.position);

        _moving = _target != (Vector2)(transform.position);
    }

    void Start()
    {
        _target = transform.position;
        Debug.Log("Droid started at " + _target);
    
    }


    void Update()
    {
        if (_target == null || !_moving)
            return;


        if ( (Vector2)(transform.position) == _target)
        {
            transform.position = _target;
            _moving = false;
            return;
        }
        

        transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * droidSpeed * 2);

        //now the rotate
            Vector3 myLocation = transform.position;
            Vector3 targetLocation = _target;
            targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position
            
            // vector from this object towards the target location
            Vector3 vectorToTarget = targetLocation - myLocation;
            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * vectorToTarget;
            
            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
            
            // changed this from a lerp to a RotateTowards because you were supplying a "speed" not an interpolation value
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, droidSpeed*200 * Time.deltaTime);
    }
}
