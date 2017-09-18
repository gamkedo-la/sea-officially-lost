using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOn : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
        Debug.Log("Turning on fog via script");
        RenderSettings.fog = true;
    }
}
