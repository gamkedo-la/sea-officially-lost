using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System;

public class InventoryMgr : MonoBehaviour
{
    bool invPanelToggle = false;
    [SerializeField]
    private Inventory inventoryList;
    [SerializeField]
    private GameObject inventoryPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            invPanelToggle =! invPanelToggle;
            if (invPanelToggle)
                inventoryPanel.SetActive(true);
            else inventoryPanel.SetActive(false);
        }   
    }
}
