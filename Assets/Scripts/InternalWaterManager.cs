using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InternalWaterManager : MonoBehaviour {

    public Transform startPosition;
    public Transform endPosition;
    public float fillTime = 5.0f;
    public GameObject water;
    public GameObject closedDoor;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerController;
    public ParticleSystem waterEffect;

    private Quaternion playerStartLook;
    private float startTime = -1.0f;
    private float endTime = -1.0f;
    private bool camAboveWater = true;

	// Use this for initialization
	void Start () {
        water.transform.position = startPosition.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (startTime > 0) {
            float fillProgress = (Time.timeSinceLevelLoad - startTime) / (endTime - startTime);
            fillProgress = Mathf.Min(fillProgress, 1.0f);
            water.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, fillProgress);
            Camera.main.transform.rotation = Quaternion.Slerp(playerStartLook, Quaternion.LookRotation((closedDoor.transform.position - Camera.main.transform.position)), fillProgress * 2);
            if (camAboveWater) {
                if (water.transform.position.y > Camera.main.transform.position.y) {
                    camAboveWater = false;
                    RenderSettings.fog = true;
                    waterEffect.Stop();
                    Debug.Log("Water passed camera at " + fillProgress);
                }
            }
        }
	}

    public void ClickAction() {
        RaiseWater();

    }

    public void RaiseWater() {
        if (startTime < 0) {
			startTime = Time.timeSinceLevelLoad;
			endTime = startTime + fillTime;

            waterEffect.Play();
            playerController.enabled = false;
            playerStartLook = Camera.main.transform.rotation;
        }

    }
}
