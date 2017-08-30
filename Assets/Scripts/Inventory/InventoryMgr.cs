using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField]
    private GameObject ItemTemplate;//Inventory Item Container
    [SerializeField]
    private GameObject CurrencyTemplate;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PopulateInventory(inventoryList);
            invPanelToggle =! invPanelToggle;
            if (invPanelToggle)
                inventoryPanel.SetActive(true);
            else inventoryPanel.SetActive(false);
        }   
    }

    public void ClearBufferInventory()
    {
        Transform ScrollViewContent = inventoryPanel.transform.Find("InvPanel/Scroll View/Viewport/Content/Inventory Item Container");
        foreach(Transform inspectorLiveContainer in ScrollViewContent)
        {
            Destroy(inspectorLiveContainer);
        }
    }

    public void PopulateInventory(Inventory inventoryList)
    {
        Transform ScrollViewContent = inventoryPanel.transform.Find("InvPanel/Scroll View/Viewport/Content");
        foreach(var item in inventoryList.InventoryItems)
        {
            GameObject newItem = Instantiate(ItemTemplate, ScrollViewContent);
            newItem.transform.localScale = Vector3.one;
            
            newItem.transform.Find("Image/ItemImage").GetComponent<Image>().sprite = item.Sprite;
            newItem.transform.Find("ItemName").GetComponent<Text>().text = item.Name;
            newItem.transform.Find("Description").GetComponent<Text>().text = item.Description;

            foreach (var cur in item.PurchasePrice)
            {
                GameObject newCurrency = Instantiate(CurrencyTemplate, newItem.transform.Find("Currency/List"));
                newCurrency.transform.localScale = Vector3.one;

                newCurrency.transform.Find("Image").GetComponent<Image>().sprite = cur.Currency.Image;
                newCurrency.transform.Find("Amount").GetComponent<Text>().text = cur.Amount.ToString();
            }
        }
    }

    public void CloseInventoryWindow()
    {
        inventoryPanel.SetActive(false);
    }
    
}
