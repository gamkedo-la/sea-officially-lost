using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/Inventory", fileName ="Inventory Data")]
public class Inventory : ScriptableObject {

    public List<Item> InventoryItems = new List<Item>();//HOW DOES THIS WORK?

    public int CopperCoins;
    [Range(12, 24)]
    public int TotalBagSlots;
    
}
