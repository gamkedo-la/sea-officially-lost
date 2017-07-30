using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float pitchSpeed = 5.0f;
    public float rollSpeed = 5.0f;
    public float yawSpeed = 5.0f;
    public float swimSpeed = 10.0f;
    public float strafeSpeed = 5.0f;
    public float riseSpeed = 10.0f;

    public float scanRange = 500.0f;
    public ParticleSystem scannerBeam;
    public InventoryManager scannedItem;
    public Slider scanProgress;
    public InventoryManager playerShip;
    public float scanAmount = 0.0f;
    public Text oxygenLevel;
    public float oxygenLeft = 50.0f;

    public bool upLooksDown = false;

    public InventoryManager playerInventory;
    public int currentInventoryUsed;

	public Text inventoryDisplay;
    public float sonarRange = 40.0f;

    public Text insanityMeter;
    private float insanityCounter = 0.0f;
    private float insanityRange = 10.0f;

    private bool scanning = false;

    private Rigidbody rb;
    public float scanRate = 40.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float speed = 0.0f;
    private float speedDecay = 0.8f;
    private float speedLateral = 0.0f;
    private float speedRise = 0.0f;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        playerInventory.ScannedInventory();
        //playerShip.ScannedInventory();
        StartCoroutine(SonarPing());
        StartCoroutine(InsanityUpdate());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

	}

    void ReleaseMouse() {
        if (Cursor.visible == false) {
            Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
        } else {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Steering control
        /*transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime * (upLooksDown ? 1.0f : -1.0f));
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, Input.GetAxis("Roll") * rollSpeed * Time.deltaTime * -1.0f);*/
        yaw += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
        pitch += Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime * (upLooksDown ? 1.0f : -1.0f);
        Quaternion rotNow = Quaternion.identity;
        rotNow *= Quaternion.AngleAxis(yaw, Vector3.up);
        rotNow *= Quaternion.AngleAxis(pitch, Vector3.right);
        transform.rotation = rotNow;

        speed += Input.GetAxis("Vertical") * swimSpeed * Time.deltaTime;
		rb.AddForce(transform.forward * speed);

		speedLateral += Input.GetAxis("Horizontal") * strafeSpeed * Time.deltaTime;
		rb.AddForce(transform.right * speedLateral);

        if (Input.GetButton("Jump")) {
            speedRise += riseSpeed * Time.deltaTime;
        }

        rb.AddForce(Vector3.up * speedRise);

        if (Input.GetKeyDown(KeyCode.Minus)) {
            ReleaseMouse();
        }

        if (Input.GetKeyDown(KeyCode.Equals)) {
            oxygenLeft = 1000.0f;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene("Sealab");
        }

        oxygenLevel.text = "Oxygen left: " + oxygenLeft;
        oxygenLeft -= Time.deltaTime;

		// Scanner!
		RaycastHit rhInfo;
        bool inventoryInFrontOfMe = false;
        if (Physics.Raycast(transform.position, transform.forward, out rhInfo, scanRange))
		{
			Debug.Log("We hit: " + rhInfo.collider.name);
            scannedItem = rhInfo.collider.gameObject.GetComponent<InventoryManager>();
            inventoryInFrontOfMe = (scannedItem != null);

		}
		else
		{
            if (scannedItem != null) {
                scannedItem.ToggleInventoryPanel(false);    
            }
		}

		if (inventoryInFrontOfMe)
		{
			scannedItem.ToggleInventoryPanel(true);

            if (Input.GetButtonDown("Fire1") && scannedItem.InventoryKnown())
			{

				scannedItem.TransferInventoryInto(playerInventory);
			}
		}



		if (Input.GetButtonDown("Fire2"))
		{
			scannerBeam.Play();
            scanning = true;
		}
		if (Input.GetButtonUp("Fire2"))
		{
			
			//scannedItem = null;
            scannerBeam.Stop();

            scanning = false;
		}

        currentInventoryUsed = playerInventory.CountCurrentInventory();

        if (scanning) {

            scanAmount += Time.deltaTime * scanRate;
            if (scanAmount >= scanProgress.maxValue) {
                scanAmount = scanProgress.maxValue;
                if (inventoryInFrontOfMe) {
                    scannedItem.ScannedInventory();
					Debug.Log("We are scanning and progress hit max: " + scanProgress.value);
                }
            }

        } 
        scanProgress.value = scanAmount;
        insanityMeter.text = "Insanity: " + insanityCounter;
	}

    private void FixedUpdate()
    {
        if (scanning == false) {
            scanAmount *= 0.85f;
        }
        speed *= speedDecay;
        speedLateral *= speedDecay;
        speedRise *= speedDecay;
    }

    IEnumerator SonarPing() {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            Collider [] sonarHits = Physics.OverlapSphere(transform.position, sonarRange, LayerMask.GetMask("sonarDetects"));
            Debug.Log(sonarHits.Length);
            float nearestFoundDist = sonarRange + 1.0f;
            int nearestHitIndex = -1;
            for (int i = 0; i < sonarHits.Length; i++) {
                float thisDist = Vector3.Distance(transform.position, sonarHits[i].transform.position);
                if (thisDist < nearestFoundDist) {
                    nearestFoundDist = thisDist;
                    nearestHitIndex = i;
                }
            } // end for loop for sonarHits
            //if (nearestHitIndex != -1) {
            //    oxygenLevel.text = "" + (nearestFoundDist / sonarRange);
            //}
            //Debug.Log(sonarHits[nearestHitIndex].name);
        } // end while true
    }  // end SonarPing

    internal IEnumerator InsanityUpdate() {
        while(true) {
            yield return new WaitForSeconds(0.5f);
            Collider[] insanityHits = Physics.OverlapSphere(transform.position, insanityRange, LayerMask.GetMask("insanityDetects"));
            Debug.Log("Insanity hits = " + insanityHits.Length);
            float insanitySum = 0.0f;
            for (int i = 0; i < insanityHits.Length; i++) {
                InsanityFacts tempIF = insanityHits[i].GetComponent<InsanityFacts>();
                if (tempIF != null) {
                    Debug.Log(insanityHits[i].name + " had no facts!");
                }
                float thisDist = Vector3.Distance(transform.position, insanityHits[i].transform.position);
                float distPerc = 1.0f - thisDist / insanityRange;
                insanitySum += distPerc * tempIF.insanityImpact;
            } // end for loop for sanityHits
            Debug.Log("InsanitySum = " + insanitySum);
            insanityCounter = insanitySum;
        } // end while true
    } // end sanity update
}
