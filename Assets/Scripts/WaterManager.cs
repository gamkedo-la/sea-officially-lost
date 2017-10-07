using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WaterManager : MonoBehaviour {

    public Transform upperPosition;
    public Transform lowerPosition;
    public float fillTime = 5.0f;
    public GameObject waterBase;
    public GameObject closedDoor;
    public ParticleSystem waterEffect;
    public Transform playerSpawn;
    public bool startsWet;
    public string sceneToLoad;

    private Quaternion playerStartLook;
    private Vector3 playerStartPosition;
    private Transform startPosition;
    private Transform endPosition;
    private float startTime = -1.0f;
    private float endTime = -1.0f;
    private bool camAboveWater = true;
    private float waterCameraAdjust = 1.218f;
    private GameObject water;

	// Use this for initialization
	void Start () {
        water = Instantiate(waterBase, waterBase.transform.position, Quaternion.identity);
        PlayerCommon.instance.transform.position = FudgedPlayerSpawnPosition();
        if (startsWet) {
            startPosition = upperPosition;
            endPosition = lowerPosition;
        } else {
			startPosition = lowerPosition;
			endPosition = upperPosition;
        }
        water.transform.position = startPosition.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (startTime > 0) {
            float fillProgress = (Time.timeSinceLevelLoad - startTime) / (endTime - startTime);
            fillProgress = Mathf.Min(fillProgress, 1.0f);
            water.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, fillProgress);
            Camera.main.transform.rotation = Quaternion.Slerp(playerStartLook, playerSpawn.rotation, fillProgress * 2);
            float positionChangeMultiplier;
            if (startsWet) {
                positionChangeMultiplier = 2.5f;
            } else {
                positionChangeMultiplier = 6.0f;
            }
            PlayerCommon.instance.transform.position = Vector3.Lerp(playerStartPosition, FudgedPlayerSpawnPosition(), Mathf.Min(fillProgress * positionChangeMultiplier, 1.0f));
            if (camAboveWater) {
                if (startsWet) {
					if (water.transform.position.y < Camera.main.transform.position.y)
					{
						camAboveWater = true;
						RenderSettings.fog = false;
						Debug.Log("Water passed camera at " + fillProgress);

					}
				}
				else
				{
					if (water.transform.position.y > Camera.main.transform.position.y)
					{
						camAboveWater = false;
						RenderSettings.fog = true;
						
						waterEffect.Stop();

						Debug.Log("Water passed camera at " + fillProgress);
					}
				}

            }
            if (fillProgress >= 1.0f) {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
	}

    Vector3 FudgedPlayerSpawnPosition() {
        float heightFudgeFactor;
		if (startsWet)
		{
			heightFudgeFactor = 1.218f;	
		}
		else
		{
			heightFudgeFactor = (-1.229f) - (-0.81641f);
		}
        return playerSpawn.transform.position + Vector3.up * heightFudgeFactor;

    }

    public void ClickAction() {
        RaiseWater();

    }

    public void RaiseWater() {
        if (startTime < 0) {
			startTime = Time.timeSinceLevelLoad;
			endTime = startTime + fillTime;

            water.SetActive(true);

            if (startsWet == false) {
                waterEffect.Play();    
            }

            UnityStandardAssets.Characters.FirstPerson.FirstPersonController underwaterPlayerController = PlayerCommon.instance.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
	        PlayerController inbasePlayerController = PlayerCommon.instance.GetComponent<PlayerController>();
            if (underwaterPlayerController != null){
                underwaterPlayerController.enabled = false;
            }
            if (inbasePlayerController != null) {
                inbasePlayerController.enabled = false;
            }
            playerStartLook = Camera.main.transform.rotation;
            playerStartPosition = PlayerCommon.instance.transform.position;
        }

    }
}
