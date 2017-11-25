using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpiderCrabWithIKController : MonoBehaviour
{
    [Header("IK settings")]
    [SerializeField] InverseKinematicsController m_ikController;
    [SerializeField] float m_idleDistanceThreshold = 0f;
    [SerializeField] float m_idleLearningRate = 2f;
    [SerializeField] float m_feedingDistanceThreshold = 0.1f;
    [SerializeField] float m_feedingLearningRate = 10f;

    [Header("Gizmo settings")]
    [SerializeField] float m_gizmoRadius = 0.2f;

    [Header("Target")]
    [SerializeField] bool m_showTargetGizmo = true;
    [SerializeField] Color m_targetGizmoColour = Color.red;
    [SerializeField] Transform m_target;

    [Header("Rest point")]
    [SerializeField] bool m_showRestPointGizmo;
    [SerializeField] Color m_restPointGizmoColour = Color.magenta;
    [SerializeField] Transform m_restPoint;

    [Header("Mouth position")]
    [SerializeField] bool m_showMouthPositionGizmo;
    [SerializeField] Color m_mouthGizmoColour = Color.yellow;
    [SerializeField] Transform m_mouthPosition;
    
    [Header("Food positions")]
    [SerializeField] bool m_showFoodPositionGizmo;
    [SerializeField] Color m_foodGizmoColour = Color.green;
    [SerializeField] Transform[] m_foodPositions;

    [Header("Feeding behaviour")]
    [SerializeField] Vector2 m_idleTimeMinMax = new Vector2(10f, 30f);
    [SerializeField] Vector2 m_feedingTimeMinMax = new Vector2(10f, 30f);
    [SerializeField] float m_settlingTime = 3f;

    private Animator m_anim;
    private int m_chewHash;
    private bool m_feeding = false;
    private Coroutine m_feedingCoroutine;


    void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_chewHash = Animator.StringToHash("Chew");
    }


    void Start()
    {
        SetTargetPosition(m_restPoint);
        SetIkSettings(m_idleDistanceThreshold, m_idleLearningRate);
        StartCoroutine(IdleFeedingTranstion());
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_feeding = !m_feeding;
    }


    private void Chew()
    {
        m_anim.SetTrigger(m_chewHash);
    }


    void OnDrawGizmos()
    {
        if (m_showTargetGizmo && m_target != null)
        {
            Gizmos.color = m_targetGizmoColour;
            Gizmos.DrawSphere(m_target.position, m_gizmoRadius);
        }

        if (m_showRestPointGizmo && m_restPoint != null)
        {
            Gizmos.color = m_restPointGizmoColour;
            Gizmos.DrawSphere(m_restPoint.position, m_gizmoRadius);
        }

        if (m_showMouthPositionGizmo && m_mouthPosition != null)
        {
            Gizmos.color = m_mouthGizmoColour;
            Gizmos.DrawSphere(m_mouthPosition.position, m_gizmoRadius);
        }

        if (m_showFoodPositionGizmo)
        {
            Gizmos.color = m_foodGizmoColour;

            for (int i = 0; i < m_foodPositions.Length; i++)
                Gizmos.DrawSphere(m_foodPositions[i].position, m_gizmoRadius);
        }
    }


    private IEnumerator IdleFeedingTranstion()
    {
        while (true)
        {
            float idleTime = Random.Range(m_idleTimeMinMax.x, m_idleTimeMinMax.y);
            //print("Idle time: " + idleTime);

            yield return new WaitForSeconds(idleTime);

            m_feeding = true;
            SetIkSettings(m_feedingDistanceThreshold, m_feedingLearningRate);
            m_feedingCoroutine = StartCoroutine(Feeding());

            float feedingTime = Random.Range(m_feedingTimeMinMax.x, m_feedingTimeMinMax.y);
            //print("Feeding time: " + feedingTime);

            yield return new WaitForSeconds(feedingTime);

            m_feeding = false;
            SetIkSettings(m_idleDistanceThreshold, m_idleLearningRate);
            StopCoroutine(m_feedingCoroutine);

            SetTargetPosition(m_restPoint);

            yield return null;
        }
    }


    private IEnumerator Feeding()
    {
        while (m_feeding)
        {
            int foodPositionIndex = Random.Range(0, m_foodPositions.Length);
            var foodPosition = m_foodPositions[foodPositionIndex];

            SetTargetPosition(foodPosition);

            yield return new WaitForSeconds(m_settlingTime);

            SetTargetPosition(m_mouthPosition);

            yield return new WaitForSeconds(m_settlingTime);
        }
    }


    private void SetTargetPosition(Transform targetPosition)
    {
        if (m_target != null && targetPosition != null)
        {
            m_target.parent = targetPosition;
            m_target.localPosition = Vector3.zero;
        }
    }


    private void SetIkSettings(float distanceThreshold, float learningRate)
    {
        if (m_ikController == null)
            return;

        m_ikController.DistanceThreshold = distanceThreshold;
        m_ikController.LearningRate = learningRate;
    }
}
