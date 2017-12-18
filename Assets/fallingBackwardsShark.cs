using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingBackwardsShark : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.forward * Time.deltaTime;
		transform.Rotate(Vector3.forward, Time.deltaTime * 3.4f);
	}
}
