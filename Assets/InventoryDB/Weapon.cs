using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName ="Item/Weapon" , fileName ="Weapon Name")]

public class Weapon : Item
{
    [Header("Weapon Properties")]
    public Types ItemType;

    [Range(1,2)]
    public int Hands;
    public enum Types
    {
        Staff,
        Sword,
        Dagger
    }
    
}
