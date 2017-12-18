using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour {

    public bool isExit = false;

    private bool m_open;

    public AirlockDoorController ADC;

    void Start() {
        ADC = transform.parent.GetComponentInParent<AirlockDoorController>();
        Debug.Log("ADC is not null: " + ADC != null);
        if (isExit == false) {
            OpenDoor();
        }
    }

    public void ClickAction()
    {
        Debug.Log("You clicked me!");
        if (isExit == false)
        {
            OpenDoor();
        }
    }

    public void OpenDoor() {
        m_open = !m_open;
        ADC.OpenDoor(m_open);
        AkSoundEngine.PostEvent("Play_Door", gameObject);
    }
}
