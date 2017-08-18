using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightBehavoiur : MonoBehaviour {
    Transform lightOfFlashlight;
    Camera firstPersonCam;
    private void Awake() {
        //Child zero should be all the game objects related to the light of the flashlight.
        lightOfFlashlight = transform.GetChild(0);
        firstPersonCam = Camera.main;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            SetFlashlightPower(!lightOfFlashlight.gameObject.activeSelf);
        }

        Movement();
    }

    void Movement() {
        transform.rotation = firstPersonCam.transform.rotation;
    }

    void SetFlashlightPower (bool on) {
        if (on == true) {
            lightOfFlashlight.gameObject.SetActive(true);
        }
        else {
            lightOfFlashlight.gameObject.SetActive(false);
        }

    }
}
