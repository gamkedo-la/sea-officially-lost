using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/World Items", fileName = "World Item Data")]

public class WorldItems : ScriptableObject
{
    public List<Item> AvailableWorldItems;
}


