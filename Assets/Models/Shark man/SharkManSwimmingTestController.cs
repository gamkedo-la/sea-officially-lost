using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SharkManSwimmingTestController : MonoBehaviour
{
    [SerializeField] int m_biteLayerIndex = 1;
    [SerializeField] string m_biteStateName = "Bite";
    [SerializeField] string m_biteTriggerName = "Bite";
    [SerializeField] float m_speedChangeRate = 0.2f;


    private Animator m_anim;
    private string m_biteLayerName;
    private int m_biteTriggerHash;
    private int m_biteStateHash;
    private float m_speed = 0.5f;

    //private int m_baseLayerState;
    //private int m_biteLayerState;

    void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_biteLayerName = m_anim.GetLayerName(m_biteLayerIndex);
        m_biteStateHash = Animator.StringToHash(m_biteLayerName + "." + m_biteStateName);
        m_biteTriggerHash = Animator.StringToHash(m_biteTriggerName);
        //print(m_biteStateHash);
    }


    void Update()
    {
        //m_baseLayerState = m_anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        //m_biteLayerState = m_anim.GetCurrentAnimatorStateInfo(1).fullPathHash;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var state = m_anim.GetCurrentAnimatorStateInfo(m_biteLayerIndex);

            //print(state.fullPathHash);

            if (state.fullPathHash != m_biteStateHash)
                m_anim.SetTrigger(m_biteTriggerHash);
        }

        if (Input.GetKey(KeyCode.X))
        {
            m_speed += Time.deltaTime * m_speedChangeRate;
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            m_speed -= Time.deltaTime * m_speedChangeRate;
        }

        m_speed = Mathf.Clamp01(m_speed);

        m_anim.SetFloat("Speed", m_speed);
    }
}
