﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;
    [SerializeField] private UnityStandardAssets.Characters.FirstPerson.MouseLook m_MouseLook;
    public float pitchSpeed = 5.0f; //Might do nothing
    public float rollSpeed = 5.0f; //Might do nothing
    public float yawSpeed = 5.0f; //Might do nothing
    public float swimSpeed = 10.0f;
    public float strafeSpeed = 5.0f;
    public float riseSpeed = 10.0f;


    public float scanRange = 500.0f;
    public float scanAmount = 0.0f;
    public Text oxygenLevel;

    public bool upLooksDown = false;

    public InventoryManager playerInventory;
    public int currentInventoryUsed;

	public Text inventoryDisplay;


    public Text insanityMeter;
    private float insanityCounter = 0.0f;
    private float insanityRange = 10.0f;
    public int knowledgeLevel = 0;

    private bool scanning = false;

    private Rigidbody rb;
    public float scanRate = 40.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float speed = 0.0f;
    private float speedDecay = 0.8f;
    private float speedLateral = 0.0f;
    private float speedRise = 0.0f;

    //inventory related declarations pick-up item variables
    float m_MaxInteractDistance = 2.0f;
    GameObject pickedUpItem;
	public GameObject inventoryMgr;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        m_MouseLook.Init(transform, Camera.main.transform);
        playerInventory.ScannedInventory();
        //playerShip.ScannedInventory();

        StartCoroutine(InsanityUpdate());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        instance = this;

        m_MouseLook.XSensitivity = 1 + PlayerPrefs.GetInt("LookSensitivityX", 20)/20;
        m_MouseLook.YSensitivity = 1 + PlayerPrefs.GetInt("LookSensitivityY", 20)/20;
        m_MouseLook.invertedX = PlayerPrefs.GetInt("LookInveredX", 0) > 0;
        m_MouseLook.invertedY = PlayerPrefs.GetInt("LookInveredY", 0) > 0;


    }

    public void ReleaseMouse() {
        if (Cursor.visible == false) {
            Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
            Debug.Log("Cursor visible " + Cursor.visible);
        } else {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Steering control
        /*transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime * (upLooksDown ? 1.0f : -1.0f));
        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, Input.GetAxis("Roll") * rollSpeed * Time.deltaTime * -1.0f);*/
        m_MouseLook.LookRotation(transform, Camera.main.transform);
//        yaw += Input.GetAxis("Mouse X") * m_MouseLook. * Time.deltaTime;
//        pitch += Input.GetAxis("Mouse Y") * PlayerCommon.mouseSensitivity * Time.deltaTime * (PlayerCommon.inverted ? -1.0f : 1.0f);
//        pitch = Mathf.Clamp(pitch, -40.0f, 40.0f);
//        Quaternion rotNow = Quaternion.identity;
//        rotNow *= Quaternion.AngleAxis(yaw, Vector3.up);
//        rotNow *= Quaternion.AngleAxis(pitch, Vector3.right);
//        transform.rotation = rotNow;

        speed += Input.GetAxis("Vertical") * swimSpeed * Time.deltaTime;
        rb.AddForce(Camera.main.transform.forward * speed);

        speedLateral += Input.GetAxis("Horizontal") * strafeSpeed * Time.deltaTime;
        rb.AddForce(transform.right * speedLateral);

        if (Input.GetButton("Jump"))
        {
            speedRise += riseSpeed * Time.deltaTime;
        }

        rb.AddForce(Vector3.up * speedRise);

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.Tab))
        {
            ReleaseMouse();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("sealab v2");
        }

        //pick-up item testing for inventory
        //Debug.DrawRay(transform.position, Vector3.forward * 10, Color.red);
        if (Input.GetButtonDown("PickUpItem"))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, m_MaxInteractDistance))
            {
                //Debug.Log("I can see " + hit.transform.gameObject.name);
                if (hit.transform.gameObject.CompareTag("canPickUp"))
                {
                    pickedUpItem = hit.transform.gameObject;
                    Debug.Log("Picked up " + hit.transform.gameObject.name);
                    inventoryMgr.GetComponent<InventoryMgr>().GetItem(pickedUpItem);
                }
            }
        }

    }

    private void FixedUpdate()
    {
        if (scanning == false) {
            scanAmount *= 0.85f;
        }
        speed *= speedDecay;
        speedLateral *= speedDecay;
        speedRise *= speedDecay;
        m_MouseLook.UpdateCursorLock();
    }


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
