using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float pitchSpeed = 5.0f;
    public float rollSpeed = 5.0f;
    public float yawSpeed = 5.0f;
    public float throttleSpeed = 10.0f;


    public float scanRange = 500.0f;
    public ParticleSystem scannerBeam;
    public InventoryManager scannedItem;
    public Slider scanProgress;
    public InventoryManager playerShip;
    public float scanAmount = 0.0f;

    public bool upLooksDown = false;

    public InventoryManager playerInventory;
    public int currentInventoryUsed;

	public Text inventoryDisplay;

    private bool scanning = false;

    private Rigidbody rb;
    public float scanRate = 40.0f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        playerInventory.ScannedInventory();
        playerShip.ScannedInventory();

	}
	
	// Update is called once per frame
	void Update () {
        // Steering control
        transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime * (upLooksDown ? 1.0f : -1.0f));
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, Input.GetAxis("Roll") * rollSpeed * Time.deltaTime * -1.0f);

        rb.AddForce(transform.forward * Input.GetAxis("Throttle") * throttleSpeed * Time.deltaTime);

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


	}

    private void FixedUpdate()
    {
        if (scanning == false) {
            scanAmount *= 0.85f;
        }
    }
}
