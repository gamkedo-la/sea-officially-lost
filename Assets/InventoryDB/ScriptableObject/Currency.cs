using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System;

[CreateAssetMenu(menuName ="Item/Currency" , fileName = "Generic Currency Name")]
public class Currency : ScriptableObject
{
    public string Name;
    public Sprite Image;
}
