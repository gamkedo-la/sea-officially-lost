using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System;

[CreateAssetMenu(menuName = "Item/Consumables", fileName = "Consumable Name")]
public class Consumable : Item
{
    [Header("Consumable Properties")]
    public Types ItemType;

    public int Amount;
    public enum Types
    {
        ManaPotion,
        HealthPotion,
        Poison
    }
}



