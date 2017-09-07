using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenSystem : MonoBehaviour {
    [SerializeField]
    Image oxygenBar;
    [SerializeField]
    float maxOxygenUnits = 30;

    float currentOxygenUnits;
    float oxygenUnit;
    private void Awake() {
        if (oxygenBar)
        {
            oxygenUnit = oxygenBar.rectTransform.sizeDelta.y / maxOxygenUnits;
            currentOxygenUnits = oxygenBar.rectTransform.sizeDelta.y;
         
        }
        else
        {
            Debug.Log("oxygen bar not defined for oxygen system");
        }
        
    }

    //Starts depleting oxygen at one unit person second.
    private void Start() {
        StartCoroutine(OxygenDepletion(oxygenUnit));
    }

    //How many units will be depleted per second. Ex: "oxygenUnit" will result in 1 unit per second.
    IEnumerator OxygenDepletion(float unitsPerSecond) {
        while (currentOxygenUnits > 0) {
            yield return new WaitForSeconds(0.1f);
            currentOxygenUnits -= unitsPerSecond / 10;

            //Oxygen bar.
            oxygenBar.rectTransform.sizeDelta = new Vector2(oxygenBar.rectTransform.sizeDelta.x, currentOxygenUnits);
        }
    }
}