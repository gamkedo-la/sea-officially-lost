using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpiderCrabWithIKController : MonoBehaviour
{
    [Header("IK settings")]
    [SerializeField] InverseKinematicsController m_ikControllerLeft;
    [SerializeField] InverseKinematicsController m_ikControllerRight;
    [SerializeField] float m_idleDistanceThreshold = 0f;
    [SerializeField] float m_idleLearningRate = 2f;
    [SerializeField] float m_feedingDistanceThreshold = 0.1f;
    [SerializeField] float m_feedingLearningRate = 10f;

    [Header("Targets")]
    [SerializeField] bool m_showTargetGizmo = true;
    [SerializeField] Color m_targetLeftGizmoColour = Color.red;
    [SerializeField] Color m_targetRightGizmoColour = Color.red;
    [SerializeField] float m_targetGizmoRadius = 0.13f;
    [SerializeField] Transform m_targetLeft;
    [SerializeField] Transform m_targetRight;

    [Header("Rest points")]
    [SerializeField] bool m_showRestPointGizmo;
    [SerializeField] Color m_restPointLeftGizmoColour = Color.blue;
    [SerializeField] Color m_restPointRightGizmoColour = Color.blue;
    [SerializeField] float m_restPointGizmoRadius = 0.1f;
    [SerializeField] Transform m_restPointLeft;
    [SerializeField] Transform m_restPointRight;

    [Header("Mouth position")]
    [SerializeField] bool m_showMouthPositionGizmo;
    [SerializeField] Color m_mouthGizmoColour = Color.yellow;
    [SerializeField] float m_mouthPositionGizmoRadius = 0.1f;
    [SerializeField] Transform m_mouthPosition;
    
    [Header("Food positions")]
    [SerializeField] bool m_showFoodPositionGizmo;
    [SerializeField] Color m_foodLeftGizmoColour = Color.green;
    [SerializeField] Color m_foodRightGizmoColour = Color.cyan;
    [SerializeField] float m_foodPositionGizmoRadius = 0.1f;
    [SerializeField] Transform[] m_foodPositionsLeft;
    [SerializeField] Transform[] m_foodPositionsRight;

    [Header("Feeding behaviour")]
    [SerializeField] Vector2 m_idleTimeMinMax = new Vector2(10f, 30f);
    [SerializeField] Vector2 m_feedingTimeMinMax = new Vector2(10f, 30f);
    [SerializeField] float m_settlingTime = 3f;
    [SerializeField] float m_secondClawDelay = 2f;

    private Animator m_anim;
    private int m_chewHash;
    private bool m_feeding = false;
    private Coroutine m_feedingCoroutineLeft;
    private Coroutine m_feedingCoroutineRight;


    void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_chewHash = Animator.StringToHash("Chew");
    }


    void Start()
    {
        SetTargetPosition(m_targetLeft, m_restPointLeft);
        SetTargetPosition(m_targetRight, m_restPointRight);
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


    private IEnumerator IdleFeedingTranstion()
    {
        while (true)
        {
            float idleTime = Random.Range(m_idleTimeMinMax.x, m_idleTimeMinMax.y);
            //print("Idle time: " + idleTime);

            yield return new WaitForSeconds(idleTime);

            m_feeding = true;
            SetIkSettings(m_feedingDistanceThreshold, m_feedingLearningRate);
            StartFeedingCoroutines();

            float feedingTime = Random.Range(m_feedingTimeMinMax.x, m_feedingTimeMinMax.y);
            //print("Feeding time: " + feedingTime);

            yield return new WaitForSeconds(feedingTime);

            m_feeding = false;
            SetIkSettings(m_idleDistanceThreshold, m_idleLearningRate);
            StopCoroutine(m_feedingCoroutineLeft);
            StopCoroutine(m_feedingCoroutineRight);

            SetTargetPosition(m_targetLeft, m_restPointLeft);
            SetTargetPosition(m_targetRight, m_restPointRight);

            yield return null;
        }
    }


    private void StartFeedingCoroutines()
    {
        if (Random.value > 0.5)
        {
            m_feedingCoroutineLeft = StartCoroutine(Feeding(m_targetLeft, 0f, m_foodPositionsLeft));
            m_feedingCoroutineRight = StartCoroutine(Feeding(m_targetRight, m_secondClawDelay, m_foodPositionsRight));
        }
        else
        {
            m_feedingCoroutineRight = StartCoroutine(Feeding(m_targetRight, 0f, m_foodPositionsRight));
            m_feedingCoroutineLeft = StartCoroutine(Feeding(m_targetLeft, m_secondClawDelay, m_foodPositionsLeft));
        }
    }


    private IEnumerator Feeding(Transform target, float delay, Transform[] foodPositions)
    {
        yield return new WaitForSeconds(delay);

        while (m_feeding)
        {
            int foodPositionIndex = Random.Range(0, foodPositions.Length);
            var foodPosition = foodPositions[foodPositionIndex];

            SetTargetPosition(target, foodPosition);

            yield return new WaitForSeconds(m_settlingTime);

            SetTargetPosition(target, m_mouthPosition);

            yield return new WaitForSeconds(m_settlingTime);
        }
    }


    private void SetTargetPosition(Transform target, Transform targetPosition)
    {
        if (target != null && targetPosition != null)
        {
            target.parent = targetPosition;
            target.localPosition = Vector3.zero;
        }
    }


    private void SetIkSettings(float distanceThreshold, float learningRate)
    {
        if (m_ikControllerLeft != null)
        {
            m_ikControllerLeft.DistanceThreshold = distanceThreshold;
            m_ikControllerLeft.LearningRate = learningRate;
        }

        if (m_ikControllerRight != null)
        {
            m_ikControllerRight.DistanceThreshold = distanceThreshold;
            m_ikControllerRight.LearningRate = learningRate;
        }
    }


    void OnDrawGizmos()
    {
        if (m_showTargetGizmo && m_targetLeft != null)
        {
            Gizmos.color = m_targetLeftGizmoColour;
            Gizmos.DrawSphere(m_targetLeft.position, m_targetGizmoRadius);
        }

        if (m_showTargetGizmo && m_targetRight != null)
        {
            Gizmos.color = m_targetRightGizmoColour;
            Gizmos.DrawSphere(m_targetRight.position, m_targetGizmoRadius);
        }

        if (m_showMouthPositionGizmo && m_mouthPosition != null)
        {
            Gizmos.color = m_mouthGizmoColour;
            Gizmos.DrawSphere(m_mouthPosition.position, m_mouthPositionGizmoRadius);
        }

        if (m_showFoodPositionGizmo)
        {
            if (m_foodPositionsLeft != null)
            {
                Gizmos.color = m_foodLeftGizmoColour;

                for (int i = 0; i < m_foodPositionsLeft.Length; i++)
                    Gizmos.DrawSphere(m_foodPositionsLeft[i].position, m_foodPositionGizmoRadius);
            }

            if (m_foodPositionsRight != null)
            {
                Gizmos.color = m_foodRightGizmoColour;

                for (int i = 0; i < m_foodPositionsRight.Length; i++)
                    Gizmos.DrawSphere(m_foodPositionsRight[i].position, m_foodPositionGizmoRadius);
            }
        }

        if (m_showRestPointGizmo && m_restPointLeft != null)
        {
            Gizmos.color = m_restPointLeftGizmoColour;
            Gizmos.DrawSphere(m_restPointLeft.position, m_restPointGizmoRadius);
        }

        if (m_showRestPointGizmo && m_restPointRight != null)
        {
            Gizmos.color = m_restPointRightGizmoColour;
            Gizmos.DrawSphere(m_restPointRight.position, m_restPointGizmoRadius);
        }
    }
}
