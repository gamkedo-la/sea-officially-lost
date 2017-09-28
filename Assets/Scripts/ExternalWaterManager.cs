using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalWaterManager : MonoBehaviour
{

    public Transform startPosition;
    public Transform endPosition;
    public float fillTime = 5.0f;
    public GameObject water;
    public GameObject closedDoor;
    public PlayerController playerController;

    private Quaternion playerStartLook;
    private float startTime = -1.0f;
    private float endTime = -1.0f;
    private bool camAboveWater = false;

    // Use this for initialization
    void Start()
    {
        water.transform.position = startPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime > 0)
        {
            float fillProgress = (Time.timeSinceLevelLoad - startTime) / (endTime - startTime);
            fillProgress = Mathf.Min(fillProgress, 1.0f);
            water.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, fillProgress);
            Camera.main.transform.rotation = Quaternion.Slerp(playerStartLook, Quaternion.LookRotation((closedDoor.transform.position - Camera.main.transform.position)), fillProgress * 2);
            if (camAboveWater)
            {
                if (water.transform.position.y < Camera.main.transform.position.y)
                {
                    camAboveWater = true;
                    RenderSettings.fog = false;

                    Debug.Log("Water passed camera at " + fillProgress);
                }
            }
        }
    }

    public void ClickAction()
    {
        RaiseWater();

    }

    public void RaiseWater()
    {
        if (startTime < 0)
        {
            water.SetActive(true);
            startTime = Time.timeSinceLevelLoad;
            endTime = startTime + fillTime;

            playerController.enabled = false;
            playerStartLook = Camera.main.transform.rotation;
        }

    }
}
