using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAttackTrigger : MonoBehaviour
{
    private SpiderCrabWithIKController m_controller;


    void Awake()
    {
        m_controller = GetComponentInParent<SpiderCrabWithIKController>();
    }
	

    void OnTriggerEnter(Collider other)
    {
        m_controller.AttackTriggerEnter(other);
    }


    void OnTriggerExit(Collider other)
    {
        m_controller.AttackTriggerExit(other);
    }
}
