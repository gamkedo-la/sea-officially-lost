using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(CurrencyDefinition))]//CurrencyDrawer actually belongs to CurrencyDefinition, hence setting its attribute to it
public class CurrencyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);//get rect position off a prefixLabel that points to GUIContect at start of function

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var currencyRect = new Rect(position.x, position.y, 100, position.height);//position then width and height
        var amountRect = new Rect(position.x + 110,position.y, 75, position.height);//position offset by 110

        EditorGUI.PropertyField(currencyRect, property.FindPropertyRelative("Currency"), GUIContent.none);//will look at this by name in CurrencyDefinition.cs
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("Amount"), GUIContent.none);

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
    
}
