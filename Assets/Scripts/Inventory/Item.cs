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

    [Header("Trade Properties"), Tooltip("Currency and Price the player can purchase the item for")]
    public CurrencyDefinition[] PurchasePrice;

    [Range(0,1), Tooltip("Deduction the merchant takes to purchase the item back")]
    public float SellPriceReduction = 0.10f;
}
