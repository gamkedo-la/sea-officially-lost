﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/Inventory", fileName ="Inventory Data")]
public class Inventory : ScriptableObject {

    public List<Item> InventoryList = new List<Item>();//HOW DOES THIS WORK?
}
