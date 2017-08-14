using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inv_Colour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Image backgroundImage = GameObject.Find("PlayerInvBgImage").GetComponent<Image>();
        backgroundImage.color = UnityEngine.Color.blue;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
