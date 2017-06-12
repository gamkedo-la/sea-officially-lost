using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour {

    public GameObject resourceContainer;
    public Vector3 spawnPoint;

	// Use this for initialization
	void Start () {
        resourceContainer = (GameObject)GameObject.Instantiate(resourceContainer, spawnPoint, Quaternion.identity);
        InventoryManager tempIM = resourceContainer.gameObject.GetComponent<InventoryManager>();
        tempIM.InitializeInventory(10, "Gold", 10);
	}
	
}
