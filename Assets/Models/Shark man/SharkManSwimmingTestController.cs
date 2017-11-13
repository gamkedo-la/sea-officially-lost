using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float m_speed = 1.0f;
    private float rotationSPeed = 2.0f;

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

        if (Input.GetKeyDown(KeyCode.B))
        {
            var state = m_anim.GetCurrentAnimatorStateInfo(m_biteLayerIndex);

            //print(state.fullPathHash);

            if (state.fullPathHash != m_biteStateHash)
                m_anim.SetTrigger(m_biteTriggerHash);
        }

        if (Input.GetKey(KeyCode.X))
        {
            m_speed += Time.deltaTime * m_speedChangeRate;
            Debug.Log("m_speed is " + m_speed);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            m_speed -= Time.deltaTime * m_speedChangeRate;
        }

        //m_speed = Mathf.Clamp01(m_speed);

        m_anim.SetFloat("Speed", m_speed);

        Vector3 direction = PlayerCommon.instance.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                              Quaternion.LookRotation(direction),
                                              rotationSPeed * Time.deltaTime);

        transform.position += transform.forward * Time.deltaTime * 2.0f;
        Vector3 desiredPosition = transform.position;
        float terrainY = Terrain.activeTerrain.SampleHeight(transform.position);
        float tooCloseToGround = 2.0f;
        if (terrainY > desiredPosition.y + tooCloseToGround)
        {
            Debug.Log("I was under the terrain!");
            desiredPosition.y = Mathf.Max(desiredPosition.y, (terrainY + tooCloseToGround));
            transform.position = desiredPosition;
        }

        //Debug.Log("distance value " + direction.magnitude);
        if (direction.magnitude < 8.3f) {
            var state = m_anim.GetCurrentAnimatorStateInfo(m_biteLayerIndex);

            //print(state.fullPathHash);

            if (state.fullPathHash != m_biteStateHash)
                m_anim.SetTrigger(m_biteTriggerHash);
        }

        if (direction.magnitude < 7.0f) {
            Debug.Log("You died!");
            SceneManager.LoadScene("sealab v2");
        }
    }
}
