using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Droid : MonoBehaviour
{
    public int droidSpeed;
    public int droidRotateSpeed;

    public float Energy;

    private Vector2 _finalTarget;
    private Vector2 _interimTarget;

    private Queue<Vector2> _movementPath = new Queue<Vector2>();

    private bool _moving = false;

    //how we determine if we're there yet?
    private bool isTargetXGreater;
    private bool isTargetYGreater;

    private float checkPosition(float current , float target)
    {
        if (current > target)
            return current-1;

        if (current < target)
            return current+1;

        return current;
    }

    private void generateMovementPath()
    {
        float tmpX = transform.position.x;
        float tmpY = transform.position.y;
        
        bool movingX = true;
        while ( tmpX != _finalTarget.x || tmpY != _finalTarget.y)
        {
            if (movingX)
                tmpX = checkPosition(tmpX, _finalTarget.x);
            else
                tmpY = checkPosition(tmpY, _finalTarget.y);

            movingX = !movingX;
            _movementPath.Enqueue(new Vector2(tmpX , tmpY));
        }

        foreach (Vector2 v2 in _movementPath)
            Debug.Log(v2);
        if (_movementPath.Count > 0)
        {
            Energy--;
            _interimTarget = _movementPath.Dequeue();
        }
    }

    public void SetTargetLocation(Vector2 target)
    {
        if (_moving || (target.x == 0 && target.y == 0)) return;
        _finalTarget = target;
        //offset due to rotation point
        _finalTarget.x += 0.5f;
        _finalTarget.y += 0.5f;

        generateMovementPath();

        _moving = _finalTarget != (Vector2)(transform.position);
    }

    void Start()
    {
        _finalTarget = transform.position;
    }


    void Update()
    {
        if (Energy <= 0)
            return;

        if (_finalTarget == null || !_moving)
            return;

        if ( (Vector2)(transform.position) == _finalTarget)
        {
            transform.position = _finalTarget;
            _moving = false;
            return;
        }
        
        if ( _interimTarget != _finalTarget && _movementPath.Count > 0 && _interimTarget.x == transform.position.x && _interimTarget.y == transform.position.y)
        {
            _interimTarget = _movementPath.Dequeue();
            Energy--;
        }


        Vector3 myLocation = transform.position;
        Vector3 targetLocation = _interimTarget;
        targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position
        
        Vector3 vectorToTarget = targetLocation - myLocation;
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * vectorToTarget;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, droidSpeed*50 * Time.deltaTime);

        if (targetRotation == transform.rotation)
            transform.position = Vector3.MoveTowards(transform.position, _interimTarget, Time.deltaTime * droidSpeed * 2);

    }
}
