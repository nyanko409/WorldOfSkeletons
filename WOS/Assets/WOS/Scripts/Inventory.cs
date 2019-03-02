using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
	public static bool isOpen;
	private GameObject inventoryScreen;
	private GameObject itemTooltip;

	public List<GameObject> slots;
	private List<GameObject> equipSlots;
	private ItemDatabase database;

	public Item draggingItem;
	public int draggingItemAmount;
	public GameObject draggingItemSprite;

	private GameObject dropItemPanel;

	// Use this for initialization
	void Start ()
	{
		inventoryScreen = GameObject.Find ("Inventory");
		slots = new List<GameObject> ();
		equipSlots = new List<GameObject> ();
		database = GameObject.Find ("SaveLoad").GetComponent<ItemDatabase> ();
		itemTooltip = GameObject.Find ("Tooltip");
		draggingItemSprite = GameObject.Find ("Dragging Item Sprite");
		dropItemPanel = GameObject.Find ("Drop Item");
		SaveLoadData.inventoryScreen = inventoryScreen;

		addSlotsInOrder ();
		setTooltip ();

//		addItemToSlot(database.getItem(1));
//		addItemToSlot(database.getItem(1));
//		addItemToSlot(database.getItem(50));
//		addItemToSlot(database.getItem(250));
//		addItemToSlot(database.getItem(450));
//		addItemToSlot(database.getItem(650));
//		addItemToSlot(database.getItem(850));
//		addItemToSlot(database.getItem(1050));
//		addItemToSlot(database.getItem(1250));

		inventoryScreen.SetActive (false);
		itemTooltip.SetActive (false);
		draggingItemSprite.SetActive (false);
		dropItemPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.I) && !isOpen && !IngameMenu.isPaused && !PlayerStats.isDead)
		{
			//open inventory and close skill trees if it is open
			SkillTreeMage.isOpen = false;
			inventoryScreen.SetActive (true);
			isOpen = true;
		}
		else if (Input.GetKeyDown (KeyCode.I) && isOpen)
		{
			//close inventory
			inventoryScreen.SetActive (false);
			isOpen = false;
		}
		else if (!isOpen)
		{
			inventoryScreen.SetActive(false);
			itemTooltip.SetActive(false);
		}

		//if is dragging, set sprite and follow mouse position and activate drop item panel
		if (Slot.isDraggingItem)
		{
			currentlyDragging ();
			dropItemPanel.SetActive(true);
		}
		else
		{
			draggingItemSprite.SetActive(false);
			dropItemPanel.SetActive(false);
		}
	}

	public void addItemToSlot(Item item)
	{
		if (item != null)
		{
			//stack if same item and stackable and does not exceed
			for (int i=0; i<slots.Count; i++)
			{
				if (slots [i].GetComponent<Slot> ().item != null && slots [i].GetComponent<Slot> ().item.itemID == item.itemID)
				{
					if ((slots [i].GetComponent<Slot> ().currentStack + 1) <= item.stackSize)
					{
						slots [i].GetComponent<Slot> ().currentStack++;						
						return;
					}
				}
			}

			//if the item is not stackable and slot is empty add item
			for (int i=0; i < slots.Count; i++)
			{
				if (slots [i].GetComponent<Slot> ().item == null)
				{
					slots [i].GetComponent<Slot> ().item = item;
					slots [i].GetComponent<Slot> ().currentStack ++;
					return;
				}
			}
		}
	}

	void setTooltip()
	{
		for (int i = 0; i < slots.Count; i++)
		{
			slots[i].GetComponent<Slot>().tooltip = itemTooltip;
		}
		for (int i = 0; i < equipSlots.Count; i++)
		{
			equipSlots[i].GetComponent<EquipSlot>().tooltip = itemTooltip;
		}
	}

	void dropDraggingItemOnGround()
	{
		GameObject obj = (GameObject)Instantiate (draggingItem.itemModel, transform.position, Quaternion.identity);
		obj.GetComponent<ItemObj> ().item = draggingItem;
		obj.GetComponent<ItemObj> ().amount = draggingItemAmount;
		obj.name = draggingItem.itemName;
		if (draggingItemAmount > 1)
		{
			obj.name += " (" + draggingItemAmount + ")";
		}
		draggingItem = null;
		Slot.isDraggingItem = false;
	}

	void currentlyDragging()
	{
		draggingItemSprite.SetActive (true);
		draggingItemSprite.GetComponent<Image> ().sprite = draggingItem.itemIcon;
		draggingItemSprite.GetComponent<RectTransform> ().position = Input.mousePosition;
	}

	public bool checkIfInventoryIsFull()
	{
		for (int i=0; i<slots.Count; i++)
		{
			if(slots[i].GetComponent<Slot>().item == null)
			{
				return false;
			}
		}
		return true;
	}

	void addSlotsInOrder()
	{
		for(int i = 1; i <= 24; i++)
		{
			slots.Add(GameObject.Find("Slot " + i.ToString()));
		}

		equipSlots = GameObject.FindGameObjectsWithTag("EquipSlot").ToList();
	}

	// set -1 if item is null
	public int getItemID(int slotNumber)
	{
		if (slots [slotNumber].GetComponent<Slot> ().item != null)
		{
			return slots [slotNumber].GetComponent<Slot> ().item.itemID;
		}
		return -1;
	}

	public void loadItem(int slotNumber, int itemID, int amount)
	{

		if (slots [slotNumber].GetComponent<Slot> ().item == null)
		{
			if (itemID != -1)
			{
				slots [slotNumber].GetComponent<Slot> ().item = database.getItem(itemID);
				slots [slotNumber].GetComponent<Slot> ().currentStack = amount;
			}
		}
	}
}
