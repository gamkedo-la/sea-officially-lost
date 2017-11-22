using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicsController : MonoBehaviour
{
    [SerializeField] bool m_showGizmos;
    [SerializeField] float m_gizmoRadius = 0.2f;
    [SerializeField] Transform m_target;
    [SerializeField] RobotJoint[] Joints;
    [SerializeField] float SamplingDistance = 1f;
    [SerializeField] float DistanceThreshold = 0.1f;
    [SerializeField] float LearningRate = 10f;

    private Vector3[] angles;
    private Vector3 testPoint;
    private Quaternion rotation;


    void LateUpdate()
    {
        if (m_target != null)
        {
            angles = new Vector3[Joints.Length];

            for (int i = 0; i < angles.Length; i++)
                angles[i] = Joints[i].Angle;

            InverseKinematics(m_target.position, angles);

            for (int i = 0; i < Joints.Length; i++)
            {
                var joint = Joints[i];
                var angle = angles[i];

                joint.Angle = angle;
            }
        }
    }


    private Vector3 ForwardKinematics(Vector3[] angles)
    {
        Vector3 prevPoint = Joints[0].transform.position;

        Quaternion rotation = Quaternion.identity;
        for (int i = 1; i < Joints.Length; i++)
        {
            // Rotates around a new axis
            rotation *= Quaternion.Euler(angles[i - 1]);
            //rotation *= Quaternion.AngleAxis(angles[i - 1], Joints[i - 1].Axis);
            Vector3 nextPoint = prevPoint + rotation * Joints[i].StartOffset;

            prevPoint = nextPoint;
        }

        testPoint = prevPoint;

        return prevPoint;
    }


    private float DistanceFromTarget(Vector3 target, Vector3[] angles)
    {
        Vector3 point = ForwardKinematics(angles);
        return Vector3.Distance(point, target);
    }


    private float PartialGradient(Vector3 target, Vector3[] angles, int i)
    {
        // Saves the angle,
        // it will be restored later
        var angle = angles[i];

        // Gradient : [F(x+SamplingDistance) - F(x)] / h
        float f_x = DistanceFromTarget(target, angles);

        angles[i] += SamplingDistance * Joints[i].Axis;
        float f_x_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_x_plus_d - f_x) / SamplingDistance;

        // Restores
        angles[i] = angle;

        return gradient;
    }


    private void InverseKinematics(Vector3 target, Vector3[] angles)
    {
        float distanceFromTarget = DistanceFromTarget(target, angles);

        //print(distanceFromTarget);

        if (distanceFromTarget < DistanceThreshold)
            return;

        for (int i = Joints.Length - 1; i >= 0; i--)
        {
            // Gradient descent
            // Update : Solution -= LearningRate * Gradient
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= Joints[i].Axis * LearningRate * gradient;

            // Clamp
            float axisAngle = AxisAngle(angles[i], Joints[i].Axis);
            axisAngle = Mathf.Clamp(axisAngle, Joints[i].MinAngle, Joints[i].MaxAngle);
            angles[i] = SetAxisAngle(axisAngle, angles[i], Joints[i].Axis);

            // Early termination
            if (DistanceFromTarget(target, angles) < DistanceThreshold)
                return;
        }
    }


    private float AxisAngle(Vector3 rotation, Vector3 axis)
    {
        var angle = Vector3.Scale(rotation, axis).magnitude;

        return angle;
    }


    private Vector3 SetAxisAngle(float axisAngle, Vector3 angle, Vector3 axis)
    {
        var angleAlongAxis = axis * axisAngle;
        var otherAngles = Vector3.Scale(Vector3.one - axis, angle);

        //print("Initial angle: " + angle);
        //print("axisAngle: " + axisAngle);
        //print("axis:" + axis);
        //print("angleAlongAxis: " + angleAlongAxis);
        //print("otherAngles: " + otherAngles);

        angle = angleAlongAxis + otherAngles;

        //print("Final angle: " + angle);

        return angle;
    }


    void OnDrawGizmos()
    {
        if (!m_showGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(testPoint, m_gizmoRadius);
    }
}
