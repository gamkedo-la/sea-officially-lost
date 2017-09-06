using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System;
using UnityEngine.EventSystems;

public class InventoryMgr : MonoBehaviour
{
    [SerializeField]
    private Inventory inventoryList;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject ItemTemplate;//Inventory Item Container
    [SerializeField]
    private GameObject CurrencyTemplate;
    [SerializeField]
    public WorldItems worldItems;

    private Text BagSpaceText;
    private Text copperCurrencyText;
    private Text goldCurrencyText;
    private Text silverCurrencyText;
	public GameObject playerController;


	private void Awake()
    {
        BagSpaceText = inventoryPanel.transform.Find("Footer/BagDetails/Stats").GetComponent<Text>();
        copperCurrencyText = inventoryPanel.transform.Find("Footer/CurrencyDetails/Coin_Copper/Text").GetComponent<Text>();
        goldCurrencyText = inventoryPanel.transform.Find("Footer/CurrencyDetails/Coin_Gold/Text").GetComponent<Text>();
        silverCurrencyText = inventoryPanel.transform.Find("Footer/CurrencyDetails/Coin_Silver/Text").GetComponent<Text>();
    }

    public void Start()
    {
        BagSpaceText.text = string.Format("{0}/{1}", inventoryList.InventoryItems.Count, inventoryList.TotalBagSlots);
		
		
		UpdateCurrency();
    }
    
    public void UpdateCurrency()
    {
        int[] cur = inventoryList.GetCoinCurrency();

        copperCurrencyText.text = cur[0].ToString();
        goldCurrencyText.text = cur[1].ToString();
        silverCurrencyText.text = cur[2].ToString();

    }

    public void PurchaseItem(Item purchasedItem)
    {
        //deduct the money from player
        inventoryList.CopperCoins -= purchasedItem.PurchasePriceInCopper(inventoryList.CopperCoins);
        inventoryList.GoldCoins -= purchasedItem.PurchasePriceInCopper(inventoryList.GoldCoins);
        inventoryList.SilverCoins -= purchasedItem.PurchasePriceInCopper(inventoryList.SilverCoins);
        UpdateCurrency();

        //add the item to the player's inventory
        inventoryList.InventoryItems.Add(purchasedItem);

        //add it to the UI Screen
        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryWindow();
        }   
    }

    private void ToggleInventoryWindow()
    {
        ClearBufferInventory();
        PopulateInventory(inventoryList);
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void ClearBufferInventory()
    {
        Transform ScrollViewContent = inventoryPanel.transform.Find("InvPanel/Scroll View/Viewport/Content");

        foreach (Transform inspectorLiveContainerAsChild in ScrollViewContent.transform)
        {
            Debug.Log(inspectorLiveContainerAsChild.name);
            Destroy(inspectorLiveContainerAsChild.gameObject);
        }
    }

    public void BuyOnClick(GameObject pickedUpItem)
    {
		Item purchasedItem = worldItems.AvailableWorldItems.Find(x => x.Name.Equals(
			pickedUpItem.gameObject.name));
		
		Debug.Log("purchItem " + purchasedItem);
		PurchaseItem(purchasedItem);
		
        pickedUpItem.SetActive(false);
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
		//PlayerController playerController = gameObject.GetComponent<PlayerController>();//TODO: WHY CAN'T SET THIS IN START AND playerController.ReleaseMouse() in this method? Most importantly even here, why not work?
		playerController.GetComponent<PlayerController>().ReleaseMouse();
		
		Debug.Log("Im closing and should have executed ReleaseMouse()");
    }
    
}
