using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GoblinSharkController : MonoBehaviour
{
    private Animator m_anim;
    static int m_biteState = Animator.StringToHash("Bite Layer.Bite");

    void Awake()
    {
        m_anim = GetComponent<Animator>();
        //print(m_biteState);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var state = m_anim.GetCurrentAnimatorStateInfo(0);

            //print(state.fullPathHash);

            if (state.fullPathHash != m_biteState)
                m_anim.SetTrigger("Bite");
        }
    }
}
