using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// quick little text scroller for a credits screen by mcfunkypants
public class TextScroller : MonoBehaviour {

	public float scrollSpeedPerSecond = 50.0f;
	public string LevelToloadWhenDone = "MainMenu";
	private float currentY = 0.0f;

	// Use this for initialization
	void Start () {
		currentY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		currentY += Time.deltaTime * scrollSpeedPerSecond;
		transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

		if (Input.GetKey(KeyCode.Escape) ||
			Input.GetKey(KeyCode.Return) ||
			Input.GetKey(KeyCode.Space) ||
			currentY > 2400) // FIXME - hardcoded: measure the size of the text?
		{
			Debug.Log("Credits are done. Loading: " + LevelToloadWhenDone);
			SceneManager.LoadScene(LevelToloadWhenDone);
		}
	}
}
