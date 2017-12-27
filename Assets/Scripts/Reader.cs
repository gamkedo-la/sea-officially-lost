using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reader : MonoBehaviour {

    private Text localText;

    public void ClickAction()
    {
        localText = gameObject.GetComponentInChildren<Text>();
        ReaderManager.instance.SetReaderText(localText.text);
        ReaderManager.instance.ToggleReader();
    }
}
