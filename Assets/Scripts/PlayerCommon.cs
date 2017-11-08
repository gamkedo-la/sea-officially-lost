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

    //inventory related declarations pick-up item variables
    float m_MaxInteractDistance = 2.0f;
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

        StartCoroutine(InsanityUpdate());
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit rhInfo;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out rhInfo, 5.0f, ~LayerMask.GetMask("Ignore Raycast")))
			{
				Debug.Log("Raycast hit " + rhInfo.collider.gameObject.name);
				rhInfo.collider.gameObject.SendMessage("ClickAction", SendMessageOptions.DontRequireReceiver);
			}
		}

		if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Tab))
		{
			ReleaseMouse();
		}

        //pick up item for attributes inventory
        if (Input.GetButtonDown("PickUpItem"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, m_MaxInteractDistance))
            {
                Debug.Log("I can see " + hit.transform.gameObject.name);
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

            grainSettings.intensity = insanityCounter;
            vignetteSettings.intensity = insanityCounter;
            ppProfile.grain.settings = grainSettings;
            ppProfile.vignette.settings = vignetteSettings;

        } // end while true
    } // end sanity update
}
