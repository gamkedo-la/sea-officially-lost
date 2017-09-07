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
    private Text nightSightItemAttributeText;
    private Text phelpsFinsItemAttributeText;
    private Text cinziasLungsItemAttributeText;
	public GameObject playerController;


	private void Awake()
    {
        BagSpaceText = inventoryPanel.transform.Find("Footer/BagDetails/Stats").GetComponent<Text>();
		nightSightItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/NightSight/Text").GetComponent<Text>();
		phelpsFinsItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/PhelpsFins/Text").GetComponent<Text>();
		cinziasLungsItemAttributeText = inventoryPanel.transform.Find("Footer/ItemAttributeDetails/CinziasLungs/Text").GetComponent<Text>();
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

		nightSightItemAttributeText.text = attr[0].ToString();
		phelpsFinsItemAttributeText.text = attr[1].ToString();
		cinziasLungsItemAttributeText.text = attr[2].ToString();

    }

	public void UpdateBagSlotsUsed()
	{
		BagSpaceText.text = string.Format("{0}/{1}", inventoryList.InventoryItems.Count, inventoryList.TotalBagSlots);
	}

    public void GetItem(Item addedItem)
    {
		//add attribute of item 
		//Debug.Log("nightsight before" + inventoryList.NightSight);
		
		inventoryList.NightSight= Mathf.Max(inventoryList.NightSight+ addedItem.ItemAttributeIncreaseAmountCalculation(),0);
		//Debug.Log("nightsight after" + inventoryList.NightSight);
		inventoryList.PhelpsFins= Mathf.Max(inventoryList.PhelpsFins+ addedItem.ItemAttributeIncreaseAmountCalculation(),0);
        inventoryList.CinziasLungs= Math.Max(inventoryList.CinziasLungs+ addedItem.ItemAttributeIncreaseAmountCalculation(),0);
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

		GameObject newItemAttribute = Instantiate(ItemAttributeTemplate, newItem.transform.Find("ItemAttribute/List"));
                newItemAttribute.transform.localScale = Vector3.one;

		/*newCurrency.transform.Find("Image").GetComponent<Image>().sprite = inventoryList.InventoryItems.Currency.Image;//TODO HOW PICK UP ATTRIBUTE?
newCurrency.transform.Find("Amount").GetComponent<Text>().text = inventoryList.InventoryItems.Amount.ToString();*/
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

            foreach (var attr in item.ItemAttributeIncreaseAmount)
            {
                GameObject newItemAttribute = Instantiate(ItemAttributeTemplate, newItem.transform.Find("ItemAttribute/List"));
                newItemAttribute.transform.localScale = Vector3.one;

                newItemAttribute.transform.Find("Image").GetComponent<Image>().sprite = attr.ItemAttribute.Image;
                newItemAttribute.transform.Find("Amount").GetComponent<Text>().text = attr.Amount.ToString();
			}
        }
    }

	public void onAwakeItemAttributeCalculation()
	{
		Transform ScrollViewContent = inventoryPanel.transform.Find("InvPanel/Scroll View/Viewport/Content");
		foreach (var item in inventoryList.InventoryItems)
		{
			inventoryList.NightSight += item.ItemAttributeIncreaseAmountCalculation();
			inventoryList.PhelpsFins += item.ItemAttributeIncreaseAmountCalculation();//TODO:which way to have array returned and not run the function 3 times? Currently ItemAttributeIncreaseAmountCalculation only returns NightSight, to set return for each itemattribute
			inventoryList.CinziasLungs += item.ItemAttributeIncreaseAmountCalculation();
		}
	}

    public void CloseInventoryWindow()
    {
        inventoryPanel.SetActive(false);
		//PlayerController playerController = gameObject.GetComponent<PlayerController>();//TODO: WHY THIS LINE DOES NOT DO WHAT THE ONE BELOW DOES AND CAN'T SET THIS IN START (not update to optimize)?
		playerController.GetComponent<PlayerController>().ReleaseMouse();
    }
    
}
