using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryResetScript : MonoBehaviour {

    public Inventory inventoryList;

    public void ClearInventoryList()
    {
        inventoryList.InventoryItems = new List<Item>();
    }
}
