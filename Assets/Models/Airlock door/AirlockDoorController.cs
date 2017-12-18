using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AirlockDoorController : MonoBehaviour
{
    private Animator m_anim;
    private int m_openBoolHash;

	void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_openBoolHash = Animator.StringToHash("Open");
    }

    void Start() {
        OpenDoor(true);
    }

    public void OpenDoor(bool open)
    {
        Debug.Log("Open door got called");
        m_anim.SetBool(m_openBoolHash, open);
    }
}
