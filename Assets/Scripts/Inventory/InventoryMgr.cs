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
    private Text sonarRangeItemAttributeText;
    private Text anxietyConstraintItemAttributeText;
    public GameObject playerController;

	public void Awake()
    {
		inventoryList.OxygenCapacity = 0;
		inventoryList.SwimSpeed = 0;
		inventoryList.SonarRange = 0;
        inventoryList.AnxietyConstraint = 0;
        BagSpaceText = inventoryPanel.transform.Find("Footer/BagDetails/Stats").GetComponent<Text>();
		oxygenCapacityItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/OxygenCapacity/Text").GetComponent<Text>();
		swimSpeedItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/SwimSpeed/Text").GetComponent<Text>();
		sonarRangeItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/SonarRange/Text").GetComponent<Text>();
        anxietyConstraintItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/AnxietyConstraint/Text").GetComponent<Text>();
        onAwakeItemAttributeCalculation();
	}

    public void Start()
    {
        BagSpaceText.text = string.Format("{0}/{1}", inventoryList.InventoryItems.Count, inventoryList.TotalBagSlots);
		
		
		UpdateItemAttribute();
    }
    
    public void UpdateItemAttribute()
    {
        int[] attr = inventoryList.GetItemAttributeAmount();

		oxygenCapacityItemAttributeText.text = attr[0].ToString();
		swimSpeedItemAttributeText.text = attr[1].ToString();
		sonarRangeItemAttributeText.text = attr[2].ToString();
        anxietyConstraintItemAttributeText.text = attr[3].ToString();

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
		//inventoryList.PhelpsFins += item.ItemAttributeIncreaseAmountCalculation();//TODO:which way to have array returned and not run the function 3 times? Currently ItemAttributeIncreaseAmountCalculation only returns NightSight, to set return for each itemattribute
		//inventoryList.Breath += item.ItemAttributeIncreaseAmountCalculation();
		inventoryList.SonarRange += addedItem.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Sonar Range")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
        inventoryList.AnxietyConstraint += addedItem.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Anxiety Constraint")).Select(s => s.Amount).DefaultIfEmpty(0).Single();


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
		populateAttributesForItem(addedItem, newItem);
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
			populateAttributesForItem(item, newItem);
            
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
			inventoryList.SonarRange += item.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Sonar Range")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
            inventoryList.AnxietyConstraint += item.ItemAttributeIncreaseAmount.Where(x => x.ItemAttribute.Name.Equals("Anxiety Constraint")).Select(s => s.Amount).DefaultIfEmpty(0).Single();
        }
	}

    public void CloseInventoryWindow()
    {
        inventoryPanel.SetActive(false);
		//PlayerController playerController = gameObject.GetComponent<PlayerController>();//TODO: WHY THIS LINE DOES NOT DO WHAT THE ONE BELOW DOES AND CAN'T SET THIS IN START (not update to optimize)?
		playerController.GetComponent<PlayerController>().ReleaseMouse();
    }
    
}
