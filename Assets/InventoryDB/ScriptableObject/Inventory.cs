using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Inventory", fileName = "Inventory Data")]
public class Inventory : ScriptableObject {

	public List<Item> InventoryItems = new List<Item>();

	public int OxygenCapacity;
	public int SwimSpeed;
    [Range(2, 24)]
	public int TotalBagSlots;
	public enum attrEnum {OXYGEN_CAPACITY,SWIM_SPEED};//TODO: EXPLORE ENUM

	public int [] GetItemAttributeAmount()
    {
        /*int currency=inventoryList.CopperCoins;//need to set this to abilities
        return currency;*/
        int[] itemAttribute = new int[] { 0, 0};

        itemAttribute[0] = OxygenCapacity;
        itemAttribute[1] = SwimSpeed;

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


