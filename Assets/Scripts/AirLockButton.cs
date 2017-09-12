using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AirLockButton : MonoBehaviour {
    
	
	public void ClickAction () {
		SceneManager.LoadScene("main");
	}
}
