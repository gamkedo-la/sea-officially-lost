using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoosterItems : MonoBehaviour {

    public GameObject oxygenBooster;
    public GameObject swimSpeedBooster;

    void Start() {
        InventoryMgr tempIM = GameObject.FindWithTag("InventoryManager").GetComponent<InventoryMgr>();

        if (tempIM != null) {
            if (tempIM.HasOxygen()) {
                oxygenBooster.SetActive(false);
            } else {
                oxygenBooster.SetActive(true);
            }

            if (tempIM.HasSwimSpeedBoost()) {
                swimSpeedBooster.SetActive(false);
            } else {
                swimSpeedBooster.SetActive(true);
            }
        }
    }
}
