using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System;

[CreateAssetMenu(menuName ="Item/Generic", fileName ="Generic File Name")]
public class Item : ScriptableObject
{
    [Header("General Properties"), Tooltip("Name of the item to display in UI")]
    public string Name = "New Item";

    [Tooltip("Description of item to display on UI"), Multiline(3)]
    public string Description = "Item Description";

    [Range(1, 20), Tooltip("Minimum level required to use the item. 0 is no level requirement")]
    public int MinimumPlayerLevel;

    [Tooltip("Item Image that is displayed on the UI")]
    public Sprite Sprite;

    [Header("Attribute Properties"), Tooltip("ItemAttribute and AttributeIncreaseAmount the player can gain with this item")]
    public List<ItemAttributeDefinition> ItemAttributeIncreaseAmount;

    /*public int ItemAttributeIncreaseAmountCalculation()
    {
		int nightSight = 0;
		int phelpsFins = 0;
		int breath = 0;
		//Debug.Log("coinType +" + coinType);
		//coinType = 0;
		//if (coinType.Equals("CopperCoins"))
		//{
		//Debug.Log("Coin is Copper in coinType.Equals");
		nightSight += ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Night Sight")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
		//}
		phelpsFins += ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Phelps Fins")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
		breath += ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Breath")).Select(s => s.Amount).DefaultIfEmpty(0).Single();

		return nightSight;//TODO: NEED TO SETUP TO RETURN THE THREE
    }*/
}
