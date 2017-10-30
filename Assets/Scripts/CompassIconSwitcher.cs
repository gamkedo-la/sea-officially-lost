using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassIconSwitcher : MonoBehaviour {

    public Sprite[] compassImageSprites;

    // This is automatically added to the compassIndicator object in the resources directory
    // Use this for initialization
    public Sprite GetImage(int image) {
        return compassImageSprites[image];
    }
}
