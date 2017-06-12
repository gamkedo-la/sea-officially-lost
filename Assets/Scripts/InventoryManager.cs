using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    Dictionary<string, int> inventory = new Dictionary<string, int>();
    public int capacity = 10;

    public Text inventoryDisplay;

    public void Start() {
        UpdateText();
    }

    public int CheckCapacity() {
        return capacity - CountCurrentInventory();
    }

    public int CountCurrentInventory() {
        int currentUsedSpace = 0;
		foreach (KeyValuePair<string, int> eachType in inventory)
		{
			currentUsedSpace += eachType.Value;
		}
		return currentUsedSpace;
    }

    public void UpdateText() {
        inventoryDisplay.text = "Inventory:\n";
        foreach (KeyValuePair<string, int> eachType in inventory) {
            inventoryDisplay.text += eachType.Key + ":" + eachType.Value + "\n";
        }
    }

    /*public void UpdateInventory(ScannableObject scannedObject) {
        CheckAndUpdateInventory(scannedObject.resourceType, scannedObject.resourceValue);
    }*/

    void CheckAndUpdateInventory(string type, int count) {
        if (inventory.ContainsKey(type)) {
            inventory[type] += count;
        } else {
            inventory.Add(type, count);
        }
        UpdateText();
    }

    public void TransferFromInto(InventoryManager intoInventory) {
        int mySpaceUsed = CheckCapacity();
        int theirSpaceUsed = intoInventory.CheckCapacity();
        if (mySpaceUsed > theirSpaceUsed) {
            Debug.Log("Not enough space");
        } else {
			foreach (KeyValuePair<string, int> eachType in inventory)
			{
                CheckAndUpdateInventory(eachType.Key, eachType.Value);
			}
        }
    }

}
