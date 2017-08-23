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
    [Header("General Properties")]
    public string Name = "New Item";
    [Multiline(3)]
    public string Description = "Item Description";
    [Range(1, 20)]
    public int MinimumPlayerLevel;
    public Texture2D Sprite;
    [Header("Trade Properties")]
    public CurrencyDefinition[] PurchasePrice;
    public float SellPriceReduction = 0.10f;
}
