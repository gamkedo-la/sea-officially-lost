using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OxygenSystem : MonoBehaviour {
    [SerializeField]
    Image oxygenBar;
    [SerializeField]
    public float maxOxygenUnits = 30;

    float currentOxygenUnits;
    float oxygenUnit;
    private void Awake() {
        if (oxygenBar)
        {
            SetOxygenSystem();
        }
        else
        {
            Debug.Log("oxygen bar not defined for oxygen system");
        }
        
    }

    void SetOxygenSystem() {
        Debug.LogWarning("rectTransform.y " + oxygenBar.rectTransform.sizeDelta.y + " maxXyGenUnits " + maxOxygenUnits);
        oxygenUnit = oxygenBar.rectTransform.sizeDelta.y / maxOxygenUnits;

        Debug.LogWarning("Set Oxygen unit to " + oxygenUnit);

        currentOxygenUnits = oxygenBar.rectTransform.sizeDelta.y;

        Debug.LogWarning("Max Oxygen Units " + maxOxygenUnits);
    }

    public void SetOxygenUnits(int newMax) {
        maxOxygenUnits = newMax;
        StopCoroutine("OxygenDepletion");
        SetOxygenSystem();
        StartCoroutine(OxygenDepletion(oxygenUnit));
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
            Debug.LogWarning("currentOxygenUnits " + currentOxygenUnits);
            if (currentOxygenUnits <= 0){
                SceneManager.LoadScene("Modular Base Staging");
            }
            //Oxygen bar.
            oxygenBar.rectTransform.sizeDelta = new Vector2(oxygenBar.rectTransform.sizeDelta.x, currentOxygenUnits);
        }
    }
}