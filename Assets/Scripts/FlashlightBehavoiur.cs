using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightBehavoiur : MonoBehaviour {
    Transform lightOfFlashlight;
    Camera firstPersonCam;
    public Text barText;

    public float batteryLifetime = 30.0f;
    public float batterRechargeTime = 5.0f;

    private int maxPowerBars = 6;
    private float currentPower = 1.0f;
    private bool powerOn = true;
    private bool switchOn = true;

    private void Awake() {
        //Child zero should be all the game objects related to the light of the flashlight.
        lightOfFlashlight = transform.GetChild(0);
        firstPersonCam = Camera.main;
        StartCoroutine(FlashlightFlicker());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            switchOn = !switchOn;
            SetFlashlightPower(switchOn);
        }

        /*int barsToShowNow = Mathf.FloorToInt(maxPowerBars * currentPower);
        if (barsToShowNow != barText.text.Length) {
            string newBar = "";
            for (int i = 0; i < barsToShowNow; i++) {
                newBar += "|";
            }
            barText.text = newBar;
        }

        if (powerOn) {
            currentPower -= Time.deltaTime;
            if (currentPower < 0.0f) {
                currentPower = 0.0f;
            }
        } else {
            currentPower += Time.deltaTime;
            if (currentPower > 1.0f) {
                currentPower = 1.0f;
            }
        }*/

        Movement();
    }

    IEnumerator FlashlightFlicker () {
        while (true) {
            yield return new WaitForSeconds(Random.Range(5*60, 5*60 + 45));
			int flickerCount = Random.Range(2, 4);
			for (int i = 0; i < flickerCount; i++)
			{
				SetFlashlightPower(false);
				yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
                SetFlashlightPower(switchOn);
				yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
			}
            flickerCount = Random.Range(4, 7);
			for (int i = 0; i < flickerCount; i++)
			{
				SetFlashlightPower(false);
				yield return new WaitForSeconds(0.5f * Random.Range(0.09f, 0.2f));
				SetFlashlightPower(switchOn);
				yield return new WaitForSeconds(0.5f * Random.Range(0.2f, 0.4f));
			}
            SetFlashlightPower(switchOn);
        }
    }

    void Movement() {
        transform.rotation = firstPersonCam.transform.rotation;
    }

    void SetFlashlightPower (bool on) {
        powerOn = on;
        lightOfFlashlight.gameObject.SetActive(powerOn);
        if (powerOn) {
            barText.text = "||||||";
        } else {
            barText.text = "";
        }
    }
}
