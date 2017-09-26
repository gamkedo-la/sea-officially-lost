using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
}
