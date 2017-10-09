using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassIconSwitcher : MonoBehaviour {

    public Sprite[] compassImageSprites;

    // Use this for initialization
    public Sprite GetImage(int image) {
        return compassImageSprites[image];
    }
}
