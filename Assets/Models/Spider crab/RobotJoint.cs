using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 Axis;        

    public float MinAngle = -360f;
    public float MaxAngle = 360f;

    [HideInInspector]
    public Vector3 StartOffset;

    [HideInInspector]
    public bool RootJoint;

    private Vector3 angle;


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

        if (RootJoint)
            angle = transform.eulerAngles;
        else
            angle = transform.localEulerAngles;
    }


    public Vector3 Angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;

            if (RootJoint)
                transform.rotation = Quaternion.Euler(angle);
            else
                transform.localRotation = Quaternion.Euler(angle);
        }
    }
}
