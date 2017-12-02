using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpiderCrabWithIKController : MonoBehaviour
{
    private enum TargetType 
    {
        Idle,
        Food,
        Mouth,
        Attack,
    }


    [Header("Animation test")]
    [SerializeField] bool m_allowAnimationTestKeys;

    [Header("IK settings")]
    [SerializeField] InverseKinematicsController m_ikControllerLeft;
    [SerializeField] InverseKinematicsController m_ikControllerRight;
    [SerializeField] float m_idleDistanceThreshold = 0f;
    [SerializeField] float m_idleLearningRate = 2f;
    [SerializeField] float m_feedingDistanceThreshold = 0.1f;
    [SerializeField] float m_feedingLearningRate = 10f;
    [SerializeField] float m_attackLearningRate = 20f;
    [SerializeField] float m_attackDistanceThreshold = 0f;

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
    [SerializeField] float m_feedingTargetDistanceToTriggerPincer = 0.2f;

    [Header("Attack behaviour")]
    [SerializeField] float m_attackTargetDistanceToTriggerPincer = 0.5f;


    private Animator m_anim;
    private int m_chewHash;
    private int m_leftPincerClosedHash;
    private int m_rightPincerClosedHash;
    private bool m_feeding = false;
    private bool m_leftPincerClosed;
    private bool m_rightPincerClosed;
    private Coroutine m_feedingCoroutineLeft;
    private Coroutine m_feedingCoroutineRight;
    private TargetType m_leftClawTargetType;
    private TargetType m_rightClawTargetType;


    void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_chewHash = Animator.StringToHash("Chew");
        m_leftPincerClosedHash = Animator.StringToHash("Left pincer closed");
        m_rightPincerClosedHash = Animator.StringToHash("Right pincer closed");
    }


    void Start()
    {
        SetDefaultState();
    }


    private void SetDefaultState()
    {
        m_leftClawTargetType = TargetType.Idle;
        m_rightClawTargetType = TargetType.Idle;
        SetTargetPosition(m_targetLeft, m_restPointLeft);
        SetTargetPosition(m_targetRight, m_restPointRight);
        SetIkSettings(m_idleDistanceThreshold, m_idleLearningRate);
        StartCoroutine(IdleFeedingTranstion());
    }


    void Update()
    {
        CheckLeftPincerState();
        CheckRightPincerState();

        if (!m_allowAnimationTestKeys)
            return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            m_leftPincerClosed = !m_leftPincerClosed;
            LeftPincer(m_leftPincerClosed);
            print("Left pincer closed: " + m_leftPincerClosed);
        } 

        if (Input.GetKeyDown(KeyCode.R))
        {
            m_rightPincerClosed = !m_rightPincerClosed;
            RightPincer(m_rightPincerClosed);
            print("Right pincer closed: " + m_leftPincerClosed);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Chew();
            print("Chew");
        }
    }


    private void CheckLeftPincerState()
    {
        float distanceToTarget = m_ikControllerLeft.DistanceFromTarget;

        switch (m_leftClawTargetType)
        {
            case TargetType.Idle:
                break;

            case TargetType.Food:
                LeftPincer(distanceToTarget < m_feedingTargetDistanceToTriggerPincer);
                break;

            case TargetType.Mouth:
                LeftPincer(distanceToTarget > m_feedingTargetDistanceToTriggerPincer);
                break;

            case TargetType.Attack:
                LeftPincer(distanceToTarget < m_attackTargetDistanceToTriggerPincer);
                break;
        }
    }


    private void CheckRightPincerState()
    {
        float distanceToTarget = m_ikControllerRight.DistanceFromTarget;

        switch (m_rightClawTargetType)
        {
            case TargetType.Idle:
                break;

            case TargetType.Food:
                RightPincer(distanceToTarget < m_feedingTargetDistanceToTriggerPincer);
                break;

            case TargetType.Mouth:
                RightPincer(distanceToTarget > m_feedingTargetDistanceToTriggerPincer);
                break;

            case TargetType.Attack:
                RightPincer(distanceToTarget < m_attackTargetDistanceToTriggerPincer);
                break;
        }
    }


    public void Chew()
    {
        m_anim.SetTrigger(m_chewHash);
    }


    private void LeftPincer(bool closed)
    {
        m_anim.SetBool(m_leftPincerClosedHash, closed);
    } 


    private void RightPincer(bool closed)
    {
        m_anim.SetBool(m_rightPincerClosedHash, closed);
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

            m_leftClawTargetType = TargetType.Idle;
            m_rightClawTargetType = TargetType.Idle;

            LeftPincer(false);
            RightPincer(false);
            
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

            // Bleurgh, this is horrible
            if (target == m_targetLeft)
                m_leftClawTargetType = TargetType.Food;
            else
                m_rightClawTargetType = TargetType.Food;

            yield return new WaitForSeconds(m_settlingTime);

            SetTargetPosition(target, m_mouthPosition);

            if (target == m_targetLeft)
                m_leftClawTargetType = TargetType.Mouth;
            else
                m_rightClawTargetType = TargetType.Mouth;

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
        SetIkSettings(m_ikControllerLeft, distanceThreshold, learningRate);
        SetIkSettings(m_ikControllerRight, distanceThreshold, learningRate);
    }


    private void SetIkSettings(InverseKinematicsController ikController, float distanceThreshold, float learningRate)
    {
        if (ikController != null)
        {
            ikController.DistanceThreshold = distanceThreshold;
            ikController.LearningRate = learningRate;
        }
    }


    public void AttackTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        m_leftClawTargetType = TargetType.Attack;
        SetTargetPosition(m_targetLeft, other.transform);
        SetIkSettings(m_ikControllerLeft, m_attackDistanceThreshold, m_attackLearningRate);
    }


    public void AttackTriggerExit(Collider other)
    {
        SetDefaultState();
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
