using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] Vector3 m_axis;
	[SerializeField] float m_rotationRate = 5f;

    private Quaternion rotation;
        

    void Start()
    {
        rotation = transform.rotation;
    }


    void LateUpdate()
    {
        rotation *= Quaternion.Euler(m_axis * m_rotationRate * Time.deltaTime);
        transform.rotation = rotation;
    }
}
