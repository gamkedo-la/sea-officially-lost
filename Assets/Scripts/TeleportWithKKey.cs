using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWithKKey : MonoBehaviour {

    void Start() {
        Debug.Log("Press K to teleport " + name);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K)) {
            
            PlayerCommon.instance.transform.position = transform.position; 
        }
	}
}
