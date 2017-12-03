using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMandiblesTrigger : MonoBehaviour
{
    private SpiderCrabWithIKController m_controller;


    void Awake()
    {
        m_controller = GetComponentInParent<SpiderCrabWithIKController>();
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("crab mouth") && m_controller != null)
            m_controller.Chew();
    }
}
