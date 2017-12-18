using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateEndGame : MonoBehaviour {

    public GameObject portalObject;

    public void ClickAction()
    {
        portalObject.SetActive(true);
    }
}
