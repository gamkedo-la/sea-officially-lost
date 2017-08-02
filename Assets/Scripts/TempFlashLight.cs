using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempFlashLight : MonoBehaviour {

    public Light flashlight;
    public Light cookie;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (flashlight.enabled) {
                flashlight.enabled = false;
                cookie.enabled = false;
            } else {
                flashlight.enabled = true;
                cookie.enabled = true;
            }
        }
	}
}
