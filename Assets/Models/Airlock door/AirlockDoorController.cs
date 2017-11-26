using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AirlockDoorController : MonoBehaviour
{
    private Animator m_anim;
    private int m_openBoolHash;

    private bool m_open;


	void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_openBoolHash = Animator.StringToHash("Open");
    }

	
	void Update ()
    { 
	    if (Input.GetKeyDown(KeyCode.Space))
        {
            m_open = !m_open;

            OpenDoor(m_open);
        }	
	}


    public void OpenDoor(bool open)
    {
        m_anim.SetBool(m_openBoolHash, open);
    }
}
