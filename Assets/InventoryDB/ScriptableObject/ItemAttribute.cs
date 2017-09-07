using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System;

[CreateAssetMenu(menuName ="Item/ItemAttribute" , fileName = "Generic ItemAttribute Name")]
public class ItemAttribute : ScriptableObject
{
    public string Name;
    public Sprite Image;
}