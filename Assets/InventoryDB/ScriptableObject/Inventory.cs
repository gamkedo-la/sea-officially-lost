using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/Inventory", fileName ="Inventory Data")]
public class Inventory : ScriptableObject {

    public List<Item> InventoryItems = new List<Item>();//HOW DOES THIS WORK?

    public int CopperCoins;
    public int GoldCoins;
    public int SilverCoins;
    [Range(12, 24)]
    public int TotalBagSlots;

    public int [] GetCoinCurrency()
    {
        /*int currency=inventoryList.CopperCoins;//need to set this to abilities
        return currency;*/
        int[] currency = new int[] { 0, 0, 0 };

        currency[0] = CopperCoins;
        currency[1] = SilverCoins;
        currency[2] = GoldCoins;

        return currency;//WHY CAN'T USE return currency;??
    }
}
