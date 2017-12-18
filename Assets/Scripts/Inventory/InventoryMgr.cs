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
    private GameObject ItemAttributeTemplate;
    [SerializeField]
    public WorldItems worldItems;

    private Text BagSpaceText;
    private Text oxygenCapacityItemAttributeText;
    private Text swimSpeedItemAttributeText;
    public GameObject playerController;

	public void Awake()
    {
        //ClearInventoryList();
        inventoryList.OxygenCapacity = 0;
		inventoryList.SwimSpeed = 0;
        BagSpaceText = inventoryPanel.transform.Find("Footer/BagDetails/Stats").GetComponent<Text>();
		oxygenCapacityItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/OxygenCapacity/Text").GetComponent<Text>();
		swimSpeedItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/SwimSpeed/Text").GetComponent<Text>();
        onAwakeItemAttributeCalculation();
	}

    //clear inventory list at start
    public void ClearInventoryList()
    {
        inventoryList.InventoryItems = new List<Item>();
    }

    public void Start()
    {
        BagSpaceText.text = string.Format("{0}/{1}", inventoryList.InventoryItems.Count, inventoryList.TotalBagSlots);
		
		
		UpdateItemAttribute();
    }
    
    public void UpdateItemAttribute()
    {
        int[] attr = inventoryList.GetItemAttributeAmount();

		//update Attribute value in UI on right hand side box next to description of item 
        oxygenCapacityItemAttributeText.text = attr[0].ToString();
		swimSpeedItemAttributeText.text = attr[1].ToString();
    }

	public void UpdateBagSlotsUsed()
	{
		BagSpaceText.text = string.Format("{0}/{1}", inventoryList.InventoryItems.Count, inventoryList.TotalBagSlots);
	}

    public void GetItem(Item addedItem)
    {
        //add attribute of item 
        //Debug.Log("nightsight before" + inventoryList.NightSight);
        //Debug.Log("oxygen capacity before" + inventoryList.OxygenCapacity);

        //inventoryList.NightSight= Mathf.Max(inventoryList.NightSight+ addedItem.ItemAttributeIncreaseAmountCalculation(),0);
        inventoryList.OxygenCapacity += addedItem.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Oxygen Capacity")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
		inventoryList.SwimSpeed += addedItem.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Swim Speed")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
        inventoryList.JewelryBox += addedItem.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Jewelry Box")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
		//inventoryList.PhelpsFins += item.ItemAttributeIncreaseAmountCalculation();//TODO:which way to have array returned and not run the function 3 times? Currently ItemAttributeIncreaseAmountCalculation only returns NightSight, to set return for each itemattribute
		//inventoryList.Breath += item.ItemAttributeIncreaseAmountCalculation();


        UpdateItemAttribute();

		//add the item to the player's inventory
		inventoryList.InventoryItems.Add(addedItem);
		UpdateBagSlotsUsed();

		//add it to the UI Screen
		Transform ScrollViewContent = inventoryPanel.transform.Find("InvPanel/Scroll View/Viewport/Content");
		GameObject newItem = Instantiate(ItemTemplate, ScrollViewContent);
		newItem.transform.localScale = Vector3.one;

		newItem.transform.Find("Image/ItemImage").GetComponent<Image>().sprite = addedItem.Sprite;
		newItem.transform.Find("ItemName").GetComponent<Text>().text = addedItem.Name;
		newItem.transform.Find("Description").GetComponent<Text>().text = addedItem.Description;
		//populateAttributesForItem(addedItem, newItem);
	}

	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventoryWindow();
        }   
    }

    public bool HasOxygen(){
        return inventoryList.OxygenCapacity > 0;
    }

    public bool HasSwimSpeedBoost() {
        return inventoryList.SwimSpeed > 0;
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

    public void GetItem(GameObject pickedUpItem)
    {
		Item addedItem = worldItems.AvailableWorldItems.Find(x => x.Name.Equals(
			pickedUpItem.gameObject.name));
		
		Debug.Log("purchItem " + addedItem);
		GetItem(addedItem);
		
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
			//populateAttributesForItem(item, newItem);
            
        }
    }

	public void populateAttributesForItem(Item whichItem, GameObject whichGO)
	{
		foreach (var attr in whichItem.ItemAttributeIncreaseAmount)
		{
			GameObject newItemAttribute = Instantiate(ItemAttributeTemplate, whichGO.transform.Find("ItemAttribute/List"));
			newItemAttribute.transform.localScale = Vector3.one;

			newItemAttribute.transform.Find("Image").GetComponent<Image>().sprite = attr.ItemAttribute.Image;
			newItemAttribute.transform.Find("Amount").GetComponent<Text>().text = attr.Amount.ToString();
		}
	}

	public void onAwakeItemAttributeCalculation()
	{
		Transform ScrollViewContent = inventoryPanel.transform.Find("InvPanel/Scroll View/Viewport/Content");
		foreach (var item in inventoryList.InventoryItems)
		{

			//inventoryList.NightSight += item.ItemAttributeIncreaseAmountCalculation();
			inventoryList.OxygenCapacity += item.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Oxygen Capacity")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
			inventoryList.SwimSpeed += item.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Swim Speed")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
			//inventoryList.PhelpsFins += item.ItemAttributeIncreaseAmountCalculation();//TODO:which way to have array returned and not run the function 3 times? Currently ItemAttributeIncreaseAmountCalculation only returns NightSight, to set return for each itemattribute
			//inventoryList.Breath += item.ItemAttributeIncreaseAmountCalculation();
        }
	}

    public void CloseInventoryWindow()
    {
        inventoryPanel.SetActive(false);
		//PlayerController playerController = gameObject.GetComponent<PlayerController>();//TODO: WHY THIS LINE DOES NOT DO WHAT THE ONE BELOW DOES AND CAN'T SET THIS IN START (not update to optimize)?
		playerController.GetComponent<PlayerController>().ReleaseMouse();
    }
    
}
