using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpiderCrabController : MonoBehaviour
{
    [SerializeField] Vector2 m_idleTimeMinMax = new Vector2(10f, 30f);
    [SerializeField] Vector2 m_feedingTimeMinMax = new Vector2(10f, 30f);
    

    private Animator m_anim;
    private bool m_feeding;

    private int m_feedingHash;
    private int m_chewHash;


    void Awake()
    {
        m_anim = GetComponent<Animator>();
        string baseLayerName = m_anim.GetLayerName(0);
        m_feedingHash = Animator.StringToHash("Feeding");
        m_chewHash = Animator.StringToHash("Chew"); 
    }


    void Start()
    {
        StartCoroutine(IdelFeedingTranstion());
    }


    private IEnumerator IdelFeedingTranstion()
    {
        m_anim.SetBool(m_feedingHash, false);

        while (true)
        {
            float idleTime = Random.Range(m_idleTimeMinMax.x, m_idleTimeMinMax.y);
            print("Idle time: " + idleTime);

            yield return new WaitForSeconds(idleTime);

            m_anim.SetBool(m_feedingHash, true);

            float feedingTime = Random.Range(m_feedingTimeMinMax.x, m_feedingTimeMinMax.y);
            print("Feeding time: " + feedingTime);

            yield return new WaitForSeconds(feedingTime);

            m_anim.SetBool(m_feedingHash, false);

            yield return null;
        }
    }


    public void Chew()
    {
        m_anim.SetTrigger(m_chewHash);
    }


    //void OnAnimatorIK(int layerIndex)
    //{
    //    print("OnAnimatorIK called on layer " + layerIndex);
    //}
}
