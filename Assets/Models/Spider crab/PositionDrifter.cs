using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDrifter : MonoBehaviour
{
    [SerializeField] float m_driftRadius = 0.3f;
    [SerializeField] float m_driftRate = 0.1f;


    private Vector3 m_startPosition;
    private Vector3 m_scale;
    private float m_offset;


    void Awake()
    {
        m_startPosition = transform.localPosition;
        m_scale = new Vector3(1.0f / transform.lossyScale.x, 1.0f / transform.lossyScale.y, 1.0f / transform.lossyScale.z);
        m_offset = Random.Range(0f, 10f);
    }


	void Update()
    {
        float radius = Mathf.PerlinNoise(m_offset, Time.time * m_driftRate) * m_driftRadius;
        float theta = Mathf.PerlinNoise(m_offset + 1f, Time.time * m_driftRate) * Mathf.PI * 2f;
        float thi = Mathf.PerlinNoise(m_offset + 2f, Time.time * m_driftRate) * Mathf.PI;

        float x = radius * Mathf.Sin(thi) * Mathf.Cos(theta) * m_scale.x;
        float y = radius * Mathf.Sin(thi) * Mathf.Sin(theta) * m_scale.y;
        float z = radius * Mathf.Cos(thi) * m_scale.z;

        transform.localPosition = m_startPosition + new Vector3(x, y, z);	
	}
}
