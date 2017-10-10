using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassIconSet : MonoBehaviour {

    public enum CompassImages
    {
        unknown,
        danger,
        fish,
        base_beacon,
        future_base_beacon,
        plane_wreck_beacon,
        cave_beacon,
        waypoint
    };

    public Image myIcon;
    public CompassImages myKind = CompassImages.unknown;
	// Use this for initialization
	void Start () {
        if (LayerMask.NameToLayer("sonarDetects") != gameObject.layer) {
            Debug.Log("Overwriting layer for " + name + " from " + gameObject.layer);
            gameObject.layer = LayerMask.NameToLayer("sonarDetects");
        }

        if (gameObject.tag != "compassTarget") {
            Debug.Log("Overwriting tag for  " + name + " from " + tag);
            gameObject.tag = "compassTarget";
        }
        GameObject tempGO = (GameObject)Instantiate(Resources.Load("compassIndicator"));
        myIcon = tempGO.GetComponent<Image>();
        myIcon.transform.SetParent(GameObject.Find("radarBackground").transform);
        myIcon.sprite = tempGO.GetComponent<CompassIconSwitcher>().GetImage((int)myKind);
	}
}
