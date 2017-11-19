using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 Axis;
    public Vector3 StartOffset;

    public float MinAngle = -360f;
    public float MaxAngle = 360f;

    void Awake()
    {
        StartOffset = Vector3.Scale(transform.localPosition, transform.lossyScale);
    }
}
