using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class PlayerCommon : MonoBehaviour {

    public static PlayerCommon instance;

    PostProcessingProfile ppProfile;

    public Text insanityMeter;
    private float insanityCounter = 0.0f;
    private float insanityRange = 10.0f;
    private float insanityTimeAboveThreshold = 0.0f;
    private float insanityThreshold = 0.9f;
    public float insanityTimeBeforeSharkSummon = 10.0f;
    public GameObject sharkMan;
    private GameObject summonedShark;
    public int knowledgeLevel = 5;

    //inventory related declarations pick-up item variables
    public float m_MaxInteractDistance = 2.0f;
    GameObject pickedUpItem;
    public GameObject inventoryMgr;
   
    void Awake() {
        instance = this;
    }
    
	// Use this for initialization
	void Start () {
        ppProfile = Instantiate(Camera.main.GetComponent<PostProcessingBehaviour>().profile);
        Camera.main.GetComponent<PostProcessingBehaviour>().profile = ppProfile;
        ColorGradingModel.Settings colorGrading = ppProfile.colorGrading.settings;
        colorGrading.basic.postExposure = (PlayerPrefs.GetInt("Brightness", 50) - 50) / 50;
        ppProfile.colorGrading.settings = colorGrading;

        Camera.main.fieldOfView = PlayerPrefs.GetInt("FieldOfView", 60);
        Debug.Log("Player common started!");
        StartCoroutine(InsanityUpdate());
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Action"))
		{
			RaycastHit rhInfo;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rhInfo, 5.0f, ~LayerMask.GetMask("Ignore Raycast")))
			{
				Debug.Log("Raycast hit " + rhInfo.collider.gameObject.name);
				rhInfo.collider.gameObject.SendMessage("ClickAction", SendMessageOptions.DontRequireReceiver);
			}
		}

		if (Input.GetKeyDown(KeyCode.Minus))
		{
			ReleaseMouse();
		}

        /* if(Input.GetKeyDown(KeyCode.F)) {
            insanityCounter = 5.0f;
        } */

        //pick up item for attributes inventory
        if (Input.GetButtonDown("Action"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, m_MaxInteractDistance))
            {
                Debug.Log("I can see " + hit.transform.gameObject.name + "and it has tag " + hit.transform.gameObject.tag);
                if (hit.transform.gameObject.CompareTag("canPickUp"))
                {
                    pickedUpItem = hit.transform.gameObject;
                    Debug.Log("Picked up " + hit.transform.gameObject.name);
                    inventoryMgr.GetComponent<InventoryMgr>().GetItem(pickedUpItem);
                }
            }
        }
    }

    public void ReleaseMouse()
	{
		if (Cursor.visible == false)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			Debug.Log("Cursor visible " + Cursor.visible);
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

    internal IEnumerator InsanityUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            GrainModel.Settings grainSettings = ppProfile.grain.settings;
            VignetteModel.Settings vignetteSettings = ppProfile.vignette.settings;

            Debug.Log("Intensity is: " + grainSettings.intensity);
            Collider[] insanityHits = Physics.OverlapSphere(transform.position, insanityRange, LayerMask.GetMask("insanityDetects"));
            //Debug.Log("Insanity hits = " + insanityHits.Length);
            float insanitySum = 0.0f;
            for (int i = 0; i < insanityHits.Length; i++)
            {
                InsanityFacts tempIF = insanityHits[i].GetComponent<InsanityFacts>();
                if (tempIF != null)
                {
                    Debug.Log(insanityHits[i].name + " had no facts!");
                }
                float thisDist = Vector3.Distance(transform.position, insanityHits[i].transform.position);
                float distPerc = 1.0f - thisDist / insanityRange;
                insanitySum += distPerc * tempIF.insanityImpact;
            } // end for loop for sanityHits
            //Debug.Log("InsanitySum = " + insanitySum);

            float insanityFallOffPerc = 0.1f;
            insanityCounter = insanitySum * insanityFallOffPerc + insanityCounter * (1.0f - insanityFallOffPerc);
            if (insanityCounter > insanityThreshold) {
                insanityTimeAboveThreshold += Time.deltaTime;
                //Debug.Log("Time above threshold: " + insanityTimeAboveThreshold);
                if (insanityTimeAboveThreshold >= insanityTimeBeforeSharkSummon) {
                    if (summonedShark == null) {
                        Vector3 desiredPosition = transform.position + transform.forward * 25.0f;
                        float terrainY = Terrain.activeTerrain.SampleHeight(transform.position);
                        float tooCloseToGround = 2.0f;
                        if (terrainY > desiredPosition.y + tooCloseToGround)
                        {
                            Debug.Log("I was under the terrain!");
                            desiredPosition.y = Mathf.Max(desiredPosition.y, (terrainY + tooCloseToGround));
                        }
                        summonedShark = GameObject.Instantiate(sharkMan, desiredPosition, Quaternion.LookRotation(-transform.forward));    
                        Debug.Log("Shark got summoned!");
                    } else {
                        Debug.Log("Shark already present, you really don't want more than one.");
                    }

                }
            } else {
                Debug.Log("Resetting threshold to 0");
                insanityTimeAboveThreshold = 0.0f;
            }
            grainSettings.intensity = insanityCounter;
            vignetteSettings.intensity = insanityCounter;
            ppProfile.grain.settings = grainSettings;
            ppProfile.vignette.settings = vignetteSettings;

        } // end while true
    } // end sanity update
}
