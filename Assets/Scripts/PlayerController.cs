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

    public bool upLooksDown = false;

    public InventoryManager playerInventory;
    public int currentInventoryUsed;

	public Text inventoryDisplay;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
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
        if (Physics.Raycast(transform.position, transform.forward, out rhInfo, scanRange)) {
            Debug.Log("We hit: " + rhInfo.collider.name);
            InventoryManager tempIM = rhInfo.collider.gameObject.GetComponent<InventoryManager>();
            if (tempIM != null){
                
                if (Input.GetButtonDown("Fire1")) {
                    tempIM.TransferInventoryInto(playerInventory);
                }
				if (Input.GetButtonDown("Fire2")) {
					tempIM.ToggleInventoryPanel(true);
                    scannerBeam.Play();
				}
                if (Input.GetButtonUp("Fire2")) {
                    tempIM.ToggleInventoryPanel(false);
                    scannerBeam.Stop();
                }
            }
        }


        currentInventoryUsed = playerInventory.CountCurrentInventory();
	}
}
