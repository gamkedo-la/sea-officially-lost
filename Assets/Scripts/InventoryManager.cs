using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    Dictionary<string, int> inventory = new Dictionary<string, int>();
    public int capacity = 10;

    public GameObject inventoryPanel;
    public Text inventoryDisplay;
    private bool inventoryKnown = false;
    public bool unlimitedStorage = false;

    public void Start() {
        UpdateText();
    }

    public void ScannedInventory() {
        inventoryKnown = true;
        UpdateText();
    }

    public bool InventoryKnown() {
        return inventoryKnown;
    }

    public void ToggleInventoryPanel(bool toggle) {
        inventoryPanel.SetActive(toggle);
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
            if (inventoryKnown) {
				inventoryDisplay.text = "Inventory:\n";
				foreach (KeyValuePair<string, int> eachType in inventory)
				{
					inventoryDisplay.text += eachType.Key + ":" + eachType.Value + "\n";
				}
            } else {
                inventoryDisplay.text = "Unknown contents.  Scan to reveal.";
            }

        }
    }

    // This used for containers that are not the player or the player ship only.
    public void InitializeInventory(int cap, string type, int count, GameObject inventoryDisplayObject) {
        
        inventoryPanel = inventoryDisplayObject;
        inventoryDisplay = inventoryPanel.GetComponent<Text>();
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

    public void TransferInventoryInto(InventoryManager intoInventory, int transferAmount = 1) {
        int mySpaceUsed = CountCurrentInventory();
        int theirCapacity = intoInventory.CheckCapacity();
        if (mySpaceUsed <= theirCapacity || unlimitedStorage) {
			List<string> keys = new List<string>(inventory.Keys);
			foreach (string key in keys)
			{
				if (inventory[key] > 0)
				{
					intoInventory.CheckAndUpdateInventory(key, transferAmount);
					inventory[key] -= 1;
					UpdateText();
				}
			}
        } else {
            Debug.Log("Not enough space");
        }
    }
}
