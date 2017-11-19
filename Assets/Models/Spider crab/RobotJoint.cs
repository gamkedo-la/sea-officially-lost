using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 Axis;
    public Vector3 StartOffset;

    public float MinAngle = -360f;
    public float MaxAngle = 360f;

    public bool RootJoint;


    void Awake()
    {
        if (transform.parent == null || transform.parent.GetComponent<RobotJoint>() == null)
        {
            StartOffset = Vector3.zero;
            RootJoint = true;
        }
        else
        {
            var worldOffset = transform.position - transform.parent.position;
            StartOffset = transform.parent.InverseTransformDirection(worldOffset);
        }
    }


    public Vector3 Angle()
    {
        if (RootJoint)
            return transform.eulerAngles;
        else
            return transform.localEulerAngles;
    }
}
