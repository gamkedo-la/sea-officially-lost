using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReaderManager : MonoBehaviour {

    public static ReaderManager instance;

    public GameObject readerCanvas;
    public Text readerText;
    public bool isOpen = false;
    public bool justClicked = true;


	// Use this for initialization
	void Start () {
        instance = this;
	}
	
    public void SetReaderText(string content) {
        readerText.text = content;
    }

    public void ToggleReader() {
        isOpen = !isOpen;
        readerCanvas.SetActive(isOpen);
    }
}
