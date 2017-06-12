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

    public string ReportCurrentInventory() {
        string currentInventory = "";
		foreach (KeyValuePair<string, int> eachType in inventory)
		{
			currentInventory += eachType.Key + ":" + eachType.Value + "\n";
		}
        return currentInventory;
    }

    public void UpdateText() {
        if (inventoryDisplay != null) {
			inventoryDisplay.text = "Inventory:\n";
			foreach (KeyValuePair<string, int> eachType in inventory)
			{
				inventoryDisplay.text += eachType.Key + ":" + eachType.Value + "\n";
			}
        }
    }

    public void InitializeInventory(int cap, string type, int count) {
        capacity = cap;
        CheckAndUpdateInventory(type, count);
    }

    void CheckAndUpdateInventory(string type, int count) {
        if (inventory.ContainsKey(type)) {
            inventory[type] += count;
        } else {
            inventory.Add(type, count);
        }
        UpdateText();
    }

    public void TransferInventoryInto(InventoryManager intoInventory) {
        int mySpaceUsed = CountCurrentInventory();
        int theirCapacity = intoInventory.CheckCapacity();
        if (mySpaceUsed > theirCapacity) {
            Debug.Log("Not enough space");
        } else {
			foreach (KeyValuePair<string, int> eachType in inventory)
			{
                intoInventory.CheckAndUpdateInventory(eachType.Key, eachType.Value);
			}
        }
    }

}
