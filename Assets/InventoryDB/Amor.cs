using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Item/Amor", fileName = "Amor Name")]
public class Amor : Item
{
    
    [Header("Armor Properties"), Tooltip ("Slot that the armor can be equiped on")]
    public Types ItemType;
    public enum Types
    {
        Head,
        Shoulder,
        Chest,
        Belt,
        Feet,
        Wrist,
        Hands
    }

}
