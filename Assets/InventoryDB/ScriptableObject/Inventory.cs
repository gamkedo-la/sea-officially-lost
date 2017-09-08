using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Inventory", fileName = "Inventory Data")]
public class Inventory : ScriptableObject {

	public List<Item> InventoryItems = new List<Item>();

	public int NightSight;
	public int PhelpsFins;
	public int Breath;
	[Range(12, 24)]
	public int TotalBagSlots;
	public enum attrEnum {NIGHT_SIGHT,PHELPS_FINS,BREATH };//TODO: EXPLORE ENUM

	public int [] GetItemAttributeAmount()
    {
        /*int currency=inventoryList.CopperCoins;//need to set this to abilities
        return currency;*/
        int[] itemAttribute = new int[] { 0, 0, 0 };

        itemAttribute[0] = NightSight;
        itemAttribute[1] = PhelpsFins;
        itemAttribute[2] = Breath;

        return itemAttribute;//WHY CAN'T USE return currency;??
    }

	/*TODO: EXPLORE LIST ATTRIBUTES
	public List<GameObject> ReturnGOs()
	{
		//attrEnum.NIGHT_SIGHT;//TODO: EXPLORE ENUM


		List<GameObject> tempList;
		tempList.Add(new GameObject());
		return tempList;
	}
	*/
}


