using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivePosition
{
    private Vector3 initialPosition;
    private Vector3 endPosition;
    private Quaternion initialRotation;
    private Quaternion endRotation;

    private Vector3 currentPosition;
    private Quaternion currentRotaion;

    private float moveTime, rotateTime;
    Action endAction;
    public LivePosition(Vector3 initialPosition, Vector3 endPosition, Quaternion initialRotation, Quaternion endRotation)
    {
        this.initialPosition = initialPosition;
        this.endPosition = endPosition;
        this.initialRotation = initialRotation;
        this.endRotation = endRotation;

        currentPosition = initialPosition;
        currentRotaion = initialRotation;

        moveTime = 0;
        rotateTime = 0;
    }

    public void OnUpdate()
    {
        if (currentPosition != endPosition)
        {
            moveTime += Time.deltaTime;
            currentPosition = Vector3.Lerp(initialPosition, endPosition, moveTime);
            if(moveTime > 0.5f)
            {
                rotateTime += Time.deltaTime * 3;
                currentRotaion = Quaternion.Lerp(initialRotation, endRotation, rotateTime);
            }
        }
        else
        {
            if (endAction != null)
            {
                endAction.Invoke();
                endAction = null;
            }
        }
    }

    public Vector3 GetPosition()
    {
        return currentPosition;
    }
    public Quaternion GetRotation()
    {
        return currentRotaion;
    }

    public void SetEndCall(Action endAction)
    {
        this.endAction = endAction;
    }
}
