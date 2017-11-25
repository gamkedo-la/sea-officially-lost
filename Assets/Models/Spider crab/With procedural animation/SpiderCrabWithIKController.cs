using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpiderCrabWithIKController : MonoBehaviour
{
    [SerializeField] float m_gizmoRadius = 0.2f;

    [Header("Target")]
    [SerializeField] bool m_showTargetGizmo = true;
    [SerializeField] Transform m_target;

    [Header("Mouth position")]
    [SerializeField] bool m_showMouthPositionGizmo;
    [SerializeField] Transform m_mouthPosition;
    

    [Header("Food positions")]
    [SerializeField] bool m_showFoodPositionGizmo;
    [SerializeField] Transform[] m_foodPositions;


    private Animator m_anim;
    private int m_chewHash;


    void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_chewHash = Animator.StringToHash("Chew");
    }


    void Start()
    {

    }


    private void Chew()
    {
        m_anim.SetTrigger(m_chewHash);
    }


    void OnDrawGizmos()
    {
        if (m_showTargetGizmo && m_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_target.position, m_gizmoRadius);
        }

        if (m_showMouthPositionGizmo && m_mouthPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(m_mouthPosition.position, m_gizmoRadius);
        }

        if (m_showFoodPositionGizmo)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < m_foodPositions.Length; i++)
                Gizmos.DrawSphere(m_foodPositions[i].position, m_gizmoRadius);
        }
    }
}
