using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePaneWiggler : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.localRotation = Quaternion.Euler(0.4f * Mathf.Cos(Time.time * 0.5f), 0.65f * Mathf.Cos(Time.time * 0.85f), 0.0f);
	}
}
